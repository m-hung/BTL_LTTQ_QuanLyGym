using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public class ucAdminBranch : UserControl
    {
        // =====================================================================
        // THEME & COLORS
        // =====================================================================
        private static readonly Color PageBg = Color.FromArgb(15, 15, 17);
        private static readonly Color CardBg = Color.FromArgb(24, 24, 27);
        private static readonly Color ItemBg = Color.FromArgb(20, 20, 23);
        private static readonly Color ItemHoverBg = Color.FromArgb(30, 30, 35);
        private static readonly Color Border = Color.FromArgb(45, 45, 50);
        private static readonly Color TextColor = Color.FromArgb(244, 244, 245);
        private static readonly Color MutedColor = Color.FromArgb(161, 161, 170);
        private static readonly Color DimColor = Color.FromArgb(113, 113, 122);
        private static readonly Color Orange = Color.FromArgb(249, 115, 22);
        private static readonly Color OrangeDark = Color.FromArgb(234, 88, 12);
        private static readonly Color Green = Color.FromArgb(34, 197, 94);
        private static readonly Color Red = Color.FromArgb(239, 68, 68);
        private static readonly Color Blue = Color.FromArgb(59, 130, 246);
        private static readonly Color Purple = Color.FromArgb(168, 85, 247);
        private static readonly CultureInfo ViCulture = new CultureInfo("vi-VN");

        // =====================================================================
        // DATA REPOSITORY (Chi nhánh, Nhân sự và Học viên)
        // =====================================================================
        private readonly List<BranchModel> _branches = new List<BranchModel>();
        private readonly Dictionary<int, List<BranchStaffItem>> _branchStaffMap = new Dictionary<int, List<BranchStaffItem>>();
        private readonly Dictionary<int, List<BranchMemberItem>> _branchMemberMap = new Dictionary<int, List<BranchMemberItem>>();

        private BranchModel? _selectedBranch;
        private string _branchSearchText = string.Empty;
        private int _branchFilterMode = 0; // 0: Tất cả, 1: Đang mở (Online), 2: Tạm đóng (Offline)

        // Sub-tabs in Right Workspace
        private int _selectedDetailTab = 0; // 0: Nhân viên, 1: Học viên, 2: Cơ sở vật chất

        // Master UI Controls
        private Label _lblSummaryBranches = null!;
        private Label _lblSummaryOnline = null!;
        private Label _lblSummaryOffline = null!;
        private Label _lblSummaryMembers = null!;
        private Label _lblSummaryStaff = null!;
        private FlowLayoutPanel _flpBranchCards = null!;
        private Button _btnFilterAll = null!;
        private Button _btnFilterOnline = null!;
        private Button _btnFilterOffline = null!;

        // Detail Workspace Controls
        private Panel _pnlRightWorkspace = null!;
        private Label _lblDetailName = null!;
        private Label _lblDetailCode = null!;
        private Label _lblDetailAddress = null!;
        private Label _lblDetailPhone = null!;
        private Label _lblDetailEmail = null!;
        private Label _lblDetailManager = null!;
        private Label _lblDetailStatusBadge = null!;
        private Button _btnToggleBranchStatus = null!;
        private Button _btnEditBranch = null!;
        private Button _btnDeleteBranch = null!;

        // Mini KPI Badges
        private Label _lblKpiMembersVal = null!;
        private Label _lblKpiStaffVal = null!;
        private Label _lblKpiHoursVal = null!;
        private Label _lblKpiRevenueVal = null!;

        // Sub-tab Header Buttons
        private Button _btnTabStaff = null!;
        private Button _btnTabMembers = null!;
        private Button _btnTabFacility = null!;
        private Panel _pnlTabContainer = null!;

        // Grids & Content Panels
        private DataGridView _gridStaff = null!;
        private DataGridView _gridMembers = null!;
        private Panel _pnlFacilityContent = null!;
        private TextBox _txtStaffSearch = null!;
        private FilterDropdown _ddStaffRole = null!;
        private TextBox _txtMemberSearch = null!;
        private FilterDropdown _ddMemberPackage = null!;

        public ucAdminBranch()
        {
            InitializeData();
            BuildUI();
            RefreshBranchList();
            SelectBranch(_branches.FirstOrDefault());
        }

        // =====================================================================
        // KHỞI TẠO DỮ LIỆU BAN ĐẦU
        // =====================================================================
        private void InitializeData()
        {
            var b1 = new BranchModel
            {
                Id = 1,
                Code = "CN-Q1",
                Name = "GymFit Quận 1 - Bến Nghé",
                Address = "Số 12 Mạc Đĩnh Chi, P. Bến Nghé, Quận 1, TP.HCM",
                Phone = "028 3822 9988",
                Email = "quan1@gymfit.vn",
                Manager = "Nguyễn Hải Long",
                OpenTime = "05:30",
                CloseTime = "22:30",
                AreaSqM = 1500,
                IsOnline = true,
                Note = "Trụ sở chính tiêu chuẩn 5 sao: Bể bơi bốn mùa, Sauna đá muối Himalaya, Boxing Ring & Yoga Studio",
                MemberCount = 420,
                StaffCount = 12,
                MonthlyRevenue = 380000000
            };

            var b2 = new BranchModel
            {
                Id = 2,
                Code = "CN-HK",
                Name = "GymFit Hoàn Kiếm - Tràng Tiền",
                Address = "Số 24 Tràng Tiền, Q. Hoàn Kiếm, Hà Nội",
                Phone = "024 3936 7788",
                Email = "hoankiem@gymfit.vn",
                Manager = "Hoàng Thu Thảo",
                OpenTime = "06:00",
                CloseTime = "22:00",
                AreaSqM = 1200,
                IsOnline = true,
                Note = "Vị trí trung tâm thủ đô, trang bị máy Technogym thế hệ mới, phòng tập nhóm GroupX hiện đại",
                MemberCount = 350,
                StaffCount = 10,
                MonthlyRevenue = 285000000
            };

            var b3 = new BranchModel
            {
                Id = 3,
                Code = "CN-CG",
                Name = "GymFit Cầu Giấy - Duy Tân",
                Address = "Số 88 Duy Tân, P. Dịch Vọng Hậu, Q. Cầu Giấy, Hà Nội",
                Phone = "024 3795 6677",
                Email = "caugiay@gymfit.vn",
                Manager = "Đỗ Đình Mạnh",
                OpenTime = "05:30",
                CloseTime = "22:00",
                AreaSqM = 1100,
                IsOnline = true,
                Note = "Phục vụ cộng đồng văn phòng công nghệ, khu Cardio view kính Panorama, dàn tạ Rogue chuyên nghiệp",
                MemberCount = 290,
                StaffCount = 9,
                MonthlyRevenue = 190000000
            };

            var b4 = new BranchModel
            {
                Id = 4,
                Code = "CN-BT",
                Name = "GymFit Bình Thạnh - Pearl Plaza",
                Address = "Tầng 4 Pearl Plaza, 561A Điện Biên Phủ, P. 25, Q. Bình Thạnh, TP.HCM",
                Phone = "028 3512 4455",
                Email = "binhthanh@gymfit.vn",
                Manager = "Phan Thanh Nam",
                OpenTime = "06:00",
                CloseTime = "22:00",
                AreaSqM = 950,
                IsOnline = true,
                Note = "Tích hợp trong TTTM cao cấp, thuận tiện đỗ xe ô tô, quầy Bar dinh dưỡng bổ sung Protein Shake",
                MemberCount = 240,
                StaffCount = 8,
                MonthlyRevenue = 165000000
            };

            var b5 = new BranchModel
            {
                Id = 5,
                Code = "CN-HC",
                Name = "GymFit Hải Châu - Bạch Đằng",
                Address = "Số 178 Bạch Đằng, P. Hải Châu 1, Q. Hải Châu, Đà Nẵng",
                Phone = "0236 388 9911",
                Email = "haichau@gymfit.vn",
                Manager = "Lê Tuấn Anh",
                OpenTime = "06:00",
                CloseTime = "21:30",
                AreaSqM = 850,
                IsOnline = false, // Tạm đóng bảo trì theo đúng dashboard "1 đang bảo trì"
                Note = "Đang tạm ngừng đón khách để nâng cấp sàn tạ cao su chống rung và bảo dưỡng hệ thống điều hòa không khí",
                MemberCount = 180,
                StaffCount = 6,
                MonthlyRevenue = 140000000
            };

            _branches.AddRange(new[] { b1, b2, b3, b4, b5 });

            // Seed nhân viên cho các chi nhánh
            SeedStaffData(b1.Id, "CN-Q1");
            SeedStaffData(b2.Id, "CN-HK");
            SeedStaffData(b3.Id, "CN-CG");
            SeedStaffData(b4.Id, "CN-BT");
            SeedStaffData(b5.Id, "CN-HC");

            // Seed học viên cho các chi nhánh
            SeedMemberData(b1.Id);
            SeedMemberData(b2.Id);
            SeedMemberData(b3.Id);
            SeedMemberData(b4.Id);
            SeedMemberData(b5.Id);
        }

        private void SeedStaffData(int branchId, string code)
        {
            var list = new List<BranchStaffItem>
            {
                new BranchStaffItem { StaffId = $"NV-{code}-01", Name = "Trần Đình Trọng", Role = "PT", Phone = "0908 123 456", Shift = "Ca sáng (06:00 - 14:00)", JoinDate = "12/03/2023", IsWorking = true },
                new BranchStaffItem { StaffId = $"NV-{code}-02", Name = "Nguyễn Thuỳ Dung", Role = "Thu ngân", Phone = "0934 567 890", Shift = "Ca chiều (14:00 - 22:00)", JoinDate = "05/06/2023", IsWorking = true },
                new BranchStaffItem { StaffId = $"NV-{code}-03", Name = "Đặng Văn Lâm", Role = "PT", Phone = "0912 345 678", Shift = "Ca chiều (14:00 - 22:00)", JoinDate = "18/09/2023", IsWorking = true },
                new BranchStaffItem { StaffId = $"NV-{code}-04", Name = "Phạm Thị Lan", Role = "Tạp vụ", Phone = "0978 901 234", Shift = "Ca sáng (06:00 - 14:00)", JoinDate = "01/11/2022", IsWorking = true },
                new BranchStaffItem { StaffId = $"NV-{code}-05", Name = "Vũ Đình Toàn", Role = "Bảo vệ", Phone = "0989 012 345", Shift = "Ca xoay", JoinDate = "15/01/2024", IsWorking = true }
            };
            _branchStaffMap[branchId] = list;
        }

        private void SeedMemberData(int branchId)
        {
            var list = new List<BranchMemberItem>
            {
                new BranchMemberItem { MemberId = $"HV-{branchId}01", Name = "Lê Hoàng Yến", Phone = "0909 333 444", Package = "Gói VIP Diamond (12T)", AssignedPt = "Trần Đình Trọng", StartDate = "10/01/2026", ExpireDate = "10/01/2027", Status = "Đang tập" },
                new BranchMemberItem { MemberId = $"HV-{branchId}02", Name = "Phạm Minh Cường", Phone = "0988 555 666", Package = "Gói Platinum 6 Tháng", AssignedPt = "Tự tập", StartDate = "15/08/2025", ExpireDate = "15/02/2026", Status = "Đang tập" },
                new BranchMemberItem { MemberId = $"HV-{branchId}03", Name = "Vũ Thị Phương", Phone = "0912 777 888", Package = "Gói PT 1-1 Chuyên Sâu", AssignedPt = "Đặng Văn Lâm", StartDate = "01/09/2025", ExpireDate = "01/11/2025", Status = "Sắp hết hạn" },
                new BranchMemberItem { MemberId = $"HV-{branchId}04", Name = "Hoàng Tuấn Khang", Phone = "0933 111 222", Package = "Gói Classic 3 Tháng", AssignedPt = "Tự tập", StartDate = "05/05/2025", ExpireDate = "05/08/2025", Status = "Đã hết hạn" },
                new BranchMemberItem { MemberId = $"HV-{branchId}05", Name = "Ngô Bảo Châu", Phone = "0944 888 999", Package = "Gói VIP Diamond (12T)", AssignedPt = "Trần Đình Trọng", StartDate = "20/02/2026", ExpireDate = "20/02/2027", Status = "Đang tập" }
            };
            _branchMemberMap[branchId] = list;
        }

        // =====================================================================
        // XÂY DỰNG TOÀN BỘ GIAO DIỆN
        // =====================================================================
        private void BuildUI()
        {
            BackColor = PageBg;
            Dock = DockStyle.Fill;
            Font = new Font("Segoe UI", 9F);
            Padding = new Padding(16, 12, 16, 14);

            // 1. Top Command Header Bar
            var pnlTopBar = BuildTopCommandBar();

            // 2. Master-Detail Container (2 Cột ổn định, không lỗi SplitterDistance)
            var pnlBodyContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = PageBg,
                Padding = new Padding(0, 10, 0, 0)
            };

            // Cột trái: Master Branch Rails
            var pnlLeftMaster = BuildLeftMasterRail();
            pnlLeftMaster.Dock = DockStyle.Left;
            pnlLeftMaster.Width = 380;

            // Khoảng đệm giữa 2 cột
            var pnlGap = new Panel
            {
                Dock = DockStyle.Left,
                Width = 10,
                BackColor = PageBg
            };

            // Cột phải: Detail & Management Workspace
            _pnlRightWorkspace = BuildRightDetailWorkspace();
            _pnlRightWorkspace.Dock = DockStyle.Fill;

            pnlBodyContainer.Controls.Add(_pnlRightWorkspace);
            pnlBodyContainer.Controls.Add(pnlGap);
            pnlBodyContainer.Controls.Add(pnlLeftMaster);

            Controls.Add(pnlBodyContainer);
            Controls.Add(pnlTopBar);
        }

        // ---------------------------------------------------------------------
        // 1. THANH ĐIỀU HÀNH TRÊN CÙNG (TOP HEADER)
        // ---------------------------------------------------------------------
        private Panel BuildTopCommandBar()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = Color.FromArgb(20, 20, 23),
                Margin = new Padding(0, 0, 0, 10),
                Padding = new Padding(16, 8, 16, 8)
            };
            pnl.Paint += (s, e) =>
            {
                using var pen = new Pen(Border);
                e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            // Tiêu đề & phụ đề bên trái
            var pnlTitle = new Panel { Dock = DockStyle.Left, Width = 450, BackColor = Color.Transparent };
            var lblTitle = new Label
            {
                Text = "🏢  QUẢN LÝ HỆ THỐNG CHI NHÁNH",
                Font = new Font("Segoe UI", 12.5F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(0, 4),
                AutoSize = true,
                UseMnemonic = false
            };
            var lblSub = new Label
            {
                Text = "Kiểm soát trạng thái Online/Offline & Quản lý nhân sự, học viên tại từng cơ sở",
                Font = new Font("Segoe UI", 8F),
                ForeColor = MutedColor,
                Location = new Point(2, 28),
                AutoSize = true,
                UseMnemonic = false
            };
            pnlTitle.Controls.Add(lblTitle);
            pnlTitle.Controls.Add(lblSub);

            // Nút "Thêm chi nhánh mới" bên phải
            var btnAddBranch = new Button
            {
                Text = "➕  Thêm chi nhánh mới",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Right,
                Width = 200,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btnAddBranch.FlatAppearance.BorderSize = 0;
            btnAddBranch.FlatAppearance.MouseOverBackColor = OrangeDark;
            btnAddBranch.Click += BtnAddBranch_Click;

            // Cụm thống kê nhanh (KPI Pills) ở giữa
            var flpPills = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(10, 8, 10, 0)
            };

            _lblSummaryBranches = CreateStatPill("🏢 5 Chi nhánh", TextColor, Color.FromArgb(39, 39, 42));
            _lblSummaryOnline = CreateStatPill("🟢 4 Đang mở (ONL)", Green, Color.FromArgb(20, 40, 25));
            _lblSummaryOffline = CreateStatPill("🔴 1 Tạm đóng (OFF)", Red, Color.FromArgb(45, 20, 20));
            _lblSummaryMembers = CreateStatPill("👥 1,480 Học viên", Blue, Color.FromArgb(20, 30, 45));
            _lblSummaryStaff = CreateStatPill("👔 45 Nhân sự", Purple, Color.FromArgb(35, 20, 45));

            flpPills.Controls.Add(_lblSummaryBranches);
            flpPills.Controls.Add(_lblSummaryOnline);
            flpPills.Controls.Add(_lblSummaryOffline);
            flpPills.Controls.Add(_lblSummaryMembers);
            flpPills.Controls.Add(_lblSummaryStaff);

            pnl.Controls.Add(flpPills);
            pnl.Controls.Add(btnAddBranch);
            pnl.Controls.Add(pnlTitle);
            return pnl;
        }

        private Label CreateStatPill(string text, Color foreColor, Color bg)
        {
            var lbl = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = foreColor,
                BackColor = bg,
                Padding = new Padding(10, 6, 10, 6),
                Margin = new Padding(0, 0, 8, 0),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            lbl.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(63, 63, 70));
                e.Graphics.DrawRectangle(p, 0, 0, lbl.Width - 1, lbl.Height - 1);
            };
            return lbl;
        }

        // ---------------------------------------------------------------------
        // 2. CỘT TRÁI: MASTER BRANCH DIRECTORY (DANH SÁCH CHI NHÁNH)
        // ---------------------------------------------------------------------
        private Panel BuildLeftMasterRail()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Padding = new Padding(12, 12, 12, 12)
            };
            pnl.Paint += (s, e) =>
            {
                using var pen = new Pen(Border);
                e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            // Thanh tìm kiếm chi nhánh
            var pnlSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = ItemBg,
                Margin = new Padding(0, 0, 0, 8),
                Padding = new Padding(8, 7, 8, 4)
            };
            pnlSearch.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlSearch.Width - 1, pnlSearch.Height - 1);
            };

            var txtSearch = new TextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                PlaceholderText = "🔍 Tìm kiếm theo tên hoặc địa chỉ..."
            };
            txtSearch.TextChanged += (s, e) =>
            {
                _branchSearchText = txtSearch.Text.Trim();
                RefreshBranchList();
            };
            pnlSearch.Controls.Add(txtSearch);

            // Nút bộ lọc trạng thái (Tất cả | Đang mở | Tạm đóng)
            var pnlFilters = new Panel
            {
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 6, 0, 8)
            };

            _btnFilterAll = CreateFilterTabBtn("Tất cả (5)", true);
            _btnFilterOnline = CreateFilterTabBtn("Đang mở (4)", false);
            _btnFilterOffline = CreateFilterTabBtn("Tạm đóng (1)", false);

            _btnFilterAll.Click += (s, e) => SetBranchFilter(0);
            _btnFilterOnline.Click += (s, e) => SetBranchFilter(1);
            _btnFilterOffline.Click += (s, e) => SetBranchFilter(2);

            _btnFilterAll.Location = new Point(0, 2);
            _btnFilterOnline.Location = new Point(105, 2);
            _btnFilterOffline.Location = new Point(220, 2);

            pnlFilters.Controls.Add(_btnFilterAll);
            pnlFilters.Controls.Add(_btnFilterOnline);
            pnlFilters.Controls.Add(_btnFilterOffline);

            // Danh sách cuộn các thẻ chi nhánh
            _flpBranchCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 8, 0, 8)
            };

            pnl.Controls.Add(_flpBranchCards);
            pnl.Controls.Add(pnlFilters);
            pnl.Controls.Add(pnlSearch);
            return pnl;
        }

        private Button CreateFilterTabBtn(string text, bool isActive)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = isActive ? Orange : MutedColor,
                BackColor = isActive ? Color.FromArgb(39, 39, 42) : Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 30),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderColor = isActive ? Orange : Border;
            btn.FlatAppearance.BorderSize = 1;
            return btn;
        }

        private void SetBranchFilter(int mode)
        {
            _branchFilterMode = mode;
            _btnFilterAll.ForeColor = (mode == 0) ? Orange : MutedColor;
            _btnFilterAll.FlatAppearance.BorderColor = (mode == 0) ? Orange : Border;
            _btnFilterAll.BackColor = (mode == 0) ? Color.FromArgb(39, 39, 42) : Color.Transparent;

            _btnFilterOnline.ForeColor = (mode == 1) ? Green : MutedColor;
            _btnFilterOnline.FlatAppearance.BorderColor = (mode == 1) ? Green : Border;
            _btnFilterOnline.BackColor = (mode == 1) ? Color.FromArgb(20, 40, 25) : Color.Transparent;

            _btnFilterOffline.ForeColor = (mode == 2) ? Red : MutedColor;
            _btnFilterOffline.FlatAppearance.BorderColor = (mode == 2) ? Red : Border;
            _btnFilterOffline.BackColor = (mode == 2) ? Color.FromArgb(45, 20, 20) : Color.Transparent;

            RefreshBranchList();
        }

        // ---------------------------------------------------------------------
        // 3. CỘT PHẢI: DETAIL & MANAGEMENT WORKSPACE
        // ---------------------------------------------------------------------
        private Panel BuildRightDetailWorkspace()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Padding = new Padding(16, 14, 16, 14)
            };
            pnl.Paint += (s, e) =>
            {
                using var pen = new Pen(Border);
                e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            // A. Header thông tin chi nhánh đang chọn + Nút ONL/OFF + Sửa/Xóa
            var pnlBranchHeader = BuildBranchHeaderSection();
            var pnlGap1 = new Panel { Dock = DockStyle.Top, Height = 12, BackColor = Color.Transparent };

            // B. 4 Thẻ KPI Mini của riêng chi nhánh này
            var pnlKpis = BuildBranchKpiStrip();
            var pnlGap2 = new Panel { Dock = DockStyle.Top, Height = 14, BackColor = Color.Transparent };

            // C. Khu vực điều hướng Tab quản lý (Nhân viên / Học viên / Cơ sở vật chất)
            var pnlTabHeader = BuildTabHeaderSection();
            var pnlGap3 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // D. Khung chứa nội dung của Tab đang chọn
            _pnlTabContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 4, 0, 0)
            };

            // Dựng sẵn 3 view tab
            _gridStaff = BuildStaffGridView();
            _gridMembers = BuildMemberGridView();
            _pnlFacilityContent = BuildFacilitySection();

            // Mặc định nạp tab 0 (Nhân viên)
            SwitchDetailTab(0);

            pnl.Controls.Add(_pnlTabContainer);
            pnl.Controls.Add(pnlGap3);
            pnl.Controls.Add(pnlTabHeader);
            pnl.Controls.Add(pnlGap2);
            pnl.Controls.Add(pnlKpis);
            pnl.Controls.Add(pnlGap1);
            pnl.Controls.Add(pnlBranchHeader);
            return pnl;
        }

        private Panel BuildBranchHeaderSection()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 118,
                BackColor = ItemBg,
                Padding = new Padding(16, 14, 16, 14)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            // Cột bên phải: Nút ONL/OFF to bản + Nút Sửa + Nút Xóa
            var pnlActions = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };

            _btnToggleBranchStatus = new Button
            {
                Text = "⚡  Bật / Tắt chi nhánh",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Size = new Size(270, 38),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                UseVisualStyleBackColor = false
            };
            _btnToggleBranchStatus.Click += (s, e) => ToggleCurrentBranchStatus();

            var flpSmallBtns = new FlowLayoutPanel
            {
                Size = new Size(270, 36),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0, 8, 0, 0)
            };

            _btnEditBranch = new Button
            {
                Text = "✏️ Sửa chi nhánh",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = TextColor,
                BackColor = Color.FromArgb(39, 39, 42),
                Size = new Size(160, 36),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                UseVisualStyleBackColor = false
            };
            _btnEditBranch.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 70);
            _btnEditBranch.Click += BtnEditBranch_Click;

            _btnDeleteBranch = new Button
            {
                Text = "🗑️ Xóa",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Red,
                BackColor = Color.FromArgb(45, 20, 20),
                Size = new Size(100, 36),
                Margin = new Padding(10, 0, 0, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                UseVisualStyleBackColor = false
            };
            _btnDeleteBranch.FlatAppearance.BorderColor = Red;
            _btnDeleteBranch.Click += BtnDeleteBranch_Click;

            flpSmallBtns.Controls.Add(_btnEditBranch);
            flpSmallBtns.Controls.Add(_btnDeleteBranch);

            pnlActions.Controls.Add(_btnToggleBranchStatus);
            pnlActions.Controls.Add(flpSmallBtns);

            // Cột bên trái: Tên chi nhánh, Mã CN, Địa chỉ, SĐT, Quản lý, Huy hiệu trạng thái
            var pnlInfo = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var pnlTopTitle = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 32,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            _lblDetailName = new Label
            {
                Text = "Tên chi nhánh",
                Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true
            };

            _lblDetailCode = new Label
            {
                Text = "CN-01",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Orange,
                BackColor = Color.FromArgb(45, 30, 20),
                Padding = new Padding(6, 3, 6, 3),
                Margin = new Padding(10, 2, 10, 0),
                AutoSize = true
            };

            _lblDetailStatusBadge = new Label
            {
                Text = "🟢 HOẠT ĐỘNG (ONLINE)",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Green,
                BackColor = Color.FromArgb(20, 40, 25),
                Padding = new Padding(8, 3, 8, 3),
                Margin = new Padding(0, 2, 0, 0),
                AutoSize = true
            };

            pnlTopTitle.Controls.Add(_lblDetailName);
            pnlTopTitle.Controls.Add(_lblDetailCode);
            pnlTopTitle.Controls.Add(_lblDetailStatusBadge);

            _lblDetailAddress = new Label
            {
                Text = "📍 Địa chỉ: ...",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Location = new Point(0, 36),
                AutoSize = true
            };

            _lblDetailPhone = new Label
            {
                Text = "📞 Hotline: ...",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Location = new Point(0, 62),
                AutoSize = true
            };

            _lblDetailEmail = new Label
            {
                Text = "✉️ Email: ...",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Location = new Point(220, 62),
                AutoSize = true
            };

            _lblDetailManager = new Label
            {
                Text = "👔 Quản lý: ...",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Orange,
                Location = new Point(440, 62),
                AutoSize = true
            };

            pnlInfo.Controls.Add(pnlTopTitle);
            pnlInfo.Controls.Add(_lblDetailAddress);
            pnlInfo.Controls.Add(_lblDetailPhone);
            pnlInfo.Controls.Add(_lblDetailEmail);
            pnlInfo.Controls.Add(_lblDetailManager);

            pnl.Controls.Add(pnlInfo);
            pnl.Controls.Add(pnlActions);
            return pnl;
        }

        // ---------------------------------------------------------------------
        // B. DẢI MINI KPI CỦA CHI NHÁNH
        // ---------------------------------------------------------------------
        private Panel BuildBranchKpiStrip()
        {
            var pnlWrapper = new Panel
            {
                Dock = DockStyle.Top,
                Height = 96,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 2, 0, 2)
            };

            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            tbl.Controls.Add(CreateMiniKpiCard("👥  TỔNG HỌC VIÊN", out _lblKpiMembersVal, Blue), 0, 0);
            tbl.Controls.Add(CreateMiniKpiCard("👔  ĐỘI NGŨ NHÂN SỰ", out _lblKpiStaffVal, Purple), 1, 0);
            tbl.Controls.Add(CreateMiniKpiCard("⏰  GIỜ HOẠT ĐỘNG", out _lblKpiHoursVal, Green), 2, 0);
            tbl.Controls.Add(CreateMiniKpiCard("💰  DOANH THU THÁNG", out _lblKpiRevenueVal, Orange), 3, 0);

            pnlWrapper.Controls.Add(tbl);
            return pnlWrapper;
        }

        private Panel CreateMiniKpiCard(string title, out Label lblVal, Color accent)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ItemBg,
                Margin = new Padding(4, 0, 4, 0),
                Padding = new Padding(14, 10, 14, 10)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            var lblT = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = MutedColor,
                Dock = DockStyle.Top,
                Height = 22,
                UseMnemonic = false
            };

            lblVal = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                ForeColor = accent,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                UseMnemonic = false
            };

            pnl.Controls.Add(lblVal);
            pnl.Controls.Add(lblT);
            return pnl;
        }

        // ---------------------------------------------------------------------
        // C. THANH ĐIỀU HƯỚNG SUB-TABS (NHÂN VIÊN / HỌC VIÊN / CƠ SỞ)
        // ---------------------------------------------------------------------
        private Panel BuildTabHeaderSection()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 4, 0, 4)
            };

            var flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            _btnTabStaff = CreateSubTabButton("👔  Nhân viên chi nhánh (0)", true);
            _btnTabMembers = CreateSubTabButton("👥  Hội viên & Học viên (0)", false);
            _btnTabFacility = CreateSubTabButton("🏢  Cơ sở vật chất & Tiện ích", false);

            _btnTabStaff.Click += (s, e) => SwitchDetailTab(0);
            _btnTabMembers.Click += (s, e) => SwitchDetailTab(1);
            _btnTabFacility.Click += (s, e) => SwitchDetailTab(2);

            flp.Controls.Add(_btnTabStaff);
            flp.Controls.Add(_btnTabMembers);
            flp.Controls.Add(_btnTabFacility);

            pnl.Controls.Add(flp);
            return pnl;
        }

        private Button CreateSubTabButton(string text, bool isActive)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = isActive ? Orange : MutedColor,
                BackColor = isActive ? ItemBg : Color.Transparent,
                Height = 38,
                AutoSize = true,
                Padding = new Padding(16, 0, 16, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderColor = isActive ? Orange : Border;
            btn.FlatAppearance.BorderSize = 1;
            return btn;
        }

        private void SwitchDetailTab(int index)
        {
            _selectedDetailTab = index;

            _btnTabStaff.ForeColor = (index == 0) ? Orange : MutedColor;
            _btnTabStaff.BackColor = (index == 0) ? ItemBg : Color.Transparent;
            _btnTabStaff.FlatAppearance.BorderColor = (index == 0) ? Orange : Border;

            _btnTabMembers.ForeColor = (index == 1) ? Orange : MutedColor;
            _btnTabMembers.BackColor = (index == 1) ? ItemBg : Color.Transparent;
            _btnTabMembers.FlatAppearance.BorderColor = (index == 1) ? Orange : Border;

            _btnTabFacility.ForeColor = (index == 2) ? Orange : MutedColor;
            _btnTabFacility.BackColor = (index == 2) ? ItemBg : Color.Transparent;
            _btnTabFacility.FlatAppearance.BorderColor = (index == 2) ? Orange : Border;

            _pnlTabContainer.Controls.Clear();
            if (index == 0)
            {
                var pnlStaffView = BuildStaffTabWrapper();
                _pnlTabContainer.Controls.Add(pnlStaffView);
                RefreshStaffGrid();
            }
            else if (index == 1)
            {
                var pnlMemberView = BuildMemberTabWrapper();
                _pnlTabContainer.Controls.Add(pnlMemberView);
                RefreshMemberGrid();
            }
            else
            {
                _pnlTabContainer.Controls.Add(_pnlFacilityContent);
                RefreshFacilityContent();
            }
        }

        // ---------------------------------------------------------------------
        // TAB 1: NHÂN VIÊN CHI NHÁNH
        // ---------------------------------------------------------------------
        private Panel BuildStaffTabWrapper()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };

            // Sub Toolbar
            var pnlTool = new Panel { Dock = DockStyle.Top, Height = 42, BackColor = Color.Transparent, Margin = new Padding(0, 0, 0, 6) };

            var btnAddStaff = new Button
            {
                Text = "➕  Thêm nhân viên",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(180, 34),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                UseVisualStyleBackColor = false
            };
            btnAddStaff.FlatAppearance.BorderSize = 0;
            btnAddStaff.Click += BtnAddStaffToBranch_Click;

            var flpLeft = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };

            // Search box
            var pnlSearch = new Panel
            {
                Size = new Size(240, 34),
                BackColor = ItemBg,
                Padding = new Padding(8, 7, 8, 4),
                Margin = new Padding(0, 0, 10, 0)
            };
            pnlSearch.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlSearch.Width - 1, pnlSearch.Height - 1);
            };

            _txtStaffSearch = new TextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                PlaceholderText = "🔍 Tìm nhân viên..."
            };
            _txtStaffSearch.TextChanged += (s, e) => RefreshStaffGrid();
            pnlSearch.Controls.Add(_txtStaffSearch);

            // Filter Role
            _ddStaffRole = new FilterDropdown
            {
                Size = new Size(145, 34),
                Margin = new Padding(0)
            };
            _ddStaffRole.SetItems(new[] { "Tất cả vị trí", "PT", "Thu ngân", "Tạp vụ", "Bảo vệ" });
            _ddStaffRole.SelectedIndexChanged += (s, e) => RefreshStaffGrid();

            flpLeft.Controls.Add(pnlSearch);
            flpLeft.Controls.Add(_ddStaffRole);

            pnlTool.Controls.Add(btnAddStaff);
            pnlTool.Controls.Add(flpLeft);

            pnl.Controls.Add(_gridStaff);
            pnl.Controls.Add(pnlTool);
            return pnl;
        }

        private DataGridView BuildStaffGridView()
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = ItemBg,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(39, 39, 42),
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowTemplate = { Height = 40 }
            };

            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(28, 28, 32),
                ForeColor = MutedColor,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                SelectionBackColor = Color.FromArgb(28, 28, 32),
                Padding = new Padding(8, 0, 0, 0)
            };
            dgv.ColumnHeadersHeight = 36;

            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9F),
                SelectionBackColor = Color.FromArgb(39, 39, 45),
                SelectionForeColor = Color.White,
                Padding = new Padding(8, 0, 0, 0)
            };

            dgv.Columns.Add("StaffId", "Mã NV");
            dgv.Columns["StaffId"]!.Width = 120;

            dgv.Columns.Add("Name", "Họ và tên");
            dgv.Columns["Name"]!.Width = 190;

            dgv.Columns.Add("Role", "Chức vụ");
            dgv.Columns["Role"]!.Width = 110;

            dgv.Columns.Add("Phone", "Số điện thoại");
            dgv.Columns["Phone"]!.Width = 130;

            dgv.Columns.Add("Shift", "Ca làm việc");
            dgv.Columns["Shift"]!.Width = 190;

            dgv.Columns.Add("Status", "Trạng thái");
            dgv.Columns["Status"]!.Width = 160;

            // Nút thao tác xóa
            var btnCol = new DataGridViewButtonColumn
            {
                Name = "Action",
                HeaderText = "Thao tác",
                Text = "🗑️ Xóa",
                UseColumnTextForButtonValue = true,
                Width = 90,
                FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(btnCol);

            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dgv.Columns["Action"]!.Index)
                {
                    string id = dgv.Rows[e.RowIndex].Cells["StaffId"].Value?.ToString() ?? "";
                    string name = dgv.Rows[e.RowIndex].Cells["Name"].Value?.ToString() ?? "";
                    if (MessageBox.Show($"Xác nhận xóa nhân viên '{name}' ({id}) khỏi chi nhánh {_selectedBranch?.Name}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (_selectedBranch != null && _branchStaffMap.ContainsKey(_selectedBranch.Id))
                        {
                            _branchStaffMap[_selectedBranch.Id].RemoveAll(x => x.StaffId == id);
                            _selectedBranch.StaffCount = _branchStaffMap[_selectedBranch.Id].Count;
                            RefreshStaffGrid();
                            UpdateHeaderAndPills();
                        }
                    }
                }
            };

            return dgv;
        }

        private void RefreshStaffGrid()
        {
            if (_gridStaff == null || _selectedBranch == null) return;

            _gridStaff.Rows.Clear();
            if (!_branchStaffMap.ContainsKey(_selectedBranch.Id)) return;

            var list = _branchStaffMap[_selectedBranch.Id].AsEnumerable();

            string q = _txtStaffSearch?.Text.Trim().ToLower() ?? "";
            if (!string.IsNullOrEmpty(q))
            {
                list = list.Where(x => x.Name.ToLower().Contains(q) || x.StaffId.ToLower().Contains(q) || x.Phone.Contains(q));
            }

            int roleIdx = _ddStaffRole?.SelectedIndex ?? 0;
            if (roleIdx > 0)
            {
                string r = _ddStaffRole!.SelectedText;
                list = list.Where(x => x.Role.Equals(r, StringComparison.OrdinalIgnoreCase));
            }

            foreach (var st in list)
            {
                int rIdx = _gridStaff.Rows.Add(st.StaffId, st.Name, st.Role, st.Phone, st.Shift, st.IsWorking ? "🟢 Đang làm việc" : "⚪ Nghỉ phép");
                var row = _gridStaff.Rows[rIdx];
                if (st.Role == "PT") row.Cells["Role"].Style.ForeColor = Orange;
                else if (st.Role == "Thu ngân") row.Cells["Role"].Style.ForeColor = Green;
                else if (st.Role == "Tạp vụ") row.Cells["Role"].Style.ForeColor = Purple;
                else row.Cells["Role"].Style.ForeColor = Blue;
            }

            _btnTabStaff.Text = $"👔  Nhân viên chi nhánh ({_branchStaffMap[_selectedBranch.Id].Count})";
        }

        // ---------------------------------------------------------------------
        // TAB 2: HỌC VIÊN & HỘI VIÊN CHI NHÁNH
        // ---------------------------------------------------------------------
        private Panel BuildMemberTabWrapper()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };

            // Sub Toolbar
            var pnlTool = new Panel { Dock = DockStyle.Top, Height = 42, BackColor = Color.Transparent, Margin = new Padding(0, 0, 0, 6) };

            var btnAddMember = new Button
            {
                Text = "➕  Đăng ký học viên",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(180, 34),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                UseVisualStyleBackColor = false
            };
            btnAddMember.FlatAppearance.BorderSize = 0;
            btnAddMember.Click += BtnAddMemberToBranch_Click;

            var flpLeft = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };

            // Search box
            var pnlSearch = new Panel
            {
                Size = new Size(240, 34),
                BackColor = ItemBg,
                Padding = new Padding(8, 7, 8, 4),
                Margin = new Padding(0, 0, 10, 0)
            };
            pnlSearch.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlSearch.Width - 1, pnlSearch.Height - 1);
            };

            _txtMemberSearch = new TextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                PlaceholderText = "🔍 Tìm học viên theo tên/SĐT..."
            };
            _txtMemberSearch.TextChanged += (s, e) => RefreshMemberGrid();
            pnlSearch.Controls.Add(_txtMemberSearch);

            // Filter Package
            _ddMemberPackage = new FilterDropdown
            {
                Size = new Size(160, 34),
                Margin = new Padding(0)
            };
            _ddMemberPackage.SetItems(new[] { "Tất cả gói tập", "VIP Diamond", "Platinum", "Classic", "PT 1-1" });
            _ddMemberPackage.SelectedIndexChanged += (s, e) => RefreshMemberGrid();

            flpLeft.Controls.Add(pnlSearch);
            flpLeft.Controls.Add(_ddMemberPackage);

            pnlTool.Controls.Add(btnAddMember);
            pnlTool.Controls.Add(flpLeft);

            pnl.Controls.Add(_gridMembers);
            pnl.Controls.Add(pnlTool);
            return pnl;
        }

        private DataGridView BuildMemberGridView()
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = ItemBg,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(39, 39, 42),
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowTemplate = { Height = 40 }
            };

            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(28, 28, 32),
                ForeColor = MutedColor,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                SelectionBackColor = Color.FromArgb(28, 28, 32),
                Padding = new Padding(8, 0, 0, 0)
            };
            dgv.ColumnHeadersHeight = 36;

            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9F),
                SelectionBackColor = Color.FromArgb(39, 39, 45),
                SelectionForeColor = Color.White,
                Padding = new Padding(8, 0, 0, 0)
            };

            dgv.Columns.Add("MemberId", "Mã HV");
            dgv.Columns["MemberId"]!.Width = 110;

            dgv.Columns.Add("Name", "Họ và tên");
            dgv.Columns["Name"]!.Width = 180;

            dgv.Columns.Add("Phone", "Số điện thoại");
            dgv.Columns["Phone"]!.Width = 130;

            dgv.Columns.Add("Package", "Gói tập");
            dgv.Columns["Package"]!.Width = 180;

            dgv.Columns.Add("PT", "HLV phụ trách");
            dgv.Columns["PT"]!.Width = 140;

            dgv.Columns.Add("Expire", "Hạn gói");
            dgv.Columns["Expire"]!.Width = 110;

            dgv.Columns.Add("Status", "Trạng thái");
            dgv.Columns["Status"]!.Width = 150;

            var btnCol = new DataGridViewButtonColumn
            {
                Name = "Action",
                HeaderText = "Thao tác",
                Text = "🗑️ Xóa",
                UseColumnTextForButtonValue = true,
                Width = 90,
                FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(btnCol);

            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dgv.Columns["Action"]!.Index)
                {
                    string id = dgv.Rows[e.RowIndex].Cells["MemberId"].Value?.ToString() ?? "";
                    string name = dgv.Rows[e.RowIndex].Cells["Name"].Value?.ToString() ?? "";
                    if (MessageBox.Show($"Xác nhận xóa học viên '{name}' ({id}) khỏi cơ sở {_selectedBranch?.Name}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (_selectedBranch != null && _branchMemberMap.ContainsKey(_selectedBranch.Id))
                        {
                            _branchMemberMap[_selectedBranch.Id].RemoveAll(x => x.MemberId == id);
                            _selectedBranch.MemberCount = _branchMemberMap[_selectedBranch.Id].Count;
                            RefreshMemberGrid();
                            UpdateHeaderAndPills();
                        }
                    }
                }
            };

            return dgv;
        }

        private void RefreshMemberGrid()
        {
            if (_gridMembers == null || _selectedBranch == null) return;

            _gridMembers.Rows.Clear();
            if (!_branchMemberMap.ContainsKey(_selectedBranch.Id)) return;

            var list = _branchMemberMap[_selectedBranch.Id].AsEnumerable();

            string q = _txtMemberSearch?.Text.Trim().ToLower() ?? "";
            if (!string.IsNullOrEmpty(q))
            {
                list = list.Where(x => x.Name.ToLower().Contains(q) || x.MemberId.ToLower().Contains(q) || x.Phone.Contains(q));
            }

            int pIdx = _ddMemberPackage?.SelectedIndex ?? 0;
            if (pIdx > 0)
            {
                string pkg = _ddMemberPackage!.SelectedText;
                list = list.Where(x => x.Package.Contains(pkg));
            }

            foreach (var m in list)
            {
                int rIdx = _gridMembers.Rows.Add(m.MemberId, m.Name, m.Phone, m.Package, m.AssignedPt, m.ExpireDate, m.Status);
                var row = _gridMembers.Rows[rIdx];
                if (m.Status == "Đang tập") row.Cells["Status"].Style.ForeColor = Green;
                else if (m.Status == "Sắp hết hạn") row.Cells["Status"].Style.ForeColor = Orange;
                else row.Cells["Status"].Style.ForeColor = Red;
            }

            _btnTabMembers.Text = $"👥  Hội viên & Học viên ({_branchMemberMap[_selectedBranch.Id].Count})";
        }

        // ---------------------------------------------------------------------
        // TAB 3: CƠ SỞ VẬT CHẤT & TIỆN ÍCH
        // ---------------------------------------------------------------------
        private Panel BuildFacilitySection()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ItemBg,
                Padding = new Padding(20, 16, 20, 16)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            return pnl;
        }

        private void RefreshFacilityContent()
        {
            _pnlFacilityContent.Controls.Clear();
            if (_selectedBranch == null) return;

            var stack = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 3,
                BackColor = Color.Transparent
            };
            stack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            stack.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            stack.Controls.Add(CreateFacilityCard("📐 DIỆN TÍCH HOẠT ĐỘNG", $"{_selectedBranch.AreaSqM:N0} m² sàn tập luyện", "Thiết kế không gian mở, trang bị máy lạnh công nghiệp & lọc không khí HEPA."), 0, 0);
            stack.Controls.Add(CreateFacilityCard("🏊 TIỆN ÍCH ĐẶC QUYỀN", "Bể bơi • Sauna • Boxing Ring", "Phòng xông hơi khô/ướt đá muối Himalaya, bể bơi 4 mùa nước ấm."), 1, 0);

            stack.Controls.Add(CreateFacilityCard("🅿️ ĐỖ XE & AN NINH", "Bãi giữ xe ô tô & xe máy miễn phí", "Hệ thống Camera AI 24/7, thẻ từ thông minh SmartLock kiểm soát ra vào."), 0, 1);
            stack.Controls.Add(CreateFacilityCard("🥤 QUẦY BAR NƯỚC & PHỤ KIỆN", "Protein Shake & Bổ sung điện giải", "Phục vụ thức uống thể thao, khăn tập miễn phí và tủ đồ cá nhân."), 1, 1);

            stack.Controls.Add(CreateFacilityCard("📋 GHI CHÚ VẬN HÀNH", _selectedBranch.Note, "Liên hệ quản lý cơ sở khi có sự cố kỹ thuật hoặc bảo trì thiết bị."), 0, 2);
            stack.SetColumnSpan(stack.GetControlFromPosition(0, 2)!, 2);

            _pnlFacilityContent.Controls.Add(stack);
        }

        private Panel CreateFacilityCard(string title, string mainText, string subText)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Margin = new Padding(6, 6, 6, 6),
                Padding = new Padding(14, 12, 14, 12)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            var lblT = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Orange,
                Dock = DockStyle.Top,
                Height = 20
            };

            var lblM = new Label
            {
                Text = mainText,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = TextColor,
                Dock = DockStyle.Top,
                Height = 24
            };

            var lblS = new Label
            {
                Text = subText,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Dock = DockStyle.Fill
            };

            pnl.Controls.Add(lblS);
            pnl.Controls.Add(lblM);
            pnl.Controls.Add(lblT);
            return pnl;
        }

        // =====================================================================
        // LOGIC CHỌN VÀ HIỂN THỊ DỮ LIỆU CHI NHÁNH
        // =====================================================================
        private void SelectBranch(BranchModel? branch)
        {
            _selectedBranch = branch;
            UpdateHeaderAndPills();
            RefreshBranchCardsSelection();

            if (_selectedDetailTab == 0) RefreshStaffGrid();
            else if (_selectedDetailTab == 1) RefreshMemberGrid();
            else RefreshFacilityContent();
        }

        private void UpdateHeaderAndPills()
        {
            if (_selectedBranch == null)
            {
                _lblDetailName.Text = "Chưa chọn chi nhánh";
                _lblDetailCode.Text = "---";
                _lblDetailAddress.Text = "📍 ...";
                _lblDetailPhone.Text = "📞 ...";
                _lblDetailEmail.Text = "✉️ ...";
                _lblDetailManager.Text = "👔 ...";
                _btnToggleBranchStatus.Enabled = false;
                _btnEditBranch.Enabled = false;
                _btnDeleteBranch.Enabled = false;
                return;
            }

            _btnToggleBranchStatus.Enabled = true;
            _btnEditBranch.Enabled = true;
            _btnDeleteBranch.Enabled = true;

            _lblDetailName.Text = _selectedBranch.Name;
            _lblDetailCode.Text = _selectedBranch.Code;
            _lblDetailAddress.Text = $"📍 Địa chỉ: {_selectedBranch.Address}";
            _lblDetailPhone.Text = $"📞 Hotline: {_selectedBranch.Phone}";
            _lblDetailEmail.Text = $"✉️ Email: {_selectedBranch.Email}";
            _lblDetailManager.Text = $"👔 Quản lý cơ sở: {_selectedBranch.Manager}";

            if (_selectedBranch.IsOnline)
            {
                _lblDetailStatusBadge.Text = "🟢 ĐANG HOẠT ĐỘNG (ONLINE)";
                _lblDetailStatusBadge.ForeColor = Green;
                _lblDetailStatusBadge.BackColor = Color.FromArgb(20, 40, 25);

                _btnToggleBranchStatus.Text = "⏸️  Tắt chi nhánh (Offline)";
                _btnToggleBranchStatus.ForeColor = Red;
                _btnToggleBranchStatus.BackColor = Color.FromArgb(45, 20, 20);
                _btnToggleBranchStatus.FlatAppearance.BorderColor = Red;
            }
            else
            {
                _lblDetailStatusBadge.Text = "🔴 TẠM ĐÓNG CỬA (OFFLINE)";
                _lblDetailStatusBadge.ForeColor = Red;
                _lblDetailStatusBadge.BackColor = Color.FromArgb(45, 20, 20);

                _btnToggleBranchStatus.Text = "⚡  Bật chi nhánh (Online)";
                _btnToggleBranchStatus.ForeColor = Color.White;
                _btnToggleBranchStatus.BackColor = Green;
                _btnToggleBranchStatus.FlatAppearance.BorderColor = Green;
            }

            int mCount = _branchMemberMap.ContainsKey(_selectedBranch.Id) ? _branchMemberMap[_selectedBranch.Id].Count : _selectedBranch.MemberCount;
            int sCount = _branchStaffMap.ContainsKey(_selectedBranch.Id) ? _branchStaffMap[_selectedBranch.Id].Count : _selectedBranch.StaffCount;

            _lblKpiMembersVal.Text = $"{mCount:N0} người";
            _lblKpiStaffVal.Text = $"{sCount:N0} nhân viên";
            _lblKpiHoursVal.Text = $"{_selectedBranch.OpenTime} - {_selectedBranch.CloseTime}";
            _lblKpiRevenueVal.Text = _selectedBranch.MonthlyRevenue.ToString("N0", ViCulture) + " ₫";

            _btnTabStaff.Text = $"👔  Nhân viên chi nhánh ({sCount})";
            _btnTabMembers.Text = $"👥  Hội viên & Học viên ({mCount})";

            // Cập nhật thống kê toàn hệ thống ở Top Header
            int totalBranches = _branches.Count;
            int onlineBranches = _branches.Count(b => b.IsOnline);
            int offlineBranches = totalBranches - onlineBranches;
            int totalMembers = _branches.Sum(b => _branchMemberMap.ContainsKey(b.Id) ? _branchMemberMap[b.Id].Count : b.MemberCount);
            int totalStaff = _branches.Sum(b => _branchStaffMap.ContainsKey(b.Id) ? _branchStaffMap[b.Id].Count : b.StaffCount);

            _lblSummaryBranches.Text = $"🏢 {totalBranches} Chi nhánh";
            _lblSummaryOnline.Text = $"🟢 {onlineBranches} Đang mở (ONL)";
            _lblSummaryOffline.Text = $"🔴 {offlineBranches} Tạm đóng (OFF)";
            _lblSummaryMembers.Text = $"👥 {totalMembers:N0} Học viên";
            _lblSummaryStaff.Text = $"👔 {totalStaff} Nhân sự";
        }

        // =====================================================================
        // REFRESH DANH SÁCH THẺ CHI NHÁNH CỘT TRÁI
        // =====================================================================
        private void RefreshBranchList()
        {
            _flpBranchCards.Controls.Clear();

            var query = _branches.AsEnumerable();

            // Bộ lọc trạng thái
            if (_branchFilterMode == 1) query = query.Where(b => b.IsOnline);
            else if (_branchFilterMode == 2) query = query.Where(b => !b.IsOnline);

            // Tìm kiếm
            if (!string.IsNullOrEmpty(_branchSearchText))
            {
                string s = _branchSearchText.ToLower();
                query = query.Where(b => b.Name.ToLower().Contains(s) || b.Address.ToLower().Contains(s) || b.Code.ToLower().Contains(s));
            }

            var list = query.ToList();
            foreach (var b in list)
            {
                var card = CreateBranchStationCard(b);
                _flpBranchCards.Controls.Add(card);
            }

            // Nếu chi nhánh đang chọn không còn nằm trong danh sách lọc, chọn chi nhánh đầu tiên
            if (_selectedBranch != null && !list.Any(x => x.Id == _selectedBranch.Id))
            {
                SelectBranch(list.FirstOrDefault());
            }
        }

        private Control CreateBranchStationCard(BranchModel b)
        {
            bool isSelected = (_selectedBranch?.Id == b.Id);

            var pnl = new Panel
            {
                Width = 350,
                Height = 118,
                BackColor = isSelected ? Color.FromArgb(32, 32, 38) : ItemBg,
                Margin = new Padding(0, 0, 0, 10),
                Cursor = Cursors.Hand,
                Tag = b
            };

            pnl.Paint += (s, e) =>
            {
                var g = e.Graphics;
                using var pBorder = new Pen(isSelected ? Orange : Border, isSelected ? 1.5f : 1f);
                g.DrawRectangle(pBorder, 0, 0, pnl.Width - 1, pnl.Height - 1);

                // Dải màu cam nổi bật mép trái nếu được chọn
                if (isSelected)
                {
                    using var bBar = new SolidBrush(Orange);
                    g.FillRectangle(bBar, 0, 0, 4, pnl.Height);
                }
            };

            // Dòng 1: Tên chi nhánh + Mã CN
            var lblName = new Label
            {
                Text = b.Name,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = isSelected ? Color.White : TextColor,
                Location = new Point(12, 10),
                Size = new Size(260, 20),
                AutoEllipsis = true,
                UseMnemonic = false
            };

            var lblCode = new Label
            {
                Text = b.Code,
                Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                ForeColor = Orange,
                BackColor = Color.FromArgb(40, 28, 20),
                Padding = new Padding(4, 2, 4, 2),
                Location = new Point(280, 8),
                AutoSize = true,
                UseMnemonic = false
            };

            // Dòng 2: Địa chỉ
            var lblAddr = new Label
            {
                Text = "📍 " + b.Address,
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = MutedColor,
                Location = new Point(12, 32),
                Size = new Size(325, 18),
                AutoEllipsis = true,
                UseMnemonic = false
            };

            // Dòng 3: Dải trạng thái + Nút bật/tắt Onl/Off nhanh
            var lblStatus = new Label
            {
                Text = b.IsOnline ? "🟢 ONLINE (MỞ CỬA)" : "🔴 OFFLINE (ĐÓNG)",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = b.IsOnline ? Green : Red,
                BackColor = b.IsOnline ? Color.FromArgb(20, 40, 25) : Color.FromArgb(45, 20, 20),
                Padding = new Padding(6, 2, 6, 2),
                Location = new Point(12, 54),
                AutoSize = true,
                UseMnemonic = false
            };

            var btnQuickToggle = new Button
            {
                Text = b.IsOnline ? "⏸️ Tắt" : "⚡ Bật",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = b.IsOnline ? Red : Green,
                BackColor = Color.Transparent,
                Size = new Size(72, 26),
                Location = new Point(265, 52),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                UseVisualStyleBackColor = false
            };
            btnQuickToggle.FlatAppearance.BorderColor = b.IsOnline ? Red : Green;
            btnQuickToggle.FlatAppearance.BorderSize = 1;
            btnQuickToggle.Click += (s, e) =>
            {
                b.IsOnline = !b.IsOnline;
                UpdateHeaderAndPills();
                RefreshBranchList();
            };

            // Dòng 4: Chỉ số phụ
            int mCount = _branchMemberMap.ContainsKey(b.Id) ? _branchMemberMap[b.Id].Count : b.MemberCount;
            int sCount = _branchStaffMap.ContainsKey(b.Id) ? _branchStaffMap[b.Id].Count : b.StaffCount;
            var lblStats = new Label
            {
                Text = $"👥 {mCount} học viên  •  👔 {sCount} nhân sự  •  📐 {b.AreaSqM:N0} m²",
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = DimColor,
                Location = new Point(12, 88),
                AutoSize = true
            };

            // Sự kiện click chọn chi nhánh
            Action clickAction = () => SelectBranch(b);
            pnl.Click += (s, e) => clickAction();
            lblName.Click += (s, e) => clickAction();
            lblAddr.Click += (s, e) => clickAction();
            lblStats.Click += (s, e) => clickAction();

            pnl.Controls.Add(lblCode);
            pnl.Controls.Add(lblName);
            pnl.Controls.Add(lblAddr);
            pnl.Controls.Add(lblStatus);
            pnl.Controls.Add(btnQuickToggle);
            pnl.Controls.Add(lblStats);

            return pnl;
        }

        private void RefreshBranchCardsSelection()
        {
            foreach (Control c in _flpBranchCards.Controls)
            {
                if (c is Panel pnl && pnl.Tag is BranchModel b)
                {
                    bool isSel = (_selectedBranch?.Id == b.Id);
                    pnl.BackColor = isSel ? Color.FromArgb(32, 32, 38) : ItemBg;
                    pnl.Invalidate();
                }
            }
        }

        // =====================================================================
        // THAO TÁC CHI NHÁNH: BẬT/TẮT, THÊM, SỬA, XÓA
        // =====================================================================
        private void ToggleCurrentBranchStatus()
        {
            if (_selectedBranch == null) return;

            string actionName = _selectedBranch.IsOnline ? "TẠM ĐÓNG CỬA (OFFLINE)" : "BẬT HOẠT ĐỘNG (ONLINE)";
            string prompt = _selectedBranch.IsOnline
                ? $"Bạn có chắc chắn muốn TẠM ĐÓNG cửa chi nhánh '{_selectedBranch.Name}'?\n(Hệ thống sẽ chuyển chi nhánh sang trạng thái bảo trì/ngoại tuyến)"
                : $"Bạn có chắc chắn muốn MỞ CỬA HOẠT ĐỘNG lại cho chi nhánh '{_selectedBranch.Name}'?";

            if (MessageBox.Show(prompt, "Xác nhận chuyển trạng thái", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _selectedBranch.IsOnline = !_selectedBranch.IsOnline;
                UpdateHeaderAndPills();
                RefreshBranchList();
                MessageBox.Show($"Đã chuyển trạng thái chi nhánh '{_selectedBranch.Name}' thành: {actionName} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAddBranch_Click(object? sender, EventArgs e)
        {
            using var dlg = new frmBranchEdit(null);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ResultBranch != null)
            {
                _branches.Add(dlg.ResultBranch);
                _branchStaffMap[dlg.ResultBranch.Id] = new List<BranchStaffItem>();
                _branchMemberMap[dlg.ResultBranch.Id] = new List<BranchMemberItem>();

                // Tự động gán 2 nhân viên mặc định và 3 học viên thử nghiệm
                SeedStaffData(dlg.ResultBranch.Id, dlg.ResultBranch.Code);
                SeedMemberData(dlg.ResultBranch.Id);

                RefreshBranchList();
                SelectBranch(dlg.ResultBranch);
                MessageBox.Show($"Thêm mới chi nhánh '{dlg.ResultBranch.Name}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnEditBranch_Click(object? sender, EventArgs e)
        {
            if (_selectedBranch == null) return;

            using var dlg = new frmBranchEdit(_selectedBranch);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ResultBranch != null)
            {
                UpdateHeaderAndPills();
                RefreshBranchList();
                MessageBox.Show($"Cập nhật thông tin chi nhánh '{_selectedBranch.Name}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDeleteBranch_Click(object? sender, EventArgs e)
        {
            if (_selectedBranch == null) return;

            if (_branches.Count <= 1)
            {
                MessageBox.Show("Hệ thống phải duy trì ít nhất 1 chi nhánh, không thể xóa chi nhánh duy nhất này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int mCount = _branchMemberMap.ContainsKey(_selectedBranch.Id) ? _branchMemberMap[_selectedBranch.Id].Count : 0;
            int sCount = _branchStaffMap.ContainsKey(_selectedBranch.Id) ? _branchStaffMap[_selectedBranch.Id].Count : 0;

            string warning = $"Xác nhận XÓA VĨNH VIỄN chi nhánh:\n👉 '{_selectedBranch.Name}' ({_selectedBranch.Code})?\n\nHiện chi nhánh đang có: {mCount} học viên và {sCount} nhân sự.";

            if (MessageBox.Show(warning, "Cảnh báo xóa chi nhánh", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int delId = _selectedBranch.Id;
                _branches.RemoveAll(x => x.Id == delId);
                _branchStaffMap.Remove(delId);
                _branchMemberMap.Remove(delId);

                RefreshBranchList();
                SelectBranch(_branches.FirstOrDefault());
                MessageBox.Show("Đã xóa chi nhánh thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // =====================================================================
        // THAO TÁC THÊM NHÂN VIÊN & HỌC VIÊN
        // =====================================================================
        private void BtnAddStaffToBranch_Click(object? sender, EventArgs e)
        {
            if (_selectedBranch == null) return;

            using var dlg = new frmBranchStaffAdd(_selectedBranch.Name);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ResultStaff != null)
            {
                if (!_branchStaffMap.ContainsKey(_selectedBranch.Id))
                    _branchStaffMap[_selectedBranch.Id] = new List<BranchStaffItem>();

                _branchStaffMap[_selectedBranch.Id].Add(dlg.ResultStaff);
                _selectedBranch.StaffCount = _branchStaffMap[_selectedBranch.Id].Count;
                RefreshStaffGrid();
                UpdateHeaderAndPills();
                MessageBox.Show($"Đã phân công nhân viên '{dlg.ResultStaff.Name}' vào chi nhánh {_selectedBranch.Name} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAddMemberToBranch_Click(object? sender, EventArgs e)
        {
            if (_selectedBranch == null) return;

            using var dlg = new frmBranchMemberAdd(_selectedBranch.Name);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ResultMember != null)
            {
                if (!_branchMemberMap.ContainsKey(_selectedBranch.Id))
                    _branchMemberMap[_selectedBranch.Id] = new List<BranchMemberItem>();

                _branchMemberMap[_selectedBranch.Id].Add(dlg.ResultMember);
                _selectedBranch.MemberCount = _branchMemberMap[_selectedBranch.Id].Count;
                RefreshMemberGrid();
                UpdateHeaderAndPills();
                MessageBox.Show($"Đã đăng ký học viên '{dlg.ResultMember.Name}' tại chi nhánh {_selectedBranch.Name} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
