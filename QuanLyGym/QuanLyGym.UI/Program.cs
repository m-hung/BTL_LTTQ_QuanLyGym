using QuanLyGym.UI.Forms.Common;
using QuanLyGym.UI.Forms.PT;

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
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            // Đang mở trực tiếp giao diện PT để test (khi cần login thì đổi lại frmLogin)
            Application.Run(new frmPTMain());
        }
    }
}