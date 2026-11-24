using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using QuanLyGym.DAL;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.BLL.Services
{
    public enum ResetPasswordStatus
    {
        Success,
        InvalidOrExpired,   // Mã sai, hết hạn, hoặc không có yêu cầu nào (cố tình không phân biệt)
        TooManyAttempts,    // Nhập sai quá số lần cho phép -> mã bị hủy
        WeakPassword        // Mật khẩu mới quá ngắn
    }

    public class ResetPasswordResult
    {
        public ResetPasswordStatus Status { get; set; }
        public string? Username { get; set; }
    }

    /// Nghiệp vụ quên mật khẩu / quên tên đăng nhập, định danh tài khoản bằng email (email là duy nhất).
    public class PasswordResetService
    {
        public const int CodeLifetimeMinutes = 5;       // Mã có hiệu lực 5 phút
        public const int ResendCooldownSeconds = 60;    // Giãn cách giữa 2 lần gửi
        public const int MaxFailedAttempts = 5;         // Nhập sai tối đa 5 lần / mã
        public const int MaxCodesPerHour = 5;           // Tối đa 5 mã / giờ / tài khoản (chống dò mã)
        public const int MinPasswordLength = 6;

        // Giới hạn tần suất gửi "tên đăng nhập" (lưu trong bộ nhớ)
        private static readonly ConcurrentDictionary<string, DateTime> _usernameThrottle = new();

        private readonly EmailService _email;

        public PasswordResetService() : this(new EmailService()) { }

        public PasswordResetService(EmailService email)
        {
            _email = email;
        }

        private static string NormalizeEmail(string? email) => (email ?? "").Trim().ToLowerInvariant();

        // ----------------------------------------------------------------
        // 1. Yêu cầu mã xác nhận
        // ----------------------------------------------------------------

        /// Tạo mã OTP và gửi tới email.
        /// email lạ / tài khoản bị khóa / gửi quá nhanh đều trả về bình thường (không gửi gì).
        /// Chỉ ném lỗi khi hệ thống chưa cấu hình mail hoặc gửi mail thất bại.
        public async Task RequestResetCodeAsync(string email)
        {
            email = NormalizeEmail(email);
            _email.EnsureConfigured();

            using var db = new AppDbContext();

            var account = await db.Accounts
                .FirstOrDefaultAsync(a => a.Email != null && a.Email.ToLower() == email && a.Status);
            if (account == null) return;

            var now = DateTime.Now;

            // Chống spam: mã đang còn hiệu lực vừa gửi cách đây chưa tới 60 giây -> bỏ qua
            var latest = await db.PasswordResets
                .Where(r => r.AccountID == account.AccountID)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync();
            if (latest != null && !latest.IsUsed && (now - latest.CreatedAt).TotalSeconds < ResendCooldownSeconds)
                return;

            // Chống dò mã: tối đa 5 mã / giờ
            var since = now.AddHours(-1);
            int recent = await db.PasswordResets.CountAsync(r => r.AccountID == account.AccountID && r.CreatedAt > since);
            if (recent >= MaxCodesPerHour) return;

            // Vô hiệu hóa các mã cũ
            var oldCodes = await db.PasswordResets
                .Where(r => r.AccountID == account.AccountID && !r.IsUsed)
                .ToListAsync();
            foreach (var old in oldCodes) old.IsUsed = true;

            string code = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
            var reset = new PasswordReset
            {
                AccountID = account.AccountID,
                CodeHash = HashCode(code),
                CreatedAt = now,
                ExpiresAt = now.AddMinutes(CodeLifetimeMinutes)
            };
            db.PasswordResets.Add(reset);
            await db.SaveChangesAsync();

            try
            {
                await _email.SendResetCodeAsync(account.Email!, code, CodeLifetimeMinutes);
            }
            catch
            {
                // Gửi mail thất bại -> xóa mã vừa tạo để người dùng thử lại ngay được
                db.PasswordResets.Remove(reset);
                await db.SaveChangesAsync();
                throw;
            }
        }

        // ----------------------------------------------------------------
        // 2. Xác nhận mã + đặt mật khẩu mới
        // ----------------------------------------------------------------

        public ResetPasswordResult ResetPassword(string email, string code, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword) || newPassword.Length < MinPasswordLength)
                return new ResetPasswordResult { Status = ResetPasswordStatus.WeakPassword };

            email = NormalizeEmail(email);
            code = (code ?? "").Trim();

            using var db = new AppDbContext();

            var account = db.Accounts
                .FirstOrDefault(a => a.Email != null && a.Email.ToLower() == email && a.Status);
            if (account == null)
                return new ResetPasswordResult { Status = ResetPasswordStatus.InvalidOrExpired };

            var now = DateTime.Now;
            var reset = db.PasswordResets
                .Where(r => r.AccountID == account.AccountID && !r.IsUsed && r.ExpiresAt > now)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefault();
            if (reset == null)
                return new ResetPasswordResult { Status = ResetPasswordStatus.InvalidOrExpired };

            if (!VerifyCode(code, reset.CodeHash))
            {
                reset.FailedAttempts++;
                if (reset.FailedAttempts >= MaxFailedAttempts)
                {
                    reset.IsUsed = true; // Khóa mã này, buộc phải xin mã mới
                    db.SaveChanges();
                    return new ResetPasswordResult { Status = ResetPasswordStatus.TooManyAttempts };
                }

                db.SaveChanges();
                return new ResetPasswordResult { Status = ResetPasswordStatus.InvalidOrExpired };
            }

            // Mã đúng -> đổi mật khẩu
            account.PasswordHash = PasswordHasher.Hash(newPassword);

            // Mã đã dùng xong, hủy luôn các mã còn lại của tài khoản
            reset.IsUsed = true;
            var others = db.PasswordResets.Where(r => r.AccountID == account.AccountID && !r.IsUsed).ToList();
            foreach (var o in others) o.IsUsed = true;

            db.SaveChanges();

            return new ResetPasswordResult { Status = ResetPasswordStatus.Success, Username = account.Username };
        }

        // ----------------------------------------------------------------
        // 3. Quên tên đăng nhập
        // ----------------------------------------------------------------

        /// Gửi tên đăng nhập tới email. Không bao giờ hiển thị tên đăng nhập trên màn hình
        /// và cũng không báo email có tồn tại hay không.
        public async Task SendUsernameAsync(string email)
        {
            email = NormalizeEmail(email);
            _email.EnsureConfigured();

            if (_usernameThrottle.TryGetValue(email, out var last) &&
                (DateTime.Now - last).TotalSeconds < ResendCooldownSeconds)
                return;

            using var db = new AppDbContext();
            var account = await db.Accounts.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Email != null && a.Email.ToLower() == email && a.Status);
            if (account == null) return;

            _usernameThrottle[email] = DateTime.Now;
            try
            {
                await _email.SendUsernameAsync(account.Email!, account.Username);
            }
            catch
            {
                _usernameThrottle.TryRemove(email, out _);
                throw;
            }
        }

        // ----------------------------------------------------------------
        // Băm / kiểm tra mã OTP ("salt:hash", SHA-256)
        // ----------------------------------------------------------------

        private static string HashCode(string code)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(ComputeHash(salt, code));
        }

        private static bool VerifyCode(string code, string stored)
        {
            var parts = stored.Split(':');
            if (parts.Length != 2) return false;

            try
            {
                byte[] salt = Convert.FromBase64String(parts[0]);
                byte[] expected = Convert.FromBase64String(parts[1]);
                return CryptographicOperations.FixedTimeEquals(ComputeHash(salt, code), expected);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static byte[] ComputeHash(byte[] salt, string code)
        {
            byte[] codeBytes = Encoding.UTF8.GetBytes(code);
            byte[] data = new byte[salt.Length + codeBytes.Length];
            Buffer.BlockCopy(salt, 0, data, 0, salt.Length);
            Buffer.BlockCopy(codeBytes, 0, data, salt.Length, codeBytes.Length);
            return SHA256.HashData(data);
        }
    }
}
