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
                // Theo dõi tiến độ Member 1 (Trần Văn Hùng - VIP Diamond)
                new Progress { ProgressID = 1, MemberID = 1, PTID = 1, RecordDate = new DateTime(2026, 1, 5), Height = 175m, Weight = 78.0m, BodyFat = 22.0m, MuscleMass = 33.5m, BMI = 25.5m, Chest = 96m, Waist = 84m, Hip = 98m, Arm = 34.0m, Thigh = 56m, Note = "Chỉ số ban đầu bắt đầu tập với PT Nam" },
                new Progress { ProgressID = 2, MemberID = 1, PTID = 1, RecordDate = new DateTime(2026, 3, 5), Height = 175m, Weight = 76.0m, BodyFat = 19.5m, MuscleMass = 34.2m, BMI = 24.8m, Chest = 98m, Waist = 81m, Hip = 97m, Arm = 35.5m, Thigh = 57m, Note = "Giảm 2kg mỡ, tăng cơ tay và ngực rõ rệt" },
                new Progress { ProgressID = 3, MemberID = 1, PTID = 1, RecordDate = new DateTime(2026, 6, 5), Height = 175m, Weight = 74.0m, BodyFat = 17.0m, MuscleMass = 35.0m, BMI = 24.2m, Chest = 101m, Waist = 78m, Hip = 96m, Arm = 36.5m, Thigh = 58m, Note = "Thể trạng săn chắc, form siết nét" },
                new Progress { ProgressID = 4, MemberID = 1, PTID = 1, RecordDate = new DateTime(2026, 10, 15), Height = 175m, Weight = 72.5m, BodyFat = 15.5m, MuscleMass = 35.8m, BMI = 23.7m, Chest = 103m, Waist = 76m, Hip = 95m, Arm = 37.5m, Thigh = 58.5m, Note = "Đạt mục tiêu thể hình Lean Muscle mong muốn" },

                // Theo dõi tiến độ Member 2 (Lê Thị Lan - Gold)
                new Progress { ProgressID = 5, MemberID = 2, PTID = 1, RecordDate = new DateTime(2026, 2, 5), Height = 162m, Weight = 58.0m, BodyFat = 28.0m, MuscleMass = 22.0m, BMI = 22.1m, Chest = 86m, Waist = 72m, Hip = 92m, Arm = 26.0m, Thigh = 52m, Note = "Bắt đầu lộ trình giảm mỡ toàn thân" },
                new Progress { ProgressID = 6, MemberID = 2, PTID = 1, RecordDate = new DateTime(2026, 5, 10), Height = 162m, Weight = 54.5m, BodyFat = 24.5m, MuscleMass = 23.0m, BMI = 20.8m, Chest = 85m, Waist = 67m, Hip = 90m, Arm = 25.0m, Thigh = 50m, Note = "Vòng eo giảm 5cm, sức bền tăng tốt" },
                new Progress { ProgressID = 7, MemberID = 2, PTID = 2, RecordDate = new DateTime(2026, 9, 20), Height = 162m, Weight = 52.0m, BodyFat = 21.0m, MuscleMass = 23.8m, BMI = 19.8m, Chest = 85m, Waist = 64m, Hip = 89m, Arm = 24.5m, Thigh = 49m, Note = "Body cân đối, cơ bụng bắt đầu hiện rãnh 11" },

                // Theo dõi tiến độ Member 3 (Phạm Quỳnh Hoa - Silver)
                new Progress { ProgressID = 8, MemberID = 3, PTID = 2, RecordDate = new DateTime(2026, 3, 5), Height = 168m, Weight = 62.0m, BodyFat = 26.0m, MuscleMass = 25.0m, BMI = 22.0m, Chest = 88m, Waist = 70m, Hip = 94m, Arm = 27.0m, Thigh = 53m, Note = "Bắt đầu tập định hình vóc dáng cùng PT Bích" },
                new Progress { ProgressID = 9, MemberID = 3, PTID = 2, RecordDate = new DateTime(2026, 7, 15), Height = 168m, Weight = 58.5m, BodyFat = 22.5m, MuscleMass = 26.2m, BMI = 20.7m, Chest = 89m, Waist = 66m, Hip = 94m, Arm = 26.5m, Thigh = 52m, Note = "Độ dẻo dai và thể lực cải thiện vượt bậc" },

                // Theo dõi tiến độ Member 6
                new Progress { ProgressID = 10, MemberID = 6, PTID = 3, RecordDate = new DateTime(2026, 3, 10), Height = 178m, Weight = 82.0m, BodyFat = 24.0m, MuscleMass = 35.0m, BMI = 25.9m, Chest = 100m, Waist = 88m, Hip = 101m, Arm = 35.0m, Thigh = 59m, Note = "Tập tăng cơ giảm mỡ ban đầu" }
            );
        }
    }
}
