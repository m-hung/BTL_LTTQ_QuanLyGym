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
                new Equipment { EquipmentID = 1, RoomID = 2, EquipmentName = "Máy chạy bộ Technogym", EquipmentCode = "CB-01", PurchaseDate = new DateTime(2025, 6, 15), Price = 25000000, Status = "Tốt", Description = "Máy chạy bộ cao cấp" },
                new Equipment { EquipmentID = 2, RoomID = 1, EquipmentName = "Dàn gánh tạ Squat Rack", EquipmentCode = "SQ-01", PurchaseDate = new DateTime(2025, 6, 20), Price = 15000000, Status = "Tốt", Description = "Khung gánh tạ an toàn" }
            );
        }
    }
}