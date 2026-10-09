using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class WorkingSchedule
    {
        [Key]
        public int WorkingScheduleID { get; set; }

        public int PTID { get; set; }
        [ForeignKey("PTID")]
        public PT? PT { get; set; }

        public DateTime WorkDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        public int? BranchID { get; set; }
        [ForeignKey("BranchID")]
        public Branch? Branch { get; set; }
    }
}