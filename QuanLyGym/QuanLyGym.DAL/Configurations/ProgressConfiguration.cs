using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class ProgressConfiguration : IEntityTypeConfiguration<Progress>
    {
        public void Configure(EntityTypeBuilder<Progress> builder)
        {
            builder.HasData(
                new Progress { ProgressID = 1, MemberID = 1, PTID = 1, RecordDate = new DateTime(2026, 3, 1), Height = 172, Weight = 68, BodyFat = 18, MuscleMass = 32, BMI = 23, Chest = 95, Waist = 78, Hip = 92, Arm = 34, Thigh = 54, Note = "Chỉ số ban đầu bình thường" }
            );
        }
    }
}