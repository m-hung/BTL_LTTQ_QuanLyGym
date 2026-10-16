using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Controls
{
    public class DualLineSplineChart : Control
    {
        private string _chartTitle = "XU HƯỚNG DOANH THU (TRIỆU đ)";
        private string[] _xLabels = new[] { "T1/26", "T2/26", "T3/26", "T4/26", "T5/26", "T6/26", "T7/26", "T8/26", "T9/26" };
        private float[] _revenueValues = new float[] { 500, 520, 600, 580, 650, 670, 640, 815, 867 };
        private float[] _memberValues = new float[] { 130, 140, 160, 150, 185, 195, 190, 228, 245 };

        private float _maxRevenue = 1000f;
        private float[] _revenueTicks = new float[] { 0, 250, 500, 750, 1000 };

        private float _maxMembers = 260f;
        private float[] _memberTicks = new float[] { 0, 65, 130, 195, 260 };

        private int _hoverIndex = -1;

        // Filter buttons: Tháng, Quý, Năm
        private readonly string[] _filterOptions = new[] { "Tháng", "Quý", "Năm" };
        private int _selectedFilter = 0;
        private int _hoveredFilter = -1;
        private RectangleF[] _filterRects = new RectangleF[3];

        public event EventHandler<string>? FilterChanged;

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

        public void ApplyFilter(int filterIndex)
        {
            if (filterIndex < 0 || filterIndex >= _filterOptions.Length) return;
            _selectedFilter = filterIndex;
            if (filterIndex == 0) // Tháng
            {
                _xLabels = new[] { "T1/26", "T2/26", "T3/26", "T4/26", "T5/26", "T6/26", "T7/26", "T8/26", "T9/26" };
                _revenueValues = new float[] { 500, 520, 600, 580, 650, 670, 640, 815, 867 };
                _memberValues = new float[] { 130, 140, 160, 150, 185, 195, 190, 228, 245 };
                _maxRevenue = 1000f;
                _revenueTicks = new float[] { 0, 250, 500, 750, 1000 };
                _maxMembers = 260f;
                _memberTicks = new float[] { 0, 65, 130, 195, 260 };
            }
            else if (filterIndex == 1) // Quý
            {
                _xLabels = new[] { "Q1/26", "Q2/26", "Q3/26" };
                _revenueValues = new float[] { 1620, 1900, 2322 };
                _memberValues = new float[] { 430, 530, 663 };
                _maxRevenue = 2800f;
                _revenueTicks = new float[] { 0, 700, 1400, 2100, 2800 };
                _maxMembers = 800f;
                _memberTicks = new float[] { 0, 200, 400, 600, 800 };
            }
            else if (filterIndex == 2) // Năm
            {
                _xLabels = new[] { "2024", "2025", "2026" };
                _revenueValues = new float[] { 4800, 5600, 6009 };
                _memberValues = new float[] { 1450, 1820, 2123 };
                _maxRevenue = 8000f;
                _revenueTicks = new float[] { 0, 2000, 4000, 6000, 8000 };
                _maxMembers = 2800f;
                _memberTicks = new float[] { 0, 700, 1400, 2100, 2800 };
            }
            Invalidate();
        }

        public DualLineSplineChart()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Size = new Size(1150, 260);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // Check filter buttons hover
            int newFilterHover = -1;
            for (int i = 0; i < _filterRects.Length; i++)
            {
                if (_filterRects[i].Contains(e.Location))
                {
                    newFilterHover = i;
                    break;
                }
            }

            if (newFilterHover != _hoveredFilter)
            {
                _hoveredFilter = newFilterHover;
                Cursor = _hoveredFilter != -1 ? Cursors.Hand : Cursors.Default;
                Invalidate();
            }

            // Check chart point hover
            int newHover = GetPointIndexAt(e.Location);
            if (newHover != _hoverIndex)
            {
                _hoverIndex = newHover;
                if (_hoveredFilter == -1)
                {
                    Cursor = _hoverIndex != -1 ? Cursors.Hand : Cursors.Default;
                }
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_hoverIndex != -1 || _hoveredFilter != -1)
            {
                _hoverIndex = -1;
                _hoveredFilter = -1;
                Cursor = Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            for (int i = 0; i < _filterRects.Length; i++)
            {
                if (_filterRects[i].Contains(e.Location))
                {
                    if (_selectedFilter != i)
                    {
                        ApplyFilter(i);
                        FilterChanged?.Invoke(this, _filterOptions[i]);
                    }
                    break;
                }
            }
        }

        private int GetPointIndexAt(Point pt)
        {
            int padLeft = 46;
            int padRight = 46;
            int padTop = 40;
            int padBottom = 45;
            int chartWidth = Width - padLeft - padRight;
            int chartHeight = Height - padTop - padBottom;
            int count = Math.Min(_xLabels.Length, _revenueValues.Length);
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

            int padLeft = 46;
            int padRight = 46;
            int padTop = 40;
            int padBottom = 45;

            // 2. Draw Title
            using (var brushTitle = new SolidBrush(AdminTheme.TextMuted))
            {
                var titleRect = new Rectangle(16, 12, 350, 22);
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(_chartTitle.ToUpperInvariant(), AdminTheme.FontCardTitle, brushTitle, titleRect, sf);
            }

            // 3. Draw Filter Buttons on Top-Right (Tháng | Quý | Năm)
            DrawFilterButtons(g);

            int chartWidth = Width - padLeft - padRight;
            int chartHeight = Height - padTop - padBottom;
            if (chartWidth <= 10 || chartHeight <= 10) return;

            // 4. Draw Left Y Gridlines & Y-Axis Labels (Revenue)
            using (var penGrid = new Pen(AdminTheme.GridLine, 1f))
            using (var brushRevAxis = new SolidBrush(AdminTheme.TextDim))
            {
                var sfLeft = new StringFormat
                {
                    Alignment = StringAlignment.Far,
                    LineAlignment = StringAlignment.Center
                };

                foreach (var tick in _revenueTicks)
                {
                    float ratio = tick / _maxRevenue;
                    float y = padTop + chartHeight - (ratio * chartHeight);

                    // Gridline across
                    g.DrawLine(penGrid, padLeft, y, padLeft + chartWidth, y);

                    // Left Label
                    var labelRect = new RectangleF(padLeft - 44, y - 9, 38, 18);
                    g.DrawString(tick.ToString("0"), AdminTheme.FontSmall, brushRevAxis, labelRect, sfLeft);
                }
            }

            // 5. Draw Right Y-Axis Labels (Members)
            using (var brushMemAxis = new SolidBrush(AdminTheme.TextDim))
            {
                var sfRight = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };

                foreach (var tick in _memberTicks)
                {
                    float ratio = tick / _maxMembers;
                    float y = padTop + chartHeight - (ratio * chartHeight);

                    var labelRect = new RectangleF(padLeft + chartWidth + 6, y - 9, 38, 18);
                    g.DrawString(tick.ToString("0"), AdminTheme.FontSmall, brushMemAxis, labelRect, sfRight);
                }
            }

            // 6. Calculate Data Points
            int count = Math.Min(_xLabels.Length, Math.Min(_revenueValues.Length, _memberValues.Length));
            if (count < 2) return;

            var revPoints = new PointF[count];
            var memPoints = new PointF[count];
            float stepX = (float)chartWidth / (count - 1);

            for (int i = 0; i < count; i++)
            {
                float x = padLeft + i * stepX;

                // Revenue Point
                float ratioRev = Math.Max(0, Math.Min(1f, _revenueValues[i] / _maxRevenue));
                float yRev = padTop + chartHeight - (ratioRev * chartHeight);
                revPoints[i] = new PointF(x, yRev);

                // Member Point
                float ratioMem = Math.Max(0, Math.Min(1f, _memberValues[i] / _maxMembers));
                float yMem = padTop + chartHeight - (ratioMem * chartHeight);
                memPoints[i] = new PointF(x, yMem);
            }

            // 7. Draw Revenue Area Gradient & Curve (Orange)
            using (var revCurvePath = new GraphicsPath())
            {
                revCurvePath.AddCurve(revPoints, 0.40f);

                using (var fillPath = (GraphicsPath)revCurvePath.Clone())
                {
                    fillPath.AddLine(revPoints[count - 1], new PointF(revPoints[count - 1].X, padTop + chartHeight));
                    fillPath.AddLine(new PointF(revPoints[count - 1].X, padTop + chartHeight), new PointF(revPoints[0].X, padTop + chartHeight));
                    fillPath.AddLine(new PointF(revPoints[0].X, padTop + chartHeight), revPoints[0]);
                    fillPath.CloseFigure();

                    using (var fillBrush = new LinearGradientBrush(
                        new PointF(0, padTop),
                        new PointF(0, padTop + chartHeight),
                        Color.FromArgb(50, 255, 107, 0),
                        Color.FromArgb(4, 255, 107, 0)))
                    {
                        g.FillPath(fillBrush, fillPath);
                    }
                }

                using (var penRev = new Pen(AdminTheme.PrimaryOrange, 2.2f))
                {
                    penRev.LineJoin = LineJoin.Round;
                    g.DrawPath(penRev, revCurvePath);
                }
            }

            // 8. Draw Member Curve (Blue)
            using (var memCurvePath = new GraphicsPath())
            {
                memCurvePath.AddCurve(memPoints, 0.40f);

                using (var penMem = new Pen(AdminTheme.Blue, 2.2f))
                {
                    penMem.LineJoin = LineJoin.Round;
                    g.DrawPath(penMem, memCurvePath);
                }
            }

            // 9. Draw X-Axis Labels
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
                    float x = revPoints[i].X;
                    var labelRect = new RectangleF(x - 25, padTop + chartHeight + 6, 50, 18);
                    g.DrawString(_xLabels[i], fontX, brushX, labelRect, sfX);
                }
            }

            // 10. Draw Legend at Bottom Center
            DrawLegend(g, padTop + chartHeight + 24);

            // 11. Draw Hover Guide & Dual Tooltip
            if (_hoverIndex >= 0 && _hoverIndex < count)
            {
                var ptRev = revPoints[_hoverIndex];
                var ptMem = memPoints[_hoverIndex];

                // Vertical dashed guideline
                using (var penGuide = new Pen(Color.FromArgb(90, 255, 255, 255), 1.2f))
                {
                    penGuide.DashStyle = DashStyle.Dash;
                    g.DrawLine(penGuide, ptRev.X, padTop, ptRev.X, padTop + chartHeight);
                }

                // Orange Dot on Revenue Curve
                using (var brushGlow = new SolidBrush(Color.FromArgb(70, 255, 107, 0)))
                {
                    g.FillEllipse(brushGlow, ptRev.X - 6, ptRev.Y - 6, 12, 12);
                }
                using (var brushOuter = new SolidBrush(AdminTheme.PrimaryOrange))
                using (var brushInner = new SolidBrush(Color.White))
                {
                    g.FillEllipse(brushOuter, ptRev.X - 4, ptRev.Y - 4, 8, 8);
                    g.FillEllipse(brushInner, ptRev.X - 2, ptRev.Y - 2, 4, 4);
                }

                // Blue Dot on Member Curve
                using (var brushGlow = new SolidBrush(Color.FromArgb(70, 47, 128, 237)))
                {
                    g.FillEllipse(brushGlow, ptMem.X - 6, ptMem.Y - 6, 12, 12);
                }
                using (var brushOuter = new SolidBrush(AdminTheme.Blue))
                using (var brushInner = new SolidBrush(Color.White))
                {
                    g.FillEllipse(brushOuter, ptMem.X - 4, ptMem.Y - 4, 8, 8);
                    g.FillEllipse(brushInner, ptMem.X - 2, ptMem.Y - 2, 4, 4);
                }

                // Dual Tooltip Card
                DrawDualTooltip(g, ptRev.X, Math.Min(ptRev.Y, ptMem.Y), _xLabels[_hoverIndex], _revenueValues[_hoverIndex], _memberValues[_hoverIndex]);
            }
        }

        private void DrawFilterButtons(Graphics g)
        {
            int btnWidth = 46;
            int btnHeight = 24;
            int totalW = btnWidth * _filterOptions.Length;
            int startX = Width - 20 - totalW;
            int startY = 12;

            for (int i = 0; i < _filterOptions.Length; i++)
            {
                var rect = new RectangleF(startX + i * btnWidth, startY, btnWidth, btnHeight);
                _filterRects[i] = rect;

                bool isSelected = (i == _selectedFilter);
                bool isHovered = (i == _hoveredFilter);

                Color bg = isSelected
                    ? Color.FromArgb(50, 52, 65)
                    : (isHovered ? Color.FromArgb(38, 40, 50) : Color.FromArgb(24, 25, 32));
                Color border = isSelected ? Color.FromArgb(80, 84, 105) : Color.FromArgb(42, 44, 55);
                Color textCol = isSelected ? Color.White : (isHovered ? Color.White : AdminTheme.TextMuted);

                using (var path = AdminTheme.GetRoundedRectanglePath(Rectangle.Round(rect), 4))
                {
                    using (var brushBg = new SolidBrush(bg))
                    {
                        g.FillPath(brushBg, path);
                    }
                    using (var penBorder = new Pen(border, 1f))
                    {
                        g.DrawPath(penBorder, path);
                    }
                }

                using (var brushText = new SolidBrush(textCol))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    var font = isSelected ? AdminTheme.FontBadge : AdminTheme.FontSmall;
                    g.DrawString(_filterOptions[i], font, brushText, rect, sf);
                }
            }
        }

        private void DrawLegend(Graphics g, float y)
        {
            float centerX = Width / 2f;
            var font = AdminTheme.FontSmall;

            // Orange legend: Doanh thu (tr.đ)
            using (var pen = new Pen(AdminTheme.PrimaryOrange, 2f))
            using (var brushDot = new SolidBrush(AdminTheme.PrimaryOrange))
            using (var brushText = new SolidBrush(AdminTheme.TextMuted))
            {
                float x1 = centerX - 120;
                g.DrawLine(pen, x1, y + 6, x1 + 18, y + 6);
                g.FillEllipse(brushDot, x1 + 6, y + 3, 6, 6);
                g.DrawString("Doanh thu (tr.đ)", font, brushText, x1 + 24, y);
            }

            // Blue legend: Hội viên mới
            using (var pen = new Pen(AdminTheme.Blue, 2f))
            using (var brushDot = new SolidBrush(AdminTheme.Blue))
            using (var brushText = new SolidBrush(AdminTheme.TextMuted))
            {
                float x2 = centerX + 20;
                g.DrawLine(pen, x2, y + 6, x2 + 18, y + 6);
                g.FillEllipse(brushDot, x2 + 6, y + 3, 6, 6);
                g.DrawString("Hội viên mới", font, brushText, x2 + 24, y);
            }
        }

        private void DrawDualTooltip(Graphics g, float centerX, float targetY, string month, float revVal, float memVal)
        {
            using var fontTitle = new Font("Segoe UI", 8F, FontStyle.Bold);
            using var fontVal = new Font("Segoe UI", 8.5F, FontStyle.Regular);

            string title = month;
            string revText = $"Doanh thu (tr.đ) : {revVal:0}";
            string memText = $"Hội viên mới : {memVal:0}";

            float tipWidth = 155f;
            float tipHeight = 58f;

            float tipX = (centerX > Width / 2f) ? (centerX - tipWidth - 12f) : (centerX + 12f);
            if (tipX < 8) tipX = 8;
            if (tipX + tipWidth > Width - 8) tipX = Width - tipWidth - 8;

            float tipY = Math.Max(38f, Math.Min(Height - tipHeight - 40f, targetY + 8f));

            var tipRect = new RectangleF(tipX, tipY, tipWidth, tipHeight);

            // Shadow
            var shadowRect = new RectangleF(tipX - 1, tipY - 1, tipWidth + 2, tipHeight + 2);
            using (var shadowPath = AdminTheme.GetRoundedRectanglePath(Rectangle.Round(shadowRect), 6))
            using (var brushShadow = new SolidBrush(Color.FromArgb(80, 0, 0, 0)))
            {
                g.FillPath(brushShadow, shadowPath);
            }

            // Card background & border
            using (var path = AdminTheme.GetRoundedRectanglePath(Rectangle.Round(tipRect), 6))
            {
                using (var brushBg = new SolidBrush(Color.FromArgb(24, 25, 32)))
                {
                    g.FillPath(brushBg, path);
                }
                using (var penBorder = new Pen(Color.FromArgb(55, 58, 72), 1f))
                {
                    g.DrawPath(penBorder, path);
                }
            }

            // Content
            using (var brushTitle = new SolidBrush(AdminTheme.TextWhite))
            {
                g.DrawString(title, fontTitle, brushTitle, tipX + 12, tipY + 6);
            }

            using (var brushRev = new SolidBrush(AdminTheme.PrimaryOrange))
            {
                g.DrawString(revText, fontVal, brushRev, tipX + 12, tipY + 22);
            }

            using (var brushMem = new SolidBrush(AdminTheme.Blue))
            {
                g.DrawString(memText, fontVal, brushMem, tipX + 12, tipY + 38);
            }
        }
    }
}
