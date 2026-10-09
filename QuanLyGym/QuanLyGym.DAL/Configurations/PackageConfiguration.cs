using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasData(
                new Package { PackageID = 1, PackageName = "Gói Thường 1 Tháng", Duration = 30, Price = 500000, TotalSessions = 30, Benefits = "Tập tự do các phòng", Description = "Gói cơ bản", Status = true, BranchID = 1 },
                new Package { PackageID = 2, PackageName = "Gói VIP 3 Tháng + PT", Duration = 90, Price = 3000000, TotalSessions = 36, Benefits = "Kèm PT 1-1, Nước uống miễn phí", Description = "Gói cao cấp", Status = true, BranchID = 1 }
            );
        }
    }
}