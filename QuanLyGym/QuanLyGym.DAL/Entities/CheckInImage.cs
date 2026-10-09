using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class CheckInImage
    {
        [Key]
        public int ImageID { get; set; }

        public int CheckInID { get; set; }
        [ForeignKey("CheckInID")]
        public CheckIn? CheckIn { get; set; }

        [StringLength(500)]
        public string ImagePath { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.Now;

        public int? VerifiedBy { get; set; }
        [ForeignKey("VerifiedBy")]
        public PT? VerifiedByPT { get; set; }

        [StringLength(255)]
        public string VerificationNote { get; set; } = string.Empty;
    }
}