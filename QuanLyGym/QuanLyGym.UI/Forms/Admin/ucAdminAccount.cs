using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public class ucAdminAccount : UserControl
    {
        // =====================================================================
        // MÀU SẮC CHỦ ĐẠO (DARK GYM THEME)
        // =====================================================================
        private static readonly Color HeaderBg = Color.FromArgb(10, 10, 12);
        private static readonly Color CardBg = Color.FromArgb(24, 24, 27);
        private static readonly Color ItemBg = Color.FromArgb(20, 20, 23);
        private static readonly Color Border = Color.FromArgb(45, 45, 50);
        private static readonly Color TextColor = Color.FromArgb(244, 244, 245);
        private static readonly Color MutedColor = Color.FromArgb(161, 161, 170);

        private static readonly Color Orange = Color.FromArgb(249, 115, 22);
        private static readonly Color Blue = Color.FromArgb(59, 130, 246);
        private static readonly Color Cyan = Color.FromArgb(6, 182, 212);
        private static readonly Color Green = Color.FromArgb(34, 197, 94);
        private static readonly Color Gold = Color.FromArgb(234, 179, 8);
        private static readonly Color Purple = Color.FromArgb(168, 85, 247);
        private static readonly Color Red = Color.FromArgb(239, 68, 68);

        // =====================================================================
        // DATA MODEL
        // =====================================================================
        public class AccountModel
        {
            public int Id { get; set; }
            public string AccountType { get; set; } = "Admin"; // "Admin", "Staff", "Member"
            public string Username { get; set; } = "";
            public string PasswordHash { get; set; } = "••••••••";
            public string FullName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public string SubRoleOrTitle { get; set; } = ""; // E.g. Super Admin, HLV Thể hình, etc.
            public string BranchName { get; set; } = "";
            public string Tier { get; set; } = ""; // Classic, Silver, Gold, Platinum, VIP Diamond (for Member)
            public string MemberCode { get; set; } = ""; // E.g. HV-101
            public bool IsActive { get; set; } = true;
            public DateTime CreatedAt { get; set; }
            public DateTime? LastLogin { get; set; }
        }

        private readonly List<AccountModel> _accounts = new();

        // 3 TABS: 0 = Admin, 1 = Staff, 2 = Member
        private int _selectedTab = 0;
        private bool _isInitializing = true;
        private Button _btnTabAdmin = null!;
        private Button _btnTabStaff = null!;
        private Button _btnTabMember = null!;

        // KPI Labels
        private Label _lblKpiTotalVal = null!;
        private Label _lblKpiAdminVal = null!;
        private Label _lblKpiStaffVal = null!;
        private Label _lblKpiMemberVal = null!;
        private Label _lblKpiLockedVal = null!;

        // Filter controls
        private TextBox _txtSearch = null!;
        private ComboBox _cboStatusFilter = null!;
        private ComboBox _cboBranchFilter = null!;
        private ComboBox _cboRoleOrTierFilter = null!;

        // DataGridView
        private DataGridView _grid = null!;

        public ucAdminAccount()
        {
            DoubleBuffered = true;
            InitializeSampleData();
            BuildUI();
            RefreshAll();
        }

        private void InitializeSampleData()
        {
            int nextId = 1;

            // 1. ADMIN ACCOUNTS (5 tài khoản)
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Admin",
                Username = "admin_master",
                FullName = "Nguyễn Quản Trị",
                Email = "quantri.master@gymfit.vn",
                Phone = "0901.888.999",
                SubRoleOrTitle = "Super Admin (Toàn quyền)",
                BranchName = "Tất cả chi nhánh",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 1),
                LastLogin = DateTime.Now.AddMinutes(-12)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Admin",
                Username = "admin_hcm",
                FullName = "Trần Minh Tuấn",
                Email = "tuan.tran@gymfit.vn",
                Phone = "0903.112.334",
                SubRoleOrTitle = "Quản lý Chi nhánh",
                BranchName = "GymFit Quận 1 - Bến Nghé",
                IsActive = true,
                CreatedAt = new DateTime(2025, 3, 15),
                LastLogin = DateTime.Now.AddHours(-2)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Admin",
                Username = "admin_hn",
                FullName = "Phạm Quốc Huy",
                Email = "huy.pham@gymfit.vn",
                Phone = "0982.556.778",
                SubRoleOrTitle = "Quản lý Chi nhánh",
                BranchName = "GymFit Hoàn Kiếm - Tràng Tiền",
                IsActive = true,
                CreatedAt = new DateTime(2025, 4, 10),
                LastLogin = DateTime.Now.AddHours(-5)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Admin",
                Username = "admin_finance",
                FullName = "Lê Thị Ngọc Bích",
                Email = "bich.le@gymfit.vn",
                Phone = "0934.778.899",
                SubRoleOrTitle = "Quản lý Tài chính & Kế toán",
                BranchName = "Tất cả chi nhánh",
                IsActive = true,
                CreatedAt = new DateTime(2025, 5, 20),
                LastLogin = DateTime.Now.AddDays(-1)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Admin",
                Username = "admin_audit",
                FullName = "Đỗ Hoàng Long",
                Email = "long.audit@gymfit.vn",
                Phone = "0918.445.667",
                SubRoleOrTitle = "Giám sát Vận hành & Thiết bị",
                BranchName = "Tất cả chi nhánh",
                IsActive = false, // Tạm khóa
                CreatedAt = new DateTime(2025, 6, 1),
                LastLogin = DateTime.Now.AddDays(-14)
            });

            // 2. STAFF / PT ACCOUNTS (11 tài khoản)
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Staff",
                Username = "pt_long",
                FullName = "Vũ Hoàng Long",
                Email = "long.vu@gymfit.vn",
                Phone = "0908.123.456",
                SubRoleOrTitle = "HLV Thể hình Cá nhân",
                BranchName = "GymFit Quận 1 - Bến Nghé",
                IsActive = true,
                CreatedAt = new DateTime(2025, 2, 10),
                LastLogin = DateTime.Now.AddMinutes(-35)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Staff",
                Username = "pt_thao",
                FullName = "Phan Thị Thảo",
                Email = "thao.phan@gymfit.vn",
                Phone = "0938.654.321",
                SubRoleOrTitle = "HLV Yoga & Pilates",
                BranchName = "GymFit Bình Thạnh - Landmark",
                IsActive = true,
                CreatedAt = new DateTime(2025, 3, 1),
                LastLogin = DateTime.Now.AddHours(-1)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Staff",
                Username = "pt_vinh",
                FullName = "Lê Công Vinh",
                Email = "vinh.le@gymfit.vn",
                Phone = "0977.889.900",
                SubRoleOrTitle = "HLV Boxing & Kickfit",
                BranchName = "GymFit Hoàn Kiếm - Tràng Tiền",
                IsActive = true,
                CreatedAt = new DateTime(2025, 3, 15),
                LastLogin = DateTime.Now.AddHours(-3)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Staff",
                Username = "pt_bich",
                FullName = "Trần Thị Bích",
                Email = "bich.tran@gymfit.vn",
                Phone = "0912.345.678",
                SubRoleOrTitle = "HLV Trưởng Chi Nhánh",
                BranchName = "GymFit Quận 1 - Bến Nghé",
                IsActive = true,
                CreatedAt = new DateTime(2025, 1, 15),
                LastLogin = DateTime.Now.AddHours(-4)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Staff",
                Username = "pt_tuan",
                FullName = "Hoàng Quốc Tuấn",
                Email = "tuan.hoang@gymfit.vn",
                Phone = "0944.556.677",
                SubRoleOrTitle = "HLV Thể hình Cá nhân",
                BranchName = "GymFit Cầu Giấy - Duy Tân",
                IsActive = true,
                CreatedAt = new DateTime(2025, 4, 1),
                LastLogin = DateTime.Now.AddDays(-1)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Staff",
                Username = "pt_mai",
                FullName = "Nguyễn Tuyết Mai",
                Email = "mai.nguyen@gymfit.vn",
                Phone = "0968.112.233",
                SubRoleOrTitle = "HLV Yoga & Giãn cơ",
                BranchName = "GymFit Quận 1 - Bến Nghé",
                IsActive = true,
                CreatedAt = new DateTime(2025, 5, 12),
                LastLogin = DateTime.Now.AddDays(-2)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Staff",
                Username = "rec_linh",
                FullName = "Đỗ Thùy Linh",
                Email = "linh.do@gymfit.vn",
                Phone = "0981.234.567",
                SubRoleOrTitle = "Nhân viên Lễ tân",
                BranchName = "GymFit Quận 1 - Bến Nghé",
                IsActive = true,
                CreatedAt = new DateTime(2025, 2, 20),
                LastLogin = DateTime.Now.AddMinutes(-10)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Staff",
                Username = "rec_chau",
                FullName = "Ngô Bảo Châu",
                Email = "chau.ngo@gymfit.vn",
                Phone = "0919.876.543",
                SubRoleOrTitle = "Nhân viên Lễ tân",
                BranchName = "GymFit Hoàn Kiếm - Tràng Tiền",
                IsActive = true,
                CreatedAt = new DateTime(2025, 3, 25),
                LastLogin = DateTime.Now.AddHours(-6)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Staff",
                Username = "cashier_nam",
                FullName = "Võ Phương Nam",
                Email = "nam.vo@gymfit.vn",
                Phone = "0933.445.566",
                SubRoleOrTitle = "Nhân viên Thu ngân & Bán hàng",
                BranchName = "GymFit Bình Thạnh - Landmark",
                IsActive = true,
                CreatedAt = new DateTime(2025, 4, 18),
                LastLogin = DateTime.Now.AddDays(-1)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Staff",
                Username = "tech_hien",
                FullName = "Trần Đức Hiển",
                Email = "hien.tran@gymfit.vn",
                Phone = "0972.334.455",
                SubRoleOrTitle = "Kỹ thuật viên Thiết bị",
                BranchName = "Tất cả chi nhánh",
                IsActive = true,
                CreatedAt = new DateTime(2025, 6, 1),
                LastLogin = DateTime.Now.AddDays(-3)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Staff",
                Username = "pt_khoa",
                FullName = "Bùi Đăng Khoa",
                Email = "khoa.bui@gymfit.vn",
                Phone = "0989.112.334",
                SubRoleOrTitle = "HLV Thể hình Cá nhân",
                BranchName = "GymFit Cầu Giấy - Duy Tân",
                IsActive = false, // Nghỉ việc / Đang khóa
                CreatedAt = new DateTime(2025, 1, 10),
                LastLogin = DateTime.Now.AddDays(-45)
            });

            // 3. MEMBER ACCOUNTS (11 tài khoản)
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Member",
                Username = "member_nam",
                FullName = "Đặng Hoàng Nam",
                MemberCode = "HV-101",
                Tier = "VIP Diamond",
                BranchName = "GymFit Quận 1 - Bến Nghé",
                Email = "nam.dang@investcorp.vn",
                Phone = "0912.888.999",
                IsActive = true,
                CreatedAt = new DateTime(2025, 6, 19),
                LastLogin = DateTime.Now.AddHours(-1)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Member",
                Username = "member_ha",
                FullName = "Lê Thị Thu Hà",
                MemberCode = "HV-102",
                Tier = "Gold",
                BranchName = "GymFit Bình Thạnh - Landmark",
                Email = "thuha.le@techcombank.com.vn",
                Phone = "0987.654.321",
                IsActive = true,
                CreatedAt = new DateTime(2025, 4, 15),
                LastLogin = DateTime.Now.AddHours(-8)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Member",
                Username = "member_duc",
                FullName = "Trần Minh Đức",
                MemberCode = "HV-103",
                Tier = "Platinum",
                BranchName = "GymFit Hoàn Kiếm - Tràng Tiền",
                Email = "minhduc.tran@vinfast.vn",
                Phone = "0909.112.233",
                IsActive = true,
                CreatedAt = new DateTime(2025, 5, 20),
                LastLogin = DateTime.Now.AddDays(-1)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Member",
                Username = "member_vy",
                FullName = "Võ Thảo Vy",
                MemberCode = "HV-104",
                Tier = "Silver",
                BranchName = "GymFit Cầu Giấy - Duy Tân",
                Email = "thaovy.vo@shopee.vn",
                Phone = "0932.445.566",
                IsActive = true,
                CreatedAt = new DateTime(2025, 7, 1),
                LastLogin = DateTime.Now.AddDays(-2)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Member",
                Username = "member_dung",
                FullName = "Phạm Tuấn Dũng",
                MemberCode = "HV-105",
                Tier = "Classic",
                BranchName = "GymFit Quận 1 - Bến Nghé",
                Email = "tuandung.pham@fpt.com.vn",
                Phone = "0971.223.344",
                IsActive = true,
                CreatedAt = new DateTime(2025, 8, 10),
                LastLogin = DateTime.Now.AddDays(-3)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Member",
                Username = "member_huong",
                FullName = "Nguyễn Lan Hương",
                MemberCode = "HV-106",
                Tier = "VIP Diamond",
                BranchName = "GymFit Hoàn Kiếm - Tràng Tiền",
                Email = "lanhuong.nguyen@vietjetair.com",
                Phone = "0915.778.899",
                IsActive = true,
                CreatedAt = new DateTime(2025, 6, 25),
                LastLogin = DateTime.Now.AddHours(-15)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Member",
                Username = "member_thanh",
                FullName = "Bùi Văn Thành",
                MemberCode = "HV-110",
                Tier = "Gold",
                BranchName = "GymFit Bình Thạnh - Landmark",
                Email = "thanhbui.sales@vingroup.net",
                Phone = "0977.443.221",
                IsActive = true,
                CreatedAt = new DateTime(2025, 5, 2),
                LastLogin = DateTime.Now.AddHours(-20)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Member",
                Username = "member_linh",
                FullName = "Hoàng Thuỳ Linh",
                MemberCode = "HV-109",
                Tier = "Platinum",
                BranchName = "GymFit Quận 1 - Bến Nghé",
                Email = "thuylinh.hoang@vietnamairlines.com",
                Phone = "0938.999.444",
                IsActive = true,
                CreatedAt = new DateTime(2025, 8, 18),
                LastLogin = DateTime.Now.AddDays(-2)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Member",
                Username = "member_mai",
                FullName = "Phan Quỳnh Mai",
                MemberCode = "HV-113",
                Tier = "Silver",
                BranchName = "GymFit Hoàn Kiếm - Tràng Tiền",
                Email = "quynhmai.phan@hust.edu.vn",
                Phone = "0946.332.112",
                IsActive = true,
                CreatedAt = new DateTime(2025, 9, 5),
                LastLogin = DateTime.Now.AddDays(-4)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Member",
                Username = "member_quang",
                FullName = "Đinh Quang Sáng",
                MemberCode = "HV-114",
                Tier = "Classic",
                BranchName = "GymFit Cầu Giấy - Duy Tân",
                Email = "quangsang.dinh@gmail.com",
                Phone = "0904.556.778",
                IsActive = true,
                CreatedAt = new DateTime(2025, 9, 20),
                LastLogin = DateTime.Now.AddDays(-1)
            });
            _accounts.Add(new AccountModel
            {
                Id = nextId++,
                AccountType = "Member",
                Username = "member_dang",
                FullName = "Trịnh Hải Đăng",
                MemberCode = "HV-112",
                Tier = "Gold",
                BranchName = "GymFit Quận 1 - Bến Nghé",
                Email = "haidang.trinh@shipping.vn",
                Phone = "0915.667.889",
                IsActive = false, // Tạm bảo lưu / Khóa
                CreatedAt = new DateTime(2025, 5, 10),
                LastLogin = DateTime.Now.AddDays(-28)
            });
        }

        private void BuildUI()
        {
            Dock = DockStyle.Fill;
            BackColor = HeaderBg;
            Padding = new Padding(12, 10, 12, 10);

            // A. Top Header
            var pnlHeader = BuildTopHeaderSection();
            var pnlGap1 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // B. KPI metrics strip
            var pnlKpis = BuildKpiStrip();
            var pnlGap2 = new Panel { Dock = DockStyle.Top, Height = 12, BackColor = Color.Transparent };

            // C. 3-Tab Segmented Switcher (Admin / Staff / Member)
            var pnlTabs = BuildRoleTabsSection();
            var pnlGap3 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // D. Toolbar Bộ lọc và Tìm kiếm
            var pnlToolbar = BuildFilterToolbar();
            var pnlGap4 = new Panel { Dock = DockStyle.Top, Height = 8, BackColor = Color.Transparent };

            // E. Lưới dữ liệu chính (DataGridView)
            var pnlGridContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Padding = new Padding(12, 12, 12, 12)
            };
            pnlGridContainer.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlGridContainer.Width - 1, pnlGridContainer.Height - 1);
            };

            _grid = BuildMainGridView();
            pnlGridContainer.Controls.Add(_grid);

            // Add Controls Dock order
            Controls.Add(pnlGridContainer);
            Controls.Add(pnlGap4);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlGap3);
            Controls.Add(pnlTabs);
            Controls.Add(pnlGap2);
            Controls.Add(pnlKpis);
            Controls.Add(pnlGap1);
            Controls.Add(pnlHeader);
        }

        // =====================================================================
        // A. TOP HEADER SECTION
        // =====================================================================
        private Panel BuildTopHeaderSection()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 62,
                BackColor = CardBg,
                Padding = new Padding(16, 10, 16, 10)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            var lblTitle = new Label
            {
                Text = "QUẢN LÝ TÀI KHOẢN & PHÂN QUYỀN HỆ THỐNG",
                Font = new Font("Segoe UI", 12.5F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(16, 10),
                AutoSize = true
            };

            var lblSubtitle = new Label
            {
                Text = "Phân tách độc lập 3 phân hệ: Quản trị viên (Admin) • Nhân viên & HLV • Hội viên Gym",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Location = new Point(18, 34),
                AutoSize = true
            };

            // Nút Tạo tài khoản mới
            var btnAdd = new Button
            {
                Text = "➕  Tạo tài khoản mới",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(185, 38),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnCreateAccount_Click;

            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblSubtitle);
            pnl.Controls.Add(btnAdd);
            return pnl;
        }

        // =====================================================================
        // B. KPI STRIP (5 Cards)
        // =====================================================================
        private Panel BuildKpiStrip()
        {
            var pnl = new Panel { Dock = DockStyle.Top, Height = 96, BackColor = Color.Transparent };
            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            for (int i = 0; i < 5; i++) tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            tbl.Controls.Add(CreateKpiCard("TỔNG TÀI KHOẢN", out _lblKpiTotalVal, "0 tài khoản", Orange, "Toàn hệ thống"), 0, 0);
            tbl.Controls.Add(CreateKpiCard("QUẢN TRỊ VIÊN (ADMIN)", out _lblKpiAdminVal, "0 admin", Purple, "Toàn quyền & CN"), 1, 0);
            tbl.Controls.Add(CreateKpiCard("NHÂN VIÊN & HLV", out _lblKpiStaffVal, "0 nhân sự", Blue, "HLV, Lễ tân, Thu ngân"), 2, 0);
            tbl.Controls.Add(CreateKpiCard("HỘI VIÊN GYM", out _lblKpiMemberVal, "0 hội viên", Green, "App GymFit Member"), 3, 0);
            tbl.Controls.Add(CreateKpiCard("TÀI KHOẢN ĐANG KHÓA", out _lblKpiLockedVal, "0 tài khoản", Red, "Vô hiệu hóa tạm thời"), 4, 0);

            pnl.Controls.Add(tbl);
            return pnl;
        }

        private Control CreateKpiCard(string title, out Label valLabel, string defaultVal, Color accent, string subtext)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Margin = new Padding(3, 0, 3, 0),
                Padding = new Padding(14, 10, 14, 10)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
                using var b = new SolidBrush(accent);
                e.Graphics.FillRectangle(b, 0, 0, 4, pnl.Height);
            };

            var lblT = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = MutedColor,
                Dock = DockStyle.Top,
                Height = 20,
                TextAlign = ContentAlignment.MiddleLeft,
                UseMnemonic = false
            };

            valLabel = new Label
            {
                Text = defaultVal,
                Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                ForeColor = accent,
                Dock = DockStyle.Top,
                Height = 32,
                TextAlign = ContentAlignment.MiddleLeft,
                UseMnemonic = false
            };

            var lblS = new Label
            {
                Text = subtext,
                Font = new Font("Segoe UI", 8F),
                ForeColor = MutedColor,
                Dock = DockStyle.Bottom,
                Height = 18,
                TextAlign = ContentAlignment.MiddleLeft,
                UseMnemonic = false
            };

            pnl.Controls.Add(lblS);
            pnl.Controls.Add(valLabel);
            pnl.Controls.Add(lblT);
            return pnl;
        }

        // =====================================================================
        // C. 3-TAB SEGMENTED ROLE SWITCHER (Admin / Staff / Member)
        // =====================================================================
        private Panel BuildRoleTabsSection()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = CardBg,
                Padding = new Padding(8, 6, 8, 6)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            var flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.Transparent,
                WrapContents = false
            };

            _btnTabAdmin = CreateTabButton("🛡️  Quản trị viên (Admin)", 0);
            _btnTabStaff = CreateTabButton("👔  Nhân viên & HLV (Staff)", 1);
            _btnTabMember = CreateTabButton("🏃  Hội viên Gym (Member)", 2);

            flp.Controls.Add(_btnTabAdmin);
            flp.Controls.Add(_btnTabStaff);
            flp.Controls.Add(_btnTabMember);

            pnl.Controls.Add(flp);
            return pnl;
        }

        private Button CreateTabButton(string text, int tabIndex)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = MutedColor,
                BackColor = Color.Transparent,
                Size = new Size(240, 36),
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(4, 0, 4, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => SwitchTab(tabIndex);
            return btn;
        }

        private void SwitchTab(int tabIndex)
        {
            _isInitializing = true;
            _selectedTab = tabIndex;
            UpdateTabStyles();
            ConfigureGridColumnsForCurrentTab();
            UpdateRoleOrTierFilterOptions();
            _isInitializing = false;
            RefreshDataGrid();
        }

        private void UpdateTabStyles()
        {
            Button[] tabs = { _btnTabAdmin, _btnTabStaff, _btnTabMember };
            Color[] activeAccents = { Purple, Blue, Green };

            for (int i = 0; i < tabs.Length; i++)
            {
                if (i == _selectedTab)
                {
                    tabs[i].ForeColor = Color.White;
                    tabs[i].BackColor = activeAccents[i];
                }
                else
                {
                    tabs[i].ForeColor = MutedColor;
                    tabs[i].BackColor = ItemBg;
                }
            }
        }

        // =====================================================================
        // D. FILTER TOOLBAR
        // =====================================================================
        private Panel BuildFilterToolbar()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = CardBg,
                Padding = new Padding(12, 8, 12, 8)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            // 1. Search Box
            var pnlSearch = new Panel
            {
                Dock = DockStyle.Left,
                Width = 280,
                BackColor = ItemBg,
                Padding = new Padding(10, 8, 10, 8)
            };
            pnlSearch.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlSearch.Width - 1, pnlSearch.Height - 1);
            };

            _txtSearch = new TextBox
            {
                Dock = DockStyle.Fill,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.None,
                PlaceholderText = "🔍 Tìm username, họ tên, email, SĐT..."
            };
            _txtSearch.TextChanged += (s, e) => RefreshDataGrid();
            pnlSearch.Controls.Add(_txtSearch);

            // Gaps & Dropdowns
            var pnlGapSearch = new Panel { Dock = DockStyle.Left, Width = 12, BackColor = Color.Transparent };

            // Status Filter Dropdown
            _cboStatusFilter = new ComboBox
            {
                Dock = DockStyle.Left,
                Width = 170,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat
            };
            _cboStatusFilter.Items.AddRange(new object[]
            {
                "Tất cả trạng thái",
                "🟢 Đang hoạt động",
                "🔴 Đang bị khóa"
            });
            _cboStatusFilter.SelectedIndex = 0;
            _cboStatusFilter.SelectedIndexChanged += (s, e) => RefreshDataGrid();

            var pnlGapStatus = new Panel { Dock = DockStyle.Left, Width = 8, BackColor = Color.Transparent };

            // Branch Filter Dropdown
            _cboBranchFilter = new ComboBox
            {
                Dock = DockStyle.Left,
                Width = 210,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat
            };
            _cboBranchFilter.Items.AddRange(new object[]
            {
                "Tất cả chi nhánh",
                "GymFit Quận 1 - Bến Nghé",
                "GymFit Bình Thạnh - Landmark",
                "GymFit Hoàn Kiếm - Tràng Tiền",
                "GymFit Cầu Giấy - Duy Tân"
            });
            _cboBranchFilter.SelectedIndex = 0;
            _cboBranchFilter.SelectedIndexChanged += (s, e) => RefreshDataGrid();

            var pnlGapBranch = new Panel { Dock = DockStyle.Left, Width = 8, BackColor = Color.Transparent };

            // Role / Tier Filter Dropdown
            _cboRoleOrTierFilter = new ComboBox
            {
                Dock = DockStyle.Left,
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9F),
                FlatStyle = FlatStyle.Flat
            };
            _cboRoleOrTierFilter.SelectedIndexChanged += (s, e) => RefreshDataGrid();

            // Right side buttons: Reset & Refresh
            var btnResetFilter = new Button
            {
                Text = "↺  Đặt lại",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = MutedColor,
                BackColor = ItemBg,
                Size = new Size(90, 36),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnResetFilter.FlatAppearance.BorderColor = Border;
            btnResetFilter.Click += (s, e) =>
            {
                _txtSearch.Text = "";
                _cboStatusFilter.SelectedIndex = 0;
                _cboBranchFilter.SelectedIndex = 0;
                if (_cboRoleOrTierFilter.Items.Count > 0) _cboRoleOrTierFilter.SelectedIndex = 0;
                RefreshDataGrid();
            };

            pnl.Controls.Add(btnResetFilter);
            pnl.Controls.Add(_cboRoleOrTierFilter);
            pnl.Controls.Add(pnlGapBranch);
            pnl.Controls.Add(_cboBranchFilter);
            pnl.Controls.Add(pnlGapStatus);
            pnl.Controls.Add(_cboStatusFilter);
            pnl.Controls.Add(pnlGapSearch);
            pnl.Controls.Add(pnlSearch);

            return pnl;
        }

        private void UpdateRoleOrTierFilterOptions()
        {
            _cboRoleOrTierFilter.Items.Clear();
            if (_selectedTab == 0) // Admin
            {
                _cboRoleOrTierFilter.Items.AddRange(new object[]
                {
                    "Tất cả quyền hạn",
                    "Super Admin",
                    "Quản lý Chi nhánh",
                    "Quản lý Tài chính",
                    "Giám sát Vận hành"
                });
            }
            else if (_selectedTab == 1) // Staff
            {
                _cboRoleOrTierFilter.Items.AddRange(new object[]
                {
                    "Tất cả vị trí",
                    "HLV Thể hình Cá nhân",
                    "HLV Yoga & Pilates",
                    "HLV Boxing & Kickfit",
                    "HLV Trưởng Chi Nhánh",
                    "Nhân viên Lễ tân",
                    "Nhân viên Thu ngân"
                });
            }
            else // Member
            {
                _cboRoleOrTierFilter.Items.AddRange(new object[]
                {
                    "Tất cả hạng thẻ",
                    "VIP Diamond",
                    "Platinum",
                    "Gold",
                    "Silver",
                    "Classic"
                });
            }
            _cboRoleOrTierFilter.SelectedIndex = 0;
        }

        // =====================================================================
        // E. MAIN DATAGRIDVIEW SETUP & CONFIGURATION
        // =====================================================================
        private DataGridView BuildMainGridView()
        {
            var g = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = CardBg,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Border,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AutoGenerateColumns = false,
                RowTemplate = { Height = 44 },
                ColumnHeadersHeight = 40,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            };

            // Header Style
            g.ColumnHeadersDefaultCellStyle.BackColor = HeaderBg;
            g.ColumnHeadersDefaultCellStyle.ForeColor = MutedColor;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            // Default Row Style
            g.DefaultCellStyle.BackColor = ItemBg;
            g.DefaultCellStyle.ForeColor = TextColor;
            g.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(40, 40, 50);
            g.DefaultCellStyle.SelectionForeColor = TextColor;
            g.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            // Alternating Row Style
            g.AlternatingRowsDefaultCellStyle.BackColor = CardBg;
            g.AlternatingRowsDefaultCellStyle.ForeColor = TextColor;
            g.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(40, 40, 50);
            g.AlternatingRowsDefaultCellStyle.SelectionForeColor = TextColor;

            // Events
            g.CellContentClick += Grid_CellContentClick;
            g.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0) EditSelectedAccount();
            };

            return g;
        }

        private void ConfigureGridColumnsForCurrentTab()
        {
            _grid.Columns.Clear();
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Stt",
                HeaderText = "STT",
                FillWeight = 35
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Username",
                HeaderText = "USERNAME",
                FillWeight = 110
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FullName",
                HeaderText = "HỌ VÀ TÊN",
                FillWeight = 150
            });

            if (_selectedTab == 0) // Admin
            {
                _grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "RoleOrTier",
                    HeaderText = "PHÂN QUYỀN QUẢN TRỊ",
                    FillWeight = 160
                });
                _grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Branch",
                    HeaderText = "CHI NHÁNH PHỤ TRÁCH",
                    FillWeight = 150
                });
            }
            else if (_selectedTab == 1) // Staff
            {
                _grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "RoleOrTier",
                    HeaderText = "VỊ TRÍ / CHUYÊN MÔN",
                    FillWeight = 160
                });
                _grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Branch",
                    HeaderText = "CƠ SỞ LÀM VIỆC",
                    FillWeight = 150
                });
            }
            else // Member
            {
                _grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "RoleOrTier",
                    HeaderText = "HẠNG THẺ GYM",
                    FillWeight = 110
                });
                _grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Branch",
                    HeaderText = "CHI NHÁNH ĐĂNG KÝ",
                    FillWeight = 150
                });
            }

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Contact",
                HeaderText = "LIÊN HỆ (EMAIL / SĐT)",
                FillWeight = 170
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LastLogin",
                HeaderText = "ĐĂNG NHẬP CUỐI",
                FillWeight = 120
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "TRẠNG THÁI",
                FillWeight = 110
            });

            // Action button columns
            _grid.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "ColEdit",
                HeaderText = "SỬA",
                Text = "✏️ Sửa",
                UseColumnTextForButtonValue = true,
                FillWeight = 65,
                FlatStyle = FlatStyle.Flat
            });

            _grid.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "ColResetPass",
                HeaderText = "MẬT KHẨU",
                Text = "🔑 Reset",
                UseColumnTextForButtonValue = true,
                FillWeight = 75,
                FlatStyle = FlatStyle.Flat
            });

            _grid.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "ColToggleStatus",
                HeaderText = "KHÓA / MỞ",
                Text = "🔒 Khóa",
                UseColumnTextForButtonValue = false,
                FillWeight = 85,
                FlatStyle = FlatStyle.Flat
            });
        }

        // =====================================================================
        // F. REFRESH & BINDING
        // =====================================================================
        private void RefreshAll()
        {
            _isInitializing = true;
            UpdateTabStyles();
            ConfigureGridColumnsForCurrentTab();
            UpdateRoleOrTierFilterOptions();
            RefreshKpis();
            _isInitializing = false;
            RefreshDataGrid();
        }

        private void RefreshKpis()
        {
            int total = _accounts.Count;
            int adminCount = _accounts.Count(a => a.AccountType == "Admin");
            int staffCount = _accounts.Count(a => a.AccountType == "Staff");
            int memberCount = _accounts.Count(a => a.AccountType == "Member");
            int lockedCount = _accounts.Count(a => !a.IsActive);

            _lblKpiTotalVal.Text = $"{total} tài khoản";
            _lblKpiAdminVal.Text = $"{adminCount} admin";
            _lblKpiStaffVal.Text = $"{staffCount} nhân sự";
            _lblKpiMemberVal.Text = $"{memberCount} hội viên";
            _lblKpiLockedVal.Text = $"{lockedCount} đang khóa";

            // Update Tab counts
            _btnTabAdmin.Text = $"🛡️  Quản trị viên (Admin) • {adminCount}";
            _btnTabStaff.Text = $"👔  Nhân viên & HLV • {staffCount}";
            _btnTabMember.Text = $"🏃  Hội viên Gym • {memberCount}";
        }

        private void RefreshDataGrid()
        {
            if (_isInitializing || _grid == null || _grid.Columns.Count == 0) return;

            string targetType = _selectedTab == 0 ? "Admin" : (_selectedTab == 1 ? "Staff" : "Member");

            // Filter ONLY accounts for current tab (NO MIXING!)
            var query = _accounts.Where(a => a.AccountType == targetType).AsEnumerable();

            // 1. Search text
            string q = _txtSearch?.Text?.Trim() ?? "";
            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(a =>
                    a.Username.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.FullName.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.Email.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.Phone.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    a.MemberCode.Contains(q, StringComparison.OrdinalIgnoreCase)
                );
            }

            // 2. Status filter
            if (_cboStatusFilter != null && _cboStatusFilter.SelectedIndex > 0)
            {
                bool wantActive = _cboStatusFilter.SelectedIndex == 1;
                query = query.Where(a => a.IsActive == wantActive);
            }

            // 3. Branch filter
            if (_cboBranchFilter != null && _cboBranchFilter.SelectedIndex > 0)
            {
                string selBranch = _cboBranchFilter.SelectedItem?.ToString() ?? "";
                query = query.Where(a => a.BranchName.Contains(selBranch, StringComparison.OrdinalIgnoreCase) || a.BranchName.Contains("Tất cả", StringComparison.OrdinalIgnoreCase));
            }

            // 4. Role / Tier filter
            if (_cboRoleOrTierFilter != null && _cboRoleOrTierFilter.SelectedIndex > 0)
            {
                string selRoleOrTier = _cboRoleOrTierFilter.SelectedItem?.ToString() ?? "";
                if (_selectedTab == 2)
                {
                    query = query.Where(a => a.Tier.Equals(selRoleOrTier, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    query = query.Where(a => a.SubRoleOrTitle.Contains(selRoleOrTier, StringComparison.OrdinalIgnoreCase));
                }
            }

            var list = query.OrderByDescending(a => a.CreatedAt).ToList();

            _grid.Rows.Clear();
            int stt = 1;

            foreach (var item in list)
            {
                int rowIndex = _grid.Rows.Add();
                var row = _grid.Rows[rowIndex];
                row.Tag = item;

                row.Cells["Stt"].Value = stt++;
                row.Cells["Username"].Value = item.Username;

                string nameDisplay = string.IsNullOrEmpty(item.MemberCode) ? item.FullName : $"{item.FullName} ({item.MemberCode})";
                row.Cells["FullName"].Value = nameDisplay;

                string roleOrTier = _selectedTab == 2 ? item.Tier : item.SubRoleOrTitle;
                row.Cells["RoleOrTier"].Value = roleOrTier;
                row.Cells["Branch"].Value = item.BranchName;
                row.Cells["Contact"].Value = $"{item.Phone} • {item.Email}";

                string lastLoginStr = item.LastLogin.HasValue
                    ? item.LastLogin.Value.ToString("dd/MM/yyyy HH:mm")
                    : "Chưa đăng nhập";
                row.Cells["LastLogin"].Value = lastLoginStr;

                row.Cells["Status"].Value = item.IsActive ? "🟢 Hoạt động" : "🔴 Đang khóa";
                row.Cells["ColToggleStatus"].Value = item.IsActive ? "🔒 Khóa" : "🔓 Mở";

                // Styling
                row.Cells["Username"].Style.ForeColor = Orange;
                row.Cells["FullName"].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

                // Tier colors
                if (_selectedTab == 2)
                {
                    if (item.Tier == "VIP Diamond") row.Cells["RoleOrTier"].Style.ForeColor = Orange;
                    else if (item.Tier == "Platinum") row.Cells["RoleOrTier"].Style.ForeColor = Purple;
                    else if (item.Tier == "Gold") row.Cells["RoleOrTier"].Style.ForeColor = Gold;
                    else if (item.Tier == "Silver") row.Cells["RoleOrTier"].Style.ForeColor = Cyan;
                    else row.Cells["RoleOrTier"].Style.ForeColor = Blue;
                }
                else
                {
                    row.Cells["RoleOrTier"].Style.ForeColor = _selectedTab == 0 ? Purple : Blue;
                }

                row.Cells["Status"].Style.ForeColor = item.IsActive ? Green : Red;
                row.Cells["ColToggleStatus"].Style.ForeColor = item.IsActive ? Red : Green;
            }
        }

        // =====================================================================
        // G. GRID ACTIONS: EDIT, RESET PASSWORD, LOCK/UNLOCK
        // =====================================================================
        private void Grid_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = _grid.Rows[e.RowIndex];
            if (row.Tag is not AccountModel acc) return;

            string colName = _grid.Columns[e.ColumnIndex].Name;

            if (colName == "ColEdit")
            {
                EditAccount(acc);
            }
            else if (colName == "ColResetPass")
            {
                ResetPassword(acc);
            }
            else if (colName == "ColToggleStatus")
            {
                ToggleAccountStatus(acc);
            }
        }

        private void EditSelectedAccount()
        {
            if (_grid.CurrentRow?.Tag is AccountModel acc)
            {
                EditAccount(acc);
            }
        }

        private void EditAccount(AccountModel acc)
        {
            var editModel = new frmAccountEdit.AccountEditModel
            {
                Id = acc.Id,
                AccountType = acc.AccountType,
                Username = acc.Username,
                FullName = acc.FullName,
                Email = acc.Email,
                Phone = acc.Phone,
                SubRoleOrTitle = acc.SubRoleOrTitle,
                BranchName = acc.BranchName,
                Tier = acc.Tier,
                MemberCode = acc.MemberCode,
                IsActive = acc.IsActive,
                CreatedAt = acc.CreatedAt,
                LastLogin = acc.LastLogin
            };

            using var dlg = new frmAccountEdit(editModel);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.UpdatedAccount != null)
            {
                acc.FullName = dlg.UpdatedAccount.FullName;
                acc.Email = dlg.UpdatedAccount.Email;
                acc.Phone = dlg.UpdatedAccount.Phone;
                acc.BranchName = dlg.UpdatedAccount.BranchName;
                acc.IsActive = dlg.UpdatedAccount.IsActive;
                if (!string.IsNullOrEmpty(dlg.UpdatedAccount.SubRoleOrTitle)) acc.SubRoleOrTitle = dlg.UpdatedAccount.SubRoleOrTitle;
                if (!string.IsNullOrEmpty(dlg.UpdatedAccount.Tier)) acc.Tier = dlg.UpdatedAccount.Tier;

                RefreshKpis();
                RefreshDataGrid();
                MessageBox.Show($"Đã cập nhật thành công thông tin tài khoản '{acc.Username}'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ResetPassword(AccountModel acc)
        {
            using var dlg = new frmAccountChangePassword(acc.Username, acc.FullName, acc.AccountType == "Admin" ? acc.SubRoleOrTitle : (acc.AccountType == "Staff" ? acc.SubRoleOrTitle : $"Hội viên {acc.Tier}"));
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                MessageBox.Show($"Đã đổi mật khẩu thành công cho tài khoản '{acc.Username}'!\n(Yêu cầu đổi mật khẩu ở lần đăng nhập tiếp theo: {(dlg.RequireChangeOnNextLogin ? "Có" : "Không")})", "Đổi mật khẩu thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ToggleAccountStatus(AccountModel acc)
        {
            string action = acc.IsActive ? "KHÓA" : "MỞ KHÓA";
            string msg = acc.IsActive
                ? $"Bạn có chắc chắn muốn KHÓA tài khoản '{acc.Username}' ({acc.FullName})?\nNgười dùng sẽ không thể đăng nhập vào hệ thống khi bị khóa."
                : $"Bạn có chắc chắn muốn MỞ KHÓA cho tài khoản '{acc.Username}' ({acc.FullName})?";

            if (MessageBox.Show(msg, $"Xác nhận {action} tài khoản", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                acc.IsActive = !acc.IsActive;
                RefreshKpis();
                RefreshDataGrid();
                MessageBox.Show($"Đã {action.ToLower()} thành công tài khoản '{acc.Username}'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCreateAccount_Click(object? sender, EventArgs e)
        {
            string defaultType = _selectedTab == 0 ? "Admin" : (_selectedTab == 1 ? "Staff" : "Member");
            var existing = _accounts.Select(a => a.Username);

            using var dlg = new frmAccountCreate(defaultType, existing);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.CreatedAccount != null)
            {
                var r = dlg.CreatedAccount;
                var newAcc = new AccountModel
                {
                    Id = _accounts.Count > 0 ? _accounts.Max(a => a.Id) + 1 : 1,
                    AccountType = r.AccountType,
                    Username = r.Username,
                    FullName = r.FullName,
                    Email = r.Email,
                    Phone = r.Phone,
                    SubRoleOrTitle = r.SubRoleOrTitle,
                    BranchName = r.BranchName,
                    Tier = r.Tier,
                    MemberCode = r.MemberCode,
                    IsActive = r.IsActive,
                    CreatedAt = DateTime.Now,
                    LastLogin = null
                };

                _accounts.Insert(0, newAcc);

                // Switch to the newly created account's tab if needed
                int targetTab = newAcc.AccountType == "Admin" ? 0 : (newAcc.AccountType == "Staff" ? 1 : 2);
                if (_selectedTab != targetTab)
                {
                    SwitchTab(targetTab);
                }
                else
                {
                    RefreshKpis();
                    RefreshDataGrid();
                }

                MessageBox.Show($"Đã tạo thành công tài khoản '{newAcc.Username}' cho '{newAcc.FullName}' ({newAcc.AccountType})!", "Tạo tài khoản thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
