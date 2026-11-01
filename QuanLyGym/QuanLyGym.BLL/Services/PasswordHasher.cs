using System.Security.Cryptography;

namespace QuanLyGym.BLL.Services
{
    /// <summary>
    /// Băm mật khẩu bằng PBKDF2-SHA256 (có sẵn trong .NET, không cần thư viện ngoài).
    /// Chuỗi lưu trong DB có dạng:  PBKDF2$số_vòng_lặp$salt(base64)$hash(base64)
    /// Mỗi lần băm dùng một salt ngẫu nhiên nên cùng một mật khẩu cho ra chuỗi khác nhau.
    /// </summary>
    public static class PasswordHasher
    {
        private const string Prefix = "PBKDF2";
        public const int Iterations = 100_000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public static string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
            return $"{Prefix}${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        /// <summary>true nếu chuỗi đã ở dạng băm (khác với mật khẩu thường của dữ liệu cũ).</summary>
        public static bool IsHashed(string? stored) =>
            !string.IsNullOrEmpty(stored) && stored.StartsWith(Prefix + "$", StringComparison.Ordinal);

        public static bool Verify(string password, string? stored)
        {
            if (!IsHashed(stored)) return false;

            string[] parts = stored!.Split('$');
            if (parts.Length != 4 || !int.TryParse(parts[1], out int iterations) || iterations <= 0)
                return false;

            try
            {
                byte[] salt = Convert.FromBase64String(parts[2]);
                byte[] expected = Convert.FromBase64String(parts[3]);
                byte[] actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
                return CryptographicOperations.FixedTimeEquals(actual, expected);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>true nếu chuỗi băm dùng số vòng lặp thấp hơn hiện tại (nên băm lại khi người dùng đăng nhập).</summary>
        public static bool NeedsRehash(string stored)
        {
            string[] parts = stored.Split('$');
            return parts.Length != 4 || !int.TryParse(parts[1], out int it) || it < Iterations;
        }
    }
}
