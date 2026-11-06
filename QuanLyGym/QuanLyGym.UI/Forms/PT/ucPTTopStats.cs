using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.PT
{
    public partial class ucPTTopStats : UserControl
    {
        private int _weekOffset = 0;
        private string _weeklySessionsCount = "24";
        private string _weeklyHours = "28.5";

        public ucPTTopStats()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = PTUITheme.BgDark;
            this.Height = 148;
        }

        public void SetWeekStats(int weekOffset, string weeklySessions, string weeklyHours)
        {
            _weekOffset = weekOffset;
            _weeklySessionsCount = weeklySessions;
            _weeklyHours = weeklyHours;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int totalWidth = this.ClientSize.Width;
            int pad = totalWidth < 1100 ? 14 : 20;
            int gap = totalWidth < 1100 ? 10 : 14;
            int availableWidth = totalWidth - (pad * 2) - (gap * 3);
            int cardWidth = Math.Max(100, availableWidth / 4);
            int cardHeight = 132;
            int y = 8;

            DateTime baseMonday = new DateTime(2026, 11, 2).AddDays(_weekOffset * 7);
            DateTime baseSunday = baseMonday.AddDays(6);
            int weekNumber = 45 + _weekOffset;
            string weekRangeText = $"Tuần {weekNumber} ({baseMonday:dd/MM} - {baseSunday:dd/MM})";

            // Card 1: Ca trực cố định
            string card1Sub = _weekOffset == 0 ? "08:00 – 16:00 (Hôm nay)" : $"08:00 – 16:00 (Thứ 5, {baseMonday.AddDays(3):dd/MM})";
            DrawStatCard(g, new Rectangle(pad, y, cardWidth, cardHeight),
                title: "CA TRỰC CỐ ĐỊNH",
                mainValue: "Ca Sáng",
                subUnit: "",
                subText: card1Sub,
                subColor: PTUITheme.PrimaryLime);

            // Card 2: Ca tập hôm nay / đầu tuần
            int x2 = pad + cardWidth + gap;
            string card2Title = _weekOffset == 0 ? "CA TẬP HÔM NAY" : "CA TẬP ĐẦU TUẦN";
            string card2Val = _weekOffset == 0 ? "4" : (_weekOffset > 0 ? "3" : "5");
            string card2Sub = _weekOffset == 0 ? "Đã hoàn thành 2 buổi" : (_weekOffset > 0 ? "Kế hoạch ngày đầu tuần" : "Đã hoàn thành tất cả");
            DrawStatCard(g, new Rectangle(x2, y, cardWidth, cardHeight),
                title: card2Title,
                mainValue: card2Val,
                subUnit: "/ 5",
                subText: card2Sub,
                subColor: PTUITheme.TextMuted);

            // Card 3: Ca tập theo tuần
            int x3 = x2 + cardWidth + gap;
            DrawStatCard(g, new Rectangle(x3, y, cardWidth, cardHeight),
                title: "CA TẬP THEO TUẦN",
                mainValue: _weeklySessionsCount,
                subUnit: "",
                subText: weekRangeText,
                subColor: PTUITheme.TextMuted);

            // Card 4: Tổng thời gian dạy trong tuần
            int x4 = x3 + cardWidth + gap;
            DrawStatCard(g, new Rectangle(x4, y, cardWidth, cardHeight),
                title: "TỔNG THỜI GIAN DẠY",
                mainValue: _weeklyHours,
                subUnit: "h",
                subText: "Mục tiêu: 32h / tuần",
                subColor: PTUITheme.TextMuted);
        }

        private void DrawStatCard(Graphics g, Rectangle bounds, string title, string mainValue, string subUnit, string subText, Color subColor)
        {
            // Nền bo góc của thẻ thống kê
            PTUITheme.DrawCard(g, bounds, radius: 12);

            int innerPad = bounds.Width < 170 ? 12 : 16;

            // 1. Dòng tiêu đề
            int titleY = bounds.Y + 14;
            Rectangle titleRect = new Rectangle(bounds.X + innerPad, titleY, bounds.Width - (innerPad * 2), 20);
            Font titleFont = bounds.Width < 170 ? PTUITheme.FontSmall : PTUITheme.FontSmallBold;
            TextRenderer.DrawText(g, title, titleFont, titleRect, PTUITheme.TextMuted,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            // 2. Con số thống kê chính và đơn vị (Hiển thị liền mạch, thẳng hàng cùng 1 dòng)
            int valX = bounds.X + innerPad;
            int valY = bounds.Y + 40;

            Font valFont = bounds.Width < 170 ? new Font("Segoe UI", 18f, FontStyle.Bold) : PTUITheme.FontStatNumber;
            Size mainSize = TextRenderer.MeasureText(g, mainValue, valFont, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
            TextRenderer.DrawText(g, mainValue, valFont, new Point(valX, valY), PTUITheme.TextWhite, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);

            // Đơn vị đi kèm ("/ 5" hoặc "h") căn chuẩn chân chữ với số chính
            if (!string.IsNullOrEmpty(subUnit))
            {
                string cleanUnit = subUnit.Trim();
                Font unitFont = new Font("Segoe UI", 12f, FontStyle.Bold);
                Size unitSize = TextRenderer.MeasureText(g, cleanUnit, unitFont, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);

                int unitX = valX + mainSize.Width + 6;
                // Căn thẳng hàng đáy chữ (baseline) chính xác
                int unitY = valY + (mainSize.Height - unitSize.Height) - 2;

                TextRenderer.DrawText(g, cleanUnit, unitFont, new Point(unitX, unitY), PTUITheme.TextMuted, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
            }

            // 3. Dòng phụ chú dưới chân thẻ
            int subY = bounds.Bottom - 26;
            Rectangle subRect = new Rectangle(bounds.X + innerPad, subY, bounds.Width - (innerPad * 2), 18);
            TextRenderer.DrawText(g, subText, PTUITheme.FontSmall, subRect, subColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }
    }
}
