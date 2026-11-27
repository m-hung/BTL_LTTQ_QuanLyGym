using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using QuanLyGym.BLL.Models;
using QuanLyGym.BLL.Services;

namespace QuanLyGym.UI.Forms.Admin
{
    public class ucAdminEquipment : UserControl
    {
        // =====================================================================
        // MÀU SẮC CHỦ ĐẠO (DARK GYM THEME)
        // =====================================================================
        private static readonly Color PageBg = Color.FromArgb(15, 15, 17);
        private static readonly Color HeaderBg = Color.FromArgb(10, 10, 12);
        private static readonly Color CardBg = Color.FromArgb(24, 24, 27);
        private static readonly Color ItemBg = Color.FromArgb(20, 20, 23);
        private static readonly Color BorderColor = Color.FromArgb(45, 45, 50);
        private static readonly Color TextColor = Color.FromArgb(244, 244, 245);
        private static readonly Color MutedColor = Color.FromArgb(161, 161, 170);

        private static readonly Color Orange = Color.FromArgb(249, 115, 22);
        private static readonly Color Blue = Color.FromArgb(59, 130, 246);
        private static readonly Color Cyan = Color.FromArgb(6, 182, 212);
        private static readonly Color Green = Color.FromArgb(34, 197, 94);
        private static readonly Color Yellow = Color.FromArgb(234, 179, 8);
        private static readonly Color Red = Color.FromArgb(239, 68, 68);

        // =====================================================================
        // DATA REPOSITORY
        // =====================================================================
        private List<EquipmentDto> _allEquipments = new();
        private List<BranchLookupDto> _branches = new();
        private List<RoomLookupDto> _rooms = new();

        // FILTER STATE
        private string _statusFilter = "Tất cả";
        private int _branchFilterId = 0;
        private int _roomFilterId = 0;
        private string _searchKeyword = string.Empty;

        // CONTROLS
        private Label _lblKpiTotalVal = null!;
        private Label _lblKpiGoodVal = null!;
        private Label _lblKpiMaintenanceVal = null!;
        private Label _lblKpiBrokenVal = null!;
        private Label _lblKpiValueVal = null!;

        private Button _btnTabAll = null!;
        private Button _btnTabGood = null!;
        private Button _btnTabMaintenance = null!;
        private Button _btnTabBroken = null!;

        private TextBox _txtSearch = null!;
        private ComboBox _cboBranch = null!;
        private ComboBox _cboRoom = null!;
        private DataGridView _grid = null!;
        private Label _lblFooterCount = null!;

        public ucAdminEquipment()
        {
            BuildUI();
            ReloadFromDatabase();
        }

        public void ReloadFromDatabase()
        {
            try
            {
                _branches = EquipmentService.Instance.GetBranchLookups();
                _rooms = EquipmentService.Instance.GetAllRooms();
                _allEquipments = EquipmentService.Instance.GetEquipments(null);

                PopulateBranchCombo();
                UpdateKpis();
                ApplyFilterAndRender();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ucAdminEquipment.ReloadFromDatabase] Error: {ex.Message}");
            }
        }

