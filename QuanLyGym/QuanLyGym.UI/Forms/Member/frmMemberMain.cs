using QuanLyGym.UI.Helpers;

namespace QuanLyGym.UI.Forms.Member
{
    public partial class frmMemberMain : Form
    {
        public frmMemberMain()
        {
            InitializeComponent();
        }

        private void frmMemberMain_Load(object? sender, EventArgs e)
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
