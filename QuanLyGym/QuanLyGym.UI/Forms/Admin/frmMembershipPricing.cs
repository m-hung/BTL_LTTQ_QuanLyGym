using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public class frmMembershipPricing : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private static readonly Color BgColor = Color.FromArgb(15, 15, 17);
        private static readonly Color CardBg = Color.FromArgb(24, 24, 27);
        private static readonly Color ItemBg = Color.FromArgb(20, 20, 23);
        private static readonly Color Border = Color.FromArgb(45, 45, 50);
        private static readonly Color TextColor = Color.FromArgb(244, 244, 245);
        private static readonly Color MutedColor = Color.FromArgb(161, 161, 170);
        private static readonly Color Orange = Color.FromArgb(249, 115, 22);
        private static readonly Color Blue = Color.FromArgb(59, 130, 246);
        private static readonly Color Cyan = Color.FromArgb(6, 182, 212);
        private static readonly Color Gold = Color.FromArgb(234, 179, 8);
        private static readonly Color Purple = Color.FromArgb(168, 85, 247);
        private static readonly Color Green = Color.FromArgb(34, 197, 94);
        private static readonly CultureInfo ViCulture = new CultureInfo("vi-VN");

        public class PricingTier
        {
            public string Name { get; set; } = "";
            public string Subtitle { get; set; } = "";
            public decimal MonthlyPrice { get; set; }
            public decimal HalfYearPrice { get; set; }
            public decimal AnnualPrice { get; set; }
            public Color AccentColor { get; set; }
            public string Badge { get; set; } = "";
            public bool IsPopular { get; set; }
            public List<string> Features { get; set; } = new();
        }

        private static readonly List<PricingTier> Tiers = new()
        {
            new PricingTier
            {
                Name = "Classic",
                Subtitle = "Khởi đầu rèn luyện thể chất",
                MonthlyPrice = 450000,
                HalfYearPrice = 2400000,
                AnnualPrice = 4500000,
                AccentColor = Blue,
                Badge = "CƠ BẢN",
                IsPopular = false,
                Features = new List<string>
                {
                    "✓ Tập tại 1 chi nhánh đăng ký",
                    "✓ Khung giờ chuẩn: 06:00 - 16:00 hàng ngày",
                    "✓ Toàn bộ máy Cardio & Khu tạ cơ bản",
                    "✓ Tủ đồ locker tiêu chuẩn & Phòng tắm nóng lạnh",
                    "✗ Không bao gồm Bể bơi / Sauna",
                    "✗ Chưa bao gồm HLV kèm riêng"
                }
            },
            new PricingTier
            {
                Name = "Silver",
                Subtitle = "Linh hoạt giờ giấc mọi thời điểm",
                MonthlyPrice = 750000,
                HalfYearPrice = 4050000,
                AnnualPrice = 7500000,
                AccentColor = Cyan,
                Badge = "TIÊU CHUẨN",
                IsPopular = false,
                Features = new List<string>
                {
                    "✓ Tập tại 1 chi nhánh đăng ký",
                    "✓ KHÔNG GIỚI HẠN giờ tập (05:30 - 22:30)",
                    "✓ Toàn bộ khu máy tập hiện đại & Khu Crossfit",
                    "✓ Sử dụng phòng xông hơi Sauna khô & ướt",
                    "✓ Tủ locker SmartLock bảo mật cao",
                    "✗ Chưa bao gồm Bể bơi bốn mùa"
                }
            },
            new PricingTier
            {
                Name = "Gold",
                Subtitle = "Trải nghiệm tiện ích Gym & Wellness 5 sao",
                MonthlyPrice = 1200000,
                HalfYearPrice = 6480000,
                AnnualPrice = 12000000,
                AccentColor = Gold,
                Badge = "PHỔ BIẾN NHẤT ⭐",
                IsPopular = true,
                Features = new List<string>
                {
                    "✓ Tập tại 1 chi nhánh chính + 1 chi nhánh phụ",
                    "✓ Giờ tập không giới hạn 05:30 - 22:30",
                    "✓ Bể bơi bốn mùa nước ấm & Sauna đá muối",
                    "✓ Tham gia tất cả lớp Yoga, Zumba, GroupX",
                    "✓ Tặng 02 buổi tập kèm 1-1 cùng HLV cá nhân",
                    "✓ Nước uống giải khát & Detox hoa quả miễn phí"
                }
            },
            new PricingTier
            {
                Name = "Platinum",
                Subtitle = "Thẻ thương gia toàn quốc không giới hạn",
                MonthlyPrice = 2000000,
                HalfYearPrice = 10800000,
                AnnualPrice = 20000000,
                AccentColor = Purple,
                Badge = "THƯƠNG GIA",
                IsPopular = false,
                Features = new List<string>
                {
                    "✓ Tập tự do tại TẤT CẢ chi nhánh toàn quốc",
                    "✓ Đầy đủ tiện ích Gym, Bể bơi, Sauna, Boxing",
                    "✓ Khăn tập cao cấp miễn phí mỗi buổi",
                    "✓ Tủ đồ SmartLock cố định riêng",
                    "✓ Tặng 04 buổi tập cùng PT cá nhân chuyên sâu",
                    "✓ Miễn phí đo chỉ số InBody định kỳ hàng tháng"
                }
            },
            new PricingTier
            {
                Name = "VIP Diamond",
                Subtitle = "Đẳng cấp thượng lưu & Dịch vụ tối thượng",
                MonthlyPrice = 3500000,
                HalfYearPrice = 18900000,
                AnnualPrice = 35000000,
                AccentColor = Orange,
                Badge = "ĐẶC QUYỀN VIP 💎",
                IsPopular = false,
                Features = new List<string>
                {
                    "✓ Toàn quyền đặc quyền hệ thống trên toàn quốc",
                    "✓ Sử dụng phòng VIP Lounge & Ghế Massage cao cấp",
                    "✓ 08 buổi tập kèm 1-1 chuyên sâu cùng Master PT",
                    "✓ Đồ uống Whey Protein & BCAA không giới hạn",
                    "✓ Tặng 01 Thẻ phụ miễn phí cho người thân / đối tác",
                    "✓ Chỗ đỗ xe ô tô VIP ưu tiên trước sảnh"
                }
            }
        };

        public frmMembershipPricing()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(1180, 680);
            BackColor = BgColor;
            ShowInTaskbar = false;
            DoubleBuffered = true;

            Paint += (s, e) =>
            {
                using var p = new Pen(Border, 1.5f);
                e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
            };

            // 1. Header Bar
            var pnlTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = ItemBg,
                Padding = new Padding(20, 0, 16, 0)
            };
            pnlTitle.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };
            pnlTitle.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawLine(p, 0, pnlTitle.Height - 1, pnlTitle.Width, pnlTitle.Height - 1);
            };

            var lblTitle = new Label
            {
                Text = "⭐  BẢNG GIÁ & CHÍNH SÁCH QUYỀN LỢI CÁC HẠNG THẺ HỘI VIÊN",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(20, 12)
            };

            var lblSub = new Label
            {
                Text = "Áp dụng đồng bộ trên toàn hệ thống GymFit - Hỗ trợ nâng cấp linh hoạt bù chênh lệch",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                AutoSize = true,
                Location = new Point(22, 35)
            };

            var btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = MutedColor,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(36, 36),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(pnlTitle.Width - 48, 12),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            pnlTitle.Controls.Add(lblTitle);
            pnlTitle.Controls.Add(lblSub);
            pnlTitle.Controls.Add(btnClose);

            // 2. Bottom Close Button Bar
            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 56,
                BackColor = ItemBg,
                Padding = new Padding(20, 10, 20, 10)
            };
            pnlBottom.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawLine(p, 0, 0, pnlBottom.Width, 0);
            };

            var lblBottomNote = new Label
            {
                Text = "💡 Mẹo: Khi nâng cấp thẻ, hệ thống sẽ tự động trừ đi giá trị các ngày chưa sử dụng của gói cũ.",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Italic),
                ForeColor = MutedColor,
                Dock = DockStyle.Left,
                AutoSize = true,
                Padding = new Padding(0, 8, 0, 0)
            };

            var btnDone = new Button
            {
                Text = "Đóng bảng giá",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(130, 36),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnDone.FlatAppearance.BorderSize = 0;
            btnDone.Click += (s, e) => this.Close();

            pnlBottom.Controls.Add(lblBottomNote);
            pnlBottom.Controls.Add(btnDone);

            // 3. Main Center Content: 5 Tier Cards in a responsive table
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16, 16, 16, 16),
                BackColor = BgColor,
                AutoScroll = true
            };

            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            for (int i = 0; i < 5; i++)
            {
                tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            }

            for (int i = 0; i < Tiers.Count; i++)
            {
                tbl.Controls.Add(CreateTierCard(Tiers[i]), i, 0);
            }

            pnlBody.Controls.Add(tbl);

            Controls.Add(pnlBody);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTitle);
        }

        private Control CreateTierCard(PricingTier tier)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Margin = new Padding(6, 4, 6, 4),
                Padding = new Padding(14, 14, 14, 14)
            };

            pnl.Paint += (s, e) =>
            {
                var g = e.Graphics;
                using var pen = new Pen(tier.IsPopular ? tier.AccentColor : Border, tier.IsPopular ? 2f : 1f);
                g.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);

                if (tier.IsPopular)
                {
                    using var brush = new SolidBrush(tier.AccentColor);
                    g.FillRectangle(brush, 0, 0, pnl.Width, 4);
                }
            };

            // Badge
            var lblBadge = new Label
            {
                Text = tier.Badge,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = tier.AccentColor,
                BackColor = Color.FromArgb(35, tier.AccentColor.R, tier.AccentColor.G, tier.AccentColor.B),
                Padding = new Padding(6, 3, 6, 3),
                AutoSize = true,
                Dock = DockStyle.Top
            };

            // Title
            var lblName = new Label
            {
                Text = tier.Name,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = TextColor,
                Dock = DockStyle.Top,
                Height = 32,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Subtitle
            var lblSub = new Label
            {
                Text = tier.Subtitle,
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = MutedColor,
                Dock = DockStyle.Top,
                Height = 28
            };

            // Monthly Price
            var pnlPrice = new Panel
            {
                Dock = DockStyle.Top,
                Height = 68,
                BackColor = ItemBg,
                Margin = new Padding(0, 6, 0, 10),
                Padding = new Padding(8, 6, 8, 6)
            };
            pnlPrice.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlPrice.Width - 1, pnlPrice.Height - 1);
            };

            var lblPriceNum = new Label
            {
                Text = tier.MonthlyPrice.ToString("N0", ViCulture) + " ₫",
                Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                ForeColor = tier.AccentColor,
                Dock = DockStyle.Top,
                Height = 26,
                TextAlign = ContentAlignment.MiddleCenter
            };
            var lblPriceUnit = new Label
            {
                Text = "theo tháng • Gói 1 năm: " + tier.AnnualPrice.ToString("N0", ViCulture) + " ₫",
                Font = new Font("Segoe UI", 7F),
                ForeColor = MutedColor,
                Dock = DockStyle.Top,
                Height = 20,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlPrice.Controls.Add(lblPriceUnit);
            pnlPrice.Controls.Add(lblPriceNum);

            // Features List
            var flpFeatures = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 8, 0, 0),
                Padding = new Padding(0, 6, 0, 0)
            };

            foreach (var f in tier.Features)
            {
                bool isNegative = f.StartsWith("✗");
                var lblF = new Label
                {
                    Text = f,
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = isNegative ? Color.FromArgb(113, 113, 122) : (f.StartsWith("✓ KHÔNG") ? tier.AccentColor : TextColor),
                    Width = 190,
                    AutoSize = true,
                    Margin = new Padding(0, 0, 0, 8)
                };
                flpFeatures.Controls.Add(lblF);
            }

            pnl.Controls.Add(flpFeatures);
            pnl.Controls.Add(pnlPrice);
            pnl.Controls.Add(lblSub);
            pnl.Controls.Add(lblName);
            pnl.Controls.Add(lblBadge);

            return pnl;
        }
    }
}
