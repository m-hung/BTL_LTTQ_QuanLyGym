using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Controls
{
    public class BarChartControl : Control
    {
        private string _chartTitle = "BIỂU ĐỒ";
        private string[] _categories = Array.Empty<string>();
        private float[] _values = Array.Empty<float>();
        private float _maxY = 100f;
        private float[] _yTicks = new float[] { 0, 25, 50, 75, 100 };
        private Color _barColor = AdminTheme.PrimaryOrange;
        private int _hoverIndex = -1;

        [Category("Appearance")]
        public string ChartTitle
        {
            get => _chartTitle;
            set { _chartTitle = value; Invalidate(); }
        }

        [Category("Behavior")]
        public int HoverIndex
        {
            get => _hoverIndex;
            set { _hoverIndex = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color BarColor
        {
            get => _barColor;
            set { _barColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public string Unit { get; set; } = "";

        [Category("Appearance")]
        public string TooltipPrefix { get; set; } = "";

        public void SetData(string[] categories, float[] values, float maxY, float[] yTicks)
        {
            _categories = categories ?? Array.Empty<string>();
            _values = values ?? Array.Empty<float>();
            _maxY = maxY > 0 ? maxY : 100f;
            _yTicks = yTicks ?? new float[] { 0, maxY };
            Invalidate();
        }

        public BarChartControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Size = new Size(420, 210);
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
            int padLeft = 40;
            int padRight = 16;
            int padTop = 34;
            int padBottom = 30;
            int chartWidth = Width - padLeft - padRight;
            int chartHeight = Height - padTop - padBottom;
            int count = Math.Min(_categories.Length, _values.Length);
            if (count == 0 || chartWidth <= 0 || chartHeight <= 0) return -1;

            float slotWidth = (float)chartWidth / count;
            if (pt.X < padLeft || pt.X > padLeft + chartWidth || pt.Y < padTop || pt.Y > padTop + chartHeight)
                return -1;

            int index = (int)((pt.X - padLeft) / slotWidth);
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

            int padLeft = 40;
            int padRight = 16;
            int padTop = 34;
            int padBottom = 30;

            // 2. Draw Title
            using (var brushTitle = new SolidBrush(AdminTheme.TextMuted))
            {
                var titleRect = new Rectangle(16, 14, Width - 32, 20);
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

            // 3. Draw Y Gridlines & Y-Axis Labels
            using (var penGrid = new Pen(AdminTheme.GridLine, 1f))
            using (var brushAxis = new SolidBrush(AdminTheme.TextDim))
            {
                var sfY = new StringFormat
                {
                    Alignment = StringAlignment.Far,
                    LineAlignment = StringAlignment.Center
                };

                foreach (var tick in _yTicks)
                {
                    float ratio = tick / _maxY;
                    float y = padTop + chartHeight - (ratio * chartHeight);

                    // Gridline
                    g.DrawLine(penGrid, padLeft, y, padLeft + chartWidth, y);

                    // Label
                    var labelRect = new RectangleF(padLeft - 42, y - 9, 36, 18);
                    g.DrawString(tick.ToString("0"), AdminTheme.FontSmall, brushAxis, labelRect, sfY);
                }
            }

            // 4. Draw Bars
            int count = Math.Min(_categories.Length, _values.Length);
            if (count == 0) return;

            float slotWidth = (float)chartWidth / count;
            float barWidth = Math.Min(32f, slotWidth * 0.40f);

            float hoveredCenterX = 0f;
            float hoveredBarY = 0f;

            for (int i = 0; i < count; i++)
            {
                float centerX = padLeft + (i + 0.5f) * slotWidth;
                float barX = centerX - barWidth / 2f;

                float ratio = Math.Max(0, Math.Min(1f, _values[i] / _maxY));
                float barHeight = ratio * chartHeight;
                float barY = padTop + chartHeight - barHeight;

                bool isHovered = (i == _hoverIndex);
                if (isHovered)
                {
                    hoveredCenterX = centerX;
                    hoveredBarY = barY;
                }

                if (barHeight > 0)
                {
                    var barRect = new RectangleF(barX, barY, barWidth, barHeight);
                    Color curColor = isHovered
                        ? Color.FromArgb(Math.Min(255, _barColor.R + 40), Math.Min(255, _barColor.G + 40), Math.Min(255, _barColor.B + 40))
                        : _barColor;

                    using (var brushBar = new SolidBrush(curColor))
                    using (var path = GetBarPath(barRect, 3))
                    {
                        g.FillPath(brushBar, path);
                    }

                    // Top highlight cap if hovered
                    if (isHovered)
                    {
                        using (var penGlow = new Pen(Color.FromArgb(180, 255, 255, 255), 1.5f))
                        using (var path = GetBarPath(barRect, 3))
                        {
                            g.DrawPath(penGlow, path);
                        }
                    }

                    // Draw Value Above Bar (matching user's mockup: 28, 35, 42, 38, 51, 45)
                    using (var brushVal = new SolidBrush(isHovered ? AdminTheme.TextWhite : Color.FromArgb(170, 180, 195)))
                    using (var fontVal = new Font("Segoe UI", 7.5F, FontStyle.Bold))
                    {
                        var sfVal = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Far
                        };
                        var valRect = new RectangleF(centerX - slotWidth / 2f, barY - 18, slotWidth, 16);
                        g.DrawString(_values[i].ToString("0"), fontVal, brushVal, valRect, sfVal);
                    }
                }

                // 5. Draw Category Label at Bottom
                Color catColor = isHovered ? AdminTheme.TextWhite : AdminTheme.TextDim;
                using (var brushCat = new SolidBrush(catColor))
                {
                    var sfCat = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Near
                    };
                    var catRect = new RectangleF(centerX - slotWidth / 2f, padTop + chartHeight + 4, slotWidth, 18);
                    var fontCat = isHovered ? AdminTheme.FontBodyBold : AdminTheme.FontSmall;
                    g.DrawString(_categories[i], fontCat, brushCat, catRect, sfCat);
                }
            }

            // 6. Draw Tooltip popup for hovered bar
            if (_hoverIndex >= 0 && _hoverIndex < count)
            {
                string title = string.IsNullOrEmpty(TooltipPrefix)
                    ? _categories[_hoverIndex]
                    : $"{TooltipPrefix} {_categories[_hoverIndex]}";

                string valText = string.IsNullOrEmpty(Unit)
                    ? $"{_values[_hoverIndex]:N0}"
                    : $"{_values[_hoverIndex]:N0} {Unit}";

                DrawTooltip(g, hoveredCenterX, hoveredBarY, title, valText, _barColor);
            }
        }

        private void DrawTooltip(Graphics g, float centerX, float targetY, string title, string valText, Color accentColor)
        {
            using var fontTitle = new Font("Segoe UI", 7.5F, FontStyle.Regular);
            using var fontVal = new Font("Segoe UI", 9F, FontStyle.Bold);

            var sizeTitle = g.MeasureString(title, fontTitle);
            var sizeVal = g.MeasureString(valText, fontVal);

            float tipWidth = Math.Max(sizeTitle.Width, sizeVal.Width) + 20f;
            float tipHeight = 40f;

            float tipX = centerX - tipWidth / 2f;
            if (tipX < 4) tipX = 4;
            if (tipX + tipWidth > Width - 4) tipX = Width - tipWidth - 4;

            float tipY = targetY - tipHeight - 8;
            bool caretBelow = true;
            if (tipY < 4)
            {
                tipY = targetY + 10;
                caretBelow = false;
            }

            var tipRect = new RectangleF(tipX, tipY, tipWidth, tipHeight);

            // Shadow
            var shadowRect = new RectangleF(tipX - 1, tipY - 1, tipWidth + 2, tipHeight + 2);
            using (var shadowPath = AdminTheme.GetRoundedRectanglePath(Rectangle.Round(shadowRect), 6))
            using (var brushShadow = new SolidBrush(Color.FromArgb(70, 0, 0, 0)))
            {
                g.FillPath(brushShadow, shadowPath);
            }

            // Tooltip Card background & border
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

            // Caret Pointer
            if (caretBelow)
            {
                var caretPoints = new[]
                {
                    new PointF(centerX - 4, tipY + tipHeight),
                    new PointF(centerX + 4, tipY + tipHeight),
                    new PointF(centerX, tipY + tipHeight + 5)
                };
                using var brushCaret = new SolidBrush(Color.FromArgb(250, 26, 28, 36));
                g.FillPolygon(brushCaret, caretPoints);
                using var penCaret = new Pen(Color.FromArgb(75, 78, 96), 1f);
                g.DrawLine(penCaret, centerX - 4, tipY + tipHeight, centerX, tipY + tipHeight + 5);
                g.DrawLine(penCaret, centerX, tipY + tipHeight + 5, centerX + 4, tipY + tipHeight);
            }

            // Tooltip Content
            using (var brushTitle = new SolidBrush(AdminTheme.TextMuted))
            {
                g.DrawString(title, fontTitle, brushTitle, tipX + 10, tipY + 4);
            }

            using (var brushVal = new SolidBrush(accentColor))
            {
                g.DrawString(valText, fontVal, brushVal, tipX + 10, tipY + 18);
            }
        }

        private GraphicsPath GetBarPath(RectangleF rect, float radius)
        {
            var path = new GraphicsPath();
            if (rect.Width <= 0 || rect.Height <= 0) return path;

            float diameter = radius * 2;
            if (rect.Height < diameter)
            {
                path.AddRectangle(rect);
                return path;
            }

            // Top-left arc
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            // Top-right arc
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            // Right edge down to bottom
            path.AddLine(new PointF(rect.Right, rect.Y + radius), new PointF(rect.Right, rect.Bottom));
            // Bottom edge to left
            path.AddLine(new PointF(rect.Right, rect.Bottom), new PointF(rect.Left, rect.Bottom));
            // Left edge up to arc
            path.AddLine(new PointF(rect.Left, rect.Bottom), new PointF(rect.Left, rect.Y + radius));
            path.CloseFigure();

            return path;
        }
    }
}
