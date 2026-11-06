using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.PT
{
    public partial class ucTheoDoiTheChat : UserControl
    {
        public class MemberPhysicalProfile
        {
            public string MemberCode { get; set; } = "";
            public string FullName { get; set; } = "";
            public string Initials { get; set; } = "";
            public string Phone { get; set; } = "";
            public string PackageName { get; set; } = "";
            public decimal CurrentWeight { get; set; }
            public decimal Height { get; set; }
            public decimal BodyFat { get; set; }
            public decimal Waist { get; set; }
            public decimal BMI { get; set; }
            public string BmiStatus { get; set; } = "Chuẩn";
            public DateTime LastRecordedDate { get; set; }
            public string[] MonthLabels { get; set; } = Array.Empty<string>();
            public double[] WeightHistory { get; set; } = Array.Empty<double>();
            public double[] BodyFatHistory { get; set; } = Array.Empty<double>();
            public string BenchPressPR { get; set; } = "75 kg";
            public string SquatPR { get; set; } = "105 kg";
            public string DeadliftPR { get; set; } = "130 kg";
            public Color AvatarColor { get; set; } = PTUITheme.PrimaryLime;
        }

        private List<MemberPhysicalProfile> _profiles = new List<MemberPhysicalProfile>();
        private MemberPhysicalProfile _currentProfile = null!;

        public ucTheoDoiTheChat()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = PTUITheme.BgDark;

            InitProfiles();
            _currentProfile = _profiles[0];
            LoadProfileToUI(_currentProfile);

            pnlChart.Paint += PnlChart_Paint;
            pnlChart.Resize += (s, e) => pnlChart.Invalidate();

            pnlRecordsCard.Paint += PnlRecordsCard_Paint;
            pnlRecordsCard.Resize += (s, e) => pnlRecordsCard.Invalidate();

            pnlMeasurementsCard.Paint += PnlMeasurementsCard_Paint;
            pnlMeasurementsCard.Resize += (s, e) =>
            {
                RelayoutMeasurementsControls();
                pnlMeasurementsCard.Invalidate();
            };

            pnlBottomRow.Resize += (s, e) =>
            {
                pnlMeasurementsCard.Width = (pnlBottomRow.ClientSize.Width - 16) / 2;
            };

            pnlMemberBanner.Paint += PnlMemberBanner_Paint;
            pnlMemberBanner.Resize += (s, e) => pnlMemberBanner.Invalidate();

            btnSaveMeasurement.MouseEnter += (s, e) => { btnSaveMeasurement.BackColor = PTUITheme.LimeHover; btnSaveMeasurement.Invalidate(); };
            btnSaveMeasurement.MouseLeave += (s, e) => { btnSaveMeasurement.BackColor = PTUITheme.PrimaryLime; btnSaveMeasurement.Invalidate(); };
            btnSaveMeasurement.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle br = btnSaveMeasurement.ClientRectangle;
                using (SolidBrush sb = new SolidBrush(btnSaveMeasurement.BackColor))
                {
                    PTUITheme.FillRoundedRectangle(pe.Graphics, sb, br, 8);
                }
                TextRenderer.DrawText(pe.Graphics, btnSaveMeasurement.Text, btnSaveMeasurement.Font, br, Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            };
        }

        private void InitProfiles()
        {
            _profiles = new List<MemberPhysicalProfile>
            {
                new MemberPhysicalProfile
                {
                    MemberCode = "HV001",
                    FullName = "Nguyễn Đức An",
                    Initials = "DA",
                    Phone = "0912.345.678",
                    PackageName = "Tăng Cơ Hypertrophy",
                    CurrentWeight = 72.0m,
                    Height = 175m,
                    BodyFat = 16.5m,
                    Waist = 79m,
                    BMI = 23.5m,
                    BmiStatus = "Chuẩn",
                    LastRecordedDate = new DateTime(2026, 11, 4),
                    MonthLabels = new string[] { "T1 (78k)", "T2 (77k)", "T3 (76k)", "T4 (74.8k)", "T5 (73.5k)", "T6 (72k)" },
                    WeightHistory = new double[] { 78.0, 77.0, 76.0, 74.8, 73.5, 72.0 },
                    BodyFatHistory = new double[] { 22.0, 20.5, 19.0, 18.2, 17.0, 16.5 },
                    BenchPressPR = "75 kg",
                    SquatPR = "105 kg",
                    DeadliftPR = "130 kg",
                    AvatarColor = PTUITheme.PrimaryLime
                },
                new MemberPhysicalProfile
                {
                    MemberCode = "HV002",
                    FullName = "Phạm Mai Linh",
                    Initials = "ML",
                    Phone = "0988.765.432",
                    PackageName = "Body Transformation Siết Mỡ",
                    CurrentWeight = 52.0m,
                    Height = 162m,
                    BodyFat = 20.8m,
                    Waist = 64m,
                    BMI = 19.8m,
                    BmiStatus = "Thon Gọn",
                    LastRecordedDate = new DateTime(2026, 11, 2),
                    MonthLabels = new string[] { "T1 (56k)", "T2 (55k)", "T3 (54.2k)", "T4 (53.5k)", "T5 (52.8k)", "T6 (52k)" },
                    WeightHistory = new double[] { 56.0, 55.0, 54.2, 53.5, 52.8, 52.0 },
                    BodyFatHistory = new double[] { 25.5, 24.0, 23.2, 22.5, 21.6, 20.8 },
                    BenchPressPR = "32.5 kg",
                    SquatPR = "55 kg",
                    DeadliftPR = "65 kg",
                    AvatarColor = PTUITheme.Cyan
                },
                new MemberPhysicalProfile
                {
                    MemberCode = "HV003",
                    FullName = "Lê Hoàng Nam",
                    Initials = "HN",
                    Phone = "0903.112.233",
                    PackageName = "Sức Bền & Thể Lực",
                    CurrentWeight = 68.0m,
                    Height = 172m,
                    BodyFat = 14.5m,
                    Waist = 76m,
                    BMI = 23.0m,
                    BmiStatus = "Chuẩn VĐV",
                    LastRecordedDate = new DateTime(2026, 10, 30),
                    MonthLabels = new string[] { "T1 (71k)", "T2 (70.5k)", "T3 (69.8k)", "T4 (69k)", "T5 (68.5k)", "T6 (68k)" },
                    WeightHistory = new double[] { 71.0, 70.5, 69.8, 69.0, 68.5, 68.0 },
                    BodyFatHistory = new double[] { 17.5, 16.8, 16.0, 15.5, 15.0, 14.5 },
                    BenchPressPR = "65 kg",
                    SquatPR = "95 kg",
                    DeadliftPR = "115 kg",
                    AvatarColor = Color.FromArgb(251, 146, 60)
                },
                new MemberPhysicalProfile
                {
                    MemberCode = "HV004",
                    FullName = "Đỗ Bảo Trâm",
                    Initials = "BT",
                    Phone = "0977.889.900",
                    PackageName = "Tập Nữ Cơ Bản 1–1",
                    CurrentWeight = 49.0m,
                    Height = 158m,
                    BodyFat = 23.0m,
                    Waist = 63m,
                    BMI = 19.6m,
                    BmiStatus = "Chuẩn",
                    LastRecordedDate = new DateTime(2026, 10, 28),
                    MonthLabels = new string[] { "T1 (51k)", "T2 (50.5k)", "T3 (50k)", "T4 (49.8k)", "T5 (49.4k)", "T6 (49k)" },
                    WeightHistory = new double[] { 51.0, 50.5, 50.0, 49.8, 49.4, 49.0 },
                    BodyFatHistory = new double[] { 26.0, 25.4, 24.8, 24.2, 23.5, 23.0 },
                    BenchPressPR = "25 kg",
                    SquatPR = "45 kg",
                    DeadliftPR = "50 kg",
                    AvatarColor = Color.FromArgb(168, 85, 247)
                },
                new MemberPhysicalProfile
                {
                    MemberCode = "HV005",
                    FullName = "Trần Quốc Huy",
                    Initials = "QH",
                    Phone = "0934.556.677",
                    PackageName = "Giảm Cân Cấp Tốc HIIT",
                    CurrentWeight = 78.0m,
                    Height = 176m,
                    BodyFat = 19.2m,
                    Waist = 83m,
                    BMI = 25.2m,
                    BmiStatus = "Đang Giảm Mỡ",
                    LastRecordedDate = new DateTime(2026, 10, 24),
                    MonthLabels = new string[] { "T1 (85k)", "T2 (83k)", "T3 (81.5k)", "T4 (80k)", "T5 (79k)", "T6 (78k)" },
                    WeightHistory = new double[] { 85.0, 83.0, 81.5, 80.0, 79.0, 78.0 },
                    BodyFatHistory = new double[] { 25.0, 23.5, 22.0, 21.0, 20.0, 19.2 },
                    BenchPressPR = "70 kg",
                    SquatPR = "90 kg",
                    DeadliftPR = "120 kg",
                    AvatarColor = Color.FromArgb(56, 189, 248)
                },
                new MemberPhysicalProfile
                {
                    MemberCode = "HV006",
                    FullName = "Vũ Thị Hương",
                    Initials = "TH",
                    Phone = "0965.443.322",
                    PackageName = "Dãn Cơ & Phục Hồi Chấn Thương",
                    CurrentWeight = 54.0m,
                    Height = 163m,
                    BodyFat = 21.5m,
                    Waist = 67m,
                    BMI = 20.3m,
                    BmiStatus = "Chuẩn",
                    LastRecordedDate = new DateTime(2026, 10, 20),
                    MonthLabels = new string[] { "T1 (56k)", "T2 (55.5k)", "T3 (55k)", "T4 (54.8k)", "T5 (54.4k)", "T6 (54k)" },
                    WeightHistory = new double[] { 56.0, 55.5, 55.0, 54.8, 54.4, 54.0 },
                    BodyFatHistory = new double[] { 23.5, 23.0, 22.6, 22.2, 21.8, 21.5 },
                    BenchPressPR = "30 kg",
                    SquatPR = "40 kg",
                    DeadliftPR = "55 kg",
                    AvatarColor = Color.FromArgb(244, 114, 182)
                }
            };
        }

        private void LoadProfileToUI(MemberPhysicalProfile p)
        {
            _currentProfile = p;
            txtWeight.Text = p.CurrentWeight.ToString("0.0");
            txtHeight.Text = p.Height.ToString("0");
            txtBodyFat.Text = p.BodyFat.ToString("0.0");
            txtWaist.Text = p.Waist.ToString("0");

            pnlMemberBanner.Invalidate();
            pnlChart.Invalidate();
            pnlMeasurementsCard.Invalidate();
            pnlRecordsCard.Invalidate();
        }

        public void SelectMemberByName(string memberName)
        {
            var found = _profiles.FirstOrDefault(p =>
                p.FullName.Equals(memberName, StringComparison.OrdinalIgnoreCase) ||
                memberName.Contains(p.FullName) ||
                p.FullName.Contains(memberName));

            if (found != null)
            {
                txtSearchMember.Text = found.FullName;
                LoadProfileToUI(found);
            }
        }

        private void btnSearchMember_Click(object sender, EventArgs e)
        {
            string keyword = txtSearchMember.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập Tên, Số điện thoại hoặc Mã hội viên cần tra cứu!",
                    "Nhập Từ Khóa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var found = _profiles.FirstOrDefault(p =>
                p.FullName.ToLower().Contains(keyword) ||
                p.Phone.Contains(keyword) ||
                p.MemberCode.ToLower().Contains(keyword) ||
                p.PackageName.ToLower().Contains(keyword));

            if (found != null)
            {
                LoadProfileToUI(found);
            }
            else
            {
                MessageBox.Show($"Không tìm thấy hội viên nào khớp với từ khóa \"{txtSearchMember.Text}\" trong danh sách đã dạy!",
                    "Không Tìm Thấy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtSearchMember_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnSearchMember_Click(sender, EventArgs.Empty);
            }
        }

        private void RelayoutMeasurementsControls()
        {
            int cardW = pnlMeasurementsCard.ClientSize.Width;
            int pad = 20;
            int gap = 16;
            int colW = Math.Max(120, (cardW - (pad * 2) - gap) / 2);

            int col1X = pad;
            int col2X = pad + colW + gap;

            // Row 1: Cân nặng & Chiều cao
            lblWeight.Location = new Point(col1X, 56);
            txtWeight.Location = new Point(col1X, 74);
            txtWeight.Size = new Size(colW, 25);

            lblHeight.Location = new Point(col2X, 56);
            txtHeight.Location = new Point(col2X, 74);
            txtHeight.Size = new Size(colW, 25);

            // Row 2: Tỷ lệ mỡ & Vòng eo
            lblBodyFat.Location = new Point(col1X, 116);
            txtBodyFat.Location = new Point(col1X, 134);
            txtBodyFat.Size = new Size(colW, 25);

            lblWaist.Location = new Point(col2X, 116);
            txtWaist.Location = new Point(col2X, 134);
            txtWaist.Size = new Size(colW, 25);

            // Row 3: Nút Lưu
            btnSaveMeasurement.Location = new Point(pad, 178);
            btnSaveMeasurement.Size = new Size(cardW - (pad * 2), 40);
        }

        private void PnlMemberBanner_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle r = pnlMemberBanner.ClientRectangle;
            PTUITheme.DrawCard(g, new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1), radius: 10,
                customBg: Color.FromArgb(20, 24, 32), customBorder: Color.FromArgb(35, 42, 56));

            if (_currentProfile == null) return;

            // Avatar initial
            Rectangle avRect = new Rectangle(14, 12, 40, 40);
            using (SolidBrush b = new SolidBrush(Color.FromArgb(32, _currentProfile.AvatarColor.R, _currentProfile.AvatarColor.G, _currentProfile.AvatarColor.B)))
            {
                PTUITheme.FillRoundedRectangle(g, b, avRect, 8);
            }
            using (Pen p = new Pen(Color.FromArgb(80, _currentProfile.AvatarColor.R, _currentProfile.AvatarColor.G, _currentProfile.AvatarColor.B), 1.2f))
            {
                PTUITheme.DrawRoundedRectangle(g, p, avRect, 8);
            }
            TextRenderer.DrawText(g, _currentProfile.Initials, PTUITheme.FontBold, avRect, _currentProfile.AvatarColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // Subtitle & Member name
            Rectangle subRect = new Rectangle(64, 12, 350, 16);
            TextRenderer.DrawText(g, "HỒ SƠ THỂ CHẤT & KẾT QUẢ ĐO INBODY", PTUITheme.FontSmallBold, subRect, PTUITheme.TextMuted,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            Rectangle nameRect = new Rectangle(64, 30, 450, 24);
            string titleText = $"{_currentProfile.FullName} ({_currentProfile.MemberCode})  •  Gói: {_currentProfile.PackageName}  •  SĐT: {_currentProfile.Phone}";
            TextRenderer.DrawText(g, titleText, PTUITheme.FontBold, nameRect, PTUITheme.TextWhite,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

            // Badge ngày đo gần nhất ở góc phải (thay thế cho combobox cũ)
            Rectangle dateBadge = new Rectangle(r.Width - 230, 16, 215, 30);
            PTUITheme.DrawCard(g, dateBadge, radius: 6, customBg: Color.FromArgb(28, 34, 46), customBorder: Color.FromArgb(45, 55, 75));
            Rectangle dateTextRect = new Rectangle(dateBadge.X + 8, dateBadge.Y, dateBadge.Width - 16, dateBadge.Height);
            TextRenderer.DrawText(g, $"📅 Đo gần nhất: {_currentProfile.LastRecordedDate:dd/MM/yyyy}", PTUITheme.FontSmallBold, dateTextRect, PTUITheme.PrimaryLime,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void PnlChart_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle r = pnlChart.ClientRectangle;
            PTUITheme.DrawCard(g, new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1), radius: 12);

            if (_currentProfile == null) return;

            // Chart Title
            Rectangle titleRect = new Rectangle(20, 14, 400, 22);
            TextRenderer.DrawText(g, $"BIẾN THIÊN CÂN NẶNG & BODY FAT (%) - {_currentProfile.FullName.ToUpper()}", PTUITheme.FontSection, titleRect, PTUITheme.TextWhite,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            Rectangle subTitleRect = new Rectangle(20, 36, 400, 18);
            TextRenderer.DrawText(g, "Theo dõi qua các lần đo InBody định kỳ trong quá trình kèm 1-1", PTUITheme.FontSmall, subTitleRect, PTUITheme.TextMuted,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // Legends at top-right
            int legX = Math.Max(420, r.Width - 260);
            Rectangle leg1 = new Rectangle(legX, 16, 120, 20);
            TextRenderer.DrawText(g, "● Cân nặng (kg)", PTUITheme.FontSmallBold, leg1, PTUITheme.PrimaryLime,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            Rectangle leg2 = new Rectangle(legX + 125, 16, 120, 20);
            TextRenderer.DrawText(g, "● Body Fat (%)", PTUITheme.FontSmallBold, leg2, PTUITheme.Cyan,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            string[] months = _currentProfile.MonthLabels;
            double[] weights = _currentProfile.WeightHistory;
            double[] bodyFats = _currentProfile.BodyFatHistory;

            if (months.Length == 0) return;

            int chartLeft = 50;
            int chartRight = r.Width - 50;
            int chartTop = 72;
            int chartBottom = r.Height - 38;
            int chartHeight = chartBottom - chartTop;
            int count = months.Length;
            int stepX = (chartRight - chartLeft) / Math.Max(1, count - 1);

            // Draw horizontal grid lines
            using (Pen gridPen = new Pen(Color.FromArgb(28, 33, 44), 1))
            {
                for (int i = 0; i < 4; i++)
                {
                    int gy = chartTop + i * (chartHeight / 3);
                    g.DrawLine(gridPen, chartLeft, gy, chartRight, gy);
                }
            }

            Point[] weightPoints = new Point[count];
            Point[] fatPoints = new Point[count];

            double minW = weights.Min() - 2.0;
            double maxW = weights.Max() + 2.0;
            double rangeW = Math.Max(1.0, maxW - minW);

            double minF = bodyFats.Min() - 2.0;
            double maxF = bodyFats.Max() + 2.0;
            double rangeF = Math.Max(1.0, maxF - minF);

            for (int i = 0; i < count; i++)
            {
                int px = chartLeft + i * stepX;

                double wRatio = (weights[i] - minW) / rangeW;
                int wy = chartBottom - (int)(wRatio * chartHeight);
                weightPoints[i] = new Point(px, wy);

                double fRatio = (bodyFats[i] - minF) / rangeF;
                int fy = chartBottom - (int)(fRatio * chartHeight);
                fatPoints[i] = new Point(px, fy);
            }

            // Draw Area Gradient under Weight curve
            using (GraphicsPath areaPath = new GraphicsPath())
            {
                areaPath.AddLines(weightPoints);
                areaPath.AddLine(weightPoints[count - 1].X, chartBottom, weightPoints[0].X, chartBottom);
                areaPath.CloseFigure();

                using (LinearGradientBrush lgb = new LinearGradientBrush(
                    new Point(0, chartTop), new Point(0, chartBottom),
                    Color.FromArgb(35, PTUITheme.PrimaryLime), Color.Transparent))
                {
                    g.FillPath(lgb, areaPath);
                }
            }

            // Draw Weight Line (Lime Green)
            using (Pen weightPen = new Pen(PTUITheme.PrimaryLime, 2.5f))
            {
                g.DrawLines(weightPen, weightPoints);
            }

            // Draw Body Fat Line (Cyan Dotted)
            using (Pen fatPen = new Pen(PTUITheme.Cyan, 2f))
            {
                fatPen.DashStyle = DashStyle.Dash;
                g.DrawLines(fatPen, fatPoints);
            }

            // Draw Points and Labels
            for (int i = 0; i < count; i++)
            {
                Point wp = weightPoints[i];
                using (SolidBrush b = new SolidBrush(PTUITheme.PrimaryLime))
                {
                    g.FillEllipse(b, wp.X - 5, wp.Y - 5, 10, 10);
                }
                using (SolidBrush bInner = new SolidBrush(Color.Black))
                {
                    g.FillEllipse(bInner, wp.X - 2, wp.Y - 2, 4, 4);
                }

                Rectangle wLblRect = new Rectangle(wp.X - 35, wp.Y - 22, 70, 18);
                TextRenderer.DrawText(g, months[i], PTUITheme.FontSmallBold, wLblRect, PTUITheme.TextWhite,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

                Point fp = fatPoints[i];
                using (SolidBrush b = new SolidBrush(PTUITheme.Cyan))
                {
                    g.FillEllipse(b, fp.X - 4, fp.Y - 4, 8, 8);
                }

                Rectangle fLblRect = new Rectangle(fp.X - 35, fp.Y + 6, 70, 18);
                TextRenderer.DrawText(g, $"{bodyFats[i]}%", PTUITheme.FontSmall, fLblRect, PTUITheme.Cyan,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }
        }

        private void PnlMeasurementsCard_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle r = pnlMeasurementsCard.ClientRectangle;
            PTUITheme.DrawCard(g, new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1), radius: 12);

            Rectangle titleRect = new Rectangle(20, 16, 240, 22);
            TextRenderer.DrawText(g, "Cập Nhật Chỉ Số Đo Mới", PTUITheme.FontSection, titleRect, PTUITheme.PrimaryLime,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            if (_currentProfile != null)
            {
                string bmiBadgeText = $"BMI: {_currentProfile.BMI} ({_currentProfile.BmiStatus})";
                Rectangle bmiRect = new Rectangle(r.Width - 170, 16, 150, 24);
                PTUITheme.DrawBadge(g, bmiBadgeText, bmiRect, PTUITheme.LimeDark, PTUITheme.PrimaryLime, PTUITheme.FontSmallBold, 12);
            }
        }

        private void PnlRecordsCard_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle r = pnlRecordsCard.ClientRectangle;
            PTUITheme.DrawCard(g, new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1), radius: 12);

            Rectangle titleRect = new Rectangle(20, 16, 300, 22);
            TextRenderer.DrawText(g, "Kỷ Lục Mức Tạ Cá Nhân (PR – 1RM)", PTUITheme.FontSection, titleRect, PTUITheme.Cyan,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            string bench = _currentProfile?.BenchPressPR ?? "75 kg";
            string squat = _currentProfile?.SquatPR ?? "105 kg";
            string deadlift = _currentProfile?.DeadliftPR ?? "130 kg";

            DrawPRRow(g, 20, 54, r.Width - 40, "Barbell Bench Press (Ngực)", bench);
            DrawPRRow(g, 20, 102, r.Width - 40, "Barbell Back Squat (Đùi Sau)", squat);
            DrawPRRow(g, 20, 150, r.Width - 40, "Conventional Deadlift (Lưng Xô)", deadlift);
        }

        private void DrawPRRow(Graphics g, int x, int y, int width, string exerciseName, string weight)
        {
            Rectangle rowRect = new Rectangle(x, y, width, 40);
            PTUITheme.DrawCard(g, rowRect, radius: 8, customBg: Color.FromArgb(19, 23, 31), customBorder: Color.FromArgb(32, 38, 50));

            Rectangle nameRect = new Rectangle(rowRect.X + 12, rowRect.Y, rowRect.Width - 120, rowRect.Height);
            TextRenderer.DrawText(g, exerciseName, PTUITheme.FontBold, nameRect, PTUITheme.TextWhite,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            Rectangle weightRect = new Rectangle(rowRect.Right - 100, rowRect.Y, 88, rowRect.Height);
            TextRenderer.DrawText(g, weight, PTUITheme.FontBold, weightRect, PTUITheme.PrimaryLime,
                TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
        }

        private void btnSaveMeasurement_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtWeight.Text, out decimal w) &&
                decimal.TryParse(txtHeight.Text, out decimal h) &&
                decimal.TryParse(txtBodyFat.Text, out decimal f) &&
                decimal.TryParse(txtWaist.Text, out decimal wst))
            {
                _currentProfile.CurrentWeight = w;
                _currentProfile.Height = h;
                _currentProfile.BodyFat = f;
                _currentProfile.Waist = wst;
                _currentProfile.LastRecordedDate = DateTime.Today;

                if (h > 0)
                {
                    decimal hM = h / 100m;
                    _currentProfile.BMI = Math.Round(w / (hM * hM), 1);
                }

                MessageBox.Show($"Đã lưu kết quả đo InBody mới thành công cho hội viên {_currentProfile.FullName}!\n\nCân nặng: {w} kg\nBody Fat: {f}%\nBMI: {_currentProfile.BMI}",
                    "Thông Báo Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                pnlMemberBanner.Invalidate();
                pnlChart.Invalidate();
                pnlMeasurementsCard.Invalidate();
            }
            else
            {
                MessageBox.Show("Vui lòng kiểm tra lại định dạng các chỉ số đo (phải là số hợp lệ)!",
                    "Dữ Liệu Không Hợp Lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUpdatePR_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Cập nhật mức tạ PR cho hội viên {_currentProfile.FullName}.\nTính năng đang lưu trữ các mốc kỷ lục 1RM của hội viên.",
                "Cập Nhật Kỷ Lục PR", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
