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
                // Branch 1 (Quận 1)
                new Room { RoomID = 1, RoomName = "Phòng Gym & Tạ Tầng 1", RoomType = "Gym", Capacity = 60, Description = "Khu vực tập tạ tổng hợp và máy khối", Status = true, BranchID = 1 },
                new Room { RoomID = 2, RoomName = "Phòng Cardio Tầng 2", RoomType = "Cardio", Capacity = 40, Description = "Máy chạy bộ, xe đạp và máy trượt tuyết", Status = true, BranchID = 1 },

                // Branch 2 (Bình Thạnh)
                new Room { RoomID = 3, RoomName = "Phòng Gym Free Weight Tầng 1", RoomType = "Gym", Capacity = 50, Description = "Khu tạ tự do và dàn Squat/Bench", Status = true, BranchID = 2 },
                new Room { RoomID = 4, RoomName = "Phòng Cardio & Functional Tầng 2", RoomType = "Functional", Capacity = 35, Description = "Khu chức năng và tim mạch", Status = true, BranchID = 2 },

                // Branch 3 (Quận 7)
                new Room { RoomID = 5, RoomName = "Phòng Strength & Conditioning", RoomType = "Gym", Capacity = 45, Description = "Khu tập thể lực và tạ khối", Status = true, BranchID = 3 },
                new Room { RoomID = 6, RoomName = "Phòng Studio & Cardio Tầng 2", RoomType = "Cardio", Capacity = 30, Description = "Khu cardio và tập studio", Status = true, BranchID = 3 }
            );
        }
    }
}
