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
                new Incident { IncidentID = 1, EquipmentID = 1, ReportedBy = 1, ReportDate = new DateTime(2026, 2, 10), Description = "Máy chạy bộ phát ra tiếng kêu lạ ở băng tải", Status = "Resolved", ResolvedDate = new DateTime(2026, 2, 11), Note = "Đã siết lại ốc và căn chỉnh băng tải" }
            );
        }
    }
}