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
                new Package { PackageID = 1, PackageName = "Classic", Duration = 30, Price = 500000, TotalSessions = 30, IsPTPackage = false, Benefits = "Tập tự do không giới hạn các khung giờ, tủ đồ cá nhân", Description = "Gói tập tự do tiêu chuẩn", Status = true, BranchID = 1 },
                new Package { PackageID = 2, PackageName = "Silver", Duration = 90, Price = 2800000, TotalSessions = 24, IsPTPackage = true, Benefits = "Kèm PT 1-1 (12 buổi PT), kiểm tra chỉ số InBody định kỳ", Description = "Gói tập rèn luyện chuyên sâu cùng HLV cá nhân", Status = true, BranchID = 1 },
                new Package { PackageID = 3, PackageName = "Gold", Duration = 180, Price = 5800000, TotalSessions = 48, IsPTPackage = true, Benefits = "Kèm PT 1-1 (36 buổi PT), đo InBody định kỳ, phòng xông hơi Sauna", Description = "Gói tập Gold cao cấp toàn diện", Status = true, BranchID = 1 },
                new Package { PackageID = 4, PackageName = "VIP Diamond", Duration = 365, Price = 12500000, TotalSessions = 100, IsPTPackage = true, Benefits = "Kèm PT 1-1 không giới hạn, full dịch vụ VIP Sauna, khăn tập & locker riêng", Description = "Gói tập thượng lưu VIP Diamond đẳng cấp nhất", Status = true, BranchID = 1 }
            );
        }
    }
}
