using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyGym.DAL.Entities
{
    public class ClassEnrollment
    {
        [Key]
        public int EnrollmentID { get; set; }

        public int ClassID { get; set; }
        [ForeignKey("ClassID")]
        public Class? Class { get; set; }

        public int MemberID { get; set; }
        [ForeignKey("MemberID")]
        public Member? Member { get; set; }

        public DateTime EnrollmentDate { get; set; }

        [Required]
        [StringLength(20)]
        public string AttendanceStatus { get; set; } = "Attended"; // Attended / Absent / Cancelled

        [StringLength(255)]
        public string Note { get; set; } = string.Empty;
    }
}
