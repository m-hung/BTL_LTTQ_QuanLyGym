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
    public class ucAdminPackage : UserControl
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
        private static readonly Color Purple = Color.FromArgb(168, 85, 247);

        // =====================================================================
        // DATA REPOSITORY
        // =====================================================================
        private List<PackageDto> _allPackages = new();

        // FILTER STATE
        private string _serviceFilter = "Tất cả";
        private string _searchKeyword = string.Empty;

        // KPI LABELS
        private Label _lblKpiTotalPkgVal = null!;
        private Label _lblKpiTotalSubsVal = null!;
        private Label _lblKpiRevenueVal = null!;
        private Label _lblKpiPtCountVal = null!;
        private Label _lblKpiPopularVal = null!;

        // CARDS CONTAINER (Phần 1 - như ảnh 1)
        private TableLayoutPanel _tblCards = null!;

        // TOOLBAR & FILTER TABS
        private Button _btnTabAll = null!;
        private Button _btnTabSelf = null!;
        private Button _btnTabPT = null!;
        private TextBox _txtSearch = null!;

        // DATA GRID (Phần 2 - như ảnh 2)
        private DataGridView _grid = null!;
        private Label _lblGridCountBadge = null!;

        public ucAdminPackage()
        {
            BuildUI();
            ReloadFromDatabase();
        }

        public void ReloadFromDatabase()
        {
            try
            {
                _allPackages = PackageService.Instance.GetPackages(null);

                UpdateKpis();
                RenderCards();
                ApplyFilterAndRenderGrid();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ucAdminPackage.ReloadFromDatabase] Error: {ex.Message}");
            }
        }

        private void BuildUI()
        {
            BackColor = PageBg;
            Dock = DockStyle.Fill;
            Padding = new Padding(16, 12, 16, 14);
            AutoScroll = true;

            // 1. TOP HEADER BAR
            var pnlHeader = BuildHeaderBar();
            var pnlGap1 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // 2. KPI STRIP (5 Cards)
            var pnlKpis = BuildKpiRow();
            var pnlGap2 = new Panel { Dock = DockStyle.Top, Height = 12, BackColor = Color.Transparent };

            // 3. CARDS SECTION (Phần 1: 4 Thẻ gói tập như ảnh mẫu)
            var pnlCardsSection = BuildCardsSection();
            var pnlGap3 = new Panel { Dock = DockStyle.Top, Height = 14, BackColor = Color.Transparent };

            // 4. TOOLBAR TÌM KIẾM & BỘ LỌC
            var pnlToolbar = BuildToolbar();
            var pnlGap4 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };

            // 5. MAIN DATA GRID SECTION (Phần 2: Bảng tổng hợp chi tiết)
            var pnlGridSection = BuildGridSection();

            // Docking Stack Order:
            // pnlGridSection (Fill) -> pnlGap4 -> pnlToolbar -> pnlGap3 -> pnlCardsSection -> pnlGap2 -> pnlKpis -> pnlGap1 -> pnlHeader
            Controls.Add(pnlGridSection);
            Controls.Add(pnlGap4);
            Controls.Add(pnlToolbar);
            Controls.Add(pnlGap3);
            Controls.Add(pnlCardsSection);
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
                Text = "📦 QUẢN LÝ GÓI TẬP GYM",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Margin = new Padding(0, 8, 12, 0)
            };
            var lblSub = new Label
            {
                Text = "|   Quản lý các gói tập, mức giá niêm yết, thời hạn sử dụng và quyền lợi kèm PT",
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
                Text = "➕ Thêm gói tập mới",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Blue,
                Size = new Size(175, 36),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 1, 0, 0)
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            btnAdd.Click += (s, e) => OpenEditModal(null);
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
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));

            var cardTotal = CreateKpiCard("TỔNG GÓI TẬP", "0 gói", "Hệ thống toàn chuỗi", TextColor, out _lblKpiTotalPkgVal);
            var cardSubs = CreateKpiCard("LƯỢT ĐĂNG KÝ", "0 lượt", "Hội viên đang tham gia", Cyan, out _lblKpiTotalSubsVal);
            var cardRev = CreateKpiCard("DOANH THU GÓI TẬP", "0 ₫", "Tổng giá trị hợp đồng", Orange, out _lblKpiRevenueVal);
            var cardPt = CreateKpiCard("GÓI KÈM PT", "0 gói", "Có huấn luyện viên riêng", Yellow, out _lblKpiPtCountVal);
            var cardPop = CreateKpiCard("PHỔ BIẾN NHẤT", "-", "Được chọn nhiều nhất", Green, out _lblKpiPopularVal);

            tbl.Controls.Add(cardTotal, 0, 0);
            tbl.Controls.Add(cardSubs, 1, 0);
            tbl.Controls.Add(cardRev, 2, 0);
            tbl.Controls.Add(cardPt, 3, 0);
            tbl.Controls.Add(cardPop, 4, 0);

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
        // 3. CARDS SECTION (Phần 1 - như ảnh 1 của user)
        // =====================================================================
        private Panel BuildCardsSection()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 220,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };

            _tblCards = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 4,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            _tblCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            _tblCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            _tblCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            _tblCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));

            pnl.Controls.Add(_tblCards);
            return pnl;
        }

        private void RenderCards()
        {
            _tblCards.Controls.Clear();

            // Hiển thị tối đa 4 gói tập chính
            var displayList = _allPackages.Take(4).ToList();
            for (int i = 0; i < displayList.Count; i++)
            {
                var pkg = displayList[i];
                var card = CreatePackageCard(pkg);
                _tblCards.Controls.Add(card, i, 0);
            }
        }

        private Panel CreatePackageCard(PackageDto pkg)
        {
            var pnl = new Panel
            {
                BackColor = CardBg,
                Dock = DockStyle.Fill,
                Margin = new Padding(4),
                Padding = new Padding(14, 12, 14, 12)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(BorderColor, 1.2f);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            // Top Header: Tên gói & Badge PT/Tự tập
            var pnlCardHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.Transparent
            };

            var lblName = new Label
            {
                Text = pkg.DisplayTitle,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblBadge = new Label
            {
                Text = pkg.IsPTPackage ? "👤 Kèm PT" : "🏃 Tự tập",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = pkg.IsPTPackage ? Color.FromArgb(248, 113, 113) : Color.FromArgb(147, 197, 253),
                BackColor = pkg.IsPTPackage ? Color.FromArgb(127, 29, 29) : Color.FromArgb(30, 41, 59),
                Dock = DockStyle.Right,
                AutoSize = true,
                Padding = new Padding(6, 3, 6, 3),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlCardHeader.Controls.Add(lblBadge);
            pnlCardHeader.Controls.Add(lblName);
            pnl.Controls.Add(pnlCardHeader);

            // Giá lớn
            var lblPrice = new Label
            {
                Text = $"{pkg.Price:N0} VNĐ",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Cyan,
                Dock = DockStyle.Top,
                Height = 32,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnl.Controls.Add(lblPrice);

            // Thời hạn
            var lblDuration = new Label
            {
                Text = $"🕒 Thời hạn: {pkg.Duration} ngày",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Yellow,
                Dock = DockStyle.Top,
                Height = 20,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnl.Controls.Add(lblDuration);

            // Mô tả tóm tắt
            var lblDesc = new Label
            {
                Text = !string.IsNullOrWhiteSpace(pkg.Description) ? pkg.Description : "Gói tập gym chất lượng cao toàn hệ thống.",
                Font = new Font("Segoe UI", 8F),
                ForeColor = MutedColor,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 4, 0, 0)
            };
            pnl.Controls.Add(lblDesc);

            // Bottom Actions & Lượt đăng ký
            var pnlCardBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 44,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 6, 0, 0)
            };

            var lblSubs = new Label
            {
                Text = $"Lượt đăng ký: {pkg.SubscriberCount} lượt",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.FromArgb(212, 212, 216),
                Dock = DockStyle.Top,
                Height = 16
            };
            pnlCardBottom.Controls.Add(lblSubs);

            var flpBtns = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 26,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };

            var btnEdit = new Button
            {
                Text = "✏ Sửa",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Blue,
                BackColor = ItemBg,
                Size = new Size(70, 26),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 6, 0)
            };
            btnEdit.FlatAppearance.BorderColor = Color.FromArgb(59, 130, 246);
            btnEdit.Click += (s, e) => OpenEditModal(pkg);
            flpBtns.Controls.Add(btnEdit);

            var btnDel = new Button
            {
                Text = "🗑 Xóa",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Red,
                BackColor = ItemBg,
                Size = new Size(70, 26),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0)
            };
            btnDel.FlatAppearance.BorderColor = Color.FromArgb(239, 68, 68);
            btnDel.Click += (s, e) => ConfirmDelete(pkg);
            flpBtns.Controls.Add(btnDel);

            pnlCardBottom.Controls.Add(flpBtns);
            pnl.Controls.Add(pnlCardBottom);

            // Đảm bảo trật tự Dock bên trong Card
            lblPrice.BringToFront();
            lblDuration.BringToFront();
            lblDesc.BringToFront();
            pnlCardBottom.BringToFront();

            return pnl;
        }

        // =====================================================================
        // 4. TOOLBAR TÌM KIẾM & BỘ LỌC
        // =====================================================================
        private Panel BuildToolbar()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Top,
                Height = 48,
                BackColor = CardBg,
                Padding = new Padding(10, 6, 12, 6)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(BorderColor);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            // Search Box (Bên trái)
            var pnlSearch = new Panel
            {
                Dock = DockStyle.Left,
                Width = 420,
                Height = 34,
                BackColor = ItemBg,
                Padding = new Padding(10, 7, 8, 4)
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
                PlaceholderText = "🔍 Tìm kiếm theo tên gói tập hoặc mô tả..."
            };
            _txtSearch.TextChanged += (s, e) =>
            {
                _searchKeyword = _txtSearch.Text.Trim();
                ApplyFilterAndRenderGrid();
            };
            pnlSearch.Controls.Add(_txtSearch);
            pnl.Controls.Add(pnlSearch);

            // Filter Tabs (Bên phải)
            var flpTabs = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            _btnTabAll = CreateFilterTab("Tất cả", true, (s, e) => SwitchTab("Tất cả"));
            _btnTabSelf = CreateFilterTab("🏃 Tự tập", false, (s, e) => SwitchTab("Tự tập"));
            _btnTabPT = CreateFilterTab("🏋 Kèm PT", false, (s, e) => SwitchTab("Kèm PT"));

            flpTabs.Controls.Add(_btnTabAll);
            flpTabs.Controls.Add(_btnTabSelf);
            flpTabs.Controls.Add(_btnTabPT);
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
                Margin = new Padding(0, 0, 6, 0),
                Padding = new Padding(12, 0, 12, 0)
            };
            btn.FlatAppearance.BorderColor = active ? Orange : BorderColor;
            btn.FlatAppearance.BorderSize = 1;
            btn.Click += onClick;
            return btn;
        }

        private void SwitchTab(string targetService)
        {
            _serviceFilter = targetService;

            _btnTabAll.ForeColor = _serviceFilter == "Tất cả" ? Orange : MutedColor;
            _btnTabAll.BackColor = _serviceFilter == "Tất cả" ? Color.FromArgb(39, 39, 42) : Color.Transparent;
            _btnTabAll.FlatAppearance.BorderColor = _serviceFilter == "Tất cả" ? Orange : BorderColor;

            _btnTabSelf.ForeColor = _serviceFilter == "Tự tập" ? Cyan : MutedColor;
            _btnTabSelf.BackColor = _serviceFilter == "Tự tập" ? Color.FromArgb(39, 39, 42) : Color.Transparent;
            _btnTabSelf.FlatAppearance.BorderColor = _serviceFilter == "Tự tập" ? Cyan : BorderColor;

            _btnTabPT.ForeColor = _serviceFilter == "Kèm PT" ? Red : MutedColor;
            _btnTabPT.BackColor = _serviceFilter == "Kèm PT" ? Color.FromArgb(39, 39, 42) : Color.Transparent;
            _btnTabPT.FlatAppearance.BorderColor = _serviceFilter == "Kèm PT" ? Red : BorderColor;

            ApplyFilterAndRenderGrid();
        }

        // =====================================================================
        // 5. MAIN DATA GRID SECTION (Phần 2 - Bảng tổng hợp chi tiết như ảnh 2)
        // =====================================================================
        private Panel BuildGridSection()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CardBg,
                Padding = new Padding(14)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(BorderColor, 1f);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            // Section Header
            var pnlSectionHead = new Panel
            {
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 0, 0, 8)
            };

            var lblGridTitle = new Label
            {
                Text = "📋 Bảng Tổng Hợp Chi Tiết Gói Tập",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };

            _lblGridCountBadge = new Label
            {
                Text = "Tổng cộng: 0 gói",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Blue,
                Dock = DockStyle.Right,
                AutoSize = true,
                Padding = new Padding(8, 4, 8, 4),
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnlSectionHead.Controls.Add(_lblGridCountBadge);
            pnlSectionHead.Controls.Add(lblGridTitle);
            pnl.Controls.Add(pnlSectionHead);

            // DataGridView
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

            // Columns Definition (y hệt bảng tổng hợp ảnh 2 của user)
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCode",
                HeaderText = "MÃ GÓI",
                FillWeight = 55,
                MinimumWidth = 85,
                DefaultCellStyle = { ForeColor = Cyan, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "TÊN GÓI",
                FillWeight = 160,
                MinimumWidth = 190,
                DefaultCellStyle = { ForeColor = Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDuration",
                HeaderText = "THỜI HẠN",
                FillWeight = 65,
                MinimumWidth = 95,
                DefaultCellStyle = { ForeColor = TextColor, Alignment = DataGridViewContentAlignment.MiddleCenter },
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPrice",
                HeaderText = "GIÁ NIÊM YẾT",
                FillWeight = 85,
                MinimumWidth = 120,
                DefaultCellStyle = { ForeColor = Cyan, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Alignment = DataGridViewContentAlignment.MiddleRight },
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleRight } }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPT",
                HeaderText = "DỊCH VỤ KÈM PT",
                FillWeight = 90,
                MinimumWidth = 120,
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } }
            });

            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSubs",
                HeaderText = "SỐ KHÁCH ĐĂNG KÝ",
                FillWeight = 85,
                MinimumWidth = 115,
                HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } }
            });

            // Action Buttons
            var colEdit = new DataGridViewButtonColumn
            {
                Name = "colEdit",
                HeaderText = "HÀNH ĐỘNG",
                FillWeight = 40,
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

            // Custom Paint for PT badge and Subscriber badge
            _grid.CellPainting += Grid_CellPainting;
            _grid.CellContentClick += Grid_CellContentClick;

            pnl.Controls.Add(_grid);
            _grid.BringToFront();

            return pnl;
        }

        private void Grid_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // PT Service Badge
            if (e.ColumnIndex == _grid.Columns["colPT"].Index && _grid.Rows[e.RowIndex].Tag is PackageDto item)
            {
                e.PaintBackground(e.ClipBounds, true);

                Color badgeBg = item.IsPTPackage ? Color.FromArgb(127, 29, 29) : Color.FromArgb(30, 41, 59);
                Color badgeBorder = item.IsPTPackage ? Color.FromArgb(239, 68, 68) : Color.FromArgb(71, 85, 105);
                Color badgeText = item.IsPTPackage ? Color.FromArgb(248, 113, 113) : Color.FromArgb(148, 163, 184);
                string text = item.IsPTPackage ? "✔ Có HLV riêng" : "Không có PT";

                int w = Math.Min(110, e.CellBounds.Width - 12);
                int h = 26;
                int x = e.CellBounds.X + (e.CellBounds.Width - w) / 2;
                int y = e.CellBounds.Y + (e.CellBounds.Height - h) / 2;
                var rect = new Rectangle(x, y, w, h);

                using (var brush = new SolidBrush(badgeBg)) e.Graphics.FillRectangle(brush, rect);
                using (var pen = new Pen(badgeBorder, 1f)) e.Graphics.DrawRectangle(pen, rect);
                using (var brush = new SolidBrush(badgeText))
                using (var font = new Font("Segoe UI", 8F, FontStyle.Bold))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    e.Graphics.DrawString(text, font, brush, rect, sf);
                }
                e.Handled = true;
                return;
            }

            // Subscriber Count Badge
            if (e.ColumnIndex == _grid.Columns["colSubs"].Index && _grid.Rows[e.RowIndex].Tag is PackageDto pkg)
            {
                e.PaintBackground(e.ClipBounds, true);

                Color badgeBg = Color.FromArgb(30, 58, 138); // Dark blue
                Color badgeBorder = Color.FromArgb(59, 130, 246);
                Color badgeText = Color.FromArgb(191, 219, 254);
                string text = $"{pkg.SubscriberCount} hội viên";

                int w = Math.Min(95, e.CellBounds.Width - 12);
                int h = 26;
                int x = e.CellBounds.X + (e.CellBounds.Width - w) / 2;
                int y = e.CellBounds.Y + (e.CellBounds.Height - h) / 2;
                var rect = new Rectangle(x, y, w, h);

                using (var brush = new SolidBrush(badgeBg)) e.Graphics.FillRectangle(brush, rect);
                using (var pen = new Pen(badgeBorder, 1f)) e.Graphics.DrawRectangle(pen, rect);
                using (var brush = new SolidBrush(badgeText))
                using (var font = new Font("Segoe UI", 8F, FontStyle.Bold))
                {
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    e.Graphics.DrawString(text, font, brush, rect, sf);
                }
                e.Handled = true;
                return;
            }
        }

        private void Grid_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = _grid.Rows[e.RowIndex];
            if (row.Tag is not PackageDto item) return;

            // Sửa
            if (e.ColumnIndex == _grid.Columns["colEdit"].Index)
            {
                OpenEditModal(item);
            }
            // Xóa
            else if (e.ColumnIndex == _grid.Columns["colDel"].Index)
            {
                ConfirmDelete(item);
            }
        }

        // =====================================================================
        // BUSINESS & RENDERING LOGIC
        // =====================================================================
        private void UpdateKpis()
        {
            var summary = PackageService.Instance.GetSummary();

            _lblKpiTotalPkgVal.Text = $"{summary.TotalPackages} gói";
            _lblKpiTotalSubsVal.Text = $"{summary.TotalSubscribers} lượt";
            _lblKpiRevenueVal.Text = summary.TotalRevenue >= 1_000_000m
                ? $"{(summary.TotalRevenue / 1_000_000m):N1} triệu đ"
                : $"{summary.TotalRevenue:N0} đ";

            _lblKpiPtCountVal.Text = $"{summary.PTPackagesCount} gói";
            _lblKpiPopularVal.Text = summary.MostPopularPackage;

            _lblGridCountBadge.Text = $"Tổng cộng: {_allPackages.Count} gói";

            int selfCount = _allPackages.Count(x => !x.IsPTPackage);
            int ptCount = _allPackages.Count(x => x.IsPTPackage);

            _btnTabAll.Text = $"Tất cả ({_allPackages.Count})";
            _btnTabSelf.Text = $"🏃 Tự tập ({selfCount})";
            _btnTabPT.Text = $"🏋 Kèm PT ({ptCount})";
        }

        private void ApplyFilterAndRenderGrid()
        {
            var filtered = _allPackages.AsEnumerable();

            // Service type filter
            if (_serviceFilter == "Tự tập")
            {
                filtered = filtered.Where(x => !x.IsPTPackage);
            }
            else if (_serviceFilter == "Kèm PT")
            {
                filtered = filtered.Where(x => x.IsPTPackage);
            }

            // Keyword filter
            if (!string.IsNullOrWhiteSpace(_searchKeyword))
            {
                string kw = _searchKeyword.ToLower();
                filtered = filtered.Where(x =>
                    x.PackageName.ToLower().Contains(kw) ||
                    x.DisplayTitle.ToLower().Contains(kw) ||
                    x.Description.ToLower().Contains(kw) ||
                    x.PackageCode.ToLower().Contains(kw)
                );
            }

            var list = filtered.ToList();

            _grid.Rows.Clear();
            foreach (var item in list)
            {
                int rowIndex = _grid.Rows.Add(
                    item.PackageCode,
                    item.DisplayTitle,
                    item.DisplayDuration,
                    item.DisplayPrice,
                    item.DisplayPTService,
                    item.DisplaySubscriberCount
                );
                _grid.Rows[rowIndex].Tag = item;
            }
        }

        private void OpenEditModal(PackageDto? item)
        {
            using var dlg = new frmPackageEdit(item);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ReloadFromDatabase();
            }
        }

        private void ConfirmDelete(PackageDto item)
        {
            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa gói tập \"{item.DisplayTitle}\" ({item.PackageCode})?\n\nNếu đang có hội viên sử dụng gói này, hệ thống sẽ tự động chuyển sang trạng thái [Tạm dừng mở bán] để bảo vệ hợp đồng hội viên.",
                "Xác nhận xóa gói tập",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                var (success, msg) = PackageService.Instance.DeletePackage(item.PackageID);
                if (success)
                {
                    MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ReloadFromDatabase();
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
