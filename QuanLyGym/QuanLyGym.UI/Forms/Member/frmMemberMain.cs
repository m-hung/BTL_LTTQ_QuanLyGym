using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QuanLyGym.UI.Forms.Member.Views;
using QuanLyGym.UI.Helpers;
using QuanLyGym.UI.Utils;

namespace QuanLyGym.UI.Forms.Member
{
    public partial class frmMemberMain : Form
    {
        private Panel pnlSidebar = null!;
        private Panel pnlMainArea = null!;
        private Panel pnlHeader = null!;
        private Panel pnlContentContainer = null!;
        private Label lblHeaderTitle = null!;
        private Label lblHeaderSubtitle = null!;
        private Label lblHeaderClock = null!;
        private System.Windows.Forms.Timer? timerHeaderClock;

        // Navigation Buttons
        private Button btnNavDashboard = null!;
        private Button btnNavMembership = null!;
        private Button btnNavSchedule = null!;
        private Button btnNavCheckIn = null!;
        private Button btnNavProgress = null!;
        private Button btnNavProfile = null!;
        private List<Button> navButtons = new List<Button>();

        // Cached Views
        private readonly Dictionary<string, UserControl> viewCache = new Dictionary<string, UserControl>();
        private string currentViewKey = "";

        public frmMemberMain()
        {
            if (!QuanLyGym.UI.Services.MemberService.Instance.LoadCurrentMemberByAccount(UserSession.AccountID))
            {
                // Fallback demo member if needed
            }

            InitializeComponent();

            DisplayHelper.AutoConfigureForm(this);

            SetupModernDashboardLayout();
            NavigateTo("Dashboard");
        }

        private void SetupModernDashboardLayout()
        {
            this.SuspendLayout();
            this.BackColor = UITheme.Background;
            this.Font = UITheme.FontRegular;

            float dpiScale = Math.Max(1.0f, (float)this.DeviceDpi / 96f);

            // 1. Sidebar (Dock Left)
            pnlSidebar = CreateSidebar(dpiScale);
            pnlSidebar.Dock = DockStyle.Left;

            // 2. Main Area (Dock Fill) bao bọc Header và Content
            pnlMainArea = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UITheme.Background
            };

            // 2.1. Header Bar (Dock Top trong pnlMainArea)
            pnlHeader = CreateHeaderBar(dpiScale);
            pnlHeader.Dock = DockStyle.Top;

            // 2.2. Content Container (Dock Fill trong pnlMainArea)
            pnlContentContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UITheme.Background,
                AutoScroll = false
            };

            pnlMainArea.Controls.Clear();
            pnlMainArea.Controls.Add(pnlContentContainer);
            pnlMainArea.Controls.Add(pnlHeader);

            this.Controls.Clear();
            this.Controls.Add(pnlMainArea);
            this.Controls.Add(pnlSidebar);

            this.ResumeLayout(true);

