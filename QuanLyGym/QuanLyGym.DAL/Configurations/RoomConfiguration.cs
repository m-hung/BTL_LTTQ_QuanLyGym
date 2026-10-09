using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasData(
                new Room { RoomID = 1, RoomName = "Phòng Gym Tầng 1", RoomType = "Gym", Capacity = 50, Description = "Khu vực tập tạ tổng hợp", Status = true, BranchID = 1 },
                new Room { RoomID = 2, RoomName = "Phòng Cardio Tầng 2", RoomType = "Cardio", Capacity = 30, Description = "Máy chạy bộ và xe đạp", Status = true, BranchID = 1 }
            );
        }
    }
}