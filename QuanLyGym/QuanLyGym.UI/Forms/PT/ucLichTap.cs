using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.PT
{
    public partial class ucLichTap : UserControl
    {
        public class Session1On1
        {
            public string MemberInitials { get; set; } = "";
            public string MemberName { get; set; } = "";
            public string ProgressText { get; set; } = "";
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public string TimeText => $"{StartTime:hh\\:mm} – {EndTime:hh\\:mm}";
            public string DayName { get; set; } = "";
            public string Location { get; set; } = "";
            public string[] Exercises { get; set; } = Array.Empty<string>();
            public bool IsToday { get; set; }
            public bool IsUrgent { get; set; }
            public string StatusBadge { get; set; } = "Sắp tới giờ";
        }

        private List<Session1On1> _allTodaySessions = new List<Session1On1>();
        private Session1On1? _selectedSession;

        public ucLichTap()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = PTUITheme.BgDark;

            InitSampleData();
            SetupScheduleGrid();
            SetupRightPanel();
        }

        private void InitSampleData()
        {
            _allTodaySessions = new List<Session1On1>
            {
                new Session1On1
                {
                    MemberInitials = "DA",
                    MemberName = "Nguyễn Đức An",
                    ProgressText = "Tiến độ: 18 / 36 Buổi",
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(8, 30, 0),
                    DayName = "Thứ 2",
                    Location = "Zone A - Khu Tạ Tự Do (Free-weight)",
                    Exercises = new string[]
                    {
                        "1. Barbell Flat Bench Press: 4 sets x 8–10 reps (65kg)",
                        "2. Incline DB Press: 3 sets x 12 reps",
                        "3. Cable Tricep Pushdown: 4 sets x 15 reps"
                    },
                    IsToday = true,
                    IsUrgent = true,
                    StatusBadge = "SẮP TỚI GIỜ"
                },
                new Session1On1
                {
                    MemberInitials = "ML",
                    MemberName = "Phạm Mai Linh",
                    ProgressText = "Tiến độ: 15 / 30 Buổi",
                    StartTime = new TimeSpan(17, 0, 0),
                    EndTime = new TimeSpan(18, 30, 0),
                    DayName = "Thứ 2",
                    Location = "Zone B - Cardio & Giảm Mỡ Siết Eo",
                    Exercises = new string[]
                    {
                        "1. Romanian Deadlift: 4 sets x 12 reps (45kg)",
                        "2. Dumbbell Walking Lunges: 3 sets x 20 bước",
                        "3. HIIT Cardio Máy Chạy: 20 phút chạy ngắt quãng"
                    },
                    IsToday = true,
                    IsUrgent = false,
                    StatusBadge = "CA CHIỀU"
                }
            };

            // Ưu tiên theo thời gian: Sắp tới giờ tập lên đầu tiên
            _allTodaySessions = _allTodaySessions.OrderBy(s => s.StartTime).ToList();
            _selectedSession = _allTodaySessions.FirstOrDefault();
        }

        private void SetupScheduleGrid()
        {
            pnlScheduleGrid.Paint += PnlScheduleGrid_Paint;
            pnlScheduleGrid.Resize += (s, e) => pnlScheduleGrid.Invalidate();
        }

        private void PnlScheduleGrid_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = pnlScheduleGrid.ClientRectangle;
            PTUITheme.DrawCard(g, new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1), radius: 12);

            string[] headers = new string[] { "Khung Giờ", "Thứ 2 (05)", "Thứ 3 (06)", "Thứ 4 (07)", "Thứ 5 (08)", "Thứ 6 (09)", "Thứ 7 (10)" };
            int colCount = headers.Length;
            int timeColWidth = 115;
            int dayColWidth = Math.Max(70, (rect.Width - timeColWidth - 32) / (colCount - 1));

            int headerY = 14;
            int headerHeight = 36;
            int tableLeft = 16;
            int tableRight = tableLeft + timeColWidth + (colCount - 1) * dayColWidth;
            int tableBottom = rect.Height - 16;

            // Header background row
            Rectangle headerBg = new Rectangle(tableLeft, headerY, tableRight - tableLeft, headerHeight);
            using (SolidBrush bHdr = new SolidBrush(Color.FromArgb(20, 24, 33)))
            {
                PTUITheme.FillRoundedRectangle(g, bHdr, headerBg, 6);
            }

            // Draw Column Headers
            for (int i = 0; i < colCount; i++)
            {
                int colX = (i == 0) ? tableLeft : tableLeft + timeColWidth + (i - 1) * dayColWidth;
                int w = (i == 0) ? timeColWidth : dayColWidth;
                Rectangle headerCell = new Rectangle(colX, headerY, w, headerHeight);

                if (i == 1) // Thứ 2 (Hôm nay - cột được highlight)
                {
                    Rectangle todayBadge = new Rectangle(colX + 3, headerY + 4, w - 6, headerHeight - 8);
                    PTUITheme.DrawBadge(g, headers[i], todayBadge, PTUITheme.LimeDark, PTUITheme.PrimaryLime, PTUITheme.FontSmallBold, 6);
                }
                else
                {
                    TextRenderer.DrawText(g, headers[i], (i == 0) ? PTUITheme.FontSmallBold : PTUITheme.FontSmall, headerCell,
                        (i == 0) ? PTUITheme.TextWhite : PTUITheme.TextMuted,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                }

                // Vertical column divider lines
                if (i > 0)
                {
                    using (Pen pDiv = new Pen(Color.FromArgb(32, 37, 50), 1))
                    {
                        g.DrawLine(pDiv, colX, headerY, colX, tableBottom);
                    }
                }
            }

            // Row definitions
            string[] timeSlots = new string[] { "07:00 – 08:30", "09:00 – 10:30", "14:00 – 15:30", "17:00 – 18:30" };
            int startRowY = headerY + headerHeight;
            int rowHeight = (tableBottom - startRowY) / timeSlots.Length;

            for (int r = 0; r < timeSlots.Length; r++)
            {
                int rowY = startRowY + r * rowHeight;

                // Horizontal row grid line
                using (Pen borderPen = new Pen(Color.FromArgb(32, 37, 50), 1))
                {
                    g.DrawLine(borderPen, tableLeft, rowY, tableRight, rowY);
                }

                // Time label cell
                Rectangle timeCell = new Rectangle(tableLeft + 6, rowY, timeColWidth - 12, rowHeight);
                TextRenderer.DrawText(g, timeSlots[r], PTUITheme.FontSmall, timeCell, PTUITheme.TextMuted,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                // Schedule Cells
                if (r == 0)
                {
                    DrawScheduleCard(g, GetCellRect(1, rowY, tableLeft, timeColWidth, dayColWidth, rowHeight), "HV: Đức An", "07:00 - 08:30", true);
                    DrawScheduleCard(g, GetCellRect(5, rowY, tableLeft, timeColWidth, dayColWidth, rowHeight), "HV: Đức An", "07:00 - 08:30", false);
                }
                else if (r == 1)
                {
                    DrawScheduleCard(g, GetCellRect(2, rowY, tableLeft, timeColWidth, dayColWidth, rowHeight), "HV: Hoàng Nam", "09:00 - 10:30", false);
                    DrawScheduleCard(g, GetCellRect(4, rowY, tableLeft, timeColWidth, dayColWidth, rowHeight), "HV: Hoàng Nam", "09:00 - 10:30", false);
                }
                else if (r == 2)
                {
                    DrawScheduleCard(g, GetCellRect(3, rowY, tableLeft, timeColWidth, dayColWidth, rowHeight), "HV: Bảo Trâm", "14:00 - 15:30", false);
                }
                else if (r == 3)
                {
                    DrawScheduleCard(g, GetCellRect(1, rowY, tableLeft, timeColWidth, dayColWidth, rowHeight), "HV: Mai Linh", "17:00 - 18:30", false);
                    DrawScheduleCard(g, GetCellRect(5, rowY, tableLeft, timeColWidth, dayColWidth, rowHeight), "HV: Mai Linh", "17:00 - 18:30", false);
                }
            }

            // Bottom border
            using (Pen borderPen = new Pen(Color.FromArgb(32, 37, 50), 1))
            {
                g.DrawLine(borderPen, tableLeft, tableBottom, tableRight, tableBottom);
            }
        }

        private Rectangle GetCellRect(int dayIndex, int rowY, int tableLeft, int timeColWidth, int dayColWidth, int rowHeight)
        {
            int x = tableLeft + timeColWidth + (dayIndex - 1) * dayColWidth + 4;
            int w = dayColWidth - 8;
            int y = rowY + 6;
            int h = rowHeight - 12;
            return new Rectangle(x, y, w, h);
        }

        private void DrawScheduleCard(Graphics g, Rectangle rect, string memberName, string timeStr, bool isHighlighted)
        {
            Color bg = isHighlighted ? Color.FromArgb(42, 54, 26) : Color.FromArgb(28, 35, 26);
            Color border = isHighlighted ? PTUITheme.PrimaryLime : Color.FromArgb(65, 85, 38);
            Color fg = PTUITheme.PrimaryLime;

            using (SolidBrush b = new SolidBrush(bg))
            {
                PTUITheme.FillRoundedRectangle(g, b, rect, 6);
            }
            using (Pen p = new Pen(border, isHighlighted ? 1.5f : 1f))
            {
                PTUITheme.DrawRoundedRectangle(g, p, rect, 6);
            }

            // Member name on top, time on bottom
            Rectangle topText = new Rectangle(rect.X + 4, rect.Y + 4, rect.Width - 8, rect.Height / 2);
            TextRenderer.DrawText(g, memberName, PTUITheme.FontSmallBold, topText, fg,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            Rectangle btmText = new Rectangle(rect.X + 4, rect.Y + rect.Height / 2, rect.Width - 8, rect.Height / 2 - 4);
            TextRenderer.DrawText(g, timeStr, PTUITheme.FontSmall, btmText, isHighlighted ? PTUITheme.TextWhite : PTUITheme.TextMuted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }

        private void SetupRightPanel()
        {
            pnlRightDetails.Paint += PnlRightDetails_Paint;
            pnlRightDetails.Resize += (s, e) =>
            {
                RelayoutRightDetailsControls();
                pnlRightDetails.Invalidate();
            };
            UpdateTodaySessionsButtons();
        }

        private void RelayoutRightDetailsControls()
        {
            int w = pnlRightDetails.ClientSize.Width;
            int h = pnlRightDetails.ClientSize.Height;
            int pad = 20;
            int innerW = w - (pad * 2);

            // btnCheckIn
            btnCheckIn.Location = new Point(pad, 335);
            btnCheckIn.Size = new Size(innerW, 40);

            // btnEditSchedule & btnCancelSchedule
            int subBtnW = (innerW - 10) / 2;
            btnEditSchedule.Location = new Point(pad, 382);
            btnEditSchedule.Size = new Size(subBtnW, 36);

            btnCancelSchedule.Location = new Point(pad + subBtnW + 10, 382);
            btnCancelSchedule.Size = new Size(subBtnW, 36);

            // Today sessions label & list
            lblTodaySessionsHeader.Location = new Point(pad, 430);
            pnlTodaySessionButtons.Location = new Point(pad, 452);
            pnlTodaySessionButtons.Size = new Size(innerW, Math.Max(80, h - 465));

            foreach (Control c in pnlTodaySessionButtons.Controls)
            {
                c.Width = pnlTodaySessionButtons.ClientSize.Width - 6;
            }
        }

        private void UpdateTodaySessionsButtons()
        {
            pnlTodaySessionButtons.Controls.Clear();
            pnlTodaySessionButtons.SuspendLayout();

            for (int i = 0; i < _allTodaySessions.Count; i++)
            {
                var s = _allTodaySessions[i];
                bool isSelected = (_selectedSession == s);

                Button btn = new Button
                {
                    Width = pnlTodaySessionButtons.ClientSize.Width - 6,
                    Height = 44,
                    Margin = new Padding(0, 0, 0, 6),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = isSelected ? Color.FromArgb(38, 48, 28) : Color.FromArgb(24, 28, 38),
                    ForeColor = isSelected ? PTUITheme.PrimaryLime : PTUITheme.TextWhite,
                    Font = PTUITheme.FontSmallBold,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Cursor = Cursors.Hand,
                    UseMnemonic = false,
                    Text = $"  {s.TimeText}  •  {s.MemberName}  ({(s.IsUrgent ? "Sắp tới" : "Hôm nay")})"
                };
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = isSelected ? PTUITheme.PrimaryLime : PTUITheme.CardBorder;

                btn.Click += (sObj, eArgs) =>
                {
                    _selectedSession = s;
                    UpdateTodaySessionsButtons();
                    pnlRightDetails.Invalidate();
                };

                pnlTodaySessionButtons.Controls.Add(btn);
            }

            pnlTodaySessionButtons.ResumeLayout(true);
        }

        private void PnlRightDetails_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = pnlRightDetails.ClientRectangle;
            PTUITheme.DrawCard(g, new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1), radius: 12);

            int pad = 20;

            // Title Pill: BUỔI HUẤN LUYỆN 1-1 (Đặt lùi vào an toàn, không bao giờ bị cắt chữ)
            Rectangle pillRect = new Rectangle(pad, 16, 165, 26);
            PTUITheme.DrawBadge(g, "BUỔI HUẤN LUYỆN 1-1", pillRect, PTUITheme.PrimaryLime, Color.Black, PTUITheme.FontSmallBold, 13);

            // Priority notice text
            Rectangle noteRect = new Rectangle(pillRect.Right + 12, 16, rect.Width - pillRect.Right - 20, 26);
            TextRenderer.DrawText(g, "Ưu tiên giờ gần nhất", PTUITheme.FontSmallBold, noteRect, PTUITheme.PrimaryLime,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            if (_selectedSession == null) return;

            // Member Info Card
            Rectangle memCardRect = new Rectangle(pad, 52, rect.Width - (pad * 2), 68);
            PTUITheme.DrawCard(g, memCardRect, radius: 10, customBg: Color.FromArgb(28, 32, 44), customBorder: Color.FromArgb(42, 48, 64));

            // Avatar initial DA
            Rectangle avatarRect = new Rectangle(memCardRect.X + 12, memCardRect.Y + 14, 40, 40);
            using (SolidBrush b = new SolidBrush(PTUITheme.PrimaryLime))
            {
                PTUITheme.FillRoundedRectangle(g, b, avatarRect, 8);
            }
            TextRenderer.DrawText(g, _selectedSession.MemberInitials, PTUITheme.FontBold, avatarRect, Color.Black,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // Name & Progress
            Rectangle nameRect = new Rectangle(avatarRect.Right + 12, memCardRect.Y + 12, memCardRect.Width - 170, 22);
            TextRenderer.DrawText(g, _selectedSession.MemberName, PTUITheme.FontBold, nameRect, PTUITheme.TextWhite,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            Rectangle progRect = new Rectangle(avatarRect.Right + 12, memCardRect.Y + 34, memCardRect.Width - 170, 20);
            TextRenderer.DrawText(g, _selectedSession.ProgressText, PTUITheme.FontSmall, progRect, PTUITheme.PrimaryLime,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // Badge on top-right of member card: Rộng rãi, chữ rõ ràng
            Rectangle badgeRect = new Rectangle(memCardRect.Right - 105, memCardRect.Y + 20, 95, 26);
            PTUITheme.DrawBadge(g, _selectedSession.StatusBadge, badgeRect,
                _selectedSession.IsUrgent ? PTUITheme.LimeDark : Color.FromArgb(32, 38, 50),
                _selectedSession.IsUrgent ? PTUITheme.PrimaryLime : PTUITheme.TextMuted,
                PTUITheme.FontSmallBold, 6);

            // Section: THỜI GIAN & ĐỊA ĐIỂM
            int yLocation = 130;
            Rectangle secLocTitle = new Rectangle(pad, yLocation, rect.Width - (pad * 2), 18);
            TextRenderer.DrawText(g, "THỜI GIAN & ĐỊA ĐIỂM", PTUITheme.FontSmallBold, secLocTitle, PTUITheme.TextMuted,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            Rectangle timeValRect = new Rectangle(pad, yLocation + 20, rect.Width - (pad * 2), 22);
            TextRenderer.DrawText(g, $"{_selectedSession.TimeText} ({_selectedSession.DayName} - Hôm nay)", PTUITheme.FontBold, timeValRect, PTUITheme.TextWhite,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            Rectangle locValRect = new Rectangle(pad, yLocation + 42, rect.Width - (pad * 2), 20);
            TextRenderer.DrawText(g, _selectedSession.Location, PTUITheme.FontSmall, locValRect, PTUITheme.Cyan,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // Section: GIÁO ÁN BUỔI TẬP
            int yLesson = yLocation + 70;
            Rectangle secLessonTitle = new Rectangle(pad, yLesson, rect.Width - (pad * 2), 18);
            TextRenderer.DrawText(g, "GIÁO ÁN BUỔI TẬP", PTUITheme.FontSmallBold, secLessonTitle, PTUITheme.TextMuted,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            Rectangle lessonCard = new Rectangle(pad, yLesson + 22, rect.Width - (pad * 2), 98);
            PTUITheme.DrawCard(g, lessonCard, radius: 8, customBg: Color.FromArgb(17, 20, 27), customBorder: Color.FromArgb(34, 38, 50));

            int lineY = lessonCard.Y + 8;
            foreach (var ex in _selectedSession.Exercises)
            {
                Rectangle exRect = new Rectangle(lessonCard.X + 12, lineY, lessonCard.Width - 24, 24);
                TextRenderer.DrawText(g, ex, PTUITheme.FontSmall, exRect, PTUITheme.TextWhite,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
                lineY += 28;
            }
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Đã điểm danh thành công buổi huấn luyện của [{_selectedSession?.MemberName}]!",
                "Điểm Danh Buổi Tập", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditSchedule_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Mở form sửa lịch cho buổi huấn luyện của [{_selectedSession?.MemberName}].",
                "Sửa Lịch Buổi Tập", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancelSchedule_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show($"Bạn có chắc chắn muốn hủy ca hẹn với [{_selectedSession?.MemberName}] hôm nay?",
                "Xác Nhận Hủy Ca", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                MessageBox.Show("Ca hẹn đã được hủy thành công.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
