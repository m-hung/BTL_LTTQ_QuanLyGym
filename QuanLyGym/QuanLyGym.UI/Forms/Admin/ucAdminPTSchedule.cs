using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using QuanLyGym.BLL.Models;
using QuanLyGym.BLL.Services;

namespace QuanLyGym.UI.Forms.Admin
{
    public class ucAdminPTSchedule : UserControl
    {
        // Colors - Dark Gym Theme
        private static readonly Color BgColor = Color.FromArgb(15, 15, 17);
        private static readonly Color CardBg = Color.FromArgb(24, 24, 27);
        private static readonly Color ItemBg = Color.FromArgb(20, 20, 23);
        private static readonly Color BorderColor = Color.FromArgb(45, 45, 50);
        private static readonly Color TextColor = Color.FromArgb(244, 244, 245);
        private static readonly Color MutedColor = Color.FromArgb(161, 161, 170);
        private static readonly Color Orange = Color.FromArgb(249, 115, 22);
        private static readonly Color Cyan = Color.FromArgb(6, 182, 212);
        private static readonly Color Blue = Color.FromArgb(56, 189, 248);
        private static readonly Color Green = Color.FromArgb(34, 197, 94);
        private static readonly Color Yellow = Color.FromArgb(234, 179, 8);
        private static readonly Color Red = Color.FromArgb(239, 68, 68);

        // Controls
        private Label _lblKpiTotal = null!;
        private Label _lblKpiToday = null!;
        private Label _lblKpiCompleted = null!;
        private Label _lblKpiPending = null!;
        private Label _lblKpiPTCount = null!;

        private TextBox _txtSearch = null!;
        private ComboBox _cboPTFilter = null!;
        private ComboBox _cboDateFilter = null!;
        private Button _btnTabAll = null!;
        private Button _btnTabApproved = null!;
        private Button _btnTabCompleted = null!;
        private Button _btnTabPending = null!;
        private Button _btnTabCancelled = null!;

        private Label _lblGridCount = null!;
        private DataGridView _grid = null!;

        // Data State
        private List<ScheduleDto> _allSchedules = new();
        private List<ScheduleDto> _filteredSchedules = new();
        private List<PTLookupDto> _pts = new();
        private string _statusFilter = "Tất cả";
        private string _searchKeyword = "";
        private int? _selectedPTID = null;
        private string _dateFilter = "Tất cả";

        public ucAdminPTSchedule()
        {
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            BackColor = BgColor;

            BuildUI();
            LoadData();
        }

        private void BuildUI()
        {
            var pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BgColor,
                Padding = new Padding(16, 12, 16, 16),
                AutoScroll = true
            };
            Controls.Add(pnlMain);

            // 1. Header Bar
            var pnlHeader = BuildHeaderBar();
            pnlMain.Controls.Add(pnlHeader);

            // Spacing
            var spacer1 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };
            pnlMain.Controls.Add(spacer1);

            // 2. 5 KPI Cards
            var pnlKpis = BuildKpiSection();
            pnlMain.Controls.Add(pnlKpis);

            // Spacing
            var spacer2 = new Panel { Dock = DockStyle.Top, Height = 14, BackColor = Color.Transparent };
            pnlMain.Controls.Add(spacer2);

            // 3. Toolbar (Tìm kiếm & Bộ lọc)
            var pnlToolbar = BuildToolbar();
            pnlMain.Controls.Add(pnlToolbar);

            // Spacing
            var spacer3 = new Panel { Dock = DockStyle.Top, Height = 12, BackColor = Color.Transparent };
            pnlMain.Controls.Add(spacer3);

            // 4. Bảng DataGridView danh sách Lịch Hẹn PT
            var pnlGrid = BuildGridSection();
            pnlMain.Controls.Add(pnlGrid);

