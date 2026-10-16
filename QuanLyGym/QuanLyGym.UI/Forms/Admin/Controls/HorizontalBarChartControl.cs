using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Controls
{
    public class HorizontalBarChartControl : Control
    {
        private string _chartTitle = "DOANH THU CHI NHÁNH T9/2026";
        private string[] _categories = new[] { "Hoàn Kiếm", "Cầu Giấy", "Quận 1" };
        private float[] _values = new float[] { 300, 220, 365 };
        private float _maxX = 380f;
        private float[] _xTicks = new float[] { 0, 95, 190, 285, 380 };
        private Color _barColor = AdminTheme.PrimaryOrange;
        private int _hoverIndex = -1;

        [Category("Appearance")]
        public string ChartTitle
        {
            get => _chartTitle;
            set { _chartTitle = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color BarColor
        {
            get => _barColor;
            set { _barColor = value; Invalidate(); }
        }

        public void SetData(string[] categories, float[] values, float maxX = 0, float[]? xTicks = null)
        {
            _categories = categories ?? Array.Empty<string>();
            _values = values ?? Array.Empty<float>();
            if (maxX > 0)
            {
                _maxX = maxX;
            }
            else
            {
                float maxVal = 0;
                foreach (var v in _values) if (v > maxVal) maxVal = v;
                _maxX = maxVal > 0 ? (float)(Math.Ceiling(maxVal / 50.0) * 50.0) : 100f;
            }

            if (xTicks != null && xTicks.Length > 0)
            {
                _xTicks = xTicks;
            }
            else
            {
                float step = _maxX / 4f;
                _xTicks = new float[] { 0, step, step * 2, step * 3, _maxX };
            }

            Invalidate();
        }

        public HorizontalBarChartControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Size = new Size(580, 220);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int newHover = GetBarIndexAt(e.Location);
            if (newHover != _hoverIndex)
            {
                _hoverIndex = newHover;
                Cursor = newHover != -1 ? Cursors.Hand : Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_hoverIndex != -1)
            {
                _hoverIndex = -1;
                Cursor = Cursors.Default;
                Invalidate();
            }
        }

        private int GetBarIndexAt(Point pt)
        {
            int padLeft = 105;
            int padRight = 24;
            int padTop = 38;
            int padBottom = 26;
            int chartWidth = Width - padLeft - padRight;
            int chartHeight = Height - padTop - padBottom;
            int count = Math.Min(_categories.Length, _values.Length);
            if (count == 0 || chartWidth <= 0 || chartHeight <= 0) return -1;

            if (pt.X < padLeft || pt.X > padLeft + chartWidth || pt.Y < padTop || pt.Y > padTop + chartHeight)
                return -1;

            float slotHeight = (float)chartHeight / count;
            int index = (int)((pt.Y - padTop) / slotHeight);
            if (index >= 0 && index < count) return index;
            return -1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // 1. Draw Card Background
            AdminTheme.DrawCard(g, ClientRectangle, radius: 8);

            int padLeft = 105;
            int padRight = 24;
            int padTop = 38;
            int padBottom = 26;

            // 2. Draw Title
            using (var brushTitle = new SolidBrush(AdminTheme.TextMuted))
            {
                var titleRect = new Rectangle(16, 12, Width - 32, 20);
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(_chartTitle.ToUpperInvariant(), AdminTheme.FontCardTitle, brushTitle, titleRect, sf);
            }

            int chartWidth = Width - padLeft - padRight;
            int chartHeight = Height - padTop - padBottom;
            if (chartWidth <= 10 || chartHeight <= 10) return;

            // 3. Draw Vertical Gridlines & X-Axis Labels
            using (var penGrid = new Pen(AdminTheme.GridLine, 1f))
            using (var brushAxis = new SolidBrush(AdminTheme.TextDim))
            {
                var sfX = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Near
                };

                foreach (var tick in _xTicks)
                {
                    float ratio = tick / _maxX;
                    float x = padLeft + (ratio * chartWidth);

                    // Gridline
                    g.DrawLine(penGrid, x, padTop, x, padTop + chartHeight);

                    // X-axis label at bottom
                    var labelRect = new RectangleF(x - 25, padTop + chartHeight + 4, 50, 18);
                    g.DrawString(tick.ToString("0"), AdminTheme.FontSmall, brushAxis, labelRect, sfX);
                }
            }

            // 4. Draw Horizontal Bars
            int count = Math.Min(_categories.Length, _values.Length);
            if (count == 0) return;

            float slotHeight = (float)chartHeight / count;
            float barHeight = Math.Min(22f, Math.Max(14f, slotHeight * 0.48f));

            float hoveredBarRight = 0f;
            float hoveredCenterY = 0f;

            for (int i = 0; i < count; i++)
            {
                float centerY = padTop + (i + 0.5f) * slotHeight;
                float barY = centerY - barHeight / 2f;

                float ratio = Math.Max(0, Math.Min(1f, _values[i] / _maxX));
                float curBarWidth = ratio * chartWidth;

                bool isHovered = (i == _hoverIndex);
                if (isHovered)
                {
                    hoveredBarRight = padLeft + curBarWidth;
                    hoveredCenterY = centerY;
                }

                // Y-Axis Category Label on Left - NoWrap to avoid awkward word breaks
                Color labelColor = isHovered ? AdminTheme.TextWhite : AdminTheme.TextDim;
                var fontLabel = isHovered ? AdminTheme.FontBodyBold : AdminTheme.FontSmall;
                using (var brushLabel = new SolidBrush(labelColor))
                {
                    var sfY = new StringFormat
                    {
                        Alignment = StringAlignment.Far,
                        LineAlignment = StringAlignment.Center,
                        FormatFlags = StringFormatFlags.NoWrap
                    };
                    var labelRect = new RectangleF(8, barY - 4, padLeft - 18, barHeight + 8);
                    g.DrawString(_categories[i], fontLabel, brushLabel, labelRect, sfY);
                }

                if (curBarWidth > 0)
                {
                    var barRect = new RectangleF(padLeft, barY, curBarWidth, barHeight);
                    Color curColor = isHovered
                        ? Color.FromArgb(Math.Min(255, _barColor.R + 35), Math.Min(255, _barColor.G + 35), Math.Min(255, _barColor.B + 35))
                        : _barColor;

                    using (var brushBar = new SolidBrush(curColor))
                    using (var path = GetHorizontalBarPath(barRect, 4))
                    {
                        g.FillPath(brushBar, path);
                    }

                    if (isHovered)
                    {
                        using (var penGlow = new Pen(Color.FromArgb(180, 255, 255, 255), 1.5f))
                        using (var path = GetHorizontalBarPath(barRect, 4))
                        {
                            g.DrawPath(penGlow, path);
                        }
                    }
                }
            }

            // 5. Tooltip on Hover
            if (_hoverIndex >= 0 && _hoverIndex < count)
            {
                string title = $"Chi nhánh {_categories[_hoverIndex]}";
                string valText = $"{_values[_hoverIndex]:N0} triệu đ";
                DrawTooltip(g, hoveredBarRight, hoveredCenterY, title, valText);
            }
        }

        private void DrawTooltip(Graphics g, float targetX, float centerY, string title, string valText)
        {
            using var fontTitle = new Font("Segoe UI", 7.5F, FontStyle.Regular);
            using var fontVal = new Font("Segoe UI", 9F, FontStyle.Bold);

            float tipWidth = 135f;
            float tipHeight = 40f;

            float tipX = targetX - tipWidth - 10;
            if (tipX < 90) tipX = targetX + 10;

            float tipY = centerY - tipHeight / 2f;
            if (tipY < 4) tipY = 4;
            if (tipY + tipHeight > Height - 4) tipY = Height - tipHeight - 4;

            var tipRect = new RectangleF(tipX, tipY, tipWidth, tipHeight);

            // Shadow
            var shadowRect = new RectangleF(tipX - 1, tipY - 1, tipWidth + 2, tipHeight + 2);
            using (var shadowPath = AdminTheme.GetRoundedRectanglePath(Rectangle.Round(shadowRect), 6))
            using (var brushShadow = new SolidBrush(Color.FromArgb(70, 0, 0, 0)))
            {
                g.FillPath(brushShadow, shadowPath);
            }

            // Card
            using (var path = AdminTheme.GetRoundedRectanglePath(Rectangle.Round(tipRect), 5))
            {
                using (var brushBg = new SolidBrush(Color.FromArgb(250, 26, 28, 36)))
                {
                    g.FillPath(brushBg, path);
                }
                using (var penBorder = new Pen(Color.FromArgb(75, 78, 96), 1f))
                {
                    g.DrawPath(penBorder, path);
                }
            }

            using (var brushTitle = new SolidBrush(AdminTheme.TextMuted))
            {
                g.DrawString(title, fontTitle, brushTitle, tipX + 10, tipY + 4);
            }

            using (var brushVal = new SolidBrush(AdminTheme.PrimaryOrange))
            {
                g.DrawString(valText, fontVal, brushVal, tipX + 10, tipY + 18);
            }
        }

        private GraphicsPath GetHorizontalBarPath(RectangleF rect, float radius)
        {
            var path = new GraphicsPath();
            if (rect.Width <= 0 || rect.Height <= 0) return path;

            float diameter = radius * 2;
            if (rect.Width < diameter)
            {
                path.AddRectangle(rect);
                return path;
            }

            // Top-left
            path.AddLine(rect.Left, rect.Top, rect.Right - radius, rect.Top);
            // Top-right arc
            path.AddArc(rect.Right - diameter, rect.Top, diameter, diameter, 270, 90);
            // Bottom-right arc
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            // Bottom-left
            path.AddLine(rect.Right - radius, rect.Bottom, rect.Left, rect.Bottom);
            path.CloseFigure();

            return path;
        }
    }
}
