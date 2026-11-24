using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class IncidentConfiguration : IEntityTypeConfiguration<Incident>
    {
        public void Configure(EntityTypeBuilder<Incident> builder)
        {
            builder.HasData(
                new Incident { IncidentID = 1, EquipmentID = 1, ReportedBy = 1, ReportDate = new DateTime(2026, 2, 10), Description = "Máy chạy bộ phát ra tiếng kêu lạ ở băng tải khi đạt tốc độ trên 12km/h", Status = "Resolved", ResolvedDate = new DateTime(2026, 2, 11), Note = "Đã siết lại ốc và căn chỉnh tâm băng chuyền" },
                new Incident { IncidentID = 2, EquipmentID = 3, ReportedBy = 2, ReportDate = new DateTime(2026, 3, 12), Description = "Đệm da ghế đẩy ngực bị rách nhẹ ở góc mép", Status = "Resolved", ResolvedDate = new DateTime(2026, 3, 14), Note = "Đã bọc lại lớp da simili cao cấp mới" },
                new Incident { IncidentID = 3, EquipmentID = 4, ReportedBy = 2, ReportDate = new DateTime(2026, 4, 5), Description = "Cáp kéo xô có hiện tượng sờn vỏ nhựa bảo vệ", Status = "Resolved", ResolvedDate = new DateTime(2026, 4, 6), Note = "Đã thay cáp mới an toàn tuyệt đối" },
                new Incident { IncidentID = 4, EquipmentID = 5, ReportedBy = 3, ReportDate = new DateTime(2026, 5, 20), Description = "Màn hình xe đạp không nhận tín hiệu nhịp tim từ tay cầm", Status = "Resolved", ResolvedDate = new DateTime(2026, 5, 21), Note = "Đã nối lại dây cảm biến bị tuột bên trong" },
                new Incident { IncidentID = 5, EquipmentID = 6, ReportedBy = 4, ReportDate = new DateTime(2026, 6, 18), Description = "Khóa chốt an toàn máy Leg Press bị rít khó gạt", Status = "Resolved", ResolvedDate = new DateTime(2026, 6, 19), Note = "Đã tra dầu bôi trơn và vệ sinh trục chốt" },
                new Incident { IncidentID = 6, EquipmentID = 7, ReportedBy = 4, ReportDate = new DateTime(2026, 7, 22), Description = "Ròng rọc dàn cáp đôi xoay không đều gây cọ xát", Status = "Resolved", ResolvedDate = new DateTime(2026, 7, 23), Note = "Đã thay vòng bi ròng rọc bên trái" },
                new Incident { IncidentID = 7, EquipmentID = 8, ReportedBy = 5, ReportDate = new DateTime(2026, 8, 30), Description = "Bàn đạp máy trượt tuyết phát ra tiếng cót két khi đạp nhanh", Status = "Resolved", ResolvedDate = new DateTime(2026, 8, 31), Note = "Đã siết ốc trục cam và tra mỡ chịu nhiệt" },
                new Incident { IncidentID = 8, EquipmentID = 9, ReportedBy = 1, ReportDate = new DateTime(2026, 9, 14), Description = "Quả tạ 15kg bị lỏng ốc đầu ren cao su", Status = "Resolved", ResolvedDate = new DateTime(2026, 9, 15), Note = "Đã dùng keo khóa ren vặn chặt cố định" },
                new Incident { IncidentID = 9, EquipmentID = 7, ReportedBy = 4, ReportDate = new DateTime(2026, 10, 22), Description = "Chốt chọn mức tạ bị cong nhẹ do hội viên thả tạ rơi tự do", Status = "In Progress", ResolvedDate = null, Note = "Đang chờ nhà cung cấp giao chốt tạ mới thay thế" },
                new Incident { IncidentID = 10, EquipmentID = 10, ReportedBy = 6, ReportDate = new DateTime(2026, 10, 25), Description = "Dây kéo tay cầm máy chèo thuyền bị xoắn nhẹ cần căn lại", Status = "New", ResolvedDate = null, Note = "Đã giao kỹ thuật viên ca sáng kiểm tra" }
            );
        }
    }
}
