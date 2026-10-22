namespace QuanLyGym.UI.Forms.Common
{
    /// <summary>
    /// Form Quên mật khẩu – HIỆN CHỈ CÓ GIAO DIỆN, chưa xử lý logic (kiểm tra DB, gửi email...).
    /// </summary>
    public partial class frmQuenMatKhau : Form
    {
        private static readonly Color MauVienThuong = Color.FromArgb(203, 213, 225);
        private static readonly Color MauVienFocus = Color.FromArgb(249, 115, 22);

        public frmQuenMatKhau()
        {
            InitializeComponent();
        }

        /// <param name="tenDangNhapGoiY">Tên đăng nhập đã gõ ở form đăng nhập (điền sẵn cho tiện).</param>
        public frmQuenMatKhau(string tenDangNhapGoiY) : this()
        {
            txtTenDangNhap.Text = tenDangNhapGoiY;
        }

        private void frmQuenMatKhau_Shown(object? sender, EventArgs e)
        {
            if (txtTenDangNhap.Text == "") txtTenDangNhap.Focus();
            else txtEmail.Focus();
        }

        private void btnGui_Click(object? sender, EventArgs e)
        {
            // Chỉ kiểm tra dữ liệu nhập ở mức giao diện
            if (txtTenDangNhap.Text.Trim() == "")
            {
                lblLoi.Text = "⚠ Vui lòng nhập tên đăng nhập.";
                txtTenDangNhap.Focus();
                return;
            }

            string email = txtEmail.Text.Trim();
            if (email == "" || !email.Contains('@') || !email.Contains('.'))
            {
                lblLoi.Text = "⚠ Vui lòng nhập email hợp lệ.";
                txtEmail.Focus();
                return;
            }

            // TODO: Xử lý logic quên mật khẩu ở giai đoạn sau
            MessageBox.Show(this, "Chức năng đang được phát triển.\nVui lòng liên hệ quản trị viên để được hỗ trợ.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtInput_TextChanged(object? sender, EventArgs e) => lblLoi.Text = "";

        private void txtTenDangNhap_Enter(object? sender, EventArgs e) => pnlTenDangNhapBorder.BackColor = MauVienFocus;
        private void txtTenDangNhap_Leave(object? sender, EventArgs e) => pnlTenDangNhapBorder.BackColor = MauVienThuong;
        private void txtEmail_Enter(object? sender, EventArgs e) => pnlEmailBorder.BackColor = MauVienFocus;
        private void txtEmail_Leave(object? sender, EventArgs e) => pnlEmailBorder.BackColor = MauVienThuong;
    }
}
