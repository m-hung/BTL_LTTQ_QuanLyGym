using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasData(
                // =========================================================================
                // Thông báo hệ thống, nhắc nhở & khuyến mãi trải dài trong năm 2026
                // =========================================================================
                new Notification
                {
                    NotificationID = 1,
                    AccountID = null, // Toàn hệ thống
                    Title = "Chúc Mừng Năm Mới 2026 - Lịch Hoạt Động Tết",
                    Content = "Hệ thống QuanLyGym trân trọng gửi lời chúc mừng năm mới 2026 đến toàn thể quý Hội viên và HLV. Hệ thống sẽ mở cửa xuyên suốt phục vụ quý khách.",
                    NotificationType = "System",
                    IsRead = true,
                    CreatedAt = new DateTime(2026, 1, 1, 8, 0, 0),
                    BranchID = null
                },
                new Notification
                {
                    NotificationID = 2,
                    AccountID = 7, // Member Khánh Hưng
                    Title = "Xác Nhận Kích Hoạt Gói VIP Diamond",
                    Content = "Chào bạn Khánh Hưng! Hợp đồng gói tập VIP Diamond của bạn đã được kích hoạt thành công. HLV cá nhân Nguyễn Văn Nam sẽ đồng hành cùng bạn.",
                    NotificationType = "Reminder",
                    IsRead = true,
                    CreatedAt = new DateTime(2026, 1, 1, 9, 30, 0),
                    BranchID = 1
                },
                new Notification
                {
                    NotificationID = 3,
                    AccountID = null,
                    Title = "Khai Trương Studio GroupX & Lớp Học Nhóm Mới",
                    Content = "QuanLyGym chính thức mở các lớp học nhóm: Yoga, Zumba, Boxing và Pilates tại cả 3 chi nhánh với đội ngũ HLV hàng đầu. Đăng ký ngay tại quầy lễ tân!",
                    NotificationType = "Promotion",
                    IsRead = true,
                    CreatedAt = new DateTime(2026, 1, 10, 10, 0, 0),
                    BranchID = null
                },
                new Notification
                {
                    NotificationID = 4,
                    AccountID = null,
                    Title = "Ưu Đãi Đặc Biệt Quốc Tế Phụ Nữ 8/3",
                    Content = "Tặng ngay 20% khi đăng ký hoặc nâng cấp gói tập và giảm 50% phí thuê HLV kèm riêng cho phái đẹp trong tuần lễ từ 01/03 đến 08/03/2026.",
                    NotificationType = "Promotion",
                    IsRead = true,
                    CreatedAt = new DateTime(2026, 3, 1, 8, 0, 0),
                    BranchID = null
                },
                new Notification
                {
                    NotificationID = 5,
                    AccountID = 8, // Member Minh Hưng
                    Title = "Nhắc Nhở Đo Lại Chỉ Số Thể Chất InBody Định Kỳ",
                    Content = "Đã 3 tháng kể từ lần kiểm tra thể chất gần nhất. Mời bạn đến phòng đo InBody tại Chi nhánh 1 để HLV Nam đánh giá lại tiến độ tăng cơ giảm mỡ.",
                    NotificationType = "Reminder",
                    IsRead = true,
                    CreatedAt = new DateTime(2026, 4, 15, 14, 0, 0),
                    BranchID = 1
                },
                new Notification
                {
                    NotificationID = 6,
                    AccountID = null,
                    Title = "Hoàn Tất Bảo Trì & Nâng Cấp Hệ Thống Máy Chạy Bộ",
                    Content = "Hệ thống máy chạy bộ Matrix và dàn tạ khối tại Chi nhánh Quận 1 & Bình Thạnh đã được bảo dưỡng toàn diện, sẵn sàng phục vụ quý hội viên.",
                    NotificationType = "Maintenance",
                    IsRead = true,
                    CreatedAt = new DateTime(2026, 6, 2, 9, 0, 0),
                    BranchID = 1
                },
                new Notification
                {
                    NotificationID = 7,
                    AccountID = null,
                    Title = "Chào Hè Sôi Động 2026 - Thử Thách Đốt Mỡ 30 Ngày",
                    Content = "Tham gia giải chạy và thử thách Fitness 30 ngày để nhận gói tập VIP 6 tháng và phần thưởng giá trị từ Ban quản lý QuanLyGym.",
                    NotificationType = "System",
                    IsRead = true,
                    CreatedAt = new DateTime(2026, 7, 1, 8, 30, 0),
                    BranchID = null
                },
                new Notification
                {
                    NotificationID = 8,
                    AccountID = 9, // Member Quỳnh Hoa
                    Title = "Thông Báo Sắp Hết Hạn Thẻ Tập",
                    Content = "Thẻ tập của bạn sắp hết hạn trong 7 ngày tới. Hãy liên hệ bộ phận Lễ tân hoặc gia hạn trên ứng dụng để nhận ngay ưu đãi 10% gói tập mới.",
                    NotificationType = "Reminder",
                    IsRead = false,
                    CreatedAt = new DateTime(2026, 9, 25, 10, 0, 0),
                    BranchID = 1
                },
                new Notification
                {
                    NotificationID = 9,
                    AccountID = null,
                    Title = "Đại Tiệc Siêu Sale Black Friday Tháng 11/2026",
                    Content = "Cơ hội duy nhất trong năm: Mua 1 năm tặng 3 tháng, giảm giá cực sốc toàn bộ các gói Classic, Silver, Gold và VIP Diamond!",
                    NotificationType = "Promotion",
                    IsRead = false,
                    CreatedAt = new DateTime(2026, 11, 20, 8, 0, 0),
                    BranchID = null
                },
                new Notification
                {
                    NotificationID = 10,
                    AccountID = 2, // PT Nam
                    Title = "Thông Báo Lịch Đặt Hẹn PT Mới Tháng 12",
                    Content = "Bạn có 15 lịch hẹn tập luyện mới từ hội viên trải dài suốt tháng 12/2026. Vui lòng kiểm tra mục Lịch tập và xác nhận các ca chờ duyệt.",
                    NotificationType = "Reminder",
                    IsRead = false,
                    CreatedAt = new DateTime(2026, 11, 27, 8, 0, 0),
                    BranchID = 1
                },
                new Notification
                {
                    NotificationID = 11,
                    AccountID = null,
                    Title = "Kế Hoạch Tổng Vệ Sinh & Kiểm Tra An Toàn Cuối Năm",
                    Content = "Chi nhánh Quận 7 và Bình Thạnh sẽ thực hiện kiểm tra an toàn thiết bị phòng tập định kỳ vào ngày 15/12/2026 từ 22:00 đến 24:00.",
                    NotificationType = "Maintenance",
                    IsRead = false,
                    CreatedAt = new DateTime(2026, 11, 27, 9, 0, 0),
                    BranchID = 3
                }
            );
        }
    }
}
