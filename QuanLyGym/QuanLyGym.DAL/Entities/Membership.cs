using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class Membership
    {
        [Key]
        public int MembershipID { get; set; }

        public int MemberID { get; set; }
        [ForeignKey("MemberID")]
        public Member? Member { get; set; }

        public int PackageID { get; set; }
        [ForeignKey("PackageID")]
        public Package? Package { get; set; }

        public DateTime RegisterDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int UsedSessions { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Active";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? BranchID { get; set; }
        [ForeignKey("BranchID")]
        public Branch? Branch { get; set; }
    }
}