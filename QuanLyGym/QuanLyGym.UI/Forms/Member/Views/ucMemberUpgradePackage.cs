using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyGym.DAL.Entities;
using QuanLyGym.UI.Helpers;
using QuanLyGym.UI.Services;
using QuanLyGym.UI.Utils;

namespace QuanLyGym.UI.Forms.Member.Views
{
    public class ucMemberUpgradePackage : UserControl
    {
        private Action<string>? _navigateCallback;

        public ucMemberUpgradePackage(Action<string>? navigateCallback = null)
        {
            _navigateCallback = navigateCallback;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = UITheme.Background;
            this.Dock = DockStyle.Fill;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(1000, 750);

            float scale = Math.Max(1.0f, (float)this.DeviceDpi / 96f);

            TableLayoutPanel pnlRootLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding((int)(16 * scale), (int)(12 * scale), (int)(16 * scale), (int)(14 * scale)),
                BackColor = UITheme.Background
            };
            pnlRootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            pnlRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, (int)(95 * scale)));  // Hàng 0: Hero Banner
            pnlRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, (int)(330 * scale))); // Hàng 1: Thẻ gói PT (truy vấn động từ CSDL)
            pnlRootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));                // Hàng 2: Bảng so sánh chi tiết
            // 1. Hero Banner
            RoundedPanel pnlHero = CreateHeroBanner(scale);
            pnlHero.Dock = DockStyle.Fill;
            pnlHero.Margin = new Padding(0, 0, 0, (int)(10 * scale));

            // 2. Thẻ gói PT (truy vấn động từ CSDL qua MemberService)
            TableLayoutPanel pnlCards = CreatePackagesRow(scale);
            pnlCards.Dock = DockStyle.Fill;
            pnlCards.Margin = new Padding(0, 0, 0, (int)(10 * scale));

            // 3. Bảng so sánh quyền lợi (truy vấn động tất cả các gói từ CSDL)
            RoundedPanel pnlComparison = CreateComparisonCard(scale);
            pnlComparison.Dock = DockStyle.Fill;
            pnlComparison.Margin = new Padding(0);

            pnlRootLayout.Controls.Add(pnlHero, 0, 0);
            pnlRootLayout.Controls.Add(pnlCards, 0, 1);
            pnlRootLayout.Controls.Add(pnlComparison, 0, 2);

            this.Controls.Add(pnlRootLayout);
            this.ResumeLayout(true);
        }

        private RoundedPanel CreateHeroBanner(float scale)
        {
            RoundedPanel banner = new RoundedPanel
            {
                CornerRadius = 14,
                BackColor = Color.FromArgb(46, 16, 101),       // Deep Purple
                GradientEndColor = Color.FromArgb(109, 40, 217), // Violet 700
                GradientAngle = 25f,
                BorderColor = Color.FromArgb(245, 158, 11),
                BorderWidth = 1,
                Padding = new Padding(0)
            };

            Panel pnlRightAction = new Panel
            {
                Dock = DockStyle.Right,
                Width = (int)(210 * scale),
                Padding = new Padding(0, (int)(24 * scale), (int)(16 * scale), (int)(24 * scale)),
                BackColor = Color.Transparent
            };

            Button btnBackToCurrent = new Button
            {
                Text = "💳 Xem gói hiện tại",
                Dock = DockStyle.Fill,
                Font = UITheme.FontBold,
                BackColor = Color.FromArgb(254, 243, 199),
                ForeColor = Color.FromArgb(180, 83, 9),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnBackToCurrent.FlatAppearance.BorderSize = 0;
            btnBackToCurrent.Click += (s, e) => _navigateCallback?.Invoke("Membership");
            pnlRightAction.Controls.Add(btnBackToCurrent);

            FlowLayoutPanel flowText = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding((int)(20 * scale), (int)(12 * scale), 0, 0),
                BackColor = Color.Transparent
            };

            Label lblTitle = new Label
            {
                Text = "⚡ NÂNG CẤP LỘ TRÌNH 1-1 CÙNG HUẤN LUYỆN VIÊN CHUYÊN NGHIỆP",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, (int)(3 * scale)),
                UseMnemonic = false
            };

            Label lblDesc = new Label
            {
                Text = "Chuyển từ gói tự do Classic lên gói có PT để được HLV thiết kế giáo án riêng, cam kết hiệu quả tăng cơ giảm mỡ rõ rệt.",
                Font = UITheme.FontSmall,
                ForeColor = Color.FromArgb(237, 233, 254),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, (int)(3 * scale)),
                UseMnemonic = false
            };

            Label lblOffer = new Label
            {
                Text = "🎁 ĐẶC QUYỀN HỘI VIÊN CLASSIC: Giảm 10% học phí + TẶNG THÊM 03 BUỔI PT khi đăng ký nâng cấp trong tháng này!",
                Font = new Font("Segoe UI", 8.2f, FontStyle.Bold),
                ForeColor = Color.FromArgb(253, 224, 71), // Yellow
                AutoSize = true,
                Margin = new Padding(0),
                UseMnemonic = false
            };

            flowText.Controls.Add(lblTitle);
            flowText.Controls.Add(lblDesc);
            flowText.Controls.Add(lblOffer);

            banner.Controls.Add(flowText);
            banner.Controls.Add(pnlRightAction);

            return banner;
        }

        private TableLayoutPanel CreatePackagesRow(float scale)
        {
            // Truy vấn động danh sách các gói PT từ CSDL
            List<Package> ptPackages = MemberService.Instance.GetPTPackages();
            int count = Math.Max(1, ptPackages.Count);

            TableLayoutPanel table = new TableLayoutPanel
            {
                ColumnCount = count,
                RowCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(0)
            };

            float percentPerCol = 100f / count;
            for (int i = 0; i < count; i++)
            {
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, percentPerCol));
            }
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            for (int i = 0; i < ptPackages.Count; i++)
            {
                Package pkg = ptPackages[i];
                RoundedPanel card = CreateDynamicPackageCard(pkg, scale);
                table.Controls.Add(card, i, 0);
            }

            return table;
        }

        private RoundedPanel CreateDynamicPackageCard(Package pkg, float scale)
        {
            string lowerName = pkg.PackageName.ToLower();

            // Nhận diện kiểu dáng và màu sắc theo gói tập trong CSDL
            string badgeText;
            Color themeColor;
            Color headerBg;
            Color borderColor;
            bool isFeatured = false;

            if (lowerName.Contains("gold"))
            {
                badgeText = "PHỔ BIẾN NHẤT ⭐";
                themeColor = Color.FromArgb(217, 119, 6);        // Amber 600
                headerBg = Color.FromArgb(254, 243, 199);         // Amber 100
                borderColor = Color.FromArgb(245, 158, 11);       // Amber 500
                isFeatured = true;
            }
            else if (lowerName.Contains("diamond") || lowerName.Contains("vip"))
            {
                badgeText = "CAO CẤP NHẤT 💎";
                themeColor = Color.FromArgb(109, 40, 217);      // Violet 700
                headerBg = Color.FromArgb(245, 243, 255);        // Violet 50
                borderColor = Color.FromArgb(167, 139, 250);      // Violet 400
                isFeatured = false;
            }
            else if (lowerName.Contains("silver"))
            {
                badgeText = "TIẾT KIỆM & HIỆU QUẢ";
                themeColor = Color.FromArgb(71, 85, 105);        // Slate 600
                headerBg = Color.FromArgb(241, 245, 249);        // Slate 100
                borderColor = Color.FromArgb(203, 213, 225);      // Slate 300
                isFeatured = false;
            }
            else
            {
                badgeText = "GÓI HUẤN LUYỆN 1-1";
                themeColor = Color.FromArgb(37, 99, 235);        // Blue 600
                headerBg = Color.FromArgb(239, 246, 255);        // Blue 50
                borderColor = Color.FromArgb(191, 219, 254);      // Blue 200
                isFeatured = false;
            }

            string title = pkg.PackageName.ToUpper().StartsWith("GÓI") 
                ? pkg.PackageName.ToUpper() 
                : $"GÓI {pkg.PackageName.ToUpper()} PT";

            string subtitle = !string.IsNullOrWhiteSpace(pkg.Description) 
                ? pkg.Description 
                : "Huấn luyện chuyên sâu cùng HLV";

            string priceText = $"{pkg.Price:N0} đ";
            string durationInfo = $"Hạn: {pkg.Duration} ngày | {pkg.TotalPTSessions} Buổi PT 1-1";

            // Phân tích quyền lợi động từ CSDL
            List<string> perks = new List<string>();
            if (!string.IsNullOrWhiteSpace(pkg.Benefits))
            {
                var splitItems = pkg.Benefits.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in splitItems)
                {
                    string clean = item.Trim();
                    if (!string.IsNullOrEmpty(clean))
                    {
                        if (!clean.StartsWith("✓") && !clean.StartsWith("-") && !clean.StartsWith("•"))
                        {
                            clean = "✓ " + clean;
                        }
                        perks.Add(clean);
                    }
                }
            }

            // Bổ sung các đặc quyền tiêu chuẩn nếu trong CSDL chưa ghi đủ
            if (!perks.Any(p => p.ToLower().Contains("tự do")))
            {
                perks.Add("✓ Luyện tập tự do toàn hệ thống Titan Fitness");
            }

            if (pkg.TotalPTSessions >= 24 && !perks.Any(p => p.ToLower().Contains("xông hơi") || p.ToLower().Contains("sauna")))
            {
                perks.Add("✓ Tủ Smart Locker riêng & Phòng xông hơi Sauna VIP");
            }
            else if (!perks.Any(p => p.ToLower().Contains("locker")))
            {
                perks.Add("✓ Miễn phí tủ đồ Smart Locker & nước uống");
            }

            RoundedPanel card = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                CornerRadius = 14,
                BackColor = UITheme.CardBg,
                BorderColor = borderColor,
                BorderWidth = isFeatured ? 2 : 1,
                Margin = new Padding((int)(5 * scale)),
                Padding = new Padding((int)(12 * scale), (int)(10 * scale), (int)(12 * scale), (int)(10 * scale))
            };

            // Header Section
            int headerH = (int)Math.Round(72f * scale);
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = headerH,
                BackColor = headerBg,
                Padding = new Padding((int)(10 * scale), (int)(6 * scale), (int)(10 * scale), (int)(6 * scale))
            };

            Label lblBadge = new Label
            {
                Text = badgeText,
                Font = new Font("Segoe UI", 7.2f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = themeColor,
                Padding = new Padding((int)(6 * scale), (int)(2 * scale), (int)(6 * scale), (int)(2 * scale)),
                AutoSize = true,
                Location = new Point((int)(10 * scale), (int)(6 * scale)),
                UseMnemonic = false
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
                ForeColor = themeColor,
                AutoSize = true,
                Location = new Point((int)(10 * scale), (int)(26 * scale)),
                UseMnemonic = false
            };

            Label lblSub = new Label
            {
                Text = subtitle.Length > 45 ? subtitle.Substring(0, 42) + "..." : subtitle,
                Font = new Font("Segoe UI", 8f),
                ForeColor = UITheme.TextMuted,
                AutoSize = true,
                Location = new Point((int)(10 * scale), (int)(48 * scale)),
                UseMnemonic = false
            };

            Label lblPrice = new Label
            {
                Text = priceText,
                Font = new Font("Segoe UI", 12.5f, FontStyle.Bold),
                ForeColor = themeColor,
                AutoSize = true,
                Location = new Point(Math.Max(120, pnlHeader.Width - (int)(130 * scale)), (int)(14 * scale)),
                UseMnemonic = false
            };

            pnlHeader.Controls.Add(lblBadge);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);
            pnlHeader.Controls.Add(lblPrice);

            pnlHeader.Resize += (s, e) =>
            {
                lblPrice.Location = new Point(Math.Max(120, pnlHeader.Width - lblPrice.Width - (int)(10 * scale)), (int)(14 * scale));
            };

            // Duration info strip
            Label lblDuration = new Label
            {
                Text = "⏱️ " + durationInfo,
                Font = UITheme.FontSmallBold,
                ForeColor = themeColor,
                Dock = DockStyle.Top,
                Height = (int)(22 * scale),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding((int)(4 * scale), 0, 0, 0),
                UseMnemonic = false
            };

            // Action Button (kết nối trực tiếp với RequestUpgradePackage)
            Button btnAction = new Button
            {
                Text = isFeatured ? $"⚡ Đăng ký {pkg.PackageName} ngay" : $"Đăng ký {title}",
                Font = UITheme.FontBold,
                Dock = DockStyle.Bottom,
                Height = (int)(36 * scale),
                BackColor = themeColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnAction.FlatAppearance.BorderSize = 0;
            btnAction.Click += (s, e) =>
            {
                var ask = MessageBox.Show(
                    $"Bạn có chắc chắn muốn gửi yêu cầu nâng cấp lên {pkg.PackageName}?\n\n" +
                    $"• Giá niêm yết trong hệ thống: {pkg.Price:N0} đ\n" +
                    $"• Thời hạn sử dụng: {pkg.Duration} ngày\n" +
                    $"• Số buổi PT kèm 1-1: {pkg.TotalPTSessions} buổi\n\n" +
                    "Bộ phận Lễ tân và Huấn luyện viên trưởng sẽ liên hệ trực tiếp để tư vấn giáo án và kích hoạt gói cho bạn.",
                    "Xác nhận nâng cấp gói tập",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (ask == DialogResult.Yes)
                {
                    bool ok = MemberService.Instance.RequestUpgradePackage(pkg.PackageID, out string msg);
                    if (ok)
                    {
                        MessageBox.Show(msg, "Đăng ký thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _navigateCallback?.Invoke("Membership");
                    }
                    else
                    {
                        MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            };

            // Perks List
            FlowLayoutPanel flowPerks = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding((int)(4 * scale), (int)(6 * scale), 0, 0),
                BackColor = Color.Transparent
            };

            foreach (var p in perks)
            {
                Label item = new Label
                {
                    Text = p,
                    Font = UITheme.FontSmall,
                    ForeColor = p.Contains("TẶNG") ? Color.FromArgb(180, 83, 9) : UITheme.TextDark,
                    AutoSize = true,
                    Margin = new Padding(0, 0, 0, (int)(4 * scale)),
                    UseMnemonic = false
                };
                if (p.Contains("TẶNG"))
                {
                    item.Font = new Font("Segoe UI", 8.2f, FontStyle.Bold);
                }
                flowPerks.Controls.Add(item);
            }

            card.Controls.Add(flowPerks);
            card.Controls.Add(btnAction);
            card.Controls.Add(lblDuration);
            card.Controls.Add(pnlHeader);

            return card;
        }

        private RoundedPanel CreateComparisonCard(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 14,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding((int)(16 * scale), (int)(10 * scale), (int)(16 * scale), (int)(10 * scale))
            };

            int titleH = (int)Math.Round(30f * scale);
            Label lblTitle = new Label
            {
                Text = "📊 BẢNG SO SÁNH QUYỀN LỢI CHI TIẾT GIỮA CÁC GÓI TẬP (DỮ LIỆU CSDL)",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = titleH,
                UseMnemonic = false
            };

            DataGridView dgv = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgv);
            dgv.RowTemplate.Height = (int)Math.Round(34f * scale);
            dgv.ColumnHeadersHeight = (int)Math.Round(34f * scale);

            // Lấy tất cả các gói từ CSDL
            List<Package> allPackages = MemberService.Instance.GetAllPackages();

            // Cột tiêu chí
            dgv.Columns.Add(UITheme.CreateFillColumn("Criterion", "Tiêu chí so sánh", 25f, 130));

            // Thêm cột động cho từng gói tập
            float colWeight = 75f / Math.Max(1, allPackages.Count);
            foreach (var pkg in allPackages)
            {
                string headerName;
                if (!pkg.IsPTPackage)
                {
                    headerName = $"{pkg.PackageName} (Hiện tại)";
                }
                else if (pkg.PackageName.ToLower().Contains("gold"))
                {
                    headerName = $"{pkg.PackageName} PT ⭐";
                }
                else if (pkg.PackageName.ToLower().Contains("diamond") || pkg.PackageName.ToLower().Contains("vip"))
                {
                    headerName = $"{pkg.PackageName} 💎";
                }
                else
                {
                    headerName = $"{pkg.PackageName} PT";
                }

                dgv.Columns.Add(UITheme.CreateFillColumn($"pkg_{pkg.PackageID}", headerName, colWeight, 95));
            }

            // Dòng 1: Giá niêm yết
            List<object> rowPrice = new List<object> { "Giá gói tập (CSDL)" };
            foreach (var pkg in allPackages)
            {
                rowPrice.Add($"{pkg.Price:N0} đ / {pkg.Duration} ngày");
            }
            dgv.Rows.Add(rowPrice.ToArray());

            // Dòng 2: Huấn luyện viên cá nhân (PT)
            List<object> rowPT = new List<object> { "Huấn luyện viên cá nhân (PT)" };
            foreach (var pkg in allPackages)
            {
                rowPT.Add(pkg.IsPTPackage ? $"{pkg.TotalPTSessions} buổi kèm 1-1" : "Không có (Tập tự do)");
            }
            dgv.Rows.Add(rowPT.ToArray());

            // Dòng 3: Thời hạn gói
            List<object> rowDuration = new List<object> { "Thời hạn thẻ tập" };
            foreach (var pkg in allPackages)
            {
                rowDuration.Add($"{pkg.Duration} ngày");
            }
            dgv.Rows.Add(rowDuration.ToArray());

            // Dòng 4: Giáo án luyện tập
            List<object> rowPlan = new List<object> { "Giáo án luyện tập" };
            foreach (var pkg in allPackages)
            {
                if (!pkg.IsPTPackage)
                    rowPlan.Add("Tự tìm hiểu & luyện tập");
                else if (pkg.TotalPTSessions <= 12)
                    rowPlan.Add("Giáo án chuẩn định hướng");
                else if (pkg.TotalPTSessions <= 24)
                    rowPlan.Add("Cá nhân hóa theo thể trạng");
                else
                    rowPlan.Add("Master chuyên biệt 100%");
            }
            dgv.Rows.Add(rowPlan.ToArray());

            // Dòng 5: Chế độ dinh dưỡng
            List<object> rowDiet = new List<object> { "Chế độ dinh dưỡng" };
            foreach (var pkg in allPackages)
            {
                if (!pkg.IsPTPackage)
                    rowDiet.Add("Không có");
                else if (pkg.TotalPTSessions <= 12)
                    rowDiet.Add("Tư vấn cơ bản");
                else if (pkg.TotalPTSessions <= 24)
                    rowDiet.Add("Thực đơn chi tiết hàng tuần");
                else
                    rowDiet.Add("Chuyên gia dinh dưỡng 24/7");
            }
            dgv.Rows.Add(rowDiet.ToArray());

            // Dòng 6: Tủ Smart Locker & Tiện ích
            List<object> rowLocker = new List<object> { "Tủ Smart Locker cá nhân" };
            foreach (var pkg in allPackages)
            {
                if (!pkg.IsPTPackage)
                    rowLocker.Add("Tủ dùng chung theo buổi");
                else if (pkg.TotalPTSessions <= 12)
                    rowLocker.Add("Smart Locker theo buổi");
                else if (pkg.TotalPTSessions <= 24)
                    rowLocker.Add("Smart Locker riêng & Sauna");
                else
                    rowLocker.Add("Smart Locker VIP & Sauna trọn đời");
            }
            dgv.Rows.Add(rowLocker.ToArray());

            // Dòng 7: Đo chỉ số InBody
            List<object> rowInBody = new List<object> { "Đo chỉ số InBody & vóc dáng" };
            foreach (var pkg in allPackages)
            {
                if (!pkg.IsPTPackage)
                    rowInBody.Add("1 lần / tháng");
                else if (pkg.TotalPTSessions <= 12)
                    rowInBody.Add("2 lần / tháng");
                else if (pkg.TotalPTSessions <= 24)
                    rowInBody.Add("Hàng tuần");
                else
                    rowInBody.Add("Bất kỳ lúc nào");
            }
            dgv.Rows.Add(rowInBody.ToArray());

            // Dòng 8: Cam kết kết quả thể hình
            List<object> rowCommitment = new List<object> { "Cam kết kết quả thể hình" };
            foreach (var pkg in allPackages)
            {
                if (!pkg.IsPTPackage)
                    rowCommitment.Add("Không cam kết");
                else if (pkg.TotalPTSessions <= 12)
                    rowCommitment.Add("Hỗ trợ chuẩn form");
                else if (pkg.TotalPTSessions <= 24)
                    rowCommitment.Add("Cam kết tăng cơ / giảm mỡ");
                else
                    rowCommitment.Add("Cam kết bằng văn bản");
            }
            dgv.Rows.Add(rowCommitment.ToArray());

            dgv.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex == 1 && e.Value != null)
                {
                    e.CellStyle.ForeColor = UITheme.TextMuted;
                }
                else if (e.ColumnIndex >= 2 && e.ColumnIndex < dgv.Columns.Count && e.Value != null)
                {
                    string colHeader = dgv.Columns[e.ColumnIndex].HeaderText;
                    if (colHeader.Contains("Gold"))
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(217, 119, 6);
                        e.CellStyle.Font = UITheme.FontBold;
                    }
                    else if (colHeader.Contains("Diamond") || colHeader.Contains("VIP"))
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(109, 40, 217);
                        e.CellStyle.Font = UITheme.FontBold;
                    }
                }
            };

            card.Controls.Add(dgv);
            card.Controls.Add(lblTitle);

            return card;
        }
    }
}
