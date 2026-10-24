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
            InitializeComponent();
            SetupModernDashboardLayout();
            NavigateTo("Dashboard");
        }

        private void SetupModernDashboardLayout()
        {
            this.SuspendLayout();
            this.BackColor = UITheme.Background;
            this.Font = UITheme.FontRegular;

            // 1. Sidebar (Dock Left)
            pnlSidebar = CreateSidebar();
            this.Controls.Add(pnlSidebar);

            // 2. Top Header Bar (Dock Top)
            // pnlHeader được tạo và gán từ hàm CreateHeaderBar
            pnlHeader = CreateHeaderBar();
            this.Controls.Add(pnlHeader);

            // 3. Main Content Container (Dock Fill)
            pnlContentContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UITheme.Background,
                AutoScroll = true
            };
            this.Controls.Add(pnlContentContainer);

            // Bring header and sidebar to front
            pnlHeader.BringToFront();
            pnlSidebar.BringToFront();
            pnlContentContainer.BringToFront();

            this.ResumeLayout(false);

            // Start live clock for header
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

        private Panel CreateSidebar()
        {
            Panel sidebar = new Panel
            {
                Width = 260,
                Dock = DockStyle.Left,
                BackColor = UITheme.SidebarBg,
                Padding = new Padding(14, 18, 14, 18)
            };

            // 1. Brand Logo
            Panel pnlLogo = new Panel { Dock = DockStyle.Top, Height = 60 };
            Label lblLogoText = new Label
            {
                Text = "⚡ TITAN GYM",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(8, 6),
                AutoSize = true
            };
            Label lblSlogan = new Label
            {
                Text = "MEMBER PORTAL DASHBOARD",
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = UITheme.Primary,
                Location = new Point(10, 36),
                AutoSize = true
            };
            pnlLogo.Controls.Add(lblLogoText);
            pnlLogo.Controls.Add(lblSlogan);

            // 2. Member Mini Profile Card
            Panel pnlProfile = new Panel
            {
                Dock = DockStyle.Top,
                Height = 84,
                BackColor = Color.FromArgb(32, 38, 52),
                Padding = new Padding(12),
                Margin = new Padding(0, 10, 0, 16)
            };

            Panel pnlAvatar = new Panel
            {
                Size = new Size(46, 46),
                BackColor = UITheme.Primary,
                Location = new Point(10, 18)
            };
            Label lblAvatarTxt = new Label
            {
                Text = "H",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlAvatar.Controls.Add(lblAvatarTxt);

            Label lblUserName = new Label
            {
                Text = string.IsNullOrEmpty(UserSession.DisplayName) ? "Trần Văn Hùng" : UserSession.DisplayName,
                Font = UITheme.FontBold,
                ForeColor = Color.White,
                Location = new Point(64, 18),
                AutoSize = true
            };

            Label lblBadge = new Label
            {
                Text = "● VIP DIAMOND",
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 211, 153), // Emerald
                Location = new Point(64, 40),
                AutoSize = true
            };

            pnlProfile.Controls.Add(pnlAvatar);
            pnlProfile.Controls.Add(lblUserName);
            pnlProfile.Controls.Add(lblBadge);

            // Divider
            Panel pnlDiv = new Panel { Dock = DockStyle.Top, Height = 14 };

            // 3. Navigation Buttons Menu
            Panel pnlMenu = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            btnNavDashboard = CreateNavButton("🏠  Tổng quan Dashboard", "Dashboard");
            btnNavMembership = CreateNavButton("💳  Gói tập & Thẻ VIP", "Membership");
            btnNavSchedule = CreateNavButton("📅  Lịch tập & Đặt lịch PT", "Schedule");
            btnNavCheckIn = CreateNavButton("📸  Điểm danh Check-in", "CheckIn");
            btnNavProgress = CreateNavButton("📊  Chỉ số InBody & Cơ thể", "Progress");
            btnNavProfile = CreateNavButton("👤  Hồ sơ & Bảo mật", "Profile");

            navButtons.Add(btnNavDashboard);
            navButtons.Add(btnNavMembership);
            navButtons.Add(btnNavSchedule);
            navButtons.Add(btnNavCheckIn);
            navButtons.Add(btnNavProgress);
            navButtons.Add(btnNavProfile);

            // Add in reverse for Dock Top
            pnlMenu.Controls.Add(btnNavProfile);
            pnlMenu.Controls.Add(btnNavProgress);
            pnlMenu.Controls.Add(btnNavCheckIn);
            pnlMenu.Controls.Add(btnNavSchedule);
            pnlMenu.Controls.Add(btnNavMembership);
            pnlMenu.Controls.Add(btnNavDashboard);

            // 4. Bottom Logout Button
            Panel pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 80 };
            Button btnLogoutNav = new Button
            {
                Text = "🚪  Đăng xuất tài khoản",
                Dock = DockStyle.Top,
                Height = 44,
                Font = UITheme.FontBold,
                ForeColor = Color.FromArgb(248, 113, 113), // Red 400
                BackColor = Color.FromArgb(38, 28, 36),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16, 0, 0, 0)
            };
            btnLogoutNav.FlatAppearance.BorderSize = 0;
            btnLogoutNav.Click += (s, e) =>
            {
                var confirm = MessageBox.Show("Bạn có muốn đăng xuất khỏi hệ thống?", "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    UserSession.Logout(this);
                }
            };

            Label lblVer = new Label
            {
                Text = "Titan Gym Member Portal v2.6 Pro",
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = Color.FromArgb(100, 116, 139),
                Dock = DockStyle.Bottom,
                Height = 24,
                TextAlign = ContentAlignment.MiddleCenter
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

        private Button CreateNavButton(string text, string viewKey)
        {
            Button btn = new Button
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 48,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = UITheme.SidebarText,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16, 0, 0, 0),
                Tag = viewKey,
                Margin = new Padding(0, 3, 0, 3)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = UITheme.SidebarHover;

            btn.Click += (s, e) => NavigateTo(viewKey);
            return btn;
        }

        private Panel CreateHeaderBar()
        {
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.White,
                Padding = new Padding(24, 12, 24, 12)
            };

            // Border line at bottom
            Panel borderBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = UITheme.Border
            };
            header.Controls.Add(borderBottom);

            // Title & Subtitle
            lblHeaderTitle = new Label
            {
                Text = "TỔNG QUAN HỘI VIÊN",
                Font = UITheme.FontSubtitle,
                ForeColor = UITheme.TextDark,
                Location = new Point(24, 14),
                AutoSize = true
            };
            lblHeaderSubtitle = new Label
            {
                Text = "Hệ thống quản lý thông tin, lịch tập & tiến trình hội viên",
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextMuted,
                Location = new Point(24, 40),
                AutoSize = true
            };
            header.Controls.Add(lblHeaderTitle);
            header.Controls.Add(lblHeaderSubtitle);

            // Right Info Items
            FlowLayoutPanel pnlRight = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 8, 0, 0)
            };

            // Branch Badge
            Label lblBranch = new Label
            {
                Text = "📍 Chi nhánh Cầu Giấy",
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.Primary,
                BackColor = UITheme.PrimaryLight,
                Padding = new Padding(10, 6, 10, 6),
                AutoSize = true,
                Margin = new Padding(6, 4, 10, 4)
            };

            // Digital Clock
            lblHeaderClock = new Label
            {
                Text = "🕒 " + DateTime.Now.ToString("HH:mm:ss  |  dd/MM/yyyy"),
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.TextRegular,
                AutoSize = true,
                Margin = new Padding(6, 8, 12, 4)
            };

            // Notification Bell
            Button btnBell = new Button
            {
                Text = "🔔 (2)",
                Font = UITheme.FontSmallBold,
                BackColor = Color.FromArgb(254, 243, 199),
                ForeColor = Color.FromArgb(180, 83, 9),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(65, 32),
                Cursor = Cursors.Hand,
                Margin = new Padding(4, 4, 6, 4)
            };
            btnBell.FlatAppearance.BorderSize = 0;
            btnBell.Click += (s, e) => MessageBox.Show("1. Nhắc nhở: Bạn có lịch tập lúc 18:00 chiều nay với HLV Nam.\n2. Thông báo: Phòng xông hơi đã hoàn thành bảo trì sẵn sàng đón khách.", "Thông báo của bạn", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Refresh Button
            Button btnRefresh = new Button
            {
                Text = "🔄 Làm mới",
                Font = UITheme.FontSmallBold,
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = UITheme.TextDark,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(88, 32),
                Cursor = Cursors.Hand,
                Margin = new Padding(4, 4, 4, 4)
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

            // Update Navigation Button styles
            foreach (var btn in navButtons)
            {
                bool isActive = (btn.Tag?.ToString() == viewKey);
                btn.BackColor = isActive ? UITheme.SidebarActive : Color.Transparent;
                btn.ForeColor = isActive ? UITheme.SidebarTextActive : UITheme.SidebarText;
            }

            // Update Header text
            switch (viewKey)
            {
                case "Dashboard":
                    lblHeaderTitle.Text = "TỔNG QUAN HỘI VIÊN (DASHBOARD)";
                    lblHeaderSubtitle.Text = "Theo dõi gói tập, lịch tập hôm nay, điểm danh và tiến độ thể hình";
                    break;
                case "Membership":
                    lblHeaderTitle.Text = "GÓI TẬP & THẺ HỘI VIÊN VIP";
                    lblHeaderSubtitle.Text = "Thông tin thẻ hội viên điện tử, thời hạn và danh sách đặc quyền gói tập";
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

            // Get or create view
            UserControl view;
            if (forceReload || !viewCache.TryGetValue(viewKey, out view!))
            {
                view = viewKey switch
                {
                    "Dashboard" => new ucMemberDashboard(key => NavigateTo(key)),
                    "Membership" => new ucMemberMembership(),
                    "Schedule" => new ucMemberSchedule(),
                    "CheckIn" => new ucMemberCheckIn(),
                    "Progress" => new ucMemberProgress(),
                    "Profile" => new ucMemberProfile(),
                    _ => new ucMemberDashboard(key => NavigateTo(key))
                };
                viewCache[viewKey] = view;
            }

            // Switch view
            pnlContentContainer.SuspendLayout();
            pnlContentContainer.Controls.Clear();
            view.Dock = DockStyle.Fill;
            pnlContentContainer.Controls.Add(view);
            pnlContentContainer.ResumeLayout(true);
        }
    }
}