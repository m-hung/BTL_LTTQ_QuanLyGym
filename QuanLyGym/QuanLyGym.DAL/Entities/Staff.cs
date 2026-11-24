using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class Staff
    {
        [Key]
        public int StaffID { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = string.Empty; // PT / Tạp vụ / Thu ngân / Bảo vệ

        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        [StringLength(15)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(100)]
        public string WorkShift { get; set; } = string.Empty; // Ca làm việc cố định

        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseSalary { get; set; } // Lương cơ bản

        public bool Status { get; set; } = true;

        public int? BranchID { get; set; }
        [ForeignKey("BranchID")]
        public Branch? Branch { get; set; }

        public int? PTID { get; set; }
        [ForeignKey("PTID")]
        public PT? PT { get; set; }
    }
}
