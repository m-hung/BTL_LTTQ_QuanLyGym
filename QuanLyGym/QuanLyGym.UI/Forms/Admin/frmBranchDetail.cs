using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public sealed class frmBranchDetail : Form
    {
        // P/Invoke để kéo thả form không viền
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        // Colors & Theme
        private static readonly Color PageBg = Color.FromArgb(15, 15, 17);
        private static readonly Color CardBg = Color.FromArgb(24, 24, 27);
        private static readonly Color ItemBg = Color.FromArgb(20, 20, 23);
        private static readonly Color Border = Color.FromArgb(45, 45, 50);
        private static readonly Color TextColor = Color.FromArgb(244, 244, 245);
        private static readonly Color MutedColor = Color.FromArgb(161, 161, 170);
        private static readonly Color DimColor = Color.FromArgb(113, 113, 122);
        private static readonly Color Orange = Color.FromArgb(249, 115, 22);
        private static readonly Color Green = Color.FromArgb(34, 197, 94);
        private static readonly Color Red = Color.FromArgb(239, 68, 68);
        private static readonly Color Blue = Color.FromArgb(59, 130, 246);
        private static readonly Color Purple = Color.FromArgb(168, 85, 247);
        private static readonly CultureInfo ViCulture = new CultureInfo("vi-VN");

        private readonly BranchModel _branch;
        private readonly List<BranchStaffItem> _staffList;
        private readonly List<BranchMemberItem> _memberList;

        public bool HasChanged { get; private set; }

        // UI Header Controls
        private Label _lblDetailName = null!;
        private Label _lblDetailCode = null!;
        private Label _lblDetailAddress = null!;
        private Label _lblDetailPhone = null!;
        private Label _lblDetailEmail = null!;
        private Label _lblDetailManager = null!;
        private Label _lblDetailStatusBadge = null!;
        private Button _btnToggleBranchStatus = null!;
        private Button _btnEditBranch = null!;

        // Mini KPI Badges
        private Label _lblKpiMembersVal = null!;
        private Label _lblKpiStaffVal = null!;
        private Label _lblKpiHoursVal = null!;
        private Label _lblKpiRevenueVal = null!;

        // Sub-tabs
        private int _selectedDetailTab = 0;
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

        public frmBranchDetail(BranchModel branch, List<BranchStaffItem> staffList, List<BranchMemberItem> memberList)
        {
            _branch = branch;
            _staffList = staffList;
            _memberList = memberList;

            BuildUI();
            UpdateHeaderAndKpis();
            SwitchDetailTab(0);
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(960, 680);
            BackColor = PageBg;
            ShowInTaskbar = false;
            DoubleBuffered = true;
            KeyPreview = true;

            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            };

            Paint += (s, e) =>
            {
                using var pen = new Pen(Border, 1.5f);
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            };

            // 1. Title bar
            var pnlTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.FromArgb(10, 10, 12),
                Padding = new Padding(16, 0, 12, 0)
            };
            pnlTitle.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };

            var lblTitleText = new Label
            {
                Text = "🏢  TRUNG TÂM QUẢN LÝ VẬN HÀNH CHI NHÁNH",
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Orange,
                AutoSize = true,
                Location = new Point(16, 12),
                UseMnemonic = false
            };

            var btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = MutedColor,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(36, 32),
                Location = new Point(pnlTitle.Width - 44, 6),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.MouseEnter += (s, e) => { btnClose.ForeColor = Color.White; btnClose.BackColor = Color.FromArgb(60, 20, 20); };
            btnClose.MouseLeave += (s, e) => { btnClose.ForeColor = MutedColor; btnClose.BackColor = Color.Transparent; };
            btnClose.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };

            pnlTitle.Resize += (s, e) =>
            {
                btnClose.Location = new Point(pnlTitle.Width - 44, 6);
            };

            pnlTitle.Controls.Add(lblTitleText);
            pnlTitle.Controls.Add(btnClose);

            // 2. Bottom Footer Bar
            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 52,
                BackColor = Color.FromArgb(20, 20, 23),
                Padding = new Padding(16, 8, 16, 8)
            };
            pnlBottom.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawLine(p, 0, 0, pnlBottom.Width, 0);
            };

            var lblBottomHint = new Label
            {
                Text = "💡 Nhấn phím ESC hoặc nút 'Đóng cửa sổ' để quay lại danh mục chi nhánh.",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = DimColor,
                AutoSize = true,
                Location = new Point(16, 16)
            };

            var btnCloseBottom = new Button
            {
                Text = "✕  Đóng cửa sổ",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = TextColor,
                BackColor = Color.FromArgb(39, 39, 42),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 36),
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                UseVisualStyleBackColor = false
            };
            btnCloseBottom.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 70);
            btnCloseBottom.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };

            pnlBottom.Controls.Add(btnCloseBottom);
            pnlBottom.Controls.Add(lblBottomHint);

            // 3. Main Body Container
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = PageBg,
                Padding = new Padding(16, 12, 16, 10)
            };

            // Section A: Branch Header Banner
            var pnlBranchHeader = BuildBranchHeaderSection();
            var pnlGap1 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // Section B: 4 Mini KPIs
            var pnlKpis = BuildBranchKpiStrip();
            var pnlGap2 = new Panel { Dock = DockStyle.Top, Height = 12, BackColor = Color.Transparent };

            // Section C: Tabs Header
            var pnlTabHeader = BuildTabHeaderSection();
            var pnlGap3 = new Panel { Dock = DockStyle.Top, Height = 8, BackColor = Color.Transparent };

            // Section D: Tab Container
            _pnlTabContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            _gridStaff = BuildStaffGridView();
            _gridMembers = BuildMemberGridView();
            _pnlFacilityContent = BuildFacilitySection();

            pnlBody.Controls.Add(_pnlTabContainer);
            pnlBody.Controls.Add(pnlGap3);
            pnlBody.Controls.Add(pnlTabHeader);
            pnlBody.Controls.Add(pnlGap2);
            pnlBody.Controls.Add(pnlKpis);
            pnlBody.Controls.Add(pnlGap1);
            pnlBody.Controls.Add(pnlBranchHeader);

            Controls.Add(pnlBody);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTitle);
        }

        // =====================================================================
        // SECTION A: BRANCH HEADER BANNER
        // =====================================================================
        private Panel BuildBranchHeaderSection()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 116,
                BackColor = CardBg,
                Padding = new Padding(16, 12, 16, 12)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            // Actions on right
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
                Size = new Size(250, 38),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                UseVisualStyleBackColor = false
            };
            _btnToggleBranchStatus.Click += (s, e) => ToggleStatus();

            _btnEditBranch = new Button
            {
                Text = "✏️ Sửa thông tin cơ sở",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = TextColor,
                BackColor = Color.FromArgb(39, 39, 42),
                Size = new Size(250, 36),
                Margin = new Padding(0, 8, 0, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                UseVisualStyleBackColor = false
            };
            _btnEditBranch.FlatAppearance.BorderColor = Color.FromArgb(63, 63, 70);
            _btnEditBranch.Click += BtnEditBranch_Click;

            pnlActions.Controls.Add(_btnToggleBranchStatus);
            pnlActions.Controls.Add(_btnEditBranch);

            // Left details
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
                Text = _branch.Name,
                Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true
            };

            _lblDetailCode = new Label
            {
                Text = _branch.Code,
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
                Text = $"📍 Địa chỉ: {_branch.Address}",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Location = new Point(0, 36),
                AutoSize = true
            };

            _lblDetailPhone = new Label
            {
                Text = $"📞 Hotline: {_branch.Phone}",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Location = new Point(0, 62),
                AutoSize = true
            };

            _lblDetailEmail = new Label
            {
                Text = $"✉️ Email: {_branch.Email}",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Location = new Point(200, 62),
                AutoSize = true
            };

            _lblDetailManager = new Label
            {
                Text = $"👔 Quản lý: {_branch.Manager}",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Orange,
                Location = new Point(410, 62),
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

        // =====================================================================
        // SECTION B: 4 MINI KPIS
        // =====================================================================
        private Panel BuildBranchKpiStrip()
        {
            var pnlWrapper = new Panel
            {
                Dock = DockStyle.Top,
                Height = 92,
                BackColor = Color.Transparent,
                Padding = new Padding(0)
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

        // =====================================================================
        // SECTION C: SUB-TABS
        // =====================================================================
        private Panel BuildTabHeaderSection()
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
                WrapContents = false
            };

            _btnTabStaff = CreateSubTabButton($"👔  Nhân viên chi nhánh ({_staffList.Count})", true);
            _btnTabMembers = CreateSubTabButton($"👥  Hội viên & Học viên ({_memberList.Count})", false);
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
                BackColor = isActive ? CardBg : Color.Transparent,
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
            _btnTabStaff.BackColor = (index == 0) ? CardBg : Color.Transparent;
            _btnTabStaff.FlatAppearance.BorderColor = (index == 0) ? Orange : Border;

            _btnTabMembers.ForeColor = (index == 1) ? Orange : MutedColor;
            _btnTabMembers.BackColor = (index == 1) ? CardBg : Color.Transparent;
            _btnTabMembers.FlatAppearance.BorderColor = (index == 1) ? Orange : Border;

            _btnTabFacility.ForeColor = (index == 2) ? Orange : MutedColor;
            _btnTabFacility.BackColor = (index == 2) ? CardBg : Color.Transparent;
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

        // =====================================================================
        // TAB 1: NHÂN VIÊN
        // =====================================================================
        private Panel BuildStaffTabWrapper()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };

            var pnlTool = new Panel { Dock = DockStyle.Top, Height = 42, BackColor = Color.Transparent, Margin = new Padding(0, 0, 0, 6) };

            var btnAddStaff = new Button
            {
                Text = "➕  Thêm nhân viên",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(170, 34),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                UseVisualStyleBackColor = false
            };
            btnAddStaff.FlatAppearance.BorderSize = 0;
            btnAddStaff.Click += BtnAddStaff_Click;

            var flpLeft = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            var pnlSearch = new Panel
            {
                Size = new Size(250, 34),
                BackColor = CardBg,
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
                BackColor = CardBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                PlaceholderText = "🔍 Tìm nhân viên theo tên/Mã/SĐT..."
            };
            _txtStaffSearch.TextChanged += (s, e) => RefreshStaffGrid();
            pnlSearch.Controls.Add(_txtStaffSearch);

            _ddStaffRole = new FilterDropdown
            {
                Size = new Size(150, 34),
                Margin = new Padding(0)
            };
            _ddStaffRole.SetItems(new[] { "Tất cả vị trí", "PT", "Thu ngân", "Tạp vụ", "Bảo vệ" });
            _ddStaffRole.SelectedIndexChanged += (s, e) => RefreshStaffGrid();

            flpLeft.Controls.Add(pnlSearch);
            flpLeft.Controls.Add(_ddStaffRole);

            pnlTool.Controls.Add(btnAddStaff);
            pnlTool.Controls.Add(flpLeft);

            var pnlGridWrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Padding = new Padding(1)
            };
            pnlGridWrapper.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlGridWrapper.Width - 1, pnlGridWrapper.Height - 1);
            };
            pnlGridWrapper.Controls.Add(_gridStaff);

            pnl.Controls.Add(pnlGridWrapper);
            pnl.Controls.Add(pnlTool);
            return pnl;
        }

        private DataGridView BuildStaffGridView()
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
                RowTemplate = { Height = 40 },
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
            dgv.ColumnHeadersHeight = 36;

            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = CardBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9F),
                SelectionBackColor = Color.FromArgb(39, 39, 45),
                SelectionForeColor = Color.White,
                Padding = new Padding(8, 0, 0, 0)
            };

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "StaffId", HeaderText = "Mã NV", FillWeight = 110, MinimumWidth = 90 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Họ và tên", FillWeight = 170, MinimumWidth = 140 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Role", HeaderText = "Chức vụ", FillWeight = 95, MinimumWidth = 80 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", HeaderText = "Số điện thoại", FillWeight = 115, MinimumWidth = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Shift", HeaderText = "Ca làm việc", FillWeight = 165, MinimumWidth = 140 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Trạng thái", FillWeight = 125, MinimumWidth = 110 });

            var btnCol = new DataGridViewButtonColumn
            {
                Name = "Action",
                HeaderText = "Thao tác",
                Text = "🗑️ Xóa",
                UseColumnTextForButtonValue = true,
                FillWeight = 70,
                MinimumWidth = 65,
                FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(btnCol);

            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dgv.Columns["Action"]!.Index)
                {
                    string id = dgv.Rows[e.RowIndex].Cells["StaffId"].Value?.ToString() ?? "";
                    string name = dgv.Rows[e.RowIndex].Cells["Name"].Value?.ToString() ?? "";
                    if (MessageBox.Show($"Xác nhận xóa nhân viên '{name}' ({id}) khỏi cơ sở {_branch.Name}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        _staffList.RemoveAll(x => x.StaffId == id);
                        _branch.StaffCount = _staffList.Count;
                        HasChanged = true;
                        RefreshStaffGrid();
                        UpdateHeaderAndKpis();
                    }
                }
            };

            return dgv;
        }

        private void RefreshStaffGrid()
        {
            if (_gridStaff == null) return;
            _gridStaff.Rows.Clear();

            var list = _staffList.AsEnumerable();

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

            _btnTabStaff.Text = $"👔  Nhân viên chi nhánh ({_staffList.Count})";
        }

        // =====================================================================
        // TAB 2: HỌC VIÊN
        // =====================================================================
        private Panel BuildMemberTabWrapper()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };

            var pnlTool = new Panel { Dock = DockStyle.Top, Height = 42, BackColor = Color.Transparent, Margin = new Padding(0, 0, 0, 6) };

            var btnAddMember = new Button
            {
                Text = "➕  Đăng ký học viên",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(170, 34),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false,
                UseVisualStyleBackColor = false
            };
            btnAddMember.FlatAppearance.BorderSize = 0;
            btnAddMember.Click += BtnAddMember_Click;

            var flpLeft = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            var pnlSearch = new Panel
            {
                Size = new Size(250, 34),
                BackColor = CardBg,
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
                BackColor = CardBg,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                PlaceholderText = "🔍 Tìm học viên theo tên/Mã/SĐT..."
            };
            _txtMemberSearch.TextChanged += (s, e) => RefreshMemberGrid();
            pnlSearch.Controls.Add(_txtMemberSearch);

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

            var pnlGridWrapper = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Padding = new Padding(1)
            };
            pnlGridWrapper.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlGridWrapper.Width - 1, pnlGridWrapper.Height - 1);
            };
            pnlGridWrapper.Controls.Add(_gridMembers);

            pnl.Controls.Add(pnlGridWrapper);
            pnl.Controls.Add(pnlTool);
            return pnl;
        }

        private DataGridView BuildMemberGridView()
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
                RowTemplate = { Height = 40 },
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
            dgv.ColumnHeadersHeight = 36;

            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = CardBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9F),
                SelectionBackColor = Color.FromArgb(39, 39, 45),
                SelectionForeColor = Color.White,
                Padding = new Padding(8, 0, 0, 0)
            };

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "MemberId", HeaderText = "Mã HV", FillWeight = 95, MinimumWidth = 80 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Họ và tên", FillWeight = 160, MinimumWidth = 130 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone", HeaderText = "Số điện thoại", FillWeight = 115, MinimumWidth = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Package", HeaderText = "Gói tập", FillWeight = 160, MinimumWidth = 130 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "PT", HeaderText = "HLV phụ trách", FillWeight = 120, MinimumWidth = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Expire", HeaderText = "Hạn gói", FillWeight = 95, MinimumWidth = 85 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Trạng thái", FillWeight = 105, MinimumWidth = 90 });

            var btnCol = new DataGridViewButtonColumn
            {
                Name = "Action",
                HeaderText = "Thao tác",
                Text = "🗑️ Xóa",
                UseColumnTextForButtonValue = true,
                FillWeight = 70,
                MinimumWidth = 65,
                FlatStyle = FlatStyle.Flat
            };
            dgv.Columns.Add(btnCol);

            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dgv.Columns["Action"]!.Index)
                {
                    string id = dgv.Rows[e.RowIndex].Cells["MemberId"].Value?.ToString() ?? "";
                    string name = dgv.Rows[e.RowIndex].Cells["Name"].Value?.ToString() ?? "";
                    if (MessageBox.Show($"Xác nhận xóa học viên '{name}' ({id}) khỏi cơ sở {_branch.Name}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        _memberList.RemoveAll(x => x.MemberId == id);
                        _branch.MemberCount = _memberList.Count;
                        HasChanged = true;
                        RefreshMemberGrid();
                        UpdateHeaderAndKpis();
                    }
                }
            };

            return dgv;
        }

        private void RefreshMemberGrid()
        {
            if (_gridMembers == null) return;
            _gridMembers.Rows.Clear();

            var list = _memberList.AsEnumerable();

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

            _btnTabMembers.Text = $"👥  Hội viên & Học viên ({_memberList.Count})";
        }

        // =====================================================================
        // TAB 3: CƠ SỞ VẬT CHẤT
        // =====================================================================
        private Panel BuildFacilitySection()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = CardBg,
                Padding = new Padding(16, 14, 16, 14)
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

            stack.Controls.Add(CreateFacilityCard("📐 DIỆN TÍCH HOẠT ĐỘNG", $"{_branch.AreaSqM:N0} m² sàn tập luyện", "Thiết kế không gian mở, trang bị máy lạnh công nghiệp & lọc không khí HEPA."), 0, 0);
            stack.Controls.Add(CreateFacilityCard("🏊 TIỆN ÍCH ĐẶC QUYỀN", "Bể bơi • Sauna • Boxing Ring", "Phòng xông hơi khô/ướt đá muối Himalaya, bể bơi 4 mùa nước ấm."), 1, 0);

            stack.Controls.Add(CreateFacilityCard("🅿️ ĐỖ XE & AN NINH", "Bãi giữ xe ô tô & xe máy miễn phí", "Hệ thống Camera AI 24/7, thẻ từ thông minh SmartLock kiểm soát ra vào."), 0, 1);
            stack.Controls.Add(CreateFacilityCard("🥤 QUẦY BAR NƯỚC & PHỤ KIỆN", "Protein Shake & Bổ sung điện giải", "Phục vụ thức uống thể thao, khăn tập miễn phí và tủ đồ cá nhân."), 1, 1);

            stack.Controls.Add(CreateFacilityCard("📋 GHI CHÚ VẬN HÀNH", _branch.Note, "Liên hệ quản lý cơ sở khi có sự cố kỹ thuật hoặc bảo trì thiết bị."), 0, 2);
            stack.SetColumnSpan(stack.GetControlFromPosition(0, 2)!, 2);

            _pnlFacilityContent.Controls.Add(stack);
        }

        private Panel CreateFacilityCard(string title, string mainText, string subText)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ItemBg,
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
        // ACTIONS & SYNCS
        // =====================================================================
        private void UpdateHeaderAndKpis()
        {
            _lblDetailName.Text = _branch.Name;
            _lblDetailCode.Text = _branch.Code;
            _lblDetailAddress.Text = $"📍 Địa chỉ: {_branch.Address}";
            _lblDetailPhone.Text = $"📞 Hotline: {_branch.Phone}";
            _lblDetailEmail.Text = $"✉️ Email: {_branch.Email}";
            _lblDetailManager.Text = $"👔 Quản lý: {_branch.Manager}";

            if (_branch.IsOnline)
            {
                _lblDetailStatusBadge.Text = "🟢 HOẠT ĐỘNG (ONLINE)";
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

            _lblKpiMembersVal.Text = $"{_memberList.Count:N0} người";
            _lblKpiStaffVal.Text = $"{_staffList.Count:N0} nhân sự";
            _lblKpiHoursVal.Text = $"{_branch.OpenTime} - {_branch.CloseTime}";
            _lblKpiRevenueVal.Text = _branch.MonthlyRevenue.ToString("N0", ViCulture) + " ₫";

            _btnTabStaff.Text = $"👔  Nhân viên chi nhánh ({_staffList.Count})";
            _btnTabMembers.Text = $"👥  Hội viên & Học viên ({_memberList.Count})";
        }

        private void ToggleStatus()
        {
            string actionName = _branch.IsOnline ? "TẠM ĐÓNG CỬA (OFFLINE)" : "BẬT HOẠT ĐỘNG (ONLINE)";
            string prompt = _branch.IsOnline
                ? $"Bạn có chắc chắn muốn TẠM ĐÓNG cửa chi nhánh '{_branch.Name}'?\n(Hệ thống sẽ chuyển chi nhánh sang trạng thái bảo trì/ngoại tuyến)"
                : $"Bạn có chắc chắn muốn MỞ CỬA HOẠT ĐỘNG lại cho chi nhánh '{_branch.Name}'?";

            if (MessageBox.Show(prompt, "Xác nhận chuyển trạng thái", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _branch.IsOnline = !_branch.IsOnline;
                HasChanged = true;
                UpdateHeaderAndKpis();
                MessageBox.Show($"Đã chuyển trạng thái chi nhánh '{_branch.Name}' thành: {actionName} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnEditBranch_Click(object? sender, EventArgs e)
        {
            using var dlg = new frmBranchEdit(_branch);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ResultBranch != null)
            {
                HasChanged = true;
                UpdateHeaderAndKpis();
                MessageBox.Show($"Cập nhật thông tin chi nhánh '{_branch.Name}' thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAddStaff_Click(object? sender, EventArgs e)
        {
            using var dlg = new frmBranchStaffAdd(_branch.Name);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ResultStaff != null)
            {
                _staffList.Add(dlg.ResultStaff);
                _branch.StaffCount = _staffList.Count;
                HasChanged = true;
                RefreshStaffGrid();
                UpdateHeaderAndKpis();
                MessageBox.Show($"Đã phân công nhân viên '{dlg.ResultStaff.Name}' vào chi nhánh {_branch.Name} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnAddMember_Click(object? sender, EventArgs e)
        {
            using var dlg = new frmBranchMemberAdd(_branch.Name);
            if (dlg.ShowDialog(this) == DialogResult.OK && dlg.ResultMember != null)
            {
                _memberList.Add(dlg.ResultMember);
                _branch.MemberCount = _memberList.Count;
                HasChanged = true;
                RefreshMemberGrid();
                UpdateHeaderAndKpis();
                MessageBox.Show($"Đã đăng ký học viên '{dlg.ResultMember.Name}' tại chi nhánh {_branch.Name} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
