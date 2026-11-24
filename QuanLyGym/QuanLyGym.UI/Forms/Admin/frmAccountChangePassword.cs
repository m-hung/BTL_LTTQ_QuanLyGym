using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public class frmAccountChangePassword : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        private static readonly Color BgColor = Color.FromArgb(15, 15, 17);
        private static readonly Color CardBg = Color.FromArgb(24, 24, 27);
        private static readonly Color ItemBg = Color.FromArgb(20, 20, 23);
        private static readonly Color Border = Color.FromArgb(45, 45, 50);
        private static readonly Color TextColor = Color.FromArgb(244, 244, 245);
        private static readonly Color MutedColor = Color.FromArgb(161, 161, 170);
        private static readonly Color Orange = Color.FromArgb(249, 115, 22);
        private static readonly Color Green = Color.FromArgb(34, 197, 94);
        private static readonly Color Red = Color.FromArgb(239, 68, 68);

        private readonly string _username;
        private readonly string _fullName;
        private readonly string _role;

        private TextBox _txtNewPassword = null!;
        private TextBox _txtConfirmPassword = null!;
        private CheckBox _chkRequireChange = null!;
        private Label _lblStrength = null!;
        private Panel _pnlStrengthBar = null!;

        public string NewPassword { get; private set; } = string.Empty;
        public bool RequireChangeOnNextLogin { get; private set; }

        public frmAccountChangePassword(string username, string fullName, string role)
        {
            _username = username;
            _fullName = fullName;
            _role = role;
            BuildUI();
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(500, 480);
            BackColor = BgColor;
            ShowInTaskbar = false;
            DoubleBuffered = true;
            KeyPreview = true;

            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            };

            Paint += (s, e) =>
            {
                using var p = new Pen(Border, 1.5f);
                e.Graphics.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
            };

            // 1. Title bar
            var pnlTitle = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = ItemBg,
                Padding = new Padding(16, 0, 16, 0)
            };
            pnlTitle.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };
            pnlTitle.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawLine(p, 0, pnlTitle.Height - 1, pnlTitle.Width, pnlTitle.Height - 1);
            };

            var lblTitle = new Label
            {
                Text = "🔑  ĐỔI / RESET MẬT KHẨU TÀI KHOẢN",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(16, 12)
            };

            var lblSub = new Label
            {
                Text = $"Cập nhật mật khẩu đăng nhập cho người dùng {_username}",
                Font = new Font("Segoe UI", 8F),
                ForeColor = MutedColor,
                AutoSize = true,
                Location = new Point(18, 33)
            };

            var btnClose = new Button
            {
                Text = "✕",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = MutedColor,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(36, 36),
                Location = new Point(500 - 16 - 36, 10),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.MouseEnter += (s, e) => { btnClose.BackColor = Red; btnClose.ForeColor = Color.White; };
            btnClose.MouseLeave += (s, e) => { btnClose.BackColor = Color.Transparent; btnClose.ForeColor = MutedColor; };
            btnClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            pnlTitle.Controls.Add(lblTitle);
            pnlTitle.Controls.Add(lblSub);
            pnlTitle.Controls.Add(btnClose);

            // 2. Body
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24, 20, 24, 20),
                BackColor = BgColor
            };

            // User Info Banner
            var pnlUser = new Panel
            {
                Location = new Point(24, 16),
                Size = new Size(452, 64),
                BackColor = CardBg
            };
            pnlUser.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlUser.Width - 1, pnlUser.Height - 1);
            };

            var lblUName = new Label
            {
                Text = $"👤  {_fullName} ({_username})",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(16, 12),
                AutoSize = true
            };
            var lblURole = new Label
            {
                Text = $"Vai trò tài khoản: {_role}",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Orange,
                Location = new Point(16, 34),
                AutoSize = true
            };
            pnlUser.Controls.Add(lblUName);
            pnlUser.Controls.Add(lblURole);
            pnlBody.Controls.Add(pnlUser);

            // Mật khẩu mới label + Random button
            int curY = 96;
            var lblNewPassTitle = new Label
            {
                Text = "MẬT KHẨU MỚI *",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24, curY),
                AutoSize = true
            };
            pnlBody.Controls.Add(lblNewPassTitle);

            var btnRandom = new Button
            {
                Text = "🎲  Tạo ngẫu nhiên",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Orange,
                BackColor = Color.FromArgb(40, 25, 20),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 24),
                Location = new Point(346, curY - 3),
                Cursor = Cursors.Hand
            };
            btnRandom.FlatAppearance.BorderColor = Orange;
            btnRandom.Click += (s, e) => GenerateRandomPassword();
            pnlBody.Controls.Add(btnRandom);

            curY += 24;
            var pnlTxtNew = CreateTextBoxPanel(out _txtNewPassword, true);
            pnlTxtNew.Location = new Point(24, curY);
            pnlTxtNew.Size = new Size(452, 38);
            _txtNewPassword.TextChanged += (s, e) => UpdatePasswordStrength();
            pnlBody.Controls.Add(pnlTxtNew);

            curY += 44;
            // Password strength bar
            var pnlBarContainer = new Panel
            {
                Location = new Point(24, curY),
                Size = new Size(320, 6),
                BackColor = Color.FromArgb(39, 39, 42)
            };
            _pnlStrengthBar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 0,
                BackColor = Red
            };
            pnlBarContainer.Controls.Add(_pnlStrengthBar);
            pnlBody.Controls.Add(pnlBarContainer);

            _lblStrength = new Label
            {
                Text = "Độ mạnh: Chưa nhập",
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = MutedColor,
                Location = new Point(350, curY - 5),
                AutoSize = true
            };
            pnlBody.Controls.Add(_lblStrength);

            curY += 18;
            var lblConfirmTitle = new Label
            {
                Text = "XÁC NHẬN MẬT KHẨU MỚI *",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24, curY),
                AutoSize = true
            };
            pnlBody.Controls.Add(lblConfirmTitle);

            curY += 24;
            var pnlTxtConfirm = CreateTextBoxPanel(out _txtConfirmPassword, true);
            pnlTxtConfirm.Location = new Point(24, curY);
            pnlTxtConfirm.Size = new Size(452, 38);
            pnlBody.Controls.Add(pnlTxtConfirm);

            curY += 46;
            _chkRequireChange = new CheckBox
            {
                Text = "Bắt buộc người dùng đổi mật khẩu ở lần đăng nhập tiếp theo",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextColor,
                Location = new Point(26, curY),
                Size = new Size(450, 24),
                Checked = true
            };
            pnlBody.Controls.Add(_chkRequireChange);

            // 3. Bottom actions
            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 62,
                BackColor = ItemBg,
                Padding = new Padding(24, 12, 24, 12)
            };
            pnlBottom.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawLine(p, 0, 0, pnlBottom.Width, 0);
            };

            var btnCancel = new Button
            {
                Text = "Hủy bỏ",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = MutedColor,
                BackColor = CardBg,
                Size = new Size(110, 38),
                Dock = DockStyle.Left,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderColor = Border;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            var btnSave = new Button
            {
                Text = "💾  Lưu mật khẩu mới",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(180, 38),
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnSave);

            Controls.Add(pnlBody);
            Controls.Add(pnlBottom);
            Controls.Add(pnlTitle);
        }

        private Panel CreateTextBoxPanel(out TextBox txt, bool isPassword)
        {
            var pnl = new Panel
            {
                BackColor = ItemBg,
                Padding = new Padding(12, 8, 40, 8)
            };
            pnl.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            txt = new TextBox
            {
                Dock = DockStyle.Fill,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.None,
                UseSystemPasswordChar = isPassword
            };

            if (isPassword)
            {
                var targetTxt = txt;
                var btnToggle = new Button
                {
                    Text = "👁",
                    Font = new Font("Segoe UI", 9F),
                    ForeColor = MutedColor,
                    BackColor = Color.Transparent,
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(30, 24),
                    Location = new Point(pnl.Width - 34, 7),
                    Cursor = Cursors.Hand,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                btnToggle.FlatAppearance.BorderSize = 0;
                btnToggle.Click += (s, e) =>
                {
                    targetTxt.UseSystemPasswordChar = !targetTxt.UseSystemPasswordChar;
                    btnToggle.ForeColor = targetTxt.UseSystemPasswordChar ? MutedColor : Orange;
                };
                pnl.Controls.Add(btnToggle);
            }

            pnl.Controls.Add(txt);
            return pnl;
        }

        private void GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789!@#$%";
            var rnd = new Random();
            var sb = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                sb.Append(chars[rnd.Next(chars.Length)]);
            }
            string pass = sb.ToString();
            _txtNewPassword.UseSystemPasswordChar = false;
            _txtConfirmPassword.UseSystemPasswordChar = false;
            _txtNewPassword.Text = pass;
            _txtConfirmPassword.Text = pass;
        }

        private void UpdatePasswordStrength()
        {
            string pass = _txtNewPassword.Text;
            if (string.IsNullOrEmpty(pass))
            {
                _pnlStrengthBar.Width = 0;
                _lblStrength.Text = "Độ mạnh: Chưa nhập";
                _lblStrength.ForeColor = MutedColor;
                return;
            }

            int score = 0;
            if (pass.Length >= 6) score++;
            if (pass.Length >= 8) score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(pass, @"[A-Z]")) score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(pass, @"[0-9]")) score++;
            if (System.Text.RegularExpressions.Regex.IsMatch(pass, @"[^a-zA-Z0-9]")) score++;

            if (score <= 2)
            {
                _pnlStrengthBar.Width = 100;
                _pnlStrengthBar.BackColor = Red;
                _lblStrength.Text = "Độ mạnh: Yếu";
                _lblStrength.ForeColor = Red;
            }
            else if (score <= 4)
            {
                _pnlStrengthBar.Width = 220;
                _pnlStrengthBar.BackColor = Orange;
                _lblStrength.Text = "Độ mạnh: Trung bình";
                _lblStrength.ForeColor = Orange;
            }
            else
            {
                _pnlStrengthBar.Width = 320;
                _pnlStrengthBar.BackColor = Green;
                _lblStrength.Text = "Độ mạnh: Rất mạnh";
                _lblStrength.ForeColor = Green;
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string newPass = _txtNewPassword.Text.Trim();
            string confirm = _txtConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(newPass))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtNewPassword.Focus();
                return;
            }

            if (newPass.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtNewPassword.Focus();
                return;
            }

            if (newPass != confirm)
            {
                MessageBox.Show("Xác nhận mật khẩu không khớp. Vui lòng kiểm tra lại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtConfirmPassword.Focus();
                return;
            }

            NewPassword = newPass;
            RequireChangeOnNextLogin = _chkRequireChange.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
