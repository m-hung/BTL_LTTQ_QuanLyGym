using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasData(
                new Member { MemberID = 1, AccountID = 3, FullName = "Trần Văn Hùng", Gender = "Nam", DateOfBirth = new DateTime(2000, 10, 10), Phone = "0977777777", Email = "hung@gmail.com", Address = "Cầu Giấy, Hà Nội", Height = 172, Weight = 68, BodyFat = 18, MuscleMass = 32, BMI = 23, Avatar = "", CreatedAt = new DateTime(2026, 1, 1), BranchID = 1 }
            );
        }
    }
}