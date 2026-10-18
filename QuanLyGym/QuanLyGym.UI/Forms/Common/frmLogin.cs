using QuanLyGym.BLL.Services;
using QuanLyGym.UI.Helpers;

namespace QuanLyGym.UI.Forms.Common
{
    public partial class frmLogin : Form
    {
        private static readonly Color MauVienThuong = Color.FromArgb(203, 213, 225);
        private static readonly Color MauVienFocus = Color.FromArgb(249, 115, 22);

        private readonly AuthService _authService = new AuthService();

        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Shown(object? sender, EventArgs e)
        {
            txtTenDangNhap.Focus();
        }

        private void btnDangNhap_Click(object? sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text;

            // 1. Kiểm tra dữ liệu nhập
            if (tenDangNhap == "")
            {
                HienLoi("Vui lòng nhập tên đăng nhập.", txtTenDangNhap);
                return;
            }
            if (matKhau == "")
            {
                HienLoi("Vui lòng nhập mật khẩu.", txtMatKhau);
                return;
            }

            // 2. Gọi BLL kiểm tra đăng nhập
            btnDangNhap.Enabled = false;
            btnDangNhap.Text = "Đang kiểm tra...";
            Cursor = Cursors.WaitCursor;

            try
            {
                var result = _authService.Login(tenDangNhap, matKhau);

                switch (result.Status)
                {
                    case LoginStatus.Success:
                        UserSession.AccountID = result.AccountID;
                        UserSession.Username = result.Username;
                        UserSession.Role = result.Role;
                        UserSession.FullName = result.FullName;

                        DialogResult = DialogResult.OK; // Program.cs sẽ mở form theo vai trò
                        Close();
                        break;

                    case LoginStatus.Locked:
                        HienLoi("Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên.", txtTenDangNhap);
                        break;

                    default:
                        HienLoi("Tên đăng nhập hoặc mật khẩu không đúng.", txtMatKhau);
                        txtMatKhau.SelectAll();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Không thể kết nối cơ sở dữ liệu.\n\nChi tiết: " + ex.Message,
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnDangNhap.Enabled = true;
                btnDangNhap.Text = "ĐĂNG NHẬP";
                Cursor = Cursors.Default;
            }
        }

        private void HienLoi(string thongBao, Control focusVao)
        {
            lblLoi.Text = "⚠ " + thongBao;
            focusVao.Focus();
        }

        private void txtInput_TextChanged(object? sender, EventArgs e)
        {
            lblLoi.Text = "";
        }

        private void chkHienMatKhau_CheckedChanged(object? sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }

        private void lnkQuenMatKhau_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            using var f = new frmQuenMatKhau(txtTenDangNhap.Text.Trim());
            f.ShowDialog(this);
        }

        // Đổi màu viền ô nhập khi đang gõ
        private void txtTenDangNhap_Enter(object? sender, EventArgs e) => pnlTenDangNhapBorder.BackColor = MauVienFocus;
        private void txtTenDangNhap_Leave(object? sender, EventArgs e) => pnlTenDangNhapBorder.BackColor = MauVienThuong;
        private void txtMatKhau_Enter(object? sender, EventArgs e) => pnlMatKhauBorder.BackColor = MauVienFocus;
        private void txtMatKhau_Leave(object? sender, EventArgs e) => pnlMatKhauBorder.BackColor = MauVienThuong;
    }
}
