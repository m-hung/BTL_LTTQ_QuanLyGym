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
    public class ucAdminMaintenance : UserControl
    {
        // =====================================================================
        // MÀU SẮC CHỦ ĐẠO (DARK GYM THEME)
        // =====================================================================
        private static readonly Color PageBg = Color.FromArgb(15, 15, 17);
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
        private List<EquipmentMaintenanceDto> _allMaintenances = new();
        private List<BranchLookupDto> _branches = new();

        // FILTER STATE
        private string _statusFilter = "Tất cả";
        private string _categoryFilter = "Tất cả danh mục";
        private int _branchFilterId = 0;
        private string _searchKeyword = string.Empty;

        // KPI LABELS
        private Label _lblKpiTotalCostVal = null!;
        private Label _lblKpiMonthCostVal = null!;
        private Label _lblKpiCompletedVal = null!;
        private Label _lblKpiProcessingVal = null!;
        private Label _lblKpiDueSoonVal = null!;

        // FILTER BUTTONS
        private Button _btnTabAll = null!;
        private Button _btnTabCompleted = null!;
        private Button _btnTabProcessing = null!;
        private Button _btnTabDueSoon = null!;

        private ComboBox _cboCategory = null!;
        private ComboBox _cboBranch = null!;
        private TextBox _txtSearch = null!;
        private DataGridView _grid = null!;
        private Label _lblFooterCount = null!;

        public ucAdminMaintenance()
        {
            BuildUI();
            ReloadFromDatabase();
        }

        public void ReloadFromDatabase()
        {
            try
            {
                _branches = EquipmentService.Instance.GetBranchLookups();
                _allMaintenances = MaintenanceService.Instance.GetMaintenances(null);

                PopulateBranchCombo();
                UpdateKpis();
                ApplyFilterAndRender();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ucAdminMaintenance.ReloadFromDatabase] Error: {ex.Message}");
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

            // DOCKING STACK ORDER
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
        // 1. HEADER BAR
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
                Text = "🔧 LỊCH SỬ BẢO TRÌ - BẢO DƯỠNG",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Margin = new Padding(0, 8, 12, 0)
            };
            var lblSub = new Label
            {
                Text = "|   Lên lịch định kỳ cho dàn máy chạy, giàn tạ, hệ thống điều hòa & hạch toán chi phí vận hành (OpEx)",
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
                Size = new Size(115, 36),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 1, 8, 0)
            };
            btnRefresh.FlatAppearance.BorderColor = BorderColor;
            btnRefresh.FlatAppearance.MouseOverBackColor = Color.FromArgb(39, 39, 42);
            btnRefresh.Click += (s, e) => ReloadFromDatabase();
            flpRight.Controls.Add(btnRefresh);

            var btnAdd = new Button
            {
                Text = "➕ Lên lịch bảo trì mới",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(185, 36),
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

            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));

            var cardTotalCost = CreateKpiCard("TỔNG CHI PHÍ BẢO TRÌ", "0 ₫", "Hạch toán chi phí vận hành (OpEx)", Orange, out _lblKpiTotalCostVal);
            var cardMonthCost = CreateKpiCard("CHI PHÍ THÁNG NÀY", "0 ₫", "Phát sinh trong tháng hiện tại", Cyan, out _lblKpiMonthCostVal);
            var cardCompleted = CreateKpiCard("ĐÃ HOÀN THÀNH", "0 lượt", "Đã nghiệm thu đưa vào phục vụ", Green, out _lblKpiCompletedVal);
            var cardProcessing = CreateKpiCard("ĐANG XỬ LÝ / CHỜ", "0 máy", "Đang sửa chữa hoặc lên lịch", Yellow, out _lblKpiProcessingVal);
            var cardDueSoon = CreateKpiCard("SẮP ĐẾN HẠN ĐỊNH KỲ", "0 máy", "Hạn bảo dưỡng trong 30 ngày tới", Red, out _lblKpiDueSoonVal);

            tbl.Controls.Add(cardTotalCost, 0, 0);
            tbl.Controls.Add(cardMonthCost, 1, 0);
            tbl.Controls.Add(cardCompleted, 2, 0);
            tbl.Controls.Add(cardProcessing, 3, 0);
            tbl.Controls.Add(cardDueSoon, 4, 0);

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
            _btnTabCompleted = CreateFilterTab("🟢 Hoàn thành", false, (s, e) => SwitchTab("Hoàn thành"));
            _btnTabProcessing = CreateFilterTab("🟠 Đang xử lý / Chờ", false, (s, e) => SwitchTab("Đang xử lý"));
            _btnTabDueSoon = CreateFilterTab("🔴 Sắp đến hạn", false, (s, e) => SwitchTab("Sắp đến hạn"));

            flpTabs.Controls.Add(_btnTabAll);
            flpTabs.Controls.Add(_btnTabCompleted);
            flpTabs.Controls.Add(_btnTabProcessing);
            flpTabs.Controls.Add(_btnTabDueSoon);
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
                Size = new Size(210, 34),
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
                PlaceholderText = "🔍 Tìm máy, nội dung..."
            };
            _txtSearch.TextChanged += (s, e) =>
            {
                _searchKeyword = _txtSearch.Text.Trim();
                ApplyFilterAndRender();
            };
            pnlSearch.Controls.Add(_txtSearch);
            flpRight.Controls.Add(pnlSearch);

            // Category Dropdown
            _cboCategory = new ComboBox
            {
                Size = new Size(185, 34),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 8.5F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 0, 8, 0)
            };
            _cboCategory.Items.AddRange(new object[]
            {
                "Tất cả danh mục",
                "Dàn máy chạy & Cardio",
                "Giàn tạ & Thể lực",
                "Hệ thống điều hòa"
            });
            _cboCategory.SelectedIndex = 0;
            _cboCategory.SelectedIndexChanged += (s, e) =>
            {
                _categoryFilter = _cboCategory.SelectedItem?.ToString() ?? "Tất cả danh mục";
                ApplyFilterAndRender();
            };
            flpRight.Controls.Add(_cboCategory);

            // Branch Dropdown
            _cboBranch = new ComboBox
            {
                Size = new Size(175, 34),
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 8.5F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 0, 0, 0)
            };
            _cboBranch.SelectedIndexChanged += (s, e) =>
            {
                if (_cboBranch.SelectedItem is BranchLookupDto b)
                    _branchFilterId = b.BranchID;
                else
                    _branchFilterId = 0;
                ApplyFilterAndRender();
            };
            flpRight.Controls.Add(_cboBranch);

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

            _btnTabCompleted.ForeColor = _statusFilter == "Hoàn thành" ? Green : MutedColor;
            _btnTabCompleted.BackColor = _statusFilter == "Hoàn thành" ? Color.FromArgb(39, 39, 42) : Color.Transparent;
            _btnTabCompleted.FlatAppearance.BorderColor = _statusFilter == "Hoàn thành" ? Green : BorderColor;

            _btnTabProcessing.ForeColor = _statusFilter == "Đang xử lý" ? Yellow : MutedColor;
            _btnTabProcessing.BackColor = _statusFilter == "Đang xử lý" ? Color.FromArgb(39, 39, 42) : Color.Transparent;
            _btnTabProcessing.FlatAppearance.BorderColor = _statusFilter == "Đang xử lý" ? Yellow : BorderColor;

            _btnTabDueSoon.ForeColor = _statusFilter == "Sắp đến hạn" ? Red : MutedColor;
            _btnTabDueSoon.BackColor = _statusFilter == "Sắp đến hạn" ? Color.FromArgb(39, 39, 42) : Color.Transparent;
            _btnTabDueSoon.FlatAppearance.BorderColor = _statusFilter == "Sắp đến hạn" ? Red : BorderColor;

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

            // Columns Definition
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCode",
                HeaderText = "MÃ MÁY",
                FillWeight = 50,
                MinimumWidth = 80,
                DefaultCellStyle = { ForeColor = Cyan, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "TÊN THIẾT BỊ",
                FillWeight = 135,
                MinimumWidth = 175,
                DefaultCellStyle = { ForeColor = Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCategory",
                HeaderText = "DANH MỤC",
                FillWeight = 85,
                MinimumWidth = 120,
                DefaultCellStyle = { ForeColor = Color.FromArgb(147, 197, 253) }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colLocation",
                HeaderText = "VỊ TRÍ ĐẶT MÁY",
                FillWeight = 110,
                MinimumWidth = 150,
                DefaultCellStyle = { ForeColor = MutedColor }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMaintenanceDate",
                HeaderText = "NGÀY BẢO TRÌ",
                FillWeight = 60,
                MinimumWidth = 95,
                DefaultCellStyle = { ForeColor = TextColor, Alignment = DataGridViewContentAlignment.MiddleCenter },
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNextDate",
                HeaderText = "HẠN ĐỊNH KỲ",
                FillWeight = 65,
                MinimumWidth = 100,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter },
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCost",
                HeaderText = "CHI PHÍ (OPEX)",
                FillWeight = 75,
                MinimumWidth = 110,
                DefaultCellStyle = { ForeColor = Orange, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Alignment = DataGridViewContentAlignment.MiddleRight },
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleRight } }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDescription",
                HeaderText = "NỘI DUNG BẢO TRÌ - BẢO DƯỠNG",
                FillWeight = 160,
                MinimumWidth = 200,
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

            // Custom Paint for Status Badge and Next Date Warning
            _grid.CellPainting += Grid_CellPainting;
            _grid.CellContentClick += Grid_CellContentClick;

            pnl.Controls.Add(_grid);
            return pnl;
        }

        private void Grid_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Highlight Next Maintenance Date if overdue or due soon
            if (e.ColumnIndex == _grid.Columns["colNextDate"].Index && _grid.Rows[e.RowIndex].Tag is EquipmentMaintenanceDto item)
            {
                e.PaintBackground(e.ClipBounds, true);

                Color textColor = TextColor;
                string dateText = item.DisplayNextMaintenanceDate;
                if (item.IsOverdue)
                {
                    textColor = Red;
                    dateText = $"{item.DisplayNextMaintenanceDate} (Quá hạn)";
                }
                else if (item.IsDueSoon)
                {
                    textColor = Yellow;
                    dateText = $"{item.DisplayNextMaintenanceDate} (Sắp tới)";
                }

                using var brush = new SolidBrush(textColor);
                using var font = new Font("Segoe UI", item.IsOverdue || item.IsDueSoon ? 8F : 8.5F, item.IsOverdue || item.IsDueSoon ? FontStyle.Bold : FontStyle.Regular);
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(dateText, font, brush, e.CellBounds, sf);
                e.Handled = true;
                return;
            }

            // Paint Status Badge
            if (e.ColumnIndex == _grid.Columns["colStatus"].Index && e.Value != null)
            {
                e.PaintBackground(e.ClipBounds, true);

                string status = e.Value.ToString() ?? "";
                Color badgeBg;
                Color badgeBorder;
                Color badgeText;
                string iconText = "";

                if (status == "Hoàn thành")
                {
                    badgeBg = Color.FromArgb(20, 83, 45); // Dark Green
                    badgeBorder = Color.FromArgb(34, 197, 94);
                    badgeText = Color.FromArgb(74, 222, 128);
                    iconText = "● Hoàn thành";
                }
                else if (status == "Đang xử lý")
                {
                    badgeBg = Color.FromArgb(124, 45, 18); // Dark Orange
                    badgeBorder = Color.FromArgb(249, 115, 22);
                    badgeText = Color.FromArgb(251, 146, 60);
                    iconText = "▲ Đang xử lý";
                }
                else
                {
                    badgeBg = Color.FromArgb(30, 58, 138); // Dark Blue
                    badgeBorder = Color.FromArgb(59, 130, 246);
                    badgeText = Color.FromArgb(147, 197, 253);
                    iconText = "⏰ Chờ bảo trì";
                }

                int w = Math.Min(110, e.CellBounds.Width - 10);
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
            if (row.Tag is not EquipmentMaintenanceDto item) return;

            // Sửa
            if (e.ColumnIndex == _grid.Columns["colEdit"].Index)
            {
                using var dlg = new frmMaintenanceEdit(item);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ReloadFromDatabase();
                }
            }
            // Xóa
            else if (e.ColumnIndex == _grid.Columns["colDel"].Index)
            {
                var confirm = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa phiếu bảo trì của máy \"{item.EquipmentName}\" ({item.EquipmentCode}) ngày {item.DisplayMaintenanceDate}?\n\nChi phí ({item.DisplayCost}) sẽ không còn được tính vào chi phí vận hành.",
                    "Xác nhận xóa phiếu bảo trì",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    var (success, msg) = MaintenanceService.Instance.DeleteMaintenance(item.MaintenanceID);
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
                Text = "Hiển thị 0 phiếu bảo trì",
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
            var summary = MaintenanceService.Instance.GetSummary(_branchFilterId > 0 ? _branchFilterId : null);

            _lblKpiTotalCostVal.Text = summary.TotalCost >= 1_000_000m
                ? $"{(summary.TotalCost / 1_000_000m):N2} triệu đ"
                : $"{summary.TotalCost:N0} đ";

            _lblKpiMonthCostVal.Text = summary.CurrentMonthCost >= 1_000_000m
                ? $"{(summary.CurrentMonthCost / 1_000_000m):N2} triệu đ"
                : $"{summary.CurrentMonthCost:N0} đ";

            _lblKpiCompletedVal.Text = $"{summary.CompletedCount} lượt";
            _lblKpiProcessingVal.Text = $"{summary.ProcessingCount + summary.ScheduledCount} máy";
            _lblKpiDueSoonVal.Text = $"{summary.DueSoonCount} máy";

            // Update Tab texts
            _btnTabAll.Text = $"Tất cả ({summary.TotalMaintenances})";
            _btnTabCompleted.Text = $"🟢 Hoàn thành ({summary.CompletedCount})";
            _btnTabProcessing.Text = $"🟠 Đang xử lý ({summary.ProcessingCount + summary.ScheduledCount})";
            _btnTabDueSoon.Text = $"🔴 Sắp đến hạn ({summary.DueSoonCount})";
        }

        private void ApplyFilterAndRender()
        {
            var filtered = _allMaintenances.AsEnumerable();

            // Status filter
            if (_statusFilter == "Hoàn thành")
            {
                filtered = filtered.Where(x => x.Status == "Hoàn thành");
            }
            else if (_statusFilter == "Đang xử lý")
            {
                filtered = filtered.Where(x => x.Status == "Đang xử lý" || x.Status == "Chờ bảo trì");
            }
            else if (_statusFilter == "Sắp đến hạn")
            {
                filtered = filtered.Where(x => x.IsDueSoon || x.IsOverdue);
            }

            // Category filter
            if (!string.IsNullOrWhiteSpace(_categoryFilter) && _categoryFilter != "Tất cả danh mục")
            {
                filtered = filtered.Where(x => x.EquipmentCategory == _categoryFilter);
            }

            // Branch filter
            if (_branchFilterId > 0)
            {
                filtered = filtered.Where(x => x.BranchID == _branchFilterId);
            }

            // Keyword filter
            if (!string.IsNullOrWhiteSpace(_searchKeyword))
            {
                string kw = _searchKeyword.ToLower();
                filtered = filtered.Where(x =>
                    x.EquipmentName.ToLower().Contains(kw) ||
                    x.EquipmentCode.ToLower().Contains(kw) ||
                    x.Description.ToLower().Contains(kw) ||
                    x.RoomName.ToLower().Contains(kw) ||
                    x.BranchName.ToLower().Contains(kw)
                );
            }

            var list = filtered.ToList();

            _grid.Rows.Clear();
            foreach (var item in list)
            {
                int rowIndex = _grid.Rows.Add(
                    item.EquipmentCode,
                    item.EquipmentName,
                    item.EquipmentCategory,
                    item.DisplayLocation,
                    item.DisplayMaintenanceDate,
                    item.DisplayNextMaintenanceDate,
                    item.DisplayCost,
                    item.Description,
                    item.Status
                );
                _grid.Rows[rowIndex].Tag = item;
            }

            decimal sumFiltered = list.Sum(x => x.Cost);
            string sumFormatted = sumFiltered >= 1_000_000m ? $"{(sumFiltered / 1_000_000m):N2} triệu VNĐ" : $"{sumFiltered:N0} VNĐ";
            _lblFooterCount.Text = $"Hiển thị {list.Count} / {_allMaintenances.Count} phiếu bảo trì | Chi phí vận hành tương ứng: {sumFormatted}";
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            using var dlg = new frmMaintenanceEdit(null);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ReloadFromDatabase();
            }
        }
    }
}
