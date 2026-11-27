using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        public int? AccountID { get; set; }
        [ForeignKey("AccountID")]
        public Account? Account { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Content { get; set; } = string.Empty;

        [StringLength(50)]
        public string NotificationType { get; set; } = "System"; // System, Reminder, Promotion, Maintenance

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? BranchID { get; set; }
        [ForeignKey("BranchID")]
        public Branch? Branch { get; set; }
    }
}
