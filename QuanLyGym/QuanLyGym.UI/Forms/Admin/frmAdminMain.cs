using QuanLyGym.UI.Helpers;

namespace QuanLyGym.UI.Forms.Admin
{
    public partial class frmAdminMain : Form
    {
        public frmAdminMain()
        {
            InitializeComponent();
        }

        private void frmAdminMain_Load(object? sender, EventArgs e)
        {
            lblXinChao.Text = $"Xin chào, {UserSession.DisplayName}";
            lblTenDangNhap.Text = $"Tên đăng nhập: {UserSession.Username}";
        }

        private void btnDangXuat_Click(object? sender, EventArgs e)
        {
            UserSession.Logout(this);
        }
    }
}
