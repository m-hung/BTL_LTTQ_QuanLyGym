using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class Schedule
    {
        [Key]
        public int ScheduleID { get; set; }

        public int MemberID { get; set; }
        [ForeignKey("MemberID")]
        public Member? Member { get; set; }

        public int PTID { get; set; }
        [ForeignKey("PTID")]
        public PT? PT { get; set; }

        public int RoomID { get; set; }
        [ForeignKey("RoomID")]
        public Room? Room { get; set; }

        public DateTime TrainingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        [StringLength(100)]
        public string TrainingType { get; set; } = string.Empty;

        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        [StringLength(500)]
        public string Note { get; set; } = string.Empty;

        public int? BranchID { get; set; }
        [ForeignKey("BranchID")]
        public Branch? Branch { get; set; }
    }
}