            // Thứ tự hiển thị docking
            pnlHeader.BringToFront();
            spacer1.BringToFront();
            pnlKpis.BringToFront();
            spacer2.BringToFront();
            pnlToolbar.BringToFront();
            spacer3.BringToFront();
            pnlGrid.BringToFront();
        }

        // =====================================================================
        // 1. HEADER BAR
        // =====================================================================
        private Panel BuildHeaderBar()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.Transparent
            };

            var pnlLeft = new Panel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                BackColor = Color.Transparent
            };

            var lblTitle = new Label
            {
                Text = "📅 LỊCH HẸN HUẤN LUYỆN VIÊN (PT 1-1)",
                Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                ForeColor = TextColor,
                Dock = DockStyle.Top,
                Height = 32
            };

            var lblSubtitle = new Label
            {
                Text = "Quản lý lịch đặt tập 1-1, kiểm soát thời khóa biểu HLV, phê duyệt lịch hẹn và theo dõi bài tập hội viên",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = MutedColor,
                Dock = DockStyle.Top,
                Height = 22
            };

            pnlLeft.Controls.Add(lblSubtitle);
            pnlLeft.Controls.Add(lblTitle);
            pnl.Controls.Add(pnlLeft);

            // Actions Right
            var flpRight = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 8, 0, 0)
            };

            var btnRefresh = new Button
            {
                Text = "🔄 Làm mới",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = TextColor,
                BackColor = CardBg,
                FlatStyle = FlatStyle.Flat,
                Height = 40,
                Width = 120,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 10, 0)
            };
            btnRefresh.FlatAppearance.BorderColor = BorderColor;
            btnRefresh.Click += (s, e) => LoadData();
            flpRight.Controls.Add(btnRefresh);

            var btnAdd = new Button
            {
                Text = "➕ Đặt lịch tập 1-1",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                FlatStyle = FlatStyle.Flat,
                Height = 40,
                Width = 165,
                Cursor = Cursors.Hand,
                Margin = new Padding(0)
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;
            flpRight.Controls.Add(btnAdd);

            pnl.Controls.Add(flpRight);
            return pnl;
        }

        // =====================================================================
        // 2. 5 KPI CARDS
        // =====================================================================
        private Panel BuildKpiSection()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 92,
                BackColor = Color.Transparent
            };

            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 5,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            for (int i = 0; i < 5; i++)
            {
                tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            }

            tbl.Controls.Add(CreateKpiCard("TỔNG LỊCH ĐẶT", "0 buổi", "Toàn hệ thống", Orange, out _lblKpiTotal), 0, 0);
            tbl.Controls.Add(CreateKpiCard("LỊCH TẬP HÔM NAY", "0 buổi", "Đang diễn ra", Cyan, out _lblKpiToday), 1, 0);
            tbl.Controls.Add(CreateKpiCard("ĐÃ HOÀN THÀNH", "0 buổi", "Đã xác nhận", Blue, out _lblKpiCompleted), 2, 0);
            tbl.Controls.Add(CreateKpiCard("CHỜ PHÊ DUYỆT", "0 yêu cầu", "Cần admin/PT duyệt", Yellow, out _lblKpiPending), 3, 0);
            tbl.Controls.Add(CreateKpiCard("ĐỘI NGŨ HLV (PT)", "0 PT", "Sẵn sàng huấn luyện", Green, out _lblKpiPTCount), 4, 0);

            pnl.Controls.Add(tbl);
            return pnl;
        }

        private Panel CreateKpiCard(string title, string defaultVal, string sub, Color accentColor, out Label outVal)
        {
            var pnl = new Panel
            {
                BackColor = CardBg,
                Dock = DockStyle.Fill,
                Margin = new Padding(4, 0, 4, 0),
                Padding = new Padding(14, 10, 14, 8)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(BorderColor, 1.2f);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = MutedColor,
                Dock = DockStyle.Top,
                Height = 18
            };

            var lblVal = new Label
            {
                Text = defaultVal,
                Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                ForeColor = accentColor,
                Dock = DockStyle.Top,
                Height = 28
            };
            outVal = lblVal;

            var lblSub = new Label
            {
                Text = sub,
                Font = new Font("Segoe UI", 7.5F),
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
        // 3. TOOLBAR TÌM KIẾM & BỘ LỌC
        // =====================================================================
        private Panel BuildToolbar()
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
                using var p = new Pen(BorderColor);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            // Search Box (Left)
            var pnlSearch = new Panel
            {
                Dock = DockStyle.Left,
                Width = 360,
                Height = 36,
                BackColor = ItemBg,
                Padding = new Padding(10, 8, 8, 4)
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
                PlaceholderText = "🔍 Tìm theo tên hội viên, SĐT, HLV, bài tập..."
            };
            _txtSearch.TextChanged += (s, e) =>
            {
                _searchKeyword = _txtSearch.Text.Trim();
                ApplyFilterAndRenderGrid();
            };
            pnlSearch.Controls.Add(_txtSearch);
            pnl.Controls.Add(pnlSearch);

            // Filter Combobox PT (Next to search)
            _cboPTFilter = new ComboBox
            {
                Dock = DockStyle.Left,
                Width = 220,
                Height = 34,
                BackColor = ItemBg,
                ForeColor = Cyan,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(10, 0, 0, 0)
            };
            _cboPTFilter.Items.Add("Tất cả Huấn luyện viên (PT)");
            _cboPTFilter.SelectedIndex = 0;
            _cboPTFilter.SelectedIndexChanged += (s, e) =>
            {
                if (_cboPTFilter.SelectedIndex == 0)
                {
                    _selectedPTID = null;
                }
                else if (_cboPTFilter.SelectedIndex - 1 < _pts.Count)
                {
                    _selectedPTID = _pts[_cboPTFilter.SelectedIndex - 1].PTID;
                }
                ApplyFilterAndRenderGrid();
            };

            var pnlCboWrapper = new Panel
            {
                Dock = DockStyle.Left,
                Width = 230,
                Padding = new Padding(10, 0, 0, 0),
                BackColor = Color.Transparent
            };
            pnlCboWrapper.Controls.Add(_cboPTFilter);
            pnl.Controls.Add(pnlCboWrapper);

            // Filter Combobox Date (Next)
            _cboDateFilter = new ComboBox
            {
                Dock = DockStyle.Left,
                Width = 140,
                Height = 34,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 8.5F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            _cboDateFilter.Items.AddRange(new object[] { "Tất cả ngày", "Hôm nay", "Tuần này", "Tháng này" });
            _cboDateFilter.SelectedIndex = 0;
            _cboDateFilter.SelectedIndexChanged += (s, e) =>
            {
                _dateFilter = _cboDateFilter.SelectedItem?.ToString() == "Tất cả ngày" ? "Tất cả" : (_cboDateFilter.SelectedItem?.ToString() ?? "Tất cả");
                ApplyFilterAndRenderGrid();
            };

            var pnlDateWrapper = new Panel
            {
                Dock = DockStyle.Left,
                Width = 150,
                Padding = new Padding(10, 0, 0, 0),
                BackColor = Color.Transparent
            };
            pnlDateWrapper.Controls.Add(_cboDateFilter);
            pnl.Controls.Add(pnlDateWrapper);

            // Status Filter Tabs (Right)
            var flpTabs = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            _btnTabAll = CreateFilterTab("Tất cả", true, (s, e) => SwitchTab("Tất cả"));
            _btnTabApproved = CreateFilterTab("✔ Đã duyệt", false, (s, e) => SwitchTab("Đã duyệt"));
            _btnTabCompleted = CreateFilterTab("★ Hoàn thành", false, (s, e) => SwitchTab("Hoàn thành"));
            _btnTabPending = CreateFilterTab("⏳ Chờ duyệt", false, (s, e) => SwitchTab("Chờ duyệt"));
            _btnTabCancelled = CreateFilterTab("✕ Đã hủy", false, (s, e) => SwitchTab("Đã hủy"));

            flpTabs.Controls.Add(_btnTabAll);
            flpTabs.Controls.Add(_btnTabApproved);
            flpTabs.Controls.Add(_btnTabCompleted);
            flpTabs.Controls.Add(_btnTabPending);
            flpTabs.Controls.Add(_btnTabCancelled);
            pnl.Controls.Add(flpTabs);

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
                Height = 34,
                AutoSize = true,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 5, 0),
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

            UpdateButtonState(_btnTabAll, _statusFilter == "Tất cả", Orange);
            UpdateButtonState(_btnTabApproved, _statusFilter == "Đã duyệt", Green);
            UpdateButtonState(_btnTabCompleted, _statusFilter == "Hoàn thành", Blue);
            UpdateButtonState(_btnTabPending, _statusFilter == "Chờ duyệt", Yellow);
            UpdateButtonState(_btnTabCancelled, _statusFilter == "Đã hủy", Red);

            ApplyFilterAndRenderGrid();
        }

        private void UpdateButtonState(Button btn, bool active, Color accent)
        {
            btn.ForeColor = active ? accent : MutedColor;
            btn.BackColor = active ? Color.FromArgb(39, 39, 42) : Color.Transparent;
            btn.FlatAppearance.BorderColor = active ? accent : BorderColor;
        }

        // =====================================================================
        // 4. BẢNG DATAGRIDVIEW DANH SÁCH LỊCH HẸN PT (Giống hệt ảnh mẫu)
        // =====================================================================
        private Panel BuildGridSection()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Padding = new Padding(16, 12, 16, 16)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(BorderColor);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            // Section Header
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = Color.Transparent
            };

            var lblGridTitle = new Label
            {
                Text = "📋 Danh Sách Lịch Tập Huấn Luyện 1-1",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = TextColor,
                Dock = DockStyle.Left,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlHeader.Controls.Add(lblGridTitle);

            _lblGridCount = new Label
            {
                Text = "Tổng cộng: 0 buổi tập",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Blue,
                BackColor = Color.FromArgb(30, 58, 138),
                Dock = DockStyle.Right,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(10, 4, 10, 4)
            };
            pnlHeader.Controls.Add(_lblGridCount);
            pnl.Controls.Add(pnlHeader);

            // DataGridView setup
            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = CardBg,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(35, 35, 40),
                EnableHeadersVisualStyles = false,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                RowTemplate = { Height = 66 }
            };

            // Header Style
            _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 20, 24);
            _grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(161, 161, 170);
            _grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            _grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
            _grid.ColumnHeadersHeight = 38;

            // Row Default Style
            _grid.DefaultCellStyle.BackColor = CardBg;
            _grid.DefaultCellStyle.ForeColor = TextColor;
            _grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(35, 37, 45);
            _grid.DefaultCellStyle.SelectionForeColor = TextColor;
            _grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            _grid.DefaultCellStyle.Padding = new Padding(8, 4, 8, 4);

            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Columns Definition
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "MÃ LỊCH",
                FillWeight = 42,
                MinimumWidth = 65,
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMember",
                HeaderText = "HỘI VIÊN",
                FillWeight = 110,
                MinimumWidth = 140
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPT",
                HeaderText = "HUẤN LUYỆN VIÊN (PT)",
                FillWeight = 160,
                MinimumWidth = 190
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDate",
                HeaderText = "NGÀY TẬP",
                FillWeight = 75,
                MinimumWidth = 95
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTime",
                HeaderText = "KHUNG GIỜ",
                FillWeight = 85,
                MinimumWidth = 110,
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "TRẠNG THÁI",
                FillWeight = 75,
                MinimumWidth = 95,
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } },
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colWorkout",
                HeaderText = "NỘI DUNG BÀI TẬP & GHI CHÚ",
                FillWeight = 175,
                MinimumWidth = 200
            });

            // Action Buttons
            var colAction = new DataGridViewButtonColumn
            {
                Name = "colAction",
                HeaderText = "THAO TÁC",
                FillWeight = 55,
                MinimumWidth = 75,
                Text = "Duyệt",
                UseColumnTextForButtonValue = false,
                FlatStyle = FlatStyle.Flat
            };
            colAction.DefaultCellStyle.BackColor = Color.FromArgb(39, 39, 42);
            colAction.DefaultCellStyle.ForeColor = Green;
            colAction.DefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            _grid.Columns.Add(colAction);

            var colEdit = new DataGridViewButtonColumn
            {
                Name = "colEdit",
                HeaderText = "",
                FillWeight = 45,
                MinimumWidth = 60,
                Text = "Sửa",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat
            };
            colEdit.DefaultCellStyle.BackColor = Color.FromArgb(39, 39, 42);
            colEdit.DefaultCellStyle.ForeColor = Blue;
            colEdit.DefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            _grid.Columns.Add(colEdit);

            var colDel = new DataGridViewButtonColumn
            {
                Name = "colDel",
                HeaderText = "",
                FillWeight = 45,
                MinimumWidth = 60,
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat
            };
            colDel.DefaultCellStyle.BackColor = Color.FromArgb(39, 39, 42);
            colDel.DefaultCellStyle.ForeColor = Red;
            colDel.DefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            _grid.Columns.Add(colDel);

            // Custom Painting & Events
            _grid.CellPainting += Grid_CellPainting;
            _grid.CellContentClick += Grid_CellContentClick;

            pnl.Controls.Add(_grid);
            _grid.BringToFront();

            return pnl;
        }

        // =====================================================================
        // DATA LOADING & FILTERING
        // =====================================================================
        public void LoadData()
        {
            try
            {
                // Ensure sample data from DB
                ScheduleService.Instance.EnsureInitialSchedules();

                // Load PT list for combobox
                _pts = ScheduleService.Instance.GetPTsForBooking();
                string currentPTSelection = _cboPTFilter.SelectedItem?.ToString() ?? "";
                _cboPTFilter.Items.Clear();
                _cboPTFilter.Items.Add("Tất cả Huấn luyện viên (PT)");
                foreach (var pt in _pts)
                {
                    _cboPTFilter.Items.Add(pt.FullName);
                }
                int ptIdx = _cboPTFilter.FindStringExact(currentPTSelection);
                _cboPTFilter.SelectedIndex = ptIdx >= 0 ? ptIdx : 0;

                // Load summary KPIs
                var summary = ScheduleService.Instance.GetSummary();
                _lblKpiTotal.Text = $"{summary.TotalSchedules} buổi";
                _lblKpiToday.Text = $"{summary.TodayCount} buổi";
                _lblKpiCompleted.Text = $"{summary.CompletedCount} buổi";
                _lblKpiPending.Text = $"{summary.PendingCount} yêu cầu";
                _lblKpiPTCount.Text = $"{summary.ActivePTCount} PT";

                // Load all schedules
                _allSchedules = ScheduleService.Instance.GetSchedules();

                ApplyFilterAndRenderGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách lịch hẹn PT: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilterAndRenderGrid()
        {
            var filter = new ScheduleFilterDto
            {
                Keyword = _searchKeyword,
                PTID = _selectedPTID,
                Status = _statusFilter,
                DateFilter = _dateFilter
            };

            _filteredSchedules = ScheduleService.Instance.GetSchedules(filter);
            _lblGridCount.Text = $"Tổng cộng: {_filteredSchedules.Count} buổi tập";

            _grid.Rows.Clear();
            foreach (var item in _filteredSchedules)
            {
                int rowIdx = _grid.Rows.Add();
                var row = _grid.Rows[rowIdx];
                row.Tag = item;

                row.Cells["colId"].Value = item.ScheduleCode;
                row.Cells["colMember"].Value = item.MemberName;
                row.Cells["colPT"].Value = item.PTName;
                row.Cells["colDate"].Value = item.TrainingDateDisplay;
                row.Cells["colTime"].Value = item.TimeSlotDisplay;
                row.Cells["colStatus"].Value = item.Status;
                row.Cells["colWorkout"].Value = item.FullDescription;

                // Quick Action Text
                if (item.Status == "Chờ duyệt")
                    row.Cells["colAction"].Value = "Duyệt";
                else if (item.Status == "Đã duyệt")
                    row.Cells["colAction"].Value = "Xong";
                else if (item.Status == "Hoàn thành")
                    row.Cells["colAction"].Value = "Xem";
                else
                    row.Cells["colAction"].Value = "Mở lại";
            }
        }

        // =====================================================================
        // CELL PAINTING (Vẽ 2 dòng thông tin giống hệt ảnh mẫu)
        // =====================================================================
        private void Grid_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _grid.Rows.Count) return;
            if (_grid.Rows[e.RowIndex].Tag is not ScheduleDto item) return;

            // 1. Cột Hội viên: Dòng 1 Tên đậm, Dòng 2 SĐT xám
            if (e.ColumnIndex == _grid.Columns["colMember"].Index)
            {
                e.PaintBackground(e.ClipBounds, true);

                using (var brushName = new SolidBrush(Color.White))
                using (var brushPhone = new SolidBrush(MutedColor))
                using (var fontName = new Font("Segoe UI", 9F, FontStyle.Bold))
                using (var fontPhone = new Font("Segoe UI", 8F))
                {
                    int x = e.CellBounds.X + 8;
                    int y = e.CellBounds.Y + 12;

                    e.Graphics.DrawString(item.MemberName, fontName, brushName, x, y);
                    e.Graphics.DrawString(item.MemberPhone, fontPhone, brushPhone, x, y + 20);
                }

                e.Handled = true;
                return;
            }

            // 2. Cột Huấn luyện viên (PT): Dòng 1 Tên màu xanh Blue, Dòng 2 Chuyên môn
            if (e.ColumnIndex == _grid.Columns["colPT"].Index)
            {
                e.PaintBackground(e.ClipBounds, true);

                using (var brushName = new SolidBrush(Blue))
                using (var brushSpec = new SolidBrush(Color.FromArgb(148, 163, 184)))
                using (var fontName = new Font("Segoe UI", 9F, FontStyle.Bold))
                using (var fontSpec = new Font("Segoe UI", 7.8F))
                {
                    int x = e.CellBounds.X + 8;
                    int y = e.CellBounds.Y + 12;

                    e.Graphics.DrawString(item.PTName, fontName, brushName, x, y);

                    // Giới hạn độ dài chuyên môn để không tràn
                    string spec = item.PTSpecialization;
                    if (spec.Length > 42) spec = spec.Substring(0, 39) + "...";
                    e.Graphics.DrawString(spec, fontSpec, brushSpec, x, y + 20);
                }

                e.Handled = true;
                return;
            }

            // 3. Cột Ngày tập: Dòng 1 dd/MM/yyyy, Dòng 2 Thứ trong tuần
            if (e.ColumnIndex == _grid.Columns["colDate"].Index)
            {
                e.PaintBackground(e.ClipBounds, true);

                using (var brushDate = new SolidBrush(Color.White))
                using (var brushDay = new SolidBrush(MutedColor))
                using (var fontDate = new Font("Segoe UI", 9F, FontStyle.Bold))
                using (var fontDay = new Font("Segoe UI", 8F))
                {
                    int x = e.CellBounds.X + 8;
                    int y = e.CellBounds.Y + 12;

                    e.Graphics.DrawString(item.TrainingDateDisplay, fontDate, brushDate, x, y);
                    e.Graphics.DrawString(item.DayOfWeekDisplay, fontDay, brushDay, x, y + 20);
                }

                e.Handled = true;
                return;
            }

            // 4. Cột Khung giờ: Badge pill với icon đồng hồ
            if (e.ColumnIndex == _grid.Columns["colTime"].Index)
            {
                e.PaintBackground(e.ClipBounds, true);

                Color badgeBg = Color.FromArgb(20, 24, 33);
                Color badgeBorder = Color.FromArgb(51, 65, 85);
                Color badgeText = Color.FromArgb(186, 230, 253);

                int w = Math.Min(115, e.CellBounds.Width - 8);
                int h = 28;
                int x = e.CellBounds.X + (e.CellBounds.Width - w) / 2;
                int y = e.CellBounds.Y + (e.CellBounds.Height - h) / 2;
                var rect = new Rectangle(x, y, w, h);

                using (var brush = new SolidBrush(badgeBg)) e.Graphics.FillRectangle(brush, rect);
                using (var pen = new Pen(badgeBorder, 1f)) e.Graphics.DrawRectangle(pen, rect);
                using (var brush = new SolidBrush(badgeText))
                using (var font = new Font("Segoe UI", 8F, FontStyle.Bold))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    e.Graphics.DrawString(item.TimeSlotDisplay, font, brush, rect, sf);
                }

                e.Handled = true;
                return;
            }

            // 5. Cột Trạng thái: Badge màu sắc
            if (e.ColumnIndex == _grid.Columns["colStatus"].Index)
            {
                e.PaintBackground(e.ClipBounds, true);

                Color badgeBg;
                Color badgeBorder;
                Color badgeText;
                string statusText = item.Status;

                switch (statusText)
                {
                    case "Hoàn thành":
                        badgeBg = Color.FromArgb(22, 78, 99); // Dark Cyan/Blue
                        badgeBorder = Color.FromArgb(6, 182, 212);
                        badgeText = Color.FromArgb(165, 243, 252);
                        break;
                    case "Đã duyệt":
                        badgeBg = Color.FromArgb(20, 83, 45); // Dark Green
                        badgeBorder = Color.FromArgb(34, 197, 94);
                        badgeText = Color.FromArgb(187, 247, 208);
                        break;
                    case "Chờ duyệt":
                        badgeBg = Color.FromArgb(113, 63, 18); // Dark Yellow/Amber
                        badgeBorder = Color.FromArgb(234, 179, 8);
                        badgeText = Color.FromArgb(254, 240, 138);
                        break;
                    case "Đã hủy":
                    default:
                        badgeBg = Color.FromArgb(127, 29, 29); // Dark Red
                        badgeBorder = Color.FromArgb(239, 68, 68);
                        badgeText = Color.FromArgb(254, 202, 202);
                        break;
                }

                int w = Math.Min(95, e.CellBounds.Width - 8);
                int h = 26;
                int x = e.CellBounds.X + (e.CellBounds.Width - w) / 2;
                int y = e.CellBounds.Y + (e.CellBounds.Height - h) / 2;
                var rect = new Rectangle(x, y, w, h);

                using (var brush = new SolidBrush(badgeBg)) e.Graphics.FillRectangle(brush, rect);
                using (var pen = new Pen(badgeBorder, 1f)) e.Graphics.DrawRectangle(pen, rect);
                using (var brush = new SolidBrush(badgeText))
                using (var font = new Font("Segoe UI", 7.8F, FontStyle.Bold))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    e.Graphics.DrawString(statusText, font, brush, rect, sf);
                }

                e.Handled = true;
                return;
            }

            // 6. Cột Nội dung bài tập & Ghi chú
            if (e.ColumnIndex == _grid.Columns["colWorkout"].Index)
            {
                e.PaintBackground(e.ClipBounds, true);

                using (var brushWorkout = new SolidBrush(TextColor))
                using (var brushNote = new SolidBrush(Color.FromArgb(113, 113, 122)))
                using (var fontWorkout = new Font("Segoe UI", 8.8F, FontStyle.Bold))
                using (var fontNote = new Font("Segoe UI", 7.8F))
                {
                    int x = e.CellBounds.X + 8;
                    int y = e.CellBounds.Y + 12;

                    string workout = item.TrainingType;
                    if (workout.Length > 45) workout = workout.Substring(0, 42) + "...";
                    e.Graphics.DrawString(workout, fontWorkout, brushWorkout, x, y);

                    string note = string.IsNullOrWhiteSpace(item.Note) ? "Lịch tập 1-1 tiêu chuẩn" : item.Note;
                    if (note.Length > 50) note = note.Substring(0, 47) + "...";
                    e.Graphics.DrawString(note, fontNote, brushNote, x, y + 20);
                }

                e.Handled = true;
                return;
            }
        }

        // =====================================================================
        // CELL BUTTON CLICK ACTIONS
        // =====================================================================
        private void Grid_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _grid.Rows.Count) return;
            if (_grid.Rows[e.RowIndex].Tag is not ScheduleDto item) return;

            // 1. Nút Quick Action (Duyệt / Xong / Xem / Mở lại)
            if (e.ColumnIndex == _grid.Columns["colAction"].Index)
            {
                if (item.Status == "Chờ duyệt")
                {
                    var res = ScheduleService.Instance.UpdateStatus(item.ScheduleID, "Đã duyệt");
                    if (res.Success)
                    {
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show(res.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (item.Status == "Đã duyệt")
                {
                    var res = ScheduleService.Instance.UpdateStatus(item.ScheduleID, "Hoàn thành");
                    if (res.Success)
                    {
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show(res.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (item.Status == "Hoàn thành")
                {
                    MessageBox.Show($"Buổi tập #{item.ScheduleID} đã hoàn thành xuất sắc!\n\nHội viên: {item.MemberName}\nHLV: {item.PTName}\nThời gian: {item.TrainingDateDisplay} ({item.TimeSlotDisplay})\nBài tập: {item.TrainingType}\nGhi chú: {item.Note}",
                        "Chi Tiết Buổi Tập", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (item.Status == "Đã hủy")
                {
                    if (MessageBox.Show($"Mở lại lịch hẹn #{item.ScheduleID} này thành trạng thái 'Chờ duyệt'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        ScheduleService.Instance.UpdateStatus(item.ScheduleID, "Chờ duyệt");
                        LoadData();
                    }
                }
                return;
            }

            // 2. Nút Sửa
            if (e.ColumnIndex == _grid.Columns["colEdit"].Index)
            {
                using var frm = new frmScheduleBooking(item);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    var res = ScheduleService.Instance.UpdateSchedule(frm.ResultSchedule);
                    if (res.Success)
                    {
                        MessageBox.Show("Cập nhật lịch hẹn thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show(res.Message, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                return;
            }

            // 3. Nút Xóa
            if (e.ColumnIndex == _grid.Columns["colDel"].Index)
            {
                if (MessageBox.Show($"Bạn có chắc chắn muốn xóa lịch hẹn #{item.ScheduleID} của hội viên '{item.MemberName}' với HLV '{item.PTName}'?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    var res = ScheduleService.Instance.DeleteSchedule(item.ScheduleID);
                    if (res.Success)
                    {
                        MessageBox.Show("Đã xóa lịch hẹn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show(res.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // =====================================================================
        // THÊM MỚI LỊCH TẬP
        // =====================================================================
        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            using var frm = new frmScheduleBooking();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var res = ScheduleService.Instance.AddSchedule(frm.ResultSchedule);
                if (res.Success)
                {
                    MessageBox.Show(res.Message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(res.Message, "Không thể đặt lịch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
