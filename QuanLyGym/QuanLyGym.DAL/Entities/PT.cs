using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class PT
    {
        [Key]
        public int PTID { get; set; }

        public int AccountID { get; set; }
        [ForeignKey("AccountID")]
        public Account? Account { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        [StringLength(15)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(255)]
        public string Specialization { get; set; } = string.Empty;

        public int Experience { get; set; }
        public bool Status { get; set; } = true;

        public int? BranchID { get; set; }
        [ForeignKey("BranchID")]
        public Branch? Branch { get; set; }
    }
}