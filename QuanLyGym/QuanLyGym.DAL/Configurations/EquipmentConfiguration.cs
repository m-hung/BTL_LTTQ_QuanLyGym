using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder.HasData(
                new Equipment { EquipmentID = 1, RoomID = 2, EquipmentName = "Máy chạy bộ Technogym Excite Live", EquipmentCode = "TM-01", PurchaseDate = new DateTime(2025, 6, 15), Price = 35000000, Status = "Tốt", Description = "Máy chạy bộ cao cấp màn hình cảm ứng" },
                new Equipment { EquipmentID = 2, RoomID = 1, EquipmentName = "Dàn gánh tạ Squat Rack Olympic", EquipmentCode = "SQ-01", PurchaseDate = new DateTime(2025, 6, 20), Price = 18000000, Status = "Tốt", Description = "Khung gánh tạ an toàn chịu tải 500kg" },
                new Equipment { EquipmentID = 3, RoomID = 1, EquipmentName = "Ghế đẩy ngực Incline Bench Press", EquipmentCode = "BP-01", PurchaseDate = new DateTime(2025, 7, 5), Price = 12000000, Status = "Tốt", Description = "Ghế dốc tập ngực trên chuẩn thi đấu" },
                new Equipment { EquipmentID = 4, RoomID = 1, EquipmentName = "Máy kéo xô Lat Pulldown & Row", EquipmentCode = "LP-01", PurchaseDate = new DateTime(2025, 7, 10), Price = 28000000, Status = "Tốt", Description = "Máy khối đôi kéo xô và chèo xô lưng" },
                new Equipment { EquipmentID = 5, RoomID = 2, EquipmentName = "Xe đạp tập Cardio LifeFitness", EquipmentCode = "BK-01", PurchaseDate = new DateTime(2025, 8, 1), Price = 16000000, Status = "Tốt", Description = "Xe đạp tĩnh kháng lực từ trường" },
                new Equipment { EquipmentID = 6, RoomID = 3, EquipmentName = "Máy đạp đùi Leg Press 45 độ", EquipmentCode = "LP-02", PurchaseDate = new DateTime(2025, 8, 15), Price = 32000000, Status = "Tốt", Description = "Máy đạp chân cơ đùi chịu tải trọng lớn" },
                new Equipment { EquipmentID = 7, RoomID = 3, EquipmentName = "Dàn cáp đôi Dual Cable Crossover", EquipmentCode = "CC-01", PurchaseDate = new DateTime(2025, 9, 1), Price = 42000000, Status = "Bảo trì", Description = "Dàn cáp đôi đa hướng ròng rọc nhôm" },
                new Equipment { EquipmentID = 8, RoomID = 4, EquipmentName = "Máy trượt tuyết Elliptical Matrix", EquipmentCode = "EL-01", PurchaseDate = new DateTime(2025, 9, 15), Price = 26000000, Status = "Tốt", Description = "Máy tập toàn thân bảo vệ khớp gối" },
                new Equipment { EquipmentID = 9, RoomID = 5, EquipmentName = "Bộ tạ tay Dumbbell 5-40kg kèm giá", EquipmentCode = "DB-01", PurchaseDate = new DateTime(2025, 10, 1), Price = 45000000, Status = "Tốt", Description = "Dàn tạ bọc urethene cao cấp 20 đôi" },
                new Equipment { EquipmentID = 10, RoomID = 6, EquipmentName = "Máy chèo thuyền WaterRower", EquipmentCode = "RW-01", PurchaseDate = new DateTime(2025, 10, 15), Price = 22000000, Status = "Tốt", Description = "Máy chèo thuyền cản nước bằng gỗ sồi" }
            );
        }
    }
}
