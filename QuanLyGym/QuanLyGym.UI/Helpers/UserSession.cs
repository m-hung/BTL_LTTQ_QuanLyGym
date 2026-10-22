namespace QuanLyGym.UI.Helpers
{
    /// <summary>
    /// Lưu thông tin tài khoản đang đăng nhập, dùng chung cho toàn bộ UI.
    /// </summary>
    public static class UserSession
    {
        public const string ROLE_ADMIN = "Admin";
        public const string ROLE_PT = "PT";
        public const string ROLE_MEMBER = "Member";

        public static int AccountID { get; set; }
        public static string Username { get; set; } = string.Empty;
        public static string Role { get; set; } = string.Empty;
        public static string? FullName { get; set; }

        /// <summary>Tên hiển thị: ưu tiên họ tên, không có thì dùng tên đăng nhập.</summary>
        public static string DisplayName => string.IsNullOrWhiteSpace(FullName) ? Username : FullName;

        /// <summary>true khi người dùng bấm "Đăng xuất" -> Program.cs sẽ mở lại form đăng nhập.</summary>
        public static bool IsLoggingOut { get; set; }

        public static void Clear()
        {
            AccountID = 0;
            Username = string.Empty;
            Role = string.Empty;
            FullName = null;
            IsLoggingOut = false;
        }

        /// <summary>Hỏi xác nhận và đóng form để đăng xuất.</summary>
        public static void Logout(Form currentForm)
        {
            var result = MessageBox.Show(currentForm, "Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            IsLoggingOut = true;
            currentForm.Close();
        }
    }
}
