using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class ClassEnrollmentConfiguration : IEntityTypeConfiguration<ClassEnrollment>
    {
        public void Configure(EntityTypeBuilder<ClassEnrollment> builder)
        {
            builder.HasData(
                // =========================================================================
                // Điểm danh / Đăng ký các lớp học nhóm năm 2026
                // Thuộc tính AttendanceStatus CHỈ CÓ: Attended, Absent, Cancelled
                // =========================================================================

                // Tháng 1 & 2/2026 - Chi nhánh 1
                new ClassEnrollment { EnrollmentID = 1, ClassID = 1, MemberID = 2, EnrollmentDate = new DateTime(2026, 1, 12, 6, 30, 0), AttendanceStatus = "Attended", Note = "Hoàn thành tốt các tư thế Asana chào mặt trời" },
                new ClassEnrollment { EnrollmentID = 2, ClassID = 1, MemberID = 3, EnrollmentDate = new DateTime(2026, 1, 14, 6, 30, 0), AttendanceStatus = "Attended", Note = "Tập trung nhịp thở sâu rất tốt" },
                new ClassEnrollment { EnrollmentID = 3, ClassID = 2, MemberID = 1, EnrollmentDate = new DateTime(2026, 1, 19, 18, 0, 0), AttendanceStatus = "Attended", Note = "Rèn luyện thể lực đấm bao cát cùng HLV Nam" },
                new ClassEnrollment { EnrollmentID = 4, ClassID = 2, MemberID = 4, EnrollmentDate = new DateTime(2026, 1, 21, 18, 0, 0), AttendanceStatus = "Absent", Note = "Hội viên bận công việc đột xuất chưa đến lớp" },
                new ClassEnrollment { EnrollmentID = 5, ClassID = 3, MemberID = 3, EnrollmentDate = new DateTime(2026, 2, 3, 18, 30, 0), AttendanceStatus = "Attended", Note = "Theo kịp các bước nhảy Latin vui vẻ" },
                new ClassEnrollment { EnrollmentID = 6, ClassID = 4, MemberID = 5, EnrollmentDate = new DateTime(2026, 2, 10, 7, 0, 0), AttendanceStatus = "Attended", Note = "Bài tập cơ bụng thon gọn cùng HLV Thảo" },

                // Tháng 3 & 4/2026 - Chi nhánh 2
                new ClassEnrollment { EnrollmentID = 7, ClassID = 5, MemberID = 7, EnrollmentDate = new DateTime(2026, 3, 9, 6, 30, 0), AttendanceStatus = "Attended", Note = "Dẻo dai khớp vai và cột sống" },
                new ClassEnrollment { EnrollmentID = 8, ClassID = 6, MemberID = 6, EnrollmentDate = new DateTime(2026, 3, 16, 18, 0, 0), AttendanceStatus = "Attended", Note = "Lực đấm và bước di chuyển tiến bộ rõ rệt" },
                new ClassEnrollment { EnrollmentID = 9, ClassID = 7, MemberID = 8, EnrollmentDate = new DateTime(2026, 3, 24, 18, 30, 0), AttendanceStatus = "Cancelled", Note = "Hội viên xin hủy lịch trước 1 ngày do đi công tác" },
                new ClassEnrollment { EnrollmentID = 10, ClassID = 8, MemberID = 7, EnrollmentDate = new DateTime(2026, 4, 7, 7, 0, 0), AttendanceStatus = "Attended", Note = "Chỉnh sửa tư thế vai gù hiệu quả" },

                // Tháng 5 & 6/2026 - Chi nhánh 3
                new ClassEnrollment { EnrollmentID = 11, ClassID = 9, MemberID = 10, EnrollmentDate = new DateTime(2026, 5, 11, 6, 30, 0), AttendanceStatus = "Attended", Note = "Học viên nắm vững chuỗi bài vinyasa" },
                new ClassEnrollment { EnrollmentID = 12, ClassID = 10, MemberID = 9, EnrollmentDate = new DateTime(2026, 5, 20, 18, 0, 0), AttendanceStatus = "Attended", Note = "Tập cường độ cao với HLV Hoàng Long" },
                new ClassEnrollment { EnrollmentID = 13, ClassID = 11, MemberID = 10, EnrollmentDate = new DateTime(2026, 6, 9, 18, 30, 0), AttendanceStatus = "Absent", Note = "Nghỉ không phép buổi tập tối" },
                new ClassEnrollment { EnrollmentID = 14, ClassID = 12, MemberID = 9, EnrollmentDate = new DateTime(2026, 6, 18, 7, 0, 0), AttendanceStatus = "Attended", Note = "Tập trung siết cơ mông đùi" },

                // Tháng 7 & 8/2026 - Các chi nhánh
                new ClassEnrollment { EnrollmentID = 15, ClassID = 1, MemberID = 2, EnrollmentDate = new DateTime(2026, 7, 6, 6, 30, 0), AttendanceStatus = "Attended", Note = "Độ dẻo dai cơ gân kheo cải thiện" },
                new ClassEnrollment { EnrollmentID = 16, ClassID = 2, MemberID = 1, EnrollmentDate = new DateTime(2026, 7, 15, 18, 0, 0), AttendanceStatus = "Attended", Note = "Tập đấm đá phản xạ chân mông" },
                new ClassEnrollment { EnrollmentID = 17, ClassID = 3, MemberID = 5, EnrollmentDate = new DateTime(2026, 8, 4, 18, 30, 0), AttendanceStatus = "Cancelled", Note = "Hủy lịch vì lý do sức khỏe cá nhân" },
                new ClassEnrollment { EnrollmentID = 18, ClassID = 6, MemberID = 6, EnrollmentDate = new DateTime(2026, 8, 17, 18, 0, 0), AttendanceStatus = "Attended", Note = "Buổi tập Kickfit cường độ tối đa" },

                // Tháng 9 & 10/2026 - Mùa tập cao điểm
                new ClassEnrollment { EnrollmentID = 19, ClassID = 4, MemberID = 3, EnrollmentDate = new DateTime(2026, 9, 8, 7, 0, 0), AttendanceStatus = "Attended", Note = "Cải thiện sức bền cơ bụng rãnh 11" },
                new ClassEnrollment { EnrollmentID = 20, ClassID = 5, MemberID = 8, EnrollmentDate = new DateTime(2026, 9, 21, 6, 30, 0), AttendanceStatus = "Attended", Note = "Thiền định tĩnh tâm cuối buổi tập" },
                new ClassEnrollment { EnrollmentID = 21, ClassID = 7, MemberID = 7, EnrollmentDate = new DateTime(2026, 10, 6, 18, 30, 0), AttendanceStatus = "Attended", Note = "Không khí lớp học sôi nổi nhiệt huyết" },
                new ClassEnrollment { EnrollmentID = 22, ClassID = 10, MemberID = 9, EnrollmentDate = new DateTime(2026, 10, 19, 18, 0, 0), AttendanceStatus = "Absent", Note = "Vắng mặt buổi tập đối kháng" },

                // Tháng 11/2026 - Hiện tại
                new ClassEnrollment { EnrollmentID = 23, ClassID = 1, MemberID = 3, EnrollmentDate = new DateTime(2026, 11, 9, 6, 30, 0), AttendanceStatus = "Attended", Note = "Tập thở Pranayama nâng cao" },
                new ClassEnrollment { EnrollmentID = 24, ClassID = 2, MemberID = 1, EnrollmentDate = new DateTime(2026, 11, 16, 18, 0, 0), AttendanceStatus = "Attended", Note = "Kiểm tra kỹ thuật gạt đòn và phản công" },
                new ClassEnrollment { EnrollmentID = 25, ClassID = 3, MemberID = 2, EnrollmentDate = new DateTime(2026, 11, 24, 18, 30, 0), AttendanceStatus = "Attended", Note = "Hội viên tham gia đầy đủ nhiệt tình" }
            );
        }
    }
}
