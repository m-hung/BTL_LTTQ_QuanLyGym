using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Controls
{
    public class SubTabBarControl : Control
    {
        private string[] _tabs = new[] { "TẤT CẢ" };
        private RectangleF[] _tabRects = Array.Empty<RectangleF>();
        private int _selectedIndex = 0;
        private int _hoverIndex = -1;

        public event EventHandler<int>? SelectedIndexChanged;

        [Category("Appearance")]
        public string[] Tabs
        {
            get => _tabs;
            set
            {
                _tabs = value ?? Array.Empty<string>();
                _selectedIndex = 0;
                RecalculateLayout();
                Invalidate();
            }
        }

        [Category("Behavior")]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value >= 0 && value < _tabs.Length && _selectedIndex != value)
                {
                    _selectedIndex = value;
                    Invalidate();
                    SelectedIndexChanged?.Invoke(this, _selectedIndex);
                }
            }
        }

        public string SelectedTabName => (_selectedIndex >= 0 && _selectedIndex < _tabs.Length) ? _tabs[_selectedIndex] : "";

        public SubTabBarControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Height = 36;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RecalculateLayout();
        }

        private void RecalculateLayout()
        {
            if (_tabs.Length == 0)
            {
                _tabRects = Array.Empty<RectangleF>();
                return;
            }

            _tabRects = new RectangleF[_tabs.Length];
            float curX = 4f;

            var font = AdminTheme.FontCardTitle;
            for (int i = 0; i < _tabs.Length; i++)
            {
                var size = TextRenderer.MeasureText(_tabs[i], font);
                float tabW = size.Width + 20f;
                _tabRects[i] = new RectangleF(curX, 0, tabW, Height);
                curX += tabW + 8f;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int newHover = -1;
            for (int i = 0; i < _tabRects.Length; i++)
            {
                if (_tabRects[i].Contains(e.Location))
                {
                    newHover = i;
                    break;
                }
            }

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

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            for (int i = 0; i < _tabRects.Length; i++)
            {
                if (_tabRects[i].Contains(e.Location))
                {
                    SelectedIndex = i;
                    break;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Bottom border line across entire bar
            using (var penBorder = new Pen(Color.FromArgb(32, 34, 44), 1f))
            {
                g.DrawLine(penBorder, 0, Height - 1, Width, Height - 1);
            }

            if (_tabRects.Length != _tabs.Length)
            {
                RecalculateLayout();
            }

            for (int i = 0; i < _tabs.Length; i++)
            {
                bool isSelected = (i == _selectedIndex);
                bool isHovered = (i == _hoverIndex);
                var rect = _tabRects[i];

                Color textColor = isSelected
                    ? AdminTheme.PrimaryOrange
                    : (isHovered ? Color.White : AdminTheme.TextDim);

                using (var brush = new SolidBrush(textColor))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    var font = isSelected ? AdminTheme.FontBodyBold : AdminTheme.FontSmall;
                    g.DrawString(_tabs[i], font, brush, rect, sf);
                }

                // Active bottom underline
                if (isSelected)
                {
                    int underlineH = 3;
                    using (var brushIndicator = new SolidBrush(AdminTheme.PrimaryOrange))
                    {
                        g.FillRectangle(brushIndicator, rect.X + 6, Height - underlineH, rect.Width - 12, underlineH);
                    }
                }
            }
        }
    }
}
