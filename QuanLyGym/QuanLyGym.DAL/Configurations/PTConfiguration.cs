using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class PTConfiguration : IEntityTypeConfiguration<PT>
    {
        public void Configure(EntityTypeBuilder<PT> builder)
        {
            builder.HasData(
                new PT { PTID = 1, AccountID = 2, FullName = "Nguyễn Văn Nam", Gender = "Nam", DateOfBirth = new DateTime(1995, 5, 20), Phone = "0988888888", Email = "nam@gym.com", Specialization = "Tăng cơ, Giảm mỡ", Experience = 5, Status = true, BranchID = 1 }
            );
        }
    }
}