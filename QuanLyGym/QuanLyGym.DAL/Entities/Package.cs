using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class Package
    {
        [Key]
        public int PackageID { get; set; }

        [Required]
        [StringLength(100)]
        public string PackageName { get; set; } = string.Empty;

        public int Duration { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int TotalSessions { get; set; }

        [StringLength(500)]
        public string Benefits { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool Status { get; set; } = true;

        public int? BranchID { get; set; }
        [ForeignKey("BranchID")]
        public Branch? Branch { get; set; }
    }
}