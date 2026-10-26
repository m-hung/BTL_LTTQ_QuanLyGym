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
                // ========== PT 1 (Nguyễn Văn Nam - Branch 1) ==========
                // Tuần 1 (26/10 - 01/11/2026)
                new Schedule { ScheduleID = 1, MemberID = 1, PTID = 1, RoomID = 1, TrainingDate = new DateTime(2026, 10, 26), StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(10, 0, 0), TrainingType = "Tập Ngực & Tay Sau (Hypertrophy)", Status = "Confirmed", Note = "Hội viên chuẩn bị thể lực tốt, mức tạ ngực 60kg", BranchID = 1 },
                new Schedule { ScheduleID = 2, MemberID = 2, PTID = 1, RoomID = 2, TrainingDate = new DateTime(2026, 10, 27), StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(15, 30, 0), TrainingType = "Cardio HIIT & Đốt Mỡ Chuyên Sâu", Status = "Confirmed", Note = "Đốt calo cường độ cao, theo dõi nhịp tim", BranchID = 1 },
                new Schedule { ScheduleID = 3, MemberID = 1, PTID = 1, RoomID = 1, TrainingDate = new DateTime(2026, 10, 29), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 30, 0), TrainingType = "Tập Lưng Xô & Tay Trước", Status = "Scheduled", Note = "Tập trung bài Lat Pulldown & Seated Row", BranchID = 1 },
                new Schedule { ScheduleID = 4, MemberID = 3, PTID = 1, RoomID = 1, TrainingDate = new DateTime(2026, 10, 31), StartTime = new TimeSpan(15, 0, 0), EndTime = new TimeSpan(16, 30, 0), TrainingType = "Tập Chân Mông & Giãn Cơ", Status = "Scheduled", Note = "Khởi động kỹ khớp gối", BranchID = 1 },

                // Tuần 2 (02/11 - 08/11/2026)
                new Schedule { ScheduleID = 5, MemberID = 1, PTID = 1, RoomID = 1, TrainingDate = new DateTime(2026, 11, 2), StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(10, 0, 0), TrainingType = "Tập Vai & Bụng Core", Status = "Scheduled", Note = "Đẩy tạ vai 20kg mỗi bên", BranchID = 1 },
                new Schedule { ScheduleID = 6, MemberID = 2, PTID = 1, RoomID = 1, TrainingDate = new DateTime(2026, 11, 4), StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(15, 30, 0), TrainingType = "Tập Ngực & Tay Sau Nâng Cao", Status = "Scheduled", Note = "Giảm thời gian nghỉ giữa hiệp", BranchID = 1 },
                new Schedule { ScheduleID = 7, MemberID = 1, PTID = 1, RoomID = 1, TrainingDate = new DateTime(2026, 11, 6), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 30, 0), TrainingType = "Full Body Functional Training", Status = "Scheduled", Note = "Tập liên hoàn toàn thân", BranchID = 1 },

                // Tuần 3 (09/11 - 15/11/2026)
                new Schedule { ScheduleID = 8, MemberID = 1, PTID = 1, RoomID = 1, TrainingDate = new DateTime(2026, 11, 9), StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(10, 0, 0), TrainingType = "Kiểm Tra Tiến Độ & Strength Test", Status = "Scheduled", Note = "Đánh giá sức mạnh sau chu kỳ 1 tháng", BranchID = 1 },
                new Schedule { ScheduleID = 9, MemberID = 3, PTID = 1, RoomID = 1, TrainingDate = new DateTime(2026, 11, 11), StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(15, 30, 0), TrainingType = "Tập Lưng Xô & Bắp Tay", Status = "Scheduled", Note = "Dùng đai hỗ trợ lưng", BranchID = 1 },
                new Schedule { ScheduleID = 10, MemberID = 2, PTID = 1, RoomID = 2, TrainingDate = new DateTime(2026, 11, 13), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 30, 0), TrainingType = "Cardio Circuit & Core Stability", Status = "Scheduled", Note = "Bài tập sức bền và thăng bằng", BranchID = 1 },

                // ========== PT 2 (Trần Thị Bích - Branch 1) ==========
                // Tuần 1 (26/10 - 01/11/2026)
                new Schedule { ScheduleID = 11, MemberID = 3, PTID = 2, RoomID = 1, TrainingDate = new DateTime(2026, 10, 26), StartTime = new TimeSpan(16, 0, 0), EndTime = new TimeSpan(17, 30, 0), TrainingType = "Tư Thế Squat & Cơ Đùi Nữ", Status = "Confirmed", Note = "Chỉnh sửa góc đầu gối khi xuống tạ", BranchID = 1 },
                new Schedule { ScheduleID = 12, MemberID = 2, PTID = 2, RoomID = 1, TrainingDate = new DateTime(2026, 10, 28), StartTime = new TimeSpan(17, 30, 0), EndTime = new TimeSpan(19, 0, 0), TrainingType = "Tập Mông Quả Đào & Thon Gọn Eo", Status = "Confirmed", Note = "Hip Thrust 40kg, kết hợp dây kháng lực MiniBand", BranchID = 1 },
                new Schedule { ScheduleID = 13, MemberID = 3, PTID = 2, RoomID = 2, TrainingDate = new DateTime(2026, 10, 30), StartTime = new TimeSpan(16, 0, 0), EndTime = new TimeSpan(17, 30, 0), TrainingType = "Pilates Dẻo Dai & Cột Sống", Status = "Scheduled", Note = "Kéo giãn giảm mỏi cổ vai gáy", BranchID = 1 },

                // Tuần 2 (02/11 - 08/11/2026)
                new Schedule { ScheduleID = 14, MemberID = 2, PTID = 2, RoomID = 1, TrainingDate = new DateTime(2026, 11, 3), StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(18, 30, 0), TrainingType = "Cardio Giảm Cân & Bụng Dưới", Status = "Scheduled", Note = "Tập Tabata 20s-10s và Plank", BranchID = 1 },
                new Schedule { ScheduleID = 15, MemberID = 3, PTID = 2, RoomID = 1, TrainingDate = new DateTime(2026, 11, 5), StartTime = new TimeSpan(16, 0, 0), EndTime = new TimeSpan(17, 30, 0), TrainingType = "Tập Vai Nữ & Lưng Tự Tin", Status = "Scheduled", Note = "Dumbbell Lateral Raise 3kg", BranchID = 1 },
                new Schedule { ScheduleID = 16, MemberID = 2, PTID = 2, RoomID = 2, TrainingDate = new DateTime(2026, 11, 7), StartTime = new TimeSpan(17, 30, 0), EndTime = new TimeSpan(19, 0, 0), TrainingType = "Kéo Giãn Cơ & Thư Giãn Toàn Thân", Status = "Scheduled", Note = "Foam rolling giải phóng điểm căng cơ", BranchID = 1 },

                // Tuần 3 (09/11 - 15/11/2026)
                new Schedule { ScheduleID = 17, MemberID = 3, PTID = 2, RoomID = 1, TrainingDate = new DateTime(2026, 11, 10), StartTime = new TimeSpan(16, 0, 0), EndTime = new TimeSpan(17, 30, 0), TrainingType = "Legs Day & Glute Bridge", Status = "Scheduled", Note = "Rèn luyện cơ đùi sau và mông", BranchID = 1 },
                new Schedule { ScheduleID = 18, MemberID = 2, PTID = 2, RoomID = 1, TrainingDate = new DateTime(2026, 11, 12), StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(18, 30, 0), TrainingType = "Đốt Mỡ Kháng Lực Toàn Thân", Status = "Scheduled", Note = "Tập với tạ Kettlebell và bóng Slam Ball", BranchID = 1 },

                // ========== PT 3 (Nguyễn Đức Minh - Branch 2) ==========
                // Tuần 1 (26/10 - 01/11/2026)
                new Schedule { ScheduleID = 19, MemberID = 1, PTID = 3, RoomID = 3, TrainingDate = new DateTime(2026, 10, 27), StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(9, 30, 0), TrainingType = "Powerlifting Deadlift & Lưng Dày", Status = "Confirmed", Note = "Deadlift kỹ thuật Sumo, mức tạ 100kg", BranchID = 2 },
                new Schedule { ScheduleID = 20, MemberID = 2, PTID = 3, RoomID = 3, TrainingDate = new DateTime(2026, 10, 29), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 30, 0), TrainingType = "Bench Press Chuẩn Thi Đấu", Status = "Scheduled", Note = "Tập kỹ thuật khóa khớp vai và ép xương bả vai", BranchID = 2 },
                new Schedule { ScheduleID = 21, MemberID = 1, PTID = 3, RoomID = 3, TrainingDate = new DateTime(2026, 10, 31), StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(10, 0, 0), TrainingType = "Squat Nặng & Phục Hồi Khớp", Status = "Scheduled", Note = "Back Squat 80kg x 5 reps x 5 sets", BranchID = 2 },

                // Tuần 2 (02/11 - 08/11/2026)
                new Schedule { ScheduleID = 22, MemberID = 3, PTID = 3, RoomID = 3, TrainingDate = new DateTime(2026, 11, 3), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 30, 0), TrainingType = "Kỹ Thuật Cử Tạ Olympic Cơ Bản", Status = "Scheduled", Note = "Clean & Press tạ kỹ thuật 15kg", BranchID = 2 },
                new Schedule { ScheduleID = 23, MemberID = 1, PTID = 3, RoomID = 4, TrainingDate = new DateTime(2026, 11, 6), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 30, 0), TrainingType = "Conditioning & Thể Lực Đỉnh Cao", Status = "Scheduled", Note = "Đẩy xe trượt Prowler Sled & quất dây thừng Battle Rope", BranchID = 2 },

                // Tuần 3 (09/11 - 15/11/2026)
                new Schedule { ScheduleID = 24, MemberID = 2, PTID = 3, RoomID = 3, TrainingDate = new DateTime(2026, 11, 10), StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(10, 0, 0), TrainingType = "Tập Xô Lưng Tăng Độ Dày", Status = "Scheduled", Note = "Barbell Bent Over Row & T-Bar Row", BranchID = 2 },
                new Schedule { ScheduleID = 25, MemberID = 1, PTID = 3, RoomID = 3, TrainingDate = new DateTime(2026, 11, 12), StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 30, 0), TrainingType = "Thử Thách 1RM Bench & Squat", Status = "Scheduled", Note = "Chuẩn bị khởi động dài và bảo hiểm an toàn", BranchID = 2 },
                new Schedule { ScheduleID = 26, MemberID = 3, PTID = 3, RoomID = 4, TrainingDate = new DateTime(2026, 11, 14), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 30, 0), TrainingType = "HIIT & Sức Bền Tim Mạch", Status = "Scheduled", Note = "Máy chèo thuyền kết hợp máy trượt tuyết", BranchID = 2 }
            );
        }
    }
}
