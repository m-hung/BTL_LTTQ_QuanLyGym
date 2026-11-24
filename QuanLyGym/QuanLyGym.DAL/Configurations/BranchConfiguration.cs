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
                new Branch { BranchID = 1, BranchName = "QuanLyGym Quận 1", Address = "123 Nguyễn Thị Minh Khai, Phường Bến Thành, Quận 1, TP. HCM", Phone = "02838221122", Email = "quan1@quanlygym.vn", OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(22, 0, 0), Status = true },
                new Branch { BranchID = 2, BranchName = "QuanLyGym Bình Thạnh", Address = "456 Điện Biên Phủ, Phường 25, Quận Bình Thạnh, TP. HCM", Phone = "02838332233", Email = "binhthanh@quanlygym.vn", OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(22, 0, 0), Status = true },
                new Branch { BranchID = 3, BranchName = "QuanLyGym Quận 7", Address = "789 Nguyễn Lương Bằng, Phường Tân Phú, Quận 7, TP. HCM", Phone = "02838443344", Email = "quan7@quanlygym.vn", OpenTime = new TimeSpan(6, 0, 0), CloseTime = new TimeSpan(22, 0, 0), Status = true }
            );
        }
    }
}
