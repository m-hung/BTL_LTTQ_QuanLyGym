using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin.Controls
{
    public class ExpiringMemberItem
    {
        public string Name { get; set; } = "";
        public string Branch { get; set; } = "";
        public string DaysLeft { get; set; } = "";
        public Color BadgeColor { get; set; } = AdminTheme.BadgeRed;

        public ExpiringMemberItem(string name, string branch, string daysLeft, Color badgeColor)
        {
            Name = name;
            Branch = branch;
            DaysLeft = daysLeft;
            BadgeColor = badgeColor;
        }
    }

    public class ExpiringMembersControl : Control
    {
        private string _cardTitle = "SẮP HẾT HẠN";
        private readonly List<ExpiringMemberItem> _items = new List<ExpiringMemberItem>();
        private int _hoverIndex = -1;

        [Category("Appearance")]
        public string CardTitle
        {
            get => _cardTitle;
            set { _cardTitle = value; Invalidate(); }
        }

        public ExpiringMembersControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Size = new Size(300, 260);

            // Populate sample data exactly matching the Figma design
            _items.Add(new ExpiringMemberItem("Trần Quốc Khánh", "Bình Thạnh", "1N", AdminTheme.BadgeRed));
            _items.Add(new ExpiringMemberItem("Nguyễn Thị Ích", "Hoàn Kiếm", "3N", AdminTheme.BadgeRed));
            _items.Add(new ExpiringMemberItem("Lê Minh Cường", "Cầu Giấy", "5N", AdminTheme.BadgeOrange));
            _items.Add(new ExpiringMemberItem("Vũ Thị Phương", "Hải Châu", "8N", AdminTheme.BadgeAmber));
            _items.Add(new ExpiringMemberItem("Phạm Thị Dung", "Bình Thạnh", "12N", AdminTheme.BadgeGold));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int newHover = GetItemIndexAt(e.Y);
            if (newHover != _hoverIndex)
            {
                _hoverIndex = newHover;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_hoverIndex != -1)
            {
                _hoverIndex = -1;
                Invalidate();
            }
        }

        private int GetItemIndexAt(int y)
        {
            int top = 42;
            int availableHeight = Height - top - 12;
            if (_items.Count == 0 || availableHeight <= 0) return -1;
            int rowHeight = availableHeight / _items.Count;
            int index = (y - top) / rowHeight;
            if (index >= 0 && index < _items.Count) return index;
            return -1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // 1. Draw Card Background
            AdminTheme.DrawCard(g, ClientRectangle, radius: 8);

            // 2. Draw Title
            using (var brushTitle = new SolidBrush(AdminTheme.TextMuted))
            {
                var titleRect = new Rectangle(16, 14, Width - 32, 20);
                var sf = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(_cardTitle.ToUpperInvariant(), AdminTheme.FontCardTitle, brushTitle, titleRect, sf);
            }

            // 3. Draw Items
            int top = 38;
            int availableHeight = Height - top - 8;
            if (_items.Count == 0 || availableHeight <= 0) return;

            int rowHeight = availableHeight / _items.Count;

            for (int i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                int itemY = top + i * rowHeight;
                var rowRect = new Rectangle(12, itemY, Width - 24, rowHeight);

                // Hover background
                if (i == _hoverIndex)
                {
                    using (var path = AdminTheme.GetRoundedRectanglePath(rowRect, 4))
                    using (var brushHover = new SolidBrush(Color.FromArgb(20, 255, 255, 255)))
                    {
                        g.FillPath(brushHover, path);
                    }
                }

                // Member Name
                using (var brushName = new SolidBrush(AdminTheme.TextWhite))
                {
                    var nameRect = new Rectangle(16, itemY + 1, Width - 84, 18);
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.EllipsisCharacter
                    };
                    g.DrawString(item.Name, AdminTheme.FontBodyBold, brushName, nameRect, sf);
                }

                // Branch Subtext
                using (var brushBranch = new SolidBrush(AdminTheme.TextMuted))
                {
                    var branchRect = new Rectangle(16, itemY + 18, Width - 84, 15);
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.EllipsisCharacter
                    };
                    g.DrawString(item.Branch, AdminTheme.FontSmall, brushBranch, branchRect, sf);
                }

                // Days Left Badge on Right
                int badgeW = 32;
                int badgeH = 18;
                int badgeX = Width - 16 - badgeW;
                int badgeY = itemY + (rowHeight - badgeH) / 2;
                var badgeRect = new Rectangle(badgeX, badgeY, badgeW, badgeH);

                using (var badgePath = AdminTheme.GetRoundedRectanglePath(badgeRect, 3))
                {
                    using (var brushBadge = new SolidBrush(item.BadgeColor))
                    {
                        g.FillPath(brushBadge, badgePath);
                    }

                    using (var brushText = new SolidBrush(Color.White))
                    {
                        var sfBadge = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };
                        g.DrawString(item.DaysLeft, AdminTheme.FontBadge, brushText, badgeRect, sfBadge);
                    }
                }
            }
        }
    }
}
