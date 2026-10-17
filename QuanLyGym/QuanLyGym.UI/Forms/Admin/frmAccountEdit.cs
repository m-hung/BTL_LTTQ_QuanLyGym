using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace QuanLyGym.UI.Forms.Admin
{
    public class frmAccountEdit : Form
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

        public class AccountEditModel
        {
            public int Id { get; set; }
            public string AccountType { get; set; } = "Admin"; // Admin, Staff, Member
            public string Username { get; set; } = "";
            public string FullName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Phone { get; set; } = "";
            public string SubRoleOrTitle { get; set; } = "";
            public string BranchName { get; set; } = "";
            public string Tier { get; set; } = "";
            public string MemberCode { get; set; } = "";
            public bool IsActive { get; set; } = true;
            public DateTime CreatedAt { get; set; }
            public DateTime? LastLogin { get; set; }
            public string? NewPassword { get; set; }
        }

        private readonly AccountEditModel _account;
        public AccountEditModel UpdatedAccount { get; private set; }

        private TextBox _txtUsername = null!;
        private TextBox _txtFullName = null!;
        private TextBox _txtEmail = null!;
        private TextBox _txtPhone = null!;
        private ComboBox _cboRoleOrTier = null!;
        private ComboBox _cboBranch = null!;
        private ComboBox _cboStatus = null!;
        private TextBox _txtNewPassword = null!;

        public frmAccountEdit(AccountEditModel account)
        {
            _account = account;
            UpdatedAccount = new AccountEditModel
            {
                Id = account.Id,
                AccountType = account.AccountType,
                Username = account.Username,
                FullName = account.FullName,
                Email = account.Email,
                Phone = account.Phone,
                SubRoleOrTitle = account.SubRoleOrTitle,
                BranchName = account.BranchName,
                Tier = account.Tier,
                MemberCode = account.MemberCode,
                IsActive = account.IsActive,
                CreatedAt = account.CreatedAt,
                LastLogin = account.LastLogin
            };
            BuildUI();
        }

        private void BuildUI()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(580, 690);
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

            string typeHeader = _account.AccountType == "Admin" ? "QUẢN TRỊ VIÊN" : (_account.AccountType == "Staff" ? "NHÂN VIÊN / HLV" : "HỘI VIÊN GYM");
            var lblTitle = new Label
            {
                Text = $"✏️  CHỈNH SỬA TÀI KHOẢN ({typeHeader})",
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = TextColor,
                AutoSize = true,
                Location = new Point(16, 12)
            };

            var lblSub = new Label
            {
                Text = $"Cập nhật hồ sơ, quyền hạn và bảo mật tài khoản '{_account.Username}'",
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

            // 2. Body
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24, 16, 24, 16),
                AutoScroll = true,
                BackColor = BgColor
            };

            int curY = 12;

            // Username (Readonly or Editable)
            var lblUsername = new Label
            {
                Text = "TÊN ĐĂNG NHẬP (USERNAME)",
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
            _txtUsername.Text = _account.Username;
            _txtUsername.ReadOnly = true; // Username cannot be changed to ensure consistency
            _txtUsername.ForeColor = MutedColor;
            pnlBody.Controls.Add(pnlUsername);

            curY += 46;
            // Full Name
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
            _txtFullName.Text = _account.FullName;
            pnlBody.Controls.Add(pnlFull);

            curY += 46;
            // Email and Phone
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
            _txtEmail.Text = _account.Email;

            var pnlPhone = CreateInputPanel(out _txtPhone);
            pnlPhone.Location = new Point(24 + colW + 14, curY);
            pnlPhone.Size = new Size(colW, 38);
            _txtPhone.Text = _account.Phone;

            pnlBody.Controls.Add(pnlEmail);
            pnlBody.Controls.Add(pnlPhone);

            curY += 46;
            // Dynamic Role or Tier + Branch
            string roleLabelText = _account.AccountType == "Admin" ? "CẤP BẬC / PHÂN QUYỀN" : (_account.AccountType == "Staff" ? "CHỨC VỤ / VỊ TRÍ" : "HẠNG THẺ HỘI VIÊN");
            var lblRole = new Label
            {
                Text = $"{roleLabelText} *",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24, curY),
                AutoSize = true
            };
            var lblBranch = new Label
            {
                Text = "CHI NHÁNH CƠ SỞ *",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24 + colW + 14, curY),
                AutoSize = true
            };
            pnlBody.Controls.Add(lblRole);
            pnlBody.Controls.Add(lblBranch);

            curY += 22;
            _cboRoleOrTier = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(24, curY),
                Size = new Size(colW, 34)
            };
            PopulateRoleItems();

            _cboBranch = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(24 + colW + 14, curY),
                Size = new Size(colW, 34)
            };
            _cboBranch.Items.AddRange(new object[]
            {
                "GymFit Quận 1 - Bến Nghé",
                "GymFit Bình Thạnh - Landmark",
                "GymFit Hoàn Kiếm - Tràng Tiền",
                "GymFit Cầu Giấy - Duy Tân",
                "Tất cả chi nhánh (Toàn hệ thống)"
            });
            SelectComboItem(_cboBranch, _account.BranchName);

            pnlBody.Controls.Add(_cboRoleOrTier);
            pnlBody.Controls.Add(_cboBranch);

            curY += 46;
            // Trạng thái tài khoản (Status)
            var lblStatus = new Label
            {
                Text = "TRẠNG THÁI TÀI KHOẢN *",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = TextColor,
                Location = new Point(24, curY),
                AutoSize = true
            };
            pnlBody.Controls.Add(lblStatus);

            curY += 22;
            _cboStatus = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = ItemBg,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 9.5F),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(24, curY),
                Size = new Size(532, 34)
            };
            _cboStatus.Items.AddRange(new object[]
            {
                "🟢 Đang hoạt động (Cho phép đăng nhập bình thường)",
                "🔴 Đang bị khóa (Tạm dừng quyền truy cập)"
            });
            _cboStatus.SelectedIndex = _account.IsActive ? 0 : 1;
            pnlBody.Controls.Add(_cboStatus);

            curY += 46;
            // Đổi mật khẩu mới (Tùy chọn)
            var pnlPassSection = new Panel
            {
                Location = new Point(24, curY),
                Size = new Size(532, 74),
                BackColor = CardBg,
                Padding = new Padding(14, 10, 14, 10)
            };
            pnlPassSection.Paint += (s, e) =>
            {
                using var p = new Pen(Border);
                e.Graphics.DrawRectangle(p, 0, 0, pnlPassSection.Width - 1, pnlPassSection.Height - 1);
            };

            var lblPassTitle = new Label
            {
                Text = "🔑  ĐẶT LẠI MẬT KHẨU MỚI (Bỏ trống nếu giữ nguyên)",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Orange,
                Location = new Point(14, 10),
                AutoSize = true
            };
            pnlPassSection.Controls.Add(lblPassTitle);

            var pnlTxtPass = CreateInputPanel(out _txtNewPassword);
            pnlTxtPass.Location = new Point(14, 30);
            pnlTxtPass.Size = new Size(504, 34);
            _txtNewPassword.PlaceholderText = "Nhập mật khẩu mới tại đây (tối thiểu 6 ký tự)...";
            _txtNewPassword.UseSystemPasswordChar = true;
            pnlPassSection.Controls.Add(pnlTxtPass);
            pnlBody.Controls.Add(pnlPassSection);

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
                Text = "💾  Lưu thay đổi",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Orange,
                Size = new Size(160, 38),
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

        private void PopulateRoleItems()
        {
            _cboRoleOrTier.Items.Clear();
            if (_account.AccountType == "Admin")
            {
                _cboRoleOrTier.Items.AddRange(new object[]
                {
                    "Super Admin",
                    "Quản lý Chi nhánh",
                    "Quản lý Tài chính",
                    "Giám sát Vận hành",
                    "Admin CSKH"
                });
                SelectComboItem(_cboRoleOrTier, _account.SubRoleOrTitle);
            }
            else if (_account.AccountType == "Staff")
            {
                _cboRoleOrTier.Items.AddRange(new object[]
                {
                    "HLV Thể hình Cá nhân (PT Gym)",
                    "HLV Boxing & Kickfit",
                    "HLV Yoga & Pilates",
                    "HLV Trưởng Chi Nhánh",
                    "Nhân viên Lễ tân",
                    "Nhân viên Thu ngân",
                    "Kỹ thuật viên Thiết bị"
                });
                SelectComboItem(_cboRoleOrTier, _account.SubRoleOrTitle);
            }
            else // Member
            {
                _cboRoleOrTier.Items.AddRange(new object[]
                {
                    "Classic",
                    "Silver",
                    "Gold",
                    "Platinum",
                    "VIP Diamond"
                });
                SelectComboItem(_cboRoleOrTier, _account.Tier);
            }
        }

        private static void SelectComboItem(ComboBox cbo, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
                return;
            }
            for (int i = 0; i < cbo.Items.Count; i++)
            {
                string text = cbo.Items[i]?.ToString() ?? "";
                if (text.StartsWith(value, StringComparison.OrdinalIgnoreCase) || value.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                {
                    cbo.SelectedIndex = i;
                    return;
                }
            }
            if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
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

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string fullName = _txtFullName.Text.Trim();
            string email = _txtEmail.Text.Trim();
            string phone = _txtPhone.Text.Trim();
            string newPass = _txtNewPassword.Text.Trim();

            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Vui lòng nhập họ và tên chủ tài khoản!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtFullName.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(newPass) && newPass.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới nếu thay đổi phải có ít nhất 6 ký tự!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtNewPassword.Focus();
                return;
            }

            string selectedRoleOrTier = _cboRoleOrTier.SelectedItem?.ToString() ?? "";
            string selectedBranch = _cboBranch.SelectedItem?.ToString() ?? "";
            bool isActive = _cboStatus.SelectedIndex == 0;

            UpdatedAccount.FullName = fullName;
            UpdatedAccount.Email = email;
            UpdatedAccount.Phone = phone;
            UpdatedAccount.BranchName = selectedBranch;
            UpdatedAccount.IsActive = isActive;
            UpdatedAccount.NewPassword = string.IsNullOrEmpty(newPass) ? null : newPass;

            if (_account.AccountType == "Admin" || _account.AccountType == "Staff")
            {
                UpdatedAccount.SubRoleOrTitle = selectedRoleOrTier.Split('(')[0].Trim();
            }
            else
            {
                UpdatedAccount.Tier = selectedRoleOrTier;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
