using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class Incident
    {
        [Key]
        public int IncidentID { get; set; }

        public int EquipmentID { get; set; }
        [ForeignKey("EquipmentID")]
        public Equipment? Equipment { get; set; }

        public int ReportedBy { get; set; }
        [ForeignKey("ReportedBy")]
        public Account? Account { get; set; }

        public DateTime ReportDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(30)]
        public string Status { get; set; } = "New";

        public DateTime? ResolvedDate { get; set; }

        [StringLength(500)]
        public string Note { get; set; } = string.Empty;
    }
}