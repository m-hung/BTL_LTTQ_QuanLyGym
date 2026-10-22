using QuanLyGym.UI.Forms.Admin;
using QuanLyGym.UI.Forms.Common;
using QuanLyGym.UI.Forms.Member;
using QuanLyGym.UI.Forms.PT;
using QuanLyGym.UI.Helpers;

using QuanLyGym.UI.Forms.Chat;
namespace QuanLyGym.UI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Đăng nhập -> mở form theo vai trò -> bấm Đăng xuất thì quay lại form đăng nhập
            while (true)
            {
                using (var login = new frmLogin())
                {
                    if (login.ShowDialog() != DialogResult.OK)
                        return; // Đóng form đăng nhập -> thoát ứng dụng
                }

                Form? mainForm = UserSession.Role switch
                {
                    UserSession.ROLE_ADMIN => new frmAdminMain(),
                    UserSession.ROLE_PT => new frmPTMain(),
                    UserSession.ROLE_MEMBER => new frmMemberMain(),
                    _ => null
                };

                if (mainForm == null)
                {
                    MessageBox.Show($"Vai trò \"{UserSession.Role}\" không hợp lệ.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UserSession.Clear();
                    continue;
                }

                Application.Run(mainForm);

                // Đóng form bằng nút X (không phải Đăng xuất) -> thoát ứng dụng
                if (!UserSession.IsLoggingOut) return;
                UserSession.Clear();
            }
        }
    }
}
