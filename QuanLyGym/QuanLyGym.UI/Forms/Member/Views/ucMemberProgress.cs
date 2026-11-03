using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyGym.UI.Services;
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
        private Label lblCalcBMI = null!;
        private Label lblBMICategory = null!;
        private Label lblComp = null!;
        private DataGridView dgvProgress = null!;

        // Top Metric Labels
        private Label lblWeightVal = null!;
        private Label lblHeightVal = null!;
        private Label lblBMIVal = null!;
        private Label lblBMISub = null!;
        private Label lblFatVal = null!;
        private Label lblMuscleVal = null!;

        public ucMemberProgress()
        {
            InitializeComponent();
            LoadProgressData();
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
            int statHeight = (int)Math.Round(118f * scale);
            pnlRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, statHeight)); // Row 0: Stat cards
            pnlRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));        // Row 1: Content

            // Row 0: Top Stat Pills
            TableLayoutPanel pnlStats = CreateTopMetrics(scale);
            pnlStats.Dock = DockStyle.Fill;
            pnlStats.Margin = new Padding(0, 0, 0, (int)(8 * scale));
            pnlRoot.Controls.Add(pnlStats, 0, 0);

            // Row 1: Two Column Layout (Form 42%, History Table 58%)
            TableLayoutPanel pnlCols = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(0)
            };
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42f));
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58f));
            pnlCols.RowStyles.Clear();
            pnlCols.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            RoundedPanel cardForm = CreateRecordFormCard(scale);
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
        }

        private TableLayoutPanel CreateTopMetrics(float scale)
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                ColumnCount = 5,
                RowCount = 1,
                Dock = DockStyle.Fill
            };
            for (int i = 0; i < 5; i++) table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            table.RowStyles.Clear();
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            table.Controls.Add(CreateStatPill("CÂN NẶNG HIỆN TẠI", "-- kg", "Chỉ số gần nhất", UITheme.Primary, scale, out lblWeightVal, out _), 0, 0);
            table.Controls.Add(CreateStatPill("CHIỀU CAO", "-- cm", "Đo InBody", UITheme.TextDark, scale, out lblHeightVal, out _), 1, 0);
            table.Controls.Add(CreateStatPill("CHỈ SỐ BMI", "--", "Thể trạng", UITheme.Success, scale, out lblBMIVal, out lblBMISub), 2, 0);
            table.Controls.Add(CreateStatPill("TỶ LỆ MỠ", "-- %", "Body Fat", UITheme.Warning, scale, out lblFatVal, out _), 3, 0);
            table.Controls.Add(CreateStatPill("KHỐI LƯỢNG CƠ", "-- kg", "Muscle Mass", UITheme.Secondary, scale, out lblMuscleVal, out _), 4, 0);

            return table;
        }

        private RoundedPanel CreateStatPill(string label, string val, string sub, Color color, float scale, out Label outVal, out Label outSub)
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
            outVal = new Label { Text = val, Font = new Font("Segoe UI", 13.5f, FontStyle.Bold), ForeColor = color, AutoSize = true, Margin = new Padding(0, 0, 0, (int)(2 * scale)), UseMnemonic = false };
            outSub = new Label { Text = sub, Font = UITheme.FontSmall, ForeColor = UITheme.TextMuted, AutoSize = true, Margin = new Padding(0), UseMnemonic = false };

            flow.Controls.Add(lbl);
            flow.Controls.Add(outVal);
            flow.Controls.Add(outSub);
            card.Controls.Add(flow);
            return card;
        }

        private RoundedPanel CreateRecordFormCard(float scale)
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
                Text = "📝 NHẬP CHỈ SỐ INBODY MỚI",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = (int)(28 * scale),
                UseMnemonic = false
            };

            TableLayoutPanel pnlForm = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 8,
                AutoScroll = true,
                Padding = new Padding(0, (int)(4 * scale), 0, 0)
            };
            pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // Date Row
            Label lblDate = new Label { Text = "Ngày đo:", Font = UITheme.FontSmallBold, AutoSize = true, UseMnemonic = false };
            DateTimePicker dtp = new DateTimePicker { Value = DateTime.Today, Format = DateTimePickerFormat.Short, Dock = DockStyle.Top, Font = UITheme.FontRegular };

            // Height & Weight Row
            TableLayoutPanel rowHW = new TableLayoutPanel { Dock = DockStyle.Top, Height = (int)(58 * scale), ColumnCount = 2, RowCount = 2 };
            rowHW.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            rowHW.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            Label lblH = new Label { Text = "Chiều cao (cm):", Font = UITheme.FontSmallBold, AutoSize = true, UseMnemonic = false };
            Label lblW = new Label { Text = "Cân nặng (kg):", Font = UITheme.FontSmallBold, AutoSize = true, UseMnemonic = false };
            numHeight = new NumericUpDown { Dock = DockStyle.Top, Minimum = 100, Maximum = 220, Value = 172, DecimalPlaces = 0, Font = UITheme.FontRegular };
            numWeight = new NumericUpDown { Dock = DockStyle.Top, Minimum = 30, Maximum = 200, Value = 68, DecimalPlaces = 1, Font = UITheme.FontRegular };
            numHeight.ValueChanged += (s, e) => RecalculateBMI();
            numWeight.ValueChanged += (s, e) => RecalculateBMI();
            rowHW.Controls.Add(lblH, 0, 0);
            rowHW.Controls.Add(lblW, 1, 0);
            rowHW.Controls.Add(numHeight, 0, 1);
            rowHW.Controls.Add(numWeight, 1, 1);

            // Real-time BMI Display Box
            Panel pnlBMIBox = new Panel
            {
                Dock = DockStyle.Top,
                Height = (int)(40 * scale),
                BackColor = UITheme.PrimaryLight,
                Margin = new Padding(0, (int)(4 * scale), 0, (int)(4 * scale)),
                Padding = new Padding((int)(8 * scale))
            };
            lblCalcBMI = new Label { Text = "BMI: 23.0", Font = UITheme.FontBold, ForeColor = UITheme.Primary, Dock = DockStyle.Left, AutoSize = true, UseMnemonic = false };
            lblBMICategory = new Label { Text = "(Thể trạng: Cân đối)", Font = UITheme.FontSmallBold, ForeColor = UITheme.Success, Dock = DockStyle.Right, AutoSize = true, UseMnemonic = false };
            pnlBMIBox.Controls.Add(lblCalcBMI);
            pnlBMIBox.Controls.Add(lblBMICategory);

            // Body Fat & Muscle Row
            TableLayoutPanel rowFM = new TableLayoutPanel { Dock = DockStyle.Top, Height = (int)(58 * scale), ColumnCount = 2, RowCount = 2 };
            rowFM.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            rowFM.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            Label lblFat = new Label { Text = "Tỷ lệ mỡ (%):", Font = UITheme.FontSmallBold, AutoSize = true, UseMnemonic = false };
            Label lblMus = new Label { Text = "Cơ bắp (kg):", Font = UITheme.FontSmallBold, AutoSize = true, UseMnemonic = false };
            numBodyFat = new NumericUpDown { Dock = DockStyle.Top, Minimum = 3, Maximum = 60, Value = 18, DecimalPlaces = 1, Font = UITheme.FontRegular };
            numMuscle = new NumericUpDown { Dock = DockStyle.Top, Minimum = 10, Maximum = 100, Value = (decimal)32.5, DecimalPlaces = 1, Font = UITheme.FontRegular };
            rowFM.Controls.Add(lblFat, 0, 0);
            rowFM.Controls.Add(lblMus, 1, 0);
            rowFM.Controls.Add(numBodyFat, 0, 1);
            rowFM.Controls.Add(numMuscle, 1, 1);

            // Chest & Waist Row
            TableLayoutPanel rowCW = new TableLayoutPanel { Dock = DockStyle.Top, Height = (int)(58 * scale), ColumnCount = 2, RowCount = 2 };
            rowCW.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            rowCW.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            Label lblC = new Label { Text = "Vòng ngực (cm):", Font = UITheme.FontSmallBold, AutoSize = true, UseMnemonic = false };
            Label lblWa = new Label { Text = "Vòng eo (cm):", Font = UITheme.FontSmallBold, AutoSize = true, UseMnemonic = false };
            numChest = new NumericUpDown { Dock = DockStyle.Top, Minimum = 50, Maximum = 150, Value = 96, DecimalPlaces = 1, Font = UITheme.FontRegular };
            numWaist = new NumericUpDown { Dock = DockStyle.Top, Minimum = 40, Maximum = 150, Value = 78, DecimalPlaces = 1, Font = UITheme.FontRegular };
            rowCW.Controls.Add(lblC, 0, 0);
            rowCW.Controls.Add(lblWa, 1, 0);
            rowCW.Controls.Add(numChest, 0, 1);
            rowCW.Controls.Add(numWaist, 1, 1);

            // Hip & Arm Row
            TableLayoutPanel rowHA = new TableLayoutPanel { Dock = DockStyle.Top, Height = (int)(58 * scale), ColumnCount = 2, RowCount = 2 };
            rowHA.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            rowHA.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            Label lblHip = new Label { Text = "Vòng mông (cm):", Font = UITheme.FontSmallBold, AutoSize = true, UseMnemonic = false };
            Label lblArm = new Label { Text = "Bắp tay (cm):", Font = UITheme.FontSmallBold, AutoSize = true, UseMnemonic = false };
            numHip = new NumericUpDown { Dock = DockStyle.Top, Minimum = 50, Maximum = 160, Value = 95, DecimalPlaces = 1, Font = UITheme.FontRegular };
            numArm = new NumericUpDown { Dock = DockStyle.Top, Minimum = 15, Maximum = 65, Value = 34, DecimalPlaces = 1, Font = UITheme.FontRegular };
            rowHA.Controls.Add(lblHip, 0, 0);
            rowHA.Controls.Add(lblArm, 1, 0);
            rowHA.Controls.Add(numHip, 0, 1);
            rowHA.Controls.Add(numArm, 1, 1);

            // Save Button
            Button btnSave = new Button
            {
                Text = "💾 LƯU CHỈ SỐ INBODY MỚI",
                Font = UITheme.FontBold,
                Height = (int)(42 * scale),
                Dock = DockStyle.Top,
                Margin = new Padding(0, (int)(8 * scale), 0, 0),
                BackColor = UITheme.Primary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) =>
            {
                decimal w = numWeight.Value;
                decimal h = numHeight.Value;
                decimal fat = numBodyFat.Value;
                decimal muscle = numMuscle.Value;
                decimal chest = numChest.Value;
                decimal waist = numWaist.Value;
                decimal hip = numHip.Value;
                decimal arm = numArm.Value;

                bool ok = MemberService.Instance.SaveProgress(w, h, fat, muscle, chest, waist, hip, arm, 0, "Hội viên tự ghi nhận chỉ số InBody");
                if (ok)
                {
                    LoadProgressData();
                    MessageBox.Show("Đã lưu chỉ số InBody mới thành công vào cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không thể lưu chỉ số InBody vào cơ sở dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            pnlForm.Controls.Add(lblDate);
            pnlForm.Controls.Add(dtp);
            pnlForm.Controls.Add(rowHW);
            pnlForm.Controls.Add(pnlBMIBox);
            pnlForm.Controls.Add(rowFM);
            pnlForm.Controls.Add(rowCW);
            pnlForm.Controls.Add(rowHA);
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
                Text = "📜 LỊCH SỬ CÁC LẦN ĐO CHỈ SỐ INBODY",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = (int)(32 * scale),
                UseMnemonic = false
            };

            // Summary Comparison Banner
            Panel pnlSummary = new Panel
            {
                Dock = DockStyle.Top,
                Height = (int)(56 * scale),
                BackColor = Color.FromArgb(236, 253, 245), // Emerald light
                Padding = new Padding((int)(10 * scale)),
                Margin = new Padding(0, 0, 0, (int)(8 * scale))
            };
            lblComp = new Label
            {
                Text = "💡 Đang tải dữ liệu tiến trình...",
                Font = UITheme.FontBold,
                ForeColor = Color.FromArgb(6, 95, 70),
                Dock = DockStyle.Fill,
                UseMnemonic = false
            };
            pnlSummary.Controls.Add(lblComp);

            dgvProgress = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgvProgress);
            dgvProgress.RowTemplate.Height = (int)Math.Round(36f * scale);
            dgvProgress.ColumnHeadersHeight = (int)Math.Round(34f * scale);

            dgvProgress.Columns.Add(UITheme.CreateFillColumn("Date", "Ngày đo", 13f, 80));
            dgvProgress.Columns.Add(UITheme.CreateFillColumn("Weight", "Cân nặng", 12f, 75));
            dgvProgress.Columns.Add(UITheme.CreateFillColumn("Height", "Chiều cao", 12f, 75));
            dgvProgress.Columns.Add(UITheme.CreateFillColumn("BMI", "BMI", 10f, 60));
            dgvProgress.Columns.Add(UITheme.CreateFillColumn("Fat", "Mỡ (%)", 11f, 65));
            dgvProgress.Columns.Add(UITheme.CreateFillColumn("Muscle", "Cơ (kg)", 12f, 70));
            dgvProgress.Columns.Add(UITheme.CreateFillColumn("Measure", "Ngực / Eo", 16f, 95));
            dgvProgress.Columns.Add(UITheme.CreateFillColumn("PT", "Người đo", 14f, 85));

            card.Controls.Add(dgvProgress);
            card.Controls.Add(pnlSummary);
            card.Controls.Add(lblTitle);

            return card;
        }

        private void LoadProgressData()
        {
            if (dgvProgress == null) return;
            dgvProgress.Rows.Clear();

            try
            {
                var history = MemberService.Instance.GetProgressHistory();
                if (history.Count > 0)
                {
                    foreach (var p in history)
                    {
                        string dateStr = p.RecordDate.ToString("dd/MM/yyyy");
                        string wStr = $"{p.Weight:0.0} kg";
                        string hStr = $"{p.Height:0.#} cm";
                        string bmiStr = $"{p.BMI:0.0}";
                        string fatStr = $"{p.BodyFat:0.0}%";
                        string musStr = $"{p.MuscleMass:0.0} kg";
                        string measureStr = $"{p.Chest:0} / {p.Waist:0} cm";
                        string ptStr = p.PT?.FullName ?? "Tự đo";

                        dgvProgress.Rows.Add(dateStr, wStr, hStr, bmiStr, fatStr, musStr, measureStr, ptStr);
                    }

                    // Top metric cards
                    var latest = history.First();
                    if (lblWeightVal != null) lblWeightVal.Text = $"{latest.Weight:0.0} kg";
                    if (lblHeightVal != null) lblHeightVal.Text = $"{latest.Height:0.#} cm";
                    if (lblBMIVal != null) lblBMIVal.Text = $"{latest.BMI:0.0}";
                    if (lblBMISub != null)
                    {
                        if (latest.BMI < 18.5m) lblBMISub.Text = "Gầy";
                        else if (latest.BMI <= 24.9m) lblBMISub.Text = "Cân đối";
                        else lblBMISub.Text = "Thừa cân";
                    }
                    if (lblFatVal != null) lblFatVal.Text = $"{latest.BodyFat:0.0} %";
                    if (lblMuscleVal != null) lblMuscleVal.Text = $"{latest.MuscleMass:0.0} kg";

                    // Form defaults
                    if (numWeight != null) numWeight.Value = latest.Weight;
                    if (numHeight != null) numHeight.Value = latest.Height;
                    if (numBodyFat != null) numBodyFat.Value = latest.BodyFat;
                    if (numMuscle != null) numMuscle.Value = latest.MuscleMass;
                    if (numChest != null) numChest.Value = latest.Chest;
                    if (numWaist != null) numWaist.Value = latest.Waist;
                    if (numHip != null) numHip.Value = latest.Hip;
                    if (numArm != null) numArm.Value = latest.Arm;
                    RecalculateBMI();

                    // Comparison summary
                    if (lblComp != null)
                    {
                        if (history.Count >= 2)
                        {
                            var first = history.Last();
                            decimal wDiff = latest.Weight - first.Weight;
                            decimal fDiff = latest.BodyFat - first.BodyFat;
                            decimal mDiff = latest.MuscleMass - first.MuscleMass;
                            string wText = wDiff <= 0 ? $"Giảm {Math.Abs(wDiff):0.0} kg" : $"Tăng +{wDiff:0.0} kg";
                            string fText = fDiff <= 0 ? $"Giảm {Math.Abs(fDiff):0.0}%" : $"Tăng +{fDiff:0.0}%";
                            string mText = mDiff >= 0 ? $"Tăng +{mDiff:0.0} kg" : $"Giảm {Math.Abs(mDiff):0.0} kg";

                            lblComp.Text = $"🏆 Tiến trình so với mốc ban đầu ({first.RecordDate:dd/MM/yyyy}):\n• Cân nặng: {wText} | • Mỡ: {fText} | • Cơ: {mText}!";
                        }
                        else
                        {
                            lblComp.Text = $"📊 Lần đo gần nhất ngày {latest.RecordDate:dd/MM/yyyy}. Hãy duy trì luyện tập và đo lại sau mỗi tháng để theo dõi tiến độ!";
                        }
                    }
                }
                else
                {
                    dgvProgress.Rows.Add("--", "-- kg", "-- cm", "--", "--%", "-- kg", "--", "Chưa có dữ liệu");
                    if (lblComp != null)
                    {
                        lblComp.Text = "💡 Chưa có dữ liệu InBody. Hãy nhập chỉ số đo bên trái để bắt đầu theo dõi tiến độ cơ thể!";
                    }
                }
            }
            catch
            {
                dgvProgress.Rows.Add("--", "Lỗi CSDL", "--", "--", "--", "--", "--", "--");
            }
        }
    }
}
