using System.ComponentModel.DataAnnotations;

namespace QuanLyGym.DAL.Entities
{
    public class Account
    {
        [Key]
        public int AccountID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(20)]
        public string Role { get; set; } = "Member"; // Admin / Member / PT

        public bool Status { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}