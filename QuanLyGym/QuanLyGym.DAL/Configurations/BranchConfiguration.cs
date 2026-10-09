using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasData(
                new Branch { BranchID = 1, BranchName = "QuanLyGym Cầu Giấy", Address = "123 Cầu Giấy, Hà Nội", Phone = "0987654321", Email = "caugiay@gym.com", OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(22, 0, 0), Status = true },
                new Branch { BranchID = 2, BranchName = "QuanLyGym Đống Đa", Address = "456 Xã Đàn, Hà Nội", Phone = "0912345678", Email = "dongda@gym.com", OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(22, 0, 0), Status = true }
            );
        }
    }
}