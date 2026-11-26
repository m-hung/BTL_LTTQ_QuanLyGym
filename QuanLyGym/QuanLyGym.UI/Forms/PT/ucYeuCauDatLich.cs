using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using QuanLyGym.BLL.DTOs.PT;
using QuanLyGym.BLL.Services.PT;

namespace QuanLyGym.UI.Forms.PT
{
    public partial class ucYeuCauDatLich : UserControl
    {
        private readonly PTBookingApprovalService _bookingService = new PTBookingApprovalService();
        private readonly int _ptId;
        private string _currentFilter = "all"; // all, upcoming, overdue
        private int? _expandedRejectScheduleId;

        private string _headerTitleText = "0 YÊU CẦU ĐANG CHỜ BẠN";
        private readonly string _headerSubText = "Duyệt để xác nhận ca tập với học viên, hoặc từ chối kèm lý do để học viên chọn giờ khác.";
        private string _lateFlagText = "";
        private bool _searchFocused;

        private PTUITheme.DoubleBufferedPanel pnlToast = null!;
        private Label lblToastMsg = null!;
        private bool _toastIsSuccess = true;

        private Panel pnlToolbar = null!;
        private Button btnFilterAll = null!;
        private Button btnFilterUpcoming = null!;
        private Button btnFilterOverdue = null!;
        private PTUITheme.DoubleBufferedPanel pnlSearchBox = null!;
        private TextBox txtSearch = null!;
        private Button btnSearch = null!;
        private Button btnViewSchedule = null!;

        // Khung panel thống nhất (giống .cx-panel bên WebGym: header + danh sách ngăn cách 1px)
        private PTUITheme.DoubleBufferedPanel pnlMainPanel = null!;
        private PTUITheme.DoubleBufferedPanel pnlHeaderCard = null!;
        private FlowLayoutPanel pnlRequestsList = null!;

        public event Action? OnViewScheduleRequested;

        public ucYeuCauDatLich() : this(1)
        {
        }

        public ucYeuCauDatLich(int ptId)
        {
            _ptId = ptId > 0 ? ptId : 1;
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = PTUITheme.BgDark;

            BuildUI();
            RefreshView();

            PTPortalSync.DataChanged += OnSharedDataChanged;
            this.Disposed += (s, e) => PTPortalSync.DataChanged -= OnSharedDataChanged;
            this.Load += (s, e) => { RelayoutToolbar(); RelayoutRowsWidth(); pnlHeaderCard.Invalidate(); };
            pnlRequestsList.Resize += (s, e) => RelayoutRowsWidth();
            pnlToolbar.Resize += (s, e) => RelayoutToolbar();
            pnlHeaderCard.Resize += (s, e) => pnlHeaderCard.Invalidate();
        }

        private void OnSharedDataChanged()
        {
            if (IsDisposed) return;
            if (InvokeRequired)
            {
                BeginInvoke(new Action(RefreshView));
                return;
            }
            RefreshView();
        }

        private void BuildUI()
        {
            // 1. Khung panel lớn thống nhất (.cx-panel)
            pnlMainPanel = new PTUITheme.DoubleBufferedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(18, 20, 26),
                Padding = new Padding(1)
            };
            pnlMainPanel.Paint += (s, e) =>
            {
                Rectangle r = pnlMainPanel.ClientRectangle;
                using var borderPen = new Pen(Color.FromArgb(37, 41, 52), 1f);
                e.Graphics.DrawRectangle(borderPen, 0, 0, r.Width - 1, r.Height - 1);
            };

            // 1a. Header của panel (.cx-panel-head) — tự vẽ trực tiếp bằng TextRenderer để không bị cắt chữ theo DPI
            pnlHeaderCard = new PTUITheme.DoubleBufferedPanel
            {
                Dock = DockStyle.Top,
                Height = 82,
                BackColor = Color.FromArgb(18, 20, 26)
            };
            pnlHeaderCard.Paint += PnlHeaderCard_Paint;

            // 1b. Danh sách yêu cầu xếp liền mạch, phân tách bởi kẻ ngang 1px
            pnlRequestsList = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.FromArgb(18, 20, 26),
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            EnableDoubleBuffer(pnlRequestsList);

