using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyGym.UI.Utils;

namespace QuanLyGym.UI.Forms.Member.Views
{
    public class ucMemberProgress : UserControl
    {
        private NumericUpDown numWeight = null!;
        private NumericUpDown numHeight = null!;
        private NumericUpDown numBodyFat = null!;
        private NumericUpDown numMuscle = null!;
        private NumericUpDown numChest = null!;
        private NumericUpDown numWaist = null!;
        private NumericUpDown numHip = null!;
        private NumericUpDown numArm = null!;
        private NumericUpDown numThigh = null!;
        private Label lblCalcBMI = null!;
        private Label lblBMICategory = null!;
        private DataGridView dgvProgress = null!;

        public ucMemberProgress()
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

            // Title
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 50 };
            Label lblTitle = new Label
            {
                Text = "📊 THEO DÕI CHỈ SỐ CƠ THỂ & TIẾN TRÌNH INBODY",
                Font = UITheme.FontTitle,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(0, 8)
            };
            pnlHeader.Controls.Add(lblTitle);

            // Metrics Summary Row
            TableLayoutPanel pnlStats = CreateTopMetrics();
            pnlStats.Dock = DockStyle.Top;
            pnlStats.Margin = new Padding(0, 0, 0, 16);

            // Two Column Layout
            TableLayoutPanel pnlCols = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 10, 0, 10)
            };
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46f));
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54f));

            RoundedPanel cardForm = CreateRecordFormCard();
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
        }

        private TableLayoutPanel CreateTopMetrics()
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                ColumnCount = 5,
                RowCount = 1,
                Height = 95,
                Dock = DockStyle.Top
            };
            for (int i = 0; i < 5; i++) table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));

            table.Controls.Add(CreateStatPill("CÂN NẶNG HIỆN TẠI", "68.0 kg", "Mục tiêu: 66 kg", UITheme.Primary), 0, 0);
            table.Controls.Add(CreateStatPill("CHIỀU CAO", "172 cm", "Chuẩn", UITheme.TextDark), 1, 0);
            table.Controls.Add(CreateStatPill("CHỈ SỐ BMI", "23.0", "Cân đối", UITheme.Success), 2, 0);
            table.Controls.Add(CreateStatPill("TỶ LỆ MỠ (BODY FAT)", "18.0 %", "Mục tiêu: 15%", UITheme.Warning), 3, 0);
            table.Controls.Add(CreateStatPill("KHỐI LƯỢNG CƠ", "32.5 kg", "+1.2 kg tăng", UITheme.Secondary), 4, 0);

            return table;
        }

        private RoundedPanel CreateStatPill(string label, string val, string sub, Color color)
        {
            RoundedPanel card = new RoundedPanel
            {
                Dock = DockStyle.Fill,
                CornerRadius = 10,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Margin = new Padding(5),
                Padding = new Padding(14, 10, 14, 10)
            };

            Label lbl = new Label { Text = label, Font = UITheme.FontSmallBold, ForeColor = UITheme.TextMuted, Location = new Point(12, 8), AutoSize = true };
            Label v = new Label { Text = val, Font = new Font("Segoe UI", 14f, FontStyle.Bold), ForeColor = color, Location = new Point(12, 28), AutoSize = true };
            Label s = new Label { Text = sub, Font = UITheme.FontSmall, ForeColor = UITheme.TextMuted, Location = new Point(12, 54), AutoSize = true };

            card.Controls.Add(lbl);
            card.Controls.Add(v);
            card.Controls.Add(s);
            return card;
        }

        private RoundedPanel CreateRecordFormCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 620,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(22)
            };

            // Add responsive resize handling for input widths
            card.Resize += (s, e) =>
            {
                // Determine suitable width (leave some margin)
                int w = Math.Max(260, card.ClientSize.Width - 40);
                // Adjust widths of input controls inside the form panel
                foreach (Control outer in card.Controls)
                {
                    if (outer is Panel pnlForm)
                    {
                        foreach (Control c in pnlForm.Controls)
                        {
                            if (c is DateTimePicker) c.Width = w;
                            else if (c is NumericUpDown) c.Width = w;
                            else if (c is Button btn && btn.Text.Contains("LƯU")) btn.Width = w;
                        }
                    }
                }
            };

            Label lblTitle = new Label
            {
                Text = " GHI NHẬN KẾT QUẢ INBODY MỚI",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = 32
            };

            Panel pnlForm = new Panel { Dock = DockStyle.Fill, AutoScroll = true };

            int y = 6;

            // Date & Height / Weight
            pnlForm.Controls.Add(new Label { Text = "Ngày kiểm tra đo chỉ số:", Font = UITheme.FontSmallBold, Location = new Point(4, y), AutoSize = true });
            y += 22;
            DateTimePicker dtp = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(4, y), Width = 260, Font = UITheme.FontRegular };
            pnlForm.Controls.Add(dtp);
            y += 38;

            // Height and Weight Row
            pnlForm.Controls.Add(new Label { Text = "Chiều cao (cm):", Font = UITheme.FontSmallBold, Location = new Point(4, y), AutoSize = true });
            pnlForm.Controls.Add(new Label { Text = "Cân nặng (kg):", Font = UITheme.FontSmallBold, Location = new Point(140, y), AutoSize = true });
            y += 22;

            numHeight = new NumericUpDown { Location = new Point(4, y), Width = 120, Minimum = 100, Maximum = 220, Value = 172, DecimalPlaces = 1, Font = UITheme.FontRegular };
            numWeight = new NumericUpDown { Location = new Point(140, y), Width = 120, Minimum = 30, Maximum = 200, Value = 68, DecimalPlaces = 1, Font = UITheme.FontRegular };
            numHeight.ValueChanged += (s, e) => RecalculateBMI();
            numWeight.ValueChanged += (s, e) => RecalculateBMI();

            pnlForm.Controls.Add(numHeight);
            pnlForm.Controls.Add(numWeight);
            y += 38;

            // Calculated BMI Box
            Panel pnlBMIBox = new Panel { Location = new Point(4, y), Size = new Size(260, 48), BackColor = UITheme.PrimaryLight };
            lblCalcBMI = new Label { Text = "BMI: 23.0", Font = UITheme.FontBold, ForeColor = UITheme.Primary, Location = new Point(10, 12), AutoSize = true };
            lblBMICategory = new Label { Text = "(Thể trạng: Cân đối)", Font = UITheme.FontSmallBold, ForeColor = UITheme.Success, Location = new Point(90, 14), AutoSize = true };
            pnlBMIBox.Controls.Add(lblCalcBMI);
            pnlBMIBox.Controls.Add(lblBMICategory);
            pnlForm.Controls.Add(pnlBMIBox);
            y += 56;

            // Body Fat % and Muscle Mass
            pnlForm.Controls.Add(new Label { Text = "Tỷ lệ mỡ Body Fat (%):", Font = UITheme.FontSmallBold, Location = new Point(4, y), AutoSize = true });
            pnlForm.Controls.Add(new Label { Text = "Cơ bắp (kg):", Font = UITheme.FontSmallBold, Location = new Point(140, y), AutoSize = true });
            y += 22;

            numBodyFat = new NumericUpDown { Location = new Point(4, y), Width = 120, Minimum = 3, Maximum = 60, Value = 18, DecimalPlaces = 1, Font = UITheme.FontRegular };
            numMuscle = new NumericUpDown { Location = new Point(140, y), Width = 120, Minimum = 10, Maximum = 100, Value = (decimal)32.5, DecimalPlaces = 1, Font = UITheme.FontRegular };
            pnlForm.Controls.Add(numBodyFat);
            pnlForm.Controls.Add(numMuscle);
            y += 38;

            // Measurements Row 1: Chest & Waist
            pnlForm.Controls.Add(new Label { Text = "Vòng ngực (cm):", Font = UITheme.FontSmallBold, Location = new Point(4, y), AutoSize = true });
            pnlForm.Controls.Add(new Label { Text = "Vòng eo (cm):", Font = UITheme.FontSmallBold, Location = new Point(140, y), AutoSize = true });
            y += 22;

            numChest = new NumericUpDown { Location = new Point(4, y), Width = 120, Minimum = 50, Maximum = 150, Value = 96, DecimalPlaces = 1, Font = UITheme.FontRegular };
            numWaist = new NumericUpDown { Location = new Point(140, y), Width = 120, Minimum = 40, Maximum = 150, Value = 78, DecimalPlaces = 1, Font = UITheme.FontRegular };
            pnlForm.Controls.Add(numChest);
            pnlForm.Controls.Add(numWaist);
            y += 38;

            // Measurements Row 2: Hip & Arm
            pnlForm.Controls.Add(new Label { Text = "Vòng mông (cm):", Font = UITheme.FontSmallBold, Location = new Point(4, y), AutoSize = true });
            pnlForm.Controls.Add(new Label { Text = "Bắp tay (cm):", Font = UITheme.FontSmallBold, Location = new Point(140, y), AutoSize = true });
            y += 22;

            numHip = new NumericUpDown { Location = new Point(4, y), Width = 120, Minimum = 50, Maximum = 160, Value = 95, DecimalPlaces = 1, Font = UITheme.FontRegular };
            numArm = new NumericUpDown { Location = new Point(140, y), Width = 120, Minimum = 15, Maximum = 65, Value = 34, DecimalPlaces = 1, Font = UITheme.FontRegular };
            pnlForm.Controls.Add(numHip);
            pnlForm.Controls.Add(numArm);
            y += 38;

            // Save Button
            Button btnSave = new Button
            {
                Text = "💾 LƯU CHỈ SỐ INBODY MỚI",
                Font = UITheme.FontBold,
                Height = 44,
                Width = 260,
                Location = new Point(4, y + 10),
                BackColor = UITheme.Primary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) =>
            {
                string dateStr = dtp.Value.ToString("dd/MM/yyyy");
                decimal w = numWeight.Value;
                decimal h = numHeight.Value;
                decimal fat = numBodyFat.Value;
                decimal muscle = numMuscle.Value;
                decimal bmi = Math.Round(w / ((h / 100) * (h / 100)), 1);

                dgvProgress.Rows.Insert(0, dateStr, $"{w} kg", $"{h} cm", bmi.ToString("0.0"), $"{fat}%", $"{muscle} kg", $"{numChest.Value}/{numWaist.Value} cm", "HLV Nam");
                MessageBox.Show($"Đã lưu kết quả đo InBody ngày {dateStr}!\nBMI: {bmi} | Cân nặng: {w} kg", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            pnlForm.Controls.Add(btnSave);

            card.Controls.Add(pnlForm);
            card.Controls.Add(lblTitle);

            return card;
        }

        private void RecalculateBMI()
        {
            if (numHeight != null && numWeight != null && lblCalcBMI != null && lblBMICategory != null)
            {
                decimal h = numHeight.Value / 100;
                decimal w = numWeight.Value;
                if (h > 0)
                {
                    decimal bmi = Math.Round(w / (h * h), 1);
                    lblCalcBMI.Text = $"BMI: {bmi:0.0}";

                    if (bmi < 18.5m)
                    {
                        lblBMICategory.Text = "(Thể trạng: Gầy)";
                        lblBMICategory.ForeColor = UITheme.Warning;
                    }
                    else if (bmi <= 24.9m)
                    {
                        lblBMICategory.Text = "(Thể trạng: Cân đối)";
                        lblBMICategory.ForeColor = UITheme.Success;
                    }
                    else
                    {
                        lblBMICategory.Text = "(Thể trạng: Thừa cân)";
                        lblBMICategory.ForeColor = UITheme.Danger;
                    }
                }
            }
        }

        private RoundedPanel CreateHistoryCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 620,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(20)
            };

            Label lblTitle = new Label
            {
                Text = "📜 LỊCH SỬ CÁC LẦN ĐO CHỈ SỐ INBODY",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = 34
            };

            // Summary Comparison Banner
            Panel pnlSummary = new Panel
            {
                Dock = DockStyle.Top,
                Height = 64,
                BackColor = Color.FromArgb(236, 253, 245), // Emerald light
                Padding = new Padding(12)
            };
            Label lblComp = new Label
            {
                Text = "🏆 Tiến trình ấn tượng so với mốc khởi điểm (01/01/2026):\n• Cân nặng: Giảm 4.0 kg | • Mỡ cơ thể: Giảm 4.2% | • Cơ bắp: Tăng +2.5 kg!",
                Font = UITheme.FontBold,
                ForeColor = Color.FromArgb(6, 95, 70),
                Dock = DockStyle.Fill
            };
            pnlSummary.Controls.Add(lblComp);

            dgvProgress = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgvProgress);

            dgvProgress.Columns.Add("Date", "Ngày đo");
            dgvProgress.Columns.Add("Weight", "Cân nặng");
            dgvProgress.Columns.Add("Height", "Chiều cao");
            dgvProgress.Columns.Add("BMI", "BMI");
            dgvProgress.Columns.Add("Fat", "Mỡ (%)");
            dgvProgress.Columns.Add("Muscle", "Cơ (kg)");
            dgvProgress.Columns.Add("Measure", "Ngực / Eo");
            dgvProgress.Columns.Add("PT", "HLV Đo");

            dgvProgress.Rows.Add("10/10/2026", "68.0 kg", "172 cm", "23.0", "18.0%", "32.5 kg", "96 / 78 cm", "HLV Nam");
            dgvProgress.Rows.Add("10/08/2026", "69.5 kg", "172 cm", "23.5", "19.5%", "31.8 kg", "95 / 80 cm", "HLV Nam");
            dgvProgress.Rows.Add("10/05/2026", "71.0 kg", "172 cm", "24.0", "21.0%", "31.0 kg", "94 / 82 cm", "HLV Nam");
            dgvProgress.Rows.Add("10/02/2026", "72.0 kg", "172 cm", "24.3", "22.2%", "30.0 kg", "93 / 84 cm", "HLV Nam");

            card.Controls.Add(dgvProgress);
            card.Controls.Add(pnlSummary);
            card.Controls.Add(lblTitle);

            return card;
        }
    }
}
