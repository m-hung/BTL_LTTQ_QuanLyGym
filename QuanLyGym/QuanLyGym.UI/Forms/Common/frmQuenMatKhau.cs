using System.ComponentModel;
using System.Net.Mail;
using QuanLyGym.BLL.Services;

namespace QuanLyGym.UI.Forms.Common
{
    /// <summary>
    /// Quên mật khẩu / quên tên đăng nhập, định danh bằng email.
    /// Bước 1: nhập email.  Bước 2: nhập mã OTP 6 số (gửi qua email) + mật khẩu mới.
    /// Chế độ "quên tên đăng nhập": chỉ cần email, tên đăng nhập được gửi qua email.
    /// </summary>
    public partial class frmQuenMatKhau : Form
    {
        private static readonly Color MauVienThuong = Color.FromArgb(203, 213, 225);
        private static readonly Color MauVienFocus = Color.FromArgb(249, 115, 22);
        private static readonly Color MauLoi = Color.FromArgb(220, 38, 38);
        private static readonly Color MauThongTin = Color.FromArgb(22, 163, 74);

        private readonly PasswordResetService _service = new PasswordResetService();

        private int _step = 1;              // 1: nhập email, 2: nhập mã + mật khẩu mới
        private bool _usernameMode;         // true: đang ở chế độ "quên tên đăng nhập"
        private string _email = "";
        private int _cooldown;              // số giây còn lại trước khi được gửi lại mã

        /// <summary>Tên đăng nhập của tài khoản vừa đặt lại mật khẩu (để form đăng nhập điền sẵn).</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? ResetUsername { get; private set; }

        public frmQuenMatKhau()
        {
            InitializeComponent();

            WireInput(txtEmail, pnlEmailBorder);
            WireInput(txtMa, pnlMaBorder);
            WireInput(txtMatKhauMoi, pnlMatKhauMoiBorder);
            WireInput(txtNhapLai, pnlNhapLaiBorder);

            // Ô mã xác nhận chỉ nhận chữ số
            txtMa.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
            };
            txtMa.TextChanged += (s, e) =>
            {
                string digits = new string(txtMa.Text.Where(char.IsDigit).ToArray());
                if (digits != txtMa.Text)
                {
                    txtMa.Text = digits;
                    txtMa.SelectionStart = digits.Length;
                }
            };

            ApplyLayout();
        }

        // ------------------------------------------------------------
        // Giao diện
        // ------------------------------------------------------------

        private void WireInput(TextBox txt, Panel border)
        {
            txt.Enter += (s, e) => border.BackColor = MauVienFocus;
            txt.Leave += (s, e) => border.BackColor = MauVienThuong;
            txt.TextChanged += (s, e) => lblLoi.Text = "";
        }

        /// <summary>Hiển thị đúng nội dung/vị trí theo bước và chế độ hiện tại.</summary>
        private void ApplyLayout()
        {
            pnlStep1.Visible = _step == 1;
            pnlStep2.Visible = _step == 2;
            lnkDoiCheDo.Visible = _step == 1;

            if (_step == 2)
            {
                lblIcon.Text = "✉";
                lblTitle.Text = "Nhập mã xác nhận";
                lblSubtitle.Text = $"Nếu email đã được đăng ký, mã gồm 6 số đã được gửi tới {MaskEmail(_email)}. " +
                                   $"Mã có hiệu lực trong {PasswordResetService.CodeLifetimeMinutes} phút.";
                btnTiepTuc.Text = "ĐỔI MẬT KHẨU";
                btnHuy.Text = "Quay lại";
            }
            else if (_usernameMode)
            {
                lblIcon.Text = "👤";
                lblTitle.Text = "Quên tên đăng nhập?";
                lblSubtitle.Text = "Nhập email đã đăng ký tài khoản. Chúng tôi sẽ gửi tên đăng nhập của bạn qua email này.";
                lnkDoiCheDo.Text = "← Quay lại đặt lại mật khẩu";
                btnTiepTuc.Text = "GỬI TÊN ĐĂNG NHẬP";
                btnHuy.Text = "Hủy";
            }
            else
            {
                lblIcon.Text = "🔑";
                lblTitle.Text = "Quên mật khẩu?";
                lblSubtitle.Text = "Nhập email đã đăng ký tài khoản. Chúng tôi sẽ gửi mã xác nhận gồm 6 số để bạn đặt lại mật khẩu.";
                lnkDoiCheDo.Text = "Quên tên đăng nhập?";
                btnTiepTuc.Text = "GỬI MÃ XÁC NHẬN";
                btnHuy.Text = "Hủy";
            }

            // Xếp các control phía dưới theo chiều cao của bước hiện tại
            int y;
            if (_step == 1)
            {
                y = pnlStep1.Bottom + 4;
                lnkDoiCheDo.Top = y;
                y += lnkDoiCheDo.Height + 6;
            }
            else
            {
                y = pnlStep2.Bottom + 4;
            }

            lblLoi.Top = y;
            y += lblLoi.Height + 6;
            btnHuy.Top = y;
            btnTiepTuc.Top = y;
            ClientSize = new Size(ClientSize.Width, y + btnTiepTuc.Height + 28);
        }