            // Đồng hồ thời gian thực
            timerHeaderClock = new System.Windows.Forms.Timer { Interval = 1000 };
            timerHeaderClock.Tick += (s, e) =>
            {
                if (lblHeaderClock != null && !lblHeaderClock.IsDisposed)
                {
                    lblHeaderClock.Text = "🕒 " + DateTime.Now.ToString("HH:mm:ss  |  dd/MM/yyyy");
                }
            };
            timerHeaderClock.Start();
        }

        private Panel CreateSidebar(float dpiScale)
        {
            int sidebarWidth = Math.Max(260, (int)Math.Round(260f * dpiScale));
            Panel sidebar = new Panel
            {
                Width = sidebarWidth,
                Dock = DockStyle.Left,
                BackColor = UITheme.SidebarBg,
                Padding = new Padding((int)(12 * dpiScale), (int)(14 * dpiScale), (int)(12 * dpiScale), (int)(14 * dpiScale))
            };

            // 1. Brand Logo
            int logoHeight = (int)Math.Round(64f * dpiScale);
            Panel pnlLogo = new Panel { Dock = DockStyle.Top, Height = logoHeight };
            Label lblLogoText = new Label
            {
                Text = "⚡ TITAN GYM",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point((int)(6 * dpiScale), (int)(6 * dpiScale)),
                AutoSize = true,
                UseMnemonic = false
            };
            Label lblSlogan = new Label
            {
                Text = "MEMBER PORTAL DASHBOARD",
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = UITheme.Primary,
                Location = new Point((int)(8 * dpiScale), (int)(36 * dpiScale)),
                AutoSize = true,
                UseMnemonic = false
            };
            pnlLogo.Controls.Add(lblLogoText);
            pnlLogo.Controls.Add(lblSlogan);

            // 2. Member Mini Profile Card
            int profileHeight = (int)Math.Round(76f * dpiScale);
            Panel pnlProfile = new Panel
            {
                Dock = DockStyle.Top,
                Height = profileHeight,
                BackColor = Color.FromArgb(32, 38, 52),
                Padding = new Padding((int)(8 * dpiScale)),
                Margin = new Padding(0, (int)(6 * dpiScale), 0, (int)(10 * dpiScale))
            };

            int avatarSize = (int)Math.Round(42f * dpiScale);
            Panel pnlAvatar = new Panel
            {
                Size = new Size(avatarSize, avatarSize),
                BackColor = UITheme.Primary,
                Location = new Point((int)(8 * dpiScale), (int)(16 * dpiScale))
            };
            var curMember = QuanLyGym.UI.Services.MemberService.Instance.GetCurrentMember();
            string displayName = curMember?.FullName ?? (string.IsNullOrEmpty(UserSession.DisplayName) ? "Hội viên Titan" : UserSession.DisplayName);
            string firstLetter = displayName.Length > 0 ? displayName.Substring(0, 1).ToUpper() : "H";
            Label lblAvatarTxt = new Label
            {
                Text = firstLetter,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                UseMnemonic = false
            };
            pnlAvatar.Controls.Add(lblAvatarTxt);

            Label lblUserName = new Label
            {
                Text = displayName,
                Font = UITheme.FontBold,
                ForeColor = Color.White,
                Location = new Point(avatarSize + (int)(14 * dpiScale), (int)(14 * dpiScale)),
                AutoSize = true,
                MaximumSize = new Size(sidebarWidth - avatarSize - (int)(30 * dpiScale), (int)(24 * dpiScale)),
                AutoEllipsis = true,
                UseMnemonic = false
            };

            bool isClassic = QuanLyGym.UI.Services.MemberService.Instance.IsClassicMember();
            var activeMembership = QuanLyGym.UI.Services.MemberService.Instance.GetActiveMembership();
            string pkgName = activeMembership?.Package?.PackageName ?? (isClassic ? "Classic" : "VIP Diamond");

            Label lblBadge = new Label
            {
                Text = isClassic ? "● HỘI VIÊN CLASSIC" : $"● {pkgName.ToUpper()}",
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = isClassic ? Color.FromArgb(56, 189, 248) : Color.FromArgb(52, 211, 153),
                Location = new Point(avatarSize + (int)(14 * dpiScale), (int)(38 * dpiScale)),
                AutoSize = true,
                UseMnemonic = false
            };

            pnlProfile.Controls.Add(pnlAvatar);
            pnlProfile.Controls.Add(lblUserName);
            pnlProfile.Controls.Add(lblBadge);

            // Divider
            Panel pnlDiv = new Panel { Dock = DockStyle.Top, Height = (int)(10 * dpiScale) };

            // 3. Navigation Buttons Menu
            Panel pnlMenu = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            btnNavDashboard = CreateNavButton("🏠  Tổng quan", "Dashboard", dpiScale);
            btnNavMembership = CreateNavButton("💳  Gói tập & Thẻ VIP", "Membership", dpiScale);
            btnNavSchedule = CreateNavButton("📅  Lịch tập PT", "Schedule", dpiScale);
            btnNavCheckIn = CreateNavButton("📸  Điểm danh", "CheckIn", dpiScale);
            btnNavProgress = CreateNavButton("📊  Chỉ số InBody", "Progress", dpiScale);
            btnNavProfile = CreateNavButton("👤  Hồ sơ cá nhân", "Profile", dpiScale);

            Button? btnNavUpgrade = null;
            if (isClassic)
            {
                btnNavUpgrade = CreateNavButton("⚡  Nâng cấp gói tập", "Upgrade", dpiScale);
                btnNavUpgrade.ForeColor = Color.FromArgb(245, 158, 11);
            }

            navButtons.Add(btnNavDashboard);
            navButtons.Add(btnNavMembership);
            if (!isClassic)
            {
                navButtons.Add(btnNavSchedule);
            }
            if (btnNavUpgrade != null)
            {
                navButtons.Add(btnNavUpgrade);
            }
            navButtons.Add(btnNavCheckIn);
            navButtons.Add(btnNavProgress);
            navButtons.Add(btnNavProfile);

            // Dock Top stack (thứ tự ngược để Dashboard nằm trên cùng)
            pnlMenu.Controls.Add(btnNavProfile);
            pnlMenu.Controls.Add(btnNavProgress);
            pnlMenu.Controls.Add(btnNavCheckIn);
            if (btnNavUpgrade != null)
            {
                pnlMenu.Controls.Add(btnNavUpgrade);
            }
            if (!isClassic)
            {
                pnlMenu.Controls.Add(btnNavSchedule);
            }
            pnlMenu.Controls.Add(btnNavMembership);
            pnlMenu.Controls.Add(btnNavDashboard);

            // 4. Bottom Logout Button
            int bottomHeight = (int)Math.Round(76f * dpiScale);
            Panel pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = bottomHeight };
            int logoutHeight = (int)Math.Round(44f * dpiScale);
            Button btnLogoutNav = new Button
            {
                Text = "🚪  Đăng xuất tài khoản",
                Dock = DockStyle.Top,
                Height = logoutHeight,
                Font = UITheme.FontBold,
                ForeColor = Color.FromArgb(248, 113, 113),
                BackColor = Color.FromArgb(38, 28, 36),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding((int)(12 * dpiScale), 0, (int)(8 * dpiScale), 0),
                UseMnemonic = false
            };
            btnLogoutNav.FlatAppearance.BorderSize = 0;
            btnLogoutNav.Click += (s, e) =>
            {
                UserSession.Logout(this);
            };

            Label lblVer = new Label
            {
                Text = "Titan Gym Member Portal v2.6 Pro",
                Font = new Font("Segoe UI", 7.2f),
                ForeColor = Color.FromArgb(100, 116, 139),
                Dock = DockStyle.Bottom,
                Height = (int)(22 * dpiScale),
                TextAlign = ContentAlignment.MiddleCenter,
                UseMnemonic = false
            };
            pnlBottom.Controls.Add(btnLogoutNav);
            pnlBottom.Controls.Add(lblVer);

            sidebar.Controls.Add(pnlMenu);
            sidebar.Controls.Add(pnlBottom);
            sidebar.Controls.Add(pnlDiv);
            sidebar.Controls.Add(pnlProfile);
            sidebar.Controls.Add(pnlLogo);

            return sidebar;
        }

        private Button CreateNavButton(string text, string viewKey, float dpiScale)
        {
            int btnHeight = (int)Math.Round(46f * dpiScale);
            Button btn = new Button
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = btnHeight,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = UITheme.SidebarText,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding((int)(12 * dpiScale), 0, (int)(8 * dpiScale), 0),
                Tag = viewKey,
                Margin = new Padding(0, (int)(2 * dpiScale), 0, (int)(2 * dpiScale)),
                UseMnemonic = false
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = UITheme.SidebarHover;

            btn.Click += (s, e) => NavigateTo(viewKey);
            return btn;
        }

        private Panel CreateHeaderBar(float dpiScale)
        {
            int headerHeight = (int)Math.Round(72f * dpiScale);
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = headerHeight,
                BackColor = Color.White,
                Padding = new Padding(0)
            };

            // Border bottom
            Panel borderBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = UITheme.Border
            };
            header.Controls.Add(borderBottom);

            // FlowLayoutPanel chứa tiêu đề
            FlowLayoutPanel pnlTitleFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                Padding = new Padding((int)(20 * dpiScale), (int)(12 * dpiScale), 0, 0),
                BackColor = Color.Transparent
            };

            lblHeaderTitle = new Label
            {
                Text = "TỔNG QUAN HỘI VIÊN (DASHBOARD)",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, (int)(3 * dpiScale)),
                UseMnemonic = false
            };
            lblHeaderSubtitle = new Label
            {
                Text = "Theo dõi gói tập, lịch tập hôm nay, điểm danh và tiến độ thể hình",
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextMuted,
                AutoSize = true,
                Margin = new Padding(0),
                UseMnemonic = false
            };
            pnlTitleFlow.Controls.Add(lblHeaderTitle);
            pnlTitleFlow.Controls.Add(lblHeaderSubtitle);
            header.Controls.Add(pnlTitleFlow);

            // FlowLayoutPanel bên phải
            FlowLayoutPanel pnlRight = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                WrapContents = false,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, (int)(15 * dpiScale), (int)(20 * dpiScale), 0),
                BackColor = Color.Transparent
            };

            var currentMember = QuanLyGym.UI.Services.MemberService.Instance.GetCurrentMember();
            string branchName = currentMember?.Branch?.BranchName ?? "Chi nhánh Titan Gym";
            Label lblBranch = new Label
            {
                Text = $"📍 {branchName}",
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.Primary,
                BackColor = UITheme.PrimaryLight,
                Padding = new Padding((int)(8 * dpiScale), (int)(5 * dpiScale), (int)(8 * dpiScale), (int)(5 * dpiScale)),
                AutoSize = true,
                Margin = new Padding((int)(4 * dpiScale), (int)(2 * dpiScale), (int)(8 * dpiScale), (int)(2 * dpiScale)),
                UseMnemonic = false
            };

            lblHeaderClock = new Label
            {
                Text = "🕒 " + DateTime.Now.ToString("HH:mm:ss  |  dd/MM/yyyy"),
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.TextRegular,
                AutoSize = true,
                Margin = new Padding((int)(4 * dpiScale), (int)(6 * dpiScale), (int)(10 * dpiScale), (int)(2 * dpiScale)),
                UseMnemonic = false
            };

            Button btnBell = new Button
            {
                Text = "🔔 (2)",
                Font = UITheme.FontSmallBold,
                BackColor = Color.FromArgb(254, 243, 199),
                ForeColor = Color.FromArgb(180, 83, 9),
                FlatStyle = FlatStyle.Flat,
                Size = new Size((int)(68 * dpiScale), (int)(32 * dpiScale)),
                Cursor = Cursors.Hand,
                Margin = new Padding((int)(3 * dpiScale), (int)(2 * dpiScale), (int)(4 * dpiScale), (int)(2 * dpiScale)),
                UseMnemonic = false
            };
            btnBell.FlatAppearance.BorderSize = 0;
            btnBell.Click += (s, e) => MessageBox.Show("1. Nhắc nhở: Bạn có lịch tập lúc 18:00 chiều nay với HLV Nam.\n2. Thông báo: Phòng xông hơi đã hoàn thành bảo trì sẵn sàng đón khách.", "Thông báo của bạn", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Button btnRefresh = new Button
            {
                Text = "🔄 Làm mới",
                Font = UITheme.FontSmallBold,
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = UITheme.TextDark,
                FlatStyle = FlatStyle.Flat,
                Size = new Size((int)(96 * dpiScale), (int)(32 * dpiScale)),
                Cursor = Cursors.Hand,
                Margin = new Padding((int)(3 * dpiScale), (int)(2 * dpiScale), (int)(3 * dpiScale), (int)(2 * dpiScale)),
                UseMnemonic = false
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) =>
            {
                MessageBox.Show("Đã làm mới dữ liệu hệ thống thành công!", "Làm mới", MessageBoxButtons.OK, MessageBoxIcon.Information);
                NavigateTo(currentViewKey, true);
            };

            pnlRight.Controls.Add(lblBranch);
            pnlRight.Controls.Add(lblHeaderClock);
            pnlRight.Controls.Add(btnBell);
            pnlRight.Controls.Add(btnRefresh);

            header.Controls.Add(pnlRight);
            return header;
        }

        public void NavigateTo(string viewKey, bool forceReload = false)
        {
            if (currentViewKey == viewKey && !forceReload) return;
            currentViewKey = viewKey;

            foreach (var btn in navButtons)
            {
                bool isActive = (btn.Tag?.ToString() == viewKey);
                btn.BackColor = isActive ? UITheme.SidebarActive : Color.Transparent;
                if (isActive)
                {
                    btn.ForeColor = UITheme.SidebarTextActive;
                }
                else
                {
                    btn.ForeColor = (btn.Tag?.ToString() == "Upgrade")
                        ? Color.FromArgb(245, 158, 11)
                        : UITheme.SidebarText;
                }
            }

            switch (viewKey)
            {
                case "Dashboard":
                    lblHeaderTitle.Text = "TỔNG QUAN HỘI VIÊN (DASHBOARD)";
                    lblHeaderSubtitle.Text = "Theo dõi gói tập, lịch tập hôm nay, điểm danh và tiến độ thể hình";
                    break;
                case "Membership":
                    lblHeaderTitle.Text = "GÓI TẬP & THẺ HỘI VIÊN";
                    lblHeaderSubtitle.Text = "Thông tin thẻ hội viên điện tử, thời hạn và danh sách đặc quyền gói tập";
                    break;
                case "Upgrade":
                    lblHeaderTitle.Text = "NÂNG CẤP GÓI TẬP KÈM HUẤN LUYỆN VIÊN (PT 1-1)";
                    lblHeaderSubtitle.Text = "Chuyển đổi từ gói Classic lên các gói tập có HLV cá nhân để đạt hiệu quả tối đa";
                    break;
                case "Schedule":
                    lblHeaderTitle.Text = "LỊCH TẬP & ĐẶT LỊCH HUẤN LUYỆN VIÊN (PT)";
                    lblHeaderSubtitle.Text = "Xem lịch tập trong tuần, đăng ký ca tập mới và đặt lịch hẹn 1-1 với PT";
                    break;
                case "CheckIn":
                    lblHeaderTitle.Text = "ĐIỂM DANH CHECK-IN & TẢI ẢNH MINH CHỨNG";
                    lblHeaderSubtitle.Text = "Điểm danh buổi tập theo thời gian thực và gửi ảnh cho Huấn luyện viên phê duyệt";
                    break;
                case "Progress":
                    lblHeaderTitle.Text = "THEO DÕI CHỈ SỐ INBODY & THỂ HÌNH";
                    lblHeaderSubtitle.Text = "Ghi nhận cân nặng, tỷ lệ mỡ, khối lượng cơ và so sánh tiến trình vóc dáng";
                    break;
                case "Profile":
                    lblHeaderTitle.Text = "HỒ SƠ CÁ NHÂN & BẢO MẬT TÀI KHOẢN";
                    lblHeaderSubtitle.Text = "Cập nhật thông tin liên hệ, chi nhánh tập và thay đổi mật khẩu";
                    break;
            }

            UserControl view;
            if (forceReload && viewCache.TryGetValue(viewKey, out var oldView))
            {
                pnlContentContainer.Controls.Remove(oldView);
                oldView.Dispose();
                viewCache.Remove(viewKey);
            }

            if (!viewCache.TryGetValue(viewKey, out view!))
            {
                view = viewKey switch
                {
                    "Dashboard" => new ucMemberDashboard(key => NavigateTo(key)),
                    "Membership" => new ucMemberMembership(key => NavigateTo(key)),
                    "Upgrade" => new ucMemberUpgradePackage(key => NavigateTo(key)),
                    "Schedule" => new ucMemberSchedule(),
                    "CheckIn" => new ucMemberCheckIn(),
                    "Progress" => new ucMemberProgress(),
                    "Profile" => new ucMemberProfile(),
                    _ => new ucMemberDashboard(key => NavigateTo(key))
                };
                view.Dock = DockStyle.Fill;
                viewCache[viewKey] = view;
                pnlContentContainer.Controls.Add(view);
            }

            pnlContentContainer.SuspendLayout();
            foreach (Control c in pnlContentContainer.Controls)
            {
                c.Visible = (c == view);
            }
            view.BringToFront();
            view.AutoScrollPosition = new Point(0, 0);
            pnlContentContainer.ResumeLayout(true);
            view.PerformLayout();
            view.Invalidate(true);
        }
    }
}
