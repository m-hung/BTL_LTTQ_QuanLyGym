using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.PT
{
    public partial class ucPTTopStats : UserControl
    {
        public ucPTTopStats()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = PTUITheme.BgDark;
            this.Height = 148;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int totalWidth = this.ClientSize.Width;
            int gap = 16;
            int paddingLeft = 24;
            int paddingRight = 24;
            int availableWidth = totalWidth - paddingLeft - paddingRight - (gap * 3);
            int cardWidth = Math.Max(180, availableWidth / 4);
            int cardHeight = 132;
            int y = 8;

            // Card 1: Số hội viên đảm nhận
            DrawStatCard(g, new Rectangle(paddingLeft, y, cardWidth, cardHeight),
                title: "SỐ HỘI VIÊN ĐẢM NHẬN",
                mainValue: "14",
                subUnit: "",
                subText: "+2 hội viên mới tuần này",
                subColor: PTUITheme.PrimaryLime,
                iconType: 1,
                iconColor: PTUITheme.PrimaryLime);

            // Card 2: Ca tập hôm nay
            int x2 = paddingLeft + cardWidth + gap;
            DrawStatCard(g, new Rectangle(x2, y, cardWidth, cardHeight),
                title: "CA TẬP HÔM NAY",
                mainValue: "4",
                subUnit: " / 5",
                subText: "Đã hoàn thành 2 buổi sáng",
                subColor: PTUITheme.TextMuted,
                iconType: 2,
                iconColor: PTUITheme.Cyan);

            // Card 3: Ca tập theo tuần
            int x3 = x2 + cardWidth + gap;
            DrawStatCard(g, new Rectangle(x3, y, cardWidth, cardHeight),
                title: "CA TẬP THEO TUẦN",
                mainValue: "24",
                subUnit: "",
                subText: "Tuần 41 (05/10 - 11/10)",
                subColor: PTUITheme.TextMuted,
                iconType: 3,
                iconColor: PTUITheme.PrimaryLime);

            // Card 4: Tổng thời gian dạy trong tuần
            int x4 = x3 + cardWidth + gap;
            DrawStatCard(g, new Rectangle(x4, y, cardWidth, cardHeight),
                title: "TỔNG THỜI GIAN DẠY TUẦN",
                mainValue: "28.5",
                subUnit: " h",
                subText: "Mục tiêu: 32h / tuần",
                subColor: PTUITheme.TextMuted,
                iconType: 4,
                iconColor: PTUITheme.PrimaryLime);
        }

        private void DrawStatCard(Graphics g, Rectangle bounds, string title, string mainValue, string subUnit, string subText, Color subColor, int iconType, Color iconColor)
        {
            // Draw card background & subtle border
            PTUITheme.DrawCard(g, bounds, radius: 12);

            int innerPad = 18;

            // 1. Top row: Title (Left) + Vector Icon in Glowing Pill (Right)
            int titleY = bounds.Y + 16;
            Rectangle titleRect = new Rectangle(bounds.X + innerPad, titleY, bounds.Width - (innerPad * 2) - 44, 20);
            TextRenderer.DrawText(g, title, PTUITheme.FontSmallBold, titleRect, PTUITheme.TextMuted,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            // Icon with subtle tinted glowing box
            int iconBoxSize = 30;
            Rectangle iconBox = new Rectangle(bounds.Right - innerPad - iconBoxSize, bounds.Y + 12, iconBoxSize, iconBoxSize);
            using (SolidBrush bBox = new SolidBrush(Color.FromArgb(24, iconColor.R, iconColor.G, iconColor.B)))
            {
                PTUITheme.FillRoundedRectangle(g, bBox, iconBox, 8);
            }
            using (Pen pBox = new Pen(Color.FromArgb(55, iconColor.R, iconColor.G, iconColor.B), 1f))
            {
                PTUITheme.DrawRoundedRectangle(g, pBox, iconBox, 8);
            }

            Rectangle iconRect = new Rectangle(iconBox.X + (iconBoxSize - 18) / 2, iconBox.Y + (iconBoxSize - 18) / 2, 18, 18);
            switch (iconType)
            {
                case 1:
                    PTUITheme.DrawVectorUser(g, iconRect, iconColor);
                    break;
                case 2:
                    PTUITheme.DrawVectorDumbbell(g, iconRect, iconColor);
                    break;
                case 3:
                    PTUITheme.DrawVectorCalendar(g, iconRect, iconColor);
                    break;
                case 4:
                    PTUITheme.DrawVectorClock(g, iconRect, iconColor);
                    break;
            }

            // 2. Middle row: Big Number (Y = 44)
            int valX = bounds.X + innerPad;
            int valY = bounds.Y + 44;

            Size mainSize = TextRenderer.MeasureText(mainValue, PTUITheme.FontStatNumber);
            TextRenderer.DrawText(g, mainValue, PTUITheme.FontStatNumber, new Point(valX, valY), PTUITheme.TextWhite, TextFormatFlags.NoPrefix);

            // Sub-unit: "/ 5" hoặc " h" aligned with baseline
            if (!string.IsNullOrEmpty(subUnit))
            {
                int unitX = valX + mainSize.Width - 6;
                int unitY = valY + Math.Max(8, mainSize.Height - 24);
                TextRenderer.DrawText(g, subUnit, PTUITheme.FontStatUnit, new Point(unitX, unitY), PTUITheme.TextMuted, TextFormatFlags.NoPrefix);
            }

            // 3. Bottom row: Subtext anchored near bottom with guaranteed safe spacing from big number
            int subY = Math.Max(valY + mainSize.Height + 10, bounds.Bottom - 28);
            Rectangle subRect = new Rectangle(bounds.X + innerPad, subY, bounds.Width - (innerPad * 2), 18);
            TextRenderer.DrawText(g, subText, PTUITheme.FontSmall, subRect, subColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }
    }
}
