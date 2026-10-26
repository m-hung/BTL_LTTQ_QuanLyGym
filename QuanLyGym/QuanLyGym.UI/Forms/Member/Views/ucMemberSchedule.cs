using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using QuanLyGym.UI.Utils;

namespace QuanLyGym.UI.Forms.Member.Views
{
    public class ScheduleItem
    {
        public DateTime Date { get; set; }
        public string DateDisplay => Date.ToString("dd/MM/yyyy");
        public string DayOfWeekVN
        {
            get
            {
                return Date.DayOfWeek switch
                {
                    DayOfWeek.Monday => "Thứ 2",
                    DayOfWeek.Tuesday => "Thứ 3",
                    DayOfWeek.Wednesday => "Thứ 4",
                    DayOfWeek.Thursday => "Thứ 5",
                    DayOfWeek.Friday => "Thứ 6",
                    DayOfWeek.Saturday => "Thứ 7",
                    DayOfWeek.Sunday => "Chủ nhật",
                    _ => ""
                };
            }
        }
        public string TimeRange { get; set; } = "";
        public string WorkoutType { get; set; } = "";
        public string TrainerName { get; set; } = "";
        public string Room { get; set; } = "";
        public string Source { get; set; } = ""; // "Lộ trình PT xếp" hoặc "Hội viên tự đặt"
        public string Status { get; set; } = ""; // "Đã xác nhận", "Chờ HLV duyệt", "Hoàn thành"
        public string Note { get; set; } = "";
    }

    public class ucMemberSchedule : UserControl
    {
        // Internal holder for each of the 42 calendar grid cells (reused, never destroyed)
        private class DayCellView
        {
            public Panel Panel = null!;
            public Label LblDayNum = null!;
            public Label[] EventLabels = new Label[3];
            public DateTime? Date;
            public List<ScheduleItem> DaySchedules = new List<ScheduleItem>();
        }

        // Data Store
        private readonly List<ScheduleItem> _schedules = new List<ScheduleItem>();
        private DateTime _currentMonth = new DateTime(2026, 10, 1);
        private string _selectedFilter = "Tất cả";
        private bool _isCalendarMode = false;

        // UI Controls - Left View
        private DataGridView dgvSchedule = null!;
        private Panel pnlTableView = null!;
        private Panel pnlCalendarView = null!;
        private Panel pnlViewContainer = null!;
        private TableLayoutPanel pnlCalendarGrid = null!;
        private readonly DayCellView[,] _gridCells = new DayCellView[6, 7];
        private Label lblMonthYear = null!;
        private Label lblSelectedDayDetail = null!;
        private Label lblListTitle = null!;
        private Label lblTotalStats = null!;
        private Button btnModeTable = null!;
        private Button btnModeCalendar = null!;
        private FlowLayoutPanel pnlFilterButtons = null!;

        // UI Controls - Right Booking Form
        private DateTimePicker dtpDate = null!;
        private ComboBox cboTime = null!;
        private ComboBox cboType = null!;
        private ComboBox cboTrainer = null!;
        private ComboBox cboRoom = null!;
        private TextBox txtNote = null!;
        private Button btnSubmit = null!;

        public ucMemberSchedule()
        {
            EnableDoubleBuffer(this);
            SeedInitialSchedules();
            InitializeComponent();
            RefreshAllViews();
        }

        private static void EnableDoubleBuffer(Control c)
        {
            try
            {
                typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.SetValue(c, true, null);
            }
            catch { }
        }

