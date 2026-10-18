using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.PT
{
#pragma warning disable WFO1000
    public class ModernDarkComboBox : Control
    {
        private readonly List<string> _items = new List<string>();
        private int _selectedIndex = -1;
        private bool _isHovered = false;
        private bool _isDroppedDown = false;
        private ToolStripDropDown? _dropDownMenu;
        private string _placeholderText = "Chọn...";

        public event EventHandler? SelectedIndexChanged;

        public ModernDarkComboBox()
        {
            this.SetStyle(ControlStyles.UserPaint |
                           ControlStyles.AllPaintingInWmPaint |
                           ControlStyles.OptimizedDoubleBuffer |
                           ControlStyles.ResizeRedraw |
                           ControlStyles.SupportsTransparentBackColor, true);

            this.DoubleBuffered = true;
            this.Size = new Size(280, 36);
            this.Font = PTUITheme.FontRegular;
            this.BackColor = Color.FromArgb(20, 24, 34);
            this.ForeColor = Color.White;
            this.Cursor = Cursors.Hand;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<string> Items => _items;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PlaceholderText
        {
            get => _placeholderText;
            set
            {
                _placeholderText = value;
                Invalidate();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (value >= -1 && value < _items.Count)
                {
                    if (_selectedIndex != value)
                    {
                        _selectedIndex = value;
                        Invalidate();
                        SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? SelectedItem
        {
            get => (_selectedIndex >= 0 && _selectedIndex < _items.Count) ? _items[_selectedIndex] : null;
            set
            {
                if (value == null)
                {
                    SelectedIndex = -1;
                }
                else
                {
                    int idx = _items.IndexOf(value);
                    if (idx >= 0)
                    {
                        SelectedIndex = idx;
                    }
                }
            }
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
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                if (_isDroppedDown)
                {
                    _dropDownMenu?.Close();
                }
                else
                {
                    ShowDropDown();
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Down)
            {
                if (SelectedIndex < _items.Count - 1)
                    SelectedIndex++;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (SelectedIndex > 0)
                    SelectedIndex--;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                ShowDropDown();
                e.Handled = true;
            }
        }

        private void ShowDropDown()
        {
            if (_items.Count == 0) return;

            _dropDownMenu = new ToolStripDropDown
            {
                AutoClose = true,
                DropShadowEnabled = true,
                BackColor = Color.FromArgb(20, 24, 34),
                Padding = new Padding(4),
                Margin = Padding.Empty,
                Renderer = new DarkMenuRenderer()
            };

            int menuWidth = Math.Max(this.Width, 320);

            for (int i = 0; i < _items.Count; i++)
            {
                int itemIndex = i;
                string itemText = _items[i];
                bool isCurrent = itemIndex == _selectedIndex;

                var menuItem = new ToolStripMenuItem(itemText)
                {
                    AutoSize = false,
                    Height = 36,
                    Width = menuWidth - 8,
                    Font = PTUITheme.FontRegular,
                    ForeColor = isCurrent ? PTUITheme.PrimaryLime : Color.White,
                    Padding = new Padding(12, 0, 12, 0),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                menuItem.Click += (s, e) =>
                {
                    SelectedIndex = itemIndex;
                };

                _dropDownMenu.Items.Add(menuItem);
            }

            _isDroppedDown = true;
            Invalidate();

            _dropDownMenu.Closed += (s, e) =>
            {
                _isDroppedDown = false;
                Invalidate();
            };

            _dropDownMenu.Show(this, new Point(0, this.Height + 4));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            int radius = 8;

            Color bg = _isHovered || _isDroppedDown ? Color.FromArgb(27, 32, 44) : Color.FromArgb(20, 24, 34);
            Color border = _isDroppedDown
                ? PTUITheme.PrimaryLime
                : (_isHovered ? Color.FromArgb(65, 78, 106) : Color.FromArgb(40, 48, 66));

            using (SolidBrush sb = new SolidBrush(bg))
            {
                PTUITheme.FillRoundedRectangle(g, sb, rect, radius);
            }

            using (Pen pen = new Pen(border, 1.2f))
            {
                PTUITheme.DrawRoundedRectangle(g, pen, rect, radius);
            }

            // Draw text
            string displayText = SelectedItem ?? _placeholderText;
            Color textColor = (SelectedItem != null) ? Color.White : PTUITheme.TextMuted;
            Rectangle textRect = new Rectangle(14, 0, Width - 44, Height);
            TextRenderer.DrawText(g, displayText, Font, textRect, textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            // Draw Chevron Arrow on the right
            int arrowX = Width - 20;
            int arrowY = Height / 2;
            Color arrowColor = _isDroppedDown || _isHovered ? PTUITheme.PrimaryLime : PTUITheme.TextMuted;

            using (Pen arrowPen = new Pen(arrowColor, 2f))
            {
                arrowPen.StartCap = LineCap.Round;
                arrowPen.EndCap = LineCap.Round;
                arrowPen.LineJoin = LineJoin.Round;

                if (_isDroppedDown)
                {
                    g.DrawLines(arrowPen, new Point[] {
                        new Point(arrowX - 8, arrowY + 2),
                        new Point(arrowX - 4, arrowY - 2),
                        new Point(arrowX, arrowY + 2)
                    });
                }
                else
                {
                    g.DrawLines(arrowPen, new Point[] {
                        new Point(arrowX - 8, arrowY - 2),
                        new Point(arrowX - 4, arrowY + 2),
                        new Point(arrowX, arrowY - 2)
                    });
                }
            }
        }

        private class DarkMenuRenderer : ToolStripProfessionalRenderer
        {
            public DarkMenuRenderer() : base(new DarkColorTable()) { }

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(2, 1, e.Item.Width - 4, e.Item.Height - 2);

                if (e.Item.Selected)
                {
                    using (SolidBrush b = new SolidBrush(Color.FromArgb(36, 44, 62)))
                    {
                        PTUITheme.FillRoundedRectangle(g, b, r, 6);
                    }
                    using (Pen p = new Pen(PTUITheme.PrimaryLime, 1f))
                    {
                        PTUITheme.DrawRoundedRectangle(g, p, r, 6);
                    }
                }
                else
                {
                    using (SolidBrush b = new SolidBrush(Color.FromArgb(20, 24, 34)))
                    {
                        g.FillRectangle(b, e.Item.ContentRectangle);
                    }
                }
            }

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
                using (Pen p = new Pen(Color.FromArgb(48, 58, 80), 1.5f))
                {
                    g.DrawRectangle(p, r);
                }
            }
        }

        private class DarkColorTable : ProfessionalColorTable
        {
            public override Color ToolStripDropDownBackground => Color.FromArgb(20, 24, 34);
            public override Color MenuBorder => Color.FromArgb(48, 58, 80);
            public override Color MenuItemBorder => Color.Transparent;
            public override Color MenuItemSelected => Color.FromArgb(36, 44, 62);
        }
    }
}
