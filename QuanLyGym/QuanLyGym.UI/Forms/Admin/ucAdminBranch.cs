using System;
using System.Collections.Generic;
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
        // DATA REPOSITORY
        // =====================================================================
        private readonly List<BranchModel> _branches = new List<BranchModel>();
        private readonly Dictionary<int, List<BranchStaffItem>> _branchStaffMap = new Dictionary<int, List<BranchStaffItem>>();
        private readonly Dictionary<int, List<BranchMemberItem>> _branchMemberMap = new Dictionary<int, List<BranchMemberItem>>();

        // Filter & Search states
        private int _statusFilter = 0; // 0: Tất cả, 1: Online, 2: Offline
        private string _searchText = string.Empty;
        private string _regionFilter = "Tất cả khu vực";
        private int _sortMode = 0; // 0: Mặc định, 1: Doanh thu cao nhất, 2: Học viên đông nhất, 3: Diện tích lớn nhất
        private bool _isInitializing = true;

        // KPI Value Labels
        private Label _lblKpiTotalVal = null!;
        private Label _lblKpiOnlineVal = null!;
        private Label _lblKpiOfflineVal = null!;
        private Label _lblKpiMembersVal = null!;
        private Label _lblKpiStaffVal = null!;

        // Quick Tabs
        private Button _btnTabAll = null!;
        private Button _btnTabOnline = null!;
        private Button _btnTabOffline = null!;

        // Toolbar
        private TextBox _txtSearch = null!;
        private FilterDropdown _ddRegion = null!;
        private FilterDropdown _ddSort = null!;
        private Label _lblResultCount = null!;

        // Main Grid
        private DataGridView _grid = null!;

        public ucAdminBranch()
        {
            InitializeData();
            BuildUI();
            _isInitializing = false;
            UpdateKpiCards();
            RefreshBranchGrid();
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
                Manager = "Đặng Quang Huy",
                OpenTime = "05:30",
                CloseTime = "22:00",
                AreaSqM = 1800,
                IsOnline = true,
                Note = "Tổ hợp thể thao diện tích lớn, chuyên về cử tạ Powerlifting, khu Functional Training và Crossfit",
                MemberCount = 480,
                StaffCount = 14,
                MonthlyRevenue = 410000000
            };

            var b4 = new BranchModel
            {
                Id = 4,
                Code = "CN-BT",
                Name = "GymFit Bình Thạnh - Landmark",
                Address = "Số 208 Nguyễn Hữu Cảnh, P. 22, Q. Bình Thạnh, TP.HCM",
                Phone = "028 3514 8899",
                Email = "binhthanh@gymfit.vn",
                Manager = "Lê Thị Mai Chi",
                OpenTime = "06:00",
                CloseTime = "23:00",
                AreaSqM = 1100,
                IsOnline = false, // Tạm đóng bảo dưỡng
                Note = "Đang tiến hành bảo trì định kỳ hệ thống điều hòa trung tâm & nâng cấp sàn gỗ phòng Yoga Studio",
                MemberCount = 230,
                StaffCount = 9,
                MonthlyRevenue = 195000000
            };

            var b5 = new BranchModel
            {
                Id = 5,
                Code = "CN-HC",
                Name = "GymFit Hải Châu - Bạch Đằng",
                Address = "Số 150 Bạch Đằng, P. Hải Châu 1, Q. Hải Châu, Đà Nẵng",
                Phone = "0236 3844 555",
                Email = "danang@gymfit.vn",
                Manager = "Vũ Đình Trọng",
                OpenTime = "05:30",
                CloseTime = "21:30",
                AreaSqM = 950,
                IsOnline = true,
                Note = "View sông Hàn trực diện, không gian luyện tập ngập tràn ánh sáng tự nhiên và quầy Detox Bar cao cấp",
                MemberCount = 210,
                StaffCount = 8,
                MonthlyRevenue = 160000000
            };

            _branches.AddRange(new[] { b1, b2, b3, b4, b5 });

            // Seed nhân viên
            SeedStaffData(b1.Id, "CN-Q1");
            SeedStaffData(b2.Id, "CN-HK");
            SeedStaffData(b3.Id, "CN-CG");
            SeedStaffData(b4.Id, "CN-BT");
            SeedStaffData(b5.Id, "CN-HC");

            // Seed học viên
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
        // XÂY DỰNG TOÀN BỘ GIAO DIỆN CHÍNH (ĐƠN MẶT PHẲNG, TINH GỌN, KHÔNG RỐI)
        // =====================================================================
        private void BuildUI()
        {
            BackColor = PageBg;
            Dock = DockStyle.Fill;
            Font = new Font("Segoe UI", 9F);
            Padding = new Padding(14, 10, 14, 12);

            // A. Top Header
            var pnlTopBar = BuildTopCommandBar();
            var pnlGap1 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // B. KPI Summary Strip (5 cards)
            var pnlKpis = BuildKpiStrip();
            var pnlGap2 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // C. Quick Status Filter Tabs
            var pnlTabs = BuildStatusTabsSection();
            var pnlGap3 = new Panel { Dock = DockStyle.Top, Height = 8, BackColor = Color.Transparent };

            // D. Search & Filter Toolbar
            var pnlToolbar = BuildFilterToolbar();
            var pnlGap4 = new Panel { Dock = DockStyle.Top, Height = 8, BackColor = Color.Transparent };

            // E. Lưới bảng dữ liệu các chi nhánh
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

            // Docking order
            Controls.Add(pnlGridContainer);
            Controls.Add(pnlGap4);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlGap3);
            Controls.Add(pnlTabs);
            Controls.Add(pnlGap2);
            Controls.Add(pnlKpis);
            Controls.Add(pnlGap1);
            Controls.Add(pnlTopBar);
        }

        // ---------------------------------------------------------------------
        // A. THANH ĐIỀU HÀNH TRÊN CÙNG (TOP HEADER)
        // ---------------------------------------------------------------------
        private Panel BuildTopCommandBar()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = CardBg,
                Padding = new Padding(16, 10, 16, 10)
            };
            pnl.Paint += (s, e) =>
            {
                using var pen = new Pen(Border);
                e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            var lblTitle = new Label
            {
                Text = "🏢  QUẢN LÝ HỆ THỐNG CHI NHÁNH & TRẠM VẬN HÀNH",
                Font = new Font("Segoe UI", 12.5F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(14, 8),
                AutoSize = true,
                UseMnemonic = false
            };

            var lblSub = new Label
            {
                Text = "Kiểm soát mạng lưới cơ sở, theo dõi trạng thái hoạt động & phân quyền nhân sự, hội viên",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Location = new Point(16, 32),
                AutoSize = true,
                UseMnemonic = false
            };

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
                UseMnemonic = false,
                UseVisualStyleBackColor = false
            };
            btnAddBranch.FlatAppearance.BorderSize = 0;
            btnAddBranch.FlatAppearance.MouseOverBackColor = OrangeDark;
            btnAddBranch.Click += BtnAddBranch_Click;

            pnl.Controls.Add(btnAddBranch);
            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblSub);
            return pnl;
        }

        // ---------------------------------------------------------------------
        // B. DẢI THỐNG KÊ NHANH (KPI STRIP - 5 CARDS)
        // ---------------------------------------------------------------------
        private Panel BuildKpiStrip()
        {
            var pnlWrapper = new Panel
            {
                Dock = DockStyle.Top,
                Height = 96,
                BackColor = Color.Transparent,
                Padding = new Padding(0)
            };

            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            tbl.Controls.Add(CreateKpiCard("🏢  TỔNG CHI NHÁNH", out _lblKpiTotalVal, "Toàn bộ hệ thống", TextColor), 0, 0);
            tbl.Controls.Add(CreateKpiCard("🟢  ĐANG HOẠT ĐỘNG", out _lblKpiOnlineVal, "Đang mở cửa đón khách", Green), 1, 0);
            tbl.Controls.Add(CreateKpiCard("🔴  TẠM ĐÓNG / BẢO TRÌ", out _lblKpiOfflineVal, "Tạm dừng phục vụ", Red), 2, 0);
            tbl.Controls.Add(CreateKpiCard("👥  TỔNG HỌC VIÊN", out _lblKpiMembersVal, "Hội viên toàn chuỗi", Blue), 3, 0);
            tbl.Controls.Add(CreateKpiCard("👔  ĐỘI NGŨ NHÂN SỰ", out _lblKpiStaffVal, "HLV, Quản lý & Lễ tân", Purple), 4, 0);

            pnlWrapper.Controls.Add(tbl);
            return pnlWrapper;
        }

        private Panel CreateKpiCard(string title, out Label lblVal, string subtitle, Color accentColor)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Margin = new Padding(3, 0, 3, 0),
                Padding = new Padding(14, 10, 14, 8)
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
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = accentColor,
                Dock = DockStyle.Top,
                Height = 32,
                TextAlign = ContentAlignment.MiddleLeft,
                UseMnemonic = false
            };

            var lblSub = new Label
            {
                Text = subtitle,
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = DimColor,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                UseMnemonic = false
            };

            pnl.Controls.Add(lblSub);
            pnl.Controls.Add(lblVal);
            pnl.Controls.Add(lblT);
            return pnl;
        }

        private void UpdateKpiCards()
        {
            int total = _branches.Count;
            int online = _branches.Count(b => b.IsOnline);
            int offline = total - online;
            int members = _branches.Sum(b => _branchMemberMap.ContainsKey(b.Id) ? _branchMemberMap[b.Id].Count : b.MemberCount);
            int staff = _branches.Sum(b => _branchStaffMap.ContainsKey(b.Id) ? _branchStaffMap[b.Id].Count : b.StaffCount);

            _lblKpiTotalVal.Text = $"{total} cơ sở";
            _lblKpiOnlineVal.Text = $"{online} cơ sở";
            _lblKpiOfflineVal.Text = $"{offline} cơ sở";
            _lblKpiMembersVal.Text = $"{members:N0} người";
            _lblKpiStaffVal.Text = $"{staff} nhân sự";

            _btnTabAll.Text = $"🏢  Tất cả chi nhánh ({total})";
            _btnTabOnline.Text = $"🟢  Đang hoạt động ({online})";
            _btnTabOffline.Text = $"🔴  Tạm đóng ({offline})";
        }

        // ---------------------------------------------------------------------
        // C. PHÂN ĐOẠN TAB TRẠNG THÁI (STATUS TABS)
        // ---------------------------------------------------------------------
        private Panel BuildStatusTabsSection()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };

            var flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            _btnTabAll = CreateStatusTabBtn("🏢  Tất cả chi nhánh (0)", true);
            _btnTabOnline = CreateStatusTabBtn("🟢  Đang hoạt động (0)", false);
            _btnTabOffline = CreateStatusTabBtn("🔴  Tạm đóng (0)", false);

            _btnTabAll.Click += (s, e) => SetStatusFilter(0);
            _btnTabOnline.Click += (s, e) => SetStatusFilter(1);
            _btnTabOffline.Click += (s, e) => SetStatusFilter(2);

            flp.Controls.Add(_btnTabAll);
            flp.Controls.Add(_btnTabOnline);
            flp.Controls.Add(_btnTabOffline);

            pnl.Controls.Add(flp);
            return pnl;
        }

        private Button CreateStatusTabBtn(string text, bool isActive)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = isActive ? Orange : MutedColor,
                BackColor = isActive ? CardBg : Color.Transparent,
                Height = 38,
                AutoSize = true,
                Padding = new Padding(18, 0, 18, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                UseVisualStyleBackColor = false
            };
            btn.FlatAppearance.BorderColor = isActive ? Orange : Border;
            btn.FlatAppearance.BorderSize = 1;
            return btn;
        }

        private void SetStatusFilter(int mode)
        {
            _statusFilter = mode;

            _btnTabAll.ForeColor = (mode == 0) ? Orange : MutedColor;
            _btnTabAll.BackColor = (mode == 0) ? CardBg : Color.Transparent;
            _btnTabAll.FlatAppearance.BorderColor = (mode == 0) ? Orange : Border;

            _btnTabOnline.ForeColor = (mode == 1) ? Green : MutedColor;
            _btnTabOnline.BackColor = (mode == 1) ? CardBg : Color.Transparent;
            _btnTabOnline.FlatAppearance.BorderColor = (mode == 1) ? Green : Border;

            _btnTabOffline.ForeColor = (mode == 2) ? Red : MutedColor;
            _btnTabOffline.BackColor = (mode == 2) ? CardBg : Color.Transparent;
            _btnTabOffline.FlatAppearance.BorderColor = (mode == 2) ? Red : Border;

            RefreshBranchGrid();
        }

        // ---------------------------------------------------------------------
        // D. TOOLBAR TÌM KIẾM VÀ BỘ LỌC
        // ---------------------------------------------------------------------
        private Panel BuildFilterToolbar()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = CardBg,
                Padding = new Padding(10, 5, 12, 5)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            // Search Box
            var pnlSearch = new Panel
            {
                Dock = DockStyle.Left,
                Width = 360,
                Height = 34,
                BackColor = ItemBg,
                Padding = new Padding(8, 7, 8, 4),
                Margin = new Padding(0, 0, 10, 0)
            };
            pnlSearch.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlSearch.Width - 1, pnlSearch.Height - 1);
            };

            _txtSearch = new TextBox
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                BackColor = ItemBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                PlaceholderText = "🔍 Tìm chi nhánh theo tên, mã cơ sở, địa chỉ, quản lý..."
            };
            _txtSearch.TextChanged += (s, e) =>
            {
                _searchText = _txtSearch.Text.Trim();
                RefreshBranchGrid();
            };
            pnlSearch.Controls.Add(_txtSearch);

            // Filter controls Flow
            var flpFilters = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Margin = new Padding(10, 0, 0, 0)
            };

            _ddRegion = new FilterDropdown
            {
                Size = new Size(160, 34),
                Margin = new Padding(8, 0, 6, 0)
            };
            _ddRegion.SetItems(new[] { "Tất cả khu vực", "TP. Hồ Chí Minh", "Hà Nội", "Đà Nẵng" });
            _ddRegion.SelectedIndexChanged += (s, e) =>
            {
                _regionFilter = _ddRegion.SelectedText;
                RefreshBranchGrid();
            };

            _ddSort = new FilterDropdown
            {
                Size = new Size(185, 34),
                Margin = new Padding(0, 0, 6, 0)
            };
            _ddSort.SetItems(new[] { "Sắp xếp: Mặc định", "Doanh thu cao nhất", "Học viên đông nhất", "Diện tích lớn nhất" });
            _ddSort.SelectedIndexChanged += (s, e) =>
            {
                _sortMode = _ddSort.SelectedIndex;
                RefreshBranchGrid();
            };

            var btnReset = new Button
            {
                Text = "↺ Đặt lại",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = MutedColor,
                BackColor = Color.FromArgb(32, 32, 36),
                Size = new Size(88, 34),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                UseVisualStyleBackColor = false,
                Margin = new Padding(0)
            };
            btnReset.FlatAppearance.BorderColor = Border;
            btnReset.Click += (s, e) =>
            {
                _txtSearch.Text = string.Empty;
                _ddRegion.SelectedIndex = 0;
                _ddSort.SelectedIndex = 0;
                SetStatusFilter(0);
            };

            flpFilters.Controls.Add(_ddRegion);
            flpFilters.Controls.Add(_ddSort);
            flpFilters.Controls.Add(btnReset);

            // Result count on right
            _lblResultCount = new Label
            {
                Dock = DockStyle.Right,
                AutoSize = false,
                Width = 200,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = DimColor,
                Text = "Hiển thị: 0/0 cơ sở"
            };

            pnl.Controls.Add(_lblResultCount);
            pnl.Controls.Add(flpFilters);
            pnl.Controls.Add(pnlSearch);
            return pnl;
        }

        // ---------------------------------------------------------------------
        // E. LƯỚI BẢNG DỮ LIỆU CHI NHÁNH CHÍNH
        // ---------------------------------------------------------------------
        private DataGridView BuildMainGridView()
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = CardBg,
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
                RowTemplate = { Height = 44 },
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(28, 28, 32),
                ForeColor = MutedColor,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                SelectionBackColor = Color.FromArgb(28, 28, 32),
                Padding = new Padding(8, 0, 0, 0)
            };
            dgv.ColumnHeadersHeight = 38;

            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = CardBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9F),
                SelectionBackColor = Color.FromArgb(39, 39, 45),
                SelectionForeColor = Color.White,
                Padding = new Padding(8, 0, 0, 0)
            };

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STT",
                HeaderText = "STT",
                FillWeight = 32,
                MinimumWidth = 40,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Code",
                HeaderText = "Mã cơ sở",
                FillWeight = 65,
                MinimumWidth = 75,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Tên chi nhánh",
                FillWeight = 160,
                MinimumWidth = 160
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Address",
                HeaderText = "Địa chỉ cơ sở",
                FillWeight = 220,
                MinimumWidth = 200
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Manager",
                HeaderText = "Quản lý & Hotline",
                FillWeight = 160,
                MinimumWidth = 160
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Hours",
                HeaderText = "Giờ mở cửa",
                FillWeight = 85,
                MinimumWidth = 95,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Scale",
                HeaderText = "Quy mô cơ sở",
                FillWeight = 145,
                MinimumWidth = 150
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Revenue",
                HeaderText = "Doanh thu tháng",
                FillWeight = 110,
                MinimumWidth = 110,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Trạng thái",
                FillWeight = 95,
                MinimumWidth = 95,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Thao tác: 4 nút riêng biệt, rõ ràng và trực quan
            var btnDetail = new DataGridViewButtonColumn
            {
                Name = "BtnDetail",
                HeaderText = "Chi tiết",
                Text = "👁️ Chi tiết",
                UseColumnTextForButtonValue = true,
                FillWeight = 70,
                MinimumWidth = 70,
                FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(btnDetail);

            var btnToggle = new DataGridViewButtonColumn
            {
                Name = "BtnToggle",
                HeaderText = "Bật/Tắt",
                Text = "⚡ On/Off",
                UseColumnTextForButtonValue = true,
                FillWeight = 70,
                MinimumWidth = 70,
                FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(btnToggle);

            var btnEdit = new DataGridViewButtonColumn
            {
                Name = "BtnEdit",
                HeaderText = "Sửa",
                Text = "✏️ Sửa",
                UseColumnTextForButtonValue = true,
                FillWeight = 55,
                MinimumWidth = 55,
                FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(btnEdit);

            var btnDelete = new DataGridViewButtonColumn
            {
                Name = "BtnDelete",
                HeaderText = "Xóa",
                Text = "🗑️ Xóa",
                UseColumnTextForButtonValue = true,
                FillWeight = 55,
                MinimumWidth = 55,
                FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(btnDelete);

            // Xử lý sự kiện click trên các nút thao tác
            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var branch = dgv.Rows[e.RowIndex].Tag as BranchModel;
                if (branch == null) return;

                if (e.ColumnIndex == dgv.Columns["BtnDetail"]!.Index)
                {
                    OpenBranchDetailModal(branch);
                }
                else if (e.ColumnIndex == dgv.Columns["BtnToggle"]!.Index)
                {
                    ToggleBranchStatus(branch);
                }
                else if (e.ColumnIndex == dgv.Columns["BtnEdit"]!.Index)
                {
                    EditBranch(branch);
                }
                else if (e.ColumnIndex == dgv.Columns["BtnDelete"]!.Index)
                {
                    DeleteBranch(branch);
                }
            };

            // Nhấp đúp vào dòng để mở chi tiết
            dgv.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    var branch = dgv.Rows[e.RowIndex].Tag as BranchModel;
                    if (branch != null)
                    {
                        OpenBranchDetailModal(branch);
                    }
                }
            };

            return dgv;
        }

        // =====================================================================
        // REFRESH LƯỚI BẢNG DỮ LIỆU
        // =====================================================================
        private void RefreshBranchGrid()
        {
            if (_isInitializing || _grid == null || _grid.Columns.Count == 0) return;

            _grid.Rows.Clear();

            var query = _branches.AsEnumerable();

            // 1. Lọc theo trạng thái
            if (_statusFilter == 1) query = query.Where(b => b.IsOnline);
            else if (_statusFilter == 2) query = query.Where(b => !b.IsOnline);

            // 2. Lọc theo khu vực
            if (!string.IsNullOrEmpty(_regionFilter) && _regionFilter != "Tất cả khu vực")
            {
                string r = _regionFilter.ToLower();
                if (r.Contains("hồ chí minh") || r.Contains("tp.hcm") || r.Contains("hcm"))
                    query = query.Where(b => b.Address.Contains("TP.HCM") || b.Address.Contains("Quận") || b.Address.Contains("Bình Thạnh"));
                else if (r.Contains("hà nội"))
                    query = query.Where(b => b.Address.Contains("Hà Nội") || b.Address.Contains("Cầu Giấy") || b.Address.Contains("Hoàn Kiếm"));
                else if (r.Contains("đà nẵng"))
                    query = query.Where(b => b.Address.Contains("Đà Nẵng") || b.Address.Contains("Hải Châu"));
            }

            // 3. Tìm kiếm tự do
            if (!string.IsNullOrEmpty(_searchText))
            {
                string s = _searchText.ToLower();
                query = query.Where(b => b.Name.ToLower().Contains(s)
                                      || b.Code.ToLower().Contains(s)
                                      || b.Address.ToLower().Contains(s)
                                      || b.Manager.ToLower().Contains(s)
                                      || b.Phone.Contains(s));
            }

            // 4. Sắp xếp
            if (_sortMode == 1)
                query = query.OrderByDescending(b => b.MonthlyRevenue);
            else if (_sortMode == 2)
                query = query.OrderByDescending(b => _branchMemberMap.ContainsKey(b.Id) ? _branchMemberMap[b.Id].Count : b.MemberCount);
            else if (_sortMode == 3)
                query = query.OrderByDescending(b => b.AreaSqM);
            else
                query = query.OrderBy(b => b.Id);

            var list = query.ToList();
            int stt = 1;

            foreach (var b in list)
            {
                int mCount = _branchMemberMap.ContainsKey(b.Id) ? _branchMemberMap[b.Id].Count : b.MemberCount;
                int sCount = _branchStaffMap.ContainsKey(b.Id) ? _branchStaffMap[b.Id].Count : b.StaffCount;
                string scaleText = $"{b.AreaSqM:N0} m²  •  {mCount:N0} HV  •  {sCount} NV";
                string revenueText = b.MonthlyRevenue.ToString("N0", ViCulture) + " ₫";
                string statusText = b.IsOnline ? "🟢 Đang mở" : "🔴 Tạm đóng";

                int rIdx = _grid.Rows.Add(
                    stt++,
                    b.Code,
                    b.Name,
                    b.Address,
                    $"{b.Manager} • {b.Phone}",
                    $"{b.OpenTime} - {b.CloseTime}",
                    scaleText,
                    revenueText,
                    statusText
                );

                var row = _grid.Rows[rIdx];
                row.Tag = b;

                // Color badge styling
                row.Cells["Code"].Style.ForeColor = Orange;
                row.Cells["Code"].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

                row.Cells["Name"].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                row.Cells["Name"].Style.ForeColor = TextColor;

                row.Cells["Revenue"].Style.ForeColor = Orange;
                row.Cells["Revenue"].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

                if (b.IsOnline)
                {
                    row.Cells["Status"].Style.ForeColor = Green;
                    row.Cells["Status"].Style.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                }
                else
                {
                    row.Cells["Status"].Style.ForeColor = Red;
                    row.Cells["Status"].Style.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                }
            }

            if (_lblResultCount != null)
            {
                _lblResultCount.Text = $"Hiển thị: {list.Count}/{_branches.Count} cơ sở";
            }
        }

        // =====================================================================
        // CÁC THAO TÁC NGHIỆP VỤ (CRUD, CHI TIẾT, BẬT/TẮT)
        // =====================================================================
        private void OpenBranchDetailModal(BranchModel branch)
        {
            if (!_branchStaffMap.ContainsKey(branch.Id))
                _branchStaffMap[branch.Id] = new List<BranchStaffItem>();

            if (!_branchMemberMap.ContainsKey(branch.Id))
                _branchMemberMap[branch.Id] = new List<BranchMemberItem>();

            using var dlg = new frmBranchDetail(branch, _branchStaffMap[branch.Id], _branchMemberMap[branch.Id]);
            dlg.ShowDialog(this);

            if (dlg.HasChanged)
            {
                UpdateKpiCards();
                RefreshBranchGrid();
            }
        }

        private void ToggleBranchStatus(BranchModel branch)
        {
            string actionName = branch.IsOnline ? "TẠM ĐÓNG CỬA (OFFLINE)" : "BẬT HOẠT ĐỘNG (ONLINE)";
            string prompt = branch.IsOnline
                ? $"Bạn có chắc chắn muốn TẠM ĐÓNG cửa chi nhánh '{branch.Name}'?\n(Hệ thống sẽ chuyển cơ sở sang trạng thái bảo trì/ngoại tuyến)"
                : $"Bạn có chắc chắn muốn MỞ CỬA HOẠT ĐỘNG lại cho chi nhánh '{branch.Name}'?";

            if (MessageBox.Show(prompt, "Xác nhận chuyển trạng thái", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                branch.IsOnline = !branch.IsOnline;
                UpdateKpiCards();
                RefreshBranchGrid();
                MessageBox.Show($"Đã chuyển trạng thái chi nhánh '{branch.Name}' thành: {actionName} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                // Tự động gán nhân sự & học viên mẫu ban đầu
                SeedStaffData(dlg.ResultBranch.Id, dlg.ResultBranch.Code);
                SeedMemberData(dlg.ResultBranch.Id);

                UpdateKpiCards();
                RefreshBranchGrid();
                MessageBox.Show($"Thêm mới chi nhánh '{dlg.ResultBranch.Name}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void EditBranch(BranchModel branch)
        {
            using var dlg = new frmBranchEdit(branch);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ResultBranch != null)
            {
                UpdateKpiCards();
                RefreshBranchGrid();
                MessageBox.Show($"Cập nhật thông tin chi nhánh '{branch.Name}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteBranch(BranchModel branch)
        {
            if (_branches.Count <= 1)
            {
                MessageBox.Show("Hệ thống phải duy trì ít nhất 1 chi nhánh, không thể xóa chi nhánh duy nhất này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int mCount = _branchMemberMap.ContainsKey(branch.Id) ? _branchMemberMap[branch.Id].Count : 0;
            int sCount = _branchStaffMap.ContainsKey(branch.Id) ? _branchStaffMap[branch.Id].Count : 0;

            string warning = $"Xác nhận XÓA VĨNH VIỄN chi nhánh:\n👉 '{branch.Name}' ({branch.Code})?\n\nHiện chi nhánh đang quản lý: {mCount} học viên và {sCount} nhân sự.";

            if (MessageBox.Show(warning, "Cảnh báo xóa chi nhánh", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int delId = branch.Id;
                _branches.RemoveAll(x => x.Id == delId);
                _branchStaffMap.Remove(delId);
                _branchMemberMap.Remove(delId);

                UpdateKpiCards();
                RefreshBranchGrid();
                MessageBox.Show("Đã xóa chi nhánh thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
