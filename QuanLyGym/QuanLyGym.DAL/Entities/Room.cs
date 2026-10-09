using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }

        [Required]
        [StringLength(100)]
        public string RoomName { get; set; } = string.Empty;

        [StringLength(50)]
        public string RoomType { get; set; } = string.Empty;

        public int Capacity { get; set; }

        [StringLength(255)]
        public string Description { get; set; } = string.Empty;

        public bool Status { get; set; } = true;

        public int? BranchID { get; set; }
        [ForeignKey("BranchID")]
        public Branch? Branch { get; set; }
    }
}