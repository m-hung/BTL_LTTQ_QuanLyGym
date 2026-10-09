using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasData(
                new Schedule { ScheduleID = 1, MemberID = 1, PTID = 1, RoomID = 1, TrainingDate = new DateTime(2026, 3, 1), StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(18, 30, 0), TrainingType = "Tập Ngực & Tay Sau", Status = "Completed", Note = "Hội viên tập tốt, nâng tạ ngực 50kg", BranchID = 1 }
            );
        }
    }
}