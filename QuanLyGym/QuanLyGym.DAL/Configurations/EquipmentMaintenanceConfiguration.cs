using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class EquipmentMaintenanceConfiguration : IEntityTypeConfiguration<EquipmentMaintenance>
    {
        public void Configure(EntityTypeBuilder<EquipmentMaintenance> builder)
        {
            builder.HasData(
                new EquipmentMaintenance { MaintenanceID = 1, EquipmentID = 1, MaintenanceDate = new DateTime(2025, 12, 1), NextMaintenanceDate = new DateTime(2026, 6, 1), Description = "Bảo dưỡng định kỳ thảm chạy và tra dầu băng chuyền", Cost = 500000, Status = "Hoàn thành" },
                new EquipmentMaintenance { MaintenanceID = 2, EquipmentID = 2, MaintenanceDate = new DateTime(2025, 12, 15), NextMaintenanceDate = new DateTime(2026, 6, 15), Description = "Siết chặt ốc liên kết và kiểm tra thanh an toàn J-Hooks", Cost = 300000, Status = "Hoàn thành" },
                new EquipmentMaintenance { MaintenanceID = 3, EquipmentID = 4, MaintenanceDate = new DateTime(2026, 1, 10), NextMaintenanceDate = new DateTime(2026, 7, 10), Description = "Thay dây cáp thép bọc nylon và bôi trơn ròng rọc", Cost = 750000, Status = "Hoàn thành" },
                new EquipmentMaintenance { MaintenanceID = 4, EquipmentID = 5, MaintenanceDate = new DateTime(2026, 2, 5), NextMaintenanceDate = new DateTime(2026, 8, 5), Description = "Thay bàn đạp dây đai và cảm biến nhịp tim", Cost = 600000, Status = "Hoàn thành" },
                new EquipmentMaintenance { MaintenanceID = 5, EquipmentID = 6, MaintenanceDate = new DateTime(2026, 2, 20), NextMaintenanceDate = new DateTime(2026, 8, 20), Description = "Tra mỡ vòng bi ray trượt máy đạp đùi", Cost = 450000, Status = "Hoàn thành" },
                new EquipmentMaintenance { MaintenanceID = 6, EquipmentID = 7, MaintenanceDate = new DateTime(2026, 3, 5), NextMaintenanceDate = new DateTime(2026, 9, 5), Description = "Căn chỉnh lại hệ thống đối trọng tạ cáp bên phải", Cost = 800000, Status = "Hoàn thành" },
                new EquipmentMaintenance { MaintenanceID = 7, EquipmentID = 8, MaintenanceDate = new DateTime(2026, 3, 15), NextMaintenanceDate = new DateTime(2026, 9, 15), Description = "Bảo trì mô-tơ kháng lực máy trượt tuyết", Cost = 900000, Status = "Hoàn thành" },
                new EquipmentMaintenance { MaintenanceID = 8, EquipmentID = 1, MaintenanceDate = new DateTime(2026, 6, 1), NextMaintenanceDate = new DateTime(2026, 12, 1), Description = "Kiểm tra động cơ 4.0 HP và căn chỉnh độ lệch băng tải", Cost = 650000, Status = "Hoàn thành" },
                new EquipmentMaintenance { MaintenanceID = 9, EquipmentID = 7, MaintenanceDate = new DateTime(2026, 10, 15), NextMaintenanceDate = new DateTime(2026, 11, 15), Description = "Thay thế chốt tạ selector pin bị kẹt và bảo dưỡng bánh xe dẫn hướng", Cost = 400000, Status = "Đang xử lý" },
                new EquipmentMaintenance { MaintenanceID = 10, EquipmentID = 10, MaintenanceDate = new DateTime(2026, 10, 20), NextMaintenanceDate = new DateTime(2027, 4, 20), Description = "Thay nước bình chứa và làm sạch bánh đà máy chèo thuyền", Cost = 350000, Status = "Hoàn thành" }
            );
        }
    }
}
