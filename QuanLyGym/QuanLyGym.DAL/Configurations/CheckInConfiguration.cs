using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class CheckInConfiguration : IEntityTypeConfiguration<CheckIn>
    {
        public void Configure(EntityTypeBuilder<CheckIn> builder)
        {
            builder.HasData(
                new CheckIn { CheckInID = 1, MemberID = 1, PTID = 1, CheckInTime = new DateTime(2026, 3, 1, 16, 55, 0), Status = "Approved", Note = "Check-in tập cùng PT", BranchID = 1 }
            );
        }
    }
}