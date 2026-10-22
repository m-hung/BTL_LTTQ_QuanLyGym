using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyGym.UI.Utils;

namespace QuanLyGym.UI.Forms.Member.Views
{
    public class ucMemberSchedule : UserControl
    {
        private DataGridView dgvSchedule = null!;
        private DateTimePicker dtpDate = null!;
        private ComboBox cboTime = null!;
        private ComboBox cboType = null!;
        private ComboBox cboTrainer = null!;
        private ComboBox cboRoom = null!;
        private TextBox txtNote = null!;

        public ucMemberSchedule()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.BackColor = UITheme.Background;
            this.Dock = DockStyle.Fill;
            this.AutoScroll = true;

            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(24, 20, 24, 30),
                BackColor = UITheme.Background
            };

            // Header Row
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 50 };
            Label lblTitle = new Label
            {
                Text = "📅 LỊCH TẬP & ĐẶT LỊCH HUẤN LUYỆN VIÊN (PT)",
                Font = UITheme.FontTitle,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(0, 8)
            };
            pnlHeader.Controls.Add(lblTitle);

            // Two Column Layout
            TableLayoutPanel pnlCols = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 10, 0, 10)
            };
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64f));
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36f));

            // Left: Schedule Table & Actions
            RoundedPanel cardList = CreateScheduleListCard();
            cardList.Dock = DockStyle.Top;
            cardList.Margin = new Padding(0, 0, 10, 0);

            // Right: Booking Form
            RoundedPanel cardBooking = CreateBookingFormCard();
            cardBooking.Dock = DockStyle.Top;
            cardBooking.Margin = new Padding(10, 0, 0, 0);

            pnlCols.Controls.Add(cardList, 0, 0);
            pnlCols.Controls.Add(cardBooking, 1, 0);

            pnlMain.Controls.Add(pnlCols);
            pnlMain.Controls.Add(pnlHeader);

            this.Controls.Add(pnlMain);
            this.ResumeLayout(false);
        }

        private RoundedPanel CreateScheduleListCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 620,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(20)
            };

            Panel pnlTop = new Panel { Dock = DockStyle.Top, Height = 44 };
            Label lblTitle = new Label
            {
                Text = "DANH SÁCH LỊCH TẬP ĐÃ LÊN LỊCH",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(0, 10)
            };

            // Filter Tabs
            FlowLayoutPanel pnlFilter = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight
            };
            string[] filters = new string[] { "Tất cả", "Tuần này", "Hôm nay", "Chờ duyệt" };
            foreach (var f in filters)
            {
                Button btnF = new Button
                {
                    Text = f,
                    Font = UITheme.FontSmallBold,
                    Height = 32,
                    AutoSize = true,
                    BackColor = f == "Tất cả" ? UITheme.PrimaryLight : Color.FromArgb(248, 250, 252),
                    ForeColor = f == "Tất cả" ? UITheme.Primary : UITheme.TextMuted,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Margin = new Padding(3, 4, 3, 4)
                };
                btnF.FlatAppearance.BorderSize = 0;
                pnlFilter.Controls.Add(btnF);
            }

            pnlTop.Controls.Add(lblTitle);
            pnlTop.Controls.Add(pnlFilter);

            // DataGridView
            dgvSchedule = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgvSchedule);

            var colDate = new DataGridViewTextBoxColumn { Name = "Date", HeaderText = "Ngày tập", Width = 115, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colTime = new DataGridViewTextBoxColumn { Name = "Time", HeaderText = "Khung giờ", Width = 120, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colType = new DataGridViewTextBoxColumn { Name = "Type", HeaderText = "Bộ môn", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 135 };
            var colTrainer = new DataGridViewTextBoxColumn { Name = "Trainer", HeaderText = "Huấn luyện viên PT", Width = 175, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colRoom = new DataGridViewTextBoxColumn { Name = "Room", HeaderText = "Phòng", Width = 90, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colStatus = new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Trạng thái", Width = 130, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };

            dgvSchedule.Columns.AddRange(colDate, colTime, colType, colTrainer, colRoom, colStatus);

            dgvSchedule.Rows.Add("Hôm nay (17/10)", "18:00 - 19:30", "Gym Thể hình", "HLV Nguyễn Văn Nam", "PT-01", "Đã xác nhận");
            dgvSchedule.Rows.Add("Thứ 4, 21/10", "17:30 - 19:00", "Cardio & HIIT", "HLV Nguyễn Văn Nam", "Room 02", "Đã xác nhận");
            dgvSchedule.Rows.Add("Thứ 6, 23/10", "18:00 - 19:30", "Ngực & Tay trước", "HLV Nguyễn Văn Nam", "PT-01", "Chờ HLV duyệt");
            dgvSchedule.Rows.Add("Chủ nhật, 25/10", "08:30 - 10:00", "Yoga & Giãn cơ", "HLV Lê Thu Hà", "Studio A", "Đã xác nhận");
            dgvSchedule.Rows.Add("15/10/2026", "18:00 - 19:30", "Chân - Mông", "HLV Nguyễn Văn Nam", "PT-01", "Hoàn thành");
            dgvSchedule.Rows.Add("13/10/2026", "18:00 - 19:30", "Lưng - Xô", "HLV Nguyễn Văn Nam", "PT-01", "Hoàn thành");

            dgvSchedule.CellFormatting += (s, e) =>
            {
                if (dgvSchedule.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
                {
                    string status = e.Value.ToString() ?? "";
                    if (status.Contains("Đã xác nhận") || status.Contains("Hoàn thành"))
                    {
                        e.CellStyle.ForeColor = UITheme.Success;
                        e.CellStyle.Font = UITheme.FontBold;
                    }
                    else if (status.Contains("Chờ"))
                    {
                        e.CellStyle.ForeColor = UITheme.Warning;
                        e.CellStyle.Font = UITheme.FontBold;
                    }
                }
            };

            // Bottom Buttons (Wider button to prevent text clipping "❌ Hủy lịch đã chọn")
            Panel pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(0, 8, 0, 0) };
            Button btnCancel = new Button
            {
                Text = "❌ Hủy lịch đã chọn",
                Font = UITheme.FontBold,
                Height = 38,
                Width = 220,
                BackColor = UITheme.DangerLight,
                ForeColor = UITheme.Danger,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Left
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) =>
            {
                if (dgvSchedule.SelectedRows.Count > 0)
                {
                    var res = MessageBox.Show("Bạn có chắc chắn muốn hủy buổi tập đã chọn?", "Xác nhận hủy lịch", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        dgvSchedule.Rows.RemoveAt(dgvSchedule.SelectedRows[0].Index);
                        MessageBox.Show("Đã hủy lịch tập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một lịch tập trong bảng để hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            pnlBottom.Controls.Add(btnCancel);

            card.Controls.Add(dgvSchedule);
            card.Controls.Add(pnlBottom);
            card.Controls.Add(pnlTop);

            return card;
        }

        private RoundedPanel CreateBookingFormCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 620,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(22)
            };

            Label lblTitle = new Label
            {
                Text = "📝 ĐĂNG KÝ BUỔI TẬP / HẸN PT",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = 30
            };

            Panel pnlForm = new Panel { Dock = DockStyle.Fill, AutoScroll = true };

            int y = 8;
            int inputWidth = 350;

            // 1. Chọn ngày
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

            // 2. Chọn khung giờ
            pnlForm.Controls.Add(CreateFormLabel("2. Khung giờ ca tập:", ref y));
            cboTime = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(4, y),
                Width = inputWidth,
                Font = UITheme.FontRegular
            };
            cboTime.Items.AddRange(new object[] {
                "06:00 - 07:30 (Ca sáng)",
                "08:00 - 09:30 (Ca sáng)",
                "15:00 - 16:30 (Ca chiều)",
                "17:30 - 19:00 (Ca tan tầm)",
                "18:00 - 19:30 (Ca tối)",
                "19:30 - 21:00 (Ca muộn)"
            });
            cboTime.SelectedIndex = 4;
            pnlForm.Controls.Add(cboTime);
            y += 38;

            // 3. Chọn bộ môn
            pnlForm.Controls.Add(CreateFormLabel("3. Bộ môn & Mục tiêu:", ref y));
            cboType = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(4, y),
                Width = inputWidth,
                Font = UITheme.FontRegular
            };
            cboType.Items.AddRange(new object[] {
                "Gym Thể hình (Tăng cơ)",
                "Kèm PT 1-1 chuyên sâu",
                "Cardio & Đốt mỡ (Fat Burn)",
                "Boxing / Kickfit đối kháng",
                "Yoga & Kéo giãn phục hồi"
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
                "HLV Nguyễn Văn Nam (HLV chính)",
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

            // 6. Ghi chú (Enlarged and expanded as requested)
            pnlForm.Controls.Add(CreateFormLabel("6. Ghi chú mục tiêu buổi tập & Yêu cầu HLV:", ref y));
            txtNote = new TextBox
            {
                Multiline = true,
                Height = 110,
                Width = inputWidth,
                Location = new Point(4, y),
                Font = UITheme.FontRegular,
                ScrollBars = ScrollBars.Vertical,
                Text = "Tập trung nhóm cơ Ngực và Tay sau, cần HLV hướng dẫn kỹ thuật Bench Press và căng cơ phục hồi."
            };
            pnlForm.Controls.Add(txtNote);
            y += 122;

            // Submit Button
            Button btnSubmit = new Button
            {
                Text = " Xác nhận đặt lịch tập",
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
            btnSubmit.Click += (s, e) =>
            {
                string dateStr = dtpDate.Value.ToString("dd/MM/yyyy");
                string timeStr = cboTime.Text.Split('(')[0].Trim();
                string typeStr = cboType.Text.Split('(')[0].Trim();
                string trainerStr = cboTrainer.Text.Split('(')[0].Trim();
                string roomStr = cboRoom.Text.Split('(')[0].Trim();

                dgvSchedule.Rows.Insert(0, dateStr, timeStr, typeStr, trainerStr, roomStr, "Chờ HLV duyệt");
                MessageBox.Show($"Đặt lịch thành công ngày {dateStr} lúc {timeStr}!\nYêu cầu đã gửi tới {trainerStr}.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            pnlForm.Controls.Add(btnSubmit);

            card.Controls.Add(pnlForm);
            card.Controls.Add(lblTitle);

            // Responsive width adjustment on resize
            card.Resize += (s, e) =>
            {
                int w = Math.Max(260, pnlForm.ClientSize.Width - 14);
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
    }
}