            pnlMainPanel.Controls.Add(pnlRequestsList);
            pnlMainPanel.Controls.Add(pnlHeaderCard);

            // 2. Thanh công cụ lọc + tìm kiếm + nút XEM LỊCH DẠY (góc vuông sắc nét)
            pnlToolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = Color.Transparent
            };

            btnFilterAll = CreateFilterChip("Tất cả", "all");
            btnFilterUpcoming = CreateFilterChip("Sắp tới", "upcoming");
            btnFilterOverdue = CreateFilterChip("Quá giờ", "overdue");

            // Khung bọc TextBox để cao đúng 34px bằng hai nút TÌM và XEM LỊCH DẠY
            pnlSearchBox = new PTUITheme.DoubleBufferedPanel
            {
                Size = new Size(250, 34),
                BackColor = Color.FromArgb(14, 16, 20)
            };
            pnlSearchBox.Paint += (s, e) =>
            {
                Rectangle r = pnlSearchBox.ClientRectangle;
                Color border = _searchFocused ? PTUITheme.PrimaryLime : Color.FromArgb(52, 57, 68);
                using var p = new Pen(border, 1f);
                e.Graphics.DrawRectangle(p, 0, 0, r.Width - 1, r.Height - 1);
            };

            txtSearch = new TextBox
            {
                Location = new Point(10, 8),
                Width = 230,
                Height = 20,
                Font = PTUITheme.FontRegular,
                BackColor = Color.FromArgb(14, 16, 20),
                ForeColor = PTUITheme.TextWhite,
                BorderStyle = BorderStyle.None,
                PlaceholderText = "Tìm tên, SĐT, mã HV hoặc ghi chú..."
            };
            txtSearch.GotFocus += (s, e) => { _searchFocused = true; pnlSearchBox.Invalidate(); };
            txtSearch.LostFocus += (s, e) => { _searchFocused = false; pnlSearchBox.Invalidate(); };
            txtSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    RefreshView();
                }
            };
            pnlSearchBox.Resize += (s, e) =>
            {
                txtSearch.Width = Math.Max(80, pnlSearchBox.ClientSize.Width - 20);
                txtSearch.Location = new Point(10, (pnlSearchBox.ClientSize.Height - txtSearch.Height) / 2);
            };
            pnlSearchBox.Controls.Add(txtSearch);

            btnSearch = new Button
            {
                Text = "TÌM",
                Size = new Size(64, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(22, 25, 34),
                ForeColor = PTUITheme.PrimaryLime,
                Font = PTUITheme.FontSmallBold,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnSearch.FlatAppearance.BorderSize = 1;
            btnSearch.FlatAppearance.BorderColor = Color.FromArgb(52, 57, 68);
            btnSearch.MouseEnter += (s, e) => btnSearch.FlatAppearance.BorderColor = PTUITheme.PrimaryLime;
            btnSearch.MouseLeave += (s, e) => btnSearch.FlatAppearance.BorderColor = Color.FromArgb(52, 57, 68);
            btnSearch.Click += (s, e) => RefreshView();

            btnViewSchedule = new Button
            {
                Text = "XEM LỊCH DẠY",
                Size = new Size(134, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(241, 243, 238),
                Font = PTUITheme.FontSmallBold,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnViewSchedule.FlatAppearance.BorderSize = 1;
            btnViewSchedule.FlatAppearance.BorderColor = Color.FromArgb(52, 57, 68);
            btnViewSchedule.MouseEnter += (s, e) =>
            {
                btnViewSchedule.FlatAppearance.BorderColor = PTUITheme.PrimaryLime;
                btnViewSchedule.ForeColor = PTUITheme.PrimaryLime;
            };
            btnViewSchedule.MouseLeave += (s, e) =>
            {
                btnViewSchedule.FlatAppearance.BorderColor = Color.FromArgb(52, 57, 68);
                btnViewSchedule.ForeColor = Color.FromArgb(241, 243, 238);
            };
            btnViewSchedule.Click += (s, e) => OnViewScheduleRequested?.Invoke();

            pnlToolbar.Controls.AddRange(new Control[]
            {
                btnFilterAll, btnFilterUpcoming, btnFilterOverdue, pnlSearchBox, btnSearch, btnViewSchedule
            });

            // 3. Toast Banner (.cx-toast góc vuông với viền trái 3px)
            pnlToast = new PTUITheme.DoubleBufferedPanel
            {
                Dock = DockStyle.Top,
                Height = 46,
                Visible = false
            };
            pnlToast.Paint += (s, e) =>
            {
                Rectangle r = pnlToast.ClientRectangle;
                Rectangle box = new Rectangle(0, 0, r.Width - 1, r.Height - 9);
                Color bg = _toastIsSuccess ? Color.FromArgb(20, 29, 16) : Color.FromArgb(34, 20, 24);
                Color border = _toastIsSuccess ? Color.FromArgb(95, 200, 255, 46) : Color.FromArgb(110, 255, 107, 107);
                Color accent = _toastIsSuccess ? PTUITheme.PrimaryLime : Color.FromArgb(255, 107, 107);

                using (var bBg = new SolidBrush(bg)) e.Graphics.FillRectangle(bBg, box);
                using (var pBd = new Pen(border, 1f)) e.Graphics.DrawRectangle(pBd, box);
                using (var bAcc = new SolidBrush(accent)) e.Graphics.FillRectangle(bAcc, 0, 0, 3, box.Height + 1);
            };

            lblToastMsg = new Label
            {
                Location = new Point(14, 8),
                Width = 980,
                Height = 22,
                Font = PTUITheme.FontRegular,
                ForeColor = PTUITheme.PrimaryLime,
                AutoEllipsis = true
            };

            Button btnCloseToast = new Button
            {
                Text = "✕",
                FlatStyle = FlatStyle.Flat,
                ForeColor = PTUITheme.TextMuted,
                BackColor = Color.Transparent,
                Font = PTUITheme.FontSmallBold,
                Size = new Size(28, 28),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            btnCloseToast.FlatAppearance.BorderSize = 0;
            btnCloseToast.Location = new Point(pnlToast.Width - 36, 4);
            btnCloseToast.Click += (s, e) => { pnlToast.Visible = false; };
            pnlToast.Resize += (s, e) =>
            {
                btnCloseToast.Location = new Point(pnlToast.ClientSize.Width - 36, 4);
                lblToastMsg.Width = Math.Max(200, pnlToast.ClientSize.Width - 60);
            };

            pnlToast.Controls.Add(lblToastMsg);
            pnlToast.Controls.Add(btnCloseToast);

            // Thứ tự Dock: Fill trước, các Dock.Top thêm ngược từ dưới lên trên
            pnlRoot.Controls.Add(pnlMainPanel);
            pnlRoot.Controls.Add(pnlToolbar);
            pnlRoot.Controls.Add(pnlToast);
        }

        private void PnlHeaderCard_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r = pnlHeaderCard.ClientRectangle;

            using (var linePen = new Pen(Color.FromArgb(37, 41, 52), 1f))
            {
                g.DrawLine(linePen, 0, r.Height - 1, r.Width, r.Height - 1);
            }

            int rightReserve = 0;
            if (!string.IsNullOrEmpty(_lateFlagText))
            {
                Size flagSize = TextRenderer.MeasureText(g, _lateFlagText, PTUITheme.FontSmallBold,
                    Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                int flagW = flagSize.Width + 8;
                rightReserve = flagW + 24;

                Rectangle flagRect = new Rectangle(r.Width - flagW - 24, 0, flagW, r.Height - 2);
                TextRenderer.DrawText(g, _lateFlagText, PTUITheme.FontSmallBold, flagRect,
                    Color.FromArgb(242, 177, 52),
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }

            int leftW = Math.Max(220, r.Width - 48 - rightReserve);
            using var titleFont = new Font("Segoe UI", 15f, FontStyle.Bold);
            TextRenderer.DrawText(g, _headerTitleText, titleFont,
                new Rectangle(24, 14, leftW, 30),
                Color.FromArgb(241, 243, 238),
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            TextRenderer.DrawText(g, _headerSubText, PTUITheme.FontSub,
                new Rectangle(24, 46, leftW, 22),
                PTUITheme.TextMuted,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }

        public void RefreshView()
        {
            var allPending = _bookingService.GetPendingRequests(_ptId, "all");
            int totalPending = allPending.Count;
            int lateCount = allPending.FindAll(b => b.IsOverdue).Count;
            int upcomingCount = totalPending - lateCount;

            _headerTitleText = totalPending == 0
                ? "KHÔNG CÓ YÊU CẦU NÀO"
                : $"{totalPending} YÊU CẦU ĐANG CHỜ BẠN";
            _lateFlagText = lateCount > 0 ? $"{lateCount} yêu cầu đã quá giờ tập" : string.Empty;
            pnlHeaderCard.Invalidate();

            btnFilterAll.Text = $"Tất cả ({totalPending})";
            btnFilterUpcoming.Text = $"Sắp tới ({upcomingCount})";
            btnFilterOverdue.Text = $"Quá giờ ({lateCount})";
            UpdateFilterChipStyles();
            RelayoutToolbar();

            var filtered = _bookingService.GetPendingRequests(_ptId, _currentFilter, txtSearch.Text);

            pnlRequestsList.SuspendLayout();
            pnlRequestsList.Controls.Clear();

            int rowWidth = GetTargetRowWidth();

            if (filtered.Count == 0)
            {
                pnlRequestsList.Controls.Add(CreateEmptyStatePanel(rowWidth, totalPending == 0));
            }
            else
            {
                for (int i = 0; i < filtered.Count; i++)
                {
                    pnlRequestsList.Controls.Add(CreateRequestRow(filtered[i], rowWidth, isFirst: i == 0));
                }
            }

            pnlRequestsList.ResumeLayout(true);
            RelayoutRowsWidth();
        }

        private Panel CreateRequestRow(BookingRequestDto req, int width, bool isFirst)
        {
            bool isRejectOpen = (_expandedRejectScheduleId == req.ScheduleID);
            int baseHeight = 152;
            int expandedHeight = 264;
            bool isHovered = false;

            var row = new PTUITheme.DoubleBufferedPanel
            {
                Width = width,
                Height = isRejectOpen ? expandedHeight : baseHeight,
                Margin = new Padding(0),
                BackColor = Color.FromArgb(18, 20, 26)
            };

            row.MouseEnter += (s, e) => { isHovered = true; row.Invalidate(); };
            row.MouseLeave += (s, e) =>
            {
                if (!row.ClientRectangle.Contains(row.PointToClient(Cursor.Position)))
                {
                    isHovered = false;
                    row.Invalidate();
                }
            };

            row.Paint += (s, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = row.ClientRectangle;

                // Nền cơ bản & hiệu ứng hover (.cx-row:hover)
                Color baseBg = isHovered ? Color.FromArgb(24, 27, 34) : Color.FromArgb(18, 20, 26);
                using (var bBg = new SolidBrush(baseBg))
                {
                    g.FillRectangle(bBg, r);
                }

                // Gradient nhẹ màu amber từ trái sang nếu quá giờ (.cx-req.is-late)
                if (req.IsOverdue)
                {
                    int gradW = Math.Max(220, (int)(r.Width * 0.42));
                    Rectangle gradRect = new Rectangle(0, 0, gradW, r.Height);
                    using var grad = new LinearGradientBrush(
                        gradRect,
                        Color.FromArgb(24, 242, 177, 52),
                        Color.FromArgb(0, 242, 177, 52),
                        LinearGradientMode.Horizontal);
                    g.FillRectangle(grad, gradRect);
                }

                // Đường kẻ ngang phân tách 1px ở trên (trừ hàng đầu tiên)
                if (!isFirst)
                {
                    using var topPen = new Pen(Color.FromArgb(37, 41, 52), 1f);
                    g.DrawLine(topPen, 0, 0, r.Width, 0);
                }

                // 1. Cột Ngày bên trái (.cx-date--lg - số trắng to, box cao đủ rộng không bao giờ cắt số)
                int dateX = 24;
                using var dayNumFont = new Font("Segoe UI", 22f, FontStyle.Bold);
                TextRenderer.DrawText(g, req.BookingDate.ToString("dd"), dayNumFont,
                    new Rectangle(dateX, 14, 72, 46),
                    Color.FromArgb(241, 243, 238),
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                TextRenderer.DrawText(g, req.DayShort, PTUITheme.FontSmall,
                    new Rectangle(dateX + 2, 62, 68, 18),
                    PTUITheme.TextMuted,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                TextRenderer.DrawText(g, $"Th{req.BookingDate.Month}", PTUITheme.FontSmall,
                    new Rectangle(dateX + 2, 80, 68, 18),
                    PTUITheme.TextMuted,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                // 2. Cột Nội dung chính ở giữa (.cx-req-main)
                int contentX = 114;
                int rightActWidth = 224;
                int contentW = Math.Max(240, row.Width - contentX - rightActWidth - 20);

                // Hàng 1: Giờ tập to đậm + trạng thái chữ thuần + phòng tập
                using var timeFont = new Font("Segoe UI", 14.5f, FontStyle.Bold);
                Size timeSize = TextRenderer.MeasureText(g, req.TimeSlotText, timeFont,
                    Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                TextRenderer.DrawText(g, req.TimeSlotText, timeFont,
                    new Rectangle(contentX, 16, timeSize.Width + 8, 32),
                    Color.FromArgb(241, 243, 238),
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                int relX = contentX + timeSize.Width + 14;
                Color relColor = req.IsOverdue ? Color.FromArgb(242, 177, 52) : PTUITheme.PrimaryLime;
                Size relSize = TextRenderer.MeasureText(g, req.RelativeDayText, PTUITheme.FontSmallBold,
                    Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                TextRenderer.DrawText(g, req.RelativeDayText, PTUITheme.FontSmallBold,
                    new Rectangle(relX, 21, relSize.Width + 6, 22),
                    relColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                int roomX = relX + relSize.Width + 14;
                if (contentX + contentW - roomX > 90)
                {
                    TextRenderer.DrawText(g, $"•  {req.RoomName}", PTUITheme.FontSmall,
                        new Rectangle(roomX, 21, contentX + contentW - roomX, 22),
                        PTUITheme.TextMuted,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
                }

                // Hàng 2: Avatar vuông nhỏ (.cx-avatar--sm) + Tên + SĐT & Gói tập
                Rectangle avRect = new Rectangle(contentX, 54, 36, 36);
                using (var bAv = new SolidBrush(Color.FromArgb(23, 26, 33)))
                    g.FillRectangle(bAv, avRect);
                using (var pAv = new Pen(Color.FromArgb(52, 57, 68), 1f))
                    g.DrawRectangle(pAv, avRect);
                TextRenderer.DrawText(g, req.MemberInitials, new Font("Segoe UI", 9.5f, FontStyle.Bold),
                    avRect, PTUITheme.PrimaryLime,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                int memX = avRect.Right + 12;
                int memW = Math.Max(160, contentW - 48);
                TextRenderer.DrawText(g, $"{req.MemberName}  ({req.MemberCode})", PTUITheme.FontBold,
                    new Rectangle(memX, 52, memW, 20),
                    Color.FromArgb(241, 243, 238),
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

                string subMember = string.IsNullOrWhiteSpace(req.Phone)
                    ? $"{req.Email}  •  {req.PackageName}"
                    : $"{req.Phone}  •  {req.PackageName}";
                TextRenderer.DrawText(g, subMember, PTUITheme.FontSmall,
                    new Rectangle(memX, 72, memW, 18),
                    PTUITheme.TextMuted,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

                // Hàng 3: Ghi chú (.cx-quote - góc vuông, nền nhẹ, chỉ có vạch dọc cạnh trái)
                int quoteW = Math.Min(contentW, 640);
                Rectangle quoteRect = new Rectangle(contentX, 100, quoteW, 38);
                using (var bQuote = new SolidBrush(Color.FromArgb(25, 28, 35)))
                    g.FillRectangle(bQuote, quoteRect);
                using (var bBar = new SolidBrush(Color.FromArgb(58, 65, 80)))
                    g.FillRectangle(bBar, quoteRect.X, quoteRect.Y, 2, quoteRect.Height);

                bool hasNote = !string.IsNullOrWhiteSpace(req.Notes);
                string noteDisplay = hasNote ? req.Notes : "Học viên không để lại ghi chú.";
                using var noteFont = new Font("Segoe UI", 9f, hasNote ? FontStyle.Regular : FontStyle.Italic);
                Color noteColor = hasNote ? Color.FromArgb(228, 232, 238) : Color.FromArgb(172, 180, 196);
                TextRenderer.DrawText(g, noteDisplay, noteFont,
                    new Rectangle(quoteRect.X + 14, quoteRect.Y + 4, quoteRect.Width - 24, quoteRect.Height - 8),
                    noteColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
            };

            // 3. Cột nút thao tác bên phải (.cx-req-act: góc vuông, chữ IN HOA)
            int actX = row.Width - 224;
            Button btnApprove = new Button
            {
                Text = "DUYỆT CA NÀY",
                Size = new Size(200, 40),
                Location = new Point(actX, 20),
                FlatStyle = FlatStyle.Flat,
                BackColor = PTUITheme.PrimaryLime,
                ForeColor = Color.FromArgb(10, 10, 10),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnApprove.FlatAppearance.BorderSize = 0;
            btnApprove.MouseEnter += (s, e) => btnApprove.BackColor = PTUITheme.LimeHover;
            btnApprove.MouseLeave += (s, e) => btnApprove.BackColor = PTUITheme.PrimaryLime;
            btnApprove.Click += (s, e) =>
            {
                var res = _bookingService.ApproveRequest(req.ScheduleID, _ptId);
                if (!res.Success)
                {
                    ShowToast(res.Message, false);
                    return;
                }
                _expandedRejectScheduleId = null;
                ShowToast(res.Message, true);
                RefreshView();
            };

            Button btnRejectToggle = new Button
            {
                Text = isRejectOpen ? "ĐÓNG TỪ CHỐI" : "TỪ CHỐI",
                Size = new Size(200, 38),
                Location = new Point(actX, 70),
                FlatStyle = FlatStyle.Flat,
                BackColor = isRejectOpen ? Color.FromArgb(36, 22, 26) : Color.Transparent,
                ForeColor = Color.FromArgb(255, 107, 107),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnRejectToggle.FlatAppearance.BorderSize = 1;
            btnRejectToggle.FlatAppearance.BorderColor = Color.FromArgb(135, 255, 107, 107);
            btnRejectToggle.MouseEnter += (s, e) => btnRejectToggle.BackColor = Color.FromArgb(36, 22, 26);
            btnRejectToggle.MouseLeave += (s, e) => btnRejectToggle.BackColor = isRejectOpen ? Color.FromArgb(36, 22, 26) : Color.Transparent;

            // 4. Ngăn mở rộng nhập lý do từ chối (góc vuông đồng bộ WebGym)
            Panel pnlRejectDrawer = new Panel
            {
                Location = new Point(114, baseHeight - 2),
                Width = Math.Max(300, row.Width - 138),
                Height = 98,
                BackColor = Color.FromArgb(14, 16, 20),
                Visible = isRejectOpen
            };
            pnlRejectDrawer.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(52, 57, 68), 1f);
                e.Graphics.DrawRectangle(p, 0, 0, pnlRejectDrawer.Width - 1, pnlRejectDrawer.Height - 1);
                using var bAcc = new SolidBrush(Color.FromArgb(255, 107, 107));
                e.Graphics.FillRectangle(bAcc, 0, 0, 2, pnlRejectDrawer.Height);
            };

            Label lblReasonPrompt = new Label
            {
                Text = "Lý do gửi cho học viên (tuỳ chọn):",
                Location = new Point(14, 10),
                AutoSize = true,
                Font = PTUITheme.FontSmallBold,
                ForeColor = PTUITheme.TextMuted
            };

            TextBox txtReason = new TextBox
            {
                Location = new Point(14, 34),
                Width = Math.Max(220, pnlRejectDrawer.Width - 310),
                Height = 48,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                MaxLength = 250,
                Font = PTUITheme.FontRegular,
                BackColor = Color.FromArgb(10, 12, 16),
                ForeColor = PTUITheme.TextWhite,
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "Ví dụ: Khung giờ này tôi đã kín lịch, bạn chọn 17:30–19:00 nhé."
            };

            Button btnConfirmReject = new Button
            {
                Text = "XÁC NHẬN TỪ CHỐI",
                Size = new Size(164, 36),
                Location = new Point(pnlRejectDrawer.Width - 280, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(255, 107, 107),
                ForeColor = Color.FromArgb(10, 10, 10),
                Font = PTUITheme.FontSmallBold,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnConfirmReject.FlatAppearance.BorderSize = 0;
            btnConfirmReject.Click += (s, e) =>
            {
                var res = _bookingService.RejectRequest(req.ScheduleID, txtReason.Text, _ptId);
                if (!res.Success)
                {
                    ShowToast(res.Message, false);
                    return;
                }
                _expandedRejectScheduleId = null;
                ShowToast(res.Message, true);
                RefreshView();
            };

            Button btnCancelReject = new Button
            {
                Text = "HUỶ",
                Size = new Size(92, 36),
                Location = new Point(pnlRejectDrawer.Width - 106, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = PTUITheme.TextWhite,
                Font = PTUITheme.FontSmallBold,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnCancelReject.FlatAppearance.BorderSize = 1;
            btnCancelReject.FlatAppearance.BorderColor = Color.FromArgb(52, 57, 68);
            btnCancelReject.Click += (s, e) =>
            {
                _expandedRejectScheduleId = null;
                row.Height = baseHeight;
                pnlRejectDrawer.Visible = false;
                btnRejectToggle.Text = "TỪ CHỐI";
            };

            pnlRejectDrawer.Controls.AddRange(new Control[]
            {
                lblReasonPrompt, txtReason, btnConfirmReject, btnCancelReject
            });

            btnRejectToggle.Click += (s, e) =>
            {
                bool openNow = !pnlRejectDrawer.Visible;
                _expandedRejectScheduleId = openNow ? req.ScheduleID : null;
                pnlRejectDrawer.Visible = openNow;
                row.Height = openNow ? expandedHeight : baseHeight;
                btnRejectToggle.Text = openNow ? "ĐÓNG TỪ CHỐI" : "TỪ CHỐI";
                if (openNow) txtReason.Focus();
            };

            row.Resize += (s, e) =>
            {
                int newActX = row.Width - 224;
                btnApprove.Location = new Point(newActX, 20);
                btnRejectToggle.Location = new Point(newActX, 70);

                pnlRejectDrawer.Width = Math.Max(300, row.Width - 138);
                txtReason.Width = Math.Max(200, pnlRejectDrawer.Width - 310);
                btnConfirmReject.Location = new Point(pnlRejectDrawer.Width - 280, 40);
                btnCancelReject.Location = new Point(pnlRejectDrawer.Width - 106, 40);
                row.Invalidate();
            };

            row.Controls.Add(btnApprove);
            row.Controls.Add(btnRejectToggle);
            row.Controls.Add(pnlRejectDrawer);
            return row;
        }

        private Panel CreateEmptyStatePanel(int width, bool noPendingAtAll)
        {
            var panel = new PTUITheme.DoubleBufferedPanel
            {
                Width = width,
                Height = 140,
                BackColor = Color.FromArgb(18, 20, 26),
                Margin = new Padding(0)
            };

            Label lblMain = new Label
            {
                Text = noPendingAtAll ? "Bạn đã xử lý hết yêu cầu." : "Không tìm thấy yêu cầu phù hợp với bộ lọc.",
                Location = new Point(24, 30),
                Width = Math.Max(260, width - 210),
                Height = 24,
                Font = PTUITheme.FontSection,
                ForeColor = Color.FromArgb(241, 243, 238)
            };

            Label lblDesc = new Label
            {
                Text = noPendingAtAll
                    ? "Khi học viên đặt lịch mới với bạn, yêu cầu sẽ hiện ở đây và số lượng sẽ hiện cạnh mục Yêu cầu đặt lịch trên menu."
                    : "Thử chọn bộ lọc \"Tất cả\" hoặc xoá từ khoá tìm kiếm để xem các yêu cầu khác.",
                Location = new Point(24, 58),
                Width = Math.Max(260, width - 220),
                Height = 44,
                Font = PTUITheme.FontRegular,
                ForeColor = PTUITheme.TextMuted
            };

            Button btnBack = new Button
            {
                Text = "VỀ LỊCH DẠY",
                Size = new Size(134, 38),
                Location = new Point(width - 158, 46),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = PTUITheme.PrimaryLime,
                Font = PTUITheme.FontSmallBold,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnBack.FlatAppearance.BorderSize = 1;
            btnBack.FlatAppearance.BorderColor = Color.FromArgb(52, 57, 68);
            btnBack.Click += (s, e) => OnViewScheduleRequested?.Invoke();

            panel.Resize += (s, e) =>
            {
                lblMain.Width = Math.Max(240, panel.Width - 210);
                lblDesc.Width = Math.Max(240, panel.Width - 220);
                btnBack.Location = new Point(panel.Width - 158, 46);
            };

            panel.Controls.AddRange(new Control[] { lblMain, lblDesc, btnBack });
            return panel;
        }

        private int GetTargetRowWidth()
        {
            return Math.Max(680, pnlRequestsList.ClientSize.Width);
        }

        private void RelayoutRowsWidth()
        {
            if (pnlRequestsList.ClientSize.Width <= 0) return;
            int targetW = GetTargetRowWidth();
            pnlRequestsList.SuspendLayout();
            foreach (Control c in pnlRequestsList.Controls)
            {
                if (c.Width != targetW) c.Width = targetW;
            }
            pnlRequestsList.ResumeLayout(true);
        }

        private void RelayoutToolbar()
        {
            int w = pnlToolbar.ClientSize.Width;
            if (w <= 0) return;

            btnFilterAll.Location = new Point(0, 4);
            btnFilterAll.Size = new Size(100, 34);

            btnFilterUpcoming.Location = new Point(btnFilterAll.Right + 6, 4);
            btnFilterUpcoming.Size = new Size(106, 34);

            btnFilterOverdue.Location = new Point(btnFilterUpcoming.Right + 6, 4);
            btnFilterOverdue.Size = new Size(106, 34);

            btnViewSchedule.Location = new Point(w - btnViewSchedule.Width, 4);
            btnSearch.Location = new Point(btnViewSchedule.Left - btnSearch.Width - 10, 4);

            int maxSearchW = Math.Max(140, btnSearch.Left - btnFilterOverdue.Right - 24);
            pnlSearchBox.Size = new Size(Math.Min(260, maxSearchW), 34);
            pnlSearchBox.Location = new Point(btnSearch.Left - pnlSearchBox.Width - 6, 4);
        }

        private Button CreateFilterChip(string text, string filterKey)
        {
            var btn = new Button
            {
                Text = text,
                Tag = filterKey,
                Size = new Size(104, 34),
                FlatStyle = FlatStyle.Flat,
                Font = PTUITheme.FontSmallBold,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btn.FlatAppearance.BorderSize = 1;
            btn.Click += (s, e) =>
            {
                _currentFilter = filterKey;
                RefreshView();
            };
            return btn;
        }

        private void UpdateFilterChipStyles()
        {
            ApplyChipStyle(btnFilterAll, _currentFilter == "all");
            ApplyChipStyle(btnFilterUpcoming, _currentFilter == "upcoming");
            ApplyChipStyle(btnFilterOverdue, _currentFilter == "overdue");
        }

        private static void ApplyChipStyle(Button btn, bool active)
        {
            if (active)
            {
                btn.BackColor = Color.FromArgb(241, 243, 238);
                btn.ForeColor = Color.FromArgb(10, 10, 10);
                btn.FlatAppearance.BorderColor = Color.FromArgb(241, 243, 238);
            }
            else
            {
                btn.BackColor = Color.FromArgb(18, 20, 26);
                btn.ForeColor = PTUITheme.TextMuted;
                btn.FlatAppearance.BorderColor = Color.FromArgb(52, 57, 68);
            }
        }

        private void ShowToast(string msg, bool isSuccess)
        {
            _toastIsSuccess = isSuccess;
            lblToastMsg.Text = msg;
            lblToastMsg.ForeColor = isSuccess ? PTUITheme.PrimaryLime : Color.FromArgb(255, 179, 179);
            pnlToast.Visible = true;
            pnlToast.Invalidate();
        }

        private static void EnableDoubleBuffer(Control ctrl)
        {
            try
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                    null, ctrl, new object[] { true });
            }
            catch { }
        }
    }
}
