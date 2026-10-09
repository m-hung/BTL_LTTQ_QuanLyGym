using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class CheckIn
    {
        [Key]
        public int CheckInID { get; set; }

        public int MemberID { get; set; }
        [ForeignKey("MemberID")]
        public Member? Member { get; set; }

        public int? PTID { get; set; }
        [ForeignKey("PTID")]
        public PT? PT { get; set; }

        public DateTime CheckInTime { get; set; } = DateTime.Now;

        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        [StringLength(255)]
        public string Note { get; set; } = string.Empty;

        public int? BranchID { get; set; }
        [ForeignKey("BranchID")]
        public Branch? Branch { get; set; }
    }
}