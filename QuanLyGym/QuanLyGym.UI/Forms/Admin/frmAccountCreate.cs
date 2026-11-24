using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public class frmAccountCreate : Form
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
        private static readonly Color Blue = Color.FromArgb(59, 130, 246);
        private static readonly Color Red = Color.FromArgb(239, 68, 68);

        public class AccountResult
        {
            public string AccountType { get; set; } = "Admin"; // Admin, Staff, Member
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
            public string FullName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public string SubRoleOrTitle { get; set; } = "";
            public string BranchName { get; set; } = "";
            public string Tier { get; set; } = "";
            public string MemberCode { get; set; } = "";
            public bool IsActive { get; set; } = true;
            public DateTime CreatedAt { get; set; } = DateTime.Now;
        }

        public AccountResult? CreatedAccount { get; private set; }

        private readonly string _initialType;
        private readonly HashSet<string> _existingUsernames;

        // UI inputs
        private RadioButton _rbAdmin = null!;
        private RadioButton _rbStaff = null!;
        private RadioButton _rbMember = null!;

        private TextBox _txtUsername = null!;
        private TextBox _txtPassword = null!;
        private TextBox _txtFullName = null!;
        private TextBox _txtEmail = null!;
        private TextBox _txtPhone = null!;

        // Dynamic controls
        private Label _lblDynamicTitle = null!;
        private ComboBox _cboDynamicRole = null!;
        private Label _lblBranch = null!;
        private ComboBox _cboBranch = null!;
        private Panel _pnlBranch = null!;

        private CheckBox _chkActive = null!;

        public frmAccountCreate(string initialType, IEnumerable<string> existingUsernames)
        {
            _initialType = initialType;
            _existingUsernames = new HashSet<string>(existingUsernames, StringComparer.OrdinalIgnoreCase);
            BuildUI();
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(580, 680);
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
                Text = "➕  TẠO MỚI TÀI KHOẢN HỆ THỐNG",
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(16, 12)
            };

            var lblSub = new Label
            {
                Text = "Cấp tài khoản đăng nhập cho Quản trị viên, Nhân viên hoặc Hội viên",
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
                Location = new Point(580 - 16 - 36, 10),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.MouseEnter += (s, e) => { btnClose.BackColor = Red; btnClose.ForeColor = Color.White; };
            btnClose.MouseLeave += (s, e) => { btnClose.BackColor = Color.Transparent; btnClose.ForeColor = MutedColor; };
            btnClose.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            pnlTitle.Controls.Add(lblTitle);
            pnlTitle.Controls.Add(lblSub);
            pnlTitle.Controls.Add(btnClose);

            // 2. Scrollable Body
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24, 16, 24, 16),
                AutoScroll = true,
                BackColor = BgColor
            };

            int curY = 10;

            // Loại tài khoản Selector
            var lblTypeHeader = new Label
            {
                Text = "CHỌN LOẠI TÀI KHOẢN CẦN TẠO *",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Orange,
                Location = new Point(24, curY),
                AutoSize = true
            };
            pnlBody.Controls.Add(lblTypeHeader);

            curY += 24;
            var pnlType = new Panel
            {
                Location = new Point(24, curY),
                Size = new Size(532, 46),
                BackColor = CardBg
            };
            pnlType.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlType.Width - 1, pnlType.Height - 1);
            };

            _rbAdmin = new RadioButton
            {
                Text = "🛡️ Quản trị viên (Admin)",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(14, 10),
                Size = new Size(165, 26),
                Checked = _initialType == "Admin"
            };
            _rbStaff = new RadioButton
            {
                Text = "👔 Nhân viên / PT",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(190, 10),
                Size = new Size(150, 26),
                Checked = _initialType == "Staff"
            };
            _rbMember = new RadioButton
            {
                Text = "🏃 Hội viên Gym",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(355, 10),
                Size = new Size(150, 26),
                Checked = _initialType == "Member"
            };

            _rbAdmin.CheckedChanged += (s, e) => { if (_rbAdmin.Checked) OnAccountTypeChanged(); };
            _rbStaff.CheckedChanged += (s, e) => { if (_rbStaff.Checked) OnAccountTypeChanged(); };
            _rbMember.CheckedChanged += (s, e) => { if (_rbMember.Checked) OnAccountTypeChanged(); };

            pnlType.Controls.Add(_rbAdmin);
            pnlType.Controls.Add(_rbStaff);
            pnlType.Controls.Add(_rbMember);
            pnlBody.Controls.Add(pnlType);

            curY += 56;
            // Username and Password
            var lblUsername = new Label
            {
                Text = "TÊN ĐĂNG NHẬP (USERNAME) *",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24, curY),
                AutoSize = true
            };
            pnlBody.Controls.Add(lblUsername);

            curY += 22;
            var pnlUsername = CreateInputPanel(out _txtUsername);
            pnlUsername.Location = new Point(24, curY);
            pnlUsername.Size = new Size(532, 38);
            pnlBody.Controls.Add(pnlUsername);

            curY += 46;
            var lblPass = new Label
            {
                Text = "MẬT KHẨU ĐĂNG NHẬP *",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24, curY),
                AutoSize = true
            };
            pnlBody.Controls.Add(lblPass);

            var btnRandom = new Button
            {
                Text = "🎲  Tạo ngẫu nhiên",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Orange,
                BackColor = Color.FromArgb(40, 25, 20),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 24),
                Location = new Point(426, curY - 3),
                Cursor = Cursors.Hand
            };
            btnRandom.FlatAppearance.BorderColor = Orange;
            btnRandom.Click += (s, e) =>
            {
                const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789!@#$";
                var rnd = new Random();
                var sb = new StringBuilder();
                for (int i = 0; i < 9; i++) sb.Append(chars[rnd.Next(chars.Length)]);
                _txtPassword.UseSystemPasswordChar = false;
                _txtPassword.Text = sb.ToString();
            };
            pnlBody.Controls.Add(btnRandom);

            curY += 22;
            var pnlPass = CreatePasswordPanel(out _txtPassword);
            pnlPass.Location = new Point(24, curY);
            pnlPass.Size = new Size(532, 38);
            _txtPassword.Text = "GymFit@2026";
            pnlBody.Controls.Add(pnlPass);

            curY += 46;
            // Full name
            var lblFull = new Label
            {
                Text = "HỌ VÀ TÊN NGƯỜI DÙNG *",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24, curY),
                AutoSize = true
            };
            pnlBody.Controls.Add(lblFull);

            curY += 22;
            var pnlFull = CreateInputPanel(out _txtFullName);
            pnlFull.Location = new Point(24, curY);
            pnlFull.Size = new Size(532, 38);
            pnlBody.Controls.Add(pnlFull);

            curY += 46;
            // Two columns: Email and Phone
            int colW = (532 - 14) / 2;
            var lblEmail = new Label
            {
                Text = "EMAIL LIÊN HỆ *",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24, curY),
                AutoSize = true
            };
            var lblPhone = new Label
            {
                Text = "SỐ ĐIỆN THOẠI *",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24 + colW + 14, curY),
                AutoSize = true
            };
            pnlBody.Controls.Add(lblEmail);
            pnlBody.Controls.Add(lblPhone);

            curY += 22;
            var pnlEmail = CreateInputPanel(out _txtEmail);
            pnlEmail.Location = new Point(24, curY);
            pnlEmail.Size = new Size(colW, 38);

            var pnlPhone = CreateInputPanel(out _txtPhone);
            pnlPhone.Location = new Point(24 + colW + 14, curY);
            pnlPhone.Size = new Size(colW, 38);

            pnlBody.Controls.Add(pnlEmail);
            pnlBody.Controls.Add(pnlPhone);

            curY += 46;
            // Dynamic Role Title
            _lblDynamicTitle = new Label
            {
                Text = "CẤP BẬC / PHÂN QUYỀN *",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24, curY),
                AutoSize = true
            };
            pnlBody.Controls.Add(_lblDynamicTitle);

            curY += 22;
            _cboDynamicRole = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(24, curY),
                Size = new Size(532, 32)
            };
            pnlBody.Controls.Add(_cboDynamicRole);

            curY += 42;
            // Branch Selection
            _lblBranch = new Label
            {
                Text = "CHI NHÁNH CƠ SỞ TRỰC THUỘC *",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24, curY),
                AutoSize = true
            };
            pnlBody.Controls.Add(_lblBranch);

            curY += 22;
            _pnlBranch = new Panel
            {
                Location = new Point(24, curY),
                Size = new Size(532, 34),
                BackColor = Color.Transparent
            };
            _cboBranch = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                FlatStyle = FlatStyle.Flat
            };
            _cboBranch.Items.AddRange(new object[]
            {
                "GymFit Quận 1 - Bến Nghé",
                "GymFit Bình Thạnh - Landmark",
                "GymFit Hoàn Kiếm - Tràng Tiền",
                "GymFit Cầu Giấy - Duy Tân",
                "Tất cả chi nhánh (Toàn hệ thống)"
            });
            _cboBranch.SelectedIndex = 0;
            _pnlBranch.Controls.Add(_cboBranch);
            pnlBody.Controls.Add(_pnlBranch);

            curY += 44;
            // Status checkbox
            _chkActive = new CheckBox
            {
                Text = "Kích hoạt tài khoản ngay (Cho phép đăng nhập sau khi tạo)",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Green,
                Location = new Point(26, curY),
                Size = new Size(530, 24),
                Checked = true
            };
            pnlBody.Controls.Add(_chkActive);

            // Trigger initial dynamic roles
            OnAccountTypeChanged();

            // 3. Bottom Toolbar
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
                Text = "💾  Tạo tài khoản",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(170, 38),
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

        private void OnAccountTypeChanged()
        {
            _cboDynamicRole.Items.Clear();

            if (_rbAdmin.Checked)
            {
                _lblDynamicTitle.Text = "CẤP BẬC / PHÂN QUYỀN QUẢN TRỊ *";
                _cboDynamicRole.Items.AddRange(new object[]
                {
                    "Super Admin (Toàn quyền hệ thống)",
                    "Quản lý Chi nhánh (Branch Manager)",
                    "Quản lý Tài chính & Kế toán",
                    "Giám sát Vận hành & Thiết bị",
                    "Chăm sóc Khách hàng (Admin CSKH)"
                });
                _lblBranch.Visible = true;
                _pnlBranch.Visible = true;
                _cboBranch.SelectedIndex = 4; // Toàn hệ thống
            }
            else if (_rbStaff.Checked)
            {
                _lblDynamicTitle.Text = "VỊ TRÍ / VAI TRÒ NHÂN VIÊN *";
                _cboDynamicRole.Items.AddRange(new object[]
                {
                    "HLV Thể hình Cá nhân (Personal Trainer - Gym)",
                    "HLV Boxing & Kickfit",
                    "HLV Yoga & Giãn cơ Pilates",
                    "HLV Trưởng Chi Nhánh (Head Coach)",
                    "Nhân viên Lễ tân (Front Desk)",
                    "Nhân viên Thu ngân & Bán hàng",
                    "Kỹ thuật viên Thiết bị & Phòng máy"
                });
                _lblBranch.Visible = true;
                _pnlBranch.Visible = true;
                _cboBranch.SelectedIndex = 0;
            }
            else // Member
            {
                _lblDynamicTitle.Text = "HẠNG THẺ HỘI VIÊN GYM *";
                _cboDynamicRole.Items.AddRange(new object[]
                {
                    "Classic (Gói tiêu chuẩn)",
                    "Silver (Gói bạc đa chi nhánh)",
                    "Gold (Gói vàng Full tiện ích)",
                    "Platinum (Gói bạch kim VIP)",
                    "VIP Diamond (Gói kim cương thượng hạng)"
                });
                _lblBranch.Visible = true;
                _pnlBranch.Visible = true;
                _cboBranch.SelectedIndex = 0;
            }

            _cboDynamicRole.SelectedIndex = 0;
        }

        private Panel CreateInputPanel(out TextBox txt)
        {
            var pnl = new Panel
            {
                BackColor = ItemBg,
                Padding = new Padding(12, 8, 12, 8)
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
                BorderStyle = BorderStyle.None
            };
            pnl.Controls.Add(txt);
            return pnl;
        }

        private Panel CreatePasswordPanel(out TextBox txt)
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
                UseSystemPasswordChar = true
            };

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
            pnl.Controls.Add(txt);
            return pnl;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string username = _txtUsername.Text.Trim();
            string password = _txtPassword.Text.Trim();
            string fullName = _txtFullName.Text.Trim();
            string email = _txtEmail.Text.Trim();
            string phone = _txtPhone.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập (Username)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtUsername.Focus();
                return;
            }

            if (_existingUsernames.Contains(username))
            {
                MessageBox.Show($"Tên đăng nhập '{username}' đã tồn tại trong hệ thống. Vui lòng chọn tên khác!", "Trùng tên đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password) || password.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có độ dài tối thiểu 6 ký tự!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtPassword.Focus();
                return;
            }

            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Vui lòng nhập họ và tên chủ tài khoản!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtFullName.Focus();
                return;
            }

            string accountType = _rbAdmin.Checked ? "Admin" : (_rbStaff.Checked ? "Staff" : "Member");
            string selectedRoleOrTier = _cboDynamicRole.SelectedItem?.ToString() ?? "";
            string selectedBranch = _cboBranch.SelectedItem?.ToString() ?? "";

            string subRole = "";
            string tier = "";
            if (accountType == "Admin")
            {
                subRole = selectedRoleOrTier.Split('(')[0].Trim();
            }
            else if (accountType == "Staff")
            {
                subRole = selectedRoleOrTier.Split('(')[0].Trim();
            }
            else
            {
                tier = selectedRoleOrTier.Split('(')[0].Trim();
            }

            CreatedAccount = new AccountResult
            {
                AccountType = accountType,
                Username = username,
                Password = password,
                FullName = fullName,
                Email = email,
                Phone = phone,
                SubRoleOrTitle = subRole,
                BranchName = selectedBranch,
                Tier = tier,
                MemberCode = accountType == "Member" ? $"HV-{new Random().Next(120, 999)}" : "",
                IsActive = _chkActive.Checked,
                CreatedAt = DateTime.Now
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
