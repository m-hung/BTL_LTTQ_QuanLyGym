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
                    Benefits = "Tập luyện không giới hạn khung giờ toàn hệ thống, Sử dụng đầy đủ hệ thống máy tạ & Cardio, Đo & phân tích chỉ số InBody định kỳ, Trang bị Smart Locker tủ đồ cá nhân, Miễn phí phòng tắm nóng lạnh & tiện ích",
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
                    Benefits = "12 buổi tập 1-1 với Huấn luyện viên, Thiết lập lộ trình cá nhân hóa, Kiểm tra chỉ số InBody hàng tháng, Luyện tập tự do toàn hệ thống Titan Fitness, Miễn phí tủ đồ Smart Locker & nước uống",
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
                    Benefits = "24 buổi tập 1-1 với Huấn luyện viên, Tư vấn dinh dưỡng chuyên sâu, Đo InBody định kỳ & theo sát vóc dáng, Sử dụng phòng xông hơi Sauna & Smart Locker riêng, Luyện tập tự do toàn hệ thống Titan Fitness, Miễn phí nước uống mỗi buổi tập",
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
                    Benefits = "36 buổi tập 1-1 cùng Huấn luyện viên cao cấp, Bác sĩ dinh dưỡng & HLV theo sát 24/7, Full dịch vụ Sauna VIP & VIP Locker riêng biệt, Miễn phí khăn tập & nước uống mỗi buổi, Luyện tập tự do toàn hệ thống Titan Fitness, Cam kết cải thiện thể hình bằng văn bản",
                    Description = "Gói tập thượng lưu cao cấp nhất, cam kết kết quả và trải nghiệm dịch vụ chuẩn 5 sao.",
                    Status = true,
                    BranchID = 1
                }
            );
        }
    }
}
