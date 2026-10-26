using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyGym.UI.Utils;

namespace QuanLyGym.UI.Forms.Member.Views
{
    public class ucMemberDashboard : UserControl
    {
        private Action<string>? _navigateCallback;

        public ucMemberDashboard(Action<string>? navigateCallback = null)
        {
            _navigateCallback = navigateCallback;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = UITheme.Background;
            this.Dock = DockStyle.Fill;
            this.AutoScroll = true;

            // Main container with padding
            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Top,
                Height = 940,
                Padding = new Padding(24, 20, 24, 30),
                BackColor = UITheme.Background
            };

            // 1. Welcome Greeting Banner
            RoundedPanel pnlBanner = CreateWelcomeBanner();
            pnlBanner.Dock = DockStyle.Top;
            pnlBanner.Margin = new Padding(0, 0, 0, 16);

            // 2. 4 Stat Cards Row
            TableLayoutPanel pnlStats = CreateStatCards();
            pnlStats.Dock = DockStyle.Top;
            pnlStats.Margin = new Padding(0, 0, 0, 20);

            // 3. Two Column Layout (Upcoming Workouts vs PT / InBody info)
            TableLayoutPanel pnlColumns = CreateTwoColumnContent();
            pnlColumns.Dock = DockStyle.Top;

            pnlMain.Controls.Add(pnlColumns);
            pnlMain.Controls.Add(pnlStats);
            pnlMain.Controls.Add(pnlBanner);

            this.Controls.Add(pnlMain);
            this.ResumeLayout(true);
        }

        private RoundedPanel CreateWelcomeBanner()
        {
            RoundedPanel banner = new RoundedPanel
            {
                Height = 100,
                CornerRadius = 14,
                BackColor = Color.FromArgb(49, 46, 129),          // Indigo 900
                GradientEndColor = Color.FromArgb(79, 70, 229),    // Indigo 600
                GradientAngle = 20f,
                BorderWidth = 0,
                Padding = new Padding(24, 18, 24, 18)
            };

            Label lblGreeting = new Label
            {
                Text = "Chào mừng trở lại, Trần Văn Hùng! 👋",
                Font = new Font("Segoe UI", 15f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(24, 18)
            };

            Label lblSub = new Label
            {
                Text = "Chúc bạn có một buổi tập luyện tràn đầy năng lượng và hiệu quả hôm nay. Kiên trì là chìa khóa của thành công!",
                Font = UITheme.FontRegular,
                ForeColor = Color.FromArgb(224, 231, 255),
                AutoSize = true,
                Location = new Point(24, 52)
            };

            Button btnCheckInQuick = new Button
            {
                Text = "📸 Điểm danh ngay",
                Size = new Size(160, 42),
                Font = UITheme.FontBold,
                BackColor = Color.FromArgb(16, 185, 129), // Emerald
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            btnCheckInQuick.FlatAppearance.BorderSize = 0;
            btnCheckInQuick.Location = new Point(banner.Width - 190, 28);
            btnCheckInQuick.Click += (s, e) => _navigateCallback?.Invoke("CheckIn");

            banner.Controls.Add(lblGreeting);
            banner.Controls.Add(lblSub);
            banner.Controls.Add(btnCheckInQuick);
            banner.Resize += (s, e) =>
            {
                btnCheckInQuick.Location = new Point(banner.Width - 190, 28);
            };

            return banner;
        }

        private TableLayoutPanel CreateStatCards()
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                ColumnCount = 4,
                RowCount = 1,
                Height = 135,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 16, 0, 8)
            };

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            table.RowStyles.Clear();
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            // Card 1: Gói tập hiện tại
            var card1 = CreateCardWidget("GÓI TẬP HỘI VIÊN", "VIP Diamond", "12 / 60 buổi (Hạn: 31/12/2026)", "💎", Color.FromArgb(79, 70, 229), 20);

            // Card 2: Buổi tập kế tiếp
            var card2 = CreateCardWidget("BUỔI TẬP KẾ TIẾP", "Hôm nay 18:00", "HLV: Nam | Phòng: PT-01", "📅", Color.FromArgb(14, 165, 233), null);

