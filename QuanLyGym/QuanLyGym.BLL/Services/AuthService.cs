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
        public string? FullName { get; set; }               
    }

    /// Xử lý nghiệp vụ đăng nhập.
    public class AuthService
    {
        // Chuỗi băm giả để tốn thời gian tương đương khi tên đăng nhập không tồn tại
        private static readonly string DummyHash = PasswordHasher.Hash("dummy-password");

        private static bool ConstantTimeEquals(string a, string b) =>
            System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(
                System.Text.Encoding.UTF8.GetBytes(a), System.Text.Encoding.UTF8.GetBytes(b));

        public LoginResult Login(string username, string password)
        {
            using var db = new AppDbContext();

            // Tìm theo tên đăng nhập rồi mới so mật khẩu (mật khẩu được lưu dạng băm)
            var account = db.Accounts.FirstOrDefault(a => a.Username == username);

            if (account == null)
            {
                // Vẫn tốn thời gian băm như bình thường để không lộ tên đăng nhập nào tồn tại
                PasswordHasher.Verify(password, DummyHash);
                return new LoginResult { Status = LoginStatus.InvalidCredentials };
            }

            if (PasswordHasher.IsHashed(account.PasswordHash))
            {
                if (!PasswordHasher.Verify(password, account.PasswordHash))
                    return new LoginResult { Status = LoginStatus.InvalidCredentials };

                // Nâng cấp dần nếu chuỗi băm cũ dùng ít vòng lặp hơn
                if (PasswordHasher.NeedsRehash(account.PasswordHash))
                {
                    account.PasswordHash = PasswordHasher.Hash(password);
                    db.SaveChanges();
                }
            }
            else
            {
                // Dữ liệu cũ còn lưu mật khẩu thường: so khớp rồi tự chuyển sang dạng băm
                if (!ConstantTimeEquals(password, account.PasswordHash))
                    return new LoginResult { Status = LoginStatus.InvalidCredentials };

                account.PasswordHash = PasswordHasher.Hash(password);
                db.SaveChanges();
            }

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

        /// Kiểm tra email đã thuộc tài khoản nào chưa (dùng khi Admin tạo/sửa tài khoản).
        /// excludeAccountId: bỏ qua chính tài khoản đang được sửa.
        public bool IsEmailInUse(string email, int? excludeAccountId = null)
        {
            string e = (email ?? "").Trim().ToLowerInvariant();
            if (e == "") return false;

            using var db = new AppDbContext();
            return db.Accounts.Any(a => a.Email != null && a.Email.ToLower() == e
                                        && (excludeAccountId == null || a.AccountID != excludeAccountId));
        }
    }
}
