using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public static class AdminTheme
    {
        // Backgrounds
        public static readonly Color BgDark = Color.FromArgb(14, 15, 18);        // #0E0F12 Main app background
        public static readonly Color HeaderBg = Color.FromArgb(18, 19, 24);      // #121318 Top bar & tab bar background
        public static readonly Color CardBg = Color.FromArgb(22, 23, 29);        // #16171D Card / container background
        public static readonly Color CardBorder = Color.FromArgb(36, 38, 48);    // #242630 Subtle border for cards
        public static readonly Color CardHover = Color.FromArgb(28, 30, 38);     // Card hover color
        public static readonly Color GridLine = Color.FromArgb(32, 34, 42);      // Gridlines in charts

        // Accent & Brand Colors
        public static readonly Color PrimaryOrange = Color.FromArgb(255, 107, 0); // #FF6B00 Gymfit brand orange
        public static readonly Color OrangeHover = Color.FromArgb(255, 128, 32);  // Hover state for orange
        public static readonly Color Green = Color.FromArgb(16, 185, 129);       // #10B981 Branch KPI
        public static readonly Color Blue = Color.FromArgb(47, 128, 237);        // #2F80ED Member KPI & chart
        public static readonly Color Purple = Color.FromArgb(168, 85, 247);      // #A855F7 Employee KPI

        // Status Badges (Sắp hết hạn)
        public static readonly Color BadgeRed = Color.FromArgb(185, 28, 28);      // #B91C1C (1N, 3N)
        public static readonly Color BadgeOrange = Color.FromArgb(234, 88, 12);   // #EA580C (5N)
        public static readonly Color BadgeAmber = Color.FromArgb(217, 119, 6);    // #D97706 (8N)
        public static readonly Color BadgeGold = Color.FromArgb(202, 138, 4);     // #CA8A04 (12N)

        // Text Colors
        public static readonly Color TextWhite = Color.FromArgb(255, 255, 255);   // White primary text
        public static readonly Color TextMuted = Color.FromArgb(142, 144, 155);  // #8E909B Secondary labels
        public static readonly Color TextDim = Color.FromArgb(92, 94, 106);      // #5C5E6A Muted axes & subtext
        public static readonly Color TextInactiveTab = Color.FromArgb(160, 162, 172);

        // Fonts
        public static readonly Font FontHeading = new Font("Segoe UI", 16F, FontStyle.Bold);
        public static readonly Font FontSubheading = new Font("Segoe UI", 11F, FontStyle.Bold);
        public static readonly Font FontCardTitle = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        public static readonly Font FontKpiValue = new Font("Segoe UI", 20F, FontStyle.Bold);
        public static readonly Font FontKpiSub = new Font("Segoe UI", 8F, FontStyle.Regular);
        public static readonly Font FontBodyBold = new Font("Segoe UI", 9F, FontStyle.Bold);
        public static readonly Font FontBody = new Font("Segoe UI", 9F, FontStyle.Regular);
        public static readonly Font FontSmall = new Font("Segoe UI", 8F, FontStyle.Regular);
        public static readonly Font FontBadge = new Font("Segoe UI", 8F, FontStyle.Bold);

        /// <summary>
        /// Tạo GraphicsPath hình chữ nhật bo tròn góc
        /// </summary>
        public static GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            if (rect.Width <= 0 || rect.Height <= 0)
                return path;

            int diameter = radius * 2;
            var arc = new Rectangle(rect.Location, new Size(diameter, diameter));

            // Top-left
            path.AddArc(arc, 180, 90);

            // Top-right
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom-right
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Bottom-left
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Vẽ một thẻ Card với nền và viền bo tròn chuẩn dark theme
        /// </summary>
        public static void DrawCard(Graphics g, Rectangle bounds, int radius = 8, Color? customBg = null, Color? customBorder = null)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var drawRect = new Rectangle(bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
            if (drawRect.Width <= 0 || drawRect.Height <= 0) return;

            using (var path = GetRoundedRectanglePath(drawRect, radius))
            {
                using (var brush = new SolidBrush(customBg ?? CardBg))
                {
                    g.FillPath(brush, path);
                }
                using (var pen = new Pen(customBorder ?? CardBorder, 1f))
                {
                    g.DrawPath(pen, path);
                }
            }
        }
    }
}
