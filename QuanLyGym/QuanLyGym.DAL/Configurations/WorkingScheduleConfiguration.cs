using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class WorkingScheduleConfiguration : IEntityTypeConfiguration<WorkingSchedule>
    {
        public void Configure(EntityTypeBuilder<WorkingSchedule> builder)
        {
            builder.HasData(
                new WorkingSchedule { WorkingScheduleID = 1, PTID = 1, WorkDate = new DateTime(2026, 3, 1), StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(20, 0, 0), Status = "Đã ca", BranchID = 1 }
            );
        }
    }
}