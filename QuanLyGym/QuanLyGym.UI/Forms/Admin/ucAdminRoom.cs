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
    public class ucAdminRoom : UserControl
    {
        // Colors - Dark Gym Theme
        private static readonly Color BgColor = Color.FromArgb(15, 15, 17);
        private static readonly Color CardBg = Color.FromArgb(24, 24, 27);
        private static readonly Color ItemBg = Color.FromArgb(20, 20, 23);
        private static readonly Color SubItemBg = Color.FromArgb(18, 18, 20);
        private static readonly Color BorderColor = Color.FromArgb(45, 45, 50);
        private static readonly Color TextColor = Color.FromArgb(244, 244, 245);
        private static readonly Color MutedColor = Color.FromArgb(161, 161, 170);
        private static readonly Color Orange = Color.FromArgb(249, 115, 22);
        private static readonly Color Cyan = Color.FromArgb(6, 182, 212);
        private static readonly Color Blue = Color.FromArgb(56, 189, 248);
        private static readonly Color Green = Color.FromArgb(34, 197, 94);
        private static readonly Color Red = Color.FromArgb(239, 68, 68);

        // KPI Labels
        private Label _lblKpiTotal = null!;
        private Label _lblKpiActive = null!;
        private Label _lblKpiInactive = null!;
        private Label _lblKpiCapacity = null!;
        private Label _lblKpiEquipments = null!;

        // Toolbar
        private TextBox _txtSearch = null!;
        private ComboBox _cboStatus = null!;
        private Button _btnTabAll = null!;
        private Button _btnTabActive = null!;
        private Button _btnTabInactive = null!;
        private Label _lblRoomCount = null!;

        // Cards Container
        private FlowLayoutPanel _flpCards = null!;

        // Data State
        private List<RoomDto> _allRooms = new();
        private List<RoomDto> _filteredRooms = new();
        private string _statusFilter = "Tất cả";
        private string _searchKeyword = "";

        public ucAdminRoom()
        {
            Name = "ucAdminRoom";
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

            // Spacer
            var sp1 = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = Color.Transparent };
            pnlMain.Controls.Add(sp1);

            // 2. 5 KPI Cards
            var pnlKpis = BuildKpiSection();
            pnlMain.Controls.Add(pnlKpis);

            // Spacer
            var sp2 = new Panel { Dock = DockStyle.Top, Height = 14, BackColor = Color.Transparent };
            pnlMain.Controls.Add(sp2);

            // 3. Toolbar
            var pnlToolbar = BuildToolbar();
            pnlMain.Controls.Add(pnlToolbar);

            // Spacer
            var sp3 = new Panel { Dock = DockStyle.Top, Height = 12, BackColor = Color.Transparent };
            pnlMain.Controls.Add(sp3);

            // 4. Cards Section (FlowLayoutPanel responsive)
            var pnlCardsSection = BuildCardsSection();
            pnlMain.Controls.Add(pnlCardsSection);

            // Thứ tự hiển thị
            pnlHeader.BringToFront();
            sp1.BringToFront();
            pnlKpis.BringToFront();
            sp2.BringToFront();
            pnlToolbar.BringToFront();
            sp3.BringToFront();
            pnlCardsSection.BringToFront();
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
                Text = "🏢 QUẢN LÝ PHÒNG TẬP & STUDIO",
                Font = new Font("Segoe UI", 13.5F, FontStyle.Bold),
                ForeColor = TextColor,
                Dock = DockStyle.Top,
                Height = 32
            };

            var lblSubtitle = new Label
            {
                Text = "Quản lý không gian tập luyện, sức chứa, phân bổ thiết bị và các lớp học tại từng khu vực",
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
                Text = "➕ Thêm phòng tập",
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

            tbl.Controls.Add(CreateKpiCard("TỔNG SỐ PHÒNG", "0 phòng", "Toàn hệ thống", Orange, out _lblKpiTotal), 0, 0);
            tbl.Controls.Add(CreateKpiCard("ĐANG HOẠT ĐỘNG", "0 phòng", "Sẵn sàng phục vụ", Green, out _lblKpiActive), 1, 0);
            tbl.Controls.Add(CreateKpiCard("TẠM NGƯNG", "0 phòng", "Bảo trì / Sửa chữa", Red, out _lblKpiInactive), 2, 0);
            tbl.Controls.Add(CreateKpiCard("TỔNG SỨC CHỨA", "0 người", "Hội viên cùng lúc", Cyan, out _lblKpiCapacity), 3, 0);
            tbl.Controls.Add(CreateKpiCard("MÁY TẬP PHÂN BỔ", "0 máy", "Đã phân bổ thiết bị", Blue, out _lblKpiEquipments), 4, 0);

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
                Width = 380,
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
                PlaceholderText = "🔍 Tìm theo tên phòng, vị trí hoặc mô tả..."
            };
            _txtSearch.TextChanged += (s, e) =>
            {
                _searchKeyword = _txtSearch.Text.Trim();
                ApplyFilterAndRender();
            };
            pnlSearch.Controls.Add(_txtSearch);
            pnl.Controls.Add(pnlSearch);

            // Dropdown Status
            _cboStatus = new ComboBox
            {
                Dock = DockStyle.Left,
                Width = 200,
                Height = 34,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 8.5F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            _cboStatus.Items.AddRange(new object[] { "-- Tất cả trạng thái --", "Hoạt động", "Tạm ngưng" });
            _cboStatus.SelectedIndex = 0;
            _cboStatus.SelectedIndexChanged += (s, e) =>
            {
                if (_cboStatus.SelectedIndex == 1) SwitchTab("Hoạt động");
                else if (_cboStatus.SelectedIndex == 2) SwitchTab("Tạm ngưng");
                else SwitchTab("Tất cả");
            };

            var pnlCboWrapper = new Panel
            {
                Dock = DockStyle.Left,
                Width = 210,
                Padding = new Padding(10, 0, 0, 0),
                BackColor = Color.Transparent
            };
            pnlCboWrapper.Controls.Add(_cboStatus);
            pnl.Controls.Add(pnlCboWrapper);

            // Filter Tabs (Right)
            var flpTabs = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            _btnTabAll = CreateFilterTab("Tất cả", true, (s, e) => SwitchTab("Tất cả"));
            _btnTabActive = CreateFilterTab("✔ Hoạt động", false, (s, e) => SwitchTab("Hoạt động"));
            _btnTabInactive = CreateFilterTab("⏸ Tạm ngưng", false, (s, e) => SwitchTab("Tạm ngưng"));

            flpTabs.Controls.Add(_btnTabAll);
            flpTabs.Controls.Add(_btnTabActive);
            flpTabs.Controls.Add(_btnTabInactive);
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

        private void SwitchTab(string targetStatus)
        {
            _statusFilter = targetStatus;

            UpdateButtonState(_btnTabAll, _statusFilter == "Tất cả", Orange);
            UpdateButtonState(_btnTabActive, _statusFilter == "Hoạt động", Green);
            UpdateButtonState(_btnTabInactive, _statusFilter == "Tạm ngưng", Red);

            int targetCbo = _statusFilter == "Hoạt động" ? 1 : (_statusFilter == "Tạm ngưng" ? 2 : 0);
            if (_cboStatus.SelectedIndex != targetCbo)
            {
                _cboStatus.SelectedIndex = targetCbo;
            }

            ApplyFilterAndRender();
        }

        private void UpdateButtonState(Button btn, bool active, Color accent)
        {
            btn.ForeColor = active ? accent : MutedColor;
            btn.BackColor = active ? Color.FromArgb(39, 39, 42) : Color.Transparent;
            btn.FlatAppearance.BorderColor = active ? accent : BorderColor;
        }

        // =====================================================================
        // 4. CARDS SECTION (GRID OF ROOM CARDS)
        // =====================================================================
        private Panel BuildCardsSection()
        {
            var pnlContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoScroll = true
            };

            // Section Header
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                BackColor = Color.Transparent,
                Padding = new Padding(4, 4, 4, 8)
            };

            var lblTitle = new Label
            {
                Text = "📋 Danh Sách Không Gian Phòng Tập & Studio",
                Font = new Font("Segoe UI Emoji", 11F, FontStyle.Bold),
                ForeColor = TextColor,
                Dock = DockStyle.Left,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                UseMnemonic = false,
                UseCompatibleTextRendering = true
            };
            pnlHeader.Controls.Add(lblTitle);

            _lblRoomCount = new Label
            {
                Text = "Tổng cộng: 0 phòng",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Cyan,
                BackColor = Color.FromArgb(20, 30, 45),
                Dock = DockStyle.Right,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(10, 4, 10, 4),
                UseMnemonic = false
            };
            pnlHeader.Controls.Add(_lblRoomCount);

            // FlowLayoutPanel for cards
            _flpCards = new FlowLayoutPanel
            {
                Name = "flpCards",
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoScroll = true,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 10, 0, 20)
            };
            pnlContainer.Controls.Add(_flpCards);
            pnlContainer.Controls.Add(pnlHeader);
            pnlHeader.SendToBack();

            // Responsive resizing
            pnlContainer.Resize += (s, e) => ResizeCards();

            return pnlContainer;
        }

        private void ResizeCards()
        {
            int totalW = _flpCards.ClientSize.Width - 20;
            if (totalW <= 0) return;

            // Tính toán số cột (3 cột nếu đủ rộng >= 1300, 2 cột nếu >= 850, 1 cột nếu hẹp)
            int cols = 3;
            if (totalW < 900) cols = 1;
            else if (totalW < 1350) cols = 2;

            int cardW = (totalW - (cols * 12)) / cols;
            if (cardW < 360) cardW = 360;

            _flpCards.SuspendLayout();
            foreach (Control c in _flpCards.Controls)
            {
                if (c is Panel card)
                {
                    card.Width = cardW;
                }
            }
            _flpCards.ResumeLayout();
        }

        // =====================================================================
        // DATA LOADING & RENDERING
        // =====================================================================
        public void LoadData()
        {
            try
            {
                RoomService.Instance.EnsureInitialRooms();

                var summary = RoomService.Instance.GetSummary();
                _lblKpiTotal.Text = $"{summary.TotalRooms} phòng";
                _lblKpiActive.Text = $"{summary.ActiveRooms} phòng";
                _lblKpiInactive.Text = $"{summary.InactiveRooms} phòng";
                _lblKpiCapacity.Text = $"{summary.TotalCapacity} người";
                _lblKpiEquipments.Text = $"{summary.TotalEquipments} máy";

                _allRooms = RoomService.Instance.GetRooms();
                ApplyFilterAndRender();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi nạp dữ liệu phòng tập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilterAndRender()
        {
            var filter = new RoomFilterDto
            {
                Keyword = _searchKeyword,
                Status = _statusFilter
            };

            _filteredRooms = RoomService.Instance.GetRooms(filter);
            _lblRoomCount.Text = $"Tổng cộng: {_filteredRooms.Count} phòng";

            _flpCards.SuspendLayout();
            _flpCards.Controls.Clear();

            int totalW = _flpCards.ClientSize.Width - 20;
            int cols = 3;
            if (totalW < 900) cols = 1;
            else if (totalW < 1350) cols = 2;
            int cardW = Math.Max(360, (totalW - (cols * 12)) / cols);

            foreach (var room in _filteredRooms)
            {
                var card = CreateRoomCard(room, cardW);
                _flpCards.Controls.Add(card);
            }

            _flpCards.ResumeLayout();
        }

        // =====================================================================
        // CREATE A ROOM CARD (GIỐNG HỆT ẢNH MẪU CỦA USER)
        // =====================================================================
        private Panel CreateRoomCard(RoomDto room, int width)
        {
            var card = new Panel
            {
                Width = width,
                Height = 400,
                BackColor = CardBg,
                Margin = new Padding(6),
                Padding = new Padding(16, 14, 16, 14)
            };
            card.Paint += (s, e) =>
            {
                using var p = new Pen(BorderColor, 1.2f);
                e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1);
            };

            // 1. Header (Tên phòng & Badge trạng thái)
            var pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Color.Transparent
            };

            var lblName = new Label
            {
                Text = room.RoomName,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = TextColor,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft,
                UseMnemonic = false
            };

            var lblBadge = new Label
            {
                Text = room.Status ? "✔ Hoạt động" : "⏸ Tạm ngưng",
                Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
                ForeColor = room.Status ? Color.FromArgb(134, 239, 172) : Color.FromArgb(254, 202, 202),
                BackColor = room.Status ? Color.FromArgb(20, 83, 45) : Color.FromArgb(127, 29, 29),
                Dock = DockStyle.Right,
                AutoSize = true,
                Height = 24,
                Padding = new Padding(8, 3, 8, 3),
                TextAlign = ContentAlignment.MiddleCenter,
                UseMnemonic = false
            };

            pnlTop.Controls.Add(lblName);
            pnlTop.Controls.Add(lblBadge);
            card.Controls.Add(pnlTop);

            // 2. Vị trí phòng (icon 📍)
            var lblLocation = new Label
            {
                Text = $"📍 {room.Location}",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Orange,
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.MiddleLeft,
                UseMnemonic = false
            };
            card.Controls.Add(lblLocation);

            // 3. Mô tả phòng
            var lblDesc = new Label
            {
                Text = string.IsNullOrWhiteSpace(room.Description) ? "Không gian tập luyện tiêu chuẩn hiện đại." : room.Description,
                Font = new Font("Segoe UI", 8F),
                ForeColor = MutedColor,
                Dock = DockStyle.Top,
                Height = 38,
                TextAlign = ContentAlignment.TopLeft,
                UseMnemonic = false
            };
            card.Controls.Add(lblDesc);

            // 4. Khối 3 ô thông số: Sức chứa, Thiết bị, Lớp học
            var pnlStats = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 60,
                RowCount = 1,
                ColumnCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 4, 0, 4)
            };
            pnlStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            pnlStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            pnlStats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));

            pnlStats.Controls.Add(CreateStatBox("Sức chứa", room.CapacityText, TextColor), 0, 0);
            pnlStats.Controls.Add(CreateStatBox("Thiết bị", room.EquipmentText, Cyan), 1, 0);
            pnlStats.Controls.Add(CreateStatBox("Lớp học", room.ClassText, Orange), 2, 0);
            card.Controls.Add(pnlStats);

            // 5. Mục Lớp học tại phòng (Classes)
            int classesHeight = room.Classes.Count >= 2 ? 88 : (room.Classes.Count == 1 ? 58 : 46);
            var pnlClasses = new Panel
            {
                Dock = DockStyle.Top,
                Height = classesHeight,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 2, 0, 4)
            };

            var lblClassesTitle = new Label
            {
                Text = "📅 Lớp học tại phòng:",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = TextColor,
                Dock = DockStyle.Top,
                Height = 22,
                TextAlign = ContentAlignment.MiddleLeft,
                UseMnemonic = false
            };

            var flpClassPills = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                AutoScroll = false,
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 2, 0, 0)
            };

            if (room.Classes.Count == 0)
            {
                var lblEmptyCls = new Label
                {
                    Text = "Không có lớp nhóm cố định (dành riêng tập 1-1 / tự do)",
                    Font = new Font("Segoe UI", 7.5F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(113, 113, 122),
                    AutoSize = true,
                    Margin = new Padding(0, 3, 0, 0),
                    UseMnemonic = false
                };
                flpClassPills.Controls.Add(lblEmptyCls);
            }
            else
            {
                foreach (var cls in room.Classes)
                {
                    var lblPill = new Label
                    {
                        Text = cls,
                        Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(224, 242, 254),
                        BackColor = Color.FromArgb(30, 58, 138),
                        AutoSize = true,
                        Height = 24,
                        Padding = new Padding(8, 3, 8, 3),
                        Margin = new Padding(0, 2, 6, 4),
                        TextAlign = ContentAlignment.MiddleCenter,
                        UseMnemonic = false
                    };
                    lblPill.Paint += (ps, pe) =>
                    {
                        using var pen = new Pen(Color.FromArgb(96, 165, 250), 1.2f);
                        pe.Graphics.DrawRectangle(pen, 0, 0, lblPill.Width - 1, lblPill.Height - 1);
                    };
                    flpClassPills.Controls.Add(lblPill);
                }
            }

            pnlClasses.Controls.Add(flpClassPills);
            pnlClasses.Controls.Add(lblClassesTitle);
            lblClassesTitle.SendToBack();
            card.Controls.Add(pnlClasses);

            // 6. Hàng Đổi trạng thái phòng (Dropdown đổi nhanh phong cách Dark Gym)
            var pnlStatusChange = new Panel
            {
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 4, 0, 0)
            };

            var lblChangeStatus = new Label
            {
                Text = "⚙ Đổi trạng thái phòng:",
                Font = new Font("Segoe UI", 8F),
                ForeColor = MutedColor,
                Dock = DockStyle.Left,
                Width = 140,
                TextAlign = ContentAlignment.MiddleLeft,
                UseMnemonic = false
            };
            pnlStatusChange.Controls.Add(lblChangeStatus);

            var btnStatusToggle = new Button
            {
                Dock = DockStyle.Fill,
                Text = room.Status ? "🟢  Hoạt động  ▾" : "⏸  Tạm ngưng  ▾",
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = room.Status ? Color.FromArgb(74, 222, 128) : Color.FromArgb(248, 113, 113),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnStatusToggle.FlatAppearance.BorderColor = Color.FromArgb(51, 65, 85);
            btnStatusToggle.FlatAppearance.BorderSize = 1;

            var cmsStatus = new ContextMenuStrip
            {
                BackColor = Color.FromArgb(24, 24, 27),
                ForeColor = Color.White,
                ShowImageMargin = false,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            var itemActive = new ToolStripMenuItem("🟢  Hoạt động (Sẵn sàng phục vụ)") { ForeColor = Color.FromArgb(74, 222, 128) };
            var itemInactive = new ToolStripMenuItem("⏸  Tạm ngưng (Bảo trì / Sửa chữa)") { ForeColor = Color.FromArgb(248, 113, 113) };

            void SetRoomStatus(bool newStatus)
            {
                if (newStatus != room.Status)
                {
                    var res = RoomService.Instance.UpdateStatus(room.RoomID, newStatus);
                    if (res.Success)
                    {
                        room.Status = newStatus;
                        btnStatusToggle.Text = newStatus ? "🟢  Hoạt động  ▾" : "⏸  Tạm ngưng  ▾";
                        btnStatusToggle.ForeColor = newStatus ? Color.FromArgb(74, 222, 128) : Color.FromArgb(248, 113, 113);
                        lblBadge.Text = newStatus ? "✔ Hoạt động" : "⏸ Tạm ngưng";
                        lblBadge.ForeColor = newStatus ? Color.FromArgb(134, 239, 172) : Color.FromArgb(254, 202, 202);
                        lblBadge.BackColor = newStatus ? Color.FromArgb(20, 83, 45) : Color.FromArgb(127, 29, 29);

                        // Cập nhật lại các thẻ KPI
                        var summary = RoomService.Instance.GetSummary();
                        _lblKpiActive.Text = $"{summary.ActiveRooms} phòng";
                        _lblKpiInactive.Text = $"{summary.InactiveRooms} phòng";
                    }
                    else
                    {
                        MessageBox.Show(res.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

            itemActive.Click += (s, e) => SetRoomStatus(true);
            itemInactive.Click += (s, e) => SetRoomStatus(false);
            cmsStatus.Items.Add(itemActive);
            cmsStatus.Items.Add(itemInactive);

            btnStatusToggle.Click += (s, e) => cmsStatus.Show(btnStatusToggle, new Point(0, btnStatusToggle.Height));

            pnlStatusChange.Controls.Add(btnStatusToggle);
            card.Controls.Add(pnlStatusChange);

            // 7. Hàng Nút Hành động dưới cùng (Chi tiết, Sửa, Xóa)
            var pnlActions = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 36,
                RowCount = 1,
                ColumnCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            pnlActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            pnlActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            pnlActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));

            var btnDetail = CreateCardButton("👁 Chi tiết", Blue, (s, e) => ShowRoomDetail(room));
            var btnEdit = CreateCardButton("✏ Sửa", Cyan, (s, e) => EditRoom(room));
            var btnDelete = CreateCardButton("🗑", Red, (s, e) => DeleteRoom(room));

            pnlActions.Controls.Add(btnDetail, 0, 0);
            pnlActions.Controls.Add(btnEdit, 1, 0);
            pnlActions.Controls.Add(btnDelete, 2, 0);
            card.Controls.Add(pnlActions);

            // Thứ tự hiển thị docking chuẩn xác
            pnlTop.BringToFront();
            lblLocation.BringToFront();
            lblDesc.BringToFront();
            pnlStats.BringToFront();
            pnlClasses.BringToFront();
            pnlStatusChange.BringToFront();
            pnlActions.BringToFront();

            return card;
        }

        private Panel CreateStatBox(string label, string val, Color valColor)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ItemBg,
                Margin = new Padding(3),
                Padding = new Padding(4, 3, 4, 3)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(BorderColor, 1f);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            var lblSub = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 7.2F),
                ForeColor = MutedColor,
                Dock = DockStyle.Top,
                Height = 17,
                TextAlign = ContentAlignment.MiddleCenter,
                UseMnemonic = false
            };

            var lblVal = new Label
            {
                Text = val,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = valColor,
                Dock = DockStyle.Top,
                Height = 25,
                TextAlign = ContentAlignment.MiddleCenter,
                UseMnemonic = false
            };

            pnl.Controls.Add(lblVal);
            pnl.Controls.Add(lblSub);

            lblSub.SendToBack();
            lblVal.BringToFront();

            return pnl;
        }

        private Button CreateCardButton(string text, Color foreColor, EventHandler onClick)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = foreColor,
                BackColor = Color.FromArgb(32, 32, 36),
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Fill,
                Margin = new Padding(2),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = BorderColor;
            btn.FlatAppearance.BorderSize = 1;
            btn.Click += onClick;
            return btn;
        }

        // =====================================================================
        // CRUD ACTIONS
        // =====================================================================
        private void ShowRoomDetail(RoomDto room)
        {
            string classesStr = room.Classes.Count > 0 ? string.Join("\n  • ", room.Classes) : "Không có lớp nhóm (khu vực tập luyện tự do/1-1)";
            string info = $"THÔNG TIN CHI TIẾT PHÒNG TẬP\n\n" +
                          $"• Tên phòng: {room.RoomName}\n" +
                          $"• Vị trí: {room.Location}\n" +
                          $"• Cơ sở / Chi nhánh: {room.BranchName}\n" +
                          $"• Sức chứa: {room.CapacityText}\n" +
                          $"• Thiết bị đã phân bổ: {room.EquipmentText}\n" +
                          $"• Trạng thái: {room.StatusText}\n" +
                          $"• Mô tả: {room.Description}\n\n" +
                          $"• Các lớp học tại phòng:\n  • {classesStr}";

            MessageBox.Show(info, $"Chi Tiết - {room.RoomName}", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void EditRoom(RoomDto room)
        {
            using var frm = new frmRoomEdit(room);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var res = RoomService.Instance.UpdateRoom(frm.ResultRoom);
                if (res.Success)
                {
                    MessageBox.Show("Cập nhật phòng tập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(res.Message, "Lỗi cập nhật", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void DeleteRoom(RoomDto room)
        {
            if (MessageBox.Show($"Bạn có chắc chắn muốn xóa phòng tập '{room.RoomName}' ({room.Location})?",
                "Xác nhận xóa phòng tập", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                var res = RoomService.Instance.DeleteRoom(room.RoomID);
                if (res.Success)
                {
                    MessageBox.Show(res.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(res.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            using var frm = new frmRoomEdit();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var res = RoomService.Instance.AddRoom(frm.ResultRoom);
                if (res.Success)
                {
                    MessageBox.Show("Thêm mới phòng tập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show(res.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
