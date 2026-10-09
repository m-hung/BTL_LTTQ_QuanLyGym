using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class EquipmentMaintenance
    {
        [Key]
        public int MaintenanceID { get; set; }

        public int EquipmentID { get; set; }
        [ForeignKey("EquipmentID")]
        public Equipment? Equipment { get; set; }

        public DateTime MaintenanceDate { get; set; }
        public DateTime NextMaintenanceDate { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Cost { get; set; }

        [StringLength(30)]
        public string Status { get; set; } = string.Empty;
    }
}