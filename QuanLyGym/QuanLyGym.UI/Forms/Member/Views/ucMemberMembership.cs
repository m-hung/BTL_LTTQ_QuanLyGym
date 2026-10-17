using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using QuanLyGym.UI.Utils;

namespace QuanLyGym.UI.Forms.Member.Views
{
    public class ucMemberMembership : UserControl
    {
        public ucMemberMembership()
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

            // Title Header
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 50 };
            Label lblTitle = new Label
            {
                Text = "💳 QUẢN LÝ GÓI TẬP & THẺ HỘI VIÊN",
                Font = UITheme.FontTitle,
                ForeColor = UITheme.TextDark,
                AutoSize = true,
                Location = new Point(0, 8)
            };
            pnlHeader.Controls.Add(lblTitle);

            // 1. Two Column Layout for Top: Digital Membership Card (Left 44%) and Active Package Details (Right 56%)
            TableLayoutPanel pnlTopCols = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Height = 265,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 4, 0, 16)
            };
            pnlTopCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44f));
            pnlTopCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56f));

            RoundedPanel cardDigital = CreateDigitalCard();
            cardDigital.Dock = DockStyle.Fill;
            cardDigital.Margin = new Padding(0, 0, 10, 0);

            RoundedPanel cardStatus = CreatePackageDetailsCard();
            cardStatus.Dock = DockStyle.Fill;
            cardStatus.Margin = new Padding(10, 0, 0, 0);

            pnlTopCols.Controls.Add(cardDigital, 0, 0);
            pnlTopCols.Controls.Add(cardStatus, 1, 0);

            // 2. Full-Width Transaction & Membership History Card (solves truncated columns)
            RoundedPanel cardHistory = CreateHistoryCard();
            cardHistory.Dock = DockStyle.Top;
            cardHistory.Margin = new Padding(0, 0, 0, 16);

            // 3. Full-Width Privileges Card
            RoundedPanel cardPrivileges = CreatePrivilegesCard();
            cardPrivileges.Dock = DockStyle.Top;

            pnlMain.Controls.Add(cardPrivileges);
            pnlMain.Controls.Add(cardHistory);
            pnlMain.Controls.Add(pnlTopCols);
            pnlMain.Controls.Add(pnlHeader);

            this.Controls.Add(pnlMain);
            this.ResumeLayout(false);
        }

        private RoundedPanel CreateDigitalCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 220,
                CornerRadius = 16,
                BackColor = Color.FromArgb(20, 24, 33),          // Metallic Dark Gold / Slate
                GradientEndColor = Color.FromArgb(45, 55, 72),
                GradientAngle = 135f,
                BorderColor = Color.FromArgb(245, 158, 11),       // Gold Amber Border
                BorderWidth = 1,
                Padding = new Padding(22)
            };

            Label lblLogo = new Label
            {
                Text = "⚡ TITAN FITNESS CLUB",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(251, 191, 36), // Gold
                AutoSize = true,
                Location = new Point(22, 18)
            };

            Label lblBadge = new Label
            {
                Text = "VIP DIAMOND",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(79, 70, 229),
                Padding = new Padding(8, 3, 8, 3),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(card.Width - 130, 18)
            };

            // Chip icon simulation
            Panel chip = new Panel
            {
                Size = new Size(36, 28),
                BackColor = Color.FromArgb(245, 158, 11),
                Location = new Point(24, 58)
            };

            Label lblCardNumber = new Label
            {
                Text = "MB-2026 •••• •••• 8899",
                Font = new Font("Consolas", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(241, 245, 249),
                AutoSize = true,
                Location = new Point(22, 110)
            };

            Label lblMemberName = new Label
            {
                Text = "TRẦN VĂN HÙNG",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(22, 155)
            };

            Label lblExpiry = new Label
            {
                Text = "HẠN DÙNG: 31/12/2026",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(203, 213, 225),
                AutoSize = true,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new Point(card.Width - 180, 160)
            };

            card.Controls.Add(lblLogo);
            card.Controls.Add(lblBadge);
            card.Controls.Add(chip);
            card.Controls.Add(lblCardNumber);
            card.Controls.Add(lblMemberName);
            card.Controls.Add(lblExpiry);

            card.Resize += (s, e) =>
            {
                lblBadge.Location = new Point(card.Width - 130, 18);
                lblExpiry.Location = new Point(card.Width - 180, 160);
            };

            return card;
        }

        private RoundedPanel CreatePrivilegesCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 165,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(20, 16, 20, 16)
            };

            Label lblTitle = new Label
            {
                Text = "⭐ ĐẶC QUYỀN HỘI VIÊN GÓI DIAMOND",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = 28
            };

            string[] benefits = new string[]
            {
                "Tập luyện không giới hạn khung giờ toàn hệ thống.",
                "Đo & phân tích chỉ số InBody 2 lần/tháng miễn phí.",
                "Miễn phí xông hơi đá muối & bể bơi 4 mùa cao cấp.",
                "Kèm 12 buổi huấn luyện 1-1 cùng HLV (PT chuyên nghiệp).",
                "Trang bị Smart Locker cá nhân & khăn tập riêng.",
                "Ưu đãi giảm 15% khi mua Whey & thực phẩm bổ sung."
            };

            TableLayoutPanel pnlList = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Padding = new Padding(0, 4, 0, 0)
            };
            pnlList.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            pnlList.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            pnlList.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            pnlList.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            pnlList.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));

            for (int i = 0; i < benefits.Length; i++)
            {
                int col = i % 2;
                int row = i / 2;
                Label item = new Label
                {
                    Text = "⭐  " + benefits[i],
                    Font = UITheme.FontRegular,
                    ForeColor = UITheme.TextRegular,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                pnlList.Controls.Add(item, col, row);
            }

            card.Controls.Add(pnlList);
            card.Controls.Add(lblTitle);

            return card;
        }

        private RoundedPanel CreatePackageDetailsCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 265,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(20, 16, 20, 14)
            };

            Label lblTitle = new Label
            {
                Text = "📋 THÔNG TIN GÓI TẬP ĐANG HOẠT ĐỘNG",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = 28
            };

            TableLayoutPanel grid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 135,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(0, 4, 0, 0)
            };
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            grid.Controls.Add(CreateInfoRow("Gói tập:", "VIP Diamond 12 Tháng"), 0, 0);
            grid.Controls.Add(CreateInfoRow("Trạng thái:", "● Đang hoạt động (Active)"), 1, 0);
            grid.Controls.Add(CreateInfoRow("Ngày đăng ký:", "01/01/2026"), 0, 1);
            grid.Controls.Add(CreateInfoRow("Ngày kết thúc:", "31/12/2026"), 1, 1);
            grid.Controls.Add(CreateInfoRow("Giá gói:", "12,000,000 VNĐ"), 0, 2);
            grid.Controls.Add(CreateInfoRow("Chi nhánh:", "Chi nhánh Cầu Giấy"), 1, 2);
            grid.Controls.Add(CreateInfoRow("Số buổi đã tập:", "12 / 60 buổi"), 0, 3);
            grid.Controls.Add(CreateInfoRow("Thời gian còn lại:", "74 ngày"), 1, 3);

            // Progress bar
            ModernProgressBar prog = new ModernProgressBar
            {
                Dock = DockStyle.Top,
                Height = 8,
                Value = 20,
                Maximum = 100,
                ProgressColor = UITheme.Primary,
                ProgressColor2 = UITheme.Secondary,
                Margin = new Padding(0, 6, 0, 10)
            };

            // Buttons
            FlowLayoutPanel pnlButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                FlowDirection = FlowDirection.LeftToRight
            };

            Button btnExtend = new Button
            {
                Text = "🔄 Gia hạn gói tập",
                Font = UITheme.FontBold,
                Size = new Size(160, 36),
                BackColor = UITheme.Primary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExtend.FlatAppearance.BorderSize = 0;
            btnExtend.Click += (s, e) => MessageBox.Show("Yêu cầu gia hạn gói VIP Diamond đã được gửi tới bộ phận Lễ tân. Chúng tôi sẽ liên hệ hỗ trợ bạn ngay!", "Gia hạn gói tập", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Button btnUpgrade = new Button
            {
                Text = "⚡ Nâng cấp Black Card",
                Font = UITheme.FontBold,
                Size = new Size(190, 36),
                BackColor = UITheme.PrimaryLight,
                ForeColor = UITheme.Primary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnUpgrade.FlatAppearance.BorderSize = 0;
            btnUpgrade.Click += (s, e) => MessageBox.Show("Đặc quyền Black Card: Kèm 30 buổi PT 1-1, Locker cá nhân vĩnh viễn và đồ uống cao cấp không giới hạn.", "Nâng cấp gói", MessageBoxButtons.OK, MessageBoxIcon.Information);

            pnlButtons.Controls.Add(btnExtend);
            pnlButtons.Controls.Add(btnUpgrade);

            card.Controls.Add(pnlButtons);
            card.Controls.Add(prog);
            card.Controls.Add(grid);
            card.Controls.Add(lblTitle);

            return card;
        }

        private Panel CreateInfoRow(string label, string val)
        {
            Panel pnl = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0, 1, 0, 1) };
            Label l = new Label { Text = label, Font = UITheme.FontSmall, ForeColor = UITheme.TextMuted, Location = new Point(0, 0), AutoSize = true };
            Label v = new Label { Text = val, Font = UITheme.FontBold, ForeColor = UITheme.TextDark, Location = new Point(0, 16), AutoSize = true };
            if (val.Contains("Đang hoạt động")) v.ForeColor = UITheme.Success;
            pnl.Controls.Add(l);
            pnl.Controls.Add(v);
            return pnl;
        }

        private RoundedPanel CreateHistoryCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 250,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(20)
            };

            Label lblTitle = new Label
            {
                Text = "📜 LỊCH SỬ ĐĂNG KÝ & GIAO DỊCH GÓI TẬP",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = 32
            };

            DataGridView dgv = new DataGridView { Dock = DockStyle.Fill };
            UITheme.StyleDataGridView(dgv);

            var colId = new DataGridViewTextBoxColumn { Name = "ID", HeaderText = "Mã HĐ", Width = 140, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colPkg = new DataGridViewTextBoxColumn { Name = "Package", HeaderText = "Gói tập", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, MinimumWidth = 230 };
            var colDate = new DataGridViewTextBoxColumn { Name = "Date", HeaderText = "Ngày kích hoạt", Width = 145, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colEnd = new DataGridViewTextBoxColumn { Name = "End", HeaderText = "Ngày kết thúc", Width = 145, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colPrice = new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "Giá tiền", Width = 150, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };
            var colStatus = new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Tình trạng", Width = 160, AutoSizeMode = DataGridViewAutoSizeColumnMode.None };

            dgv.Columns.AddRange(colId, colPkg, colDate, colEnd, colPrice, colStatus);

            dgv.Rows.Add("#HD-2026-01", "VIP Diamond 12 Tháng", "01/01/2026", "31/12/2026", "12,000,000 đ", "Đã thanh toán");
            dgv.Rows.Add("#HD-2025-02", "Gói Tập Gym 6 Tháng", "01/06/2025", "31/12/2025", "4,500,000 đ", "Đã hoàn thành");
            dgv.Rows.Add("#HD-2025-01", "Gói Trải Nghiệm 3 Tháng", "01/03/2025", "31/05/2025", "2,200,000 đ", "Đã hoàn thành");

            dgv.CellFormatting += (s, e) =>
            {
                if (dgv.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
                {
                    string status = e.Value.ToString() ?? "";
                    if (status.Contains("thanh toán") || status.Contains("hoàn thành"))
                    {
                        e.CellStyle.ForeColor = UITheme.Success;
                        e.CellStyle.Font = UITheme.FontBold;
                    }
                }
            };

            card.Controls.Add(dgv);
            card.Controls.Add(lblTitle);

            return card;
        }
    }
}
