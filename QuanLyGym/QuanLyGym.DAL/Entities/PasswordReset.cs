using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    /// Mã xác nhận (OTP) dùng để đặt lại mật khẩu. Chỉ lưu bản băm của mã, không lưu mã gốc.
    public class PasswordReset
    {
        [Key]
        public int PasswordResetID { get; set; }

        public int AccountID { get; set; }
        [ForeignKey("AccountID")]
        public Account? Account { get; set; }

        /// Định dạng "salt:hash" (Base64).
        [Required]
        [StringLength(255)]
        public string CodeHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiresAt { get; set; }

        /// Số lần nhập sai mã này.
        public int FailedAttempts { get; set; }

        /// true khi mã đã dùng, đã bị thay bằng mã mới, hoặc bị khóa do nhập sai quá nhiều.
        public bool IsUsed { get; set; }
    }
}