            // Card 3: Điểm danh tháng
            var card3 = CreateCardWidget("ĐIỂM DANH THÁNG NÀY", "14 Buổi", "Đã duyệt: 14 | Chờ duyệt: 1", "✅", Color.FromArgb(16, 185, 129), 92);

            // Card 4: Thể trạng InBody
            var card4 = CreateCardWidget("CHỈ SỐ THỂ HÌNH", "68.0 kg", "BMI: 23.0 (Cân đối) | Mỡ: 18%", "⚖️", Color.FromArgb(245, 158, 11), null);

            table.Controls.Add(card1, 0, 0);
            table.Controls.Add(card2, 1, 0);
            table.Controls.Add(card3, 2, 0);
            table.Controls.Add(card4, 3, 0);

            return table;
        }

        private RoundedPanel CreateCardWidget(string tag, string mainValue, string subText, string icon, Color accentColor, int? progressVal)
        {
            RoundedPanel card = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Margin = new Padding(6),
                Padding = new Padding(16, 14, 16, 14)
            };

            Label lblTag = new Label
            {
                Text = tag,
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.TextMuted,
                AutoSize = true,
                Location = new Point(14, 12)
            };

            Label lblVal = new Label
            {
                Text = mainValue,
                Font = new Font("Segoe UI", 15f, FontStyle.Bold),
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(14, 32)
            };

            Label lblSub = new Label
            {
                Text = subText,
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextMuted,
                AutoSize = true,
                Location = new Point(14, 68)
            };

            // Icon Badge
            Panel iconBadge = new Panel
            {
                Size = new Size(38, 38),
                BackColor = Color.FromArgb(30, accentColor),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(card.Width - 48, 14)
            };
            Label lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI Emoji", 13f),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            iconBadge.Controls.Add(lblIcon);

            card.Controls.Add(lblTag);
            card.Controls.Add(lblVal);
            card.Controls.Add(lblSub);
            card.Controls.Add(iconBadge);

            if (progressVal.HasValue)
            {
                ModernProgressBar bar = new ModernProgressBar
                {
                    Value = progressVal.Value,
                    Maximum = 100,
                    ProgressColor = accentColor,
                    ProgressColor2 = accentColor,
                    Height = 6,
                    Width = 120,
                    Location = new Point(14, 90),
                    Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
                };
                card.Controls.Add(bar);
            }

            card.Resize += (s, e) =>
            {
                iconBadge.Location = new Point(card.Width - 48, 14);
            };

            return card;
        }

        private TableLayoutPanel CreateTwoColumnContent()
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Height = 650,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 10, 0, 10)
            };

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67f)); // Left column: Workouts & Check-ins
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f)); // Right column: PT & InBody & Tips
            table.RowStyles.Clear();
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            // --- LEFT COLUMN ---
            Panel pnlLeft = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 10, 0) };

            // 1. Upcoming Workouts Card
            RoundedPanel cardSchedule = CreateUpcomingScheduleCard();
            cardSchedule.Dock = DockStyle.Top;
            cardSchedule.Margin = new Padding(0, 0, 0, 16);

            // 2. Recent Check-Ins Card
            RoundedPanel cardRecentCheckIn = CreateRecentCheckInsCard();
            cardRecentCheckIn.Dock = DockStyle.Top;

            pnlLeft.Controls.Add(cardRecentCheckIn);
            pnlLeft.Controls.Add(cardSchedule);

            // --- RIGHT COLUMN ---
            Panel pnlRight = new Panel { Dock = DockStyle.Fill, Margin = new Padding(6, 0, 0, 0) };

            // 1. Personal Trainer Card
            RoundedPanel cardPT = CreateTrainerCard();
            cardPT.Dock = DockStyle.Top;
            cardPT.Margin = new Padding(0, 0, 0, 16);

            // 2. InBody Progress Snapshot Card
            RoundedPanel cardInBody = CreateInBodySnapshotCard();
            cardInBody.Dock = DockStyle.Top;
            cardInBody.Margin = new Padding(0, 0, 0, 16);

            // 3. Quick Tips Card
            RoundedPanel cardTips = CreateTipsCard();
            cardTips.Dock = DockStyle.Top;

            pnlRight.Controls.Add(cardTips);
            pnlRight.Controls.Add(cardInBody);
            pnlRight.Controls.Add(cardPT);

            table.Controls.Add(pnlLeft, 0, 0);
            table.Controls.Add(pnlRight, 1, 0);

            return table;
        }

        private RoundedPanel CreateUpcomingScheduleCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 330,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(20)
            };

            // Header Row
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 40 };
            Label lblTitle = new Label
            {
                Text = "📅 LỊCH TẬP SẮP TỚI TRONG TUẦN",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(0, 6)
            };
            Button btnViewAll = new Button
            {
                Text = "+ Đặt lịch mới",
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.Primary,
                BackColor = UITheme.PrimaryLight,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(115, 32),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnViewAll.FlatAppearance.BorderSize = 0;
            btnViewAll.Location = new Point(card.Width - 140, 4);
            btnViewAll.Click += (s, e) => _navigateCallback?.Invoke("Schedule");

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnViewAll);
            pnlHeader.Resize += (s, e) => btnViewAll.Location = new Point(pnlHeader.Width - 120, 4);

            // DataGridView for upcoming schedules
            DataGridView dgv = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgv);

            var colDate = new DataGridViewTextBoxColumn { Name = "Date", HeaderText = "Ngày tập", Width = 110, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colTime = new DataGridViewTextBoxColumn { Name = "Time", HeaderText = "Ca tập", Width = 115, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colType = new DataGridViewTextBoxColumn { Name = "Type", HeaderText = "Bộ môn", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 130 };
            var colTrainer = new DataGridViewTextBoxColumn { Name = "Trainer", HeaderText = "Huấn luyện viên", Width = 165, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colRoom = new DataGridViewTextBoxColumn { Name = "Room", HeaderText = "Phòng", Width = 85, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colStatus = new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Trạng thái", Width = 125, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };

            dgv.Columns.AddRange(colDate, colTime, colType, colTrainer, colRoom, colStatus);

            // Sample clean data
            dgv.Rows.Add("Hôm nay", "18:00 - 19:30", "Gym Thể hình", "HLV Nguyễn Văn Nam", "PT-01", "Đã xác nhận");
            dgv.Rows.Add("Thứ 4, 21/10", "17:30 - 19:00", "Cardio & HIIT", "HLV Nguyễn Văn Nam", "Room 02", "Đã xác nhận");
            dgv.Rows.Add("Thứ 6, 23/10", "18:00 - 19:30", "Ngực & Tay trước", "HLV Nguyễn Văn Nam", "PT-01", "Đang chờ duyệt");
            dgv.Rows.Add("Chủ nhật, 25/10", "08:30 - 10:00", "Yoga & Giãn cơ", "HLV Lê Thu Hà", "Studio A", "Đã xác nhận");

            // Format status cell color
            dgv.CellFormatting += (s, e) =>
            {
                if (dgv.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
                {
                    string status = e.Value.ToString() ?? "";
                    if (status.Contains("Đã xác nhận"))
                    {
                        e.CellStyle.ForeColor = UITheme.Success;
                        e.CellStyle.Font = UITheme.FontBold;
                    }
                    else if (status.Contains("chờ"))
                    {
                        e.CellStyle.ForeColor = UITheme.Warning;
                        e.CellStyle.Font = UITheme.FontBold;
                    }
                }
            };

            card.Controls.Add(dgv);
            card.Controls.Add(pnlHeader);

            return card;
        }

        private RoundedPanel CreateRecentCheckInsCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 280,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(20)
            };

            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 36 };
            Label lblTitle = new Label
            {
                Text = "📸 NHẬT KÝ ĐIỂM DANH CHECK-IN GẦN ĐÂY",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(0, 4)
            };
            Button btnViewAll = new Button
            {
                Text = "Xem tất cả >",
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.Primary,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 28),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnViewAll.FlatAppearance.BorderSize = 0;
            btnViewAll.Location = new Point(card.Width - 120, 4);
            btnViewAll.Click += (s, e) => _navigateCallback?.Invoke("CheckIn");

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnViewAll);
            pnlHeader.Resize += (s, e) => btnViewAll.Location = new Point(pnlHeader.Width - 110, 4);

            DataGridView dgv = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgv);

            var colId = new DataGridViewTextBoxColumn { Name = "CheckInID", HeaderText = "Mã", Width = 85, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colTime = new DataGridViewTextBoxColumn { Name = "Time", HeaderText = "Thời gian check-in", Width = 145, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colPT = new DataGridViewTextBoxColumn { Name = "PT", HeaderText = "HLV xác nhận", Width = 155, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colNote = new DataGridViewTextBoxColumn { Name = "Note", HeaderText = "Ghi chú buổi tập", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 160 };
            var colStatus = new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Trạng thái", Width = 120, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };

            dgv.Columns.AddRange(colId, colTime, colPT, colNote, colStatus);

            dgv.Rows.Add("#CK-109", "15/10/2026 18:05", "Nguyễn Văn Nam", "Buổi tập Chân & Vai", "Đã duyệt");
            dgv.Rows.Add("#CK-108", "13/10/2026 18:12", "Nguyễn Văn Nam", "Cardio 45 phút + Bụng", "Đã duyệt");
            dgv.Rows.Add("#CK-107", "11/10/2026 17:58", "Nguyễn Văn Nam", "Lưng xô & Tay sau", "Đã duyệt");

            dgv.CellFormatting += (s, e) =>
            {
                if (dgv.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
                {
                    e.CellStyle.ForeColor = UITheme.Success;
                    e.CellStyle.Font = UITheme.FontBold;
                }
            };

            card.Controls.Add(dgv);
            card.Controls.Add(pnlHeader);

            return card;
        }

        private RoundedPanel CreateTrainerCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 220,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(18)
            };

            Label lblTitle = new Label
            {
                Text = "🏋️ HUẤN LUYỆN VIÊN (PT)",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = 26
            };

            // PT Info Panel
            Panel pnlPT = new Panel { Dock = DockStyle.Top, Height = 80, Margin = new Padding(0, 6, 0, 6) };

            // Avatar placeholder
            Panel pnlAvatar = new Panel
            {
                Size = new Size(60, 60),
                BackColor = Color.FromArgb(238, 242, 255),
                Location = new Point(4, 6)
            };
            Label lblAvatar = new Label
            {
                Text = "👨‍🏫",
                Font = new Font("Segoe UI Emoji", 24f),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlAvatar.Controls.Add(lblAvatar);

            Label lblName = new Label
            {
                Text = "HLV. Nguyễn Văn Nam",
                Font = UITheme.FontBold,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(74, 8)
            };
            Label lblRole = new Label
            {
                Text = "Chuyên gia Thể hình & Dinh dưỡng",
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextMuted,
                AutoSize = true,
                Location = new Point(74, 30)
            };
            Label lblPhone = new Label
            {
                Text = "📞 Hotline: 0988.123.456 (Zalo)",
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.Primary,
                AutoSize = true,
                Location = new Point(74, 50)
            };

            pnlPT.Controls.Add(pnlAvatar);
            pnlPT.Controls.Add(lblName);
            pnlPT.Controls.Add(lblRole);
            pnlPT.Controls.Add(lblPhone);

            // Rating & Action buttons
            Label lblRating = new Label
            {
                Text = "Đánh giá hội viên: ⭐⭐⭐⭐⭐ 5.0 (48 lượt)",
                Font = UITheme.FontSmallBold,
                ForeColor = Color.FromArgb(217, 119, 6),
                Dock = DockStyle.Top,
                Height = 24
            };

            Button btnContactPT = new Button
            {
                Text = "💬 Nhắn tin trao đổi với HLV",
                Font = UITheme.FontBold,
                Dock = DockStyle.Bottom,
                Height = 38,
                BackColor = UITheme.PrimaryLight,
                ForeColor = UITheme.Primary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnContactPT.FlatAppearance.BorderSize = 0;
            btnContactPT.Click += (s, e) => MessageBox.Show("Đã kết nối với HLV Nguyễn Văn Nam qua số: 0988.123.456", "Liên hệ HLV", MessageBoxButtons.OK, MessageBoxIcon.Information);

            card.Controls.Add(btnContactPT);
            card.Controls.Add(lblRating);
            card.Controls.Add(pnlPT);
            card.Controls.Add(lblTitle);

            return card;
        }

        private RoundedPanel CreateInBodySnapshotCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 265,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(18, 16, 18, 16)
            };

            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 32 };
            Label lblTitle = new Label
            {
                Text = "📊 TIẾN ĐỘ VÓC DÁNG (INBODY)",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(0, 2)
            };
            Button btnMore = new Button
            {
                Text = "Chi tiết >",
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.Primary,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(70, 26),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnMore.FlatAppearance.BorderSize = 0;
            btnMore.Location = new Point(card.Width - 90, 0);
            btnMore.Click += (s, e) => _navigateCallback?.Invoke("Progress");

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnMore);
            pnlHeader.Resize += (s, e) => btnMore.Location = new Point(pnlHeader.Width - 80, 0);

            // Metrics Grid (2 rows x 2 cols - spacious and completely visible)
            TableLayoutPanel grid = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 2,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 10, 0, 0)
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

            grid.Controls.Add(CreateMiniMetric("Chiều cao", "172 cm", "Chuẩn"), 0, 0);
            grid.Controls.Add(CreateMiniMetric("Cân nặng", "68.0 kg", "-1.5 kg tháng này"), 1, 0);
            grid.Controls.Add(CreateMiniMetric("Tỷ lệ mỡ (Fat)", "18.0 %", "Mục tiêu: 15%"), 0, 1);
            grid.Controls.Add(CreateMiniMetric("Khối lượng cơ", "32.5 kg", "+0.8 kg tăng"), 1, 1);

            card.Controls.Add(grid);
            card.Controls.Add(pnlHeader);

            return card;
        }

        private Panel CreateMiniMetric(string label, string val, string sub)
        {
            Panel pnl = new Panel { Dock = DockStyle.Fill, Margin = new Padding(4, 2, 4, 4) };
            Label lbl = new Label { Text = label, Font = UITheme.FontSmall, ForeColor = UITheme.TextMuted, AutoSize = true, Location = new Point(0, 2) };
            Label v = new Label { Text = val, Font = new Font("Segoe UI", 12f, FontStyle.Bold), ForeColor = UITheme.TextDark, AutoSize = true, Location = new Point(0, 22) };
            Label s = new Label { Text = sub, Font = new Font("Segoe UI", 8.5f), ForeColor = UITheme.Success, AutoSize = true, Location = new Point(0, 48) };
            pnl.Controls.Add(lbl);
            pnl.Controls.Add(v);
            pnl.Controls.Add(s);
            return pnl;
        }

        private RoundedPanel CreateTipsCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 100,
                CornerRadius = 12,
                BackColor = Color.FromArgb(254, 243, 199),  // Amber Light
                BorderColor = Color.FromArgb(253, 230, 138),
                Padding = new Padding(14)
            };

            Label lblTitle = new Label
            {
                Text = "💡 LỜI KHUYÊN DÀNH CHO BẠN",
                Font = UITheme.FontSmallBold,
                ForeColor = Color.FromArgb(180, 83, 9),
                Dock = DockStyle.Top,
                Height = 20
            };

            Label lblTip = new Label
            {
                Text = "Uống đủ 2 - 2.5 lít nước mỗi ngày và ngủ đủ 7-8 tiếng để cơ bắp phục hồi tối đa sau các buổi tập cường độ cao.",
                Font = UITheme.FontSmall,
                ForeColor = Color.FromArgb(146, 64, 14),
                Dock = DockStyle.Fill
            };

            card.Controls.Add(lblTip);
            card.Controls.Add(lblTitle);

            return card;
        }
    }
}
