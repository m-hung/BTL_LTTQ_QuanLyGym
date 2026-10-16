using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Controls
{
    public class KpiCardControl : Control
    {
        private string _cardTitle = "TIÊU ĐỀ";
        private string _kpiValue = "0";
        private string _kpiSubtitle = "Ghi chú";
        private Color _valueColor = AdminTheme.PrimaryOrange;
        private bool _isHovered = false;

        [Category("Appearance")]
        public string CardTitle
        {
            get => _cardTitle;
            set { _cardTitle = value; Invalidate(); }
        }

        [Category("Appearance")]
        public string KpiValue
        {
            get => _kpiValue;
            set { _kpiValue = value; Invalidate(); }
        }

        [Category("Appearance")]
        public string KpiSubtitle
        {
            get => _kpiSubtitle;
            set { _kpiSubtitle = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color ValueColor
        {
            get => _valueColor;
            set { _valueColor = value; Invalidate(); }
        }

        public KpiCardControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Size = new Size(240, 105);
            Margin = new Padding(6);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            Cursor = Cursors.Hand;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            Cursor = Cursors.Default;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Draw Card Container with subtle hover state
            Color cardBg = _isHovered ? AdminTheme.CardHover : AdminTheme.CardBg;
            Color cardBorder = _isHovered ? Color.FromArgb(70, 74, 92) : AdminTheme.CardBorder;
            AdminTheme.DrawCard(g, ClientRectangle, radius: 8, customBg: cardBg, customBorder: cardBorder);

            // Draw Content
            int padX = 14;

            // 1. Title (Uppercase, muted)
            using (var brushTitle = new SolidBrush(AdminTheme.TextMuted))
            {
                var titleRect = new Rectangle(padX, 8, Width - padX * 2, 16);
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(_cardTitle.ToUpperInvariant(), AdminTheme.FontCardTitle, brushTitle, titleRect, sf);
            }

            // 2. Big KPI Value
            using (var brushVal = new SolidBrush(_valueColor))
            using (var fontVal = new Font("Segoe UI", 17F, FontStyle.Bold))
            {
                var valRect = new Rectangle(padX, 26, Width - padX * 2, 30);
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(_kpiValue, fontVal, brushVal, valRect, sf);
            }

            // 3. Subtitle
            using (var brushSub = new SolidBrush(AdminTheme.TextDim))
            {
                var subRect = new Rectangle(padX, Height - 22, Width - padX * 2, 16);
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(_kpiSubtitle, AdminTheme.FontKpiSub, brushSub, subRect, sf);
            }
        }
    }
}
