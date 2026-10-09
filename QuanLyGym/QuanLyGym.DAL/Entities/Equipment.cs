using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class Equipment
    {
        [Key]
        public int EquipmentID { get; set; }

        public int RoomID { get; set; }
        [ForeignKey("RoomID")]
        public Room? Room { get; set; }

        [Required]
        [StringLength(100)]
        public string EquipmentName { get; set; } = string.Empty;

        [StringLength(50)]
        public string EquipmentCode { get; set; } = string.Empty;

        public DateTime PurchaseDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [StringLength(30)]
        public string Status { get; set; } = "Tốt";

        [StringLength(255)]
        public string Description { get; set; } = string.Empty;
    }
}