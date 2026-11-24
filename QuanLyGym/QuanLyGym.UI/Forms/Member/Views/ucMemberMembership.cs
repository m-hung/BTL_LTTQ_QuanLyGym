using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using QuanLyGym.UI.Helpers;
using QuanLyGym.UI.Services;
using QuanLyGym.UI.Utils;

namespace QuanLyGym.UI.Forms.Member.Views
{
    public class ucMemberMembership : UserControl
    {
        private Action<string>? _navigateCallback;

        public ucMemberMembership(Action<string>? navigateCallback = null)
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
            this.AutoScrollMinSize = new Size(1000, 640);

            float scale = Math.Max(1.0f, (float)this.DeviceDpi / 96f);

            TableLayoutPanel pnlRootLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding((int)(16 * scale), (int)(12 * scale), (int)(16 * scale), (int)(14 * scale)),
                BackColor = UITheme.Background
            };
            pnlRootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            int topRowH = (int)Math.Round(280f * scale);
            pnlRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, topRowH)); // Hàng 0: Thẻ điện tử & Thông tin gói
            pnlRootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));    // Hàng 1: Lịch sử giao dịch & Đặc quyền

            // 1. Two Column Layout for Top: Digital Card (42%) and Active Package Details (58%)
            TableLayoutPanel pnlTopCols = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 0, 0, (int)(8 * scale))
            };
            pnlTopCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42f));
            pnlTopCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58f));
            pnlTopCols.RowStyles.Clear();
            pnlTopCols.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            RoundedPanel cardDigital = CreateDigitalCard(scale);
            cardDigital.Dock = DockStyle.Fill;
            cardDigital.Margin = new Padding(0, 0, (int)(6 * scale), 0);

            RoundedPanel cardStatus = CreatePackageDetailsCard(scale);
            cardStatus.Dock = DockStyle.Fill;
            cardStatus.Margin = new Padding((int)(6 * scale), 0, 0, 0);

            pnlTopCols.Controls.Add(cardDigital, 0, 0);
            pnlTopCols.Controls.Add(cardStatus, 1, 0);

            // 2. Bottom section: Lịch sử và Đặc quyền
            TableLayoutPanel pnlBottomCols = new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 2,
                Dock = DockStyle.Fill,
                Padding = new Padding(0)
            };
            pnlBottomCols.RowStyles.Add(new RowStyle(SizeType.Percent, 55f)); // Lịch sử GD 55%
            pnlBottomCols.RowStyles.Add(new RowStyle(SizeType.Percent, 45f)); // Đặc quyền 45%
            pnlBottomCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            RoundedPanel cardHistory = CreateHistoryCard(scale);
            cardHistory.Dock = DockStyle.Fill;
            cardHistory.Margin = new Padding(0, 0, 0, (int)(6 * scale));

            RoundedPanel cardPrivileges = CreatePrivilegesCard(scale);
            cardPrivileges.Dock = DockStyle.Fill;
            cardPrivileges.Margin = new Padding(0, (int)(4 * scale), 0, 0);

            pnlBottomCols.Controls.Add(cardHistory, 0, 0);
            pnlBottomCols.Controls.Add(cardPrivileges, 0, 1);

            pnlRootLayout.Controls.Add(pnlTopCols, 0, 0);
            pnlRootLayout.Controls.Add(pnlBottomCols, 0, 1);

            this.Controls.Add(pnlRootLayout);
            this.ResumeLayout(true);
        }

        private RoundedPanel CreateDigitalCard(float scale)
        {
            bool isClassic = MemberService.Instance.IsClassicMember();
            var activeMembership = MemberService.Instance.GetActiveMembership();

            Color bgStart = isClassic ? Color.FromArgb(15, 23, 42) : Color.FromArgb(20, 24, 33);
            Color bgEnd = isClassic ? Color.FromArgb(30, 41, 59) : Color.FromArgb(45, 55, 72);
            Color borderColor = isClassic ? Color.FromArgb(56, 189, 248) : Color.FromArgb(245, 158, 11);

            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 16,
                BackColor = bgStart,
                GradientEndColor = bgEnd,
                GradientAngle = 135f,
                BorderColor = borderColor,
                BorderWidth = 1,
                Padding = new Padding((int)(18 * scale))
            };

            Label lblLogo = new Label
            {
                Text = "⚡ TITAN FITNESS CLUB",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = isClassic ? Color.FromArgb(56, 189, 248) : Color.FromArgb(251, 191, 36),
                AutoSize = true,
                Location = new Point((int)(18 * scale), (int)(16 * scale)),
                UseMnemonic = false
            };

            string badgeText = isClassic ? "CLASSIC MEMBER" : (activeMembership?.Package?.PackageName.ToUpper() ?? "VIP DIAMOND");
            Color badgeBg = isClassic ? Color.FromArgb(14, 165, 233) : Color.FromArgb(79, 70, 229);

            Label lblBadge = new Label
            {
                Text = badgeText,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = badgeBg,
                Padding = new Padding((int)(8 * scale), (int)(3 * scale), (int)(8 * scale), (int)(3 * scale)),
                AutoSize = true,
                UseMnemonic = false
            };

            Panel chip = new Panel
            {
                Size = new Size((int)(38 * scale), (int)(28 * scale)),
                BackColor = isClassic ? Color.FromArgb(56, 189, 248) : Color.FromArgb(245, 158, 11),
                Location = new Point((int)(20 * scale), (int)(56 * scale))
            };

            string cardNumber = $"MB-2026 •••• •••• {UserSession.AccountID:D4}";
            Label lblCardNumber = new Label
            {
                Text = cardNumber,
                Font = new Font("Consolas", 13.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(241, 245, 249),
                AutoSize = true,
                Location = new Point((int)(18 * scale), (int)(110 * scale)),
                UseMnemonic = false
            };

            string memberName = MemberService.Instance.GetCurrentMember()?.FullName?.ToUpper() ?? (string.IsNullOrEmpty(UserSession.DisplayName) ? "HỘI VIÊN TITAN" : UserSession.DisplayName.ToUpper());
            Label lblMemberName = new Label
            {
                Text = memberName,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                UseMnemonic = false
            };

            string expiryText = activeMembership != null
                ? $"HẠN DÙNG: {activeMembership.EndDate:dd/MM/yyyy}"
                : "HẠN DÙNG: 30/11/2026";

            Label lblExpiry = new Label
            {
                Text = expiryText,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(203, 213, 225),
                AutoSize = true,
                UseMnemonic = false
            };

            card.Controls.Add(lblLogo);
            card.Controls.Add(lblBadge);
            card.Controls.Add(chip);
            card.Controls.Add(lblCardNumber);
            card.Controls.Add(lblMemberName);
            card.Controls.Add(lblExpiry);

            card.Resize += (s, e) =>
            {
                lblBadge.Location = new Point(Math.Max(120, card.Width - lblBadge.Width - (int)(18 * scale)), (int)(16 * scale));
                lblMemberName.Location = new Point((int)(18 * scale), Math.Max(120, card.Height - (int)(38 * scale)));
                lblExpiry.Location = new Point(Math.Max(120, card.Width - lblExpiry.Width - (int)(18 * scale)), Math.Max(120, card.Height - (int)(38 * scale)));
            };

            return card;
        }

        private RoundedPanel CreatePackageDetailsCard(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding((int)(18 * scale), (int)(12 * scale), (int)(18 * scale), (int)(10 * scale))
            };

            bool isClassic = MemberService.Instance.IsClassicMember();
            var activeMembership = MemberService.Instance.GetActiveMembership();

            Label lblTitle = new Label
            {
                Text = "📋 THÔNG TIN GÓI TẬP ĐANG HOẠT ĐỘNG",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = (int)(28 * scale),
                UseMnemonic = false
            };

            TableLayoutPanel grid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(0, (int)(2 * scale), 0, 0)
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            grid.RowStyles.Clear();
            for (int r = 0; r < 4; r++) grid.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));

            string pkgName = activeMembership?.Package?.PackageName ?? (isClassic ? "Gói tập tự do (Classic)" : "Chưa kích hoạt gói");
            string startDate = activeMembership?.StartDate.ToString("dd/MM/yyyy") ?? "--";
            string endDate = activeMembership?.EndDate.ToString("dd/MM/yyyy") ?? "--";
            string price = activeMembership != null ? activeMembership.Price.ToString("N0") + " VNĐ" : "-- VNĐ";
            string branch = activeMembership?.Branch?.BranchName ?? (MemberService.Instance.GetCurrentMember()?.Branch?.BranchName ?? "Chi nhánh Titan");

            string sessionInfo = isClassic ? "Tự do không giới hạn (Không kèm PT)" : $"{activeMembership?.UsedPTSessions ?? 0} / {activeMembership?.Package?.TotalPTSessions ?? 0} buổi";
            int remainingDays = activeMembership != null ? Math.Max(0, (activeMembership.EndDate.Date - DateTime.Today).Days) : 0;

            grid.Controls.Add(CreateInfoRow("Gói tập:", pkgName), 0, 0);
            grid.Controls.Add(CreateInfoRow("Trạng thái:", activeMembership != null ? "● Đang hoạt động (Active)" : "○ Chưa kích hoạt"), 1, 0);
            grid.Controls.Add(CreateInfoRow("Ngày đăng ký:", startDate), 0, 1);
            grid.Controls.Add(CreateInfoRow("Ngày kết thúc:", endDate), 1, 1);
            grid.Controls.Add(CreateInfoRow("Giá gói:", price), 0, 2);
            grid.Controls.Add(CreateInfoRow("Chi nhánh:", branch), 1, 2);
            grid.Controls.Add(CreateInfoRow(isClassic ? "Hình thức tập:" : "Số buổi đã tập:", sessionInfo), 0, 3);
            grid.Controls.Add(CreateInfoRow("Thời gian còn lại:", $"Còn {remainingDays} ngày"), 1, 3);

            // Bottom Buttons & Progress
            Panel pnlBottomArea = new Panel { Dock = DockStyle.Bottom, Height = (int)(54 * scale) };

            int progressPercent = 0;
            if (activeMembership != null)
            {
                if (isClassic)
                {
                    int totalDays = Math.Max(1, (activeMembership.EndDate.Date - activeMembership.StartDate.Date).Days);
                    int elapsed = Math.Max(0, (DateTime.Today - activeMembership.StartDate.Date).Days);
                    progressPercent = Math.Clamp((int)Math.Round((double)elapsed / totalDays * 100), 0, 100);
                }
                else
                {
                    int totalSessions = activeMembership.Package?.TotalPTSessions ?? 0;
                    int usedSessions = activeMembership.UsedPTSessions;
                    progressPercent = totalSessions > 0 ? Math.Clamp((int)Math.Round((double)usedSessions / totalSessions * 100), 0, 100) : 0;
                }
            }

            ModernProgressBar prog = new ModernProgressBar
            {
                Dock = DockStyle.Top,
                Height = (int)(6 * scale),
                Value = progressPercent,
                Maximum = 100,
                ProgressColor = isClassic ? Color.FromArgb(14, 165, 233) : UITheme.Primary,
                ProgressColor2 = isClassic ? Color.FromArgb(56, 189, 248) : UITheme.Secondary,
                Margin = new Padding(0, 0, 0, (int)(6 * scale))
            };

            FlowLayoutPanel pnlButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = (int)(38 * scale),
                FlowDirection = FlowDirection.LeftToRight
            };

            Button btnExtend = new Button
            {
                Text = "💳 Gia hạn gói tập",
                Font = UITheme.FontBold,
                Size = new Size((int)(160 * scale), (int)(34 * scale)),
                BackColor = isClassic ? Color.FromArgb(14, 165, 233) : UITheme.Primary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnExtend.FlatAppearance.BorderSize = 0;
            btnExtend.Click += (s, e) => MessageBox.Show($"Yêu cầu gia hạn {pkgName} đã được gửi tới bộ phận Lễ tân. Chúng tôi sẽ liên hệ hỗ trợ bạn ngay!", "Gia hạn gói tập", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Button btnUpgrade = new Button
            {
                Text = isClassic ? "⚡ Nâng cấp gói kèm PT 1-1" : "⚡ Nâng cấp Black Card",
                Font = UITheme.FontBold,
                Size = new Size((int)(210 * scale), (int)(34 * scale)),
                BackColor = isClassic ? Color.FromArgb(243, 232, 255) : UITheme.PrimaryLight,
                ForeColor = isClassic ? Color.FromArgb(124, 58, 237) : UITheme.Primary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding((int)(10 * scale), 0, 0, 0),
                UseMnemonic = false
            };
            btnUpgrade.FlatAppearance.BorderSize = 0;
            btnUpgrade.Click += (s, e) =>
            {
                if (isClassic)
                {
                    _navigateCallback?.Invoke("Upgrade");
                }
                else
                {
                    MessageBox.Show("Đặc quyền Black Card: Kèm 30 buổi PT 1-1, Locker cá nhân vĩnh viễn và đồ uống cao cấp không giới hạn.", "Nâng cấp gói", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            pnlButtons.Controls.Add(btnExtend);
            pnlButtons.Controls.Add(btnUpgrade);

            pnlBottomArea.Controls.Add(pnlButtons);
            pnlBottomArea.Controls.Add(prog);

            card.Controls.Add(grid);
            card.Controls.Add(pnlBottomArea);
            card.Controls.Add(lblTitle);

            return card;
        }

        private FlowLayoutPanel CreateInfoRow(string label, string val)
        {
            FlowLayoutPanel pnl = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Margin = new Padding(2),
                BackColor = Color.Transparent
            };
            Label l = new Label { Text = label, Font = UITheme.FontSmall, ForeColor = UITheme.TextMuted, AutoSize = true, Margin = new Padding(0, 0, 0, 1), UseMnemonic = false };
            Label v = new Label { Text = val, Font = UITheme.FontBold, ForeColor = UITheme.TextDark, AutoSize = true, Margin = new Padding(0), UseMnemonic = false };
            if (val.Contains("Đang hoạt động")) v.ForeColor = UITheme.Success;
            pnl.Controls.Add(l);
            pnl.Controls.Add(v);
            return pnl;
        }

        private RoundedPanel CreateHistoryCard(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding((int)(16 * scale), (int)(10 * scale), (int)(16 * scale), (int)(10 * scale))
            };

            Label lblTitle = new Label
            {
                Text = "📜 LỊCH SỬ ĐĂNG KÝ & GIAO DỊCH GÓI TẬP",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = (int)(28 * scale),
                UseMnemonic = false
            };

            DataGridView dgv = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgv);
            dgv.RowTemplate.Height = (int)Math.Round(36f * scale);
            dgv.ColumnHeadersHeight = (int)Math.Round(34f * scale);

            dgv.Columns.Add(UITheme.CreateFillColumn("ID", "MÃ HĐ", 14f, 85));
            dgv.Columns.Add(UITheme.CreateFillColumn("Package", "Gói tập", 30f, 130));
            dgv.Columns.Add(UITheme.CreateFillColumn("Date", "Ngày kích hoạt", 16f, 100));
            dgv.Columns.Add(UITheme.CreateFillColumn("End", "Ngày kết thúc", 16f, 100));
            dgv.Columns.Add(UITheme.CreateFillColumn("Price", "Giá tiền", 12f, 85));
            dgv.Columns.Add(UITheme.CreateFillColumn("Status", "Tình trạng", 12f, 85));

            try
            {
                var history = MemberService.Instance.GetMembershipHistory();
                if (history.Count > 0)
                {
                    foreach (var m in history)
                    {
                        string hdCode = $"#HD-{m.MembershipID:D4}";
                        string pName = m.Package?.PackageName ?? "Gói tập";
                        string sDate = m.StartDate.ToString("dd/MM/yyyy");
                        string eDate = m.EndDate.ToString("dd/MM/yyyy");
                        string pPrice = $"{m.Price:N0} đ";
                        string statusStr = m.Status == "Active" ? "Đã thanh toán (Active)" : (m.Status == "Completed" ? "Đã hoàn thành" : (m.Status == "Pending" ? "Chờ duyệt" : m.Status));
                        dgv.Rows.Add(hdCode, pName, sDate, eDate, pPrice, statusStr);
                    }
                }
                else
                {
                    dgv.Rows.Add("#--", "Chưa có giao dịch gói tập", "--", "--", "--", "--");
                }
            }
            catch
            {
                dgv.Rows.Add("#--", "Lỗi tải lịch sử gói tập từ CSDL", "--", "--", "--", "--");
            }

            dgv.CellFormatting += (s, e) =>
            {
                if (dgv.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
                {
                    string status = e.Value.ToString() ?? "";
                    if (status.Contains("thanh toán") || status.Contains("hoàn thành"))
                    {
                        e.CellStyle.ForeColor = UITheme.Success;
                        e.CellStyle.Font = UITheme.FontBold;
                    }
                }
            };

            card.Controls.Add(dgv);
            card.Controls.Add(lblTitle);

            return card;
        }

        private RoundedPanel CreatePrivilegesCard(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding((int)(16 * scale), (int)(10 * scale), (int)(16 * scale), (int)(10 * scale)),
                AutoScroll = true
            };

            var activeMembership = MemberService.Instance.GetActiveMembership();
            var pkg = activeMembership?.Package;
            bool isClassic = MemberService.Instance.IsClassicMember();
            string pkgName = pkg?.PackageName?.Trim() ?? (isClassic ? "Classic" : "VIP Diamond");
            string titleText = $"⭐ ĐẶC QUYỀN HỘI VIÊN GÓI {pkgName.ToUpper()}";

            Label lblTitle = new Label
            {
                Text = titleText,
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = (int)(28 * scale),
                UseMnemonic = false
            };

            // Phân tích danh sách đặc quyền trực tiếp từ cột Benefits trong CSDL
            List<string> benefitsList = new List<string>();
            if (!string.IsNullOrWhiteSpace(pkg?.Benefits))
            {
                var splitItems = pkg.Benefits.Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in splitItems)
                {
                    string clean = item.Trim();
                    if (string.IsNullOrEmpty(clean)) continue;
                    clean = clean.TrimStart('✓', '•', '-', '*', '★', ' ').Trim();
                    if (!string.IsNullOrEmpty(clean))
                    {
                        benefitsList.Add(clean);
                    }
                }
            }

            // Fallback nếu CSDL chưa có dữ liệu quyền lợi
            if (benefitsList.Count == 0)
            {
                if (isClassic)
                {
                    benefitsList.Add("Tập luyện không giới hạn khung giờ toàn hệ thống.");
                    benefitsList.Add("Sử dụng đầy đủ hệ thống máy tạ & Cardio tiêu chuẩn.");
                    benefitsList.Add("Trang bị Smart Locker cá nhân & phòng tắm nóng lạnh.");
                    benefitsList.Add("Đo & phân tích chỉ số InBody 1 lần/tháng miễn phí.");
                }
                else
                {
                    benefitsList.Add("Tập luyện không giới hạn khung giờ toàn hệ thống.");
                    benefitsList.Add("Huấn luyện 1-1 chuyên sâu cùng Huấn luyện viên thể hình.");
                    benefitsList.Add("Tư vấn & thiết lập lộ trình dinh dưỡng cá nhân hóa.");
                    benefitsList.Add("Sử dụng phòng xông hơi Sauna & Smart Locker VIP.");
                }
            }

            int rowCount = Math.Max(1, (int)Math.Ceiling(benefitsList.Count / 2.0));
            int rowHeight = (int)(36 * scale);

            TableLayoutPanel grid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = rowCount,
                Padding = new Padding(0, (int)(8 * scale), 0, 0)
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            grid.RowStyles.Clear();
            for (int r = 0; r < rowCount; r++)
            {
                grid.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
            }

            for (int i = 0; i < benefitsList.Count; i++)
            {
                int col = i % 2;
                int row = i / 2;
                bool isSpecial = benefitsList[i].StartsWith("⚡") || benefitsList[i].Contains("Ưu đãi");
                grid.Controls.Add(CreatePrivilegeItem(benefitsList[i], scale, isSpecial), col, row);
            }

            // Đảm bảo lblTitle ở trên cùng, sau đó đến grid
            card.Controls.Add(grid);
            card.Controls.Add(lblTitle);

            return card;
        }

        private Label CreatePrivilegeItem(string text, float scale, bool isHighlight = false)
        {
            return new Label
            {
                Text = text.StartsWith("★") ? text : "★  " + text,
                Font = isHighlight ? new Font("Segoe UI", 9.5f, FontStyle.Bold) : UITheme.FontRegular,
                ForeColor = isHighlight ? Color.FromArgb(124, 58, 237) : UITheme.TextDark,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding((int)(4 * scale), 0, (int)(4 * scale), 0),
                UseMnemonic = false,
                AutoEllipsis = true
            };
        }
    }
}
