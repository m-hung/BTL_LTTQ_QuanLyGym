using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using QuanLyGym.UI.Services;
using QuanLyGym.UI.Utils;

namespace QuanLyGym.UI.Forms.Member.Views
{
    public class ucMemberCheckIn : UserControl
    {
        private class CheckInPtItem
        {
            public string Text { get; set; } = "";
            public int? Value { get; set; }
            public override string ToString() => Text;
        }

        private PictureBox picPreview = null!;
        private TextBox txtCheckInNote = null!;
        private ComboBox cboCheckInPT = null!;
        private DataGridView dgvHistory = null!;
        private Label lblClock = null!;
        private System.Windows.Forms.Timer? timerClock;
        private string? selectedImagePath = null;
        private Label lblPlaceholder = null!;

        // Stat Labels
        private Label lblTotalVal = null!;
        private Label lblApprovedVal = null!;
        private Label lblPendingVal = null!;
        private Label lblRateVal = null!;

        public ucMemberCheckIn()
        {
            InitializeComponent();
            LoadCheckInHistory();
            RefreshStatSummary();
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

            // Root TableLayoutPanel: 2 rows — Stats (Row 0), Fill content (Row 1)
            TableLayoutPanel pnlRoot = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding((int)(16 * scale), (int)(12 * scale), (int)(16 * scale), (int)(14 * scale)),
                BackColor = UITheme.Background
            };
            pnlRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            int statHeight = (int)Math.Round(110f * scale);
            pnlRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, statHeight)); // Row 0: Stat cards
            pnlRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));        // Row 1: Content

            // Row 0: Stat Cards Row
            TableLayoutPanel pnlStats = CreateStatSummary(scale);
            pnlStats.Dock = DockStyle.Fill;
            pnlStats.Margin = new Padding(0, 0, 0, (int)(8 * scale));
            pnlRoot.Controls.Add(pnlStats, 0, 0);

            // Row 1: Two Column Layout (38% Form, 62% History Table)
            TableLayoutPanel pnlCols = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(0)
            };
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38f));
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62f));
            pnlCols.RowStyles.Clear();
            pnlCols.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            RoundedPanel cardForm = CreateUploadCheckInCard(scale);
            cardForm.Dock = DockStyle.Fill;
            cardForm.Margin = new Padding(0, 0, (int)(8 * scale), 0);

            RoundedPanel cardHistory = CreateHistoryCard(scale);
            cardHistory.Dock = DockStyle.Fill;
            cardHistory.Margin = new Padding((int)(8 * scale), 0, 0, 0);

            pnlCols.Controls.Add(cardForm, 0, 0);
            pnlCols.Controls.Add(cardHistory, 1, 0);
            pnlRoot.Controls.Add(pnlCols, 0, 1);

            this.Controls.Add(pnlRoot);
            this.ResumeLayout(true);

            // Start Live Clock
            timerClock = new System.Windows.Forms.Timer { Interval = 1000 };
            timerClock.Tick += (s, e) =>
            {
                if (lblClock != null && !lblClock.IsDisposed)
                    lblClock.Text = "🕒 Thời gian hiện tại: " + DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy");
            };
            timerClock.Start();
        }

        private TableLayoutPanel CreateStatSummary(float scale)
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                ColumnCount = 4,
                RowCount = 1,
                Dock = DockStyle.Fill
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            table.RowStyles.Clear();
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            table.Controls.Add(CreateStatPill("TỔNG BUỔI ĐIỂM DANH", "0 Buổi", UITheme.Primary, scale, out lblTotalVal), 0, 0);
            table.Controls.Add(CreateStatPill("PT ĐÃ DUYỆT", "0 Buổi", UITheme.Success, scale, out lblApprovedVal), 1, 0);
            table.Controls.Add(CreateStatPill("ĐANG CHỜ DUYỆT", "0 Buổi", UITheme.Warning, scale, out lblPendingVal), 2, 0);
            table.Controls.Add(CreateStatPill("TỶ LỆ CHUYÊN CẦN", "100%", UITheme.Secondary, scale, out lblRateVal), 3, 0);

            return table;
        }

        private RoundedPanel CreateStatPill(string label, string val, Color color, float scale, out Label outValLabel)
        {
            RoundedPanel card = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                CornerRadius = 10,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Margin = new Padding((int)(4 * scale)),
                Padding = new Padding((int)(12 * scale), (int)(10 * scale), (int)(12 * scale), (int)(8 * scale))
            };

            FlowLayoutPanel flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Transparent
            };

            Label lbl = new Label { Text = label, Font = UITheme.FontSmallBold, ForeColor = UITheme.TextMuted, AutoSize = true, Margin = new Padding(0, 0, 0, (int)(2 * scale)), UseMnemonic = false };
            outValLabel = new Label { Text = val, Font = new Font("Segoe UI", 13.5f, FontStyle.Bold), ForeColor = color, AutoSize = true, Margin = new Padding(0), UseMnemonic = false };

            flow.Controls.Add(lbl);
            flow.Controls.Add(outValLabel);
            card.Controls.Add(flow);
            return card;
        }

        private RoundedPanel CreateUploadCheckInCard(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding((int)(18 * scale))
            };

            Label lblTitle = new Label
            {
                Text = "📸 CHECK-IN & XÁC NHẬN BUỔI TẬP",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = (int)(28 * scale),
                UseMnemonic = false
            };

            lblClock = new Label
            {
                Text = "🕒 Thời gian hiện tại: " + DateTime.Now.ToString("HH:mm:ss - dd/MM/yyyy"),
                Font = UITheme.FontSmallBold,
                ForeColor = UITheme.Primary,
                Dock = DockStyle.Top,
                Height = (int)(24 * scale),
                UseMnemonic = false
            };

            TableLayoutPanel pnlForm = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 8,
                AutoScroll = true,
                Padding = new Padding(0, (int)(6 * scale), 0, 0)
            };
            pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // 1. PT
            Label lblPT = new Label { Text = "Hình thức tập & Huấn luyện viên (PT):", Font = UITheme.FontSmallBold, ForeColor = UITheme.TextDark, AutoSize = true, Margin = new Padding(0, 0, 0, (int)(2 * scale)), UseMnemonic = false };
            cboCheckInPT = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Top,
                Height = (int)(30 * scale),
                Font = UITheme.FontRegular
            };

            // Populate PT dropdown
            cboCheckInPT.Items.Add(new CheckInPtItem { Text = "Tập tự do (Không kèm PT - Tự động duyệt)", Value = null });
            var pts = MemberService.Instance.GetAllPTs();
            foreach (var pt in pts)
            {
                cboCheckInPT.Items.Add(new CheckInPtItem { Text = $"HLV {pt.FullName} ({pt.Specialization})", Value = pt.PTID });
            }
            cboCheckInPT.SelectedIndex = 0;

            // 2. Photo Label & Preview
            Label lblPhoto = new Label { Text = "Ảnh chụp check-in tại phòng tập:", Font = UITheme.FontSmallBold, ForeColor = UITheme.TextDark, AutoSize = true, Margin = new Padding(0, (int)(6 * scale), 0, (int)(2 * scale)), UseMnemonic = false };
            picPreview = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = (int)(130 * scale),
                BackColor = Color.FromArgb(241, 245, 249),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            lblPlaceholder = new Label
            {
                Text = "📷 Chưa chọn ảnh\nBấm nút bên dưới để tải ảnh từ máy tính",
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextMuted,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                UseMnemonic = false
            };
            picPreview.Controls.Add(lblPlaceholder);

            // Buttons select / clear
            FlowLayoutPanel pnlImgBtns = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = (int)(38 * scale),
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, (int)(4 * scale), 0, (int)(4 * scale)),
                BackColor = Color.Transparent
            };

            Button btnChooseImg = new Button
            {
                Text = "📁 Chọn tệp ảnh...",
                Font = UITheme.FontSmallBold,
                Size = new Size((int)(140 * scale), (int)(32 * scale)),
                BackColor = UITheme.PrimaryLight,
                ForeColor = UITheme.Primary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnChooseImg.FlatAppearance.BorderSize = 0;
            btnChooseImg.Click += (s, e) =>
            {
                using OpenFileDialog ofd = new OpenFileDialog
                {
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
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
                Size = new Size((int)(95 * scale), (int)(32 * scale)),
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = UITheme.TextMuted,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false
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

            // 3. Note
            Label lblNote = new Label { Text = "Ghi chú buổi tập & Cảm nhận thể lực:", Font = UITheme.FontSmallBold, ForeColor = UITheme.TextDark, AutoSize = true, Margin = new Padding(0, (int)(4 * scale), 0, (int)(2 * scale)), UseMnemonic = false };
            txtCheckInNote = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Top,
                Height = (int)(75 * scale),
                Font = UITheme.FontRegular,
                ScrollBars = ScrollBars.Vertical,
                Text = "Hoàn thành buổi tập luyện tự do tại phòng gym."
            };

            // 4. Submit
            Button btnSubmitCheckIn = new Button
            {
                Text = "🚀 GỬI YÊU CẦU ĐIỂM DANH CHECK-IN",
                Font = UITheme.FontBold,
                Height = (int)(42 * scale),
                Dock = DockStyle.Top,
                Margin = new Padding(0, (int)(8 * scale), 0, 0),
                BackColor = Color.FromArgb(16, 185, 129), // Emerald
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnSubmitCheckIn.FlatAppearance.BorderSize = 0;
            btnSubmitCheckIn.Click += (s, e) =>
            {
                int? ptId = (cboCheckInPT.SelectedItem as CheckInPtItem)?.Value;
                string ptName = (cboCheckInPT.SelectedItem as CheckInPtItem)?.Text ?? "Cổng tự do";
                string note = txtCheckInNote.Text.Trim();

                bool ok = MemberService.Instance.SaveCheckIn(ptId, note, selectedImagePath);
                if (ok)
                {
                    LoadCheckInHistory();
                    RefreshStatSummary();
                    MessageBox.Show(
                        ptId.HasValue
                            ? $"Đã gửi yêu cầu điểm danh thành công!\n{ptName} sẽ kiểm tra và phê duyệt buổi tập cho bạn."
                            : "Điểm danh vào cửa thành công! Chúc bạn có buổi rèn luyện hiệu quả.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show("Không thể lưu thông tin điểm danh vào cơ sở dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            pnlForm.Controls.Add(lblPT);
            pnlForm.Controls.Add(cboCheckInPT);
            pnlForm.Controls.Add(lblPhoto);
            pnlForm.Controls.Add(picPreview);
            pnlForm.Controls.Add(pnlImgBtns);
            pnlForm.Controls.Add(lblNote);
            pnlForm.Controls.Add(txtCheckInNote);
            pnlForm.Controls.Add(btnSubmitCheckIn);

            card.Controls.Add(pnlForm);
            card.Controls.Add(lblClock);
            card.Controls.Add(lblTitle);

            return card;
        }

        private RoundedPanel CreateHistoryCard(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding((int)(18 * scale))
            };

            Label lblTitle = new Label
            {
                Text = "📜 LỊCH SỬ ĐIỂM DANH & PHÊ DUYỆT",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = (int)(32 * scale),
                UseMnemonic = false
            };

            dgvHistory = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgvHistory);
            dgvHistory.RowTemplate.Height = (int)Math.Round(36f * scale);
            dgvHistory.ColumnHeadersHeight = (int)Math.Round(34f * scale);

            dgvHistory.Columns.Add(UITheme.CreateFillColumn("CheckInID", "Mã", 13f, 75));
            dgvHistory.Columns.Add(UITheme.CreateFillColumn("Time", "Thời gian", 18f, 105));
            dgvHistory.Columns.Add(UITheme.CreateFillColumn("PT", "Hình thức / HLV", 24f, 130));
            dgvHistory.Columns.Add(UITheme.CreateFillColumn("Note", "Ghi chú buổi tập", 29f, 130));
            dgvHistory.Columns.Add(UITheme.CreateFillColumn("Status", "Trạng thái", 16f, 95));

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
                    else if (status.Contains("Chờ duyệt"))
                    {
                        e.CellStyle.ForeColor = UITheme.Warning;
                        e.CellStyle.Font = UITheme.FontBold;
                    }
                }
            };

            card.Controls.Add(dgvHistory);
            card.Controls.Add(lblTitle);

            return card;
        }

        private void LoadCheckInHistory()
        {
            if (dgvHistory == null) return;
            dgvHistory.Rows.Clear();

            try
            {
                var checkIns = MemberService.Instance.GetRecentCheckIns();
                if (checkIns.Count > 0)
                {
                    foreach (var c in checkIns)
                    {
                        string code = $"#CK-{c.CheckInID:D3}";
                        string timeStr = c.CheckInTime.ToString("dd/MM/yyyy HH:mm");
                        string ptName = c.PT?.FullName ?? (c.PTID == null ? "Cổng tự do (Quẹt thẻ)" : "Quầy lễ tân");
                        string note = !string.IsNullOrEmpty(c.Note) ? c.Note : "Check-in tập luyện";
                        string status = c.Status == "Approved" ? "Đã duyệt" : (c.Status == "Pending" ? "Chờ duyệt" : c.Status);
                        dgvHistory.Rows.Add(code, timeStr, ptName, note, status);
                    }
                }
                else
                {
                    dgvHistory.Rows.Add("#--", "--", "Chưa có lượt điểm danh", "Hãy check-in khi đến phòng tập", "--");
                }
            }
            catch
            {
                dgvHistory.Rows.Add("#--", "--", "Lỗi tải lịch sử check-in từ CSDL", "--", "--");
            }
        }

        private void RefreshStatSummary()
        {
            try
            {
                var list = MemberService.Instance.GetRecentCheckIns();
                int total = list.Count;
                int approved = list.Count(c => c.Status == "Approved");
                int pending = list.Count(c => c.Status == "Pending");
                int rate = total > 0 ? (int)Math.Round((double)approved / total * 100) : 100;

                if (lblTotalVal != null) lblTotalVal.Text = $"{total} Buổi";
                if (lblApprovedVal != null) lblApprovedVal.Text = $"{approved} Buổi";
                if (lblPendingVal != null) lblPendingVal.Text = $"{pending} Buổi";
                if (lblRateVal != null) lblRateVal.Text = $"{rate}% (Chuyên cần)";
            }
            catch { }
        }
    }
}
