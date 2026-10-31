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
                new Package
                {
                    PackageID = 1,
                    PackageName = "Classic",
                    Duration = 30,
                    Price = 500000,
                    TotalPTSessions = 0,
                    IsPTPackage = false,
                    Benefits = "Tập không giới hạn khung giờ, Sử dụng đầy đủ máy tạ & Cardio, Tủ đồ cá nhân",
                    Description = "Gói tập tự do tiêu chuẩn dành cho hội viên muốn duy trì rèn luyện hàng ngày.",
                    Status = true,
                    BranchID = 1
                },
                new Package
                {
                    PackageID = 2,
                    PackageName = "Silver",
                    Duration = 90,
                    Price = 2800000,
                    TotalPTSessions = 12,
                    IsPTPackage = true,
                    Benefits = "12 buổi tập 1-1 với HLV, Thiết lập lộ trình cá nhân hóa, Kiểm tra chỉ số InBody hàng tháng",
                    Description = "Gói rèn luyện chuyên sâu 3 tháng cùng HLV, phù hợp người mới bắt đầu cần định hướng.",
                    Status = true,
                    BranchID = 1
                },
                new Package
                {
                    PackageID = 3,
                    PackageName = "Gold",
                    Duration = 90,
                    Price = 5800000,
                    TotalPTSessions = 24,
                    IsPTPackage = true,
                    Benefits = "24 buổi tập 1-1 với HLV, Tư vấn dinh dưỡng chuyên sâu, Đo InBody định kỳ, Sử dụng phòng xông hơi Sauna",
                    Description = "Gói nâng cao toàn diện giúp cải thiện thể hình nhanh chóng và thư giãn sau giờ tập.",
                    Status = true,
                    BranchID = 1
                },
                new Package
                {
                    PackageID = 4,
                    PackageName = "VIP Diamond",
                    Duration = 90,
                    Price = 12500000,
                    TotalPTSessions = 36,
                    IsPTPackage = true,
                    Benefits = "36 buổi tập 1-1 với HLV cao cấp, Full dịch vụ Sauna & VIP Locker, Miễn phí khăn tập & nước uống mỗi buổi",
                    Description = "Gói tập thượng lưu cao cấp nhất, cam kết kết quả và trải nghiệm dịch vụ chuẩn 5 sao.",
                    Status = true,
                    BranchID = 1
                }
            );
        }
    }
}