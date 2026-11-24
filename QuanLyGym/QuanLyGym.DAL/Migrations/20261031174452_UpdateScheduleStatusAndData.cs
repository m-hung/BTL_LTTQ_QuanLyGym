using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyGym.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScheduleStatusAndData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalSessions",
                table: "Packages",
                newName: "TotalPTSessions");

            migrationBuilder.RenameColumn(
                name: "UsedSessions",
                table: "Memberships",
                newName: "UsedPTSessions");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 7,
                column: "Username",
                value: "member_khanhhung");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 8,
                column: "Username",
                value: "member_minhhung");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 1,
                columns: new[] { "Email", "FullName" },
                values: new object[] { "nguyenkhanhhung2512@gmail.com", "Nguyễn Khánh Hưng" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 2,
                columns: new[] { "Email", "FullName", "Gender" },
                values: new object[] { "minhhungthai7@gmail.com", "Thái Trần Minh Hưng", "Nam" });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 1,
                columns: new[] { "Benefits", "Description", "TotalPTSessions" },
                values: new object[] { "Tập không giới hạn khung giờ, Sử dụng đầy đủ máy tạ & Cardio, Tủ đồ cá nhân", "Gói tập tự do tiêu chuẩn dành cho hội viên muốn duy trì rèn luyện hàng ngày.", 0 });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 2,
                columns: new[] { "Benefits", "Description", "TotalPTSessions" },
                values: new object[] { "12 buổi tập 1-1 với HLV, Thiết lập lộ trình cá nhân hóa, Kiểm tra chỉ số InBody hàng tháng", "Gói rèn luyện chuyên sâu 3 tháng cùng HLV, phù hợp người mới bắt đầu cần định hướng.", 12 });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 3,
                columns: new[] { "Benefits", "Description", "Duration", "TotalPTSessions" },
                values: new object[] { "24 buổi tập 1-1 với HLV, Tư vấn dinh dưỡng chuyên sâu, Đo InBody định kỳ, Sử dụng phòng xông hơi Sauna", "Gói nâng cao toàn diện giúp cải thiện thể hình nhanh chóng và thư giãn sau giờ tập.", 90, 24 });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 4,
                columns: new[] { "Benefits", "Description", "Duration", "TotalPTSessions" },
                values: new object[] { "36 buổi tập 1-1 với HLV cao cấp, Full dịch vụ Sauna & VIP Locker, Miễn phí khăn tập & nước uống mỗi buổi", "Gói tập thượng lưu cao cấp nhất, cam kết kết quả và trải nghiệm dịch vụ chuẩn 5 sao.", 90, 36 });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 1,
                column: "Status",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 2,
                columns: new[] { "Note", "Status" },
                values: new object[] { "Đốt calo cường độ cao, đã hoàn thành", "Completed" });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 3,
                column: "Status",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 4,
                column: "Status",
                value: "Confirmed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 5,
                column: "Status",
                value: "Confirmed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 6,
                column: "Status",
                value: "Confirmed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 7,
                column: "Status",
                value: "Confirmed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 9,
                columns: new[] { "Note", "Status" },
                values: new object[] { "Hội viên báo bận lịch tác nghiệp công tác", "Cancelled" });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 11,
                column: "Status",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 12,
                column: "Status",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 13,
                column: "Status",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 14,
                column: "Status",
                value: "Confirmed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 15,
                column: "Status",
                value: "Confirmed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 16,
                column: "Status",
                value: "Confirmed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 19,
                column: "Status",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 20,
                column: "Status",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 21,
                column: "Status",
                value: "Confirmed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 22,
                column: "Status",
                value: "Confirmed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 23,
                column: "Status",
                value: "Confirmed");

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "ScheduleID", "BranchID", "EndTime", "MemberID", "Note", "PTID", "RoomID", "StartTime", "Status", "TrainingDate", "TrainingType" },
                values: new object[,]
                {
                    { 27, 1, new TimeSpan(0, 10, 0, 0, 0), 1, "Tập trung ép ngực với tạ đơn", 1, 1, new TimeSpan(0, 8, 30, 0, 0), "Scheduled", new DateTime(2026, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Ngực Chuyên Sâu & Flyes" },
                    { 28, 1, new TimeSpan(0, 15, 30, 0, 0), 2, "Chú ý kiểm soát nhịp tim", 1, 2, new TimeSpan(0, 14, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "HIIT Tabata & Bụng Dưới" },
                    { 29, 1, new TimeSpan(0, 10, 30, 0, 0), 1, "Kiểm tra kỹ thuật siết lưng", 1, 1, new TimeSpan(0, 9, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Lưng Dày & Deadlift" },
                    { 30, 1, new TimeSpan(0, 10, 0, 0, 0), 1, "Hội viên mới gửi yêu cầu đặt lịch", 1, 1, new TimeSpan(0, 8, 30, 0, 0), "Pending", new DateTime(2026, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Vai & Tay Sau (Super-set)" },
                    { 31, 1, new TimeSpan(0, 15, 30, 0, 0), 3, "Leg Press & Leg Extension", 1, 1, new TimeSpan(0, 14, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Chân & Đùi Trước" },
                    { 32, 1, new TimeSpan(0, 10, 30, 0, 0), 1, "Đo lại các vòng và chỉ số mỡ", 1, 1, new TimeSpan(0, 9, 0, 0, 0), "Pending", new DateTime(2026, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tổng Kết Tiến Độ & Giãn Cơ Sauna" },
                    { 33, 1, new TimeSpan(0, 17, 30, 0, 0), 3, "Sumo Squat & Cable Abduction", 2, 1, new TimeSpan(0, 16, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Mông & Đùi Trong Nữ" },
                    { 34, 1, new TimeSpan(0, 18, 30, 0, 0), 2, "Chú ý nhịp thở khi gập bụng", 2, 2, new TimeSpan(0, 17, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Cốt Lõi (Core & Abs)" },
                    { 35, 1, new TimeSpan(0, 17, 30, 0, 0), 3, "Đang chờ HLV xác nhận ca", 2, 1, new TimeSpan(0, 16, 0, 0, 0), "Pending", new DateTime(2026, 11, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pilates Thể Lực & Dáng Đẹp" },
                    { 36, 1, new TimeSpan(0, 18, 30, 0, 0), 2, "Thư giãn các cơ sau chu kỳ tập nặng", 2, 1, new TimeSpan(0, 17, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kéo Giãn Sâu & Yoga Gym" },
                    { 37, 2, new TimeSpan(0, 10, 0, 0, 0), 1, "Nâng mức tạ thử nghiệm 110kg", 3, 3, new TimeSpan(0, 8, 30, 0, 0), "Scheduled", new DateTime(2026, 11, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deadlift Sumo & Kỹ Thuật Siết Lưng" },
                    { 38, 2, new TimeSpan(0, 11, 30, 0, 0), 2, "Tập vai đẩy thanh đòn đứng", 3, 4, new TimeSpan(0, 10, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Overhead Press & Khớp Vai" },
                    { 39, 2, new TimeSpan(0, 10, 0, 0, 0), 1, "Lịch đăng ký mới cuối tháng", 3, 3, new TimeSpan(0, 8, 30, 0, 0), "Pending", new DateTime(2026, 11, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Powerlifting Complete Day" },
                    { 40, 2, new TimeSpan(0, 10, 30, 0, 0), 3, "Bài tập giải mỏi toàn thân", 3, 4, new TimeSpan(0, 9, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cardio Thể Lực & Phục Hồi Co Cơ" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 40);

            migrationBuilder.RenameColumn(
                name: "TotalPTSessions",
                table: "Packages",
                newName: "TotalSessions");

            migrationBuilder.RenameColumn(
                name: "UsedPTSessions",
                table: "Memberships",
                newName: "UsedSessions");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 7,
                column: "Username",
                value: "member_hung");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 8,
                column: "Username",
                value: "member_lan");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 1,
                columns: new[] { "Email", "FullName" },
                values: new object[] { "hung.tran@gmail.com", "Trần Văn Hùng" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 2,
                columns: new[] { "Email", "FullName", "Gender" },
                values: new object[] { "lan.le@gmail.com", "Lê Thị Lan", "Nữ" });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 1,
                columns: new[] { "Benefits", "Description", "TotalSessions" },
                values: new object[] { "Tập tự do không giới hạn các khung giờ, tủ đồ cá nhân", "Gói tập tự do tiêu chuẩn", 30 });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 2,
                columns: new[] { "Benefits", "Description", "TotalSessions" },
                values: new object[] { "Kèm PT 1-1 (12 buổi PT), kiểm tra chỉ số InBody định kỳ", "Gói tập rèn luyện chuyên sâu cùng HLV cá nhân", 24 });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 3,
                columns: new[] { "Benefits", "Description", "Duration", "TotalSessions" },
                values: new object[] { "Kèm PT 1-1 (36 buổi PT), đo InBody định kỳ, phòng xông hơi Sauna", "Gói tập Gold cao cấp toàn diện", 180, 48 });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 4,
                columns: new[] { "Benefits", "Description", "Duration", "TotalSessions" },
                values: new object[] { "Kèm PT 1-1 không giới hạn, full dịch vụ VIP Sauna, khăn tập & locker riêng", "Gói tập thượng lưu VIP Diamond đẳng cấp nhất", 365, 100 });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 1,
                column: "Status",
                value: "Confirmed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 2,
                columns: new[] { "Note", "Status" },
                values: new object[] { "Đốt calo cường độ cao, theo dõi nhịp tim", "Confirmed" });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 3,
                column: "Status",
                value: "Scheduled");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 4,
                column: "Status",
                value: "Scheduled");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 5,
                column: "Status",
                value: "Scheduled");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 6,
                column: "Status",
                value: "Scheduled");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 7,
                column: "Status",
                value: "Scheduled");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 9,
                columns: new[] { "Note", "Status" },
                values: new object[] { "Dùng đai hỗ trợ lưng", "Scheduled" });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 11,
                column: "Status",
                value: "Confirmed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 12,
                column: "Status",
                value: "Confirmed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 13,
                column: "Status",
                value: "Scheduled");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 14,
                column: "Status",
                value: "Scheduled");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 15,
                column: "Status",
                value: "Scheduled");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 16,
                column: "Status",
                value: "Scheduled");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 19,
                column: "Status",
                value: "Confirmed");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 20,
                column: "Status",
                value: "Scheduled");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 21,
                column: "Status",
                value: "Scheduled");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 22,
                column: "Status",
                value: "Scheduled");

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 23,
                column: "Status",
                value: "Scheduled");
        }
    }
}
