using Microsoft.EntityFrameworkCore;
using QuanLyGym.DAL;

namespace QuanLyGym.BLL.Services
{
    public enum LoginStatus
    {
        Success,
        InvalidCredentials,  // Sai tên đăng nhập hoặc mật khẩu
        Locked               // Tài khoản bị khóa (Status = false)
    }

    public class LoginResult
    {
        public LoginStatus Status { get; set; }
        public int AccountID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;     // Admin / PT / Member
        public string? FullName { get; set; }                // Lấy từ bảng PTs / Members (Admin không có)
    }

    /// <summary>
    /// Xử lý nghiệp vụ đăng nhập.
    /// </summary>
    public class AuthService
    {
        public LoginResult Login(string username, string password)
        {
            using var db = new AppDbContext();

            // Lưu ý: hiện tại mật khẩu trong DB đang lưu dạng chữ thường ("123456")
            var account = db.Accounts
                .AsNoTracking()
                .FirstOrDefault(a => a.Username == username && a.PasswordHash == password);

            if (account == null)
                return new LoginResult { Status = LoginStatus.InvalidCredentials };

            if (!account.Status)
                return new LoginResult { Status = LoginStatus.Locked };

            string? fullName = account.Role switch
            {
                "PT" => db.PTs.Where(p => p.AccountID == account.AccountID)
                              .Select(p => p.FullName).FirstOrDefault(),
                "Member" => db.Members.Where(m => m.AccountID == account.AccountID)
                                      .Select(m => m.FullName).FirstOrDefault(),
                _ => null
            };

            return new LoginResult
            {
                Status = LoginStatus.Success,
                AccountID = account.AccountID,
                Username = account.Username,
                Role = account.Role,
                FullName = fullName
            };
        }
    }
}
