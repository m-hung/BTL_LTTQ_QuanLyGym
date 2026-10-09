using System.ComponentModel.DataAnnotations;

namespace QuanLyGym.DAL.Entities
{
    public class Branch
    {
        [Key]
        public int BranchID { get; set; }

        [Required]
        [StringLength(100)]
        public string BranchName { get; set; } = string.Empty;

        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        [StringLength(15)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public bool Status { get; set; } = true;
    }
}