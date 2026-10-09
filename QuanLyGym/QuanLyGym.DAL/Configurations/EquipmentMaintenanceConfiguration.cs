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
                new EquipmentMaintenance { MaintenanceID = 1, EquipmentID = 1, MaintenanceDate = new DateTime(2025, 12, 1), NextMaintenanceDate = new DateTime(2026, 6, 1), Description = "Bảo dưỡng định kỳ thảm chạy và tra dầu", Cost = 500000, Status = "Hoàn thành" }
            );
        }
    }
}