        private void SeedInitialSchedules()
        {
            // 1. Lịch do PT xếp (Lộ trình cố định - Tự động hiển thị "Đã xác nhận")
            _schedules.Add(new ScheduleItem
            {
                Date = new DateTime(2026, 10, 26),
                TimeRange = "18:00 - 19:30",
                WorkoutType = "Gym Thể hình (Tăng cơ)",
                TrainerName = "HLV Nguyễn Văn Nam",
                Room = "PT-01",
                Source = "Lộ trình PT xếp",
                Status = "Đã xác nhận",
                Note = "Giáo án tăng khối lượng cơ ngực & vai"
            });
            _schedules.Add(new ScheduleItem
            {
                Date = new DateTime(2026, 10, 28),
                TimeRange = "17:30 - 19:00",
                WorkoutType = "Cardio & HIIT đốt mỡ",
                TrainerName = "HLV Nguyễn Văn Nam",
                Room = "Room 02",
                Source = "Lộ trình PT xếp",
                Status = "Đã xác nhận",
                Note = "Cardio nhịp tim vùng 2 kết hợp giãn cơ sâu"
            });
            _schedules.Add(new ScheduleItem
            {
                Date = new DateTime(2026, 11, 1),
                TimeRange = "08:30 - 10:00",
                WorkoutType = "Yoga & Giãn cơ phục hồi",
                TrainerName = "HLV Lê Thu Hà",
                Room = "Studio A",
                Source = "Lộ trình PT xếp",
                Status = "Đã xác nhận",
                Note = "Lớp nhóm phục hồi cơ khớp cuối tuần"
            });
            _schedules.Add(new ScheduleItem
            {
                Date = new DateTime(2026, 10, 23),
                TimeRange = "18:00 - 19:30",
                WorkoutType = "Chân - Mông (Leg Day)",
                TrainerName = "HLV Nguyễn Văn Nam",
                Room = "PT-01",
                Source = "Lộ trình PT xếp",
                Status = "Hoàn thành",
                Note = "Squat 80kg x 4 hiệp đạt chuẩn kỹ thuật"
            });
            _schedules.Add(new ScheduleItem
            {
                Date = new DateTime(2026, 10, 19),
                TimeRange = "18:00 - 19:30",
                WorkoutType = "Lưng - Xô & Tay trước",
                TrainerName = "HLV Nguyễn Văn Nam",
                Room = "PT-01",
                Source = "Lộ trình PT xếp",
                Status = "Hoàn thành",
                Note = "Deadlift & Pull-up"
            });

            // 2. Lịch do Hội viên chủ động đặt thêm (Đi qua luồng duyệt: "Chờ HLV duyệt")
            _schedules.Add(new ScheduleItem
            {
                Date = new DateTime(2026, 10, 30),
                TimeRange = "18:00 - 19:30",
                WorkoutType = "Ngực & Tay sau tăng cường",
                TrainerName = "HLV Nguyễn Văn Nam",
                Room = "PT-01",
                Source = "Hội viên tự đặt",
                Status = "Chờ HLV duyệt",
                Note = "Hội viên xin tập thêm trước chuyến công tác"
            });
            _schedules.Add(new ScheduleItem
            {
                Date = new DateTime(2026, 11, 4),
                TimeRange = "17:30 - 19:00",
                WorkoutType = "Boxing & Kickfit đối kháng",
                TrainerName = "HLV Trần Minh Đức",
                Room = "PT-02",
                Source = "Hội viên tự đặt",
                Status = "Chờ HLV duyệt",
                Note = "Đăng ký buổi bổ trợ xả stress"
            });
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = UITheme.Background;
            this.Dock = DockStyle.Fill;
            this.AutoScroll = true;

            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Top,
                Height = 800,
                Padding = new Padding(24, 20, 24, 30),
                BackColor = UITheme.Background
            };

