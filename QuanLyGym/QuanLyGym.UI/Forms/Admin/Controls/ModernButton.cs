using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Controls
{
    public class ModernButton : Button
    {
        private bool _isHovered = false;
        private bool _isPressed = false;

        [Category("Appearance")]
        public Color NormalColor { get; set; } = AdminTheme.PrimaryOrange;

        [Category("Appearance")]
        public Color HoverColor { get; set; } = Color.FromArgb(255, 133, 38);

        [Category("Appearance")]
        public Color PressedColor { get; set; } = Color.FromArgb(224, 94, 0);

        [Category("Appearance")]
        public int BorderRadius { get; set; } = 5;

        public ModernButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            Size = new Size(135, 32);
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _isHovered = false;
            _isPressed = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            _isPressed = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            _isPressed = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            Color curBg = _isPressed ? PressedColor : (_isHovered ? HoverColor : NormalColor);
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);

            using (var path = AdminTheme.GetRoundedRectanglePath(rect, BorderRadius))
            using (var brush = new SolidBrush(curBg))
            {
                g.FillPath(brush, path);

                // Subtle border
                Color borderColor = _isHovered ? Color.FromArgb(255, 170, 90) : curBg;
                using (var pen = new Pen(borderColor, 1f))
                {
                    g.DrawPath(pen, path);
                }
            }

            // Text
            using (var brushText = new SolidBrush(ForeColor))
            {
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(Text, Font, brushText, ClientRectangle, sf);
            }
        }
    }
}
