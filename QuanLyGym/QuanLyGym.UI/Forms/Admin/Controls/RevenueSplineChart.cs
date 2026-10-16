using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Controls
{
    public class RevenueSplineChart : Control
    {
        private string _chartTitle = "DOANH THU THEO THÁNG (TRIỆU đ)";
        private string[] _xLabels = new[] { "T1/26", "T2/26", "T3/26", "T4/26", "T5/26", "T6/26", "T7/26", "T8/26", "T9/26" };
        private float[] _dataValues = new float[] { 500, 520, 590, 560, 640, 620, 670, 780, 820 };
        private float _maxY = 1000f;
        private float[] _yTicks = new float[] { 0, 250, 500, 750, 1000 };
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

        public RevenueSplineChart()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Size = new Size(580, 230);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int newHover = GetPointIndexAt(e.Location);
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

        private int GetPointIndexAt(Point pt)
        {
            int padLeft = 44;
            int padRight = 20;
            int padTop = 36;
            int padBottom = 26;
            int chartWidth = Width - padLeft - padRight;
            int chartHeight = Height - padTop - padBottom;
            int count = Math.Min(_xLabels.Length, _dataValues.Length);
            if (count < 2 || chartWidth <= 0 || chartHeight <= 0) return -1;

            if (pt.X < padLeft - 15 || pt.X > padLeft + chartWidth + 15 || pt.Y < padTop - 10 || pt.Y > padTop + chartHeight + 20)
                return -1;

            float stepX = (float)chartWidth / (count - 1);
            int closest = -1;
            float minDist = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                float x = padLeft + i * stepX;
                float dist = Math.Abs(pt.X - x);
                if (dist < minDist && dist <= stepX * 0.65f)
                {
                    minDist = dist;
                    closest = i;
                }
            }
            return closest;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // 1. Draw Card Background
            AdminTheme.DrawCard(g, ClientRectangle, radius: 8);

            int padLeft = 44;
            int padRight = 20;
            int padTop = 36;
            int padBottom = 26;

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

            // 4. Calculate Data Points
            int count = Math.Min(_xLabels.Length, _dataValues.Length);
            if (count < 2) return;

            var points = new PointF[count];
            float stepX = (float)chartWidth / (count - 1);

            for (int i = 0; i < count; i++)
            {
                float x = padLeft + i * stepX;
                float ratio = Math.Max(0, Math.Min(1f, _dataValues[i] / _maxY));
                float y = padTop + chartHeight - (ratio * chartHeight);
                points[i] = new PointF(x, y);
            }

            // 5. Build Smooth Curve Path
            using (var curvePath = new GraphicsPath())
            {
                curvePath.AddCurve(points, 0.45f);

                // 6. Draw Gradient Area under Curve
                using (var fillPath = (GraphicsPath)curvePath.Clone())
                {
                    fillPath.AddLine(points[count - 1], new PointF(points[count - 1].X, padTop + chartHeight));
                    fillPath.AddLine(new PointF(points[count - 1].X, padTop + chartHeight), new PointF(points[0].X, padTop + chartHeight));
                    fillPath.AddLine(new PointF(points[0].X, padTop + chartHeight), points[0]);
                    fillPath.CloseFigure();

                    var fillBounds = new RectangleF(padLeft, padTop, chartWidth, chartHeight);
                    using (var fillBrush = new LinearGradientBrush(
                        new PointF(0, padTop),
                        new PointF(0, padTop + chartHeight),
                        Color.FromArgb(65, 255, 107, 0),
                        Color.FromArgb(4, 255, 107, 0)))
                    {
                        g.FillPath(fillBrush, fillPath);
                    }
                }

                // 7. Draw Curve Stroke
                using (var penLine = new Pen(AdminTheme.PrimaryOrange, 2.2f))
                {
                    penLine.LineJoin = LineJoin.Round;
                    penLine.StartCap = LineCap.Round;
                    penLine.EndCap = LineCap.Round;
                    g.DrawPath(penLine, curvePath);
                }
            }

            // 8. Draw X-Axis Labels
            for (int i = 0; i < count; i++)
            {
                bool isHovered = (i == _hoverIndex);
                Color xColor = isHovered ? AdminTheme.TextWhite : AdminTheme.TextDim;
                var fontX = isHovered ? AdminTheme.FontBodyBold : AdminTheme.FontSmall;

                using (var brushX = new SolidBrush(xColor))
                {
                    var sfX = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Near
                    };
                    float x = points[i].X;
                    var labelRect = new RectangleF(x - 25, padTop + chartHeight + 6, 50, 18);
                    g.DrawString(_xLabels[i], fontX, brushX, labelRect, sfX);
                }
            }

            // 9. Draw Hover Guide, Point & Tooltip
            if (_hoverIndex >= 0 && _hoverIndex < count)
            {
                var pt = points[_hoverIndex];

                // Vertical guideline
                using (var penGuide = new Pen(Color.FromArgb(100, 255, 107, 0), 1.2f))
                {
                    penGuide.DashStyle = DashStyle.Dash;
                    g.DrawLine(penGuide, pt.X, padTop, pt.X, padTop + chartHeight);
                }

                // Glow circle
                using (var brushGlow = new SolidBrush(Color.FromArgb(70, 255, 107, 0)))
                {
                    g.FillEllipse(brushGlow, pt.X - 7, pt.Y - 7, 14, 14);
                }

                // Center solid circle with white fill
                using (var brushOuter = new SolidBrush(AdminTheme.PrimaryOrange))
                using (var brushInner = new SolidBrush(Color.White))
                {
                    g.FillEllipse(brushOuter, pt.X - 4.5f, pt.Y - 4.5f, 9, 9);
                    g.FillEllipse(brushInner, pt.X - 2f, pt.Y - 2f, 4, 4);
                }

                // Draw Tooltip popup
                string title = $"Tháng {_xLabels[_hoverIndex]}";
                string valText = $"{_dataValues[_hoverIndex]:N0} triệu đ";
                DrawTooltip(g, pt.X, pt.Y, title, valText, AdminTheme.PrimaryOrange);
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

            float tipY = targetY - tipHeight - 10;
            bool caretBelow = true;
            if (tipY < 4)
            {
                tipY = targetY + 12;
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

            // Card background & border
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

            // Content
            using (var brushTitle = new SolidBrush(AdminTheme.TextMuted))
            {
                g.DrawString(title, fontTitle, brushTitle, tipX + 10, tipY + 4);
            }

            using (var brushVal = new SolidBrush(accentColor))
            {
                g.DrawString(valText, fontVal, brushVal, tipX + 10, tipY + 18);
            }
        }
    }
}
