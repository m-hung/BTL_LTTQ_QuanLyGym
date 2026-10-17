using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public class ucAdminMember : UserControl
    {
        // =====================================================================
        // MÀU SẮC & GIAO DIỆN CHỦ ĐẠO (DARK GYM THEME)
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

        private static readonly CultureInfo ViCulture = new CultureInfo("vi-VN");

        // =====================================================================
        // DATA MODEL
        // =====================================================================
        public class MemberItem
        {
            public int Id { get; set; }
            public string Code { get; set; } = "";
            public string FullName { get; set; } = "";
            public string Phone { get; set; } = "";
            public string Email { get; set; } = "";
            public string Address { get; set; } = "";
            public string Gender { get; set; } = "Nam";
            public DateTime DateOfBirth { get; set; }
            public string BranchName { get; set; } = "";
            public string Tier { get; set; } = "Classic"; // Classic, Silver, Gold, Platinum, VIP Diamond
            public string PackageName { get; set; } = "";
            public DateTime RegisterDate { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal PaidAmount { get; set; }
            public string AssignedPt { get; set; } = "Chưa có PT";
            public string HealthNotes { get; set; } = "";
            public decimal HeightCm { get; set; } = 170;
            public decimal WeightKg { get; set; } = 65;
            public decimal BodyFatPercent { get; set; } = 18;
            public decimal MuscleMassKg { get; set; } = 30;
            public int TotalCheckIns { get; set; } = 12;
            public DateTime? LastCheckIn { get; set; }
            public bool _isSuspended { get; set; }

            public decimal Bmi => HeightCm > 0 ? Math.Round(WeightKg / ((HeightCm / 100m) * (HeightCm / 100m)), 1) : 0;
            public int DaysRemaining => Math.Max(0, (int)(EndDate.Date - DateTime.Today).TotalDays);

            public string StatusText
            {
                get
                {
                    if (_isSuspended) return "⚪ Tạm khóa";
                    if (DaysRemaining <= 0) return "🔴 Đã hết hạn";
                    if (DaysRemaining <= 15) return "⏳ Sắp hết hạn";
                    return "🟢 Đang tập";
                }
            }

            public bool IsNew => (DateTime.Today - RegisterDate.Date).TotalDays <= 30;
            public bool IsExpiring => !_isSuspended && DaysRemaining > 0 && DaysRemaining <= 15;
            public bool IsExpired => !_isSuspended && DaysRemaining <= 0;
            public bool IsVip => Tier == "Platinum" || Tier == "VIP Diamond";
        }

        private readonly List<MemberItem> _members = new();

        // UI Controls
        private Label _lblKpiTotalVal = null!;
        private Label _lblKpiActiveVal = null!;
        private Label _lblKpiExpiringVal = null!;
        private Label _lblKpiNewVal = null!;
        private Label _lblKpiVipVal = null!;

        // Quick Tabs
        private Button _btnQuickAll = null!;
        private Button _btnQuickNew = null!;
        private Button _btnQuickExpiring = null!;
        private Button _btnQuickVip = null!;
        private Button _btnQuickExpired = null!;
        private int _quickFilterMode = 0; // 0: All, 1: New, 2: Expiring, 3: VIP, 4: Expired

        // Toolbar
        private TextBox _txtSearch = null!;
        private FilterDropdown _ddTier = null!;
        private FilterDropdown _ddGender = null!;
        private FilterDropdown _ddBranch = null!;
        private FilterDropdown _ddStatus = null!;

        // Grid
        private DataGridView _grid = null!;

        public ucAdminMember()
        {
            InitializeData();
            BuildUI();
            RefreshDataGrid();
            RefreshKpis();
        }

        // =====================================================================
        // KHỞI TẠO DỮ LIỆU BAN ĐẦU
        // =====================================================================
        private void InitializeData()
        {
            // 1. Hội viên VIP Diamond
            _members.Add(new MemberItem
            {
                Id = 1,
                Code = "HV-101",
                FullName = "Đặng Hoàng Nam",
                Gender = "Nam",
                DateOfBirth = new DateTime(1988, 4, 12),
                Phone = "0912.888.999",
                Email = "nam.dang@investcorp.vn",
                Address = "Penthouse Vista Verde, P. Thạnh Mỹ Lợi, Quận 2, TP.HCM",
                BranchName = "GymFit Quận 1 - Bến Nghé",
                Tier = "VIP Diamond",
                PackageName = "Gói Diamond 12 Tháng Thượng Lưu",
                RegisterDate = DateTime.Today.AddDays(-120),
                StartDate = DateTime.Today.AddDays(-120),
                EndDate = DateTime.Today.AddDays(245),
                PaidAmount = 35000000,
                AssignedPt = "Vũ Hoàng Long",
                HealthNotes = "Tập tăng cường thể lực lãnh đạo, xông hơi Sauna sau giờ làm việc. Thích nước khoáng ion kiềm.",
                HeightCm = 178,
                WeightKg = 76,
                BodyFatPercent = 16.2m,
                MuscleMassKg = 38.5m,
                TotalCheckIns = 64,
                LastCheckIn = DateTime.Now.AddHours(-3)
            });

            // 2. Hội viên sắp hết hạn (còn 5 ngày)
            _members.Add(new MemberItem
            {
                Id = 2,
                Code = "HV-102",
                FullName = "Trần Minh Tú",
                Gender = "Nam",
                DateOfBirth = new DateTime(1994, 8, 25),
                Phone = "0987.654.321",
                Email = "minhtu.tech@fpt.com",
                Address = "Số 15 ngõ 82 Duy Tân, Q. Cầu Giấy, Hà Nội",
                BranchName = "GymFit Cầu Giấy - Duy Tân",
                Tier = "Gold",
                PackageName = "Gói Gold 6 Tháng Năng Động",
                RegisterDate = DateTime.Today.AddDays(-175),
                StartDate = DateTime.Today.AddDays(-175),
                EndDate = DateTime.Today.AddDays(5), // Còn 5 ngày
                PaidAmount = 6480000,
                AssignedPt = "Nguyễn Đức Minh",
                HealthNotes = "Nhân viên IT văn phòng, đau vai gáy. Đang cân nhắc gia hạn thêm 12 tháng nhận ưu đãi giảm 15%.",
                HeightCm = 172,
                WeightKg = 68,
                BodyFatPercent = 19.5m,
                MuscleMassKg = 31.0m,
                TotalCheckIns = 48,
                LastCheckIn = DateTime.Now.AddDays(-1)
            });

            // 3. Hội viên mới đăng ký trong tuần
            _members.Add(new MemberItem
            {
                Id = 3,
                Code = "HV-103",
                FullName = "Nguyễn Phương Anh",
                Gender = "Nữ",
                DateOfBirth = new DateTime(1998, 11, 3),
                Phone = "0934.112.233",
                Email = "phuonganh.nguyen@gmail.com",
                Address = "Chung cư Vinhomes Central Park, Q. Bình Thạnh, TP.HCM",
                BranchName = "GymFit Bình Thạnh - Landmark",
                Tier = "Platinum",
                PackageName = "Gói Platinum 12 Tháng Toàn Diện",
                RegisterDate = DateTime.Today.AddDays(-3), // Mới 3 ngày trước
                StartDate = DateTime.Today.AddDays(-3),
                EndDate = DateTime.Today.AddDays(362),
                PaidAmount = 20000000,
                AssignedPt = "Phan Thị Thảo",
                HealthNotes = "Mục tiêu: Giảm 4kg mỡ, săn chắc cơ mông đùi. Tập lớp Yoga buổi tối & GroupX.",
                HeightCm = 163,
                WeightKg = 55,
                BodyFatPercent = 23.0m,
                MuscleMassKg = 22.4m,
                TotalCheckIns = 2,
                LastCheckIn = DateTime.Now.AddDays(-1)
            });

            // 4. Hội viên sắp hết hạn (còn 8 ngày)
            _members.Add(new MemberItem
            {
                Id = 4,
                Code = "HV-104",
                FullName = "Vũ Thị Bích Ngọc",
                Gender = "Nữ",
                DateOfBirth = new DateTime(1992, 2, 17),
                Phone = "0908.776.655",
                Email = "bichngoc.design@studio.vn",
                Address = "Số 45 Tràng Tiền, Hoàn Kiếm, Hà Nội",
                BranchName = "GymFit Hoàn Kiếm - Tràng Tiền",
                Tier = "Silver",
                PackageName = "Gói Silver 3 Tháng Tiêu Chuẩn",
                RegisterDate = DateTime.Today.AddDays(-82),
                StartDate = DateTime.Today.AddDays(-82),
                EndDate = DateTime.Today.AddDays(8), // Còn 8 ngày
                PaidAmount = 2250000,
                AssignedPt = "Lê Công Vinh",
                HealthNotes = "Khách hàng muốn nâng cấp lên hạng Gold để bơi bốn mùa vào mùa đông.",
                HeightCm = 158,
                WeightKg = 51,
                BodyFatPercent = 21.5m,
                MuscleMassKg = 20.8m,
                TotalCheckIns = 26,
                LastCheckIn = DateTime.Now.AddDays(-2)
            });

            // 5. Hội viên mới (đăng ký 7 ngày trước)
            _members.Add(new MemberItem
            {
                Id = 5,
                Code = "HV-105",
                FullName = "Lê Hoàng Phúc",
                Gender = "Nam",
                DateOfBirth = new DateTime(2001, 7, 19),
                Phone = "0945.334.455",
                Email = "phucle.student@hust.edu.vn",
                Address = "Ký túc xá Bách Khoa, Hai Bà Trưng, Hà Nội",
                BranchName = "GymFit Cầu Giấy - Duy Tân",
                Tier = "Classic",
                PackageName = "Gói Classic 6 Tháng Tiết Kiệm",
                RegisterDate = DateTime.Today.AddDays(-7),
                StartDate = DateTime.Today.AddDays(-7),
                EndDate = DateTime.Today.AddDays(173),
                PaidAmount = 2400000,
                AssignedPt = "Chưa có PT",
                HealthNotes = "Sinh viên tập tạ tăng cơ ngực và tay. Tập khung giờ trưa vắng người.",
                HeightCm = 175,
                WeightKg = 64,
                BodyFatPercent = 17.0m,
                MuscleMassKg = 31.5m,
                TotalCheckIns = 5,
                LastCheckIn = DateTime.Now.AddHours(-5)
            });

            // 6. Hội viên Gold đang tập tích cực
            _members.Add(new MemberItem
            {
                Id = 6,
                Code = "HV-106",
                FullName = "Phạm Quốc Hưng",
                Gender = "Nam",
                DateOfBirth = new DateTime(1985, 9, 30),
                Phone = "0972.123.789",
                Email = "quochung.pham@vinfast.vn",
                Address = "Số 120 Hai Bà Trưng, Quận 1, TP.HCM",
                BranchName = "GymFit Quận 1 - Bến Nghé",
                Tier = "Gold",
                PackageName = "Gói Gold 12 Tháng Toàn Năng",
                RegisterDate = DateTime.Today.AddDays(-90),
                StartDate = DateTime.Today.AddDays(-90),
                EndDate = DateTime.Today.AddDays(275),
                PaidAmount = 12000000,
                AssignedPt = "Trần Thị Bích",
                HealthNotes = "Thường xuyên check-in lúc 06:00 sáng. Bơi lội 30 phút sau khi nâng tạ.",
                HeightCm = 180,
                WeightKg = 82,
                BodyFatPercent = 18.0m,
                MuscleMassKg = 41.2m,
                TotalCheckIns = 52,
                LastCheckIn = DateTime.Now.AddHours(-1)
            });

            // 7. Hội viên đã hết hạn (hết hạn 4 ngày trước)
            _members.Add(new MemberItem
            {
                Id = 7,
                Code = "HV-107",
                FullName = "Đỗ Kim Oanh",
                Gender = "Nữ",
                DateOfBirth = new DateTime(1996, 12, 14),
                Phone = "0963.882.114",
                Email = "kimoanh.do@techcombank.com.vn",
                Address = "Tòa nhà Keangnam, Mễ Trì, Nam Từ Liêm, Hà Nội",
                BranchName = "GymFit Cầu Giấy - Duy Tân",
                Tier = "Silver",
                PackageName = "Gói Silver 3 Tháng Tiêu Chuẩn",
                RegisterDate = DateTime.Today.AddDays(-94),
                StartDate = DateTime.Today.AddDays(-94),
                EndDate = DateTime.Today.AddDays(-4), // Đã hết hạn 4 ngày
                PaidAmount = 2250000,
                AssignedPt = "Nguyễn Đức Minh",
                HealthNotes = "Cần lễ tân gọi điện nhắc gia hạn hoặc tặng voucher giảm giá 10% thẻ tái tục.",
                HeightCm = 160,
                WeightKg = 53,
                BodyFatPercent = 22.0m,
                MuscleMassKg = 21.0m,
                TotalCheckIns = 31,
                LastCheckIn = DateTime.Today.AddDays(-6)
            });

            // 8. Hội viên VIP Diamond Đà Nẵng
            _members.Add(new MemberItem
            {
                Id = 8,
                Code = "HV-108",
                FullName = "Ngô Bảo Châu",
                Gender = "Nam",
                DateOfBirth = new DateTime(1979, 6, 8),
                Phone = "0918.555.777",
                Email = "baochau.ngo@danangresort.vn",
                Address = "Biệt thự Euro Village, Sơn Trà, TP. Đà Nẵng",
                BranchName = "GymFit Hải Châu - Bạch Đằng",
                Tier = "VIP Diamond",
                PackageName = "Gói Diamond 12 Tháng Thượng Lưu",
                RegisterDate = DateTime.Today.AddDays(-40),
                StartDate = DateTime.Today.AddDays(-40),
                EndDate = DateTime.Today.AddDays(325),
                PaidAmount = 35000000,
                AssignedPt = "Vũ Hoàng Long",
                HealthNotes = "Khách hàng VIP, sử dụng dịch vụ tủ đồ riêng & phòng VIP Lounge.",
                HeightCm = 173,
                WeightKg = 72,
                BodyFatPercent = 17.5m,
                MuscleMassKg = 34.2m,
                TotalCheckIns = 22,
                LastCheckIn = DateTime.Now.AddDays(-1)
            });

            // 9. Hội viên Platinum tập đa chi nhánh
            _members.Add(new MemberItem
            {
                Id = 9,
                Code = "HV-109",
                FullName = "Hoàng Thuỳ Linh",
                Gender = "Nữ",
                DateOfBirth = new DateTime(1995, 3, 21),
                Phone = "0938.999.444",
                Email = "thuylinh.hoang@vietnamairlines.com",
                Address = "Đoàn tiếp viên VNA, P. 2, Q. Tân Bình, TP.HCM",
                BranchName = "GymFit Quận 1 - Bến Nghé",
                Tier = "Platinum",
                PackageName = "Gói Platinum 12 Tháng Toàn Quốc",
                RegisterDate = DateTime.Today.AddDays(-60),
                StartDate = DateTime.Today.AddDays(-60),
                EndDate = DateTime.Today.AddDays(305),
                PaidAmount = 20000000,
                AssignedPt = "Trần Thị Bích",
                HealthNotes = "Thường xuyên di chuyển giữa Hà Nội và TP.HCM, check-in cả CN Quận 1 và Hoàn Kiếm.",
                HeightCm = 168,
                WeightKg = 54,
                BodyFatPercent = 20.0m,
                MuscleMassKg = 24.1m,
                TotalCheckIns = 35,
                LastCheckIn = DateTime.Now.AddDays(-2)
            });

            // 10. Hội viên sắp hết hạn (còn 12 ngày)
            _members.Add(new MemberItem
            {
                Id = 10,
                Code = "HV-110",
                FullName = "Bùi Văn Thành",
                Gender = "Nam",
                DateOfBirth = new DateTime(1990, 10, 5),
                Phone = "0977.443.221",
                Email = "thanhbui.sales@vingroup.net",
                Address = "Tòa Landmark 81, Vinhomes Central Park, Bình Thạnh",
                BranchName = "GymFit Bình Thạnh - Landmark",
                Tier = "Gold",
                PackageName = "Gói Gold 6 Tháng Năng Động",
                RegisterDate = DateTime.Today.AddDays(-168),
                StartDate = DateTime.Today.AddDays(-168),
                EndDate = DateTime.Today.AddDays(12), // Còn 12 ngày
                PaidAmount = 6480000,
                AssignedPt = "Phan Thị Thảo",
                HealthNotes = "Tập duy trì vóc dáng, chuẩn bị gia hạn gói 1 năm.",
                HeightCm = 176,
                WeightKg = 73,
                BodyFatPercent = 16.8m,
                MuscleMassKg = 36.4m,
                TotalCheckIns = 44,
                LastCheckIn = DateTime.Now.AddDays(-1)
            });

            // 11. Hội viên mới (đăng ký 2 ngày trước)
            _members.Add(new MemberItem
            {
                Id = 11,
                Code = "HV-111",
                FullName = "Nguyễn Thùy Dung",
                Gender = "Nữ",
                DateOfBirth = new DateTime(2000, 1, 15),
                Phone = "0983.221.889",
                Email = "dungnguyen.koc@tiktok.com",
                Address = "Số 88 Lý Thường Kiệt, Hoàn Kiếm, Hà Nội",
                BranchName = "GymFit Hoàn Kiếm - Tràng Tiền",
                Tier = "Silver",
                PackageName = "Gói Silver 6 Tháng Tiêu Chuẩn",
                RegisterDate = DateTime.Today.AddDays(-2),
                StartDate = DateTime.Today.AddDays(-2),
                EndDate = DateTime.Today.AddDays(180),
                PaidAmount = 4050000,
                AssignedPt = "Lê Công Vinh",
                HealthNotes = "Hội viên mới, cần HLV hỗ trợ hướng dẫn làm quen các dòng máy chạy bộ & cardio.",
                HeightCm = 162,
                WeightKg = 49,
                BodyFatPercent = 21.0m,
                MuscleMassKg = 20.2m,
                TotalCheckIns = 2,
                LastCheckIn = DateTime.Now.AddHours(-2)
            });

            // 12. Hội viên tạm bảo lưu
            _members.Add(new MemberItem
            {
                Id = 12,
                Code = "HV-112",
                FullName = "Trịnh Hải Đăng",
                Gender = "Nam",
                DateOfBirth = new DateTime(1987, 5, 29),
                Phone = "0915.667.889",
                Email = "haidang.trinh@shipping.vn",
                Address = "Số 204 Nguyễn Thị Minh Khai, Quận 3, TP.HCM",
                BranchName = "GymFit Quận 1 - Bến Nghé",
                Tier = "Gold",
                PackageName = "Gói Gold 12 Tháng Toàn Năng",
                RegisterDate = DateTime.Today.AddDays(-150),
                StartDate = DateTime.Today.AddDays(-150),
                EndDate = DateTime.Today.AddDays(215),
                PaidAmount = 12000000,
                AssignedPt = "Trần Thị Bích",
                HealthNotes = "Đi công tác nước ngoài 2 tháng, đã gửi đơn xin bảo lưu thẻ từ ngày 01/10.",
                HeightCm = 174,
                WeightKg = 70,
                BodyFatPercent = 18.2m,
                MuscleMassKg = 33.0m,
                TotalCheckIns = 38,
                LastCheckIn = DateTime.Today.AddDays(-25),
                _isSuspended = true
            });
        }

        // =====================================================================
        // XÂY DỰNG GIAO DIỆN
        // =====================================================================
        private void BuildUI()
        {
            Dock = DockStyle.Fill;
            BackColor = HeaderBg;
            Padding = new Padding(12, 10, 12, 10);

            // A. Top Header
            var pnlHeader = BuildTopHeaderSection();

            // Gap 1
            var pnlGap1 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // B. Dải 5 KPI Cards
            var pnlKpis = BuildKpiStrip();

            // Gap 2
            var pnlGap2 = new Panel { Dock = DockStyle.Top, Height = 12, BackColor = Color.Transparent };

            // C. Dải Quick Filter Tabs (Tất cả, Mới, Sắp hết hạn, VIP...)
            var pnlQuickTabs = BuildQuickTabsSection();

            // Gap 3
            var pnlGap3 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // D. Toolbar Bộ lọc và Tìm kiếm
            var pnlToolbar = BuildFilterToolbar();

            // Gap 4
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

            // Add Controls theo thứ tự Docking
            Controls.Add(pnlGridContainer);
            Controls.Add(pnlGap4);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlGap3);
            Controls.Add(pnlQuickTabs);
            Controls.Add(pnlGap2);
            Controls.Add(pnlKpis);
            Controls.Add(pnlGap1);
            Controls.Add(pnlHeader);
        }

        // ---------------------------------------------------------------------
        // A. TOP HEADER SECTION
        // ---------------------------------------------------------------------
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

            // Tiêu đề trang
            var lblTitle = new Label
            {
                Text = "QUẢN LÝ HỘI VIÊN & HẠNG THẺ",
                Font = new Font("Segoe UI", 12.5F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(16, 10),
                AutoSize = true
            };

            var lblSubtitle = new Label
            {
                Text = "Theo dõi hồ sơ hội viên, chỉ số InBody, nâng cấp gói tập và chăm sóc hội viên sắp hết hạn",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Location = new Point(18, 34),
                AutoSize = true
            };

            // Nút hành động góc phải
            var flpActions = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            var btnPricing = new Button
            {
                Text = "⭐  Bảng giá & Hạng thẻ",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Gold,
                BackColor = Color.FromArgb(38, 35, 20),
                Size = new Size(180, 36),
                Margin = new Padding(0, 3, 10, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnPricing.FlatAppearance.BorderColor = Gold;
            btnPricing.Click += (s, e) =>
            {
                using var dlg = new frmMembershipPricing();
                dlg.ShowDialog(this);
            };

            var btnAddMember = new Button
            {
                Text = "➕  Đăng ký hội viên mới",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(190, 36),
                Margin = new Padding(0, 3, 10, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAddMember.FlatAppearance.BorderSize = 0;
            btnAddMember.Click += BtnAddMember_Click;

            var btnExport = new Button
            {
                Text = "📥  Xuất báo cáo",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = TextColor,
                BackColor = Color.FromArgb(39, 39, 42),
                Size = new Size(130, 36),
                Margin = new Padding(0, 3, 0, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExport.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 70);
            btnExport.Click += (s, e) =>
            {
                MessageBox.Show($"Đã xuất danh sách {_members.Count} hội viên ra báo cáo Excel thành công!", "Xuất báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            flpActions.Controls.Add(btnPricing);
            flpActions.Controls.Add(btnAddMember);
            flpActions.Controls.Add(btnExport);

            pnl.Controls.Add(lblTitle);
            pnl.Controls.Add(lblSubtitle);
            pnl.Controls.Add(flpActions);

            return pnl;
        }

        // ---------------------------------------------------------------------
        // B. DẢI 5 KPI CARDS
        // ---------------------------------------------------------------------
        private Panel BuildKpiStrip()
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
                ColumnCount = 5,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            for (int i = 0; i < 5; i++) tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            tbl.Controls.Add(CreateKpiCard("👥  TỔNG HỘI VIÊN", out _lblKpiTotalVal, Blue), 0, 0);
            tbl.Controls.Add(CreateKpiCard("🟢  ĐANG HOẠT ĐỘNG", out _lblKpiActiveVal, Green), 1, 0);
            tbl.Controls.Add(CreateKpiCard("⏳  SẮP HẾT HẠN (≤ 15 NGÀY)", out _lblKpiExpiringVal, Orange), 2, 0);
            tbl.Controls.Add(CreateKpiCard("🆕  HỘI VIÊN MỚI (30 NGÀY)", out _lblKpiNewVal, Cyan), 3, 0);
            tbl.Controls.Add(CreateKpiCard("💎  VIP & DIAMOND", out _lblKpiVipVal, Purple), 4, 0);

            pnlWrapper.Controls.Add(tbl);
            return pnlWrapper;
        }

        private Panel CreateKpiCard(string title, out Label lblVal, Color accent)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
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
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
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
        // C. THANH TAB LỌC NHANH (TẤT CẢ, MỚI, SẮP HẾT HẠN, VIP...)
        // ---------------------------------------------------------------------
        private Panel BuildQuickTabsSection()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 4, 0, 4)
            };

            var flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            _btnQuickAll = CreateQuickTabBtn("📋  Tất cả hội viên", 0, Orange);
            _btnQuickNew = CreateQuickTabBtn("🆕  Hội viên mới", 1, Cyan);
            _btnQuickExpiring = CreateQuickTabBtn("⏳  Sắp hết hạn thẻ", 2, Orange);
            _btnQuickVip = CreateQuickTabBtn("💎  Hội viên VIP", 3, Purple);
            _btnQuickExpired = CreateQuickTabBtn("⛔  Đã hết hạn / Bảo lưu", 4, Red);

            _btnQuickAll.Click += (s, e) => SetQuickFilter(0);
            _btnQuickNew.Click += (s, e) => SetQuickFilter(1);
            _btnQuickExpiring.Click += (s, e) => SetQuickFilter(2);
            _btnQuickVip.Click += (s, e) => SetQuickFilter(3);
            _btnQuickExpired.Click += (s, e) => SetQuickFilter(4);

            flp.Controls.Add(_btnQuickAll);
            flp.Controls.Add(_btnQuickNew);
            flp.Controls.Add(_btnQuickExpiring);
            flp.Controls.Add(_btnQuickVip);
            flp.Controls.Add(_btnQuickExpired);

            pnl.Controls.Add(flp);
            return pnl;
        }

        private Button CreateQuickTabBtn(string text, int mode, Color activeColor)
        {
            bool isActive = (mode == _quickFilterMode);
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = isActive ? activeColor : MutedColor,
                BackColor = isActive ? CardBg : Color.Transparent,
                Height = 38,
                AutoSize = true,
                Padding = new Padding(12, 0, 12, 0),
                Margin = new Padding(0, 0, 8, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btn.FlatAppearance.BorderColor = isActive ? activeColor : Border;
            btn.FlatAppearance.BorderSize = 1;
            return btn;
        }

        public void SetQuickFilter(int mode)
        {
            _quickFilterMode = mode;

            UpdateQuickBtnState(_btnQuickAll, 0, Orange);
            UpdateQuickBtnState(_btnQuickNew, 1, Cyan);
            UpdateQuickBtnState(_btnQuickExpiring, 2, Orange);
            UpdateQuickBtnState(_btnQuickVip, 3, Purple);
            UpdateQuickBtnState(_btnQuickExpired, 4, Red);

            RefreshDataGrid();
        }

        private void UpdateQuickBtnState(Button btn, int mode, Color activeColor)
        {
            bool active = (_quickFilterMode == mode);
            btn.ForeColor = active ? activeColor : MutedColor;
            btn.BackColor = active ? CardBg : Color.Transparent;
            btn.FlatAppearance.BorderColor = active ? activeColor : Border;
        }

        // ---------------------------------------------------------------------
        // D. TOOLBAR BỘ LỌC ĐA TIÊU CHÍ & TÌM KIẾM
        // ---------------------------------------------------------------------
        private Panel BuildFilterToolbar()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.Transparent
            };

            var flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            // Ô tìm kiếm
            var pnlSearch = new Panel
            {
                Size = new Size(240, 36),
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
                PlaceholderText = "🔍 Tìm theo tên, mã, SĐT..."
            };
            _txtSearch.TextChanged += (s, e) => RefreshDataGrid();
            pnlSearch.Controls.Add(_txtSearch);

            // Filter Hạng thẻ
            _ddTier = new FilterDropdown
            {
                Size = new Size(150, 36),
                Margin = new Padding(0, 0, 10, 0)
            };
            _ddTier.SetItems(new[] { "Tất cả hạng thẻ", "Classic", "Silver", "Gold", "Platinum", "VIP Diamond" });
            _ddTier.SelectedIndexChanged += (s, e) => RefreshDataGrid();

            // Filter Giới tính
            _ddGender = new FilterDropdown
            {
                Size = new Size(130, 36),
                Margin = new Padding(0, 0, 10, 0)
            };
            _ddGender.SetItems(new[] { "Tất cả giới tính", "Nam", "Nữ" });
            _ddGender.SelectedIndexChanged += (s, e) => RefreshDataGrid();

            // Filter Chi nhánh
            _ddBranch = new FilterDropdown
            {
                Size = new Size(170, 36),
                Margin = new Padding(0, 0, 10, 0)
            };
            _ddBranch.SetItems(new[] { "Tất cả chi nhánh", "Quận 1", "Hoàn Kiếm", "Cầu Giấy", "Bình Thạnh", "Hải Châu" });
            _ddBranch.SelectedIndexChanged += (s, e) => RefreshDataGrid();

            // Filter Trạng thái
            _ddStatus = new FilterDropdown
            {
                Size = new Size(150, 36),
                Margin = new Padding(0, 0, 10, 0)
            };
            _ddStatus.SetItems(new[] { "Tất cả trạng thái", "Đang tập", "Sắp hết hạn", "Đã hết hạn", "Tạm khóa" });
            _ddStatus.SelectedIndexChanged += (s, e) => RefreshDataGrid();

            // Nút Reset
            var btnReset = new Button
            {
                Text = "✕ Xóa lọc",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                BackColor = CardBg,
                Size = new Size(86, 36),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnReset.FlatAppearance.BorderColor = Border;
            btnReset.Click += (s, e) =>
            {
                _txtSearch.Text = "";
                _ddTier.SelectedIndex = 0;
                _ddGender.SelectedIndex = 0;
                _ddBranch.SelectedIndex = 0;
                _ddStatus.SelectedIndex = 0;
                SetQuickFilter(0);
            };

            flp.Controls.Add(pnlSearch);
            flp.Controls.Add(_ddTier);
            flp.Controls.Add(_ddGender);
            flp.Controls.Add(_ddBranch);
            flp.Controls.Add(_ddStatus);
            flp.Controls.Add(btnReset);

            pnl.Controls.Add(flp);
            return pnl;
        }

        // ---------------------------------------------------------------------
        // E. MAIN DATAGRIDVIEW
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
                RowTemplate = { Height = 46 }
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

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Định nghĩa các cột
            dgv.Columns.Add("Code", "Mã HV");
            dgv.Columns["Code"]!.MinimumWidth = 85;
            dgv.Columns["Code"]!.FillWeight = 7;

            dgv.Columns.Add("FullName", "Họ và tên");
            dgv.Columns["FullName"]!.MinimumWidth = 150;
            dgv.Columns["FullName"]!.FillWeight = 13;

            dgv.Columns.Add("Gender", "Giới tính");
            dgv.Columns["Gender"]!.MinimumWidth = 80;
            dgv.Columns["Gender"]!.FillWeight = 7;

            dgv.Columns.Add("Phone", "Số điện thoại");
            dgv.Columns["Phone"]!.MinimumWidth = 110;
            dgv.Columns["Phone"]!.FillWeight = 9;

            dgv.Columns.Add("Branch", "Chi nhánh tập");
            dgv.Columns["Branch"]!.MinimumWidth = 140;
            dgv.Columns["Branch"]!.FillWeight = 12;

            dgv.Columns.Add("Tier", "Hạng thẻ");
            dgv.Columns["Tier"]!.MinimumWidth = 95;
            dgv.Columns["Tier"]!.FillWeight = 8;

            dgv.Columns.Add("RegisterDate", "Ngày đăng ký");
            dgv.Columns["RegisterDate"]!.MinimumWidth = 100;
            dgv.Columns["RegisterDate"]!.FillWeight = 8;

            dgv.Columns.Add("Duration", "Thời hạn gói & Ngày còn lại");
            dgv.Columns["Duration"]!.MinimumWidth = 170;
            dgv.Columns["Duration"]!.FillWeight = 14;

            dgv.Columns.Add("PT", "HLV kèm");
            dgv.Columns["PT"]!.MinimumWidth = 115;
            dgv.Columns["PT"]!.FillWeight = 9;

            dgv.Columns.Add("Status", "Trạng thái");
            dgv.Columns["Status"]!.MinimumWidth = 105;
            dgv.Columns["Status"]!.FillWeight = 8;

            // Nút Thao tác 1: Chi tiết
            var colView = new DataGridViewButtonColumn
            {
                Name = "ColView",
                HeaderText = "Hồ sơ",
                Text = "👁️ Xem",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 65,
                FillWeight = 5,
                FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(colView);

            // Nút Thao tác 2: Nâng cấp / Gia hạn
            var colUpgrade = new DataGridViewButtonColumn
            {
                Name = "ColUpgrade",
                HeaderText = "Hạng thẻ",
                Text = "⚡ Nâng cấp",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 85,
                FillWeight = 7,
                FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(colUpgrade);

            // Nút Thao tác 3: Xóa
            var colDelete = new DataGridViewButtonColumn
            {
                Name = "ColDelete",
                HeaderText = "Xóa",
                Text = "🗑️",
                UseColumnTextForButtonValue = true,
                MinimumWidth = 50,
                FillWeight = 4,
                FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(colDelete);

            // Xử lý sự kiện bấm nút
            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var member = dgv.Rows[e.RowIndex].Tag as MemberItem;
                if (member == null) return;

                if (e.ColumnIndex == dgv.Columns["ColView"]!.Index)
                {
                    OpenMemberDetail(member);
                }
                else if (e.ColumnIndex == dgv.Columns["ColUpgrade"]!.Index)
                {
                    OpenMemberUpgrade(member);
                }
                else if (e.ColumnIndex == dgv.Columns["ColDelete"]!.Index)
                {
                    if (MessageBox.Show($"Xác nhận xóa hồ sơ hội viên '{member.FullName}' ({member.Code}) khỏi hệ thống?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        _members.Remove(member);
                        RefreshDataGrid();
                        RefreshKpis();
                    }
                }
            };

            // Double Click mở chi tiết
            dgv.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && dgv.Rows[e.RowIndex].Tag is MemberItem member)
                {
                    OpenMemberDetail(member);
                }
            };

            return dgv;
        }

        // =====================================================================
        // LOAD VÀ LỌC DỮ LIỆU
        // =====================================================================
        private void RefreshDataGrid()
        {
            if (_grid == null) return;
            _grid.Rows.Clear();

            var query = _members.AsEnumerable();

            // 1. Quick Filter Tabs
            if (_quickFilterMode == 1) query = query.Where(m => m.IsNew);
            else if (_quickFilterMode == 2) query = query.Where(m => m.IsExpiring);
            else if (_quickFilterMode == 3) query = query.Where(m => m.IsVip);
            else if (_quickFilterMode == 4) query = query.Where(m => m.IsExpired || m._isSuspended);

            // 2. Search Text
            string q = _txtSearch?.Text.Trim().ToLower() ?? "";
            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(m => m.FullName.ToLower().Contains(q)
                                      || m.Code.ToLower().Contains(q)
                                      || m.Phone.Contains(q)
                                      || m.Email.ToLower().Contains(q));
            }

            // 3. Filter Tier
            int tierIdx = _ddTier?.SelectedIndex ?? 0;
            if (tierIdx > 0)
            {
                string t = _ddTier!.SelectedText;
                query = query.Where(m => m.Tier.Equals(t, StringComparison.OrdinalIgnoreCase));
            }

            // 4. Filter Gender
            int gIdx = _ddGender?.SelectedIndex ?? 0;
            if (gIdx > 0)
            {
                string g = _ddGender!.SelectedText;
                query = query.Where(m => m.Gender.Equals(g, StringComparison.OrdinalIgnoreCase));
            }

            // 5. Filter Branch
            int bIdx = _ddBranch?.SelectedIndex ?? 0;
            if (bIdx > 0)
            {
                string b = _ddBranch!.SelectedText;
                query = query.Where(m => m.BranchName.Contains(b));
            }

            // 6. Filter Status
            int sIdx = _ddStatus?.SelectedIndex ?? 0;
            if (sIdx > 0)
            {
                string st = _ddStatus!.SelectedText;
                query = query.Where(m => m.StatusText.Contains(st));
            }

            var list = query.ToList();

            foreach (var m in list)
            {
                string branchShort = m.BranchName.Replace("GymFit ", "");
                int age = DateTime.Today.Year - m.DateOfBirth.Year;
                string genderAge = $"{m.Gender} ({age}t)";

                string remainText = m.DaysRemaining > 0 ? $" (Còn {m.DaysRemaining} ngày)" : " (Hết hạn)";
                string durStr = $"{m.EndDate:dd/MM/yyyy}{remainText}";

                int rIdx = _grid.Rows.Add(
                    m.Code,
                    m.FullName,
                    genderAge,
                    m.Phone,
                    branchShort,
                    m.Tier,
                    m.RegisterDate.ToString("dd/MM/yyyy"),
                    durStr,
                    m.AssignedPt,
                    m.StatusText
                );

                var row = _grid.Rows[rIdx];
                row.Tag = m;

                // Color coding for Tier
                if (m.Tier == "VIP Diamond") row.Cells["Tier"].Style.ForeColor = Orange;
                else if (m.Tier == "Platinum") row.Cells["Tier"].Style.ForeColor = Purple;
                else if (m.Tier == "Gold") row.Cells["Tier"].Style.ForeColor = Gold;
                else if (m.Tier == "Silver") row.Cells["Tier"].Style.ForeColor = Cyan;
                else row.Cells["Tier"].Style.ForeColor = Blue;

                // Ngày đăng ký
                row.Cells["RegisterDate"].Style.ForeColor = MutedColor;

                // Color coding for Duration / Remaining days
                if (m.DaysRemaining <= 0) row.Cells["Duration"].Style.ForeColor = Red;
                else if (m.DaysRemaining <= 15) row.Cells["Duration"].Style.ForeColor = Orange;
                else row.Cells["Duration"].Style.ForeColor = Green;

                // Color coding for Status
                if (m._isSuspended) row.Cells["Status"].Style.ForeColor = MutedColor;
                else if (m.DaysRemaining <= 0) row.Cells["Status"].Style.ForeColor = Red;
                else if (m.DaysRemaining <= 15) row.Cells["Status"].Style.ForeColor = Orange;
                else row.Cells["Status"].Style.ForeColor = Green;
            }
        }

        private void RefreshKpis()
        {
            int total = _members.Count;
            int active = _members.Count(m => !m._isSuspended && m.DaysRemaining > 0);
            int expiring = _members.Count(m => m.IsExpiring);
            int newCount = _members.Count(m => m.IsNew);
            int vip = _members.Count(m => m.IsVip);

            _lblKpiTotalVal.Text = $"{total} người";
            _lblKpiActiveVal.Text = $"{active} đang tập";
            _lblKpiExpiringVal.Text = $"{expiring} cần nhắc";
            _lblKpiNewVal.Text = $"+{newCount} người mới";
            _lblKpiVipVal.Text = $"{vip} VIP";

            // Cập nhật text trên các nút tab nhanh
            int expiredCount = _members.Count(m => m.IsExpired || m._isSuspended);
            _btnQuickAll.Text = $"📋  Tất cả hội viên ({total})";
            _btnQuickNew.Text = $"🆕  Hội viên mới ({newCount})";
            _btnQuickExpiring.Text = $"⏳  Sắp hết hạn ({expiring})";
            _btnQuickVip.Text = $"💎  Hội viên VIP ({vip})";
            _btnQuickExpired.Text = $"⛔  Hết hạn / Khóa ({expiredCount})";
        }

        // =====================================================================
        // THAO TÁC SỰ KIỆN: THÊM, XEM, NÂNG CẤP
        // =====================================================================
        private void BtnAddMember_Click(object? sender, EventArgs e)
        {
            using var dlg = new frmMemberEdit(null);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ResultMember != null)
            {
                _members.Insert(0, dlg.ResultMember);
                RefreshDataGrid();
                RefreshKpis();
                MessageBox.Show($"Đã đăng ký thành công hội viên mới '{dlg.ResultMember.FullName}' ({dlg.ResultMember.Code})!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OpenMemberDetail(MemberItem member)
        {
            using var dlg = new frmMemberDetail(member);
            dlg.ShowDialog(this);
            if (dlg.DataChanged)
            {
                RefreshDataGrid();
                RefreshKpis();
            }
        }

        private void OpenMemberUpgrade(MemberItem member)
        {
            using var dlg = new frmMemberUpgrade(member);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.IsUpdated)
            {
                RefreshDataGrid();
                RefreshKpis();
            }
        }
    }
}
