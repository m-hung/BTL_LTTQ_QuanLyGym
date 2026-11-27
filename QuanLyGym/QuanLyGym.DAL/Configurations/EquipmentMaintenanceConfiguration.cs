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
                // =========================================================================
                // Lịch sử bảo trì, sửa chữa thiết bị máy tập FULL 12 THÁNG NĂM 2026
                // =========================================================================

                // Tháng 1/2026
                new EquipmentMaintenance { MaintenanceID = 1, EquipmentID = 4, MaintenanceDate = new DateTime(2026, 1, 10), NextMaintenanceDate = new DateTime(2026, 7, 10), Description = "Thay dây cáp thép bọc nylon và bôi trơn ròng rọc giàn kéo xô", Cost = 750000, Status = "Hoàn thành" },

                // Tháng 2/2026
                new EquipmentMaintenance { MaintenanceID = 2, EquipmentID = 5, MaintenanceDate = new DateTime(2026, 2, 15), NextMaintenanceDate = new DateTime(2026, 8, 15), Description = "Thay bàn đạp dây đai và nối lại cảm biến nhịp tim xe đạp tập", Cost = 600000, Status = "Hoàn thành" },

                // Tháng 3/2026
                new EquipmentMaintenance { MaintenanceID = 3, EquipmentID = 7, MaintenanceDate = new DateTime(2026, 3, 12), NextMaintenanceDate = new DateTime(2026, 9, 12), Description = "Căn chỉnh lại hệ thống đối trọng tạ cáp bên phải dàn cáp đôi", Cost = 800000, Status = "Hoàn thành" },

                // Tháng 4/2026
                new EquipmentMaintenance { MaintenanceID = 4, EquipmentID = 6, MaintenanceDate = new DateTime(2026, 4, 18), NextMaintenanceDate = new DateTime(2026, 10, 18), Description = "Tra mỡ vòng bi ray trượt và chỉnh chốt an toàn máy Leg Press", Cost = 450000, Status = "Hoàn thành" },

                // Tháng 5/2026
                new EquipmentMaintenance { MaintenanceID = 5, EquipmentID = 8, MaintenanceDate = new DateTime(2026, 5, 20), NextMaintenanceDate = new DateTime(2026, 11, 20), Description = "Bảo trì mô-tơ kháng lực từ tính máy trượt tuyết Elliptical", Cost = 900000, Status = "Hoàn thành" },

                // Tháng 6/2026
                new EquipmentMaintenance { MaintenanceID = 6, EquipmentID = 1, MaintenanceDate = new DateTime(2026, 6, 8), NextMaintenanceDate = new DateTime(2026, 12, 8), Description = "Kiểm tra động cơ 4.0 HP và căn chỉnh độ lệch băng tải máy chạy bộ", Cost = 650000, Status = "Hoàn thành" },

                // Tháng 7/2026
                new EquipmentMaintenance { MaintenanceID = 7, EquipmentID = 2, MaintenanceDate = new DateTime(2026, 7, 14), NextMaintenanceDate = new DateTime(2027, 1, 14), Description = "Bọc lại đệm mút simili cao cấp và siết ốc dàn khung giàn Squat", Cost = 550000, Status = "Hoàn thành" },

                // Tháng 8/2026
                new EquipmentMaintenance { MaintenanceID = 8, EquipmentID = 3, MaintenanceDate = new DateTime(2026, 8, 22), NextMaintenanceDate = new DateTime(2027, 2, 22), Description = "Cân chỉnh độ dốc ghế đa năng và kiểm tra khóa an toàn J-Hooks", Cost = 350000, Status = "Hoàn thành" },

                // Tháng 9/2026
                new EquipmentMaintenance { MaintenanceID = 9, EquipmentID = 9, MaintenanceDate = new DateTime(2026, 9, 15), NextMaintenanceDate = new DateTime(2027, 3, 15), Description = "Sơn phủ chống rỉ và dùng keo khóa ren siết chặt đầu tạ tay cao su", Cost = 400000, Status = "Hoàn thành" },

                // Tháng 10/2026
                new EquipmentMaintenance { MaintenanceID = 10, EquipmentID = 10, MaintenanceDate = new DateTime(2026, 10, 20), NextMaintenanceDate = new DateTime(2027, 4, 20), Description = "Thay nước bình chứa và làm sạch bánh đà máy chèo thuyền", Cost = 350000, Status = "Hoàn thành" },

                // Tháng 11/2026
                new EquipmentMaintenance { MaintenanceID = 11, EquipmentID = 7, MaintenanceDate = new DateTime(2026, 11, 15), NextMaintenanceDate = new DateTime(2027, 5, 15), Description = "Thay thế chốt tạ selector pin bị kẹt và bảo dưỡng bánh xe dẫn hướng", Cost = 500000, Status = "Hoàn thành" },

                // Tháng 12/2026
                new EquipmentMaintenance { MaintenanceID = 12, EquipmentID = 1, MaintenanceDate = new DateTime(2026, 12, 10), NextMaintenanceDate = new DateTime(2027, 6, 10), Description = "Bảo dưỡng tổng thể động cơ, thay chổi than và tra dầu silicon máy chạy", Cost = 850000, Status = "Hoàn thành" }
            );
        }
    }
}
