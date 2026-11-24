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
            public int SessionId { get; set; }
            public int WeekOffset { get; set; } = 0; // 0 = Tuần 45 (hiện tại), +1 = Tuần 46, -1 = Tuần 44...
            public int DayIndex { get; set; } // 1: Thứ 2, 2: Thứ 3, 3: Thứ 4, 4: Thứ 5, 5: Thứ 6, 6: Thứ 7
            public int SlotIndex { get; set; } // 0: 07:00, 1: 09:00, 2: 14:00, 3: 17:00
            public string MemberInitials { get; set; } = "";
            public string MemberName { get; set; } = "";
            public string MemberCode { get; set; } = "";
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

        // Lưu trữ lịch tập theo tuần offset (-1: Tuần 44, 0: Tuần 45, 1: Tuần 46, ...)
        private Dictionary<int, List<Session1On1>> _weekSchedules = new Dictionary<int, List<Session1On1>>();
        private Session1On1? _selectedSession;
        private Session1On1? _hoveredSession;

        // Trạng thái hiển thị: false = Theo Tuần, true = Theo Ngày
        private bool _isDayView = false;
        private int _currentWeekOffset = 0; // 0 = Tuần 45 (hiện tại), +1: tuần sau, -1: tuần trước

        // Vùng hit-test khi người dùng click chuột
        private Dictionary<Rectangle, Session1On1> _weekSessionRects = new Dictionary<Rectangle, Session1On1>();
        private Dictionary<Rectangle, Session1On1?> _daySessionRects = new Dictionary<Rectangle, Session1On1?>();

        public ucLichTap()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = PTUITheme.BgDark;

            InitSampleData();
            SetupCalendarHeader();
            SetupScheduleGrid();
            SetupRightPanel();

            // Tự động co giãn layout các cột theo kích thước cửa sổ
            pnlContent.Resize += (s, e) => UpdateResponsiveLayout();
            this.Load += (s, e) => UpdateResponsiveLayout();
        }

        private void UpdateResponsiveLayout()
        {
            int totalW = pnlContent.ClientSize.Width;
            if (totalW <= 0) return;

            // Co giãn chiều rộng panel chi tiết bên phải theo kích thước cửa sổ
            int rightW = 360;
            if (totalW >= 1350)
                rightW = 390;
            else if (totalW >= 1150)
                rightW = 360;
            else
                rightW = 345;

            if (pnlRightContainer.Width != rightW)
            {
                pnlRightContainer.Width = rightW;
            }
        }

        public List<Session1On1> GetSessionsForWeek(int weekOffset)
        {
            if (!_weekSchedules.TryGetValue(weekOffset, out var list))
            {
                list = GenerateDynamicScheduleForWeek(weekOffset);
                _weekSchedules[weekOffset] = list;
            }

            RefreshSessionsLiveStatus();
            return list;
        }

        public void RefreshSessionsLiveStatus()
        {
            DateTime now = DateTime.Now;

            foreach (var kvp in _weekSchedules)
            {
                int weekOffset = kvp.Key;
                DateTime monday = new DateTime(2026, 11, 2).AddDays(weekOffset * 7);

                foreach (var s in kvp.Value)
                {
                    DateTime sDate = monday.AddDays(s.DayIndex - 1);
                    DateTime sStart = sDate.Date + s.StartTime;
                    DateTime sEnd = sDate.Date + s.EndTime;

                    if (weekOffset < 0 || sEnd <= now)
                    {
                        s.StatusBadge = "HOÀN THÀNH";
                        s.IsUrgent = false;
                    }
                    else if (now >= sStart && now <= sEnd)
                    {
                        s.StatusBadge = "ĐANG TẬP";
                        s.IsUrgent = true;
                    }
                    else if (sDate.Date == now.Date)
                    {
                        TimeSpan diff = sStart - now;
                        if (diff.TotalMinutes <= 90 && diff.TotalMinutes > 0)
                        {
                            s.StatusBadge = "SẮP TỚI GIỜ";
                            s.IsUrgent = true;
                        }
                        else
                        {
                            s.StatusBadge = "CHỜ TẬP";
                            s.IsUrgent = false;
                        }
                    }
                    else
                    {
                        s.StatusBadge = "ĐÃ ĐẶT";
                        s.IsUrgent = false;
                    }
                }
            }
        }

        public DateTime GetDateForSession(Session1On1 session)
        {
            DateTime monday = new DateTime(2026, 11, 2).AddDays(session.WeekOffset * 7);
            return monday.AddDays(session.DayIndex - 1);
        }

        private void UpdateTopStatsForWeek(int weekOffset)
        {
            if (topStats == null) return;

            if (weekOffset == 0)
            {
                topStats.SetWeekStats(0, "24", "28.5");
            }
            else if (weekOffset == 1)
            {
                topStats.SetWeekStats(1, "26", "31.0");
            }
            else if (weekOffset == -1)
            {
                topStats.SetWeekStats(-1, "22", "26.0");
            }
            else
            {
                int count = 20 + Math.Abs(weekOffset * 3) % 8;
                double hours = 24.0 + (count - 20) * 1.5;
                topStats.SetWeekStats(weekOffset, count.ToString(), hours.ToString("F1"));
            }
        }

        private void InitSampleData()
        {
            // 1. TUẦN 45 (Tuần hiện tại: 02/11/2026 – 08/11/2026, Hôm nay là Thứ 5, 05/11/2026)
            var week45 = new List<Session1On1>
            {
                // Thứ 2 (02/11)
                new Session1On1
                {
                    SessionId = 1,
                    WeekOffset = 0,
                    DayIndex = 1, // Thứ 2
                    SlotIndex = 0,
                    MemberInitials = "DA",
                    MemberName = "Nguyễn Đức An",
                    MemberCode = "HV001",
                    ProgressText = "Tiến độ: 17 / 36 Buổi",
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(8, 30, 0),
                    DayName = "Thứ 2",
                    Location = "Zone A - Khu Tạ Tự Do (Free-weight)",
                    Exercises = new string[]
                    {
                        "1. Barbell Flat Bench Press: 4 sets x 8 reps (65kg)",
                        "2. Incline DB Press: 3 sets x 12 reps",
                        "3. Cable Tricep Pushdown: 4 sets x 15 reps"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "HOÀN THÀNH"
                },
                new Session1On1
                {
                    SessionId = 2,
                    WeekOffset = 0,
                    DayIndex = 1, // Thứ 2
                    SlotIndex = 3,
                    MemberInitials = "ML",
                    MemberName = "Phạm Mai Linh",
                    MemberCode = "HV002",
                    ProgressText = "Tiến độ: 14 / 30 Buổi",
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
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "HOÀN THÀNH"
                },

                // Thứ 3 (03/11)
                new Session1On1
                {
                    SessionId = 3,
                    WeekOffset = 0,
                    DayIndex = 2, // Thứ 3
                    SlotIndex = 1,
                    MemberInitials = "HN",
                    MemberName = "Lê Hoàng Nam",
                    MemberCode = "HV003",
                    ProgressText = "Tiến độ: 11 / 24 Buổi",
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(10, 30, 0),
                    DayName = "Thứ 3",
                    Location = "Zone C - Khu Tập Chân & Squat Rack",
                    Exercises = new string[]
                    {
                        "1. Barbell Back Squat: 5 sets x 5 reps (90kg)",
                        "2. Bulgarian Split Squat: 3 sets x 10 reps / chân",
                        "3. Standing Calf Raise: 4 sets x 15 reps"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "HOÀN THÀNH"
                },

                // Thứ 4 (04/11)
                new Session1On1
                {
                    SessionId = 4,
                    WeekOffset = 0,
                    DayIndex = 3, // Thứ 4
                    SlotIndex = 2,
                    MemberInitials = "BT",
                    MemberName = "Trần Bảo Trâm",
                    MemberCode = "HV005",
                    ProgressText = "Tiến độ: 8 / 20 Buổi",
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(15, 30, 0),
                    DayName = "Thứ 4",
                    Location = "Zone D - Functional & Pilates Mat",
                    Exercises = new string[]
                    {
                        "1. Kettlebell Swing: 4 sets x 20 reps (16kg)",
                        "2. Battle Ropes Wave: 4 sets x 30s",
                        "3. Mat Pilates Glute Bridge: 3 sets x 15 reps"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "HOÀN THÀNH"
                },

                // Thứ 5 (05/11 - HÔM NAY)
                new Session1On1
                {
                    SessionId = 5,
                    WeekOffset = 0,
                    DayIndex = 4, // Thứ 5 (HÔM NAY)
                    SlotIndex = 1,
                    MemberInitials = "HN",
                    MemberName = "Lê Hoàng Nam",
                    MemberCode = "HV003",
                    ProgressText = "Tiến độ: 12 / 24 Buổi",
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(10, 30, 0),
                    DayName = "Thứ 5",
                    Location = "Zone C - Khu Tập Thể Lực & Plyometrics",
                    Exercises = new string[]
                    {
                        "1. Hex Bar Deadlift: 4 sets x 8 reps (100kg)",
                        "2. Box Jump: 4 sets x 10 reps",
                        "3. Core Plank with Weight: 3 sets x 60s"
                    },
                    IsToday = true,
                    IsUrgent = false,
                    StatusBadge = "HÔM NAY"
                },
                new Session1On1
                {
                    SessionId = 6,
                    WeekOffset = 0,
                    DayIndex = 4, // Thứ 5 (HÔM NAY)
                    SlotIndex = 3,
                    MemberInitials = "DA",
                    MemberName = "Nguyễn Đức An",
                    MemberCode = "HV001",
                    ProgressText = "Tiến độ: 18 / 36 Buổi",
                    StartTime = new TimeSpan(17, 0, 0),
                    EndTime = new TimeSpan(18, 30, 0),
                    DayName = "Thứ 5",
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

                // Thứ 6 (06/11)
                new Session1On1
                {
                    SessionId = 7,
                    WeekOffset = 0,
                    DayIndex = 5, // Thứ 6
                    SlotIndex = 0,
                    MemberInitials = "DA",
                    MemberName = "Nguyễn Đức An",
                    MemberCode = "HV001",
                    ProgressText = "Tiến độ: 19 / 36 Buổi",
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(8, 30, 0),
                    DayName = "Thứ 6",
                    Location = "Zone A - Khu Tạ Tự Do (Free-weight)",
                    Exercises = new string[]
                    {
                        "1. Barbell Overhead Press: 4 sets x 10 reps (40kg)",
                        "2. Lateral Dumbbell Raise: 4 sets x 15 reps (8kg)",
                        "3. Face Pull Cable: 3 sets x 15 reps"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "THỨ 6"
                },
                new Session1On1
                {
                    SessionId = 8,
                    WeekOffset = 0,
                    DayIndex = 5, // Thứ 6
                    SlotIndex = 3,
                    MemberInitials = "ML",
                    MemberName = "Phạm Mai Linh",
                    MemberCode = "HV002",
                    ProgressText = "Tiến độ: 15 / 30 Buổi",
                    StartTime = new TimeSpan(17, 0, 0),
                    EndTime = new TimeSpan(18, 30, 0),
                    DayName = "Thứ 6",
                    Location = "Zone B - Cardio & Giảm Mỡ Siết Eo",
                    Exercises = new string[]
                    {
                        "1. Hip Thrust with Barbell: 4 sets x 12 reps (60kg)",
                        "2. Cable Kickback: 3 sets x 15 reps / chân",
                        "3. Treadmill Incline Walk: 25 phút dốc 12%"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "THỨ 6"
                },

                // Thứ 7 (07/11)
                new Session1On1
                {
                    SessionId = 9,
                    WeekOffset = 0,
                    DayIndex = 6, // Thứ 7
                    SlotIndex = 1,
                    MemberInitials = "TD",
                    MemberName = "Đỗ Thùy Dung",
                    MemberCode = "HV006",
                    ProgressText = "Tiến độ: 4 / 15 Buổi",
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(10, 30, 0),
                    DayName = "Thứ 7",
                    Location = "Zone B - Máy Tập Toàn Thân",
                    Exercises = new string[]
                    {
                        "1. Seated Cable Row: 4 sets x 12 reps",
                        "2. Dumbbell Lunges: 3 sets x 15 bước",
                        "3. Cooldown & Kéo giãn thắt lưng"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "THỨ 7"
                }
            };
            _weekSchedules[0] = week45;

            // 2. TUẦN 46 (Tuần tiếp theo: 09/11/2026 – 15/11/2026) - Lịch tuần tới
            var week46 = new List<Session1On1>
            {
                new Session1On1
                {
                    SessionId = 21,
                    WeekOffset = 1,
                    DayIndex = 1, // Thứ 2 (09/11)
                    SlotIndex = 1, // 09:00 - 10:30
                    MemberInitials = "DA",
                    MemberName = "Nguyễn Đức An",
                    MemberCode = "HV001",
                    ProgressText = "Tiến độ: 20 / 36 Buổi",
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(10, 30, 0),
                    DayName = "Thứ 2",
                    Location = "Zone A - Khu Tạ Tự Do (Free-weight)",
                    Exercises = new string[]
                    {
                        "1. Incline Dumbbell Press: 4 sets x 10 reps (24kg)",
                        "2. Flat Dumbbell Fly: 3 sets x 12 reps",
                        "3. Chest Dips Bodyweight: 3 sets x 10 reps"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "ĐÃ ĐẶT"
                },
                new Session1On1
                {
                    SessionId = 22,
                    WeekOffset = 1,
                    DayIndex = 1, // Thứ 2 (09/11)
                    SlotIndex = 2, // 14:00 - 15:30
                    MemberInitials = "MT",
                    MemberName = "Vũ Minh Tuấn",
                    MemberCode = "HV004",
                    ProgressText = "Tiến độ: 8 / 24 Buổi",
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(15, 30, 0),
                    DayName = "Thứ 2",
                    Location = "Zone D - Functional & Thể Lực",
                    Exercises = new string[]
                    {
                        "1. Clean & Jerk kỹ thuật: 4 sets x 6 reps (40kg)",
                        "2. Kettlebell Snatch: 4 sets x 12 reps / tay",
                        "3. Row Ergometer: 15 phút đốt mỡ"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "ĐÃ ĐẶT"
                },
                new Session1On1
                {
                    SessionId = 23,
                    WeekOffset = 1,
                    DayIndex = 2, // Thứ 3 (10/11)
                    SlotIndex = 0, // 07:00 - 08:30
                    MemberInitials = "ML",
                    MemberName = "Phạm Mai Linh",
                    MemberCode = "HV002",
                    ProgressText = "Tiến độ: 16 / 30 Buổi",
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(8, 30, 0),
                    DayName = "Thứ 3",
                    Location = "Zone B - Cardio & Giảm Mỡ Siết Eo",
                    Exercises = new string[]
                    {
                        "1. Barbell Sumo Squat: 4 sets x 12 reps (50kg)",
                        "2. Glute Abductor Machine: 3 sets x 15 reps",
                        "3. Plank Hold: 4 sets x 45s"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "ĐÃ ĐẶT"
                },
                new Session1On1
                {
                    SessionId = 24,
                    WeekOffset = 1,
                    DayIndex = 2, // Thứ 3 (10/11)
                    SlotIndex = 3, // 17:00 - 18:30
                    MemberInitials = "HN",
                    MemberName = "Lê Hoàng Nam",
                    MemberCode = "HV003",
                    ProgressText = "Tiến độ: 13 / 24 Buổi",
                    StartTime = new TimeSpan(17, 0, 0),
                    EndTime = new TimeSpan(18, 30, 0),
                    DayName = "Thứ 3",
                    Location = "Zone A - Khu Tạ Đòn & Xô Lưng",
                    Exercises = new string[]
                    {
                        "1. Lat Pulldown Wide Grip: 4 sets x 10 reps (60kg)",
                        "2. Seated Cable Row: 4 sets x 12 reps",
                        "3. Bicep Barbell Curl: 3 sets x 12 reps (25kg)"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "ĐÃ ĐẶT"
                },
                new Session1On1
                {
                    SessionId = 25,
                    WeekOffset = 1,
                    DayIndex = 3, // Thứ 4 (11/11)
                    SlotIndex = 1, // 09:00 - 10:30
                    MemberInitials = "BT",
                    MemberName = "Trần Bảo Trâm",
                    MemberCode = "HV005",
                    ProgressText = "Tiến độ: 9 / 20 Buổi",
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(10, 30, 0),
                    DayName = "Thứ 4",
                    Location = "Zone D - Thảm Pilates & Dây Kháng Lực",
                    Exercises = new string[]
                    {
                        "1. Reformer Footwork Core: 4 sets x 15 reps",
                        "2. Resistance Band Side Walk: 3 sets x 20 bước",
                        "3. Cooldown & Giãn cơ lưng xô"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "ĐÃ ĐẶT"
                },
                new Session1On1
                {
                    SessionId = 26,
                    WeekOffset = 1,
                    DayIndex = 3, // Thứ 4 (11/11)
                    SlotIndex = 3, // 17:00 - 18:30
                    MemberInitials = "TD",
                    MemberName = "Đỗ Thùy Dung",
                    MemberCode = "HV006",
                    ProgressText = "Tiến độ: 5 / 15 Buổi",
                    StartTime = new TimeSpan(17, 0, 0),
                    EndTime = new TimeSpan(18, 30, 0),
                    DayName = "Thứ 4",
                    Location = "Zone B - Máy Tập Đa Năng",
                    Exercises = new string[]
                    {
                        "1. Leg Press Máy Ngang: 4 sets x 12 reps (80kg)",
                        "2. Dumbbell Lunges: 3 sets x 15 bước",
                        "3. Máy leo cầu thang Stairmaster: 15 phút"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "ĐÃ ĐẶT"
                },
                new Session1On1
                {
                    SessionId = 27,
                    WeekOffset = 1,
                    DayIndex = 4, // Thứ 5 (12/11)
                    SlotIndex = 0, // 07:00 - 08:30
                    MemberInitials = "DA",
                    MemberName = "Nguyễn Đức An",
                    MemberCode = "HV001",
                    ProgressText = "Tiến độ: 21 / 36 Buổi",
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(8, 30, 0),
                    DayName = "Thứ 5",
                    Location = "Zone A - Khu Tạ Tự Do (Free-weight)",
                    Exercises = new string[]
                    {
                        "1. Standing Military Press: 4 sets x 8 reps (45kg)",
                        "2. Lateral Raise Dumbbell: 4 sets x 15 reps (10kg)",
                        "3. Face Pull Cable: 3 sets x 15 reps"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "ĐÃ ĐẶT"
                },
                new Session1On1
                {
                    SessionId = 28,
                    WeekOffset = 1,
                    DayIndex = 4, // Thứ 5 (12/11)
                    SlotIndex = 2, // 14:00 - 15:30
                    MemberInitials = "HN",
                    MemberName = "Lê Hoàng Nam",
                    MemberCode = "HV003",
                    ProgressText = "Tiến độ: 14 / 24 Buổi",
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(15, 30, 0),
                    DayName = "Thứ 5",
                    Location = "Zone C - Khu Tập Chân & Squat Rack",
                    Exercises = new string[]
                    {
                        "1. Front Squat Barbell: 4 sets x 6 reps (70kg)",
                        "2. Leg Extension Máy: 3 sets x 15 reps",
                        "3. Lying Leg Curl: 3 sets x 12 reps"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "ĐÃ ĐẶT"
                },
                new Session1On1
                {
                    SessionId = 29,
                    WeekOffset = 1,
                    DayIndex = 5, // Thứ 6 (13/11)
                    SlotIndex = 1, // 09:00 - 10:30
                    MemberInitials = "MT",
                    MemberName = "Vũ Minh Tuấn",
                    MemberCode = "HV004",
                    ProgressText = "Tiến độ: 9 / 24 Buổi",
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(10, 30, 0),
                    DayName = "Thứ 6",
                    Location = "Zone D - Thể Lực & HIIT",
                    Exercises = new string[]
                    {
                        "1. Kettlebell Complex: 5 rounds x 1 phút",
                        "2. Plyo Push-up: 4 sets x 10 reps",
                        "3. Battle Ropes Wave: 4 sets x 40s"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "ĐÃ ĐẶT"
                },
                new Session1On1
                {
                    SessionId = 30,
                    WeekOffset = 1,
                    DayIndex = 5, // Thứ 6 (13/11)
                    SlotIndex = 3, // 17:00 - 18:30
                    MemberInitials = "ML",
                    MemberName = "Phạm Mai Linh",
                    MemberCode = "HV002",
                    ProgressText = "Tiến độ: 17 / 30 Buổi",
                    StartTime = new TimeSpan(17, 0, 0),
                    EndTime = new TimeSpan(18, 30, 0),
                    DayName = "Thứ 6",
                    Location = "Zone B - Cardio & Siết Mỡ",
                    Exercises = new string[]
                    {
                        "1. Barbell Hip Thrust: 4 sets x 10 reps (65kg)",
                        "2. Cable Pull Through: 3 sets x 15 reps",
                        "3. HIIT Chạy Dốc: 20 phút interval"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "ĐÃ ĐẶT"
                },
                new Session1On1
                {
                    SessionId = 31,
                    WeekOffset = 1,
                    DayIndex = 6, // Thứ 7 (14/11)
                    SlotIndex = 0, // 07:00 - 08:30
                    MemberInitials = "QH",
                    MemberName = "Đặng Quốc Huy",
                    MemberCode = "HV007",
                    ProgressText = "Tiến độ: 1 / 20 Buổi",
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(8, 30, 0),
                    DayName = "Thứ 7",
                    Location = "Zone A - Đo Chỉ Số & Thể Lực Ban Đầu",
                    Exercises = new string[]
                    {
                        "1. Đo phân tích InBody 570 chi tiết",
                        "2. Đánh giá tư thế chuyển động Overhead Squat",
                        "3. Khởi động các bài tập kích hoạt thân dưới"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "ĐÃ ĐẶT"
                }
            };
            _weekSchedules[1] = week46;

            // 3. TUẦN 44 (Tuần trước: 26/10/2026 – 01/11/2026) - Lịch tuần đã hoàn thành
            var week44 = new List<Session1On1>
            {
                new Session1On1
                {
                    SessionId = 11,
                    WeekOffset = -1,
                    DayIndex = 1, // Thứ 2 (26/10)
                    SlotIndex = 0,
                    MemberInitials = "DA",
                    MemberName = "Nguyễn Đức An",
                    MemberCode = "HV001",
                    ProgressText = "Tiến độ: 16 / 36 Buổi",
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(8, 30, 0),
                    DayName = "Thứ 2",
                    Location = "Zone A - Khu Tạ Tự Do (Free-weight)",
                    Exercises = new string[]
                    {
                        "1. Barbell Bench Press: 4 sets x 8 reps (62.5kg)",
                        "2. Dumbbell Incline Press: 3 sets x 12 reps",
                        "3. Tricep Cable Rope: 4 sets x 15 reps"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "HOÀN THÀNH"
                },
                new Session1On1
                {
                    SessionId = 12,
                    WeekOffset = -1,
                    DayIndex = 1, // Thứ 2 (26/10)
                    SlotIndex = 3,
                    MemberInitials = "ML",
                    MemberName = "Phạm Mai Linh",
                    MemberCode = "HV002",
                    ProgressText = "Tiến độ: 13 / 30 Buổi",
                    StartTime = new TimeSpan(17, 0, 0),
                    EndTime = new TimeSpan(18, 30, 0),
                    DayName = "Thứ 2",
                    Location = "Zone B - Cardio & Giảm Mỡ",
                    Exercises = new string[]
                    {
                        "1. Deadlift Romanian: 4 sets x 12 reps (40kg)",
                        "2. Lunge bước đi: 3 sets x 20 bước",
                        "3. Máy chạy bộ dốc: 20 phút"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "HOÀN THÀNH"
                },
                new Session1On1
                {
                    SessionId = 13,
                    WeekOffset = -1,
                    DayIndex = 2, // Thứ 3 (27/10)
                    SlotIndex = 1,
                    MemberInitials = "HN",
                    MemberName = "Lê Hoàng Nam",
                    MemberCode = "HV003",
                    ProgressText = "Tiến độ: 10 / 24 Buổi",
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(10, 30, 0),
                    DayName = "Thứ 3",
                    Location = "Zone C - Khu Squat Rack",
                    Exercises = new string[]
                    {
                        "1. Back Squat Barbell: 5 sets x 5 reps (85kg)",
                        "2. Leg Press Máy Đạp: 3 sets x 12 reps",
                        "3. Bắp chuối đứng: 4 sets x 15 reps"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "HOÀN THÀNH"
                },
                new Session1On1
                {
                    SessionId = 14,
                    WeekOffset = -1,
                    DayIndex = 3, // Thứ 4 (28/10)
                    SlotIndex = 2,
                    MemberInitials = "BT",
                    MemberName = "Trần Bảo Trâm",
                    MemberCode = "HV005",
                    ProgressText = "Tiến độ: 7 / 20 Buổi",
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(15, 30, 0),
                    DayName = "Thứ 4",
                    Location = "Zone D - Thảm Pilates",
                    Exercises = new string[]
                    {
                        "1. Kettlebell Swing: 4 sets x 15 reps",
                        "2. Battle Ropes: 4 sets x 30s",
                        "3. Glute Bridge Mat: 3 sets x 15 reps"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "HOÀN THÀNH"
                },
                new Session1On1
                {
                    SessionId = 15,
                    WeekOffset = -1,
                    DayIndex = 4, // Thứ 5 (29/10)
                    SlotIndex = 1,
                    MemberInitials = "HN",
                    MemberName = "Lê Hoàng Nam",
                    MemberCode = "HV003",
                    ProgressText = "Tiến độ: 10 / 24 Buổi",
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(10, 30, 0),
                    DayName = "Thứ 5",
                    Location = "Zone C - Khu Thể Lực & Deadlift",
                    Exercises = new string[]
                    {
                        "1. Deadlift Trap Bar: 4 sets x 8 reps (95kg)",
                        "2. Box Jump Bật Nhảy: 4 sets x 10 reps",
                        "3. Plank gánh bánh tạ: 3 sets x 45s"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "HOÀN THÀNH"
                },
                new Session1On1
                {
                    SessionId = 16,
                    WeekOffset = -1,
                    DayIndex = 5, // Thứ 6 (30/10)
                    SlotIndex = 0,
                    MemberInitials = "DA",
                    MemberName = "Nguyễn Đức An",
                    MemberCode = "HV001",
                    ProgressText = "Tiến độ: 16 / 36 Buổi",
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(8, 30, 0),
                    DayName = "Thứ 6",
                    Location = "Zone A - Khu Tạ Tự Do (Free-weight)",
                    Exercises = new string[]
                    {
                        "1. Overhead Press Barbell: 4 sets x 10 reps (37.5kg)",
                        "2. Dumbbell Side Raise: 4 sets x 15 reps",
                        "3. Face Pull Cable: 3 sets x 15 reps"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "HOÀN THÀNH"
                },
                new Session1On1
                {
                    SessionId = 17,
                    WeekOffset = -1,
                    DayIndex = 5, // Thứ 6 (30/10)
                    SlotIndex = 3,
                    MemberInitials = "ML",
                    MemberName = "Phạm Mai Linh",
                    MemberCode = "HV002",
                    ProgressText = "Tiến độ: 13 / 30 Buổi",
                    StartTime = new TimeSpan(17, 0, 0),
                    EndTime = new TimeSpan(18, 30, 0),
                    DayName = "Thứ 6",
                    Location = "Zone B - Cardio & Giảm Mỡ",
                    Exercises = new string[]
                    {
                        "1. Hip Thrust Barbell: 4 sets x 12 reps (55kg)",
                        "2. Cable Đá Mông: 3 sets x 15 reps",
                        "3. Đi bộ dốc máy chạy: 25 phút"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = "HOÀN THÀNH"
                }
            };
            _weekSchedules[-1] = week44;

            // Thiết lập phiên tập đang chọn và thống kê ban đầu (Ưu tiên ca hôm nay Thứ 5)
            RefreshSessionsLiveStatus();
            var currentSessions = GetSessionsForWeek(0);
            _selectedSession = currentSessions.FirstOrDefault(s => s.IsToday && s.IsUrgent) 
                ?? currentSessions.Where(s => s.IsToday).OrderByDescending(s => s.EndTime).FirstOrDefault()
                ?? currentSessions.FirstOrDefault();
            UpdateTopStatsForWeek(0);
        }

        private List<Session1On1> GenerateDynamicScheduleForWeek(int weekOffset)
        {
            var list = new List<Session1On1>();
            string[] memberNames = new string[] { "Nguyễn Đức An", "Phạm Mai Linh", "Lê Hoàng Nam", "Vũ Minh Tuấn", "Trần Bảo Trâm", "Đỗ Thùy Dung", "Đặng Quốc Huy" };
            string[] memberCodes = new string[] { "HV001", "HV002", "HV003", "HV004", "HV005", "HV006", "HV007" };
            string[] memberInitials = new string[] { "DA", "ML", "HN", "MT", "BT", "TD", "QH" };
            string[] dayNames = new string[] { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7" };

            TimeSpan[] startTimes = new TimeSpan[] { new TimeSpan(7, 0, 0), new TimeSpan(9, 0, 0), new TimeSpan(14, 0, 0), new TimeSpan(17, 0, 0) };
            TimeSpan[] endTimes = new TimeSpan[] { new TimeSpan(8, 30, 0), new TimeSpan(10, 30, 0), new TimeSpan(15, 30, 0), new TimeSpan(18, 30, 0) };

            string badge = weekOffset > 0 ? "ĐÃ ĐẶT" : "HOÀN THÀNH";
            int baseId = Math.Abs(weekOffset) * 100;

            // Tạo các ca tập rải rác trong tuần cho các tuần xa hơn
            int[] daysToInclude = (Math.Abs(weekOffset) % 2 == 0) ? new int[] { 1, 2, 4, 5, 6 } : new int[] { 1, 3, 4, 5, 6 };
            int idCounter = 0;

            foreach (int day in daysToInclude)
            {
                int slot1 = (day + Math.Abs(weekOffset)) % 4;
                int memberIdx = (day + Math.Abs(weekOffset)) % memberNames.Length;

                list.Add(new Session1On1
                {
                    SessionId = baseId + (++idCounter),
                    WeekOffset = weekOffset,
                    DayIndex = day,
                    SlotIndex = slot1,
                    MemberInitials = memberInitials[memberIdx],
                    MemberName = memberNames[memberIdx],
                    MemberCode = memberCodes[memberIdx],
                    ProgressText = $"Tiến độ: {10 + (Math.Abs(weekOffset) * 2) % 15} / 30 Buổi",
                    StartTime = startTimes[slot1],
                    EndTime = endTimes[slot1],
                    DayName = dayNames[day - 1],
                    Location = "Zone A - Huấn Luyện 1:1 Cao Cấp",
                    Exercises = new string[]
                    {
                        "1. Khởi động động học & Kích hoạt cơ chính",
                        "2. Bài tập sức mạnh trọng tâm: 4 sets x 10 reps",
                        "3. Giãn cơ hồi phục & Điều hòa nhịp tim"
                    },
                    IsToday = false,
                    IsUrgent = false,
                    StatusBadge = badge
                });

                if (day == 1 || day == 5)
                {
                    int slot2 = (slot1 + 2) % 4;
                    int m2Idx = (memberIdx + 3) % memberNames.Length;
                    list.Add(new Session1On1
                    {
                        SessionId = baseId + (++idCounter),
                        WeekOffset = weekOffset,
                        DayIndex = day,
                        SlotIndex = slot2,
                        MemberInitials = memberInitials[m2Idx],
                        MemberName = memberNames[m2Idx],
                        MemberCode = memberCodes[m2Idx],
                        ProgressText = $"Tiến độ: {8 + (Math.Abs(weekOffset) * 2) % 12} / 24 Buổi",
                        StartTime = startTimes[slot2],
                        EndTime = endTimes[slot2],
                        DayName = dayNames[day - 1],
                        Location = "Zone B - Khu Giảm Mỡ & Thể Lực",
                        Exercises = new string[]
                        {
                            "1. Khởi động khớp & Kích hoạt chuỗi cơ sau",
                            "2. Bài tập sức bền & Phản xạ cơ bắp: 4 sets x 12 reps",
                            "3. Giãn cơ căng thẳng & Điều hòa hô hấp"
                        },
                        IsToday = false,
                        IsUrgent = false,
                        StatusBadge = badge
                    });
                }
            }

            return list;
        }

        private void SetupCalendarHeader()
        {
            // Nút chuyển chế độ xem Theo Tuần / Theo Ngày
            btnWeekView.Click += (s, e) => SwitchCalendarView(false);
            btnDayView.Click += (s, e) => SwitchCalendarView(true);

            // Nút chuyển tuần trước / tuần sau (< và >)
            btnPrevWeek.Click += (s, e) => NavigateWeek(-1);
            btnNextWeek.Click += (s, e) => NavigateWeek(1);
        }

        private void SwitchCalendarView(bool isDayView)
        {
            _isDayView = isDayView;

            if (!_isDayView)
            {
                // Chế độ Theo Tuần đang chọn
                btnWeekView.BackColor = Color.FromArgb(34, 45, 22);
                btnWeekView.ForeColor = PTUITheme.PrimaryLime;
                btnWeekView.Font = PTUITheme.FontBold;
                btnWeekView.FlatAppearance.BorderColor = PTUITheme.PrimaryLime;

                btnDayView.BackColor = Color.FromArgb(22, 25, 34);
                btnDayView.ForeColor = PTUITheme.TextMuted;
                btnDayView.Font = PTUITheme.FontRegular;
                btnDayView.FlatAppearance.BorderColor = Color.FromArgb(45, 50, 65);
            }
            else
            {
                // Chế độ Theo Ngày đang chọn
                btnDayView.BackColor = Color.FromArgb(34, 45, 22);
                btnDayView.ForeColor = PTUITheme.PrimaryLime;
                btnDayView.Font = PTUITheme.FontBold;
                btnDayView.FlatAppearance.BorderColor = PTUITheme.PrimaryLime;

                btnWeekView.BackColor = Color.FromArgb(22, 25, 34);
                btnWeekView.ForeColor = PTUITheme.TextMuted;
                btnWeekView.Font = PTUITheme.FontRegular;
                btnWeekView.FlatAppearance.BorderColor = Color.FromArgb(45, 50, 65);
            }

            pnlScheduleGrid.Invalidate();
        }

        private void NavigateWeek(int direction)
        {
            _currentWeekOffset += direction;

            DateTime baseMonday = new DateTime(2026, 11, 2).AddDays(_currentWeekOffset * 7);
            DateTime baseSunday = baseMonday.AddDays(6);
            int weekNumber = 45 + _currentWeekOffset;

            lblWeekRange.Text = $"{baseMonday:dd/MM/yyyy} – {baseSunday:dd/MM/yyyy} (Tuần {weekNumber})";
            btnNextWeek.Location = new Point(lblWeekRange.Right + 12, 8);

            // Cập nhật thống kê trên topStats (Card 3 & 4)
            UpdateTopStatsForWeek(_currentWeekOffset);

            // Cập nhật ca tập được chọn sang tuần mới
            var weekSessions = GetSessionsForWeek(_currentWeekOffset);
            if (_currentWeekOffset == 0)
            {
                _selectedSession = weekSessions.FirstOrDefault(s => s.IsToday && s.IsUrgent) 
                    ?? weekSessions.Where(s => s.IsToday).OrderByDescending(s => s.EndTime).FirstOrDefault()
                    ?? weekSessions.FirstOrDefault();
            }
            else
            {
                _selectedSession = weekSessions.FirstOrDefault();
            }

            // Cập nhật các nút danh sách ca tập ở panel bên phải
            UpdateTodaySessionsButtons();

            // Vẽ lại toàn bộ lưới lịch và panel chi tiết
            pnlScheduleGrid.Invalidate();
            pnlRightDetails.Invalidate();
        }

        private void SetupScheduleGrid()
        {
            // Kích hoạt DoubleBuffered cho grid và panel chi tiết
            try
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                    null, pnlScheduleGrid, new object[] { true });

                typeof(Control).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                    null, pnlRightDetails, new object[] { true });
            }
            catch { }

            pnlScheduleGrid.Paint += PnlScheduleGrid_Paint;
            pnlScheduleGrid.Resize += (s, e) => pnlScheduleGrid.Invalidate();

            // Tương tác chuột: rê chuột đổi con trỏ bàn tay, bấm chuột để xem chi tiết ca tập
            pnlScheduleGrid.MouseMove += PnlScheduleGrid_MouseMove;
            pnlScheduleGrid.MouseLeave += (s, e) =>
            {
                if (_hoveredSession != null)
                {
                    _hoveredSession = null;
                    pnlScheduleGrid.Invalidate();
                }
            };
            pnlScheduleGrid.MouseClick += PnlScheduleGrid_MouseClick;
        }

        private void PnlScheduleGrid_MouseMove(object? sender, MouseEventArgs e)
        {
            Session1On1? hovered = null;

            if (!_isDayView)
            {
                foreach (var kvp in _weekSessionRects)
                {
                    if (kvp.Key.Contains(e.Location))
                    {
                        hovered = kvp.Value;
                        break;
                    }
                }
            }
            else
            {
                foreach (var kvp in _daySessionRects)
                {
                    if (kvp.Key.Contains(e.Location) && kvp.Value != null)
                    {
                        hovered = kvp.Value;
                        break;
                    }
                }
            }

            if (hovered != _hoveredSession)
            {
                _hoveredSession = hovered;
                pnlScheduleGrid.Cursor = hovered != null ? Cursors.Hand : Cursors.Default;
                pnlScheduleGrid.Invalidate();
            }
        }

        private void PnlScheduleGrid_MouseClick(object? sender, MouseEventArgs e)
        {
            Session1On1? clickedSession = null;

            if (!_isDayView)
            {
                foreach (var kvp in _weekSessionRects)
                {
                    if (kvp.Key.Contains(e.Location))
                    {
                        clickedSession = kvp.Value;
                        break;
                    }
                }
            }
            else
            {
                foreach (var kvp in _daySessionRects)
                {
                    if (kvp.Key.Contains(e.Location) && kvp.Value != null)
                    {
                        clickedSession = kvp.Value;
                        break;
                    }
                }
            }

            if (clickedSession != null)
            {
                _selectedSession = clickedSession;
                pnlScheduleGrid.Invalidate();
                pnlRightDetails.Invalidate();
                UpdateTodaySessionsButtons();
            }
        }

        private void PnlScheduleGrid_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = pnlScheduleGrid.ClientRectangle;
            PTUITheme.DrawCard(g, new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1), radius: 12);

            if (_isDayView)
            {
                PaintDayView(g, rect);
            }
            else
            {
                PaintWeekView(g, rect);
            }
        }

        // ================= VẼ GIAO DIỆN THEO TUẦN (WEEK VIEW) =================
        private void PaintWeekView(Graphics g, Rectangle rect)
        {
            _weekSessionRects.Clear();

            // Tính toán ngày tháng thực tế theo tuần đang xem (Mốc Tuần 45 bắt đầu từ Thứ 2: 02/11/2026)
            DateTime monday = new DateTime(2026, 11, 2).AddDays(_currentWeekOffset * 7);
            string[] headers = new string[7];
            headers[0] = rect.Width < 620 ? "Giờ" : "Khung Giờ";
            for (int d = 0; d < 6; d++)
            {
                DateTime dayDate = monday.AddDays(d);
                int dayNumber = d + 2; // Thứ 2 - Thứ 7
                headers[d + 1] = rect.Width < 620
                    ? $"T{dayNumber} ({dayDate.Day:D2})"
                    : $"Thứ {dayNumber} ({dayDate.Day:D2})";
            }

            int colCount = headers.Length;
            int timeColWidth = rect.Width < 620 ? 82 : 100;
            int availableDayWidth = rect.Width - timeColWidth - 28;
            int dayColWidth = Math.Max(50, availableDayWidth / (colCount - 1));

            int headerY = 14;
            int headerHeight = 36;
            int tableLeft = 14;
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

                // i = 4 là Thứ 5 (05/11 - Hôm nay) khi đang ở tuần hiện tại (Tuần 45 - offset 0)
                bool isTodayCol = (i == 4 && _currentWeekOffset == 0);
                if (isTodayCol)
                {
                    Rectangle todayBadge = new Rectangle(colX + 2, headerY + 4, w - 4, headerHeight - 8);
                    PTUITheme.DrawBadge(g, headers[i], todayBadge, PTUITheme.LimeDark, PTUITheme.PrimaryLime,
                        w < 65 ? new Font("Segoe UI", 7.5f, FontStyle.Bold) : PTUITheme.FontSmallBold, 6);
                }
                else
                {
                    Font hFont = (w < 65) ? new Font("Segoe UI", 7.5f, FontStyle.Regular) : PTUITheme.FontSmall;
                    TextRenderer.DrawText(g, headers[i], (i == 0) ? PTUITheme.FontSmallBold : hFont, headerCell,
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

            var currentWeekSessions = GetSessionsForWeek(_currentWeekOffset);

            for (int r = 0; r < timeSlots.Length; r++)
            {
                int rowY = startRowY + r * rowHeight;

                // Horizontal row grid line
                using (Pen borderPen = new Pen(Color.FromArgb(32, 37, 50), 1))
                {
                    g.DrawLine(borderPen, tableLeft, rowY, tableRight, rowY);
                }

                // Time label cell
                Rectangle timeCell = new Rectangle(tableLeft + 4, rowY, timeColWidth - 8, rowHeight);
                Font slotFont = timeColWidth < 90 ? new Font("Segoe UI", 7.5f, FontStyle.Regular) : PTUITheme.FontSmall;
                string slotText = timeColWidth < 90 ? timeSlots[r].Replace(" – ", "-") : timeSlots[r];
                TextRenderer.DrawText(g, slotText, slotFont, timeCell, PTUITheme.TextMuted,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                // Schedule Cells tương tác đầy đủ theo từng tuần
                var sessionsInSlot = currentWeekSessions.Where(s => s.SlotIndex == r).ToList();
                foreach (var session in sessionsInSlot)
                {
                    Rectangle cellRect = GetCellRect(session.DayIndex, rowY, tableLeft, timeColWidth, dayColWidth, rowHeight);
                    _weekSessionRects[cellRect] = session;

                    bool isSelected = (_selectedSession == session);
                    bool isHovered = (_hoveredSession == session);

                    DrawScheduleCard(g, cellRect, session.MemberName, session.TimeText, isSelected, isHovered);
                }
            }

            // Bottom border
            using (Pen borderPen = new Pen(Color.FromArgb(32, 37, 50), 1))
            {
                g.DrawLine(borderPen, tableLeft, tableBottom, tableRight, tableBottom);
            }
        }

        // ================= VẼ GIAO DIỆN THEO NGÀY (DAY VIEW) =================
        private void PaintDayView(Graphics g, Rectangle rect)
        {
            _daySessionRects.Clear();

            int pad = 20;
            DateTime monday = new DateTime(2026, 11, 2).AddDays(_currentWeekOffset * 7);
            DateTime targetDay = monday.AddDays(3); // Thứ 5 (05/11/2026)
            string dayDesc = (_currentWeekOffset == 0) ? " (HÔM NAY)" : $" (TUẦN {45 + _currentWeekOffset})";

            // 1. Banner tiêu đề ngày được chọn
            Rectangle dayHeaderRect = new Rectangle(pad, 16, rect.Width - (pad * 2), 48);
            PTUITheme.DrawCard(g, dayHeaderRect, radius: 8, customBg: Color.FromArgb(22, 26, 38), customBorder: Color.FromArgb(42, 52, 74));

            TextRenderer.DrawText(g, $"LỊCH HUẤN LUYỆN NGÀY: THỨ NĂM, {targetDay:dd/MM/yyyy}{dayDesc}", PTUITheme.FontBold,
                new Rectangle(dayHeaderRect.X + 16, dayHeaderRect.Y + 6, dayHeaderRect.Width - 32, 20), PTUITheme.PrimaryLime,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            TextRenderer.DrawText(g, "Ca trực cố định 08:00 - 16:00. Bấm vào ca tập để xem chi tiết giáo án bên phải.", PTUITheme.FontSmall,
                new Rectangle(dayHeaderRect.X + 16, dayHeaderRect.Y + 26, dayHeaderRect.Width - 32, 18), PTUITheme.TextMuted,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // 2. Danh sách 4 khung giờ trong ngày của tuần hiện tại
            string[] slots = new string[] { "07:00 – 08:30", "09:00 – 10:30", "14:00 – 15:30", "17:00 – 18:30" };
            int startY = 74;
            int availableH = rect.Height - startY - 20;
            int cardH = (availableH - (slots.Length - 1) * 12) / slots.Length;

            var currentWeekSessions = GetSessionsForWeek(_currentWeekOffset);

            for (int i = 0; i < slots.Length; i++)
            {
                int curY = startY + i * (cardH + 12);
                Rectangle cardRect = new Rectangle(pad, curY, rect.Width - (pad * 2), cardH);

                // Tìm ca tập tương ứng của Thứ 5 (DayIndex = 4 - Ngày 05/11) trong tuần đang xem
                var session = currentWeekSessions.FirstOrDefault(s => s.DayIndex == 4 && s.SlotIndex == i);
                _daySessionRects[cardRect] = session;

                bool isSelected = (_selectedSession == session && session != null);
                bool isHovered = (_hoveredSession == session && session != null);

                if (session != null)
                {
                    // Ca tập đã có hội viên đặt
                    Color cardBg = isSelected ? Color.FromArgb(36, 46, 24) : (isHovered ? Color.FromArgb(28, 36, 48) : Color.FromArgb(22, 26, 36));
                    Color cardBorder = isSelected ? PTUITheme.PrimaryLime : (isHovered ? Color.FromArgb(140, 180, 50) : Color.FromArgb(45, 55, 75));

                    PTUITheme.DrawCard(g, cardRect, radius: 10, customBg: cardBg, customBorder: cardBorder);

                    // Badge giờ bên trái
                    Rectangle timeBadge = new Rectangle(cardRect.X + 16, cardRect.Y + (cardRect.Height - 28) / 2, 110, 28);
                    PTUITheme.DrawBadge(g, slots[i], timeBadge, Color.FromArgb(32, 40, 56), PTUITheme.PrimaryLime, PTUITheme.FontSmallBold, 6);

                    // Avatar tròn viết tắt
                    Rectangle avRect = new Rectangle(timeBadge.Right + 16, cardRect.Y + (cardRect.Height - 38) / 2, 38, 38);
                    using (SolidBrush bAv = new SolidBrush(PTUITheme.PrimaryLime))
                    {
                        g.FillEllipse(bAv, avRect);
                    }
                    TextRenderer.DrawText(g, session.MemberInitials, PTUITheme.FontBold, avRect, Color.Black,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                    // Tên hội viên & tiến độ
                    Rectangle nameRect = new Rectangle(avRect.Right + 14, cardRect.Y + 12, cardRect.Width - avRect.Right - 160, 22);
                    TextRenderer.DrawText(g, $"{session.MemberName} ({session.MemberCode})", PTUITheme.FontBold, nameRect, PTUITheme.TextWhite,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

                    Rectangle descRect = new Rectangle(avRect.Right + 14, cardRect.Y + 34, cardRect.Width - avRect.Right - 160, 20);
                    TextRenderer.DrawText(g, $"{session.Location}  •  {session.ProgressText}", PTUITheme.FontSmall, descRect, PTUITheme.TextMuted,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

                    // Badge trạng thái bên phải
                    Rectangle stRect = new Rectangle(cardRect.Right - 130, cardRect.Y + (cardRect.Height - 26) / 2, 114, 26);
                    PTUITheme.DrawBadge(g, session.StatusBadge, stRect,
                        session.IsUrgent ? PTUITheme.LimeDark : Color.FromArgb(32, 40, 56),
                        session.IsUrgent ? PTUITheme.PrimaryLime : PTUITheme.Cyan,
                        PTUITheme.FontSmallBold, 6);
                }
                else
                {
                    // Ca trực / rảnh nhận lịch
                    PTUITheme.DrawCard(g, cardRect, radius: 10, customBg: Color.FromArgb(18, 20, 28), customBorder: Color.FromArgb(35, 42, 56));

                    // Badge giờ bên trái
                    Rectangle timeBadge = new Rectangle(cardRect.X + 16, cardRect.Y + (cardRect.Height - 28) / 2, 110, 28);
                    PTUITheme.DrawBadge(g, slots[i], timeBadge, Color.FromArgb(24, 28, 38), PTUITheme.TextMuted, PTUITheme.FontSmallBold, 6);

                    // Trạng thái Ca Rảnh
                    Rectangle freeRect = new Rectangle(timeBadge.Right + 16, cardRect.Y + 12, cardRect.Width - timeBadge.Right - 180, 22);
                    TextRenderer.DrawText(g, "Ca Trực Cố Định (Rảnh - Sẵn Sàng Nhận Lịch)", PTUITheme.FontBold, freeRect, Color.FromArgb(160, 170, 190),
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                    Rectangle subFree = new Rectangle(timeBadge.Right + 16, cardRect.Y + 34, cardRect.Width - timeBadge.Right - 180, 20);
                    TextRenderer.DrawText(g, "Hội viên có thể tự chọn khung giờ này để đăng ký tập cùng bạn.", PTUITheme.FontSmall, subFree, PTUITheme.TextMuted,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                    // Badge Trống
                    Rectangle freeBadge = new Rectangle(cardRect.Right - 130, cardRect.Y + (cardRect.Height - 26) / 2, 114, 26);
                    PTUITheme.DrawBadge(g, "CA TRỐNG", freeBadge, Color.FromArgb(24, 28, 38), Color.FromArgb(120, 130, 150), PTUITheme.FontSmallBold, 6);
                }
            }
        }

        private Rectangle GetCellRect(int dayIndex, int rowY, int tableLeft, int timeColWidth, int dayColWidth, int rowHeight)
        {
            int x = tableLeft + timeColWidth + (dayIndex - 1) * dayColWidth + 3;
            int w = dayColWidth - 6;
            int y = rowY + 5;
            int h = rowHeight - 10;
            return new Rectangle(x, y, w, h);
        }

        private void DrawScheduleCard(Graphics g, Rectangle rect, string memberName, string timeStr, bool isSelected, bool isHovered)
        {
            Color bg = isSelected ? Color.FromArgb(45, 60, 26) : (isHovered ? Color.FromArgb(35, 45, 30) : Color.FromArgb(26, 33, 25));
            Color border = isSelected ? PTUITheme.PrimaryLime : (isHovered ? Color.FromArgb(160, 210, 40) : Color.FromArgb(60, 80, 36));
            Color fg = PTUITheme.PrimaryLime;
            float borderWidth = isSelected ? 2f : 1f;

            using (SolidBrush b = new SolidBrush(bg))
            {
                PTUITheme.FillRoundedRectangle(g, b, rect, 6);
            }
            using (Pen p = new Pen(border, borderWidth))
            {
                PTUITheme.DrawRoundedRectangle(g, p, rect, 6);
            }

            // Tự động tinh chỉnh tên hiển thị theo độ rộng ô:
            string displayName = memberName;
            if (rect.Width < 85 && displayName.StartsWith("HV: "))
            {
                displayName = displayName.Substring(4);
            }

            Font nameFont = rect.Width < 80 ? new Font("Segoe UI", 7.5f, FontStyle.Bold) : PTUITheme.FontSmallBold;
            Font timeFont = rect.Width < 80 ? new Font("Segoe UI", 7f, FontStyle.Regular) : PTUITheme.FontSmall;

            Rectangle topText = new Rectangle(rect.X + 2, rect.Y + 4, rect.Width - 4, rect.Height / 2);
            TextRenderer.DrawText(g, displayName, nameFont, topText, fg,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            string displayTime = rect.Width < 80 ? timeStr.Replace(" – ", "-").Replace(" - ", "-") : timeStr;
            Rectangle btmText = new Rectangle(rect.X + 2, rect.Y + rect.Height / 2, rect.Width - 4, rect.Height / 2 - 4);
            TextRenderer.DrawText(g, displayTime, timeFont, btmText, isSelected ? Color.White : PTUITheme.TextMuted,
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
            int pad = 16;
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
            lblTodaySessionsHeader.Size = new Size(innerW, 18);
            lblTodaySessionsHeader.AutoEllipsis = true;
            pnlTodaySessionButtons.Location = new Point(pad, 452);
            pnlTodaySessionButtons.Size = new Size(innerW, Math.Max(80, h - 465));
            pnlTodaySessionButtons.AutoScroll = false;

            foreach (Control c in pnlTodaySessionButtons.Controls)
            {
                c.Width = pnlTodaySessionButtons.ClientSize.Width;
            }
        }

        private void UpdateTodaySessionsButtons()
        {
            pnlTodaySessionButtons.Controls.Clear();
            pnlTodaySessionButtons.SuspendLayout();
            pnlTodaySessionButtons.AutoScroll = false;

            var currentWeekSessions = GetSessionsForWeek(_currentWeekOffset);
            List<Session1On1> displayList;

            if (_currentWeekOffset == 0)
            {
                lblTodaySessionsHeader.Text = "CÁC BUỔI TẬP HÔM NAY (ƯU TIÊN THEO KHUNG GIỜ):";
                displayList = currentWeekSessions.Where(s => s.IsToday).OrderBy(s => s.StartTime).ToList();
                if (displayList.Count == 0)
                    displayList = currentWeekSessions.Take(4).ToList();
            }
            else
            {
                int weekNumber = 45 + _currentWeekOffset;
                lblTodaySessionsHeader.Text = $"CÁC BUỔI TẬP TUẦN {weekNumber} (CHỌN ĐỂ XEM CHI TIẾT):";
                displayList = currentWeekSessions.OrderBy(s => s.DayIndex).ThenBy(s => s.StartTime).Take(5).ToList();
            }

            // Cập nhật text nút điểm danh tương ứng theo trạng thái tuần và ca tập
            if (_selectedSession != null && _selectedSession.StatusBadge == "HOÀN THÀNH")
            {
                btnCheckIn.Text = "Đã Hoàn Thành Buổi Tập";
                btnCheckIn.BackColor = Color.FromArgb(28, 38, 26);
                btnCheckIn.ForeColor = PTUITheme.PrimaryLime;
                btnCheckIn.FlatAppearance.BorderColor = PTUITheme.PrimaryLime;
                btnCheckIn.Enabled = true;
            }
            else if (_currentWeekOffset == 0)
            {
                btnCheckIn.Text = "Điểm Danh Hoàn Thành Buổi";
                btnCheckIn.BackColor = PTUITheme.PrimaryLime;
                btnCheckIn.ForeColor = Color.Black;
                btnCheckIn.FlatAppearance.BorderColor = PTUITheme.PrimaryLime;
                btnCheckIn.Enabled = true;
            }
            else if (_currentWeekOffset > 0)
            {
                btnCheckIn.Text = "Xác Nhận Giữ Ca & Chuẩn Bị Giáo Án";
                btnCheckIn.BackColor = PTUITheme.PrimaryLime;
                btnCheckIn.ForeColor = Color.Black;
                btnCheckIn.Enabled = true;
            }
            else
            {
                btnCheckIn.Text = "Xem Lại Nhật Ký Buổi Tập Đã Dạy";
                btnCheckIn.BackColor = Color.FromArgb(28, 38, 26);
                btnCheckIn.ForeColor = PTUITheme.PrimaryLime;
                btnCheckIn.Enabled = true;
            }

            for (int i = 0; i < displayList.Count; i++)
            {
                var s = displayList[i];
                bool isSelected = (_selectedSession == s);

                string badgeTag;
                if (s.StatusBadge == "HOÀN THÀNH")
                    badgeTag = "Đã xong";
                else if (s.StatusBadge == "ĐANG TẬP")
                    badgeTag = "Đang tập";
                else if (s.IsUrgent)
                    badgeTag = "Sắp tới";
                else if (_currentWeekOffset == 0)
                    badgeTag = "Hôm nay";
                else
                    badgeTag = s.DayName;

                Button btn = new Button
                {
                    Width = pnlTodaySessionButtons.ClientSize.Width,
                    Height = 44,
                    Margin = new Padding(0, 0, 0, 6),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = isSelected ? Color.FromArgb(38, 48, 28) : Color.FromArgb(24, 28, 38),
                    ForeColor = isSelected ? PTUITheme.PrimaryLime : PTUITheme.TextWhite,
                    Font = PTUITheme.FontSmallBold,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Cursor = Cursors.Hand,
                    UseMnemonic = false,
                    AutoEllipsis = true,
                    Text = $"  {s.TimeText}  •  {s.MemberName}  ({badgeTag})"
                };

                btn.FlatAppearance.BorderColor = isSelected ? PTUITheme.PrimaryLime : Color.FromArgb(42, 50, 68);
                btn.FlatAppearance.BorderSize = 1;

                var currentSession = s;
                btn.Click += (s2, e2) =>
                {
                    _selectedSession = currentSession;
                    UpdateTodaySessionsButtons();
                    pnlRightDetails.Invalidate();
                    pnlScheduleGrid.Invalidate();
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

            int pad = 16;

            // Title Pill: Linh hoạt theo tuần
            string titleBadgeText = (_currentWeekOffset == 0) ? "BUỔI HUẤN LUYỆN 1-1" : (_currentWeekOffset > 0 ? $"LỊCH TUẦN {45 + _currentWeekOffset}" : $"LỊCH SỬ TUẦN {45 + _currentWeekOffset}");

            string noteText;
            if (_currentWeekOffset == 0)
            {
                if (_selectedSession != null && _selectedSession.StatusBadge == "HOÀN THÀNH")
                    noteText = "Ca gần nhất (Đã xong)";
                else if (_selectedSession != null && _selectedSession.IsUrgent)
                    noteText = "Sắp diễn ra";
                else
                    noteText = "Ưu tiên theo giờ";
            }
            else if (_currentWeekOffset > 0)
            {
                noteText = "Kế hoạch tuần tới";
            }
            else
            {
                noteText = "Đã hoàn thành";
            }

            Font titleBadgeFont = new Font("Segoe UI", 8.25f, FontStyle.Bold);
            Size titleTextSize = TextRenderer.MeasureText(g, titleBadgeText, titleBadgeFont, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
            int pillWidth = titleTextSize.Width + 16; // đệm gọn gàng
            Rectangle pillRect = new Rectangle(pad, 16, pillWidth, 26);
            PTUITheme.DrawBadge(g, titleBadgeText, pillRect, PTUITheme.PrimaryLime, Color.Black, titleBadgeFont, 6);

            // Priority notice text: Tự động vừa vặn, không bị cắt xén
            Font noteFont = new Font("Segoe UI", 7.75f, FontStyle.Regular);
            int noteX = pillRect.Right + 8;
            int noteWidth = rect.Width - noteX - pad;
            Rectangle noteRect = new Rectangle(noteX, 16, noteWidth, 26);
            TextRenderer.DrawText(g, noteText, noteFont, noteRect, PTUITheme.PrimaryLime,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            if (_selectedSession == null) return;

            // Member Info Card
            Rectangle memCardRect = new Rectangle(pad, 52, rect.Width - (pad * 2), 68);
            PTUITheme.DrawCard(g, memCardRect, radius: 10, customBg: Color.FromArgb(28, 32, 44), customBorder: Color.FromArgb(42, 48, 64));

            // Avatar initial
            Rectangle avatarRect = new Rectangle(memCardRect.X + 10, memCardRect.Y + 14, 40, 40);
            using (SolidBrush b = new SolidBrush(PTUITheme.PrimaryLime))
            {
                PTUITheme.FillRoundedRectangle(g, b, avatarRect, 8);
            }
            TextRenderer.DrawText(g, _selectedSession.MemberInitials, PTUITheme.FontBold, avatarRect, Color.Black,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // Badge on top-right of member card: Tự động đo độ rộng theo nội dung badge
            Font badgeFont = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            Size badgeTextSize = TextRenderer.MeasureText(g, _selectedSession.StatusBadge, badgeFont, Size.Empty, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
            int badgeW = Math.Max(78, badgeTextSize.Width + 14);
            Rectangle badgeRect = new Rectangle(memCardRect.Right - badgeW - 10, memCardRect.Y + 20, badgeW, 26);

            Color badgeBg = _selectedSession.IsUrgent
                ? PTUITheme.LimeDark
                : (_selectedSession.StatusBadge == "HOÀN THÀNH" ? Color.FromArgb(32, 42, 30) : Color.FromArgb(32, 38, 50));
            Color badgeFg = _selectedSession.IsUrgent
                ? PTUITheme.PrimaryLime
                : (_selectedSession.StatusBadge == "HOÀN THÀNH" ? PTUITheme.PrimaryLime : PTUITheme.Cyan);

            PTUITheme.DrawBadge(g, _selectedSession.StatusBadge, badgeRect, badgeBg, badgeFg, badgeFont, 6);

            // Name & Progress: Tận dụng toàn bộ khoảng trống từ avatar tới mép badge
            int nameX = avatarRect.Right + 10;
            int nameWidth = badgeRect.Left - nameX - 8;
            Rectangle nameRect = new Rectangle(nameX, memCardRect.Y + 12, nameWidth, 22);
            TextRenderer.DrawText(g, _selectedSession.MemberName, PTUITheme.FontBold, nameRect, PTUITheme.TextWhite,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            Rectangle progRect = new Rectangle(nameX, memCardRect.Y + 34, nameWidth, 20);
            TextRenderer.DrawText(g, _selectedSession.ProgressText, PTUITheme.FontSmall, progRect, PTUITheme.PrimaryLime,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            // Section: THỜI GIAN & ĐỊA ĐIỂM
            int yLocation = 130;
            Rectangle secLocTitle = new Rectangle(pad, yLocation, rect.Width - (pad * 2), 18);
            TextRenderer.DrawText(g, "THỜI GIAN & ĐỊA ĐIỂM", PTUITheme.FontSmallBold, secLocTitle, PTUITheme.TextMuted,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            DateTime sessionDate = GetDateForSession(_selectedSession);
            string timeTag = (_selectedSession.WeekOffset == 0 && _selectedSession.IsToday)
                ? $"{_selectedSession.TimeText} ({_selectedSession.DayName} - Hôm nay)"
                : $"{_selectedSession.TimeText} ({_selectedSession.DayName}, {sessionDate:dd/MM/yyyy})";

            Rectangle timeValRect = new Rectangle(pad, yLocation + 20, rect.Width - (pad * 2), 22);
            TextRenderer.DrawText(g, timeTag, PTUITheme.FontBold, timeValRect, PTUITheme.TextWhite,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            Rectangle locValRect = new Rectangle(pad, yLocation + 42, rect.Width - (pad * 2), 20);
            TextRenderer.DrawText(g, _selectedSession.Location, PTUITheme.FontSmall, locValRect, PTUITheme.Cyan,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

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
                Rectangle exRect = new Rectangle(lessonCard.X + 10, lineY, lessonCard.Width - 20, 24);
                TextRenderer.DrawText(g, ex, PTUITheme.FontSmall, exRect, PTUITheme.TextWhite,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
                lineY += 28;
            }
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            if (_selectedSession == null) return;
            string extra;
            if (_selectedSession.StatusBadge == "HOÀN THÀNH")
            {
                extra = "Buổi tập đã kết thúc và hoàn thành hôm nay!\nNhật ký giáo án và chỉ số tập luyện đã được lưu trữ thành công.";
            }
            else if (_selectedSession.WeekOffset == 0)
            {
                extra = "Điểm danh hoàn thành buổi tập thành công!";
            }
            else if (_selectedSession.WeekOffset > 0)
            {
                extra = $"Lịch tập thuộc Tuần {45 + _selectedSession.WeekOffset}. Đã xác nhận giữ chỗ và chuẩn bị giáo án cho hội viên!";
            }
            else
            {
                extra = $"Buổi tập thuộc Tuần {45 + _selectedSession.WeekOffset}. Đã kiểm tra lại nhật ký buổi tập!";
            }

            MessageBox.Show($"{extra}\nHội viên: [{_selectedSession.MemberName}]\nKhung giờ: {_selectedSession.TimeText} ({_selectedSession.DayName})\nĐịa điểm: {_selectedSession.Location}",
                "Thông Báo Buổi Tập", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditSchedule_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Mở form sửa lịch/đổi giờ cho buổi huấn luyện của [{_selectedSession?.MemberName}].\nKhung giờ hiện tại: {_selectedSession?.TimeText}",
                "Sửa Lịch Buổi Tập", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancelSchedule_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show($"Bạn có chắc chắn muốn hủy ca hẹn với [{_selectedSession?.MemberName}] vào {_selectedSession?.DayName} ({_selectedSession?.TimeText}) không?",
                "Xác Nhận Hủy Ca", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                MessageBox.Show("Ca hẹn đã được hủy thành công. Hệ thống đã mở lại khung giờ trống cho hội viên khác đăng ký.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
