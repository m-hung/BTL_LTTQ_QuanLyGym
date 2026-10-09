using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyGym.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountID", "CreatedAt", "PasswordHash", "Role", "Status", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "Admin", true, "admin" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "PT", true, "pt_nam" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "Member", true, "member_hung" }
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "BranchID", "Address", "BranchName", "CloseTime", "Email", "OpenTime", "Phone", "Status" },
                values: new object[,]
                {
                    { 1, "123 Cầu Giấy, Hà Nội", "QuanLyGym Cầu Giấy", new TimeSpan(0, 22, 0, 0, 0), "caugiay@gym.com", new TimeSpan(0, 6, 0, 0, 0), "0987654321", true },
                    { 2, "456 Xã Đàn, Hà Nội", "QuanLyGym Đống Đa", new TimeSpan(0, 22, 0, 0, 0), "dongda@gym.com", new TimeSpan(0, 6, 0, 0, 0), "0912345678", true }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberID", "AccountID", "Address", "Avatar", "BMI", "BodyFat", "BranchID", "CreatedAt", "DateOfBirth", "Email", "FullName", "Gender", "Height", "MuscleMass", "Phone", "Weight" },
                values: new object[] { 1, 3, "Cầu Giấy, Hà Nội", "", 23m, 18m, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2000, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "hung@gmail.com", "Trần Văn Hùng", "Nam", 172m, 32m, "0977777777", 68m });

            migrationBuilder.InsertData(
                table: "PTs",
                columns: new[] { "PTID", "AccountID", "BranchID", "DateOfBirth", "Email", "Experience", "FullName", "Gender", "Phone", "Specialization", "Status" },
                values: new object[] { 1, 2, 1, new DateTime(1995, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "nam@gym.com", 5, "Nguyễn Văn Nam", "Nam", "0988888888", "Tăng cơ, Giảm mỡ", true });

            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "PackageID", "Benefits", "BranchID", "Description", "Duration", "PackageName", "Price", "Status", "TotalSessions" },
                values: new object[,]
                {
                    { 1, "Tập tự do các phòng", 1, "Gói cơ bản", 30, "Gói Thường 1 Tháng", 500000m, true, 30 },
                    { 2, "Kèm PT 1-1, Nước uống miễn phí", 1, "Gói cao cấp", 90, "Gói VIP 3 Tháng + PT", 3000000m, true, 36 }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomID", "BranchID", "Capacity", "Description", "RoomName", "RoomType", "Status" },
                values: new object[,]
                {
                    { 1, 1, 50, "Khu vực tập tạ tổng hợp", "Phòng Gym Tầng 1", "Gym", true },
                    { 2, 1, 30, "Máy chạy bộ và xe đạp", "Phòng Cardio Tầng 2", "Cardio", true }
                });

            migrationBuilder.InsertData(
                table: "CheckIns",
                columns: new[] { "CheckInID", "BranchID", "CheckInTime", "MemberID", "Note", "PTID", "Status" },
                values: new object[] { 1, 1, new DateTime(2026, 3, 1, 16, 55, 0, 0, DateTimeKind.Unspecified), 1, "Check-in tập cùng PT", 1, "Approved" });

            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "EquipmentID", "Description", "EquipmentCode", "EquipmentName", "Price", "PurchaseDate", "RoomID", "Status" },
                values: new object[,]
                {
                    { 1, "Máy chạy bộ cao cấp", "CB-01", "Máy chạy bộ Technogym", 25000000m, new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Tốt" },
                    { 2, "Khung gánh tạ an toàn", "SQ-01", "Dàn gánh tạ Squat Rack", 15000000m, new DateTime(2025, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Tốt" }
                });

            migrationBuilder.InsertData(
                table: "Memberships",
                columns: new[] { "MembershipID", "BranchID", "CreatedAt", "EndDate", "MemberID", "PackageID", "Price", "RegisterDate", "StartDate", "Status", "UsedSessions" },
                values: new object[] { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 3000000m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 5 });

            migrationBuilder.InsertData(
                table: "Progresses",
                columns: new[] { "ProgressID", "Arm", "BMI", "BodyFat", "Chest", "Height", "Hip", "MemberID", "MuscleMass", "Note", "PTID", "RecordDate", "Thigh", "Waist", "Weight" },
                values: new object[] { 1, 34m, 23m, 18m, 95m, 172m, 92m, 1, 32m, "Chỉ số ban đầu bình thường", 1, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 54m, 78m, 68m });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "ScheduleID", "BranchID", "EndTime", "MemberID", "Note", "PTID", "RoomID", "StartTime", "Status", "TrainingDate", "TrainingType" },
                values: new object[] { 1, 1, new TimeSpan(0, 18, 30, 0, 0), 1, "Hội viên tập tốt, nâng tạ ngực 50kg", 1, 1, new TimeSpan(0, 17, 0, 0, 0), "Completed", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Ngực & Tay Sau" });

            migrationBuilder.InsertData(
                table: "WorkingSchedules",
                columns: new[] { "WorkingScheduleID", "BranchID", "EndTime", "PTID", "StartTime", "Status", "WorkDate" },
                values: new object[] { 1, 1, new TimeSpan(0, 20, 0, 0, 0), 1, new TimeSpan(0, 14, 0, 0, 0), "Đã ca", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "CheckInImages",
                columns: new[] { "ImageID", "CheckInID", "ImagePath", "UploadedAt", "VerificationNote", "VerifiedBy" },
                values: new object[] { 1, 1, "uploads/checkin_001.jpg", new DateTime(2026, 3, 1, 16, 56, 0, 0, DateTimeKind.Unspecified), "Ảnh hợp lệ", 1 });

            migrationBuilder.InsertData(
                table: "EquipmentMaintenances",
                columns: new[] { "MaintenanceID", "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate", "Status" },
                values: new object[] { 1, 500000m, "Bảo dưỡng định kỳ thảm chạy và tra dầu", 1, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàn thành" });

            migrationBuilder.InsertData(
                table: "Incidents",
                columns: new[] { "IncidentID", "Description", "EquipmentID", "Note", "ReportDate", "ReportedBy", "ResolvedDate", "Status" },
                values: new object[] { 1, "Máy chạy bộ phát ra tiếng kêu lạ ở băng tải", 1, "Đã siết lại ốc và căn chỉnh băng tải", new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resolved" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "BranchID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "EquipmentID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Progresses",
                keyColumn: "ProgressID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WorkingSchedules",
                keyColumn: "WorkingScheduleID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "EquipmentID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "BranchID",
                keyValue: 1);
        }
    }
}