            // Header Section
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 56 };
            Label lblTitle = new Label
            {
                Text = "📅 LỊCH TẬP & ĐẶT LỊCH HUẤN LUYỆN VIÊN (PT)",
                Font = UITheme.FontTitle,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(0, 4)
            };
            Label lblSubtitle = new Label
            {
                Text = "Xem lộ trình tập do PT xếp cố định hoặc chủ động đặt thêm ca tập cá nhân",
                Font = UITheme.FontRegular,
                ForeColor = UITheme.TextMuted,
                AutoSize = true,
                Location = new Point(0, 32)
            };
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // Two Column Layout
            TableLayoutPanel pnlCols = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Height = 690,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 10, 0, 10)
            };
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64f));
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36f));
            pnlCols.RowStyles.Clear();
            pnlCols.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            // Left: Schedule List Card with Table / Calendar toggle
            RoundedPanel cardLeft = CreateScheduleLeftCard();
            cardLeft.Dock = DockStyle.Fill;
            cardLeft.Margin = new Padding(0, 0, 10, 0);

            // Right: Booking Form Card
            RoundedPanel cardRight = CreateBookingFormCard();
            cardRight.Dock = DockStyle.Fill;
            cardRight.Margin = new Padding(10, 0, 0, 0);

            pnlCols.Controls.Add(cardLeft, 0, 0);
            pnlCols.Controls.Add(cardRight, 1, 0);

            pnlMain.Controls.Add(pnlCols);
            pnlMain.Controls.Add(pnlHeader);

            this.Controls.Add(pnlMain);
            this.ResumeLayout(true);
        }

        #region Left Side: Schedule Views (Table & Calendar)

        private RoundedPanel CreateScheduleLeftCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 670,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(20)
            };

            // Top Toolbar: Title + View Mode Switcher
            Panel pnlTopBar = new Panel { Dock = DockStyle.Top, Height = 48 };

            lblListTitle = new Label
            {
                Text = "DANH SÁCH LỊCH TẬP ĐÃ LÊN LỊCH",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(0, 12)
            };

            // Segmented Switcher Buttons
            Panel pnlModeSwitcher = new Panel
            {
                Dock = DockStyle.Right,
                Width = 240,
                Height = 36,
                Location = new Point(card.Width - 260, 6)
            };

            btnModeTable = new Button
            {
                Text = "📋 Dạng Bảng",
                Font = UITheme.FontSmallBold,
                Width = 115,
                Height = 34,
                Location = new Point(0, 2),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = UITheme.Primary,
                ForeColor = Color.White
            };
            btnModeTable.FlatAppearance.BorderSize = 0;

            btnModeCalendar = new Button
            {
                Text = "📅 Lịch Tháng",
                Font = UITheme.FontSmallBold,
                Width = 115,
                Height = 34,
                Location = new Point(120, 2),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = UITheme.TextRegular
            };
            btnModeCalendar.FlatAppearance.BorderSize = 0;

            btnModeTable.Click += (s, e) => SwitchViewMode(false);
            btnModeCalendar.Click += (s, e) => SwitchViewMode(true);

            pnlModeSwitcher.Controls.Add(btnModeTable);
            pnlModeSwitcher.Controls.Add(btnModeCalendar);

            pnlTopBar.Controls.Add(lblListTitle);
            pnlTopBar.Controls.Add(pnlModeSwitcher);

            // Filter Tabs Row
            pnlFilterButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 38,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 2, 0, 4)
            };

            string[] filterNames = {
                "Tất cả",
                "🏋️ Lộ trình PT xếp",
                "🙋 Hội viên tự đặt",
                "⏳ Chờ HLV duyệt",
                "✅ Đã xác nhận"
            };

            foreach (var f in filterNames)
            {
                Button btnF = new Button
                {
                    Text = f,
                    Font = UITheme.FontSmallBold,
                    Height = 30,
                    AutoSize = true,
                    BackColor = f == _selectedFilter ? UITheme.PrimaryLight : Color.FromArgb(248, 250, 252),
                    ForeColor = f == _selectedFilter ? UITheme.Primary : UITheme.TextMuted,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(0, 0, 8, 0)
                };
                btnF.FlatAppearance.BorderSize = 0;
                btnF.Click += (s, e) =>
                {
                    _selectedFilter = f;
                    foreach (Control c in pnlFilterButtons.Controls)
                    {
                        if (c is Button b)
                        {
                            bool isActive = b.Text == _selectedFilter;
                            b.BackColor = isActive ? UITheme.PrimaryLight : Color.FromArgb(248, 250, 252);
                            b.ForeColor = isActive ? UITheme.Primary : UITheme.TextMuted;
                        }
                    }
                    RefreshAllViews();
                };
                pnlFilterButtons.Controls.Add(btnF);
            }

            // Container for views
            pnlViewContainer = new Panel { Dock = DockStyle.Fill };
            EnableDoubleBuffer(pnlViewContainer);

            // 1. Table View Setup
            pnlTableView = BuildTableView();
            pnlTableView.Dock = DockStyle.Fill;
            EnableDoubleBuffer(pnlTableView);

            // 2. Calendar View Setup
            pnlCalendarView = BuildCalendarView();
            pnlCalendarView.Dock = DockStyle.Fill;
            EnableDoubleBuffer(pnlCalendarView);
            pnlCalendarView.Visible = false;

            pnlViewContainer.Controls.Add(pnlCalendarView);
            pnlViewContainer.Controls.Add(pnlTableView);

            card.Controls.Add(pnlViewContainer);
            card.Controls.Add(pnlFilterButtons);
            card.Controls.Add(pnlTopBar);

            return card;
        }

        private void SwitchViewMode(bool calendarMode)
        {
            _isCalendarMode = calendarMode;

            pnlViewContainer.SuspendLayout();

            if (_isCalendarMode)
            {
                pnlTableView.Visible = false;
                pnlCalendarView.Visible = true;
                lblListTitle.Text = "📅 LỊCH TẬP THEO THÁNG (CALENDAR VIEW)";
                btnModeCalendar.BackColor = UITheme.Primary;
                btnModeCalendar.ForeColor = Color.White;
                btnModeTable.BackColor = Color.FromArgb(241, 245, 249);
                btnModeTable.ForeColor = UITheme.TextRegular;
                RenderCalendarMonth();
            }
            else
            {
                pnlCalendarView.Visible = false;
                pnlTableView.Visible = true;
                lblListTitle.Text = "DANH SÁCH LỊCH TẬP ĐÃ LÊN LỊCH";
                btnModeTable.BackColor = UITheme.Primary;
                btnModeTable.ForeColor = Color.White;
                btnModeCalendar.BackColor = Color.FromArgb(241, 245, 249);
                btnModeCalendar.ForeColor = UITheme.TextRegular;
                RenderTableData();
            }

            pnlViewContainer.ResumeLayout(true);
        }

        private Panel BuildTableView()
        {
            Panel pnl = new Panel { Dock = DockStyle.Fill };

            dgvSchedule = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgvSchedule);

            var colDate = new DataGridViewTextBoxColumn { Name = "Date", HeaderText = "Ngày tập", Width = 110, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colTime = new DataGridViewTextBoxColumn { Name = "Time", HeaderText = "Khung giờ", Width = 105, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colType = new DataGridViewTextBoxColumn { Name = "Type", HeaderText = "Bộ môn & Mục tiêu", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 140 };
            var colTrainer = new DataGridViewTextBoxColumn { Name = "Trainer", HeaderText = "HLV phụ trách", Width = 145, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colSource = new DataGridViewTextBoxColumn { Name = "Source", HeaderText = "Nguồn gốc lịch", Width = 135, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colRoom = new DataGridViewTextBoxColumn { Name = "Room", HeaderText = "Phòng", Width = 75, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colStatus = new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Trạng thái", Width = 125, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };

            dgvSchedule.Columns.AddRange(colDate, colTime, colType, colTrainer, colSource, colRoom, colStatus);

            dgvSchedule.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0 || e.Value == null) return;

                string colName = dgvSchedule.Columns[e.ColumnIndex].Name;

                if (colName == "Source")
                {
                    string source = e.Value.ToString() ?? "";
                    if (source.Contains("Lộ trình PT"))
                    {
                        e.CellStyle.ForeColor = UITheme.Primary;
                        e.CellStyle.Font = UITheme.FontBold;
                    }
                    else
                    {
                        e.CellStyle.ForeColor = UITheme.Secondary;
                        e.CellStyle.Font = UITheme.FontBold;
                    }
                }
                else if (colName == "Status")
                {
                    string status = e.Value.ToString() ?? "";
                    if (status.Contains("Đã xác nhận"))
                    {
                        e.CellStyle.ForeColor = UITheme.Success;
                        e.CellStyle.Font = UITheme.FontBold;
                    }
                    else if (status.Contains("Chờ"))
                    {
                        e.CellStyle.ForeColor = UITheme.Warning;
                        e.CellStyle.Font = UITheme.FontBold;
                    }
                    else if (status.Contains("Hoàn thành"))
                    {
                        e.CellStyle.ForeColor = UITheme.TextMuted;
                    }
                }
            };

            // Bottom action & Stats panel
            Panel pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(0, 8, 0, 0) };

            Button btnCancel = new Button
            {
                Text = "❌ Hủy buổi tập đã chọn",
                Font = UITheme.FontBold,
                Height = 36,
                Width = 200,
                BackColor = UITheme.DangerLight,
                ForeColor = UITheme.Danger,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Left
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;

            lblTotalStats = new Label
            {
                Text = "",
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.TextMuted,
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = true
            };

            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(lblTotalStats);

            pnl.Controls.Add(dgvSchedule);
            pnl.Controls.Add(pnlBottom);
            return pnl;
        }

        private Panel BuildCalendarView()
        {
            Panel pnl = new Panel { Dock = DockStyle.Fill };

            // 1. Month Navigation & Legend Bar
            Panel pnlMonthNav = new Panel { Dock = DockStyle.Top, Height = 40 };

            Button btnPrev = new Button
            {
                Text = "◀ Tháng trước",
                Font = UITheme.FontSmallBold,
                Width = 110,
                Height = 32,
                Location = new Point(0, 4),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = UITheme.TextRegular
            };
            btnPrev.FlatAppearance.BorderSize = 0;
            btnPrev.Click += (s, e) =>
            {
                _currentMonth = _currentMonth.AddMonths(-1);
                RenderCalendarMonth();
            };

            lblMonthYear = new Label
            {
                Text = "Tháng 10 / 2026",
                Font = UITheme.FontSubtitle,
                ForeColor = UITheme.Primary,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(120, 6),
                Width = 170
            };

            Button btnNext = new Button
            {
                Text = "Tháng sau ▶",
                Font = UITheme.FontSmallBold,
                Width = 110,
                Height = 32,
                Location = new Point(300, 4),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = UITheme.TextRegular
            };
            btnNext.FlatAppearance.BorderSize = 0;
            btnNext.Click += (s, e) =>
            {
                _currentMonth = _currentMonth.AddMonths(1);
                RenderCalendarMonth();
            };

            // Legend on right
            Label lblLegend = new Label
            {
                Text = "🟣 PT xếp (Đã chốt)   |   🟠 Hội viên đặt (Chờ duyệt)",
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.TextMuted,
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = true
            };

            pnlMonthNav.Controls.Add(btnPrev);
            pnlMonthNav.Controls.Add(lblMonthYear);
            pnlMonthNav.Controls.Add(btnNext);
            pnlMonthNav.Controls.Add(lblLegend);

            // 2. Day-of-week Header
            TableLayoutPanel pnlWeekDays = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 28,
                ColumnCount = 7,
                RowCount = 1,
                BackColor = Color.FromArgb(248, 250, 252)
            };
            EnableDoubleBuffer(pnlWeekDays);
            for (int i = 0; i < 7; i++)
                pnlWeekDays.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 7f));
            pnlWeekDays.RowStyles.Clear();
            pnlWeekDays.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            string[] dayHeaders = { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "Chủ nhật" };
            for (int i = 0; i < 7; i++)
            {
                Label lblDay = new Label
                {
                    Text = dayHeaders[i],
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = UITheme.FontSmallBold,
                    ForeColor = (i == 6) ? UITheme.Danger : UITheme.TextRegular
                };
                pnlWeekDays.Controls.Add(lblDay, i, 0);
            }

            // 3. Pre-create Fixed Calendar Grid Panel (7 cols x 6 rows = 42 cells created once)
            pnlCalendarGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 7,
                RowCount = 6,
                BackColor = Color.White,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };
            EnableDoubleBuffer(pnlCalendarGrid);

            for (int i = 0; i < 7; i++)
                pnlCalendarGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 7f));
            for (int i = 0; i < 6; i++)
                pnlCalendarGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / 6f));

            pnlCalendarGrid.SuspendLayout();

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    Panel cellPanel = new Panel
                    {
                        Dock = DockStyle.Fill,
                        BackColor = Color.White,
                        Margin = new Padding(0),
                        Padding = new Padding(2)
                    };
                    EnableDoubleBuffer(cellPanel);

                    Label lblDayNum = new Label
                    {
                        Text = "",
                        Font = UITheme.FontSmallBold,
                        ForeColor = (col == 6) ? UITheme.Danger : UITheme.TextDark,
                        Dock = DockStyle.Top,
                        Height = 16,
                        TextAlign = ContentAlignment.TopLeft
                    };
                    cellPanel.Controls.Add(lblDayNum);

                    DayCellView cellView = new DayCellView
                    {
                        Panel = cellPanel,
                        LblDayNum = lblDayNum
                    };

                    // Pre-create up to 3 event label slots in each cell
                    for (int evIdx = 2; evIdx >= 0; evIdx--)
                    {
                        Label lblEvent = new Label
                        {
                            Dock = DockStyle.Top,
                            Height = 18,
                            AutoEllipsis = true,
                            Margin = new Padding(0, 1, 0, 1),
                            Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                            Visible = false,
                            Cursor = Cursors.Hand
                        };

                        int capturedIndex = evIdx;
                        lblEvent.Click += (s, e) =>
                        {
                            if (cellView.Date.HasValue && capturedIndex < cellView.DaySchedules.Count)
                            {
                                ShowDayScheduleDetail(cellView.Date.Value, cellView.DaySchedules[capturedIndex]);
                            }
                        };

                        cellView.EventLabels[evIdx] = lblEvent;
                        cellPanel.Controls.Add(lblEvent);
                    }

                    cellPanel.Click += (s, e) => HandleDayCellClick(cellView);
                    lblDayNum.Click += (s, e) => HandleDayCellClick(cellView);

                    _gridCells[row, col] = cellView;
                    pnlCalendarGrid.Controls.Add(cellPanel, col, row);
                }
            }

            pnlCalendarGrid.ResumeLayout(false);

            // 4. Bottom Selected Day Detail Panel
            Panel pnlDetail = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 62,
                BackColor = UITheme.PrimaryLight,
                Padding = new Padding(12, 8, 12, 8)
            };

            lblSelectedDayDetail = new Label
            {
                Dock = DockStyle.Fill,
                Text = "💡 Bấm vào một ngày trên lịch để xem nhanh các buổi tập chi tiết.",
                Font = UITheme.FontRegular,
                ForeColor = UITheme.TextDark,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlDetail.Controls.Add(lblSelectedDayDetail);

            pnl.Controls.Add(pnlCalendarGrid);
            pnl.Controls.Add(pnlDetail);
            pnl.Controls.Add(pnlWeekDays);
            pnl.Controls.Add(pnlMonthNav);

            return pnl;
        }

        private void HandleDayCellClick(DayCellView cellView)
        {
            if (!cellView.Date.HasValue) return;

            DateTime cellDate = cellView.Date.Value;
            if (cellView.DaySchedules.Count > 0)
            {
                ShowDayScheduleDetail(cellDate, cellView.DaySchedules[0]);
            }
            else
            {
                lblSelectedDayDetail.Text = $"📅 Ngày {cellDate:dd/MM/yyyy}: Không có buổi tập nào.";
            }
        }

        private void RenderTableData()
        {
            if (dgvSchedule == null) return;
            dgvSchedule.Rows.Clear();

            var filtered = GetFilteredSchedules();

            foreach (var item in filtered)
            {
                string dateCol = $"{item.DayOfWeekVN}, {item.Date:dd/MM}";
                dgvSchedule.Rows.Add(
                    dateCol,
                    item.TimeRange,
                    item.WorkoutType,
                    item.TrainerName,
                    item.Source,
                    item.Room,
                    item.Status
                );
            }

            int countPT = _schedules.Count(x => x.Source == "Lộ trình PT xếp");
            int countMember = _schedules.Count(x => x.Source == "Hội viên tự đặt");
            int countPending = _schedules.Count(x => x.Status == "Chờ HLV duyệt");

            if (lblTotalStats != null)
            {
                lblTotalStats.Text = $"📊 Tổng: {_schedules.Count} buổi  |  🏋️ PT xếp: {countPT}  |  🙋 Tự đặt: {countMember}  |  ⏳ Chờ duyệt: {countPending}";
            }
        }

        /// <summary>
        /// Renders Calendar month instantaneously with ZERO flicker/lag:
        /// Reuses the existing 42 cells, does NOT clear or recreate controls.
        /// </summary>
        private void RenderCalendarMonth()
        {
            if (pnlCalendarGrid == null) return;

            lblMonthYear.Text = $"Tháng {_currentMonth:MM / yyyy}";

            DateTime firstDayOfMonth = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(_currentMonth.Year, _currentMonth.Month);

            // Day of week offset: Monday = 0, Sunday = 6
            int startCol = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;

            // Only show Confirmed and Pending for Calendar View
            var monthItems = _schedules.Where(x =>
                x.Date.Year == _currentMonth.Year &&
                x.Date.Month == _currentMonth.Month &&
                (x.Status == "Đã xác nhận" || x.Status == "Chờ HLV duyệt")
            ).ToList();

            // Apply active source filter if user selected one
            if (_selectedFilter.Contains("Lộ trình PT"))
                monthItems = monthItems.Where(x => x.Source == "Lộ trình PT xếp").ToList();
            else if (_selectedFilter.Contains("Hội viên tự đặt"))
                monthItems = monthItems.Where(x => x.Source == "Hội viên tự đặt").ToList();
            else if (_selectedFilter.Contains("Chờ"))
                monthItems = monthItems.Where(x => x.Status == "Chờ HLV duyệt").ToList();

            int currentDay = 1;
            DateTime today = DateTime.Today;

            pnlCalendarGrid.SuspendLayout();

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    var cell = _gridCells[row, col];
                    if (cell == null) continue;

                    // Out of month bounds (blank cell)
                    if ((row == 0 && col < startCol) || (currentDay > daysInMonth))
                    {
                        cell.Date = null;
                        cell.DaySchedules.Clear();
                        cell.LblDayNum.Text = "";
                        cell.Panel.BackColor = Color.FromArgb(250, 250, 252);

                        for (int i = 0; i < 3; i++)
                        {
                            cell.EventLabels[i].Visible = false;
                        }
                        continue;
                    }

                    // In month bounds (active day cell)
                    DateTime cellDate = new DateTime(_currentMonth.Year, _currentMonth.Month, currentDay);
                    var daySchedules = monthItems.Where(x => x.Date.Date == cellDate.Date).ToList();

                    cell.Date = cellDate;
                    cell.DaySchedules = daySchedules;
                    cell.LblDayNum.Text = currentDay.ToString();

                    bool isToday = (cellDate.Date == today);
                    cell.Panel.BackColor = isToday ? Color.FromArgb(254, 249, 195) : Color.White;
                    cell.LblDayNum.Font = isToday ? UITheme.FontBold : UITheme.FontSmallBold;
                    cell.LblDayNum.ForeColor = (col == 6) ? UITheme.Danger : (isToday ? UITheme.PrimaryDark : UITheme.TextDark);

                    // Update event labels (instant visibility & property update)
                    for (int i = 0; i < 3; i++)
                    {
                        if (i < daySchedules.Count)
                        {
                            var s = daySchedules[i];
                            bool isFixedPT = s.Source == "Lộ trình PT xếp";
                            bool isPending = s.Status == "Chờ HLV duyệt";

                            Color pillBg = isPending ? Color.FromArgb(254, 243, 199) : (isFixedPT ? UITheme.PrimaryLight : UITheme.SuccessLight);
                            Color pillText = isPending ? Color.FromArgb(180, 83, 9) : (isFixedPT ? UITheme.Primary : UITheme.Success);
                            string icon = isPending ? "⏳" : (isFixedPT ? "🏋️" : "✅");

                            cell.EventLabels[i].Text = $"{icon} {s.TimeRange.Split('-')[0].Trim()} {s.WorkoutType.Split('(')[0].Trim()}";
                            cell.EventLabels[i].BackColor = pillBg;
                            cell.EventLabels[i].ForeColor = pillText;
                            cell.EventLabels[i].Visible = true;
                        }
                        else
                        {
                            cell.EventLabels[i].Visible = false;
                        }
                    }

                    currentDay++;
                }
            }

            pnlCalendarGrid.ResumeLayout(true);
        }

        private void ShowDayScheduleDetail(DateTime date, ScheduleItem item)
        {
            string statusIcon = item.Status == "Đã xác nhận" ? "✅" : (item.Status == "Chờ HLV duyệt" ? "⏳" : "✔️");
            string sourceIcon = item.Source == "Lộ trình PT xếp" ? "🏋️ [PT xếp cố định]" : "🙋 [Hội viên tự đặt thêm]";

            lblSelectedDayDetail.Text =
                $"📅 Ngày {date:dd/MM/yyyy} ({item.DayOfWeekVN}): {item.TimeRange} | Bộ môn: {item.WorkoutType}\n" +
                $"👨‍🏫 HLV: {item.TrainerName} | Phòng: {item.Room} | Nguồn gốc: {sourceIcon} | Trạng thái: {statusIcon} {item.Status}";
        }

        private IEnumerable<ScheduleItem> GetFilteredSchedules()
        {
            var query = _schedules.AsEnumerable();

            if (_selectedFilter.Contains("Lộ trình PT"))
            {
                query = query.Where(x => x.Source == "Lộ trình PT xếp");
            }
            else if (_selectedFilter.Contains("Hội viên tự đặt"))
            {
                query = query.Where(x => x.Source == "Hội viên tự đặt");
            }
            else if (_selectedFilter.Contains("Chờ"))
            {
                query = query.Where(x => x.Status == "Chờ HLV duyệt");
            }
            else if (_selectedFilter.Contains("Đã xác nhận"))
            {
                query = query.Where(x => x.Status == "Đã xác nhận");
            }

            return query.OrderByDescending(x => x.Date);
        }

        private void RefreshAllViews()
        {
            RenderTableData();
            if (_isCalendarMode)
            {
                RenderCalendarMonth();
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            if (dgvSchedule.SelectedRows.Count > 0)
            {
                int idx = dgvSchedule.SelectedRows[0].Index;
                var filtered = GetFilteredSchedules().ToList();
                if (idx >= 0 && idx < filtered.Count)
                {
                    var target = filtered[idx];
                    var res = MessageBox.Show(
                        $"Bạn có chắc chắn muốn hủy buổi tập ngày {target.Date:dd/MM/yyyy} ({target.TimeRange})?\n\n• Nguồn gốc: {target.Source}\n• HLV: {target.TrainerName}",
                        "Xác nhận hủy lịch",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (res == DialogResult.Yes)
                    {
                        _schedules.Remove(target);
                        RefreshAllViews();
                        MessageBox.Show("Đã hủy buổi tập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng lịch tập trong bảng để hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Right Side: Booking Form Card

        private RoundedPanel CreateBookingFormCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 670,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(22)
            };

            Label lblTitle = new Label
            {
                Text = "📝 ĐĂNG KÝ / ĐẶT LỊCH HẸN PT",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = 30
            };

            Panel pnlForm = new Panel { Dock = DockStyle.Fill, AutoScroll = true };

            int y = 6;
            int inputWidth = 350;

            // Notice Box explaining the approval workflow
            Panel pnlNotice = new Panel
            {
                Location = new Point(4, y),
                Width = inputWidth,
                Height = 44,
                BackColor = Color.FromArgb(254, 243, 199),
                Padding = new Padding(8)
            };
            Label lblNotice = new Label
            {
                Text = "💡 Lịch Hội viên đặt thêm sẽ đi qua luồng duyệt (Chờ HLV duyệt) trước khi chốt lịch chính thức.",
                Font = UITheme.FontSmallBold,
                ForeColor = Color.FromArgb(180, 83, 9),
                Dock = DockStyle.Fill
            };
            pnlNotice.Controls.Add(lblNotice);
            pnlForm.Controls.Add(pnlNotice);
            y += 50;

            // 1. Chọn ngày tập
            pnlForm.Controls.Add(CreateFormLabel("1. Chọn ngày tập:", ref y));
            dtpDate = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Location = new Point(4, y),
                Width = inputWidth,
                Font = UITheme.FontRegular,
                MinDate = DateTime.Today
            };
            pnlForm.Controls.Add(dtpDate);
            y += 38;

            // 2. Chọn khung giờ ca tập
            pnlForm.Controls.Add(CreateFormLabel("2. Khung giờ ca tập:", ref y));
            cboTime = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(4, y),
                Width = inputWidth,
                Font = UITheme.FontRegular
            };
            cboTime.Items.AddRange(new object[] {
                "06:00 - 07:30 (Ca sáng sớm)",
                "08:00 - 09:30 (Ca sáng)",
                "15:00 - 16:30 (Ca chiều)",
                "17:30 - 19:00 (Ca tan tầm)",
                "18:00 - 19:30 (Ca tối phổ biến)",
                "19:30 - 21:00 (Ca muộn)"
            });
            cboTime.SelectedIndex = 4;
            pnlForm.Controls.Add(cboTime);
            y += 38;

            // 3. Chọn bộ môn
            pnlForm.Controls.Add(CreateFormLabel("3. Bộ môn & Mục tiêu rèn luyện:", ref y));
            cboType = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(4, y),
                Width = inputWidth,
                Font = UITheme.FontRegular
            };
            cboType.Items.AddRange(new object[] {
                "Gym Thể hình (Tăng cơ giảm mỡ)",
                "Kèm PT 1-1 chuyên sâu",
                "Cardio & HIIT đốt mỡ cấp tốc",
                "Boxing / Kickfit đối kháng",
                "Yoga & Giãn cơ phục hồi"
            });
            cboType.SelectedIndex = 1;
            pnlForm.Controls.Add(cboType);
            y += 38;

            // 4. Chọn HLV PT
            pnlForm.Controls.Add(CreateFormLabel("4. Huấn luyện viên phụ trách:", ref y));
            cboTrainer = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(4, y),
                Width = inputWidth,
                Font = UITheme.FontRegular
            };
            cboTrainer.Items.AddRange(new object[] {
                "HLV Nguyễn Văn Nam (HLV Thể hình chính)",
                "HLV Lê Thu Hà (Yoga & Cardio)",
                "HLV Trần Minh Đức (Boxing & Kickfit)"
            });
            cboTrainer.SelectedIndex = 0;
            pnlForm.Controls.Add(cboTrainer);
            y += 38;

            // 5. Chọn phòng
            pnlForm.Controls.Add(CreateFormLabel("5. Phòng tập:", ref y));
            cboRoom = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(4, y),
                Width = inputWidth,
                Font = UITheme.FontRegular
            };
            cboRoom.Items.AddRange(new object[] {
                "Phòng PT-01 (Khu VIP)",
                "Phòng PT-02 (Khu VIP)",
                "Studio A (Yoga & Lớp nhóm)",
                "Khu vực Máy tập Tự do"
            });
            cboRoom.SelectedIndex = 0;
            pnlForm.Controls.Add(cboRoom);
            y += 38;

            // 6. Ghi chú & Yêu cầu HLV
            pnlForm.Controls.Add(CreateFormLabel("6. Ghi chú mục tiêu buổi tập & Yêu cầu HLV:", ref y));
            txtNote = new TextBox
            {
                Multiline = true,
                Height = 85,
                Width = inputWidth,
                Location = new Point(4, y),
                Font = UITheme.FontRegular,
                ScrollBars = ScrollBars.Vertical,
                Text = "Cần HLV hỗ trợ kỹ thuật và kiểm tra form động tác."
            };
            pnlForm.Controls.Add(txtNote);
            y += 98;

            // Submit Button
            btnSubmit = new Button
            {
                Text = "🚀 Gửi yêu cầu đặt lịch hẹn",
                Font = UITheme.FontBold,
                Height = 44,
                Width = inputWidth,
                Location = new Point(4, y),
                BackColor = UITheme.Primary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSubmit.FlatAppearance.BorderSize = 0;
            btnSubmit.Click += BtnSubmit_Click;
            pnlForm.Controls.Add(btnSubmit);

            card.Controls.Add(pnlForm);
            card.Controls.Add(lblTitle);

            // Responsive width adjustment on resize
            card.Resize += (s, e) =>
            {
                int w = Math.Max(260, pnlForm.ClientSize.Width - 14);
                pnlNotice.Width = w;
                dtpDate.Width = w;
                cboTime.Width = w;
                cboType.Width = w;
                cboTrainer.Width = w;
                cboRoom.Width = w;
                txtNote.Width = w;
                btnSubmit.Width = w;
            };

            return card;
        }

        private void BtnSubmit_Click(object? sender, EventArgs e)
        {
            DateTime chosenDate = dtpDate.Value.Date;
            string timeStr = cboTime.Text.Split('(')[0].Trim();
            string typeStr = cboType.Text.Split('(')[0].Trim();
            string trainerStr = cboTrainer.Text.Split('(')[0].Trim();
            string roomStr = cboRoom.Text.Split('(')[0].Trim();
            string noteStr = txtNote.Text.Trim();

            // Check if there is already a schedule on that date and time slot
            bool isDuplicate = _schedules.Any(x => x.Date.Date == chosenDate && x.TimeRange == timeStr);
            if (isDuplicate)
            {
                MessageBox.Show(
                    $"Bạn đã có lịch tập vào khung giờ {timeStr} ngày {chosenDate:dd/MM/yyyy}!\nVui lòng chọn khung giờ hoặc ngày khác.",
                    "Trùng lịch tập",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Create new schedule item with source "Hội viên tự đặt" and status "Chờ HLV duyệt"
            var newItem = new ScheduleItem
            {
                Date = chosenDate,
                TimeRange = timeStr,
                WorkoutType = typeStr,
                TrainerName = trainerStr,
                Room = roomStr,
                Source = "Hội viên tự đặt",
                Status = "Chờ HLV duyệt",
                Note = noteStr
            };

            _schedules.Insert(0, newItem);
            RefreshAllViews();

            MessageBox.Show(
                $"✅ Gửi yêu cầu đặt lịch hẹn thành công!\n\n" +
                $"• Ngày tập: {chosenDate:dd/MM/yyyy}\n" +
                $"• Khung giờ: {timeStr}\n" +
                $"• HLV phụ trách: {trainerStr}\n" +
                $"• Nguồn gốc: Hội viên tự đặt\n" +
                $"• Trạng thái: Chờ HLV duyệt (HLV sẽ phản hồi sớm nhất)",
                "Đặt lịch thành công",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private Label CreateFormLabel(string text, ref int y)
        {
            Label lbl = new Label
            {
                Text = text,
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(4, y)
            };
            y += 22;
            return lbl;
        }

        #endregion
    }
}
