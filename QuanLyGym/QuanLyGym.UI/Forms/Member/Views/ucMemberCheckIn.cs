using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using QuanLyGym.UI.Utils;

namespace QuanLyGym.UI.Forms.Member.Views
{
    public class ucMemberCheckIn : UserControl
    {
        private PictureBox picPreview = null!;
        private TextBox txtCheckInNote = null!;
        private ComboBox cboCheckInPT = null!;
        private DataGridView dgvHistory = null!;
        private Label lblClock = null!;
        private System.Windows.Forms.Timer? timerClock;
        private string? selectedImagePath = null;

        public ucMemberCheckIn()
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

            // Header Title
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 50 };
            Label lblTitle = new Label
            {
                Text = "📸 ĐIỂM DANH & TẢI ẢNH CHECK-IN (PT DUYỆT)",
                Font = UITheme.FontTitle,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(0, 8)
            };
            pnlHeader.Controls.Add(lblTitle);

            // Stat Cards Row
            TableLayoutPanel pnlStats = CreateStatSummary();
            pnlStats.Dock = DockStyle.Top;
            pnlStats.Margin = new Padding(0, 0, 0, 16);

            // Two Column Layout (38% Form, 62% History Table)
            TableLayoutPanel pnlCols = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 10, 0, 10)
            };
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38f)); // Left: Check-in & Photo Upload
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62f)); // Right: Approval History

            RoundedPanel cardForm = CreateUploadCheckInCard();
            cardForm.Dock = DockStyle.Top;
            cardForm.Margin = new Padding(0, 0, 10, 0);

            RoundedPanel cardHistory = CreateHistoryCard();
            cardHistory.Dock = DockStyle.Top;
            cardHistory.Margin = new Padding(10, 0, 0, 0);

            pnlCols.Controls.Add(cardForm, 0, 0);
            pnlCols.Controls.Add(cardHistory, 1, 0);

            pnlMain.Controls.Add(pnlCols);
            pnlMain.Controls.Add(pnlStats);
            pnlMain.Controls.Add(pnlHeader);

            this.Controls.Add(pnlMain);
            this.ResumeLayout(false);

            // Start Live Clock
            timerClock = new System.Windows.Forms.Timer { Interval = 1000 };
            timerClock.Tick += (s, e) =>
            {
                if (lblClock != null && !lblClock.IsDisposed)
                    lblClock.Text = "🕒 Thời gian hiện tại: " + DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy");
            };
            timerClock.Start();
        }

        private TableLayoutPanel CreateStatSummary()
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                ColumnCount = 4,
                RowCount = 1,
                Height = 90,
                Dock = DockStyle.Top
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));

            table.Controls.Add(CreateStatPill("TỔNG BUỔI ĐIỂM DANH", "15 Buổi", UITheme.Primary), 0, 0);
            table.Controls.Add(CreateStatPill("PT ĐÃ DUYỆT", "14 Buổi", UITheme.Success), 1, 0);
            table.Controls.Add(CreateStatPill("ĐANG CHỜ DUYỆT", "1 Buổi", UITheme.Warning), 2, 0);
            table.Controls.Add(CreateStatPill("TỶ LỆ CHUYÊN CẦN", "92% (Rất tốt)", UITheme.Secondary), 3, 0);

            return table;
        }

        private RoundedPanel CreateStatPill(string label, string val, Color color)
        {
            RoundedPanel card = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                CornerRadius = 10,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Margin = new Padding(6),
                Padding = new Padding(14, 10, 14, 10)
            };

            Label lbl = new Label { Text = label, Font = UITheme.FontSmallBold, ForeColor = UITheme.TextMuted, Location = new Point(14, 10), AutoSize = true };
            Label v = new Label { Text = val, Font = new Font("Segoe UI", 13f, FontStyle.Bold), ForeColor = color, Location = new Point(14, 30), AutoSize = true };

            card.Controls.Add(lbl);
            card.Controls.Add(v);
            return card;
        }

        private RoundedPanel CreateUploadCheckInCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 630,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(22)
            };

            Label lblTitle = new Label
            {
                Text = "📌 ĐIỂM DANH & TẢI ẢNH CHECK-IN",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = 28
            };

            lblClock = new Label
            {
                Text = "🕒 Thời gian hiện tại: " + DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy"),
                Font = UITheme.FontBold,
                ForeColor = UITheme.Primary,
                Dock = DockStyle.Top,
                Height = 24
            };

            Panel pnlForm = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(0, 10, 0, 0) };

            int y = 6;
            int defaultW = 340;

            // 1. Huấn luyện viên duyệt
            Label lblPT = new Label { Text = "Huấn luyện viên duyệt (PT):", Font = UITheme.FontSmallBold, ForeColor = UITheme.TextDark, Location = new Point(4, y), AutoSize = true };
            y += 22;
            cboCheckInPT = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(4, y),
                Width = defaultW,
                Font = UITheme.FontRegular
            };
            cboCheckInPT.Items.AddRange(new object[] {
                "HLV Nguyễn Văn Nam (PT chính)",
                "HLV Lê Thu Hà",
                "HLV Trần Minh Đức",
                "Quản lý ca trực / Lễ tân"
            });
            cboCheckInPT.SelectedIndex = 0;
            pnlForm.Controls.Add(lblPT);
            pnlForm.Controls.Add(cboCheckInPT);
            y += 38;

            // 2. Upload Ảnh Minh Chứng (Selfie / Phòng tập)
            Label lblPhoto = new Label { Text = "Ảnh chụp check-in tại phòng tập (Selfie/Phòng):", Font = UITheme.FontSmallBold, ForeColor = UITheme.TextDark, Location = new Point(4, y), AutoSize = true };
            y += 22;

            // Image Preview Box
            picPreview = new PictureBox
            {
                Size = new Size(defaultW, 155),
                Location = new Point(4, y),
                BackColor = Color.FromArgb(241, 245, 249),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            Label lblPlaceholder = new Label
            {
                Text = "📷 Chưa chọn ảnh\nBấm nút bên dưới để tải ảnh từ máy tính",
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextMuted,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            picPreview.Controls.Add(lblPlaceholder);
            pnlForm.Controls.Add(lblPhoto);
            pnlForm.Controls.Add(picPreview);
            y += 165;

            // Buttons to select / clear image
            FlowLayoutPanel pnlImgBtns = new FlowLayoutPanel
            {
                Location = new Point(4, y),
                Size = new Size(defaultW, 36),
                FlowDirection = FlowDirection.LeftToRight
            };

            Button btnChooseImg = new Button
            {
                Text = "📁 Chọn tệp ảnh...",
                Font = UITheme.FontSmallBold,
                Size = new Size(140, 32),
                BackColor = UITheme.PrimaryLight,
                ForeColor = UITheme.Primary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnChooseImg.FlatAppearance.BorderSize = 0;
            btnChooseImg.Click += (s, e) =>
            {
                using OpenFileDialog ofd = new OpenFileDialog
                {
                    Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                    Title = "Chọn ảnh check-in"
                };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        selectedImagePath = ofd.FileName;
                        lblPlaceholder.Visible = false;
                        picPreview.Image = Image.FromFile(selectedImagePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể nạp file ảnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            Button btnClearImg = new Button
            {
                Text = "🗑️ Xóa ảnh",
                Font = UITheme.FontSmallBold,
                Size = new Size(95, 32),
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = UITheme.TextMuted,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClearImg.FlatAppearance.BorderSize = 0;
            btnClearImg.Click += (s, e) =>
            {
                selectedImagePath = null;
                picPreview.Image = null;
                lblPlaceholder.Visible = true;
            };

            pnlImgBtns.Controls.Add(btnChooseImg);
            pnlImgBtns.Controls.Add(btnClearImg);
            pnlForm.Controls.Add(pnlImgBtns);
            y += 44;

            // 3. Ghi chú buổi tập (Enlarged and expanded as requested)
            Label lblNote = new Label { Text = "Ghi chú buổi tập & Cảm nhận thể lực:", Font = UITheme.FontSmallBold, ForeColor = UITheme.TextDark, Location = new Point(4, y), AutoSize = true };
            y += 22;
            txtCheckInNote = new TextBox
            {
                Multiline = true,
                Height = 100,
                Width = defaultW,
                Location = new Point(4, y),
                Font = UITheme.FontRegular,
                ScrollBars = ScrollBars.Vertical,
                Text = "Buổi tập Lưng xô và Cardio 30 phút, hoàn thành tốt giáo án PT giao. Cảm nhận cơ bắp đáp ứng rất tốt."
            };
            pnlForm.Controls.Add(lblNote);
            pnlForm.Controls.Add(txtCheckInNote);
            y += 112;

            // 4. Submit Button
            Button btnSubmitCheckIn = new Button
            {
                Text = "🚀 GỬI YÊU CẦU ĐIỂM DANH CHO PT DUYỆT",
                Font = UITheme.FontBold,
                Height = 44,
                Width = defaultW,
                Location = new Point(4, y),
                BackColor = Color.FromArgb(16, 185, 129), // Emerald
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSubmitCheckIn.FlatAppearance.BorderSize = 0;
            btnSubmitCheckIn.Click += (s, e) =>
            {
                string timeNow = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                string ptName = cboCheckInPT.Text.Split('(')[0].Trim();
                string note = txtCheckInNote.Text.Trim();
                string code = "#CK-" + new Random().Next(110, 999);

                dgvHistory.Rows.Insert(0, code, timeNow, ptName, note, "Chờ duyệt");
                MessageBox.Show($"Đã gửi yêu cầu điểm danh thành công!\nHLV {ptName} sẽ kiểm tra và phê duyệt buổi tập cho bạn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            pnlForm.Controls.Add(btnSubmitCheckIn);

            card.Controls.Add(pnlForm);
            card.Controls.Add(lblClock);
            card.Controls.Add(lblTitle);

            // Responsive auto resize
            card.Resize += (s, e) =>
            {
                int w = Math.Max(260, pnlForm.ClientSize.Width - 14);
                cboCheckInPT.Width = w;
                picPreview.Width = w;
                pnlImgBtns.Width = w;
                txtCheckInNote.Width = w;
                btnSubmitCheckIn.Width = w;
            };

            return card;
        }

        private RoundedPanel CreateHistoryCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 630,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(20)
            };

            Label lblTitle = new Label
            {
                Text = "📜 LỊCH SỬ ĐIỂM DANH & PHÊ DUYỆT",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = 34
            };

            dgvHistory = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgvHistory);

            var colCode = new DataGridViewTextBoxColumn { Name = "Code", HeaderText = "Mã", Width = 85, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colTime = new DataGridViewTextBoxColumn { Name = "Time", HeaderText = "Thời gian", Width = 145, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colPT = new DataGridViewTextBoxColumn { Name = "PT", HeaderText = "HLV Phê duyệt", Width = 160, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colNote = new DataGridViewTextBoxColumn { Name = "Note", HeaderText = "Ghi chú buổi tập", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 160 };
            var colStatus = new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Trạng thái", Width = 125, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };

            dgvHistory.Columns.AddRange(colCode, colTime, colPT, colNote, colStatus);

            dgvHistory.Rows.Add("#CK-426", "17/10/2026 09:49", "HLV Nguyễn Văn Nam", "Khởi động & Cardio 30p", "Chờ duyệt");
            dgvHistory.Rows.Add("#CK-109", "15/10/2026 18:05", "HLV Nguyễn Văn Nam", "Buổi tập Chân & Vai (Có ảnh)", "Đã duyệt");
            dgvHistory.Rows.Add("#CK-108", "13/10/2026 18:12", "HLV Nguyễn Văn Nam", "Cardio 45 phút + Bụng (Có ảnh)", "Đã duyệt");
            dgvHistory.Rows.Add("#CK-107", "11/10/2026 17:58", "HLV Nguyễn Văn Nam", "Lưng xô & Tay sau (Có ảnh)", "Đã duyệt");
            dgvHistory.Rows.Add("#CK-106", "09/10/2026 18:00", "HLV Nguyễn Văn Nam", "Ngực & Tay trước (Có ảnh)", "Đã duyệt");
            dgvHistory.Rows.Add("#CK-105", "06/10/2026 18:20", "HLV Nguyễn Văn Nam", "Leg day cường độ cao (Có ảnh)", "Đã duyệt");

            dgvHistory.CellFormatting += (s, e) =>
            {
                if (dgvHistory.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
                {
                    string status = e.Value.ToString() ?? "";
                    if (status.Contains("Đã duyệt"))
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

            // Double click to view details
            dgvHistory.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvHistory.Rows[e.RowIndex];
                    string code = row.Cells["Code"].Value?.ToString() ?? "";
                    string time = row.Cells["Time"].Value?.ToString() ?? "";
                    string pt = row.Cells["PT"].Value?.ToString() ?? "";
                    string status = row.Cells["Status"].Value?.ToString() ?? "";
                    string note = row.Cells["Note"].Value?.ToString() ?? "";

                    MessageBox.Show($"Mã điểm danh: {code}\nThời gian: {time}\nHLV duyệt: {pt}\nTrạng thái: {status}\nGhi chú: {note}\n\nNhận xét từ HLV: 'Thực hiện đúng kỹ thuật, tinh thần tập luyện rất tốt.'", "Chi tiết điểm danh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };

            card.Controls.Add(dgvHistory);
            card.Controls.Add(lblTitle);

            return card;
        }
    }
}
