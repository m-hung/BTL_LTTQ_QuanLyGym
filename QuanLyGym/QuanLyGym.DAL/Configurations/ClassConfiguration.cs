using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasData(
                // =========================================================================
                // Chi nhánh 1 (Quận 1): 4 lớp học nhóm với 4 HLV riêng
                // =========================================================================
                new Class
                {
                    ClassID = 1,
                    ClassName = "Lớp Yoga Căn Bản & Thiền Định",
                    ClassType = "Yoga",
                    BranchID = 1,
                    RoomID = 2,
                    PTID = 2, // HLV Trần Thị Bích
                    DayOfWeek = "Thứ 2 - 4 - 6",
                    StartTime = new TimeSpan(6, 30, 0),
                    EndTime = new TimeSpan(8, 0, 0),
                    MaxCapacity = 20,
                    Description = "Lớp Yoga asana kết hợp kỹ thuật thở Pranayama và thiền tịnh tâm, giúp giảm stress và tăng độ dẻo dai.",
                    Status = true
                },
                new Class
                {
                    ClassID = 2,
                    ClassName = "Lớp Boxing & Kickfit Cường Độ Cao",
                    ClassType = "Boxing",
                    BranchID = 1,
                    RoomID = 1,
                    PTID = 1, // HLV Nguyễn Văn Nam
                    DayOfWeek = "Thứ 2 - 4 - 6",
                    StartTime = new TimeSpan(18, 0, 0),
                    EndTime = new TimeSpan(19, 30, 0),
                    MaxCapacity = 25,
                    Description = "Kỹ thuật đấm đá cơ bản, di chuyển đối kháng và rèn luyện sức bền tim mạch, tự vệ thực chiến.",
                    Status = true
                },
                new Class
                {
                    ClassID = 3,
                    ClassName = "Lớp Zumba Dance Đốt Cháy Calo",
                    ClassType = "Zumba",
                    BranchID = 1,
                    RoomID = 2,
                    PTID = 6, // HLV Lê Hoàng Nam
                    DayOfWeek = "Thứ 3 - 5 - 7",
                    StartTime = new TimeSpan(18, 30, 0),
                    EndTime = new TimeSpan(19, 45, 0),
                    MaxCapacity = 30,
                    Description = "Vũ điệu Zumba Latin sôi động trên nền nhạc lôi cuốn, hỗ trợ đốt cháy 600-800 kcal mỗi buổi tập.",
                    Status = true
                },
                new Class
                {
                    ClassID = 4,
                    ClassName = "Lớp Pilates Định Hình & Cột Sống",
                    ClassType = "Pilates",
                    BranchID = 1,
                    RoomID = 2,
                    PTID = 7, // HLV Đặng Thu Thảo
                    DayOfWeek = "Thứ 3 - 5 - 7",
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(8, 15, 0),
                    MaxCapacity = 18,
                    Description = "Bài tập thảm Pilates tăng cường cơ lõi Core, chỉnh sửa tư thế gù lưng và thon gọn vòng eo.",
                    Status = true
                },

                // =========================================================================
                // Chi nhánh 2 (Bình Thạnh): 4 lớp học nhóm với 4 HLV riêng
                // =========================================================================
                new Class
                {
                    ClassID = 5,
                    ClassName = "Lớp Yoga Căn Bản & Thiền Định",
                    ClassType = "Yoga",
                    BranchID = 2,
                    RoomID = 4,
                    PTID = 8, // HLV Phạm Ngọc Ánh
                    DayOfWeek = "Thứ 2 - 4 - 6",
                    StartTime = new TimeSpan(6, 30, 0),
                    EndTime = new TimeSpan(8, 0, 0),
                    MaxCapacity = 20,
                    Description = "Lớp Yoga asana kết hợp kỹ thuật thở Pranayama và thiền tịnh tâm, giúp giảm stress và tăng độ dẻo dai.",
                    Status = true
                },
                new Class
                {
                    ClassID = 6,
                    ClassName = "Lớp Boxing & Kickfit Cường Độ Cao",
                    ClassType = "Boxing",
                    BranchID = 2,
                    RoomID = 3,
                    PTID = 3, // HLV Nguyễn Đức Minh
                    DayOfWeek = "Thứ 2 - 4 - 6",
                    StartTime = new TimeSpan(18, 0, 0),
                    EndTime = new TimeSpan(19, 30, 0),
                    MaxCapacity = 25,
                    Description = "Kỹ thuật đấm đá cơ bản, di chuyển đối kháng và rèn luyện sức bền tim mạch, tự vệ thực chiến.",
                    Status = true
                },
                new Class
                {
                    ClassID = 7,
                    ClassName = "Lớp Zumba Dance Đốt Cháy Calo",
                    ClassType = "Zumba",
                    BranchID = 2,
                    RoomID = 4,
                    PTID = 4, // HLV Phan Thị Thảo
                    DayOfWeek = "Thứ 3 - 5 - 7",
                    StartTime = new TimeSpan(18, 30, 0),
                    EndTime = new TimeSpan(19, 45, 0),
                    MaxCapacity = 30,
                    Description = "Vũ điệu Zumba Latin sôi động trên nền nhạc lôi cuốn, hỗ trợ đốt cháy 600-800 kcal mỗi buổi tập.",
                    Status = true
                },
                new Class
                {
                    ClassID = 8,
                    ClassName = "Lớp Pilates Định Hình & Cột Sống",
                    ClassType = "Pilates",
                    BranchID = 2,
                    RoomID = 4,
                    PTID = 9, // HLV Trần Quốc Bảo
                    DayOfWeek = "Thứ 3 - 5 - 7",
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(8, 15, 0),
                    MaxCapacity = 18,
                    Description = "Bài tập thảm Pilates tăng cường cơ lõi Core, chỉnh sửa tư thế gù lưng và thon gọn vòng eo.",
                    Status = true
                },

                // =========================================================================
                // Chi nhánh 3 (Quận 7): 4 lớp học nhóm với 4 HLV riêng
                // =========================================================================
                new Class
                {
                    ClassID = 9,
                    ClassName = "Lớp Yoga Căn Bản & Thiền Định",
                    ClassType = "Yoga",
                    BranchID = 3,
                    RoomID = 6,
                    PTID = 10, // HLV Đỗ Khánh Linh
                    DayOfWeek = "Thứ 2 - 4 - 6",
                    StartTime = new TimeSpan(6, 30, 0),
                    EndTime = new TimeSpan(8, 0, 0),
                    MaxCapacity = 20,
                    Description = "Lớp Yoga asana kết hợp kỹ thuật thở Pranayama và thiền tịnh tâm, giúp giảm stress và tăng độ dẻo dai.",
                    Status = true
                },
                new Class
                {
                    ClassID = 10,
                    ClassName = "Lớp Boxing & Kickfit Cường Độ Cao",
                    ClassType = "Boxing",
                    BranchID = 3,
                    RoomID = 5,
                    PTID = 5, // HLV Vũ Hoàng Long
                    DayOfWeek = "Thứ 2 - 4 - 6",
                    StartTime = new TimeSpan(18, 0, 0),
                    EndTime = new TimeSpan(19, 30, 0),
                    MaxCapacity = 25,
                    Description = "Kỹ thuật đấm đá cơ bản, di chuyển đối kháng và rèn luyện sức bền tim mạch, tự vệ thực chiến.",
                    Status = true
                },
                new Class
                {
                    ClassID = 11,
                    ClassName = "Lớp Zumba Dance Đốt Cháy Calo",
                    ClassType = "Zumba",
                    BranchID = 3,
                    RoomID = 6,
                    PTID = 11, // HLV Nguyễn Tiến Đạt
                    DayOfWeek = "Thứ 3 - 5 - 7",
                    StartTime = new TimeSpan(18, 30, 0),
                    EndTime = new TimeSpan(19, 45, 0),
                    MaxCapacity = 30,
                    Description = "Vũ điệu Zumba Latin sôi động trên nền nhạc lôi cuốn, hỗ trợ đốt cháy 600-800 kcal mỗi buổi tập.",
                    Status = true
                },
                new Class
                {
                    ClassID = 12,
                    ClassName = "Lớp Pilates Định Hình & Cột Sống",
                    ClassType = "Pilates",
                    BranchID = 3,
                    RoomID = 6,
                    PTID = 12, // HLV Hoàng Kim Ngân
                    DayOfWeek = "Thứ 3 - 5 - 7",
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(8, 15, 0),
                    MaxCapacity = 18,
                    Description = "Bài tập thảm Pilates tăng cường cơ lõi Core, chỉnh sửa tư thế gù lưng và thon gọn vòng eo.",
                    Status = true
                }
            );
        }
    }
}