        private void ShowError(string message, Control? focus = null)
        {
            lblLoi.ForeColor = MauLoi;
            lblLoi.Text = "⚠ " + message;
            focus?.Focus();
        }

        private void ShowInfo(string message)
        {
            lblLoi.ForeColor = MauThongTin;
            lblLoi.Text = "✓ " + message;
        }

        private void SetBusy(bool busy)
        {
            btnTiepTuc.Enabled = !busy;
            btnHuy.Enabled = !busy;
            Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
        }

        private static string MaskEmail(string email)
        {
            int at = email.IndexOf('@');
            if (at <= 1) return email;
            return email[0] + new string('*', Math.Min(at - 1, 5)) + email.Substring(at);
        }

        private static bool IsValidEmail(string text)
        {
            try
            {
                return new MailAddress(text).Address == text;
            }
            catch
            {
                return false;
            }
        }

        // ------------------------------------------------------------
        // Sự kiện
        // ------------------------------------------------------------

        private void frmQuenMatKhau_Shown(object? sender, EventArgs e)
        {
            txtEmail.Focus();
        }

        private void lnkDoiCheDo_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            _usernameMode = !_usernameMode;
            lblLoi.Text = "";
            ApplyLayout();
            txtEmail.Focus();
        }

        private void chkHienMatKhau_CheckedChanged(object? sender, EventArgs e)
        {
            txtMatKhauMoi.UseSystemPasswordChar = !chkHienMatKhau.Checked;
            txtNhapLai.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }

        private void btnHuy_Click(object? sender, EventArgs e)
        {
            if (_step == 2)
            {
                // Quay lại bước nhập email
                tmrGuiLai.Stop();
                _step = 1;
                lblLoi.Text = "";
                ApplyLayout();
                txtEmail.Focus();
                return;
            }

            DialogResult = DialogResult.Cancel;
            Close();
        }

        private async void btnTiepTuc_Click(object? sender, EventArgs e)
        {
            if (_step == 1) await XuLyBuoc1Async();
            else XuLyBuoc2();
        }

        private async void lnkGuiLai_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            lblLoi.Text = "";
            if (await YeuCauMaAsync())
            {
                StartCooldown();
                txtMa.Clear();
                txtMa.Focus();
                ShowInfo("Đã gửi lại mã xác nhận.");
            }
        }

        // ------------------------------------------------------------
        // Bước 1: nhập email
        // ------------------------------------------------------------

        private async Task XuLyBuoc1Async()
        {
            string email = txtEmail.Text.Trim();
            if (!IsValidEmail(email))
            {
                ShowError("Vui lòng nhập email hợp lệ.", txtEmail);
                return;
            }
            _email = email;

            if (_usernameMode)
            {
                SetBusy(true);
                try
                {
                    await _service.SendUsernameAsync(_email);
                    if (IsDisposed) return;

                    MessageBox.Show(this,
                        $"Nếu email {MaskEmail(_email)} đã được đăng ký, tên đăng nhập đã được gửi tới hộp thư của bạn.\n" +
                        "Vui lòng kiểm tra cả mục Spam/Thư rác.",
                        "Đã gửi yêu cầu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
                catch (Exception ex)
                {
                    HienLoiHeThong(ex);
                }
                finally
                {
                    if (!IsDisposed) SetBusy(false);
                }
                return;
            }

            if (await YeuCauMaAsync())
            {
                _step = 2;
                lblLoi.Text = "";
                ApplyLayout();
                StartCooldown();
                txtMa.Clear();
                txtMa.Focus();
            }
        }

        /// <summary>Gọi BLL tạo + gửi mã OTP. Trả về false nếu có lỗi hệ thống.</summary>
        private async Task<bool> YeuCauMaAsync()
        {
            SetBusy(true);
            try
            {
                await _service.RequestResetCodeAsync(_email);
                return true;
            }
            catch (Exception ex)
            {
                HienLoiHeThong(ex);
                return false;
            }
            finally
            {
                if (!IsDisposed) SetBusy(false);
            }
        }

        private void HienLoiHeThong(Exception ex)
        {
            if (IsDisposed) return;

            string title = ex is MailConfigException ? "Chưa cấu hình gửi email" : "Có lỗi xảy ra";
            MessageBox.Show(this, ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // ------------------------------------------------------------
        // Bước 2: nhập mã + mật khẩu mới
        // ------------------------------------------------------------

        private void XuLyBuoc2()
        {
            string code = txtMa.Text.Trim();
            string pass = txtMatKhauMoi.Text;
            string confirm = txtNhapLai.Text;

            if (code.Length != 6)
            {
                ShowError("Vui lòng nhập mã xác nhận gồm 6 số.", txtMa);
                return;
            }
            if (pass.Length < PasswordResetService.MinPasswordLength)
            {
                ShowError($"Mật khẩu mới phải có ít nhất {PasswordResetService.MinPasswordLength} ký tự.", txtMatKhauMoi);
                return;
            }
            if (pass != confirm)
            {
                ShowError("Mật khẩu nhập lại không khớp.", txtNhapLai);
                return;
            }

            SetBusy(true);
            try
            {
                var result = _service.ResetPassword(_email, code, pass);

                switch (result.Status)
                {
                    case ResetPasswordStatus.Success:
                        ResetUsername = result.Username;
                        MessageBox.Show(this,
                            "Đặt lại mật khẩu thành công!\n\n" +
                            $"Tên đăng nhập của bạn: {result.Username}\n" +
                            "Vui lòng đăng nhập bằng mật khẩu mới.",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                        break;

                    case ResetPasswordStatus.TooManyAttempts:
                        // Mã đã bị hủy -> cho phép xin mã mới ngay
                        tmrGuiLai.Stop();
                        lnkGuiLai.Enabled = true;
                        lnkGuiLai.Text = "Gửi lại mã";
                        ShowError("Bạn đã nhập sai quá nhiều lần. Hãy bấm \"Gửi lại mã\".");
                        break;

                    case ResetPasswordStatus.WeakPassword:
                        ShowError($"Mật khẩu mới phải có ít nhất {PasswordResetService.MinPasswordLength} ký tự.", txtMatKhauMoi);
                        break;

                    default:
                        ShowError("Mã xác nhận không đúng hoặc đã hết hạn.", txtMa);
                        txtMa.SelectAll();
                        break;
                }
            }
            catch (Exception ex)
            {
                HienLoiHeThong(ex);
            }
            finally
            {
                if (!IsDisposed) SetBusy(false);
            }
        }

        // ------------------------------------------------------------
        // Đếm ngược cho nút "Gửi lại mã"
        // ------------------------------------------------------------

        private void StartCooldown()
        {
            _cooldown = PasswordResetService.ResendCooldownSeconds;
            lnkGuiLai.Enabled = false;
            UpdateResendText();
            tmrGuiLai.Start();
        }

        private void tmrGuiLai_Tick(object? sender, EventArgs e)
        {
            _cooldown--;
            if (_cooldown <= 0)
            {
                tmrGuiLai.Stop();
                lnkGuiLai.Enabled = true;
            }
            UpdateResendText();
        }

        private void UpdateResendText()
        {
            lnkGuiLai.Text = _cooldown > 0 ? $"Gửi lại mã ({_cooldown}s)" : "Gửi lại mã";
        }
    }
}