        private void BuildUI()
        {
            BackColor = PageBg;
            Dock = DockStyle.Fill;
            Padding = new Padding(16, 12, 16, 14);

            // 1. TOP HEADER BAR
            var pnlHeader = BuildHeaderBar();
            var pnlGap1 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // 2. KPI CARDS (5 thẻ)
            var pnlKpis = BuildKpiRow();
            var pnlGap2 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // 3. TOOLBAR & FILTER TABS
            var pnlToolbar = BuildToolbar();
            var pnlGap3 = new Panel { Dock = DockStyle.Top, Height = 8, BackColor = Color.Transparent };

            // 4. MAIN DATA GRID CONTAINER
            var pnlGridContainer = BuildGridContainer();

            // 5. FOOTER STATUS
            var pnlFooter = BuildFooter();

            // DOCKING STACK ORDER:
            // Grid fills remaining space, footer at bottom, and top controls stacked in order:
            // GridContainer -> Footer -> Gap3 -> Toolbar -> Gap2 -> Kpis -> Gap1 -> Header
            Controls.Add(pnlGridContainer);
            Controls.Add(pnlFooter);
            Controls.Add(pnlGap3);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlGap2);
            Controls.Add(pnlKpis);
            Controls.Add(pnlGap1);
            Controls.Add(pnlHeader);
        }

        // =====================================================================
        // 1. HEADER BAR (QUẢN LÝ DANH MỤC THIẾT BỊ)
        // =====================================================================
        private Panel BuildHeaderBar()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 54,
                BackColor = CardBg,
                Padding = new Padding(16, 8, 16, 8)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(BorderColor);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            var flpLeft = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            var lblTitle = new Label
            {
                Text = "⚙ QUẢN LÝ DANH MỤC THIẾT BỊ",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Margin = new Padding(0, 8, 12, 0)
            };
            var lblSub = new Label
            {
                Text = "|   Theo dõi tình trạng hoạt động, bảo trì & tài sản trang thiết bị phòng tập",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                AutoSize = true,
                Margin = new Padding(0, 11, 0, 0)
            };
            flpLeft.Controls.Add(lblTitle);
            flpLeft.Controls.Add(lblSub);
            pnl.Controls.Add(flpLeft);

            var flpRight = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            var btnRefresh = new Button
            {
                Text = "🔄 Làm mới",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = TextColor,
                BackColor = ItemBg,
                Size = new Size(120, 36),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 1, 8, 0)
            };
            btnRefresh.FlatAppearance.BorderColor = BorderColor;
            btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 39, 42);
            btnRefresh.Click += (s, e) => ReloadFromDatabase();
            flpRight.Controls.Add(btnRefresh);

            var btnGoMaintenance = new Button
            {
                Text = "🔧 Lịch sử bảo trì",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Cyan,
                BackColor = ItemBg,
                Size = new Size(145, 36),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 1, 8, 0)
            };
            btnGoMaintenance.FlatAppearance.BorderColor = Color.FromArgb(6, 182, 212);
            btnGoMaintenance.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 39, 42);
            btnGoMaintenance.Click += (s, e) =>
            {
                var mainForm = FindForm() as frmAdminMain;
                var btn = mainForm?.Controls.Find("btnNavMaintenance", true).FirstOrDefault() as Button;
                btn?.PerformClick();
            };
            flpRight.Controls.Add(btnGoMaintenance);

            var btnAdd = new Button
            {
                Text = "➕ Thêm thiết bị mới",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(175, 36),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 1, 0, 0)
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 88, 12);
            btnAdd.Click += BtnAdd_Click;
            flpRight.Controls.Add(btnAdd);

            pnl.Controls.Add(flpRight);
            return pnl;
        }

        // =====================================================================
        // 2. KPI CARDS ROW (5 Cards)
        // =====================================================================
        private TableLayoutPanel BuildKpiRow()
        {
            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 84,
                RowCount = 1,
                ColumnCount = 5,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };

            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));

            var cardTotal = CreateKpiCard("TỔNG SỐ THIẾT BỊ", "0", "Hệ thống toàn chuỗi", TextColor, out _lblKpiTotalVal);
            var cardGood = CreateKpiCard("HOẠT ĐỘNG TỐT", "0", "Sẵn sàng đón khách", Green, out _lblKpiGoodVal);
            var cardMaintenance = CreateKpiCard("CẦN BẢO TRÌ", "0", "Cần kỹ thuật bảo dưỡng", Orange, out _lblKpiMaintenanceVal);
            var cardBroken = CreateKpiCard("HỎNG HÓC", "0", "Tạm ngừng phục vụ", Red, out _lblKpiBrokenVal);
            var cardValue = CreateKpiCard("TỔNG GIÁ TRỊ TÀI SẢN", "0 đ", "Nguyên giá đầu tư", Cyan, out _lblKpiValueVal);

            tbl.Controls.Add(cardTotal, 0, 0);
            tbl.Controls.Add(cardGood, 1, 0);
            tbl.Controls.Add(cardMaintenance, 2, 0);
            tbl.Controls.Add(cardBroken, 3, 0);
            tbl.Controls.Add(cardValue, 4, 0);

            return tbl;
        }

        private Panel CreateKpiCard(string title, string initVal, string sub, Color valColor, out Label outVal)
        {
            var pnl = new Panel
            {
                BackColor = CardBg,
                Dock = DockStyle.Fill,
                Margin = new Padding(3),
                Padding = new Padding(12, 8, 12, 8)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(BorderColor, 1f);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = MutedColor,
                Dock = DockStyle.Top,
                Height = 16
            };

            var lblVal = new Label
            {
                Text = initVal,
                Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                ForeColor = valColor,
                Dock = DockStyle.Top,
                Height = 26
            };
            outVal = lblVal;

            var lblSub = new Label
            {
                Text = sub,
                Font = new Font("Segoe UI", 7F),
                ForeColor = Color.FromArgb(113, 113, 122),
                Dock = DockStyle.Top,
                Height = 16
            };

            pnl.Controls.Add(lblSub);
            pnl.Controls.Add(lblVal);
            pnl.Controls.Add(lblTitle);

            return pnl;
        }

        // =====================================================================
        // 3. TOOLBAR & FILTER
        // =====================================================================
        private Panel BuildToolbar()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = CardBg,
                Padding = new Padding(10, 7, 12, 7)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(BorderColor);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            // Left Quick Tabs
            var flpTabs = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            _btnTabAll = CreateFilterTab("Tất cả", true, (s, e) => SwitchTab("Tất cả"));
            _btnTabGood = CreateFilterTab("🟢 Tốt", false, (s, e) => SwitchTab("Tốt"));
            _btnTabMaintenance = CreateFilterTab("🟠 Cần bảo trì", false, (s, e) => SwitchTab("Cần bảo trì"));
            _btnTabBroken = CreateFilterTab("🔴 Hỏng hóc", false, (s, e) => SwitchTab("Hỏng hóc"));

            flpTabs.Controls.Add(_btnTabAll);
            flpTabs.Controls.Add(_btnTabGood);
            flpTabs.Controls.Add(_btnTabMaintenance);
            flpTabs.Controls.Add(_btnTabBroken);
            pnl.Controls.Add(flpTabs);

            // Right Search & Dropdowns
            var flpRight = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            // Search TextBox
            var pnlSearch = new Panel
            {
                Size = new Size(220, 34),
                BackColor = ItemBg,
                Margin = new Padding(0, 0, 8, 0),
                Padding = new Padding(8, 7, 8, 4)
            };
            pnlSearch.Paint += (s, e) =>
            {
                using var p = new Pen(BorderColor, 1f);
                e.Graphics.DrawRectangle(p, 0, 0, pnlSearch.Width - 1, pnlSearch.Height - 1);
            };

            _txtSearch = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9F),
                Dock = DockStyle.Fill,
                PlaceholderText = "🔍 Tìm tên máy, NCC..."
            };
            _txtSearch.TextChanged += (s, e) =>
            {
                _searchKeyword = _txtSearch.Text.Trim();
                ApplyFilterAndRender();
            };
            pnlSearch.Controls.Add(_txtSearch);
            flpRight.Controls.Add(pnlSearch);

            // Branch Dropdown
            _cboBranch = new ComboBox
            {
                Size = new Size(175, 34),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 8.5F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 0, 8, 0)
            };
            _cboBranch.SelectedIndexChanged += CboBranch_SelectedIndexChanged;
            flpRight.Controls.Add(_cboBranch);

            // Room Dropdown
            _cboRoom = new ComboBox
            {
                Size = new Size(165, 34),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 8.5F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 0, 0, 0)
            };
            _cboRoom.SelectedIndexChanged += (s, e) =>
            {
                if (_cboRoom.SelectedItem is RoomLookupDto r)
                    _roomFilterId = r.RoomID;
                else
                    _roomFilterId = 0;
                ApplyFilterAndRender();
            };
            flpRight.Controls.Add(_cboRoom);

            pnl.Controls.Add(flpRight);
            return pnl;
        }

        private Button CreateFilterTab(string text, bool active, EventHandler onClick)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = active ? Orange : MutedColor,
                BackColor = active ? Color.FromArgb(39, 39, 42) : Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Height = 32,
                AutoSize = true,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 1, 6, 0),
                Padding = new Padding(10, 0, 10, 0)
            };
            btn.FlatAppearance.BorderColor = active ? Orange : BorderColor;
            btn.FlatAppearance.BorderSize = 1;
            btn.Click += onClick;
            return btn;
        }

        private void SwitchTab(string targetStatus)
        {
            _statusFilter = targetStatus;

            _btnTabAll.ForeColor = _statusFilter == "Tất cả" ? Orange : MutedColor;
            _btnTabAll.BackColor = _statusFilter == "Tất cả" ? Color.FromArgb(39, 39, 42) : Color.Transparent;
            _btnTabAll.FlatAppearance.BorderColor = _statusFilter == "Tất cả" ? Orange : BorderColor;

            _btnTabGood.ForeColor = _statusFilter == "Tốt" ? Green : MutedColor;
            _btnTabGood.BackColor = _statusFilter == "Tốt" ? Color.FromArgb(39, 39, 42) : Color.Transparent;
            _btnTabGood.FlatAppearance.BorderColor = _statusFilter == "Tốt" ? Green : BorderColor;

            _btnTabMaintenance.ForeColor = _statusFilter == "Cần bảo trì" ? Orange : MutedColor;
            _btnTabMaintenance.BackColor = _statusFilter == "Cần bảo trì" ? Color.FromArgb(39, 39, 42) : Color.Transparent;
            _btnTabMaintenance.FlatAppearance.BorderColor = _statusFilter == "Cần bảo trì" ? Orange : BorderColor;

            _btnTabBroken.ForeColor = _statusFilter == "Hỏng hóc" ? Red : MutedColor;
            _btnTabBroken.BackColor = _statusFilter == "Hỏng hóc" ? Color.FromArgb(39, 39, 42) : Color.Transparent;
            _btnTabBroken.FlatAppearance.BorderColor = _statusFilter == "Hỏng hóc" ? Red : BorderColor;

            ApplyFilterAndRender();
        }

        private void PopulateBranchCombo()
        {
            _cboBranch.Items.Clear();
            _cboBranch.Items.Add(new BranchLookupDto { BranchID = 0, BranchName = "Tất cả chi nhánh" });
            foreach (var b in _branches)
            {
                _cboBranch.Items.Add(b);
            }
            _cboBranch.DisplayMember = "BranchName";
            _cboBranch.ValueMember = "BranchID";
            _cboBranch.SelectedIndex = 0;
        }

        private void CboBranch_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_cboBranch.SelectedItem is BranchLookupDto b)
                _branchFilterId = b.BranchID;
            else
                _branchFilterId = 0;

            PopulateRoomCombo(_branchFilterId);
            ApplyFilterAndRender();
        }

        private void PopulateRoomCombo(int branchId)
        {
            _cboRoom.Items.Clear();
            _cboRoom.Items.Add(new RoomLookupDto { RoomID = 0, RoomName = "Tất cả phòng tập" });

            var filteredRooms = branchId == 0
                ? _rooms
                : _rooms.Where(r => r.BranchID == branchId).ToList();

            foreach (var r in filteredRooms)
            {
                _cboRoom.Items.Add(r);
            }
            _cboRoom.DisplayMember = "RoomName";
            _cboRoom.ValueMember = "RoomID";
            _cboRoom.SelectedIndex = 0;
        }

        // =====================================================================
        // 4. MAIN DATA GRID
        // =====================================================================
        private Panel BuildGridContainer()
        {
            var pnl = new Panel
            {
                BackColor = CardBg,
                Dock = DockStyle.Fill,
                Padding = new Padding(1)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(BorderColor, 1f);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = CardBg,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(35, 35, 40),
                EnableHeadersVisualStyles = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false,
                AutoGenerateColumns = false,
                RowTemplate = { Height = 44 },
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Header Style
            _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 20, 24);
            _grid.ColumnHeadersDefaultCellStyle.ForeColor = MutedColor;
            _grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            _grid.ColumnHeadersHeight = 38;
            _grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Default Cell Style
            _grid.DefaultCellStyle.BackColor = CardBg;
            _grid.DefaultCellStyle.ForeColor = TextColor;
            _grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            _grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(39, 39, 45);
            _grid.DefaultCellStyle.SelectionForeColor = TextColor;
            _grid.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            // Columns Definition - Automatically stretching across whole screen
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCode",
                HeaderText = "MÃ MÁY",
                FillWeight = 50,
                MinimumWidth = 85,
                DefaultCellStyle = { ForeColor = Cyan, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "TÊN THIẾT BỊ",
                FillWeight = 140,
                MinimumWidth = 190,
                DefaultCellStyle = { ForeColor = Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colLocation",
                HeaderText = "CHI NHÁNH & PHÒNG TẬP",
                FillWeight = 120,
                MinimumWidth = 160,
                DefaultCellStyle = { ForeColor = MutedColor }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPurchaseDate",
                HeaderText = "NGÀY MUA",
                FillWeight = 55,
                MinimumWidth = 95,
                DefaultCellStyle = { ForeColor = TextColor, Alignment = DataGridViewContentAlignment.MiddleCenter },
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPrice",
                HeaderText = "ĐƠN GIÁ",
                FillWeight = 70,
                MinimumWidth = 110,
                DefaultCellStyle = { ForeColor = Orange, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Alignment = DataGridViewContentAlignment.MiddleRight },
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleRight } }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSupplier",
                HeaderText = "NHÀ CUNG CẤP",
                FillWeight = 95,
                MinimumWidth = 130,
                DefaultCellStyle = { ForeColor = TextColor }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "TÌNH TRẠNG",
                FillWeight = 75,
                MinimumWidth = 110,
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } }
            });

            // Action Buttons
            var colEdit = new DataGridViewButtonColumn
            {
                Name = "colEdit",
                HeaderText = "THAO TÁC",
                FillWeight = 40,
                MinimumWidth = 60,
                Text = "Sửa",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat
            };
            colEdit.DefaultCellStyle.BackColor = Color.FromArgb(39, 39, 42);
            colEdit.DefaultCellStyle.ForeColor = Orange;
            colEdit.DefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            _grid.Columns.Add(colEdit);

            var colDel = new DataGridViewButtonColumn
            {
                Name = "colDel",
                HeaderText = "",
                FillWeight = 40,
                MinimumWidth = 60,
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat
            };
            colDel.DefaultCellStyle.BackColor = Color.FromArgb(39, 39, 42);
            colDel.DefaultCellStyle.ForeColor = Red;
            colDel.DefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            _grid.Columns.Add(colDel);

            // Custom Paint for Status Badge and Action Buttons
            _grid.CellPainting += Grid_CellPainting;
            _grid.CellContentClick += Grid_CellContentClick;

            pnl.Controls.Add(_grid);
            return pnl;
        }

        private void Grid_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Paint Status Badge
            if (e.ColumnIndex == _grid.Columns["colStatus"].Index && e.Value != null)
            {
                e.PaintBackground(e.ClipBounds, true);

                string status = e.Value.ToString() ?? "";
                Color badgeBg;
                Color badgeBorder;
                Color badgeText;
                string iconText = "";

                if (status == "Tốt")
                {
                    badgeBg = Color.FromArgb(20, 83, 45); // Dark Green
                    badgeBorder = Color.FromArgb(34, 197, 94);
                    badgeText = Color.FromArgb(74, 222, 128);
                    iconText = "● Tốt";
                }
                else if (status == "Cần bảo trì")
                {
                    badgeBg = Color.FromArgb(124, 45, 18); // Dark Orange
                    badgeBorder = Color.FromArgb(249, 115, 22);
                    badgeText = Color.FromArgb(251, 146, 60);
                    iconText = "▲ Cần bảo trì";
                }
                else
                {
                    badgeBg = Color.FromArgb(127, 29, 29); // Dark Red
                    badgeBorder = Color.FromArgb(239, 68, 68);
                    badgeText = Color.FromArgb(248, 113, 113);
                    iconText = "■ Hỏng hóc";
                }

                int w = Math.Min(105, e.CellBounds.Width - 12);
                int h = 26;
                int x = e.CellBounds.X + (e.CellBounds.Width - w) / 2;
                int y = e.CellBounds.Y + (e.CellBounds.Height - h) / 2;
                var rect = new Rectangle(x, y, w, h);

                using (var brush = new SolidBrush(badgeBg))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }
                using (var pen = new Pen(badgeBorder, 1f))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
                using (var brush = new SolidBrush(badgeText))
                using (var font = new Font("Segoe UI", 8F, FontStyle.Bold))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    e.Graphics.DrawString(iconText, font, brush, rect, sf);
                }

                e.Handled = true;
            }
        }

        private void Grid_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = _grid.Rows[e.RowIndex];
            if (row.Tag is not EquipmentDto item) return;

            // Sửa
            if (e.ColumnIndex == _grid.Columns["colEdit"].Index)
            {
                using var dlg = new frmEquipmentEdit(item);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ReloadFromDatabase();
                }
            }
            // Xóa
            else if (e.ColumnIndex == _grid.Columns["colDel"].Index)
            {
                var confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa thiết bị \"{item.EquipmentName}\" ({item.DisplayCode}) khỏi hệ thống?\n\nHành động này sẽ xóa dữ liệu máy và lịch bảo dưỡng liên quan.",
                    "Xác nhận xóa thiết bị",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    var (success, msg) = EquipmentService.Instance.DeleteEquipment(item.EquipmentID);
                    if (success)
                    {
                        ReloadFromDatabase();
                    }
                    else
                    {
                        MessageBox.Show(msg, "Lỗi xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // =====================================================================
        // 5. FOOTER STATUS
        // =====================================================================
        private Panel BuildFooter()
        {
            var pnl = new Panel
            {
                Height = 32,
                BackColor = Color.Transparent,
                Dock = DockStyle.Bottom,
                Padding = new Padding(0, 8, 0, 0)
            };

            _lblFooterCount = new Label
            {
                Text = "Hiển thị 0 thiết bị",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Dock = DockStyle.Left,
                AutoSize = true
            };
            pnl.Controls.Add(_lblFooterCount);

            return pnl;
        }

        // =====================================================================
        // BUSINESS & RENDERING LOGIC
        // =====================================================================
        private void UpdateKpis()
        {
            int total = _allEquipments.Count;
            int good = _allEquipments.Count(x => x.Status == "Tốt");
            int maintenance = _allEquipments.Count(x => x.Status == "Cần bảo trì");
            int broken = _allEquipments.Count(x => x.Status == "Hỏng hóc");
            decimal totalVal = _allEquipments.Sum(x => x.Price);

            _lblKpiTotalVal.Text = total.ToString("N0");
            _lblKpiGoodVal.Text = good.ToString("N0");
            _lblKpiMaintenanceVal.Text = maintenance.ToString("N0");
            _lblKpiBrokenVal.Text = broken.ToString("N0");

            if (totalVal >= 1_000_000_000m)
                _lblKpiValueVal.Text = $"{(totalVal / 1_000_000_000m):N2} tỷ đ";
            else if (totalVal >= 1_000_000m)
                _lblKpiValueVal.Text = $"{(totalVal / 1_000_000m):N1} triệu đ";
            else
                _lblKpiValueVal.Text = $"{totalVal:N0} đ";

            // Update Tab texts with count
            _btnTabAll.Text = $"Tất cả ({total})";
            _btnTabGood.Text = $"🟢 Tốt ({good})";
            _btnTabMaintenance.Text = $"🟠 Cần bảo trì ({maintenance})";
            _btnTabBroken.Text = $"🔴 Hỏng hóc ({broken})";
        }

        private void ApplyFilterAndRender()
        {
            var filtered = _allEquipments.AsEnumerable();

            // Status filter
            if (_statusFilter != "Tất cả")
            {
                filtered = filtered.Where(x => x.Status == _statusFilter);
            }

            // Branch filter
            if (_branchFilterId > 0)
            {
                filtered = filtered.Where(x => x.BranchID == _branchFilterId);
            }

            // Room filter
            if (_roomFilterId > 0)
            {
                filtered = filtered.Where(x => x.RoomID == _roomFilterId);
            }

            // Keyword filter
            if (!string.IsNullOrWhiteSpace(_searchKeyword))
            {
                string kw = _searchKeyword.ToLower();
                filtered = filtered.Where(x =>
                    x.EquipmentName.ToLower().Contains(kw) ||
                    x.EquipmentCode.ToLower().Contains(kw) ||
                    x.Supplier.ToLower().Contains(kw) ||
                    x.RoomName.ToLower().Contains(kw) ||
                    x.BranchName.ToLower().Contains(kw)
                );
            }

            var list = filtered.ToList();

            _grid.Rows.Clear();
            foreach (var item in list)
            {
                int rowIndex = _grid.Rows.Add(
                    item.DisplayCode,
                    item.EquipmentName,
                    item.DisplayLocation,
                    item.DisplayPurchaseDate,
                    item.DisplayPrice,
                    item.Supplier,
                    item.Status
                );
                _grid.Rows[rowIndex].Tag = item;
            }

            decimal sumFiltered = list.Sum(x => x.Price);
            string sumFormatted = sumFiltered >= 1_000_000m ? $"{(sumFiltered / 1_000_000m):N1} triệu VNĐ" : $"{sumFiltered:N0} VNĐ";
            _lblFooterCount.Text = $"Hiển thị {list.Count} / {_allEquipments.Count} thiết bị | Tổng giá trị danh mục: {sumFormatted}";
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            using var dlg = new frmEquipmentEdit(null);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ReloadFromDatabase();
            }
        }
    }
}
