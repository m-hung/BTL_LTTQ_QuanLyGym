using QuanLyGym.UI.Helpers;

namespace QuanLyGym.UI.Forms.PT
{
    public partial class frmPTMain : Form
    {
        public frmPTMain()
        {
            InitializeComponent();
        }

        private void frmPTMain_Load(object? sender, EventArgs e)
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
