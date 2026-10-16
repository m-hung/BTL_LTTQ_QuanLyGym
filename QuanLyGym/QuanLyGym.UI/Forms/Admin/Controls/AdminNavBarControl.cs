using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Controls
{
    public class AdminNavBarControl : Control
    {
        public class NavItem
        {
            public string Title { get; set; } = "";
            public Rectangle Bounds { get; set; }
        }

        private readonly NavItem[] _items = new[]
        {
            new NavItem { Title = "Tổng quan" },
            new NavItem { Title = "Doanh thu" },
            new NavItem { Title = "Chi nhánh" },
            new NavItem { Title = "Nhân viên" },
            new NavItem { Title = "Hội viên" },
            new NavItem { Title = "Tài khoản" }
        };

        private int _selectedIndex = 0;
        private int _hoverIndex = -1;

        public event EventHandler<int>? SelectedIndexChanged;

        [Category("Behavior")]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value >= 0 && value < _items.Length && _selectedIndex != value)
                {
                    _selectedIndex = value;
                    Invalidate();
                    SelectedIndexChanged?.Invoke(this, _selectedIndex);
                }
            }
        }

        public string SelectedTabName => _items[_selectedIndex].Title;

        public AdminNavBarControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;
            BackColor = AdminTheme.HeaderBg;
            Height = 44;
            Dock = DockStyle.Top;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RecalculateItemBounds();
        }

        private void RecalculateItemBounds()
        {
            int curX = 20;
            int itemHeight = Height;

            for (int i = 0; i < _items.Length; i++)
            {
                // Measure text width + icon + padding
                int itemWidth = 110;
                if (i == 0) itemWidth = 115;
                if (i == 1) itemWidth = 115;
                if (i == 2) itemWidth = 115;
                if (i == 3) itemWidth = 115;
                if (i == 4) itemWidth = 105;
                if (i == 5) itemWidth = 115;

                _items[i].Bounds = new Rectangle(curX, 0, itemWidth, itemHeight);
                curX += itemWidth + 8;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int newHover = -1;
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i].Bounds.Contains(e.Location))
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
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i].Bounds.Contains(e.Location))
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

            // Background
            using (var brushBg = new SolidBrush(AdminTheme.HeaderBg))
            {
                g.FillRectangle(brushBg, ClientRectangle);
            }

            // Bottom border line across entire bar
            using (var penBorder = new Pen(Color.FromArgb(28, 30, 38), 1f))
            {
                g.DrawLine(penBorder, 0, Height - 1, Width, Height - 1);
            }

            RecalculateItemBounds();

            for (int i = 0; i < _items.Length; i++)
            {
                bool isSelected = (i == _selectedIndex);
                bool isHovered = (i == _hoverIndex);
                var bounds = _items[i].Bounds;

                Color textColor = isSelected
                    ? AdminTheme.PrimaryOrange
                    : (isHovered ? Color.White : AdminTheme.TextInactiveTab);

                // Draw Icon
                int iconX = bounds.X + 8;
                int iconY = bounds.Y + (bounds.Height - 16) / 2;
                DrawTabIcon(g, i, iconX, iconY, textColor);

                // Draw Text
                using (var brushText = new SolidBrush(textColor))
                {
                    var textRect = new Rectangle(iconX + 22, bounds.Y, bounds.Width - 30, bounds.Height);
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center
                    };
                    var font = isSelected ? AdminTheme.FontBodyBold : AdminTheme.FontBody;
                    g.DrawString(_items[i].Title, font, brushText, textRect, sf);
                }

                // Active Orange Indicator Underline
                if (isSelected)
                {
                    int underlineH = 3;
                    using (var brushIndicator = new SolidBrush(AdminTheme.PrimaryOrange))
                    {
                        g.FillRectangle(brushIndicator, bounds.X + 4, bounds.Bottom - underlineH, bounds.Width - 8, underlineH);
                    }
                }
            }
        }

        private void DrawTabIcon(Graphics g, int tabIndex, int x, int y, Color color)
        {
            using (var pen = new Pen(color, 1.4f))
            using (var brush = new SolidBrush(color))
            {
                switch (tabIndex)
                {
                    case 0: // Tổng quan (4 grid squares)
                        g.DrawRectangle(pen, x, y, 6, 6);
                        g.DrawRectangle(pen, x + 9, y, 6, 6);
                        g.DrawRectangle(pen, x, y + 9, 6, 6);
                        g.DrawRectangle(pen, x + 9, y + 9, 6, 6);
                        break;

                    case 1: // Doanh thu (Trending line chart)
                        g.DrawLine(pen, x, y + 14, x + 4, y + 9);
                        g.DrawLine(pen, x + 4, y + 9, x + 8, y + 12);
                        g.DrawLine(pen, x + 8, y + 12, x + 15, y + 2);
                        // Arrow head
                        g.DrawLine(pen, x + 11, y + 2, x + 15, y + 2);
                        g.DrawLine(pen, x + 15, y + 6, x + 15, y + 2);
                        break;

                    case 2: // Chi nhánh (Building / House)
                        var roofPoints = new[]
                        {
                            new Point(x + 7, y + 1),
                            new Point(x, y + 8),
                            new Point(x + 14, y + 8)
                        };
                        g.DrawPolygon(pen, roofPoints);
                        g.DrawRectangle(pen, x + 2, y + 8, 10, 7);
                        g.DrawRectangle(pen, x + 5, y + 11, 4, 4);
                        break;

                    case 3: // Nhân viên (Users)
                        // Main user
                        g.DrawEllipse(pen, x + 2, y + 1, 6, 6);
                        g.DrawArc(pen, x, y + 8, 10, 8, 180, 180);
                        // Sub user
                        g.DrawEllipse(pen, x + 9, y + 3, 5, 5);
                        g.DrawArc(pen, x + 8, y + 9, 8, 6, 180, 180);
                        break;

                    case 4: // Hội viên (Single member silhouette)
                        g.DrawEllipse(pen, x + 4, y + 1, 7, 7);
                        g.DrawArc(pen, x + 1, y + 9, 13, 10, 180, 180);
                        break;

                    case 5: // Tài khoản (Card / Shield / Key)
                        var cardRect = new Rectangle(x, y + 2, 15, 11);
                        g.DrawRectangle(pen, cardRect);
                        g.DrawLine(pen, x, y + 5, x + 15, y + 5);
                        g.DrawRectangle(pen, x + 2, y + 8, 3, 2);
                        break;
                }
            }
        }
    }
}
