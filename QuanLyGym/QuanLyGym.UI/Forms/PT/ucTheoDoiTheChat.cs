using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.PT
{
    public partial class ucTheoDoiTheChat : UserControl
    {
        public ucTheoDoiTheChat()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = PTUITheme.BgDark;

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
            pnlMemberBanner.Resize += (s, e) =>
            {
                int padRight = 16;
                cboSelectMember.Size = new Size(330, 38);
                cboSelectMember.Location = new Point(pnlMemberBanner.ClientSize.Width - padRight - cboSelectMember.Width, (pnlMemberBanner.ClientSize.Height - cboSelectMember.Height) / 2);
                lblSelectMember.Location = new Point(cboSelectMember.Left - lblSelectMember.PreferredWidth - 12, (pnlMemberBanner.ClientSize.Height - lblSelectMember.PreferredHeight) / 2);
                pnlMemberBanner.Invalidate();
            };

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

            cboSelectMember.SelectedIndex = 0;
            cboSelectMember.SelectedIndexChanged += (s, e) => pnlMemberBanner.Invalidate();
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

        public void SelectMemberByName(string memberName)
        {
            for (int i = 0; i < cboSelectMember.Items.Count; i++)
            {
                string? itemText = cboSelectMember.Items[i]?.ToString();
                if (itemText != null && itemText.Contains(memberName))
                {
                    cboSelectMember.SelectedIndex = i;
                    break;
                }
            }
        }

        private void PnlMemberBanner_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle r = pnlMemberBanner.ClientRectangle;
            PTUITheme.DrawCard(g, new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1), radius: 10,
                customBg: Color.FromArgb(20, 24, 32), customBorder: Color.FromArgb(35, 42, 56));

            // Avatar initial dynamic
            Rectangle avRect = new Rectangle(14, 11, 38, 38);
            using (SolidBrush b = new SolidBrush(Color.FromArgb(32, 42, 56)))
            {
                PTUITheme.FillRoundedRectangle(g, b, avRect, 8);
            }

            string selectedMemberText = cboSelectMember.SelectedItem ?? "Nguyễn Đức An (Tăng Cơ Hypertrophy)";
            string initials = "DA";
            if (selectedMemberText.Contains("Mai Linh")) initials = "ML";
            else if (selectedMemberText.Contains("Hoàng Nam")) initials = "HN";
            else if (selectedMemberText.Contains("Bảo Trâm")) initials = "BT";

            TextRenderer.DrawText(g, initials, PTUITheme.FontBold, avRect, PTUITheme.PrimaryLime,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // Subtitle & Member name
            Rectangle subRect = new Rectangle(62, 12, 280, 16);
            TextRenderer.DrawText(g, "HỒ SƠ THỂ CHẤT HỘI VIÊN", PTUITheme.FontSmallBold, subRect, PTUITheme.TextMuted,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            Rectangle nameRect = new Rectangle(62, 28, 420, 22);
            TextRenderer.DrawText(g, selectedMemberText, PTUITheme.FontBold, nameRect, PTUITheme.TextWhite,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
        }

        private void PnlChart_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle r = pnlChart.ClientRectangle;
            PTUITheme.DrawCard(g, new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1), radius: 12);

            // Chart Title
            Rectangle titleRect = new Rectangle(20, 14, 380, 22);
            TextRenderer.DrawText(g, "BIẾN THIÊN CÂN NẶNG & BODY FAT (%)", PTUITheme.FontSection, titleRect, PTUITheme.TextWhite,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            Rectangle subTitleRect = new Rectangle(20, 36, 380, 18);
            TextRenderer.DrawText(g, "Theo dõi qua các lần đo InBody định kỳ", PTUITheme.FontSmall, subTitleRect, PTUITheme.TextMuted,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // Legends at top-right
            int legX = Math.Max(420, r.Width - 260);
            Rectangle leg1 = new Rectangle(legX, 16, 120, 20);
            TextRenderer.DrawText(g, "● Cân nặng (kg)", PTUITheme.FontSmallBold, leg1, PTUITheme.PrimaryLime,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            Rectangle leg2 = new Rectangle(legX + 125, 16, 120, 20);
            TextRenderer.DrawText(g, "● Body Fat (%)", PTUITheme.FontSmallBold, leg2, PTUITheme.Cyan,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            // Chart Data
            string[] months = new string[] { "T1 (78k)", "T2 (77k)", "T3 (76k)", "T4 (74.8k)", "T5 (73.5k)", "T6 (72k)" };
            double[] weights = new double[] { 78.0, 77.0, 76.0, 74.8, 73.5, 72.0 };
            double[] bodyFats = new double[] { 22.0, 20.5, 19.0, 18.2, 17.0, 16.5 };

            int chartLeft = 50;
            int chartRight = r.Width - 50;
            int chartTop = 72;
            int chartBottom = r.Height - 38;
            int chartHeight = chartBottom - chartTop;
            int count = months.Length;
            int stepX = (chartRight - chartLeft) / (count - 1);

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

            for (int i = 0; i < count; i++)
            {
                int px = chartLeft + i * stepX;

                double wRatio = (weights[i] - 70.0) / 10.0;
                int wy = chartBottom - (int)(wRatio * chartHeight);
                weightPoints[i] = new Point(px, wy);

                double fRatio = (bodyFats[i] - 15.0) / 10.0;
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

            Rectangle bmiRect = new Rectangle(r.Width - 145, 16, 125, 24);
            PTUITheme.DrawBadge(g, "BMI: 23.5 (Chuẩn)", bmiRect, PTUITheme.LimeDark, PTUITheme.PrimaryLime, PTUITheme.FontSmallBold, 12);
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

            DrawPRRow(g, 20, 54, r.Width - 40, "Barbell Bench Press (Ngực)", "75 kg");
            DrawPRRow(g, 20, 102, r.Width - 40, "Barbell Back Squat (Đùi Sau)", "105 kg");
            DrawPRRow(g, 20, 150, r.Width - 40, "Conventional Deadlift (Lưng Xô)", "130 kg");
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
            MessageBox.Show("Đã lưu kết quả đo InBody mới hôm nay thành công!",
                "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUpdatePR_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mở form cập nhật mức tạ PR 1RM cho hội viên.",
                "Cập Nhật Kỷ Lục PR", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cboSelectMember_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlMemberBanner.Invalidate();
            pnlChart.Invalidate();
        }
    }
}
