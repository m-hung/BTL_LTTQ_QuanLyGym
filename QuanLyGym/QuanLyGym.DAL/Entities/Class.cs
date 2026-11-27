using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class Class
    {
        [Key]
        public int ClassID { get; set; }

        [Required]
        [StringLength(100)]
        public string ClassName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string ClassType { get; set; } = string.Empty; // Yoga, Boxing, Zumba, Pilates

        public int BranchID { get; set; }
        [ForeignKey("BranchID")]
        public Branch? Branch { get; set; }

        public int RoomID { get; set; }
        [ForeignKey("RoomID")]
        public Room? Room { get; set; }

        public int PTID { get; set; }
        [ForeignKey("PTID")]
        public PT? PT { get; set; }

        [StringLength(50)]
        public string DayOfWeek { get; set; } = string.Empty; // e.g. "Thứ 2, 4, 6"

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public int MaxCapacity { get; set; } = 25;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool Status { get; set; } = true;
    }
}
