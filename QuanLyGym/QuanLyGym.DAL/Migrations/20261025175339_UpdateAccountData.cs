using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyGym.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccountData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkingSchedules");

            migrationBuilder.DropColumn(
                name: "BMI",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "BodyFat",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MuscleMass",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Members");

            migrationBuilder.AddColumn<bool>(
                name: "IsPTPackage",
                table: "Packages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Staffs",
                columns: table => new
                {
                    StaffID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WorkShift = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BaseSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: true),
                    PTID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.StaffID);
                    table.ForeignKey(
                        name: "FK_Staffs_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "BranchID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staffs_PTs_PTID",
                        column: x => x.PTID,
                        principalTable: "PTs",
                        principalColumn: "PTID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.Sql("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 3,
                columns: new[] { "Role", "Username" },
                values: new object[] { "PT", "pt_bichtran" });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountID", "CreatedAt", "PasswordHash", "Role", "Status", "Username" },
                values: new object[,]
                {
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "PT", true, "pt_ducminh" },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "PT", true, "pt_thaophan" },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "PT", true, "pt_hoanglong" },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "Member", true, "member_hung" },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "Member", true, "member_lan" },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "Member", true, "member_hoa" },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "Member", true, "member_dung" },
                    { 11, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "Member", true, "member_phuong" },
                    { 12, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "Member", true, "member_tuan" },
                    { 13, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "Member", true, "member_mai" },
                    { 14, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "Member", true, "member_vy" },
                    { 15, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "Member", true, "member_khoa" },
                    { 16, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "123456", "Member", true, "member_tram" }
                });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchID",
                keyValue: 1,
                columns: new[] { "Address", "BranchName", "Email", "Phone" },
                values: new object[] { "123 Nguyễn Thị Minh Khai, Phường Bến Thành, Quận 1, TP. HCM", "QuanLyGym Quận 1", "quan1@quanlygym.vn", "02838221122" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchID",
                keyValue: 2,
                columns: new[] { "Address", "BranchName", "Email", "Phone" },
                values: new object[] { "456 Điện Biên Phủ, Phường 25, Quận Bình Thạnh, TP. HCM", "QuanLyGym Bình Thạnh", "binhthanh@quanlygym.vn", "02838332233" });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "BranchID", "Address", "BranchName", "CloseTime", "Email", "OpenTime", "Phone", "Status" },
                values: new object[] { 3, "789 Nguyễn Lương Bằng, Phường Tân Phú, Quận 7, TP. HCM", "QuanLyGym Quận 7", new TimeSpan(0, 22, 0, 0, 0), "quan7@quanlygym.vn", new TimeSpan(0, 6, 0, 0, 0), "02838443344", true });

            migrationBuilder.UpdateData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 1,
                columns: new[] { "UploadedAt", "VerificationNote" },
                values: new object[] { new DateTime(2026, 10, 20, 8, 26, 0, 0, DateTimeKind.Unspecified), "Ảnh khuôn mặt rõ nét, khớp hồ sơ" });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 1,
                columns: new[] { "CheckInTime", "Note" },
                values: new object[] { new DateTime(2026, 10, 20, 8, 25, 0, 0, DateTimeKind.Unspecified), "Check-in tập buổi ngực cùng PT Nam" });

            migrationBuilder.InsertData(
                table: "CheckIns",
                columns: new[] { "CheckInID", "BranchID", "CheckInTime", "MemberID", "Note", "PTID", "Status" },
                values: new object[,]
                {
                    { 4, 1, new DateTime(2026, 10, 22, 8, 30, 0, 0, DateTimeKind.Unspecified), 1, "Check-in tập Lưng cùng PT Nam", 1, "Approved" },
                    { 15, 1, new DateTime(2026, 10, 26, 8, 20, 0, 0, DateTimeKind.Unspecified), 1, "Check-in buổi tập đầu tuần cùng PT Nam", 1, "Approved" }
                });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 1,
                column: "Description",
                value: "Bảo dưỡng định kỳ thảm chạy và tra dầu băng chuyền");

            migrationBuilder.InsertData(
                table: "EquipmentMaintenances",
                columns: new[] { "MaintenanceID", "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate", "Status" },
                values: new object[,]
                {
                    { 2, 300000m, "Siết chặt ốc liên kết và kiểm tra thanh an toàn J-Hooks", 2, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàn thành" },
                    { 8, 650000m, "Kiểm tra động cơ 4.0 HP và căn chỉnh độ lệch băng tải", 1, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàn thành" }
                });

            migrationBuilder.UpdateData(
                table: "Equipments",
                keyColumn: "EquipmentID",
                keyValue: 1,
                columns: new[] { "Description", "EquipmentCode", "EquipmentName", "Price" },
                values: new object[] { "Máy chạy bộ cao cấp màn hình cảm ứng", "TM-01", "Máy chạy bộ Technogym Excite Live", 35000000m });

            migrationBuilder.UpdateData(
                table: "Equipments",
                keyColumn: "EquipmentID",
                keyValue: 2,
                columns: new[] { "Description", "EquipmentName", "Price" },
                values: new object[] { "Khung gánh tạ an toàn chịu tải 500kg", "Dàn gánh tạ Squat Rack Olympic", 18000000m });

            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "EquipmentID", "Description", "EquipmentCode", "EquipmentName", "Price", "PurchaseDate", "RoomID", "Status" },
                values: new object[,]
                {
                    { 3, "Ghế dốc tập ngực trên chuẩn thi đấu", "BP-01", "Ghế đẩy ngực Incline Bench Press", 12000000m, new DateTime(2025, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Tốt" },
                    { 4, "Máy khối đôi kéo xô và chèo xô lưng", "LP-01", "Máy kéo xô Lat Pulldown & Row", 28000000m, new DateTime(2025, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Tốt" },
                    { 5, "Xe đạp tĩnh kháng lực từ trường", "BK-01", "Xe đạp tập Cardio LifeFitness", 16000000m, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Tốt" }
                });

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 1,
                columns: new[] { "Description", "Note" },
                values: new object[] { "Máy chạy bộ phát ra tiếng kêu lạ ở băng tải khi đạt tốc độ trên 12km/h", "Đã siết lại ốc và căn chỉnh tâm băng chuyền" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 1,
                columns: new[] { "AccountID", "Address", "Email" },
                values: new object[] { 7, "15 Lê Lợi, Quận 1, TP. HCM", "hung.tran@gmail.com" });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 1,
                columns: new[] { "Benefits", "Description", "IsPTPackage", "PackageName" },
                values: new object[] { "Tập tự do không giới hạn các khung giờ, tủ đồ cá nhân", "Gói tập tự do tiêu chuẩn", false, "Classic" });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 2,
                columns: new[] { "Benefits", "Description", "IsPTPackage", "PackageName", "Price", "TotalSessions" },
                values: new object[] { "Kèm PT 1-1 (12 buổi PT), kiểm tra chỉ số InBody định kỳ", "Gói tập rèn luyện chuyên sâu cùng HLV cá nhân", true, "Silver", 2800000m, 24 });

            migrationBuilder.InsertData(
                table: "Packages",
                columns: new[] { "PackageID", "Benefits", "BranchID", "Description", "Duration", "IsPTPackage", "PackageName", "Price", "Status", "TotalSessions" },
                values: new object[,]
                {
                    { 3, "Kèm PT 1-1 (36 buổi PT), đo InBody định kỳ, phòng xông hơi Sauna", 1, "Gói tập Gold cao cấp toàn diện", 180, true, "Gold", 5800000m, true, 48 },
                    { 4, "Kèm PT 1-1 không giới hạn, full dịch vụ VIP Sauna, khăn tập & locker riêng", 1, "Gói tập thượng lưu VIP Diamond đẳng cấp nhất", 365, true, "VIP Diamond", 12500000m, true, 100 }
                });

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 1,
                columns: new[] { "EndDate", "PackageID", "Price", "UsedSessions" },
                values: new object[] { new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 12500000m, 25 });

            migrationBuilder.UpdateData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 1,
                columns: new[] { "Email", "Experience", "Specialization" },
                values: new object[] { "nam.nguyen@quanlygym.vn", 6, "Tăng cơ Hypertrophy & Dinh dưỡng" });

            migrationBuilder.InsertData(
                table: "PTs",
                columns: new[] { "PTID", "AccountID", "BranchID", "DateOfBirth", "Email", "Experience", "FullName", "Gender", "Phone", "Specialization", "Status" },
                values: new object[] { 2, 3, 1, new DateTime(1997, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "bich.tran@quanlygym.vn", 5, "Trần Thị Bích", "Nữ", "0943210987", "Giảm mỡ Siết cơ & Pilates/Yoga", true });

            migrationBuilder.UpdateData(
                table: "Progresses",
                keyColumn: "ProgressID",
                keyValue: 1,
                columns: new[] { "BMI", "BodyFat", "Chest", "Height", "Hip", "MuscleMass", "Note", "RecordDate", "Thigh", "Waist", "Weight" },
                values: new object[] { 25.5m, 22.0m, 96m, 175m, 98m, 33.5m, "Chỉ số ban đầu bắt đầu tập với PT Nam", new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 56m, 84m, 78.0m });

            migrationBuilder.InsertData(
                table: "Progresses",
                columns: new[] { "ProgressID", "Arm", "BMI", "BodyFat", "Chest", "Height", "Hip", "MemberID", "MuscleMass", "Note", "PTID", "RecordDate", "Thigh", "Waist", "Weight" },
                values: new object[,]
                {
                    { 2, 35.5m, 24.8m, 19.5m, 98m, 175m, 97m, 1, 34.2m, "Giảm 2kg mỡ, tăng cơ tay và ngực rõ rệt", 1, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 57m, 81m, 76.0m },
                    { 3, 36.5m, 24.2m, 17.0m, 101m, 175m, 96m, 1, 35.0m, "Thể trạng săn chắc, form siết nét", 1, new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 58m, 78m, 74.0m },
                    { 4, 37.5m, 23.7m, 15.5m, 103m, 175m, 95m, 1, 35.8m, "Đạt mục tiêu thể hình Lean Muscle mong muốn", 1, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 58.5m, 76m, 72.5m }
                });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 1,
                columns: new[] { "Capacity", "Description", "RoomName" },
                values: new object[] { 60, "Khu vực tập tạ tổng hợp và máy khối", "Phòng Gym & Tạ Tầng 1" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 2,
                columns: new[] { "Capacity", "Description" },
                values: new object[] { 40, "Máy chạy bộ, xe đạp và máy trượt tuyết" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomID", "BranchID", "Capacity", "Description", "RoomName", "RoomType", "Status" },
                values: new object[,]
                {
                    { 3, 2, 50, "Khu tạ tự do và dàn Squat/Bench", "Phòng Gym Free Weight Tầng 1", "Gym", true },
                    { 4, 2, 35, "Khu chức năng và tim mạch", "Phòng Cardio & Functional Tầng 2", "Functional", true }
                });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 1,
                columns: new[] { "EndTime", "Note", "StartTime", "Status", "TrainingDate", "TrainingType" },
                values: new object[] { new TimeSpan(0, 10, 0, 0, 0), "Hội viên chuẩn bị thể lực tốt, mức tạ ngực 60kg", new TimeSpan(0, 8, 30, 0, 0), "Confirmed", new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Ngực & Tay Sau (Hypertrophy)" });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "ScheduleID", "BranchID", "EndTime", "MemberID", "Note", "PTID", "RoomID", "StartTime", "Status", "TrainingDate", "TrainingType" },
                values: new object[,]
                {
                    { 3, 1, new TimeSpan(0, 10, 30, 0, 0), 1, "Tập trung bài Lat Pulldown & Seated Row", 1, 1, new TimeSpan(0, 9, 0, 0, 0), "Scheduled", new DateTime(2026, 10, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Lưng Xô & Tay Trước" },
                    { 5, 1, new TimeSpan(0, 10, 0, 0, 0), 1, "Đẩy tạ vai 20kg mỗi bên", 1, 1, new TimeSpan(0, 8, 30, 0, 0), "Scheduled", new DateTime(2026, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Vai & Bụng Core" },
                    { 7, 1, new TimeSpan(0, 10, 30, 0, 0), 1, "Tập liên hoàn toàn thân", 1, 1, new TimeSpan(0, 9, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Full Body Functional Training" },
                    { 8, 1, new TimeSpan(0, 10, 0, 0, 0), 1, "Đánh giá sức mạnh sau chu kỳ 1 tháng", 1, 1, new TimeSpan(0, 8, 30, 0, 0), "Scheduled", new DateTime(2026, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kiểm Tra Tiến Độ & Strength Test" }
                });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "StaffID", "BaseSalary", "BranchID", "DateOfBirth", "Email", "FullName", "Gender", "PTID", "Phone", "Role", "Status", "WorkShift" },
                values: new object[,]
                {
                    { 1, 16000000m, 1, new DateTime(1995, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "nam.nguyen@quanlygym.vn", "Nguyễn Văn Nam", "Nam", 1, "0988888888", "PT", true, "Ca cố định: 08:00 - 16:00 (Thứ 2 - CN)" },
                    { 6, 9000000m, 1, new DateTime(2000, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "lananh.tn@quanlygym.vn", "Nguyễn Thị Lan Anh", "Nữ", null, "0912345679", "Thu ngân", true, "Ca sáng (06:00 - 14:00)" },
                    { 7, 9000000m, 2, new DateTime(1999, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "quanghuy.tn@quanlygym.vn", "Trần Quang Huy", "Nam", null, "0934567891", "Thu ngân", true, "Ca chiều (14:00 - 22:00)" },
                    { 9, 7500000m, 1, new DateTime(1980, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "maile.tv@quanlygym.vn", "Lê Thị Mai", "Nữ", null, "0905111222", "Tạp vụ", true, "Ca sáng (06:00 - 14:00)" },
                    { 10, 7500000m, 2, new DateTime(1982, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "saunguyen.tv@quanlygym.vn", "Nguyễn Thị Sáu", "Nữ", null, "0932333444", "Tạp vụ", true, "Ca chiều (14:00 - 22:00)" },
                    { 12, 8500000m, 1, new DateTime(1978, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "dungbui.bv@quanlygym.vn", "Bùi Văn Dũng", "Nam", null, "0911777888", "Bảo vệ", true, "Ca ngày (06:00 - 18:00)" },
                    { 13, 9000000m, 2, new DateTime(1983, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "haingo.bv@quanlygym.vn", "Ngô Quang Hải", "Nam", null, "0944888999", "Bảo vệ", true, "Ca đêm (18:00 - 06:00)" }
                });

            migrationBuilder.InsertData(
                table: "CheckInImages",
                columns: new[] { "ImageID", "CheckInID", "ImagePath", "UploadedAt", "VerificationNote", "VerifiedBy" },
                values: new object[,]
                {
                    { 4, 4, "uploads/checkin_004.jpg", new DateTime(2026, 10, 22, 8, 31, 0, 0, DateTimeKind.Unspecified), "Hợp lệ", 1 },
                    { 15, 15, "uploads/checkin_015.jpg", new DateTime(2026, 10, 26, 8, 21, 0, 0, DateTimeKind.Unspecified), "Ảnh selfie check-in tại quầy Q1", 1 }
                });

            migrationBuilder.InsertData(
                table: "EquipmentMaintenances",
                columns: new[] { "MaintenanceID", "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate", "Status" },
                values: new object[,]
                {
                    { 3, 750000m, "Thay dây cáp thép bọc nylon và bôi trơn ròng rọc", 4, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàn thành" },
                    { 4, 600000m, "Thay bàn đạp dây đai và cảm biến nhịp tim", 5, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàn thành" }
                });

            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "EquipmentID", "Description", "EquipmentCode", "EquipmentName", "Price", "PurchaseDate", "RoomID", "Status" },
                values: new object[,]
                {
                    { 6, "Máy đạp chân cơ đùi chịu tải trọng lớn", "LP-02", "Máy đạp đùi Leg Press 45 độ", 32000000m, new DateTime(2025, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Tốt" },
                    { 7, "Dàn cáp đôi đa hướng ròng rọc nhôm", "CC-01", "Dàn cáp đôi Dual Cable Crossover", 42000000m, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Bảo trì" },
                    { 8, "Máy tập toàn thân bảo vệ khớp gối", "EL-01", "Máy trượt tuyết Elliptical Matrix", 26000000m, new DateTime(2025, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Tốt" }
                });

            migrationBuilder.InsertData(
                table: "Incidents",
                columns: new[] { "IncidentID", "Description", "EquipmentID", "Note", "ReportDate", "ReportedBy", "ResolvedDate", "Status" },
                values: new object[,]
                {
                    { 2, "Đệm da ghế đẩy ngực bị rách nhẹ ở góc mép", 3, "Đã bọc lại lớp da simili cao cấp mới", new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resolved" },
                    { 3, "Cáp kéo xô có hiện tượng sờn vỏ nhựa bảo vệ", 4, "Đã thay cáp mới an toàn tuyệt đối", new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resolved" },
                    { 4, "Màn hình xe đạp không nhận tín hiệu nhịp tim từ tay cầm", 5, "Đã nối lại dây cảm biến bị tuột bên trong", new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2026, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resolved" }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberID", "AccountID", "Address", "Avatar", "BranchID", "CreatedAt", "DateOfBirth", "Email", "FullName", "Gender", "Phone" },
                values: new object[,]
                {
                    { 2, 8, "88 Pasteur, Quận 1, TP. HCM", "", 1, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "lan.le@gmail.com", "Lê Thị Lan", "Nữ", "0912345678" },
                    { 3, 9, "12 Nguyễn Huệ, Quận 1, TP. HCM", "", 1, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2001, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "hoa.pham@gmail.com", "Phạm Quỳnh Hoa", "Nữ", "0934567890" },
                    { 4, 10, "204 Hai Bà Trưng, Quận 1, TP. HCM", "", 1, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1996, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "dung.hoang@gmail.com", "Hoàng Tiến Dũng", "Nam", "0945678901" },
                    { 5, 11, "55 Nam Kỳ Khởi Nghĩa, Quận 1, TP. HCM", "", 1, new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1998, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "phuong.do@gmail.com", "Đỗ Thu Phương", "Nữ", "0956789012" },
                    { 6, 12, "32 Bạch Đằng, Quận Bình Thạnh, TP. HCM", "", 2, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1995, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "tuan.ngo@gmail.com", "Ngô Anh Tuấn", "Nam", "0967890123" },
                    { 7, 13, "150 Nơ Trang Long, Quận Bình Thạnh, TP. HCM", "", 2, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2002, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "mai.bui@gmail.com", "Bùi Tuyết Mai", "Nữ", "0978901234" },
                    { 8, 14, "82 Xô Viết Nghệ Tĩnh, Quận Bình Thạnh, TP. HCM", "", 2, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1997, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "vy.trinh@gmail.com", "Trịnh Thảo Vy", "Nữ", "0989012345" },
                    { 9, 15, "100 Huỳnh Tấn Phát, Quận 7, TP. HCM", "", 3, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1993, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "khoa.dang@gmail.com", "Đặng Đăng Khoa", "Nam", "0990123456" },
                    { 10, 16, "45 Nguyễn Thị Thập, Quận 7, TP. HCM", "", 3, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2000, 6, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "tram.vo@gmail.com", "Võ Bảo Trâm", "Nữ", "0901234567" }
                });

            migrationBuilder.InsertData(
                table: "PTs",
                columns: new[] { "PTID", "AccountID", "BranchID", "DateOfBirth", "Email", "Experience", "FullName", "Gender", "Phone", "Specialization", "Status" },
                values: new object[,]
                {
                    { 3, 4, 2, new DateTime(1994, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "minh.duc@quanlygym.vn", 7, "Nguyễn Đức Minh", "Nam", "0987543210", "Powerlifting & Thể hình chuyên nghiệp", true },
                    { 4, 5, 2, new DateTime(1998, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "thao.phan@quanlygym.vn", 4, "Phan Thị Thảo", "Nữ", "0921098765", "Cardio HIIT & Định hình vóc dáng", true },
                    { 5, 6, 3, new DateTime(1996, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "long.vu@quanlygym.vn", 5, "Vũ Hoàng Long", "Nam", "0909876543", "Phục hồi chức năng & Vận động", true }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomID", "BranchID", "Capacity", "Description", "RoomName", "RoomType", "Status" },
                values: new object[,]
                {
                    { 5, 3, 45, "Khu tập thể lực và tạ khối", "Phòng Strength & Conditioning", "Gym", true },
                    { 6, 3, 30, "Khu cardio và tập studio", "Phòng Studio & Cardio Tầng 2", "Cardio", true }
                });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "StaffID", "BaseSalary", "BranchID", "DateOfBirth", "Email", "FullName", "Gender", "PTID", "Phone", "Role", "Status", "WorkShift" },
                values: new object[,]
                {
                    { 2, 15500000m, 1, new DateTime(1997, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "bich.tran@quanlygym.vn", "Trần Thị Bích", "Nữ", 2, "0943210987", "PT", true, "Ca cố định: 14:00 - 22:00 (Thứ 2 - CN)" },
                    { 8, 9500000m, 3, new DateTime(2001, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "minhchau.tn@quanlygym.vn", "Đặng Minh Châu", "Nữ", null, "0966222333", "Thu ngân", true, "Ca xoay (08:00 - 17:00)" },
                    { 11, 7500000m, 3, new DateTime(1985, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "ngahoang.tv@quanlygym.vn", "Hoàng Thị Nga", "Nữ", null, "0988444555", "Tạp vụ", true, "Ca sáng (06:00 - 14:00)" },
                    { 14, 8500000m, 3, new DateTime(1986, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "tuando.bv@quanlygym.vn", "Đỗ Văn Tuấn", "Nam", null, "0977999000", "Bảo vệ", true, "Ca ngày (06:00 - 18:00)" }
                });

            migrationBuilder.InsertData(
                table: "CheckIns",
                columns: new[] { "CheckInID", "BranchID", "CheckInTime", "MemberID", "Note", "PTID", "Status" },
                values: new object[,]
                {
                    { 2, 1, new DateTime(2026, 10, 21, 13, 50, 0, 0, DateTimeKind.Unspecified), 2, "Check-in tập Cardio cùng PT Nam", 1, "Approved" },
                    { 3, 1, new DateTime(2026, 10, 21, 15, 55, 0, 0, DateTimeKind.Unspecified), 3, "Check-in tập Pilates cùng PT Bích", 2, "Approved" },
                    { 5, 1, new DateTime(2026, 10, 22, 17, 15, 0, 0, DateTimeKind.Unspecified), 4, "Hội viên Classic tập tự do chiều", null, "Approved" },
                    { 6, 1, new DateTime(2026, 10, 23, 7, 5, 0, 0, DateTimeKind.Unspecified), 5, "Hội viên Classic tập tự do sáng", null, "Approved" },
                    { 7, 1, new DateTime(2026, 10, 23, 17, 25, 0, 0, DateTimeKind.Unspecified), 2, "Check-in tập Mông & Đùi cùng PT Bích", 2, "Approved" },
                    { 8, 1, new DateTime(2026, 10, 24, 14, 55, 0, 0, DateTimeKind.Unspecified), 3, "Check-in tập Chân cùng PT Nam", 1, "Approved" },
                    { 9, 2, new DateTime(2026, 10, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), 6, "Hội viên Classic tập tự do chi nhánh Bình Thạnh", null, "Approved" },
                    { 10, 2, new DateTime(2026, 10, 25, 7, 55, 0, 0, DateTimeKind.Unspecified), 1, "Check-in tập Powerlifting cùng PT Minh", 3, "Approved" },
                    { 11, 2, new DateTime(2026, 10, 25, 16, 30, 0, 0, DateTimeKind.Unspecified), 7, "Hội viên Classic tập Cardio tự do", null, "Approved" },
                    { 12, 2, new DateTime(2026, 10, 25, 19, 10, 0, 0, DateTimeKind.Unspecified), 8, "Hội viên Classic tập tối", null, "Approved" },
                    { 13, 3, new DateTime(2026, 10, 25, 6, 45, 0, 0, DateTimeKind.Unspecified), 9, "Hội viên Classic tập tự do chi nhánh Q7", null, "Approved" },
                    { 14, 3, new DateTime(2026, 10, 25, 18, 20, 0, 0, DateTimeKind.Unspecified), 10, "Hội viên Classic tập tối chi nhánh Q7", null, "Approved" }
                });

            migrationBuilder.InsertData(
                table: "EquipmentMaintenances",
                columns: new[] { "MaintenanceID", "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate", "Status" },
                values: new object[,]
                {
                    { 5, 450000m, "Tra mỡ vòng bi ray trượt máy đạp đùi", 6, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàn thành" },
                    { 6, 800000m, "Căn chỉnh lại hệ thống đối trọng tạ cáp bên phải", 7, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàn thành" },
                    { 7, 900000m, "Bảo trì mô-tơ kháng lực máy trượt tuyết", 8, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàn thành" },
                    { 9, 400000m, "Thay thế chốt tạ selector pin bị kẹt và bảo dưỡng bánh xe dẫn hướng", 7, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đang xử lý" }
                });

            migrationBuilder.InsertData(
                table: "Equipments",
                columns: new[] { "EquipmentID", "Description", "EquipmentCode", "EquipmentName", "Price", "PurchaseDate", "RoomID", "Status" },
                values: new object[,]
                {
                    { 9, "Dàn tạ bọc urethene cao cấp 20 đôi", "DB-01", "Bộ tạ tay Dumbbell 5-40kg kèm giá", 45000000m, new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Tốt" },
                    { 10, "Máy chèo thuyền cản nước bằng gỗ sồi", "RW-01", "Máy chèo thuyền WaterRower", 22000000m, new DateTime(2025, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "Tốt" }
                });

            migrationBuilder.InsertData(
                table: "Incidents",
                columns: new[] { "IncidentID", "Description", "EquipmentID", "Note", "ReportDate", "ReportedBy", "ResolvedDate", "Status" },
                values: new object[,]
                {
                    { 5, "Khóa chốt an toàn máy Leg Press bị rít khó gạt", 6, "Đã tra dầu bôi trơn và vệ sinh trục chốt", new DateTime(2026, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resolved" },
                    { 6, "Ròng rọc dàn cáp đôi xoay không đều gây cọ xát", 7, "Đã thay vòng bi ròng rọc bên trái", new DateTime(2026, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(2026, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resolved" },
                    { 7, "Bàn đạp máy trượt tuyết phát ra tiếng cót két khi đạp nhanh", 8, "Đã siết ốc trục cam và tra mỡ chịu nhiệt", new DateTime(2026, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resolved" },
                    { 9, "Chốt chọn mức tạ bị cong nhẹ do hội viên thả tạ rơi tự do", 7, "Đang chờ nhà cung cấp giao chốt tạ mới thay thế", new DateTime(2026, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "In Progress" }
                });

            migrationBuilder.InsertData(
                table: "Memberships",
                columns: new[] { "MembershipID", "BranchID", "CreatedAt", "EndDate", "MemberID", "PackageID", "Price", "RegisterDate", "StartDate", "Status", "UsedSessions" },
                values: new object[,]
                {
                    { 2, 1, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, 5800000m, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 18 },
                    { 3, 1, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, 2800000m, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 10 },
                    { 4, 1, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, 500000m, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 14 },
                    { 5, 1, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1, 500000m, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 12 },
                    { 6, 2, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 1, 500000m, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 8 },
                    { 7, 2, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 1, 500000m, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 6 },
                    { 8, 2, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 1, 500000m, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 15 },
                    { 9, 3, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 1, 500000m, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 9 },
                    { 10, 3, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 1, 500000m, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 5 }
                });

            migrationBuilder.InsertData(
                table: "Progresses",
                columns: new[] { "ProgressID", "Arm", "BMI", "BodyFat", "Chest", "Height", "Hip", "MemberID", "MuscleMass", "Note", "PTID", "RecordDate", "Thigh", "Waist", "Weight" },
                values: new object[,]
                {
                    { 5, 26.0m, 22.1m, 28.0m, 86m, 162m, 92m, 2, 22.0m, "Bắt đầu lộ trình giảm mỡ toàn thân", 1, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 52m, 72m, 58.0m },
                    { 6, 25.0m, 20.8m, 24.5m, 85m, 162m, 90m, 2, 23.0m, "Vòng eo giảm 5cm, sức bền tăng tốt", 1, new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 50m, 67m, 54.5m },
                    { 7, 24.5m, 19.8m, 21.0m, 85m, 162m, 89m, 2, 23.8m, "Body cân đối, cơ bụng bắt đầu hiện rãnh 11", 2, new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 49m, 64m, 52.0m },
                    { 8, 27.0m, 22.0m, 26.0m, 88m, 168m, 94m, 3, 25.0m, "Bắt đầu tập định hình vóc dáng cùng PT Bích", 2, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 53m, 70m, 62.0m },
                    { 9, 26.5m, 20.7m, 22.5m, 89m, 168m, 94m, 3, 26.2m, "Độ dẻo dai và thể lực cải thiện vượt bậc", 2, new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 52m, 66m, 58.5m },
                    { 10, 35.0m, 25.9m, 24.0m, 100m, 178m, 101m, 6, 35.0m, "Tập tăng cơ giảm mỡ ban đầu", 3, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 59m, 88m, 82.0m }
                });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "ScheduleID", "BranchID", "EndTime", "MemberID", "Note", "PTID", "RoomID", "StartTime", "Status", "TrainingDate", "TrainingType" },
                values: new object[,]
                {
                    { 2, 1, new TimeSpan(0, 15, 30, 0, 0), 2, "Đốt calo cường độ cao, theo dõi nhịp tim", 1, 2, new TimeSpan(0, 14, 0, 0, 0), "Confirmed", new DateTime(2026, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cardio HIIT & Đốt Mỡ Chuyên Sâu" },
                    { 4, 1, new TimeSpan(0, 16, 30, 0, 0), 3, "Khởi động kỹ khớp gối", 1, 1, new TimeSpan(0, 15, 0, 0, 0), "Scheduled", new DateTime(2026, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Chân Mông & Giãn Cơ" },
                    { 6, 1, new TimeSpan(0, 15, 30, 0, 0), 2, "Giảm thời gian nghỉ giữa hiệp", 1, 1, new TimeSpan(0, 14, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Ngực & Tay Sau Nâng Cao" },
                    { 9, 1, new TimeSpan(0, 15, 30, 0, 0), 3, "Dùng đai hỗ trợ lưng", 1, 1, new TimeSpan(0, 14, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Lưng Xô & Bắp Tay" },
                    { 10, 1, new TimeSpan(0, 10, 30, 0, 0), 2, "Bài tập sức bền và thăng bằng", 1, 2, new TimeSpan(0, 9, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cardio Circuit & Core Stability" },
                    { 11, 1, new TimeSpan(0, 17, 30, 0, 0), 3, "Chỉnh sửa góc đầu gối khi xuống tạ", 2, 1, new TimeSpan(0, 16, 0, 0, 0), "Confirmed", new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tư Thế Squat & Cơ Đùi Nữ" },
                    { 12, 1, new TimeSpan(0, 19, 0, 0, 0), 2, "Hip Thrust 40kg, kết hợp dây kháng lực MiniBand", 2, 1, new TimeSpan(0, 17, 30, 0, 0), "Confirmed", new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Mông Quả Đào & Thon Gọn Eo" },
                    { 13, 1, new TimeSpan(0, 17, 30, 0, 0), 3, "Kéo giãn giảm mỏi cổ vai gáy", 2, 2, new TimeSpan(0, 16, 0, 0, 0), "Scheduled", new DateTime(2026, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pilates Dẻo Dai & Cột Sống" },
                    { 14, 1, new TimeSpan(0, 18, 30, 0, 0), 2, "Tập Tabata 20s-10s và Plank", 2, 1, new TimeSpan(0, 17, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cardio Giảm Cân & Bụng Dưới" },
                    { 15, 1, new TimeSpan(0, 17, 30, 0, 0), 3, "Dumbbell Lateral Raise 3kg", 2, 1, new TimeSpan(0, 16, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Vai Nữ & Lưng Tự Tin" },
                    { 16, 1, new TimeSpan(0, 19, 0, 0, 0), 2, "Foam rolling giải phóng điểm căng cơ", 2, 2, new TimeSpan(0, 17, 30, 0, 0), "Scheduled", new DateTime(2026, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kéo Giãn Cơ & Thư Giãn Toàn Thân" },
                    { 17, 1, new TimeSpan(0, 17, 30, 0, 0), 3, "Rèn luyện cơ đùi sau và mông", 2, 1, new TimeSpan(0, 16, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Legs Day & Glute Bridge" },
                    { 18, 1, new TimeSpan(0, 18, 30, 0, 0), 2, "Tập với tạ Kettlebell và bóng Slam Ball", 2, 1, new TimeSpan(0, 17, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đốt Mỡ Kháng Lực Toàn Thân" },
                    { 19, 2, new TimeSpan(0, 9, 30, 0, 0), 1, "Deadlift kỹ thuật Sumo, mức tạ 100kg", 3, 3, new TimeSpan(0, 8, 0, 0, 0), "Confirmed", new DateTime(2026, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Powerlifting Deadlift & Lưng Dày" },
                    { 20, 2, new TimeSpan(0, 11, 30, 0, 0), 2, "Tập kỹ thuật khóa khớp vai và ép xương bả vai", 3, 3, new TimeSpan(0, 10, 0, 0, 0), "Scheduled", new DateTime(2026, 10, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bench Press Chuẩn Thi Đấu" },
                    { 21, 2, new TimeSpan(0, 10, 0, 0, 0), 1, "Back Squat 80kg x 5 reps x 5 sets", 3, 3, new TimeSpan(0, 8, 30, 0, 0), "Scheduled", new DateTime(2026, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Squat Nặng & Phục Hồi Khớp" },
                    { 22, 2, new TimeSpan(0, 10, 30, 0, 0), 3, "Clean & Press tạ kỹ thuật 15kg", 3, 3, new TimeSpan(0, 9, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kỹ Thuật Cử Tạ Olympic Cơ Bản" },
                    { 23, 2, new TimeSpan(0, 11, 30, 0, 0), 1, "Đẩy xe trượt Prowler Sled & quất dây thừng Battle Rope", 3, 4, new TimeSpan(0, 10, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Conditioning & Thể Lực Đỉnh Cao" },
                    { 24, 2, new TimeSpan(0, 10, 0, 0, 0), 2, "Barbell Bent Over Row & T-Bar Row", 3, 3, new TimeSpan(0, 8, 30, 0, 0), "Scheduled", new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Xô Lưng Tăng Độ Dày" },
                    { 25, 2, new TimeSpan(0, 11, 30, 0, 0), 1, "Chuẩn bị khởi động dài và bảo hiểm an toàn", 3, 3, new TimeSpan(0, 10, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thử Thách 1RM Bench & Squat" },
                    { 26, 2, new TimeSpan(0, 10, 30, 0, 0), 3, "Máy chèo thuyền kết hợp máy trượt tuyết", 3, 4, new TimeSpan(0, 9, 0, 0, 0), "Scheduled", new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "HIIT & Sức Bền Tim Mạch" }
                });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "StaffID", "BaseSalary", "BranchID", "DateOfBirth", "Email", "FullName", "Gender", "PTID", "Phone", "Role", "Status", "WorkShift" },
                values: new object[,]
                {
                    { 3, 16500000m, 2, new DateTime(1994, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "minh.duc@quanlygym.vn", "Nguyễn Đức Minh", "Nam", 3, "0987543210", "PT", true, "Ca cố định: 08:00 - 16:00 (Thứ 2 - CN)" },
                    { 4, 14500000m, 2, new DateTime(1998, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "thao.phan@quanlygym.vn", "Phan Thị Thảo", "Nữ", 4, "0921098765", "PT", true, "Ca cố định: 14:00 - 22:00 (Thứ 2 - CN)" },
                    { 5, 15000000m, 3, new DateTime(1996, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "long.vu@quanlygym.vn", "Vũ Hoàng Long", "Nam", 5, "0909876543", "PT", true, "Ca cố định: 09:00 - 17:00 (Thứ 2 - CN)" }
                });

            migrationBuilder.InsertData(
                table: "CheckInImages",
                columns: new[] { "ImageID", "CheckInID", "ImagePath", "UploadedAt", "VerificationNote", "VerifiedBy" },
                values: new object[,]
                {
                    { 2, 2, "uploads/checkin_002.jpg", new DateTime(2026, 10, 21, 13, 51, 0, 0, DateTimeKind.Unspecified), "Nhận diện khuôn mặt hợp lệ", 1 },
                    { 3, 3, "uploads/checkin_003.jpg", new DateTime(2026, 10, 21, 15, 56, 0, 0, DateTimeKind.Unspecified), "Xác thực hội viên thành công", 2 },
                    { 5, 5, "uploads/checkin_005.jpg", new DateTime(2026, 10, 22, 17, 16, 0, 0, DateTimeKind.Unspecified), "Check-in camera sảnh lễ tân", 1 },
                    { 6, 6, "uploads/checkin_006.jpg", new DateTime(2026, 10, 23, 7, 6, 0, 0, DateTimeKind.Unspecified), "Check-in camera sảnh lễ tân", 1 },
                    { 7, 7, "uploads/checkin_007.jpg", new DateTime(2026, 10, 23, 17, 26, 0, 0, DateTimeKind.Unspecified), "Hội viên Bích phụ trách", 2 },
                    { 8, 8, "uploads/checkin_008.jpg", new DateTime(2026, 10, 24, 14, 56, 0, 0, DateTimeKind.Unspecified), "Xác nhận đúng lịch hẹn", 1 },
                    { 9, 9, "uploads/checkin_009.jpg", new DateTime(2026, 10, 24, 18, 1, 0, 0, DateTimeKind.Unspecified), "Check-in cửa từ Bình Thạnh", 3 },
                    { 10, 10, "uploads/checkin_010.jpg", new DateTime(2026, 10, 25, 7, 56, 0, 0, DateTimeKind.Unspecified), "PT Minh duyệt check-in", 3 },
                    { 11, 11, "uploads/checkin_011.jpg", new DateTime(2026, 10, 25, 16, 31, 0, 0, DateTimeKind.Unspecified), "Cửa từ hợp lệ", 4 },
                    { 12, 12, "uploads/checkin_012.jpg", new DateTime(2026, 10, 25, 19, 11, 0, 0, DateTimeKind.Unspecified), "Cửa từ hợp lệ", 4 },
                    { 13, 13, "uploads/checkin_013.jpg", new DateTime(2026, 10, 25, 6, 46, 0, 0, DateTimeKind.Unspecified), "Lễ tân chi nhánh Q7 duyệt", 5 },
                    { 14, 14, "uploads/checkin_014.jpg", new DateTime(2026, 10, 25, 18, 21, 0, 0, DateTimeKind.Unspecified), "Cửa từ Q7 hợp lệ", 5 }
                });

            migrationBuilder.InsertData(
                table: "EquipmentMaintenances",
                columns: new[] { "MaintenanceID", "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate", "Status" },
                values: new object[] { 10, 350000m, "Thay nước bình chứa và làm sạch bánh đà máy chèo thuyền", 10, new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàn thành" });

            migrationBuilder.InsertData(
                table: "Incidents",
                columns: new[] { "IncidentID", "Description", "EquipmentID", "Note", "ReportDate", "ReportedBy", "ResolvedDate", "Status" },
                values: new object[,]
                {
                    { 8, "Quả tạ 15kg bị lỏng ốc đầu ren cao su", 9, "Đã dùng keo khóa ren vặn chặt cố định", new DateTime(2026, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resolved" },
                    { 10, "Dây kéo tay cầm máy chèo thuyền bị xoắn nhẹ cần căn lại", 10, "Đã giao kỹ thuật viên ca sáng kiểm tra", new DateTime(2026, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "New" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_BranchID",
                table: "Staffs",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_PTID",
                table: "Staffs",
                column: "PTID");

            migrationBuilder.Sql("EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';");
            migrationBuilder.DropTable(
                name: "Staffs");

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Progresses",
                keyColumn: "ProgressID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Progresses",
                keyColumn: "ProgressID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Progresses",
                keyColumn: "ProgressID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Progresses",
                keyColumn: "ProgressID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Progresses",
                keyColumn: "ProgressID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Progresses",
                keyColumn: "ProgressID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Progresses",
                keyColumn: "ProgressID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Progresses",
                keyColumn: "ProgressID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Progresses",
                keyColumn: "ProgressID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "EquipmentID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "EquipmentID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "EquipmentID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "EquipmentID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "EquipmentID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "EquipmentID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "EquipmentID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Equipments",
                keyColumn: "EquipmentID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "BranchID",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "IsPTPackage",
                table: "Packages");

            migrationBuilder.AddColumn<decimal>(
                name: "BMI",
                table: "Members",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BodyFat",
                table: "Members",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Height",
                table: "Members",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MuscleMass",
                table: "Members",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "Members",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "WorkingSchedules",
                columns: table => new
                {
                    WorkingScheduleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchID = table.Column<int>(type: "int", nullable: true),
                    PTID = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WorkDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingSchedules", x => x.WorkingScheduleID);
                    table.ForeignKey(
                        name: "FK_WorkingSchedules_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "BranchID");
                    table.ForeignKey(
                        name: "FK_WorkingSchedules_PTs_PTID",
                        column: x => x.PTID,
                        principalTable: "PTs",
                        principalColumn: "PTID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 3,
                columns: new[] { "Role", "Username" },
                values: new object[] { "Member", "member_hung" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchID",
                keyValue: 1,
                columns: new[] { "Address", "BranchName", "Email", "Phone" },
                values: new object[] { "123 Cầu Giấy, Hà Nội", "QuanLyGym Cầu Giấy", "caugiay@gym.com", "0987654321" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchID",
                keyValue: 2,
                columns: new[] { "Address", "BranchName", "Email", "Phone" },
                values: new object[] { "456 Xã Đàn, Hà Nội", "QuanLyGym Đống Đa", "dongda@gym.com", "0912345678" });

            migrationBuilder.UpdateData(
                table: "CheckInImages",
                keyColumn: "ImageID",
                keyValue: 1,
                columns: new[] { "UploadedAt", "VerificationNote" },
                values: new object[] { new DateTime(2026, 3, 1, 16, 56, 0, 0, DateTimeKind.Unspecified), "Ảnh hợp lệ" });

            migrationBuilder.UpdateData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 1,
                columns: new[] { "CheckInTime", "Note" },
                values: new object[] { new DateTime(2026, 3, 1, 16, 55, 0, 0, DateTimeKind.Unspecified), "Check-in tập cùng PT" });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 1,
                column: "Description",
                value: "Bảo dưỡng định kỳ thảm chạy và tra dầu");

            migrationBuilder.UpdateData(
                table: "Equipments",
                keyColumn: "EquipmentID",
                keyValue: 1,
                columns: new[] { "Description", "EquipmentCode", "EquipmentName", "Price" },
                values: new object[] { "Máy chạy bộ cao cấp", "CB-01", "Máy chạy bộ Technogym", 25000000m });

            migrationBuilder.UpdateData(
                table: "Equipments",
                keyColumn: "EquipmentID",
                keyValue: 2,
                columns: new[] { "Description", "EquipmentName", "Price" },
                values: new object[] { "Khung gánh tạ an toàn", "Dàn gánh tạ Squat Rack", 15000000m });

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 1,
                columns: new[] { "Description", "Note" },
                values: new object[] { "Máy chạy bộ phát ra tiếng kêu lạ ở băng tải", "Đã siết lại ốc và căn chỉnh băng tải" });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "MemberID",
                keyValue: 1,
                columns: new[] { "AccountID", "Address", "BMI", "BodyFat", "Email", "Height", "MuscleMass", "Weight" },
                values: new object[] { 3, "Cầu Giấy, Hà Nội", 23m, 18m, "hung@gmail.com", 172m, 32m, 68m });

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 1,
                columns: new[] { "EndDate", "PackageID", "Price", "UsedSessions" },
                values: new object[] { new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3000000m, 5 });

            migrationBuilder.UpdateData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 1,
                columns: new[] { "Email", "Experience", "Specialization" },
                values: new object[] { "nam@gym.com", 5, "Tăng cơ, Giảm mỡ" });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 1,
                columns: new[] { "Benefits", "Description", "PackageName" },
                values: new object[] { "Tập tự do các phòng", "Gói cơ bản", "Gói Thường 1 Tháng" });

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 2,
                columns: new[] { "Benefits", "Description", "PackageName", "Price", "TotalSessions" },
                values: new object[] { "Kèm PT 1-1, Nước uống miễn phí", "Gói cao cấp", "Gói VIP 3 Tháng + PT", 3000000m, 36 });

            migrationBuilder.UpdateData(
                table: "Progresses",
                keyColumn: "ProgressID",
                keyValue: 1,
                columns: new[] { "BMI", "BodyFat", "Chest", "Height", "Hip", "MuscleMass", "Note", "RecordDate", "Thigh", "Waist", "Weight" },
                values: new object[] { 23m, 18m, 95m, 172m, 92m, 32m, "Chỉ số ban đầu bình thường", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 54m, 78m, 68m });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 1,
                columns: new[] { "Capacity", "Description", "RoomName" },
                values: new object[] { 50, "Khu vực tập tạ tổng hợp", "Phòng Gym Tầng 1" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 2,
                columns: new[] { "Capacity", "Description" },
                values: new object[] { 30, "Máy chạy bộ và xe đạp" });

            migrationBuilder.UpdateData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 1,
                columns: new[] { "EndTime", "Note", "StartTime", "Status", "TrainingDate", "TrainingType" },
                values: new object[] { new TimeSpan(0, 18, 30, 0, 0), "Hội viên tập tốt, nâng tạ ngực 50kg", new TimeSpan(0, 17, 0, 0, 0), "Completed", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Ngực & Tay Sau" });

            migrationBuilder.InsertData(
                table: "WorkingSchedules",
                columns: new[] { "WorkingScheduleID", "BranchID", "EndTime", "PTID", "StartTime", "Status", "WorkDate" },
                values: new object[] { 1, 1, new TimeSpan(0, 20, 0, 0, 0), 1, new TimeSpan(0, 14, 0, 0, 0), "Đã ca", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingSchedules_BranchID",
                table: "WorkingSchedules",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingSchedules_PTID",
                table: "WorkingSchedules",
                column: "PTID");

            migrationBuilder.Sql("EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';");
        }
    }
}
