using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class Progress
    {
        [Key]
        public int ProgressID { get; set; }

        public int MemberID { get; set; }
        [ForeignKey("MemberID")]
        public Member? Member { get; set; }

        public int PTID { get; set; }
        [ForeignKey("PTID")]
        public PT? PT { get; set; }

        public DateTime RecordDate { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Height { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Weight { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal BodyFat { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal MuscleMass { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal BMI { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Chest { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Waist { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Hip { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Arm { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Thigh { get; set; }

        [StringLength(500)]
        public string Note { get; set; } = string.Empty;
    }
}