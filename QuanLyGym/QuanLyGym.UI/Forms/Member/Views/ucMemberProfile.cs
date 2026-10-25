using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyGym.UI.Utils;

namespace QuanLyGym.UI.Forms.Member.Views
{
    public class ucMemberProfile : UserControl
    {
        private TextBox txtFullName = null!;
        private ComboBox cboGender = null!;
        private DateTimePicker dtpDOB = null!;
        private TextBox txtPhone = null!;
        private TextBox txtEmail = null!;
        private TextBox txtAddress = null!;
        private TextBox txtOldPass = null!;
        private TextBox txtNewPass = null!;
        private TextBox txtConfirmPass = null!;

        public ucMemberProfile()
        {
            InitializeComponent();
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
                Height = 690,
                Padding = new Padding(24, 20, 24, 30),
                BackColor = UITheme.Background
            };

            // Title
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 50 };
            Label lblTitle = new Label
            {
                Text = "👤 THÔNG TIN HỒ SƠ & BẢO MẬT TÀI KHOẢN",
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
                Height = 580,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 10, 0, 10)
            };
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            pnlCols.RowStyles.Clear();
            pnlCols.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            RoundedPanel cardInfo = CreateProfileInfoCard();
            cardInfo.Dock = DockStyle.Fill;
            cardInfo.Margin = new Padding(0, 0, 10, 0);

            RoundedPanel cardSecurity = CreateSecurityCard();
            cardSecurity.Dock = DockStyle.Fill;
            cardSecurity.Margin = new Padding(10, 0, 0, 0);

            pnlCols.Controls.Add(cardInfo, 0, 0);
            pnlCols.Controls.Add(cardSecurity, 1, 0);

            pnlMain.Controls.Add(pnlCols);
            pnlMain.Controls.Add(pnlHeader);

            this.Controls.Add(pnlMain);
            this.ResumeLayout(true);
        }

        private RoundedPanel CreateProfileInfoCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 620,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(24)
            };

            Label lblTitle = new Label
            {
                Text = "THÔNG TIN CÁ NHÂN HỘI VIÊN",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = 30
            };

            // Avatar Header Row
            Panel pnlAvatarRow = new Panel { Dock = DockStyle.Top, Height = 80, Margin = new Padding(0, 6, 0, 16) };
            Panel avatar = new Panel { Size = new Size(64, 64), BackColor = UITheme.PrimaryLight, Location = new Point(0, 8) };
            Label lblAvatarIcon = new Label { Text = "🏋️‍♂️", Font = new Font("Segoe UI Emoji", 26f), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            avatar.Controls.Add(lblAvatarIcon);

            Label lblName = new Label { Text = "Trần Văn Hùng", Font = UITheme.FontSubtitle, ForeColor = UITheme.TextDark, Location = new Point(78, 12), AutoSize = true };
            Label lblCode = new Label { Text = "Mã hội viên: #MB-001  |  Hạng thẻ: VIP Diamond", Font = UITheme.FontSmallBold, ForeColor = UITheme.Primary, Location = new Point(78, 38), AutoSize = true };
            Button btnChangeAvt = new Button
            {
                Text = "📷 Đổi ảnh",
                Font = UITheme.FontSmallBold,
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = UITheme.TextDark,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(90, 30),
                Cursor = Cursors.Hand,
                Location = new Point(340, 24)
            };
            btnChangeAvt.FlatAppearance.BorderSize = 0;
            btnChangeAvt.Click += (s, e) => MessageBox.Show("Đã cập nhật ảnh đại diện thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            pnlAvatarRow.Controls.Add(avatar);
            pnlAvatarRow.Controls.Add(lblName);
            pnlAvatarRow.Controls.Add(lblCode);
            pnlAvatarRow.Controls.Add(btnChangeAvt);

            // Form inputs
            Panel pnlInputs = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            int y = 10;

            // Full Name
            pnlInputs.Controls.Add(new Label { Text = "Họ và tên:", Font = UITheme.FontSmallBold, Location = new Point(4, y), AutoSize = true });
            y += 22;
            txtFullName = new TextBox { Text = "Trần Văn Hùng", Location = new Point(4, y), Width = 500, Font = UITheme.FontRegular };
            pnlInputs.Controls.Add(txtFullName);
            y += 38;

            // Gender & DOB
            pnlInputs.Controls.Add(new Label { Text = "Giới tính:", Font = UITheme.FontSmallBold, Location = new Point(4, y), AutoSize = true });
            pnlInputs.Controls.Add(new Label { Text = "Ngày sinh:", Font = UITheme.FontSmallBold, Location = new Point(190, y), AutoSize = true });
            y += 22;

            cboGender = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(4, y), Width = 250, Font = UITheme.FontRegular };
            cboGender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            cboGender.SelectedIndex = 0;

            dtpDOB = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(270, y), Width = 300, Font = UITheme.FontRegular, Value = new DateTime(2000, 10, 10) };
            pnlInputs.Controls.Add(cboGender);
            pnlInputs.Controls.Add(dtpDOB);
            y += 38;

            // Phone
            pnlInputs.Controls.Add(new Label { Text = "Số điện thoại:", Font = UITheme.FontSmallBold, Location = new Point(4, y), AutoSize = true });
            y += 22;
            txtPhone = new TextBox { Text = "0977777777", Location = new Point(4, y), Width = 500, Font = UITheme.FontRegular };
            pnlInputs.Controls.Add(txtPhone);
            y += 38;

            // Email
            pnlInputs.Controls.Add(new Label { Text = "Địa chỉ Email:", Font = UITheme.FontSmallBold, Location = new Point(4, y), AutoSize = true });
            y += 22;
            txtEmail = new TextBox { Text = "hung@gmail.com", Location = new Point(4, y), Width = 500, Font = UITheme.FontRegular };
            pnlInputs.Controls.Add(txtEmail);
            y += 38;

            // Address
            pnlInputs.Controls.Add(new Label { Text = "Địa chỉ liên hệ:", Font = UITheme.FontSmallBold, Location = new Point(4, y), AutoSize = true });
            y += 22;
            txtAddress = new TextBox { Text = "Cầu Giấy, Hà Nội", Location = new Point(4, y), Width = 500, Font = UITheme.FontRegular };
            pnlInputs.Controls.Add(txtAddress);
            y += 46;

            // Save Button
            Button btnSave = new Button
            {
                Text = "💾 LƯU THAY ĐỔI THÔNG TIN",
                Font = UITheme.FontBold,
                Height = 44,
                Width = 520,
                Location = new Point(4, y),
                BackColor = UITheme.Primary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) => MessageBox.Show("Đã lưu thông tin hồ sơ cá nhân thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            pnlInputs.Controls.Add(btnSave);

            card.Controls.Add(pnlInputs);
            card.Controls.Add(pnlAvatarRow);
            card.Controls.Add(lblTitle);

            return card;
        }

        private RoundedPanel CreateSecurityCard()
        {
            RoundedPanel card = new RoundedPanel
            {
                Height = 620,
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding(24)
            };

            Label lblTitle = new Label
            {
                Text = "🔒 BẢO MẬT & ĐỔI MẬT KHẨU",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = 32
            };

            Panel pnlForm = new Panel { Dock = DockStyle.Top, Height = 330 };
            int y = 10;

            // Username (read-only)
            pnlForm.Controls.Add(new Label { Text = "Tên đăng nhập (Username):", Font = UITheme.FontSmallBold, Location = new Point(4, y), AutoSize = true });
            y += 22;
            TextBox txtUser = new TextBox { Text = "member_hung", ReadOnly = true, BackColor = Color.FromArgb(248, 250, 252), Location = new Point(4, y), Width = 260, Font = UITheme.FontRegular };
            pnlForm.Controls.Add(txtUser);
            y += 38;

            // Current Password
            pnlForm.Controls.Add(new Label { Text = "Mật khẩu hiện tại:", Font = UITheme.FontSmallBold, Location = new Point(4, y), AutoSize = true });
            y += 22;
            txtOldPass = new TextBox { UseSystemPasswordChar = true, Location = new Point(4, y), Width = 360, Font = UITheme.FontRegular };
            pnlForm.Controls.Add(txtOldPass);
            y += 38;

            // New Password
            pnlForm.Controls.Add(new Label { Text = "Mật khẩu mới:", Font = UITheme.FontSmallBold, Location = new Point(4, y), AutoSize = true });
            y += 22;
            txtNewPass = new TextBox { UseSystemPasswordChar = true, Location = new Point(4, y), Width = 360, Font = UITheme.FontRegular };
            pnlForm.Controls.Add(txtNewPass);
            y += 38;

            // Confirm Password
            pnlForm.Controls.Add(new Label { Text = "Xác nhận mật khẩu mới:", Font = UITheme.FontSmallBold, Location = new Point(4, y), AutoSize = true });
            y += 22;
            txtConfirmPass = new TextBox { UseSystemPasswordChar = true, Location = new Point(4, y), Width = 360, Font = UITheme.FontRegular };
            pnlForm.Controls.Add(txtConfirmPass);
            y += 46;

            // Change Pass Button
            Button btnChangePass = new Button
            {
                Text = "🔑 Cập nhật mật khẩu mới",
                Font = UITheme.FontBold,
                Height = 42,
                Width = 380,
                Location = new Point(4, y),
                BackColor = UITheme.PrimaryLight,
                ForeColor = UITheme.Primary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnChangePass.FlatAppearance.BorderSize = 0;
            btnChangePass.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtOldPass.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu hiện tại.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtNewPass.Text) || txtNewPass.Text.Length < 6)
                {
                    MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtNewPass.Text != txtConfirmPass.Text)
                {
                    MessageBox.Show("Mật khẩu xác nhận không trùng khớp.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                MessageBox.Show("Đổi mật khẩu tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOldPass.Clear();
                txtNewPass.Clear();
                txtConfirmPass.Clear();
            };
            pnlForm.Controls.Add(btnChangePass);

            // Support info card at bottom
            RoundedPanel pnlSupport = new RoundedPanel
            {
                Dock = DockStyle.Bottom,
                Height = 150,
                CornerRadius = 10,
                BackColor = Color.FromArgb(248, 250, 252),
                BorderColor = UITheme.Border,
                Padding = new Padding(14)
            };
            Label lblSupTitle = new Label { Text = "📞 HỖ TRỢ HỘI VIÊN 24/7", Font = UITheme.FontSmallBold, ForeColor = UITheme.Primary, Dock = DockStyle.Top, Height = 22 };
            Label lblSupBody = new Label
            {
                Text = "• Hotline Gym: 1900 6868\n• Email CSKH: support@titangym.vn\n• Địa chỉ: Tầng 3, Tòa nhà Titan, Cầu Giấy, Hà Nội\n• Giờ mở cửa: 05:30 - 22:00 hàng ngày",
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextMuted,
                Dock = DockStyle.Fill
            };
            pnlSupport.Controls.Add(lblSupBody);
            pnlSupport.Controls.Add(lblSupTitle);

            card.Controls.Add(pnlSupport);
            card.Controls.Add(pnlForm);
            card.Controls.Add(lblTitle);

            return card;
        }
    }
}
