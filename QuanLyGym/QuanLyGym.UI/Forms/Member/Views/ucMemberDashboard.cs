using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyGym.UI.Forms.Member;
using QuanLyGym.UI.Helpers;
using QuanLyGym.UI.Services;
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
            this.AutoScrollMinSize = new Size(1000, 640);

            float scale = Math.Max(1.0f, (float)this.DeviceDpi / 96f);

            int bannerHeight = (int)Math.Round(95f * scale);
            int statRowHeight = (int)Math.Round(138f * scale);

            // Bố cục gốc dùng TableLayoutPanel 3 hàng co giãn linh hoạt
            TableLayoutPanel pnlRootLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding((int)(16 * scale), (int)(12 * scale), (int)(16 * scale), (int)(14 * scale)),
                BackColor = UITheme.Background
            };
            pnlRootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            pnlRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, bannerHeight));  // Hàng 0: Banner chào mừng
            pnlRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, statRowHeight)); // Hàng 1: 4 Thẻ thống kê
            pnlRootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));          // Hàng 2: Hai cột nội dung

            // 1. Banner chào mừng (Hàng 0)
            RoundedPanel pnlBanner = CreateWelcomeBanner(scale);
            pnlBanner.Dock = DockStyle.Fill;
            pnlBanner.Margin = new Padding(0, 0, 0, (int)(10 * scale));

            // 2. 4 Thẻ thống kê (Hàng 1)
            TableLayoutPanel pnlStats = CreateStatCards(scale);
            pnlStats.Dock = DockStyle.Fill;
            pnlStats.Margin = new Padding(0, 0, 0, (int)(10 * scale));

            // 3. Hai cột nội dung (Hàng 2)
            TableLayoutPanel pnlColumns = CreateTwoColumnContent(scale);
            pnlColumns.Dock = DockStyle.Fill;
            pnlColumns.Margin = new Padding(0);

            pnlRootLayout.Controls.Add(pnlBanner, 0, 0);
            pnlRootLayout.Controls.Add(pnlStats, 0, 1);
            pnlRootLayout.Controls.Add(pnlColumns, 0, 2);

            this.Controls.Add(pnlRootLayout);
            this.ResumeLayout(true);
        }

        private RoundedPanel CreateWelcomeBanner(float scale)
        {
            bool isClassic = MemberService.Instance.IsClassicMember();

            RoundedPanel banner = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = Color.FromArgb(49, 46, 129),          // Indigo 900
                GradientEndColor = Color.FromArgb(79, 70, 229),    // Indigo 600
                GradientAngle = 20f,
                BorderWidth = 0,
                Padding = new Padding(0)
            };

            Panel pnlBtnWrap = new Panel
            {
                Dock = DockStyle.Right,
                Width = (int)(200 * scale),
                Padding = new Padding(0, (int)(24 * scale), (int)(20 * scale), (int)(24 * scale)),
                BackColor = Color.Transparent
            };

            Button btnCheckInQuick = new Button
            {
                Text = isClassic ? "📸 Quẹt thẻ vào cửa" : "📸 Điểm danh ngay",
                Dock = DockStyle.Fill,
                Font = UITheme.FontBold,
                BackColor = Color.FromArgb(16, 185, 129), // Emerald
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnCheckInQuick.FlatAppearance.BorderSize = 0;
            btnCheckInQuick.Click += (s, e) => _navigateCallback?.Invoke("CheckIn");
            pnlBtnWrap.Controls.Add(btnCheckInQuick);

            FlowLayoutPanel pnlGreeting = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding((int)(20 * scale), (int)(16 * scale), 0, 0),
                BackColor = Color.Transparent
            };

            string memberName = MemberService.Instance.GetCurrentMember()?.FullName ?? (string.IsNullOrEmpty(UserSession.DisplayName) ? "Hội viên" : UserSession.DisplayName);
            Label lblGreeting = new Label
            {
                Text = $"Chào mừng trở lại, {memberName}! 👋",
                Font = new Font("Segoe UI", 13.5f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, (int)(5 * scale)),
                UseMnemonic = false
            };

            Label lblSub = new Label
            {
                Text = isClassic
                    ? "Gói tập tự do Classic - Chúc bạn có buổi luyện tập tràn đầy năng lượng và hiệu quả!"
                    : "Chúc bạn có một buổi tập tràn đầy năng lượng và đạt hiệu quả tối đa hôm nay!",
                Font = UITheme.FontRegular,
                ForeColor = Color.FromArgb(224, 231, 255),
                AutoSize = true,
                Margin = new Padding(0),
                UseMnemonic = false
            };

            pnlGreeting.Controls.Add(lblGreeting);
            pnlGreeting.Controls.Add(lblSub);

            banner.Controls.Add(pnlGreeting);
            banner.Controls.Add(pnlBtnWrap);

            return banner;
        }

        private TableLayoutPanel CreateStatCards(float scale)
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                ColumnCount = 4,
                RowCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(0)
            };

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            table.RowStyles.Clear();
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            bool isClassic = MemberService.Instance.IsClassicMember();
            var membership = MemberService.Instance.GetActiveMembership();
            var latestProgress = MemberService.Instance.GetLatestProgress();
            string weightStr = latestProgress != null ? $"{latestProgress.Weight:0.0} kg" : "Chưa có dữ liệu";
            string bodySub = latestProgress != null ? $"BMI: {latestProgress.BMI:0.0} | Mỡ: {latestProgress.BodyFat:0.0}%" : "Đo InBody tại phòng tập";

            if (isClassic)
            {
                // 1. Thẻ GÓI TẬP HỘI VIÊN: Tên gói Classic + Số ngày còn lại
                string remainDaysText;
                int? progressVal = null;
                if (membership != null && membership.EndDate >= DateTime.Today)
                {
                    int days = (membership.EndDate.Date - DateTime.Today).Days;
                    remainDaysText = $"Còn {days} ngày (Hạn: {membership.EndDate:dd/MM/yyyy})";
                    int totalDays = (membership.EndDate.Date - membership.StartDate.Date).Days;
                    if (totalDays > 0)
                    {
                        int elapsed = (DateTime.Today - membership.StartDate.Date).Days;
                        progressVal = Math.Clamp((int)Math.Round((double)elapsed / totalDays * 100), 0, 100);
                    }
                }
                else if (membership != null)
                {
                    remainDaysText = $"Hết hạn ngày {membership.EndDate:dd/MM/yyyy}";
                    progressVal = 100;
                }
                else
                {
                    remainDaysText = "Chưa kích hoạt gói tập";
                }
                var card1 = CreateCardWidget("GÓI TẬP HỘI VIÊN", "Classic", remainDaysText, "🏷️", Color.FromArgb(79, 70, 229), progressVal, scale);

                // 2. Thẻ THỜI GIAN TẬP GẦN NHẤT (Truy xuất từ bảng CheckIns)
                string lastWorkoutTime = "Chưa có lượt tập";
                string lastWorkoutSub = "Quẹt thẻ tại cổng để ghi nhận";
                var lastCheckIn = MemberService.Instance.GetLatestCheckIn();
                if (lastCheckIn != null)
                {
                    if (lastCheckIn.CheckInTime.Date == DateTime.Today)
                        lastWorkoutTime = $"Hôm nay {lastCheckIn.CheckInTime:HH:mm}";
                    else if (lastCheckIn.CheckInTime.Date == DateTime.Today.AddDays(-1))
                        lastWorkoutTime = $"Hôm qua {lastCheckIn.CheckInTime:HH:mm}";
                    else
                        lastWorkoutTime = lastCheckIn.CheckInTime.ToString("dd/MM HH:mm");

                    string bName = lastCheckIn.Branch?.BranchName ?? "Chi nhánh";
                    lastWorkoutSub = $"Tự do ra vào | Check-in {bName}";
                }
                var card2 = CreateCardWidget("THỜI GIAN TẬP GẦN NHẤT", lastWorkoutTime, lastWorkoutSub, "⏱️", Color.FromArgb(14, 165, 233), null, scale);

                // 3. Thẻ SỐ LẦN CHECK-IN THÁNG NÀY (Tập tự do không cần PT duyệt)
                var checkInStats = MemberService.Instance.GetMonthlyCheckInStats(DateTime.Today.Year, DateTime.Today.Month);
                string checkInStr = $"{checkInStats.TotalCount} Lần";
                string checkInSub = checkInStats.TotalCount > 0 ? $"Đã tập {checkInStats.DistinctDaysCount} ngày trong tháng" : "Tự do vào phòng tập (Quẹt thẻ)";
                var card3 = CreateCardWidget("SỐ LẦN CHECK-IN THÁNG NÀY", checkInStr, checkInSub, "🚪", Color.FromArgb(16, 185, 129), null, scale);

                // 4. Thẻ CHỈ SỐ THỂ HÌNH (Lấy từ bảng Progresses)
                var card4 = CreateCardWidget("CHỈ SỐ THỂ HÌNH", weightStr, bodySub, "⚖️", Color.FromArgb(245, 158, 11), null, scale);

                table.Controls.Add(card1, 0, 0);
                table.Controls.Add(card2, 1, 0);
                table.Controls.Add(card3, 2, 0);
                table.Controls.Add(card4, 3, 0);
            }
            else
            {
                // Gói VIP Diamond / Gold / Silver có PT
                string pkgName = membership?.Package?.PackageName ?? "Gói tập có PT";
                int used = membership?.UsedPTSessions ?? 0;
                int total = membership?.Package?.TotalPTSessions ?? 0;
                string expire = membership != null ? membership.EndDate.ToString("dd/MM/yyyy") : "--";
                int progress = total > 0 ? Math.Clamp((int)Math.Round((double)used / total * 100), 0, 100) : 0;
                string subText = total > 0 ? $"{used} / {total} buổi (Hạn: {expire})" : $"Hạn dùng: {expire}";

                var card1 = CreateCardWidget("GÓI TẬP HỘI VIÊN", pkgName, subText, "💎", Color.FromArgb(79, 70, 229), progress, scale);

                // Buổi tập kế tiếp: Lấy từ bảng Schedules (Member đăng ký với PT)
                var nextSchedule = MemberService.Instance.GetNextSchedule();
                string nextTimeStr = "Chưa có lịch hẹn";
                string nextSub = "Bấm Đặt lịch PT để xếp lịch";
                if (nextSchedule != null)
                {
                    if (nextSchedule.TrainingDate.Date == DateTime.Today)
                        nextTimeStr = $"Hôm nay {nextSchedule.StartTime:hh\\:mm}";
                    else if (nextSchedule.TrainingDate.Date == DateTime.Today.AddDays(1))
                        nextTimeStr = $"Ngày mai {nextSchedule.StartTime:hh\\:mm}";
                    else
                        nextTimeStr = $"{nextSchedule.TrainingDate:dd/MM} {nextSchedule.StartTime:hh\\:mm}";

                    string pt = nextSchedule.PT?.FullName ?? "HLV";
                    string branch = nextSchedule.Branch?.BranchName ?? (nextSchedule.PT?.Branch?.BranchName ?? "Chi nhánh");
                    nextSub = $"HLV: {pt} | {branch}";
                }
                var card2 = CreateCardWidget("BUỔI TẬP KẾ TIẾP", nextTimeStr, nextSub, "📅", Color.FromArgb(14, 165, 233), null, scale);

                // Điểm danh tháng này: Lấy từ bảng CheckIns
                var ptCheckInStats = MemberService.Instance.GetMonthlyCheckInStats(DateTime.Today.Year, DateTime.Today.Month);
                string ptCheckInStr = $"{ptCheckInStats.TotalCount} Buổi";
                string ptCheckInSub = $"Đã duyệt: {ptCheckInStats.ApprovedCount} | Chờ duyệt: {ptCheckInStats.PendingCount}";
                int attendanceRate = ptCheckInStats.TotalCount > 0 ? Math.Min(100, (int)Math.Round((double)ptCheckInStats.ApprovedCount / Math.Max(1, ptCheckInStats.TotalCount) * 100)) : 0;
                var card3 = CreateCardWidget("ĐIỂM DANH THÁNG NÀY", ptCheckInStr, ptCheckInSub, "✅", Color.FromArgb(16, 185, 129), attendanceRate > 0 ? attendanceRate : (int?)null, scale);

                // Chỉ số thể hình: Lấy từ bảng Progresses
                var card4 = CreateCardWidget("CHỈ SỐ THỂ HÌNH", weightStr, bodySub, "⚖️", Color.FromArgb(245, 158, 11), null, scale);

                table.Controls.Add(card1, 0, 0);
                table.Controls.Add(card2, 1, 0);
                table.Controls.Add(card3, 2, 0);
                table.Controls.Add(card4, 3, 0);
            }

            return table;
        }

        private RoundedPanel CreateCardWidget(string tag, string mainValue, string subText, string icon, Color accentColor, int? progressVal, float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Margin = new Padding((int)(4 * scale)),
                Padding = new Padding((int)(12 * scale), (int)(10 * scale), (int)(12 * scale), (int)(8 * scale))
            };

            int badgeSize = (int)Math.Round(36f * scale);
            Panel iconBadge = new Panel
            {
                Size = new Size(badgeSize, badgeSize),
                BackColor = Color.FromArgb(28, accentColor),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(Math.Max(50, card.Width - badgeSize - (int)(14 * scale)), (int)(10 * scale))
            };
            Label lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI Emoji", 12f),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                UseMnemonic = false
            };
            iconBadge.Controls.Add(lblIcon);

            FlowLayoutPanel flowText = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 0, (int)(40 * scale), 0)
            };

            Label lblTag = new Label
            {
                Text = tag,
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.TextMuted,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, (int)(3 * scale)),
                UseMnemonic = false
            };

            Label lblVal = new Label
            {
                Text = mainValue,
                Font = new Font("Segoe UI", 13.5f, FontStyle.Bold),
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, (int)(4 * scale)),
                UseMnemonic = false
            };

            Label lblSub = new Label
            {
                Text = subText,
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextMuted,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, (int)(4 * scale)),
                UseMnemonic = false
            };

            flowText.Controls.Add(lblTag);
            flowText.Controls.Add(lblVal);
            flowText.Controls.Add(lblSub);

            card.Controls.Add(flowText);
            card.Controls.Add(iconBadge);

            if (progressVal.HasValue)
            {
                ModernProgressBar bar = new ModernProgressBar
                {
                    Value = progressVal.Value,
                    Maximum = 100,
                    ProgressColor = accentColor,
                    ProgressColor2 = accentColor,
                    Dock = DockStyle.Bottom,
                    Height = (int)(6 * scale)
                };
                card.Controls.Add(bar);
                bar.BringToFront();
            }

            card.Resize += (s, e) =>
            {
                iconBadge.Location = new Point(Math.Max(50, card.Width - badgeSize - (int)(14 * scale)), (int)(10 * scale));
            };

            return card;
        }

        private TableLayoutPanel CreateTwoColumnContent(float scale)
        {
            bool isClassic = MemberService.Instance.IsClassicMember();

            TableLayoutPanel table = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(0)
            };

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36f));
            table.RowStyles.Clear();
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            if (isClassic)
            {
                // CỘT TRÁI CLASSIC: SƠ ĐỒ ĐIỂM DANH DẠNG LỊCH (HEATMAP)
                RoundedPanel calendarCard = CreateWorkoutAttendanceCalendar(scale);
                calendarCard.Dock = DockStyle.Fill;
                calendarCard.Margin = new Padding(0, 0, (int)(8 * scale), 0);
                table.Controls.Add(calendarCard, 0, 0);

                // CỘT PHẢI CLASSIC: BANNER NÂNG CẤP PT 1-1 + INBODY + TIỆN ÍCH + LỜI KHUYÊN
                TableLayoutPanel pnlRight = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 4,
                    ColumnCount = 1,
                    Margin = new Padding((int)(8 * scale), 0, 0, 0)
                };
                pnlRight.RowStyles.Add(new RowStyle(SizeType.Percent, 35f)); // Card Nâng cấp PT 1-1
                pnlRight.RowStyles.Add(new RowStyle(SizeType.Percent, 27f)); // Card InBody
                pnlRight.RowStyles.Add(new RowStyle(SizeType.Percent, 20f)); // Card Tiện ích CLB
                pnlRight.RowStyles.Add(new RowStyle(SizeType.Percent, 18f)); // Card Lời khuyên
                pnlRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

                RoundedPanel cardUpgradePT = CreateUpgradePTCard(scale);
                cardUpgradePT.Dock = DockStyle.Fill;
                cardUpgradePT.Margin = new Padding(0, 0, 0, (int)(5 * scale));

                RoundedPanel cardInBody = CreateInBodySnapshotCard(scale);
                cardInBody.Dock = DockStyle.Fill;
                cardInBody.Margin = new Padding(0, (int)(4 * scale), 0, (int)(5 * scale));

                RoundedPanel cardAmenities = CreateClubAmenitiesCard(scale);
                cardAmenities.Dock = DockStyle.Fill;
                cardAmenities.Margin = new Padding(0, (int)(4 * scale), 0, (int)(5 * scale));

                RoundedPanel cardTips = CreateTipsCard(scale);
                cardTips.Dock = DockStyle.Fill;
                cardTips.Margin = new Padding(0, (int)(4 * scale), 0, 0);

                pnlRight.Controls.Add(cardUpgradePT, 0, 0);
                pnlRight.Controls.Add(cardInBody, 0, 1);
                pnlRight.Controls.Add(cardAmenities, 0, 2);
                pnlRight.Controls.Add(cardTips, 0, 3);

                table.Controls.Add(pnlRight, 1, 0);
            }
            else
            {
                // CỘT TRÁI PT: 2 bảng (Lịch tập 52%, Điểm danh 48%)
                TableLayoutPanel pnlLeft = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 2,
                    ColumnCount = 1,
                    Margin = new Padding(0, 0, (int)(8 * scale), 0)
                };
                pnlLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 52f));
                pnlLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 48f));
                pnlLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

                RoundedPanel cardSchedule = CreateUpcomingScheduleCard(scale);
                cardSchedule.Dock = DockStyle.Fill;
                cardSchedule.Margin = new Padding(0, 0, 0, (int)(6 * scale));

                RoundedPanel cardRecentCheckIn = CreateRecentCheckInsCard(scale);
                cardRecentCheckIn.Dock = DockStyle.Fill;
                cardRecentCheckIn.Margin = new Padding(0, (int)(6 * scale), 0, 0);

                pnlLeft.Controls.Add(cardSchedule, 0, 0);
                pnlLeft.Controls.Add(cardRecentCheckIn, 0, 1);

                // CỘT PHẢI PT: HLV, InBody, Tiện ích, Lời khuyên
                TableLayoutPanel pnlRight = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 4,
                    ColumnCount = 1,
                    Margin = new Padding((int)(8 * scale), 0, 0, 0)
                };
                pnlRight.RowStyles.Add(new RowStyle(SizeType.Percent, 29f)); // Card PT
                pnlRight.RowStyles.Add(new RowStyle(SizeType.Percent, 29f)); // Card InBody
                pnlRight.RowStyles.Add(new RowStyle(SizeType.Percent, 22f)); // Card Tiện ích CLB
                pnlRight.RowStyles.Add(new RowStyle(SizeType.Percent, 20f)); // Card Lời khuyên
                pnlRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

                RoundedPanel cardPT = CreateTrainerCard(scale);
                cardPT.Dock = DockStyle.Fill;
                cardPT.Margin = new Padding(0, 0, 0, (int)(5 * scale));

                RoundedPanel cardInBody = CreateInBodySnapshotCard(scale);
                cardInBody.Dock = DockStyle.Fill;
                cardInBody.Margin = new Padding(0, (int)(4 * scale), 0, (int)(5 * scale));

                RoundedPanel cardAmenities = CreateClubAmenitiesCard(scale);
                cardAmenities.Dock = DockStyle.Fill;
                cardAmenities.Margin = new Padding(0, (int)(4 * scale), 0, (int)(5 * scale));

                RoundedPanel cardTips = CreateTipsCard(scale);
                cardTips.Dock = DockStyle.Fill;
                cardTips.Margin = new Padding(0, (int)(4 * scale), 0, 0);

                pnlRight.Controls.Add(cardPT, 0, 0);
                pnlRight.Controls.Add(cardInBody, 0, 1);
                pnlRight.Controls.Add(cardAmenities, 0, 2);
                pnlRight.Controls.Add(cardTips, 0, 3);

                table.Controls.Add(pnlLeft, 0, 0);
                table.Controls.Add(pnlRight, 1, 0);
            }

            return table;
        }

        private RoundedPanel CreateUpgradePTCard(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = Color.FromArgb(250, 245, 255), // Violet 50
                BorderColor = Color.FromArgb(221, 214, 254), // Violet 200
                Padding = new Padding((int)(14 * scale), (int)(10 * scale), (int)(14 * scale), (int)(10 * scale))
            };

            int headerH = (int)Math.Round(28f * scale);
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = headerH };
            Label lblTitle = new Label
            {
                Text = "🎯 ĐĂNG KÝ GÓI PT 1-1",
                Font = UITheme.FontSection,
                ForeColor = Color.FromArgb(109, 40, 217), // Violet 700
                AutoSize = true,
                Location = new Point(0, (int)(2 * scale)),
                UseMnemonic = false
            };

            Label lblBadge = new Label
            {
                Text = "ƯU ĐÃI NÂNG CẤP",
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(244, 63, 94), // Rose 500
                Padding = new Padding((int)(6 * scale), (int)(2 * scale), (int)(6 * scale), (int)(2 * scale)),
                AutoSize = true,
                UseMnemonic = false
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblBadge);
            pnlHeader.Resize += (s, e) =>
            {
                lblBadge.Location = new Point(Math.Max(120, pnlHeader.Width - lblBadge.Width - (int)(4 * scale)), (int)(2 * scale));
            };

            Panel pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, (int)(4 * scale), 0, 0)
            };

            Label lblHeadline = new Label
            {
                Text = "Nâng tầm vóc dáng cùng HLV Cá Nhân 1-1!",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(76, 29, 149),
                Dock = DockStyle.Top,
                Height = (int)(22 * scale),
                UseMnemonic = false
            };

            Label lblDesc = new Label
            {
                Text = "Gói Classic của bạn là gói tập tự do. Đăng ký nâng cấp lên gói Silver/Gold/VIP để được:\n• HLV kèm 1-1 suốt lộ trình luyện tập\n• Lên giáo án riêng & Chế độ dinh dưỡng chuẩn\n• Cam kết hiệu quả tăng cơ, giảm mỡ rõ rệt",
                Font = UITheme.FontSmall,
                ForeColor = Color.FromArgb(91, 33, 182),
                Dock = DockStyle.Fill,
                UseMnemonic = false
            };

            Button btnUpgrade = new Button
            {
                Text = "🚀 Xem các gói PT & Nhận tư vấn",
                Font = UITheme.FontBold,
                Dock = DockStyle.Bottom,
                Height = (int)(34 * scale),
                BackColor = Color.FromArgb(124, 58, 237), // Violet 600
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnUpgrade.FlatAppearance.BorderSize = 0;
            btnUpgrade.Click += (s, e) => _navigateCallback?.Invoke("Upgrade");

            pnlBody.Controls.Add(lblDesc);
            pnlBody.Controls.Add(btnUpgrade);
            pnlBody.Controls.Add(lblHeadline);

            card.Controls.Add(pnlBody);
            card.Controls.Add(pnlHeader);

            return card;
        }

        private RoundedPanel CreateWorkoutAttendanceCalendar(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding((int)(14 * scale), (int)(10 * scale), (int)(14 * scale), (int)(10 * scale))
            };

            // 1. Header Bar: Tiêu đề + Thống kê chuyên cần
            int headerH = (int)Math.Round(48f * scale);
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = headerH };

            Label lblTitle = new Label
            {
                Text = "📅 SƠ ĐỒ ĐIỂM DANH TẬP LUYỆN TỰ DO (" + DateTime.Today.ToString("MM/yyyy") + ")",
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
                ForeColor = UITheme.TextDark,
                Location = new Point(0, (int)(2 * scale)),
                AutoSize = true,
                UseMnemonic = false
            };

            Label lblSub = new Label
            {
                Text = "Theo dõi chuyên cần tự động khi quẹt thẻ ra vào phòng tập của gói Classic",
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextMuted,
                Location = new Point(0, (int)(24 * scale)),
                AutoSize = true,
                UseMnemonic = false
            };

            FlowLayoutPanel pnlBadges = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                BackColor = Color.Transparent
            };

            DateTime now = DateTime.Now;
            var monthlyCheckIns = MemberService.Instance.GetMonthlyCheckIns(now.Year, now.Month);
            var checkInStats = MemberService.Instance.GetMonthlyCheckInStats(now.Year, now.Month);

            Label badge1 = new Label
            {
                Text = $"✓ Đã tập: {checkInStats.DistinctDaysCount} ngày",
                Font = UITheme.FontSmallBold,
                BackColor = Color.FromArgb(209, 250, 229),
                ForeColor = Color.FromArgb(6, 95, 70),
                Padding = new Padding((int)(8 * scale), (int)(4 * scale), (int)(8 * scale), (int)(4 * scale)),
                AutoSize = true,
                Margin = new Padding((int)(4 * scale), 0, 0, 0),
                UseMnemonic = false
            };

            Label badge2 = new Label
            {
                Text = $"🔥 Chuỗi: {checkInStats.StreakDays} ngày",
                Font = UITheme.FontSmallBold,
                BackColor = Color.FromArgb(254, 243, 199),
                ForeColor = Color.FromArgb(146, 64, 14),
                Padding = new Padding((int)(8 * scale), (int)(4 * scale), (int)(8 * scale), (int)(4 * scale)),
                AutoSize = true,
                Margin = new Padding((int)(4 * scale), 0, 0, 0),
                UseMnemonic = false
            };

            Label badge3 = new Label
            {
                Text = "🎯 Mục tiêu: 20 ngày",
                Font = UITheme.FontSmallBold,
                BackColor = Color.FromArgb(238, 242, 255),
                ForeColor = Color.FromArgb(67, 56, 202),
                Padding = new Padding((int)(8 * scale), (int)(4 * scale), (int)(8 * scale), (int)(4 * scale)),
                AutoSize = true,
                Margin = new Padding((int)(4 * scale), 0, 0, 0),
                UseMnemonic = false
            };

            pnlBadges.Controls.Add(badge1);
            pnlBadges.Controls.Add(badge2);
            pnlBadges.Controls.Add(badge3);

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);
            pnlHeader.Controls.Add(pnlBadges);

            pnlHeader.Resize += (s, e) =>
            {
                pnlBadges.Location = new Point(Math.Max(150, pnlHeader.Width - pnlBadges.Width), (int)(6 * scale));
            };

            // 2. Bottom Recent History & Free Access Note (Dock Bottom)
            int bottomH = (int)Math.Round(80f * scale);
            Panel pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = bottomH,
                Padding = new Padding(0, (int)(6 * scale), 0, 0)
            };

            Label lblLogTitle = new Label
            {
                Text = "🕒 LỊCH SỬ VÀO CỬA TỰ DO GẦN NHẤT (QUẸT THẺ TỰ ĐỘNG):",
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.TextMuted,
                Dock = DockStyle.Top,
                Height = (int)(18 * scale),
                UseMnemonic = false
            };

            TableLayoutPanel tblLogItems = new TableLayoutPanel
            {
                ColumnCount = 3,
                RowCount = 1,
                Dock = DockStyle.Top,
                Height = (int)(38 * scale),
                Padding = new Padding(0)
            };
            tblLogItems.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tblLogItems.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tblLogItems.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34f));

            var recentCheckIns = MemberService.Instance.GetRecentCheckIns().Take(3).ToList();
            if (recentCheckIns.Count > 0)
            {
                for (int i = 0; i < recentCheckIns.Count; i++)
                {
                    var ci = recentCheckIns[i];
                    string timeStr = ci.CheckInTime.ToString("dd/MM HH:mm");
                    string locStr = $"Cổng {ci.Branch?.BranchName ?? "Chi nhánh"} (Vào cửa)";
                    tblLogItems.Controls.Add(CreateLogItemWidget(timeStr, locStr, scale), i, 0);
                }
            }
            else
            {
                Label lblNoLogs = new Label
                {
                    Text = "Chưa có lượt quẹt thẻ vào cửa gần đây.",
                    Font = UITheme.FontSmall,
                    ForeColor = UITheme.TextMuted,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    UseMnemonic = false
                };
                tblLogItems.Controls.Add(lblNoLogs, 0, 0);
                tblLogItems.SetColumnSpan(lblNoLogs, 3);
            }

            Label lblNote = new Label
            {
                Text = "💡 Gói Classic không giới hạn số lượt quẹt thẻ ra vào phòng tập trong khung giờ hoạt động 05:30 - 22:00.",
                Font = new Font("Segoe UI", 7.8f, FontStyle.Italic),
                ForeColor = UITheme.TextMuted,
                Dock = DockStyle.Bottom,
                Height = (int)(18 * scale),
                UseMnemonic = false
            };

            pnlBottom.Controls.Add(tblLogItems);
            pnlBottom.Controls.Add(lblNote);
            pnlBottom.Controls.Add(lblLogTitle);

            // 3. Calendar Grid Panel (Dock Fill)
            Panel pnlCalendarContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, (int)(4 * scale), 0, (int)(4 * scale))
            };

            TableLayoutPanel tblWeekHeader = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = (int)(26 * scale),
                ColumnCount = 7,
                RowCount = 1
            };
            for (int i = 0; i < 7; i++)
                tblWeekHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 7f));

            string[] dayHeaders = { "THỨ 2", "THỨ 3", "THỨ 4", "THỨ 5", "THỨ 6", "THỨ 7", "CHỦ NHẬT" };
            for (int i = 0; i < 7; i++)
            {
                Label lblDayH = new Label
                {
                    Text = dayHeaders[i],
                    Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                    ForeColor = (i == 6) ? Color.FromArgb(239, 68, 68) : Color.FromArgb(71, 85, 105),
                    BackColor = Color.FromArgb(241, 245, 249),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding((int)(2 * scale)),
                    UseMnemonic = false
                };
                tblWeekHeader.Controls.Add(lblDayH, i, 0);
            }

            int daysInCurrentMonth = DateTime.DaysInMonth(now.Year, now.Month);
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            int startOffset = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
            int totalCells = startOffset + daysInCurrentMonth;
            int numRows = (int)Math.Ceiling(totalCells / 7.0);

            TableLayoutPanel tblDays = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 7,
                RowCount = numRows
            };
            for (int i = 0; i < 7; i++)
                tblDays.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 7f));
            for (int i = 0; i < numRows; i++)
                tblDays.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / numRows));

            HashSet<int> workoutDays = monthlyCheckIns.Select(c => c.CheckInTime.Day).ToHashSet();

            int dayCounter = 1;
            for (int r = 0; r < numRows; r++)
            {
                for (int c = 0; c < 7; c++)
                {
                    int cellIndex = r * 7 + c;
                    if (cellIndex < startOffset || dayCounter > daysInCurrentMonth)
                    {
                        Panel emptyCell = new Panel
                        {
                            Dock = DockStyle.Fill,
                            BackColor = Color.FromArgb(248, 250, 252),
                            Margin = new Padding((int)(2 * scale))
                        };
                        tblDays.Controls.Add(emptyCell, c, r);
                    }
                    else
                    {
                        int currentDay = dayCounter;
                        bool isTrained = workoutDays.Contains(currentDay);
                        bool isToday = (currentDay == now.Day);

                        Panel cell = new Panel
                        {
                            Dock = DockStyle.Fill,
                            Margin = new Padding((int)(2 * scale)),
                            BackColor = isTrained ? Color.FromArgb(236, 253, 245) : (isToday ? Color.FromArgb(238, 242, 255) : Color.White)
                        };

                        cell.Paint += (s, e) =>
                        {
                            var g = e.Graphics;
                            Color bColor = isToday ? Color.FromArgb(99, 102, 241) : (isTrained ? Color.FromArgb(52, 211, 153) : Color.FromArgb(226, 232, 240));
                            int bWidth = isToday ? 2 : 1;
                            using var pen = new Pen(bColor, bWidth);
                            g.DrawRectangle(pen, 0, 0, cell.Width - 1, cell.Height - 1);
                        };

                        Label lblDate = new Label
                        {
                            Text = currentDay.ToString(),
                            Font = new Font("Segoe UI", 9f, (isTrained || isToday) ? FontStyle.Bold : FontStyle.Regular),
                            ForeColor = isToday ? Color.FromArgb(79, 70, 229) : (isTrained ? Color.FromArgb(6, 95, 70) : Color.FromArgb(51, 65, 85)),
                            Location = new Point((int)(4 * scale), (int)(3 * scale)),
                            AutoSize = true,
                            UseMnemonic = false
                        };
                        cell.Controls.Add(lblDate);

                        if (isTrained)
                        {
                            Label lblBadge = new Label
                            {
                                Text = "✓ Đã tập",
                                Font = new Font("Segoe UI", 7.2f, FontStyle.Bold),
                                ForeColor = Color.FromArgb(5, 150, 105),
                                BackColor = Color.FromArgb(209, 250, 229),
                                Padding = new Padding((int)(3 * scale), (int)(1 * scale), (int)(3 * scale), (int)(1 * scale)),
                                AutoSize = true,
                                UseMnemonic = false
                            };
                            cell.Controls.Add(lblBadge);
                            cell.Resize += (s, e) =>
                            {
                                lblBadge.Location = new Point(Math.Max(2, cell.Width - lblBadge.Width - (int)(4 * scale)), Math.Max(2, cell.Height - lblBadge.Height - (int)(4 * scale)));
                            };
                        }
                        else if (isToday)
                        {
                            Label lblToday = new Label
                            {
                                Text = "Hôm nay",
                                Font = new Font("Segoe UI", 7f, FontStyle.Bold),
                                ForeColor = Color.FromArgb(79, 70, 229),
                                Location = new Point((int)(4 * scale), (int)(20 * scale)),
                                AutoSize = true,
                                UseMnemonic = false
                            };
                            cell.Controls.Add(lblToday);
                        }

                        tblDays.Controls.Add(cell, c, r);
                        dayCounter++;
                    }
                }
            }

            pnlCalendarContainer.Controls.Add(tblDays);
            pnlCalendarContainer.Controls.Add(tblWeekHeader);

            card.Controls.Add(pnlCalendarContainer);
            card.Controls.Add(pnlBottom);
            card.Controls.Add(pnlHeader);

            return card;
        }

        private Panel CreateLogItemWidget(string timeStr, string locationStr, float scale)
        {
            Panel pnl = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding((int)(3 * scale)),
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding((int)(8 * scale), (int)(4 * scale), (int)(8 * scale), (int)(4 * scale))
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Color.FromArgb(226, 232, 240));
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            Label lblT = new Label
            {
                Text = "🟢 " + timeStr,
                Font = UITheme.FontSmallBold,
                ForeColor = Color.FromArgb(5, 150, 105),
                Dock = DockStyle.Top,
                Height = (int)(15 * scale),
                UseMnemonic = false
            };
            Label lblL = new Label
            {
                Text = locationStr,
                Font = new Font("Segoe UI", 7.5f),
                ForeColor = UITheme.TextMuted,
                Dock = DockStyle.Bottom,
                Height = (int)(14 * scale),
                UseMnemonic = false
            };
            pnl.Controls.Add(lblT);
            pnl.Controls.Add(lblL);
            return pnl;
        }

        private RoundedPanel CreateUpcomingScheduleCard(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding((int)(14 * scale), (int)(10 * scale), (int)(14 * scale), (int)(10 * scale))
            };

            int headerH = (int)Math.Round(34f * scale);
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = headerH };
            Label lblTitle = new Label
            {
                Text = "📅 LỊCH TẬP SẮP TỚI TRONG TUẦN",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(0, (int)(4 * scale)),
                UseMnemonic = false
            };
            Button btnViewAll = new Button
            {
                Text = "+ Đặt lịch mới",
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.Primary,
                BackColor = UITheme.PrimaryLight,
                FlatStyle = FlatStyle.Flat,
                Size = new Size((int)(115 * scale), (int)(28 * scale)),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnViewAll.FlatAppearance.BorderSize = 0;
            btnViewAll.Click += (s, e) => _navigateCallback?.Invoke("Schedule");

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnViewAll);
            pnlHeader.Resize += (s, e) =>
            {
                btnViewAll.Location = new Point(Math.Max(150, pnlHeader.Width - (int)(120 * scale)), (int)(2 * scale));
            };

            DataGridView dgv = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgv);
            dgv.RowTemplate.Height = (int)Math.Round(36f * scale);
            dgv.ColumnHeadersHeight = (int)Math.Round(34f * scale);

            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ngày tập", Name = "colDate", FillWeight = 18 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ca tập", Name = "colTime", FillWeight = 18 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Bộ môn", Name = "colSubject", FillWeight = 22 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Huấn luyện viên", Name = "colPT", FillWeight = 24 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Chi nhánh", Name = "colBranch", FillWeight = 22 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Trạng thái", Name = "colStatus", FillWeight = 18 });

            try
            {
                var upcoming = MemberService.Instance.GetUpcomingSchedules();
                if (upcoming.Count > 0)
                {
                    foreach (var s in upcoming.Take(6))
                    {
                        string dateStr = s.TrainingDate.Date == DateTime.Today ? "Hôm nay" : s.TrainingDate.ToString("ddd, dd/MM");
                        string timeStr = $"{s.StartTime:hh\\:mm} - {s.EndTime:hh\\:mm}";
                        string ptName = s.PT?.FullName ?? "HLV Titan";
                        string branchName = s.Branch?.BranchName ?? (s.PT?.Branch?.BranchName ?? "Chi nhánh");
                        string trainingType = !string.IsNullOrEmpty(s.TrainingType) ? s.TrainingType : "Gym Thể hình";
                        dgv.Rows.Add(dateStr, timeStr, trainingType, ptName, branchName, s.Status ?? "Đã xác nhận");
                    }
                }
                else
                {
                    dgv.Rows.Add("Chưa có lịch", "--", "Chưa có lịch tập trong tuần", "Nhấn '+ Đặt lịch mới' để xếp lịch", "--", "--");
                }
            }
            catch
            {
                dgv.Rows.Add("Lỗi dữ liệu", "--", "Không thể tải lịch tập từ CSDL", "--", "--", "--");
            }

            card.Controls.Add(dgv);
            card.Controls.Add(pnlHeader);

            return card;
        }

        private RoundedPanel CreateRecentCheckInsCard(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding((int)(14 * scale), (int)(10 * scale), (int)(14 * scale), (int)(10 * scale))
            };

            int headerH = (int)Math.Round(34f * scale);
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = headerH };
            Label lblTitle = new Label
            {
                Text = "📸 NHẬT KÝ ĐIỂM DANH CHECK-IN GẦN ĐÂY",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(0, (int)(4 * scale)),
                UseMnemonic = false
            };
            Button btnAllHistory = new Button
            {
                Text = "Xem tất cả >",
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.Primary,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size((int)(100 * scale), (int)(28 * scale)),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnAllHistory.FlatAppearance.BorderSize = 0;
            btnAllHistory.Click += (s, e) => _navigateCallback?.Invoke("CheckIn");

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnAllHistory);
            pnlHeader.Resize += (s, e) =>
            {
                btnAllHistory.Location = new Point(Math.Max(150, pnlHeader.Width - (int)(105 * scale)), (int)(2 * scale));
            };

            DataGridView dgv = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgv);
            dgv.RowTemplate.Height = (int)Math.Round(36f * scale);
            dgv.ColumnHeadersHeight = (int)Math.Round(34f * scale);

            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã", Name = "colCode", FillWeight = 14 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Thời gian check-in", Name = "colTime", FillWeight = 26 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "HLV xác nhận", Name = "colPT", FillWeight = 24 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ghi chú buổi tập", Name = "colNotes", FillWeight = 30 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Trạng thái", Name = "colStatus", FillWeight = 18 });

            try
            {
                var checkIns = MemberService.Instance.GetRecentCheckIns();
                if (checkIns.Count > 0)
                {
                    foreach (var c in checkIns.Take(5))
                    {
                        string code = $"#CK-{c.CheckInID:D3}";
                        string timeStr = c.CheckInTime.ToString("dd/MM/yyyy HH:mm");
                        string ptName = c.PT?.FullName ?? "Quầy lễ tân";
                        string status = c.Status == "Approved" ? "Đã duyệt" : (c.Status == "Pending" ? "Chờ duyệt" : c.Status);
                        string notes = !string.IsNullOrEmpty(c.Note) ? c.Note : "Tập luyện theo giáo án";
                        dgv.Rows.Add(code, timeStr, ptName, notes, status);
                    }
                }
                else
                {
                    dgv.Rows.Add("#--", "--", "Chưa có lượt điểm danh", "Hãy điểm danh khi bắt đầu buổi tập", "--");
                }
            }
            catch
            {
                dgv.Rows.Add("#--", "--", "Lỗi nạp dữ liệu", "Không thể kết nối CSDL", "--");
            }

            card.Controls.Add(dgv);
            card.Controls.Add(pnlHeader);

            return card;
        }

        private void ShowPTListDialog()
        {
            try
            {
                using var dlg = new frmMemberPTList(_navigateCallback);
                dlg.ShowDialog(this.FindForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở danh sách Huấn luyện viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private RoundedPanel CreateTrainerCard(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding((int)(14 * scale), (int)(10 * scale), (int)(14 * scale), (int)(10 * scale))
            };

            int titleH = (int)Math.Round(26f * scale);
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = titleH };
            Label lblTitle = new Label
            {
                Text = "🏋️ HUẤN LUYỆN VIÊN (PT)",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(0, 0),
                UseMnemonic = false
            };

            Label lblBadge = new Label
            {
                Text = "ĐỘI NGŨ HLV ⭐",
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(109, 40, 217), // Violet 700
                Padding = new Padding((int)(6 * scale), (int)(2 * scale), (int)(6 * scale), (int)(2 * scale)),
                AutoSize = true,
                UseMnemonic = false
            };
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblBadge);
            pnlHeader.Resize += (s, e) =>
            {
                lblBadge.Location = new Point(Math.Max(120, pnlHeader.Width - lblBadge.Width - (int)(4 * scale)), (int)(2 * scale));
            };

            Panel pnlPT = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, (int)(4 * scale), 0, 0) };

            int avatarSize = (int)Math.Round(46f * scale);
            Panel pnlAvatar = new Panel
            {
                Size = new Size(avatarSize, avatarSize),
                BackColor = UITheme.PrimaryLight,
                Dock = DockStyle.Left
            };
            Label lblAvatar = new Label
            {
                Text = "👨‍🏫",
                Font = new Font("Segoe UI Emoji", 20f),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                UseMnemonic = false
            };
            pnlAvatar.Controls.Add(lblAvatar);

            FlowLayoutPanel flowInfo = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(avatarSize + (int)(12 * scale), 0, 0, 0),
                BackColor = Color.Transparent
            };

            Label lblName = new Label
            {
                Text = "Đội ngũ Huấn luyện viên 1-1",
                Font = UITheme.FontBold,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, (int)(2 * scale)),
                UseMnemonic = false
            };
            Label lblRole = new Label
            {
                Text = "Chuyên gia Thể hình & Dinh dưỡng quốc tế",
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextMuted,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, (int)(2 * scale)),
                UseMnemonic = false
            };
            Label lblPhone = new Label
            {
                Text = "⭐ 100% Đạt chứng chỉ  |  Đánh giá 5.0 (Tuyệt vời)",
                Font = UITheme.FontSmallBold,
                ForeColor = Color.FromArgb(217, 119, 6), // Amber
                AutoSize = true,
                Margin = new Padding(0),
                UseMnemonic = false
            };

            flowInfo.Controls.Add(lblName);
            flowInfo.Controls.Add(lblRole);
            flowInfo.Controls.Add(lblPhone);

            pnlPT.Controls.Add(flowInfo);
            pnlPT.Controls.Add(pnlAvatar);

            // Nút "Danh sách PT" bấm vào hiện ra thông tin các PT
            Button btnPTList = new Button
            {
                Text = "📋 Danh sách PT",
                Font = UITheme.FontBold,
                Dock = DockStyle.Bottom,
                Height = (int)(36 * scale),
                BackColor = Color.FromArgb(109, 40, 217), // Violet 700
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnPTList.FlatAppearance.BorderSize = 0;
            btnPTList.Click += (s, e) => ShowPTListDialog();

            card.Controls.Add(pnlPT);
            card.Controls.Add(btnPTList);
            card.Controls.Add(pnlHeader);

            card.Cursor = Cursors.Hand;
            card.Click += (s, e) => ShowPTListDialog();
            lblTitle.Click += (s, e) => ShowPTListDialog();
            pnlPT.Click += (s, e) => ShowPTListDialog();

            return card;
        }

        private RoundedPanel CreateInBodySnapshotCard(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding((int)(14 * scale), (int)(10 * scale), (int)(14 * scale), (int)(10 * scale))
            };

            int headerH = (int)Math.Round(26f * scale);
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = headerH };
            Label lblTitle = new Label
            {
                Text = "📊 TIẾN ĐỘ VÓC DÁNG (INBODY)",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(0, (int)(2 * scale)),
                UseMnemonic = false
            };
            Button btnMore = new Button
            {
                Text = "Chi tiết >",
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.Primary,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size((int)(72 * scale), (int)(22 * scale)),
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnMore.FlatAppearance.BorderSize = 0;
            btnMore.Click += (s, e) => _navigateCallback?.Invoke("Progress");

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnMore);
            pnlHeader.Resize += (s, e) =>
            {
                btnMore.Location = new Point(Math.Max(120, pnlHeader.Width - (int)(76 * scale)), 0);
            };

            TableLayoutPanel grid = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 2,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, (int)(1 * scale), 0, 0)
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

            var prog = MemberService.Instance.GetLatestProgress();
            string hStr = prog != null ? $"{prog.Height:0.#} cm" : "-- cm";
            string wStr = prog != null ? $"{prog.Weight:0.0} kg" : "-- kg";
            string fStr = prog != null ? $"{prog.BodyFat:0.0} %" : "-- %";
            string mStr = prog != null ? $"{prog.MuscleMass:0.0} kg" : "-- kg";

            grid.Controls.Add(CreateMiniMetric("Chiều cao", hStr, prog != null ? $"BMI: {prog.BMI:0.0}" : "Chưa đo", scale), 0, 0);
            grid.Controls.Add(CreateMiniMetric("Cân nặng", wStr, prog != null ? "Gần nhất" : "Chưa đo", scale), 1, 0);
            grid.Controls.Add(CreateMiniMetric("Tỷ lệ mỡ (Fat)", fStr, prog != null ? "Chỉ số InBody" : "Chưa đo", scale), 0, 1);
            grid.Controls.Add(CreateMiniMetric("Khối lượng cơ", mStr, prog != null ? "Chỉ số InBody" : "Chưa đo", scale), 1, 1);

            card.Controls.Add(grid);
            card.Controls.Add(pnlHeader);

            return card;
        }

        private FlowLayoutPanel CreateMiniMetric(string label, string val, string sub, float scale)
        {
            FlowLayoutPanel pnl = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Margin = new Padding((int)(2 * scale)),
                BackColor = Color.Transparent
            };
            Label lbl = new Label { Text = label, Font = new Font("Segoe UI", 7.8f), ForeColor = UITheme.TextMuted, AutoSize = true, Margin = new Padding(0), UseMnemonic = false };
            Label v = new Label { Text = val, Font = new Font("Segoe UI", 11f, FontStyle.Bold), ForeColor = UITheme.TextDark, AutoSize = true, Margin = new Padding(0), UseMnemonic = false };
            Label s = new Label { Text = sub, Font = new Font("Segoe UI", 7.5f), ForeColor = UITheme.Success, AutoSize = true, Margin = new Padding(0), UseMnemonic = false };
            pnl.Controls.Add(lbl);
            pnl.Controls.Add(v);
            pnl.Controls.Add(s);
            return pnl;
        }

        private RoundedPanel CreateClubAmenitiesCard(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding((int)(14 * scale), (int)(10 * scale), (int)(14 * scale), (int)(10 * scale))
            };

            int titleH = (int)Math.Round(26f * scale);
            Label lblTitle = new Label
            {
                Text = "🏢 TIỆN ÍCH & THÔNG BÁO CLB",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = titleH,
                UseMnemonic = false
            };

            FlowLayoutPanel flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = new Padding(0, (int)(4 * scale), 0, 0)
            };

            Label item1 = new Label
            {
                Text = "⏰ Giờ mở cửa: 05:30 - 22:00 hàng ngày (kể cả Lễ, Tết)",
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, (int)(3 * scale)),
                UseMnemonic = false
            };
            Label item2 = new Label
            {
                Text = "🚿 Phòng xông hơi & Locker thông minh: Đang hoạt động",
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextMuted,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, (int)(3 * scale)),
                UseMnemonic = false
            };
            Label item3 = new Label
            {
                Text = "🥤 Nước ion kiềm & Khăn tập: Miễn phí tại quầy lễ tân",
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextMuted,
                AutoSize = true,
                Margin = new Padding(0),
                UseMnemonic = false
            };

            flow.Controls.Add(item1);
            flow.Controls.Add(item2);
            flow.Controls.Add(item3);

            card.Controls.Add(flow);
            card.Controls.Add(lblTitle);
            return card;
        }

        private RoundedPanel CreateTipsCard(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = Color.FromArgb(254, 243, 199),  // Amber Light
                BorderColor = Color.FromArgb(253, 230, 138),
                Padding = new Padding((int)(12 * scale), (int)(8 * scale), (int)(12 * scale), (int)(8 * scale))
            };

            int titleH = (int)Math.Round(22f * scale);
            Label lblTitle = new Label
            {
                Text = "💡 LỜI KHUYÊN SỨC KHỎE DÀNH CHO BẠN",
                Font = UITheme.FontSmallBold,
                ForeColor = Color.FromArgb(180, 83, 9),
                Dock = DockStyle.Top,
                Height = titleH,
                UseMnemonic = false
            };

            Label lblTip = new Label
            {
                Text = "Uống đủ 2 - 2.5 lít nước mỗi ngày và ngủ đủ 7-8 tiếng để cơ bắp phục hồi tối đa sau các buổi tập cường độ cao. Đừng quên nạp 20-30g protein sau buổi tập nhé!",
                Font = UITheme.FontSmall,
                ForeColor = Color.FromArgb(146, 64, 14),
                Dock = DockStyle.Fill,
                UseMnemonic = false
            };

            card.Controls.Add(lblTip);
            card.Controls.Add(lblTitle);

            return card;
        }
    }
}
