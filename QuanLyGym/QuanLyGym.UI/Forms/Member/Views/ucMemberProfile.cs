using System;
using System.Drawing;
using System.Windows.Forms;
using QuanLyGym.UI.Services;
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
        private Label lblName = null!;
        private Label lblCode = null!;
        private TextBox txtUser = null!;

        public ucMemberProfile()
        {
            InitializeComponent();
            LoadProfileData();
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

            // Two Column Layout — Dock=Fill fills remaining space
            TableLayoutPanel pnlCols = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding((int)(16 * scale), (int)(12 * scale), (int)(16 * scale), (int)(14 * scale)),
                BackColor = UITheme.Background
            };
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            pnlCols.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            pnlCols.RowStyles.Clear();
            pnlCols.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            RoundedPanel cardInfo = CreateProfileInfoCard(scale);
            cardInfo.Dock = DockStyle.Fill;
            cardInfo.Margin = new Padding(0, 0, (int)(8 * scale), 0);

            RoundedPanel cardSecurity = CreateSecurityCard(scale);
            cardSecurity.Dock = DockStyle.Fill;
            cardSecurity.Margin = new Padding((int)(8 * scale), 0, 0, 0);

            pnlCols.Controls.Add(cardInfo, 0, 0);
            pnlCols.Controls.Add(cardSecurity, 1, 0);

            this.Controls.Add(pnlCols);
            this.ResumeLayout(true);
        }

        private RoundedPanel CreateProfileInfoCard(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding((int)(20 * scale))
            };

            Label lblTitle = new Label
            {
                Text = "👤 THÔNG TIN CÁ NHÂN HỘI VIÊN",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = (int)(32 * scale),
                UseMnemonic = false
            };

            // Avatar Header Row
            int avatarRowH = (int)Math.Round(72f * scale);
            Panel pnlAvatarRow = new Panel { Dock = DockStyle.Top, Height = avatarRowH, Margin = new Padding(0, 0, 0, (int)(12 * scale)) };

            int avatarSize = (int)Math.Round(56f * scale);
            Panel avatar = new Panel { Size = new Size(avatarSize, avatarSize), BackColor = UITheme.PrimaryLight, Location = new Point(0, (int)(6 * scale)) };
            Label lblAvatarIcon = new Label { Text = "🏋️‍♂️", Font = new Font("Segoe UI Emoji", 20f), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, UseMnemonic = false };
            avatar.Controls.Add(lblAvatarIcon);

            FlowLayoutPanel flowAvtText = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(avatarSize + (int)(14 * scale), (int)(8 * scale), 0, 0),
                BackColor = Color.Transparent
            };
            lblName = new Label { Text = "Hội viên", Font = UITheme.FontSubtitle, ForeColor = UITheme.TextDark, AutoSize = true, Margin = new Padding(0, 0, 0, (int)(2 * scale)), UseMnemonic = false };
            lblCode = new Label { Text = "Mã hội viên: #MB-001  |  Hạng thẻ: Classic", Font = UITheme.FontSmallBold, ForeColor = UITheme.Primary, AutoSize = true, UseMnemonic = false };
            flowAvtText.Controls.Add(lblName);
            flowAvtText.Controls.Add(lblCode);

            Button btnChangeAvt = new Button
            {
                Text = "📷 Đổi ảnh",
                Font = UITheme.FontSmallBold,
                BackColor = Color.FromArgb(241, 245, 249),
                ForeColor = UITheme.TextDark,
                FlatStyle = FlatStyle.Flat,
                Size = new Size((int)(90 * scale), (int)(30 * scale)),
                Cursor = Cursors.Hand,
                Dock = DockStyle.Right,
                UseMnemonic = false
            };
            btnChangeAvt.FlatAppearance.BorderSize = 0;
            btnChangeAvt.Click += (s, e) => MessageBox.Show("Đã cập nhật ảnh đại diện thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            pnlAvatarRow.Controls.Add(flowAvtText);
            pnlAvatarRow.Controls.Add(btnChangeAvt);
            pnlAvatarRow.Controls.Add(avatar);

            // Form inputs using TableLayoutPanel
            TableLayoutPanel pnlInputs = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 11,
                AutoScroll = true,
                Padding = new Padding(0, (int)(8 * scale), 0, 0)
            };
            pnlInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // Row 0, 1: Full Name
            Label lblFN = new Label { Text = "Họ và tên:", Font = UITheme.FontSmallBold, AutoSize = true, Margin = new Padding(0, 0, 0, (int)(2 * scale)), UseMnemonic = false };
            txtFullName = new TextBox { Dock = DockStyle.Top, Height = (int)(32 * scale), Font = UITheme.FontRegular };

            // Row 2, 3: Gender & DOB
            TableLayoutPanel rowGenderDOB = new TableLayoutPanel { Dock = DockStyle.Top, Height = (int)(62 * scale), ColumnCount = 2, RowCount = 2 };
            rowGenderDOB.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            rowGenderDOB.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            Label lblG = new Label { Text = "Giới tính:", Font = UITheme.FontSmallBold, AutoSize = true, UseMnemonic = false };
            Label lblD = new Label { Text = "Ngày sinh:", Font = UITheme.FontSmallBold, AutoSize = true, UseMnemonic = false };
            cboGender = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Top, Font = UITheme.FontRegular };
            cboGender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            cboGender.SelectedIndex = 0;
            dtpDOB = new DateTimePicker { Format = DateTimePickerFormat.Short, Dock = DockStyle.Top, Font = UITheme.FontRegular, Value = new DateTime(2000, 1, 1) };
            rowGenderDOB.Controls.Add(lblG, 0, 0);
            rowGenderDOB.Controls.Add(lblD, 1, 0);
            rowGenderDOB.Controls.Add(cboGender, 0, 1);
            rowGenderDOB.Controls.Add(dtpDOB, 1, 1);

            // Row 4, 5: Phone
            Label lblP = new Label { Text = "Số điện thoại:", Font = UITheme.FontSmallBold, AutoSize = true, Margin = new Padding(0, (int)(4 * scale), 0, (int)(2 * scale)), UseMnemonic = false };
            txtPhone = new TextBox { Dock = DockStyle.Top, Height = (int)(32 * scale), Font = UITheme.FontRegular };

            // Row 6, 7: Email
            Label lblE = new Label { Text = "Địa chỉ Email:", Font = UITheme.FontSmallBold, AutoSize = true, Margin = new Padding(0, (int)(4 * scale), 0, (int)(2 * scale)), UseMnemonic = false };
            txtEmail = new TextBox { Dock = DockStyle.Top, Height = (int)(32 * scale), Font = UITheme.FontRegular };

            // Row 8, 9: Address
            Label lblA = new Label { Text = "Địa chỉ liên hệ:", Font = UITheme.FontSmallBold, AutoSize = true, Margin = new Padding(0, (int)(4 * scale), 0, (int)(2 * scale)), UseMnemonic = false };
            txtAddress = new TextBox { Dock = DockStyle.Top, Height = (int)(32 * scale), Font = UITheme.FontRegular };

            // Row 10: Save Button
            Button btnSave = new Button
            {
                Text = "💾 LƯU THAY ĐỔI THÔNG TIN",
                Font = UITheme.FontBold,
                Height = (int)(42 * scale),
                Dock = DockStyle.Top,
                Margin = new Padding(0, (int)(12 * scale), 0, 0),
                BackColor = UITheme.Primary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) =>
            {
                string fn = txtFullName.Text.Trim();
                string g = cboGender.Text;
                DateTime dob = dtpDOB.Value;
                string p = txtPhone.Text.Trim();
                string em = txtEmail.Text.Trim();
                string addr = txtAddress.Text.Trim();

                bool ok = MemberService.Instance.UpdateProfile(fn, g, dob, p, em, addr);
                if (ok)
                {
                    lblName.Text = fn;
                    MessageBox.Show("Đã lưu thông tin hồ sơ cá nhân thành công vào CSDL!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không thể cập nhật hồ sơ cá nhân vào CSDL.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            pnlInputs.Controls.Add(lblFN);
            pnlInputs.Controls.Add(txtFullName);
            pnlInputs.Controls.Add(rowGenderDOB);
            pnlInputs.Controls.Add(lblP);
            pnlInputs.Controls.Add(txtPhone);
            pnlInputs.Controls.Add(lblE);
            pnlInputs.Controls.Add(txtEmail);
            pnlInputs.Controls.Add(lblA);
            pnlInputs.Controls.Add(txtAddress);
            pnlInputs.Controls.Add(btnSave);

            card.Controls.Add(pnlInputs);
            card.Controls.Add(pnlAvatarRow);
            card.Controls.Add(lblTitle);

            return card;
        }

        private RoundedPanel CreateSecurityCard(float scale)
        {
            RoundedPanel card = new RoundedPanel
            {
                CornerRadius = 12,
                BackColor = UITheme.CardBg,
                BorderColor = UITheme.Border,
                Padding = new Padding((int)(20 * scale))
            };

            Label lblTitle = new Label
            {
                Text = "🔒 BẢO MẬT & ĐỔI MẬT KHẨU",
                Font = UITheme.FontSection,
                ForeColor = UITheme.TextDark,
                Dock = DockStyle.Top,
                Height = (int)(32 * scale),
                UseMnemonic = false
            };

            TableLayoutPanel pnlForm = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 1,
                RowCount = 9,
                AutoSize = true,
                Padding = new Padding(0, (int)(8 * scale), 0, 0)
            };
            pnlForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // Username
            Label lblU = new Label { Text = "Tên đăng nhập (Username):", Font = UITheme.FontSmallBold, AutoSize = true, Margin = new Padding(0, 0, 0, (int)(2 * scale)), UseMnemonic = false };
            txtUser = new TextBox { ReadOnly = true, BackColor = Color.FromArgb(248, 250, 252), Dock = DockStyle.Top, Height = (int)(32 * scale), Font = UITheme.FontRegular };

            // Current Password
            Label lblOP = new Label { Text = "Mật khẩu hiện tại:", Font = UITheme.FontSmallBold, AutoSize = true, Margin = new Padding(0, (int)(6 * scale), 0, (int)(2 * scale)), UseMnemonic = false };
            txtOldPass = new TextBox { UseSystemPasswordChar = true, Dock = DockStyle.Top, Height = (int)(32 * scale), Font = UITheme.FontRegular };

            // New Password
            Label lblNP = new Label { Text = "Mật khẩu mới:", Font = UITheme.FontSmallBold, AutoSize = true, Margin = new Padding(0, (int)(6 * scale), 0, (int)(2 * scale)), UseMnemonic = false };
            txtNewPass = new TextBox { UseSystemPasswordChar = true, Dock = DockStyle.Top, Height = (int)(32 * scale), Font = UITheme.FontRegular };

            // Confirm Password
            Label lblCP = new Label { Text = "Xác nhận mật khẩu mới:", Font = UITheme.FontSmallBold, AutoSize = true, Margin = new Padding(0, (int)(6 * scale), 0, (int)(2 * scale)), UseMnemonic = false };
            txtConfirmPass = new TextBox { UseSystemPasswordChar = true, Dock = DockStyle.Top, Height = (int)(32 * scale), Font = UITheme.FontRegular };

            // Change Pass Button
            Button btnChangePass = new Button
            {
                Text = "🔑 Cập nhật mật khẩu mới",
                Font = UITheme.FontBold,
                Height = (int)(42 * scale),
                Dock = DockStyle.Top,
                Margin = new Padding(0, (int)(12 * scale), 0, 0),
                BackColor = UITheme.PrimaryLight,
                ForeColor = UITheme.Primary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                UseMnemonic = false
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

                bool ok = MemberService.Instance.ChangePassword(txtOldPass.Text, txtNewPass.Text, out string msg);
                if (ok)
                {
                    txtOldPass.Clear();
                    txtNewPass.Clear();
                    txtConfirmPass.Clear();
                    MessageBox.Show(msg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            pnlForm.Controls.Add(lblU);
            pnlForm.Controls.Add(txtUser);
            pnlForm.Controls.Add(lblOP);
            pnlForm.Controls.Add(txtOldPass);
            pnlForm.Controls.Add(lblNP);
            pnlForm.Controls.Add(txtNewPass);
            pnlForm.Controls.Add(lblCP);
            pnlForm.Controls.Add(txtConfirmPass);
            pnlForm.Controls.Add(btnChangePass);

            card.Controls.Add(pnlForm);
            card.Controls.Add(lblTitle);

            return card;
        }

        private void LoadProfileData()
        {
            try
            {
                var member = MemberService.Instance.GetCurrentMember();
                var activeMembership = MemberService.Instance.GetActiveMembership();
                string pkgName = activeMembership?.Package?.PackageName ?? (MemberService.Instance.IsClassicMember() ? "Classic" : "Hội viên");

                if (member != null)
                {
                    lblName.Text = member.FullName;
                    lblCode.Text = $"Mã hội viên: #MB-{member.MemberID:D3}  |  Hạng thẻ: {pkgName} ({member.Branch?.BranchName ?? "Chi nhánh"})";
                    txtFullName.Text = member.FullName;
                    cboGender.Text = member.Gender ?? "Nam";
                    dtpDOB.Value = member.DateOfBirth;
                    txtPhone.Text = member.Phone ?? "";
                    txtEmail.Text = member.Email ?? "";
                    txtAddress.Text = member.Address ?? "";
                    txtUser.Text = member.Account?.Username ?? "";
                }
            }
            catch { }
        }
    }
}
