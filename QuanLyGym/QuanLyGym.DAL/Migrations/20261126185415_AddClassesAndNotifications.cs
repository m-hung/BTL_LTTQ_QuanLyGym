using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyGym.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddClassesAndNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClassType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    PTID = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    MaxCapacity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassID);
                    table.ForeignKey(
                        name: "FK_Classes_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "BranchID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_PTs_PTID",
                        column: x => x.PTID,
                        principalTable: "PTs",
                        principalColumn: "PTID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NotificationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK_Notifications_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "BranchID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ClassEnrollments",
                columns: table => new
                {
                    EnrollmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassID = table.Column<int>(type: "int", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttendanceStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassEnrollments", x => x.EnrollmentID);
                    table.ForeignKey(
                        name: "FK_ClassEnrollments_Classes_ClassID",
                        column: x => x.ClassID,
                        principalTable: "Classes",
                        principalColumn: "ClassID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassEnrollments_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountID", "CreatedAt", "Email", "PasswordHash", "Role", "Status", "Username" },
                values: new object[,]
                {
                    { 17, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "hoangnam.le@quanlygym.vn", "PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=", "PT", true, "pt_hoangnam" },
                    { 18, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "thuthao.dang@quanlygym.vn", "PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=", "PT", true, "pt_thuthao" },
                    { 19, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ngocanh.pham@quanlygym.vn", "PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=", "PT", true, "pt_ngocanh" },
                    { 20, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "quocbao.tran@quanlygym.vn", "PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=", "PT", true, "pt_quocbao" },
                    { 21, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "khanhlinh.do@quanlygym.vn", "PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=", "PT", true, "pt_khanhlinh" },
                    { 22, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "tiendat.nguyen@quanlygym.vn", "PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=", "PT", true, "pt_tiendat" },
                    { 23, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "kimngan.hoang@quanlygym.vn", "PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=", "PT", true, "pt_kimngan" }
                });

            migrationBuilder.InsertData(
                table: "CheckIns",
                columns: new[] { "CheckInID", "BranchID", "CheckInTime", "MemberID", "Note", "PTID", "Status" },
                values: new object[,]
                {
                    { 16, 1, new DateTime(2026, 11, 2, 8, 25, 0, 0, DateTimeKind.Unspecified), 1, "Check-in tập Vai & Bụng Core cùng PT Nam", 1, "Approved" },
                    { 17, 1, new DateTime(2026, 11, 4, 13, 55, 0, 0, DateTimeKind.Unspecified), 2, "Check-in tập Ngực nâng cao cùng PT Nam", 1, "Approved" },
                    { 18, 1, new DateTime(2026, 11, 5, 15, 55, 0, 0, DateTimeKind.Unspecified), 3, "Check-in tập Vai Nữ cùng PT Bích", 2, "Approved" },
                    { 19, 1, new DateTime(2026, 11, 6, 8, 55, 0, 0, DateTimeKind.Unspecified), 1, "Check-in Full Body Functional cùng PT Nam", 1, "Approved" },
                    { 20, 2, new DateTime(2026, 11, 10, 8, 20, 0, 0, DateTimeKind.Unspecified), 6, "Check-in Powerlifting cùng PT Minh", 3, "Approved" },
                    { 21, 1, new DateTime(2026, 11, 18, 13, 50, 0, 0, DateTimeKind.Unspecified), 2, "Check-in HIIT Tabata cùng PT Nam", 1, "Approved" },
                    { 22, 1, new DateTime(2026, 11, 20, 8, 55, 0, 0, DateTimeKind.Unspecified), 1, "Check-in Deadlift Lưng Dày cùng PT Nam", 1, "Approved" },
                    { 23, 2, new DateTime(2026, 11, 24, 18, 25, 0, 0, DateTimeKind.Unspecified), 7, "Check-in lớp Zumba Dance cùng PT Thảo", 4, "Approved" },
                    { 24, 1, new DateTime(2026, 11, 27, 8, 20, 0, 0, DateTimeKind.Unspecified), 1, "Check-in buổi tập sáng cùng PT Nam", 1, "Approved" }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "ClassID", "BranchID", "ClassName", "ClassType", "DayOfWeek", "Description", "EndTime", "MaxCapacity", "PTID", "RoomID", "StartTime", "Status" },
                values: new object[,]
                {
                    { 1, 1, "Lớp Yoga Căn Bản & Thiền Định", "Yoga", "Thứ 2 - 4 - 6", "Lớp Yoga asana kết hợp kỹ thuật thở Pranayama và thiền tịnh tâm, giúp giảm stress và tăng độ dẻo dai.", new TimeSpan(0, 8, 0, 0, 0), 20, 2, 2, new TimeSpan(0, 6, 30, 0, 0), true },
                    { 2, 1, "Lớp Boxing & Kickfit Cường Độ Cao", "Boxing", "Thứ 2 - 4 - 6", "Kỹ thuật đấm đá cơ bản, di chuyển đối kháng và rèn luyện sức bền tim mạch, tự vệ thực chiến.", new TimeSpan(0, 19, 30, 0, 0), 25, 1, 1, new TimeSpan(0, 18, 0, 0, 0), true },
                    { 6, 2, "Lớp Boxing & Kickfit Cường Độ Cao", "Boxing", "Thứ 2 - 4 - 6", "Kỹ thuật đấm đá cơ bản, di chuyển đối kháng và rèn luyện sức bền tim mạch, tự vệ thực chiến.", new TimeSpan(0, 19, 30, 0, 0), 25, 3, 3, new TimeSpan(0, 18, 0, 0, 0), true },
                    { 7, 2, "Lớp Zumba Dance Đốt Cháy Calo", "Zumba", "Thứ 3 - 5 - 7", "Vũ điệu Zumba Latin sôi động trên nền nhạc lôi cuốn, hỗ trợ đốt cháy 600-800 kcal mỗi buổi tập.", new TimeSpan(0, 19, 45, 0, 0), 30, 4, 4, new TimeSpan(0, 18, 30, 0, 0), true },
                    { 10, 3, "Lớp Boxing & Kickfit Cường Độ Cao", "Boxing", "Thứ 2 - 4 - 6", "Kỹ thuật đấm đá cơ bản, di chuyển đối kháng và rèn luyện sức bền tim mạch, tự vệ thực chiến.", new TimeSpan(0, 19, 30, 0, 0), 25, 5, 5, new TimeSpan(0, 18, 0, 0, 0), true }
                });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 1,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 750000m, "Thay dây cáp thép bọc nylon và bôi trơn ròng rọc giàn kéo xô", 4, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 2,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 600000m, "Thay bàn đạp dây đai và nối lại cảm biến nhịp tim xe đạp tập", 5, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 3,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 800000m, "Căn chỉnh lại hệ thống đối trọng tạ cáp bên phải dàn cáp đôi", 7, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 4,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 450000m, "Tra mỡ vòng bi ray trượt và chỉnh chốt an toàn máy Leg Press", 6, new DateTime(2026, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 5,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 900000m, "Bảo trì mô-tơ kháng lực từ tính máy trượt tuyết Elliptical", 8, new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 6,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 650000m, "Kiểm tra động cơ 4.0 HP và căn chỉnh độ lệch băng tải máy chạy bộ", 1, new DateTime(2026, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 7,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 550000m, "Bọc lại đệm mút simili cao cấp và siết ốc dàn khung giàn Squat", 2, new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 8,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 350000m, "Cân chỉnh độ dốc ghế đa năng và kiểm tra khóa an toàn J-Hooks", 3, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 9,
                columns: new[] { "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate", "Status" },
                values: new object[] { "Sơn phủ chống rỉ và dùng keo khóa ren siết chặt đầu tạ tay cao su", 9, new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàn thành" });

            migrationBuilder.InsertData(
                table: "EquipmentMaintenances",
                columns: new[] { "MaintenanceID", "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate", "Status" },
                values: new object[,]
                {
                    { 11, 500000m, "Thay thế chốt tạ selector pin bị kẹt và bảo dưỡng bánh xe dẫn hướng", 7, new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàn thành" },
                    { 12, 850000m, "Bảo dưỡng tổng thể động cơ, thay chổi than và tra dầu silicon máy chạy", 1, new DateTime(2026, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàn thành" }
                });

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 9,
                columns: new[] { "Note", "ResolvedDate", "Status" },
                values: new object[] { "Đã thay chốt tạ selector pin mới chính hãng", new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resolved" });

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 10,
                columns: new[] { "Note", "ResolvedDate", "Status" },
                values: new object[] { "Đã tháo căn chỉnh và tra dầu mượt mà", new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resolved" });

            migrationBuilder.InsertData(
                table: "Incidents",
                columns: new[] { "IncidentID", "Description", "EquipmentID", "Note", "ReportDate", "ReportedBy", "ResolvedDate", "Status" },
                values: new object[,]
                {
                    { 11, "Yên xe đạp tập xoay bị lỏng ốc vít nâng hạ", 5, "Kỹ thuật viên đã thay núm vặn khóa chốt mới", new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Resolved" },
                    { 12, "Thảm máy chạy bộ 1 bị trượt nhẹ khi chạy tốc độ cao", 1, "Đang lên lịch bảo dưỡng tổng thể trong tháng 12", new DateTime(2026, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "In Progress" }
                });

            migrationBuilder.InsertData(
                table: "Memberships",
                columns: new[] { "MembershipID", "BranchID", "CreatedAt", "EndDate", "MemberID", "PackageID", "Price", "PriceAtPurchase", "RegisterDate", "StartDate", "Status", "UsedPTSessions" },
                values: new object[,]
                {
                    { 11, 2, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 3, 5800000m, 5800000m, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 14 },
                    { 12, 1, new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 2, 2800000m, 2800000m, new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 8 },
                    { 13, 2, new DateTime(2026, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 1, 500000m, 500000m, new DateTime(2026, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 10 },
                    { 14, 1, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, 2800000m, 2800000m, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 11 },
                    { 15, 3, new DateTime(2026, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 3, 5800000m, 5800000m, new DateTime(2026, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 20 },
                    { 16, 2, new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 2, 2800000m, 2800000m, new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 9 },
                    { 17, 3, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 1, 500000m, 500000m, new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 7 },
                    { 18, 1, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 4, 12500000m, 12500000m, new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 22 },
                    { 19, 1, new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, 5800000m, 5800000m, new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 16 },
                    { 20, 2, new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 2, 2800000m, 2800000m, new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 8 },
                    { 21, 3, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 2, 2800000m, 2800000m, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 6 },
                    { 22, 2, new DateTime(2026, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 3, 5800000m, 5800000m, new DateTime(2026, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 12 },
                    { 23, 1, new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, 500000m, 500000m, new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 10 },
                    { 24, 3, new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 4, 12500000m, 12500000m, new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 15 },
                    { 25, 1, new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1, 500000m, 500000m, new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 5 },
                    { 26, 2, new DateTime(2026, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 3, 5800000m, 5800000m, new DateTime(2026, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 4 },
                    { 27, 1, new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 5800000m, 5800000m, new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 2 },
                    { 28, 1, new DateTime(2026, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, 2800000m, 2800000m, new DateTime(2026, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 1 },
                    { 29, 1, new DateTime(2026, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, 5800000m, 5800000m, new DateTime(2026, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 0 },
                    { 30, 2, new DateTime(2026, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 4, 12500000m, 12500000m, new DateTime(2026, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 0 },
                    { 31, 3, new DateTime(2026, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 2, 2800000m, 2800000m, new DateTime(2026, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 0 }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationID", "AccountID", "BranchID", "Content", "CreatedAt", "IsRead", "NotificationType", "Title" },
                values: new object[,]
                {
                    { 1, null, null, "Hệ thống QuanLyGym trân trọng gửi lời chúc mừng năm mới 2026 đến toàn thể quý Hội viên và HLV. Hệ thống sẽ mở cửa xuyên suốt phục vụ quý khách.", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), true, "System", "Chúc Mừng Năm Mới 2026 - Lịch Hoạt Động Tết" },
                    { 2, 7, 1, "Chào bạn Khánh Hưng! Hợp đồng gói tập VIP Diamond của bạn đã được kích hoạt thành công. HLV cá nhân Nguyễn Văn Nam sẽ đồng hành cùng bạn.", new DateTime(2026, 1, 1, 9, 30, 0, 0, DateTimeKind.Unspecified), true, "Reminder", "Xác Nhận Kích Hoạt Gói VIP Diamond" },
                    { 3, null, null, "QuanLyGym chính thức mở các lớp học nhóm: Yoga, Zumba, Boxing và Pilates tại cả 3 chi nhánh với đội ngũ HLV hàng đầu. Đăng ký ngay tại quầy lễ tân!", new DateTime(2026, 1, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), true, "Promotion", "Khai Trương Studio GroupX & Lớp Học Nhóm Mới" },
                    { 4, null, null, "Tặng ngay 20% khi đăng ký hoặc nâng cấp gói tập và giảm 50% phí thuê HLV kèm riêng cho phái đẹp trong tuần lễ từ 01/03 đến 08/03/2026.", new DateTime(2026, 3, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), true, "Promotion", "Ưu Đãi Đặc Biệt Quốc Tế Phụ Nữ 8/3" },
                    { 5, 8, 1, "Đã 3 tháng kể từ lần kiểm tra thể chất gần nhất. Mời bạn đến phòng đo InBody tại Chi nhánh 1 để HLV Nam đánh giá lại tiến độ tăng cơ giảm mỡ.", new DateTime(2026, 4, 15, 14, 0, 0, 0, DateTimeKind.Unspecified), true, "Reminder", "Nhắc Nhở Đo Lại Chỉ Số Thể Chất InBody Định Kỳ" },
                    { 6, null, 1, "Hệ thống máy chạy bộ Matrix và dàn tạ khối tại Chi nhánh Quận 1 & Bình Thạnh đã được bảo dưỡng toàn diện, sẵn sàng phục vụ quý hội viên.", new DateTime(2026, 6, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), true, "Maintenance", "Hoàn Tất Bảo Trì & Nâng Cấp Hệ Thống Máy Chạy Bộ" },
                    { 7, null, null, "Tham gia giải chạy và thử thách Fitness 30 ngày để nhận gói tập VIP 6 tháng và phần thưởng giá trị từ Ban quản lý QuanLyGym.", new DateTime(2026, 7, 1, 8, 30, 0, 0, DateTimeKind.Unspecified), true, "System", "Chào Hè Sôi Động 2026 - Thử Thách Đốt Mỡ 30 Ngày" },
                    { 8, 9, 1, "Thẻ tập của bạn sắp hết hạn trong 7 ngày tới. Hãy liên hệ bộ phận Lễ tân hoặc gia hạn trên ứng dụng để nhận ngay ưu đãi 10% gói tập mới.", new DateTime(2026, 9, 25, 10, 0, 0, 0, DateTimeKind.Unspecified), false, "Reminder", "Thông Báo Sắp Hết Hạn Thẻ Tập" },
                    { 9, null, null, "Cơ hội duy nhất trong năm: Mua 1 năm tặng 3 tháng, giảm giá cực sốc toàn bộ các gói Classic, Silver, Gold và VIP Diamond!", new DateTime(2026, 11, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), false, "Promotion", "Đại Tiệc Siêu Sale Black Friday Tháng 11/2026" },
                    { 10, 2, 1, "Bạn có 15 lịch hẹn tập luyện mới từ hội viên trải dài suốt tháng 12/2026. Vui lòng kiểm tra mục Lịch tập và xác nhận các ca chờ duyệt.", new DateTime(2026, 11, 27, 8, 0, 0, 0, DateTimeKind.Unspecified), false, "Reminder", "Thông Báo Lịch Đặt Hẹn PT Mới Tháng 12" },
                    { 11, null, 3, "Chi nhánh Quận 7 và Bình Thạnh sẽ thực hiện kiểm tra an toàn thiết bị phòng tập định kỳ vào ngày 15/12/2026 từ 22:00 đến 24:00.", new DateTime(2026, 11, 27, 9, 0, 0, 0, DateTimeKind.Unspecified), false, "Maintenance", "Kế Hoạch Tổng Vệ Sinh & Kiểm Tra An Toàn Cuối Năm" }
                });

            migrationBuilder.UpdateData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 1,
                column: "Specialization",
                value: "Boxing & Kickfit Cường độ cao");

            migrationBuilder.UpdateData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 2,
                column: "Specialization",
                value: "Yoga Căn Bản & Thiền Định");

            migrationBuilder.UpdateData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 3,
                column: "Specialization",
                value: "Boxing & Kickfit Cường độ cao");

            migrationBuilder.UpdateData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 4,
                column: "Specialization",
                value: "Zumba Dance Fitness & Aerobic");

            migrationBuilder.UpdateData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 5,
                column: "Specialization",
                value: "Boxing & Kickfit Cường độ cao");

            migrationBuilder.InsertData(
                table: "Progresses",
                columns: new[] { "ProgressID", "Arm", "BMI", "BodyFat", "Chest", "Height", "Hip", "MemberID", "MuscleMass", "Note", "PTID", "RecordDate", "Thigh", "Waist", "Weight" },
                values: new object[,]
                {
                    { 11, 38.0m, 23.5m, 14.8m, 104m, 175m, 95m, 1, 36.2m, "Chỉ số thể hình đạt chuẩn vận động viên, cơ bắp sắc nét", 1, new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 59m, 75m, 72.0m },
                    { 12, 24.0m, 19.6m, 20.0m, 85m, 162m, 89m, 2, 24.0m, "Vòng eo đạt chuẩn thon gọn lý tưởng, thể lực rất dẻo dai", 1, new DateTime(2026, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 48.5m, 63m, 51.5m }
                });

            migrationBuilder.InsertData(
                table: "Schedules",
                columns: new[] { "ScheduleID", "BranchID", "EndTime", "MemberID", "Note", "PTID", "RoomID", "StartTime", "Status", "TrainingDate", "TrainingType" },
                values: new object[,]
                {
                    { 41, 1, new TimeSpan(0, 15, 30, 0, 0), 2, "Kiểm tra nhịp tim và sức bền tim mạch", 1, 2, new TimeSpan(0, 14, 0, 0, 0), "Confirmed", new DateTime(2026, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cardio HIIT & Siết Cơ Bụng" },
                    { 42, 1, new TimeSpan(0, 10, 30, 0, 0), 4, "Hội viên bắt đầu lộ trình tăng cơ", 1, 1, new TimeSpan(0, 9, 0, 0, 0), "Confirmed", new DateTime(2026, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Ngực & Tay Trước Kháng Lực" },
                    { 43, 1, new TimeSpan(0, 10, 0, 0, 0), 1, "Khởi động kỹ khớp gối và cơ đùi trước", 1, 1, new TimeSpan(0, 8, 30, 0, 0), "Scheduled", new DateTime(2026, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Chân Mông & Squat Nặng" },
                    { 44, 1, new TimeSpan(0, 15, 30, 0, 0), 2, "Tập trung bài Lat Pulldown & Seated Cable Row", 1, 1, new TimeSpan(0, 14, 0, 0, 0), "Scheduled", new DateTime(2026, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Lưng Xô & Kéo Xà Trợ Lực" },
                    { 45, 1, new TimeSpan(0, 10, 0, 0, 0), 1, "Tăng mức tạ lên 90kg, kiểm tra đai lưng", 1, 1, new TimeSpan(0, 8, 30, 0, 0), "Scheduled", new DateTime(2026, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deadlift Sumo & Lưng Dày" },
                    { 46, 1, new TimeSpan(0, 16, 30, 0, 0), 3, "Rèn luyện phản xạ và đòn đấm thẳng", 1, 2, new TimeSpan(0, 15, 0, 0, 0), "Pending", new DateTime(2026, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kickfit Đốt Mỡ Cường Độ Cao" },
                    { 47, 1, new TimeSpan(0, 10, 30, 0, 0), 4, "Overhead Dumbbell Press & Dips", 1, 1, new TimeSpan(0, 9, 0, 0, 0), "Scheduled", new DateTime(2026, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Vai & Tay Sau Chuyên Sâu" },
                    { 48, 1, new TimeSpan(0, 10, 0, 0, 0), 1, "Kéo giãn cơ đùi sau sau buổi tập", 1, 1, new TimeSpan(0, 8, 30, 0, 0), "Scheduled", new DateTime(2026, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Leg Day - Leg Press & Hamstring Curl" },
                    { 49, 1, new TimeSpan(0, 15, 30, 0, 0), 2, "Tập với bóng Slam Ball và dây thừng Battle Rope", 1, 2, new TimeSpan(0, 14, 0, 0, 0), "Scheduled", new DateTime(2026, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Functional Training & Thể Lực Core" },
                    { 50, 1, new TimeSpan(0, 10, 30, 0, 0), 5, "Lịch hẹn mới đăng ký chờ xác nhận", 1, 1, new TimeSpan(0, 9, 0, 0, 0), "Pending", new DateTime(2026, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tập Toàn Thân Full Body & Giảm Mỡ" },
                    { 51, 1, new TimeSpan(0, 10, 0, 0, 0), 1, "Thử thách mức tạ 80kg có HLV bảo hiểm", 1, 1, new TimeSpan(0, 8, 30, 0, 0), "Scheduled", new DateTime(2026, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bench Press Nặng & Ngực Trên Incline" },
                    { 52, 1, new TimeSpan(0, 16, 30, 0, 0), 3, "Tập tổ hợp đòn 1-2 Hook Uppercut", 1, 2, new TimeSpan(0, 15, 0, 0, 0), "Scheduled", new DateTime(2026, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Boxing Đón Giáng Sinh - Đấm Bao Cát" },
                    { 53, 1, new TimeSpan(0, 10, 30, 0, 0), 2, "Foam Rolling và giãn cơ tĩnh toàn thân", 1, 2, new TimeSpan(0, 9, 0, 0, 0), "Confirmed", new DateTime(2026, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kéo Giãn Cơ Chuyên Sâu & Thư Giãn Noel" },
                    { 54, 1, new TimeSpan(0, 10, 0, 0, 0), 1, "Kiểm tra 1RM và chụp ảnh form dáng cuối năm", 1, 1, new TimeSpan(0, 8, 30, 0, 0), "Scheduled", new DateTime(2026, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đánh Giá Chỉ Số Thể Lực & Test Sức Mạnh" },
                    { 55, 1, new TimeSpan(0, 15, 30, 0, 0), 4, "Đo đạc chỉ số và lên giáo án năm mới 2027", 1, 1, new TimeSpan(0, 14, 0, 0, 0), "Scheduled", new DateTime(2026, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tổng Kết Chu Kỳ Tập Luyện Năm 2026" },
                    { 56, 1, new TimeSpan(0, 10, 30, 0, 0), 1, "Tập nhẹ nhàng xả stress sẵn sàng đón năm mới", 1, 1, new TimeSpan(0, 9, 0, 0, 0), "Scheduled", new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Buổi Tập Cuối Năm - Full Body Pump" },
                    { 57, 1, new TimeSpan(0, 17, 30, 0, 0), 3, "Kéo giãn cột sống", 2, 2, new TimeSpan(0, 16, 0, 0, 0), "Scheduled", new DateTime(2026, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pilates & Dáng Đẹp Cuối Năm" },
                    { 58, 2, new TimeSpan(0, 10, 30, 0, 0), 6, "Mức tạ mục tiêu 120kg", 3, 3, new TimeSpan(0, 9, 0, 0, 0), "Scheduled", new DateTime(2026, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Powerlifting Squat Heavy Day" },
                    { 59, 2, new TimeSpan(0, 16, 30, 0, 0), 7, "Nhạc Giáng Sinh sôi động", 4, 4, new TimeSpan(0, 15, 0, 0, 0), "Scheduled", new DateTime(2026, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zumba Cardio Đốt Mỡ Noel" },
                    { 60, 3, new TimeSpan(0, 11, 30, 0, 0), 9, "Tập đối kháng có giáp bảo hộ", 5, 5, new TimeSpan(0, 10, 0, 0, 0), "Scheduled", new DateTime(2026, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kickfit Thể Lực & Phản Xạ Q7" }
                });

            migrationBuilder.InsertData(
                table: "ClassEnrollments",
                columns: new[] { "EnrollmentID", "AttendanceStatus", "ClassID", "EnrollmentDate", "MemberID", "Note" },
                values: new object[,]
                {
                    { 1, "Attended", 1, new DateTime(2026, 1, 12, 6, 30, 0, 0, DateTimeKind.Unspecified), 2, "Hoàn thành tốt các tư thế Asana chào mặt trời" },
                    { 2, "Attended", 1, new DateTime(2026, 1, 14, 6, 30, 0, 0, DateTimeKind.Unspecified), 3, "Tập trung nhịp thở sâu rất tốt" },
                    { 3, "Attended", 2, new DateTime(2026, 1, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), 1, "Rèn luyện thể lực đấm bao cát cùng HLV Nam" },
                    { 4, "Absent", 2, new DateTime(2026, 1, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), 4, "Hội viên bận công việc đột xuất chưa đến lớp" },
                    { 8, "Attended", 6, new DateTime(2026, 3, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), 6, "Lực đấm và bước di chuyển tiến bộ rõ rệt" },
                    { 9, "Cancelled", 7, new DateTime(2026, 3, 24, 18, 30, 0, 0, DateTimeKind.Unspecified), 8, "Hội viên xin hủy lịch trước 1 ngày do đi công tác" },
                    { 12, "Attended", 10, new DateTime(2026, 5, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), 9, "Tập cường độ cao với HLV Hoàng Long" },
                    { 15, "Attended", 1, new DateTime(2026, 7, 6, 6, 30, 0, 0, DateTimeKind.Unspecified), 2, "Độ dẻo dai cơ gân kheo cải thiện" },
                    { 16, "Attended", 2, new DateTime(2026, 7, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), 1, "Tập đấm đá phản xạ chân mông" },
                    { 18, "Attended", 6, new DateTime(2026, 8, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), 6, "Buổi tập Kickfit cường độ tối đa" },
                    { 21, "Attended", 7, new DateTime(2026, 10, 6, 18, 30, 0, 0, DateTimeKind.Unspecified), 7, "Không khí lớp học sôi nổi nhiệt huyết" },
                    { 22, "Absent", 10, new DateTime(2026, 10, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), 9, "Vắng mặt buổi tập đối kháng" },
                    { 23, "Attended", 1, new DateTime(2026, 11, 9, 6, 30, 0, 0, DateTimeKind.Unspecified), 3, "Tập thở Pranayama nâng cao" },
                    { 24, "Attended", 2, new DateTime(2026, 11, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), 1, "Kiểm tra kỹ thuật gạt đòn và phản công" }
                });

            migrationBuilder.InsertData(
                table: "PTs",
                columns: new[] { "PTID", "AccountID", "BranchID", "DateOfBirth", "Email", "Experience", "FullName", "Gender", "Phone", "Specialization", "Status" },
                values: new object[,]
                {
                    { 6, 17, 1, new DateTime(1996, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "hoangnam.le@quanlygym.vn", 5, "Lê Hoàng Nam", "Nam", "0976123456", "Zumba Dance Fitness & Cardio", true },
                    { 7, 18, 1, new DateTime(1998, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "thuthao.dang@quanlygym.vn", 4, "Đặng Thu Thảo", "Nữ", "0938112233", "Pilates Định Hình & Phục Hồi Cột Sống", true },
                    { 8, 19, 2, new DateTime(1996, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "ngocanh.pham@quanlygym.vn", 5, "Phạm Ngọc Ánh", "Nữ", "0918776655", "Yoga Căn Bản & Thiền Định", true },
                    { 9, 20, 2, new DateTime(1995, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "quocbao.tran@quanlygym.vn", 6, "Trần Quốc Bảo", "Nam", "0903445566", "Pilates Định Hình & Phục Hồi Cột Sống", true },
                    { 10, 21, 3, new DateTime(1997, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "khanhlinh.do@quanlygym.vn", 5, "Đỗ Khánh Linh", "Nữ", "0922334455", "Yoga Căn Bản & Thiền Định", true },
                    { 11, 22, 3, new DateTime(1996, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "tiendat.nguyen@quanlygym.vn", 4, "Nguyễn Tiến Đạt", "Nam", "0933557799", "Zumba Dance Fitness & Cardio", true },
                    { 12, 23, 3, new DateTime(1999, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "kimngan.hoang@quanlygym.vn", 4, "Hoàng Kim Ngân", "Nữ", "0944668800", "Pilates Định Hình & Phục Hồi Cột Sống", true }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "ClassID", "BranchID", "ClassName", "ClassType", "DayOfWeek", "Description", "EndTime", "MaxCapacity", "PTID", "RoomID", "StartTime", "Status" },
                values: new object[,]
                {
                    { 3, 1, "Lớp Zumba Dance Đốt Cháy Calo", "Zumba", "Thứ 3 - 5 - 7", "Vũ điệu Zumba Latin sôi động trên nền nhạc lôi cuốn, hỗ trợ đốt cháy 600-800 kcal mỗi buổi tập.", new TimeSpan(0, 19, 45, 0, 0), 30, 6, 2, new TimeSpan(0, 18, 30, 0, 0), true },
                    { 4, 1, "Lớp Pilates Định Hình & Cột Sống", "Pilates", "Thứ 3 - 5 - 7", "Bài tập thảm Pilates tăng cường cơ lõi Core, chỉnh sửa tư thế gù lưng và thon gọn vòng eo.", new TimeSpan(0, 8, 15, 0, 0), 18, 7, 2, new TimeSpan(0, 7, 0, 0, 0), true },
                    { 5, 2, "Lớp Yoga Căn Bản & Thiền Định", "Yoga", "Thứ 2 - 4 - 6", "Lớp Yoga asana kết hợp kỹ thuật thở Pranayama và thiền tịnh tâm, giúp giảm stress và tăng độ dẻo dai.", new TimeSpan(0, 8, 0, 0, 0), 20, 8, 4, new TimeSpan(0, 6, 30, 0, 0), true },
                    { 8, 2, "Lớp Pilates Định Hình & Cột Sống", "Pilates", "Thứ 3 - 5 - 7", "Bài tập thảm Pilates tăng cường cơ lõi Core, chỉnh sửa tư thế gù lưng và thon gọn vòng eo.", new TimeSpan(0, 8, 15, 0, 0), 18, 9, 4, new TimeSpan(0, 7, 0, 0, 0), true },
                    { 9, 3, "Lớp Yoga Căn Bản & Thiền Định", "Yoga", "Thứ 2 - 4 - 6", "Lớp Yoga asana kết hợp kỹ thuật thở Pranayama và thiền tịnh tâm, giúp giảm stress và tăng độ dẻo dai.", new TimeSpan(0, 8, 0, 0, 0), 20, 10, 6, new TimeSpan(0, 6, 30, 0, 0), true },
                    { 11, 3, "Lớp Zumba Dance Đốt Cháy Calo", "Zumba", "Thứ 3 - 5 - 7", "Vũ điệu Zumba Latin sôi động trên nền nhạc lôi cuốn, hỗ trợ đốt cháy 600-800 kcal mỗi buổi tập.", new TimeSpan(0, 19, 45, 0, 0), 30, 11, 6, new TimeSpan(0, 18, 30, 0, 0), true },
                    { 12, 3, "Lớp Pilates Định Hình & Cột Sống", "Pilates", "Thứ 3 - 5 - 7", "Bài tập thảm Pilates tăng cường cơ lõi Core, chỉnh sửa tư thế gù lưng và thon gọn vòng eo.", new TimeSpan(0, 8, 15, 0, 0), 18, 12, 6, new TimeSpan(0, 7, 0, 0, 0), true }
                });

            migrationBuilder.InsertData(
                table: "Staffs",
                columns: new[] { "StaffID", "BaseSalary", "BranchID", "DateOfBirth", "Email", "FullName", "Gender", "PTID", "Phone", "Role", "Status", "WorkShift" },
                values: new object[,]
                {
                    { 15, 15000000m, 1, new DateTime(1996, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "hoangnam.le@quanlygym.vn", "Lê Hoàng Nam", "Nam", 6, "0976123456", "PT", true, "Ca cố định: 14:00 - 22:00 (Thứ 2 - CN)" },
                    { 16, 14500000m, 1, new DateTime(1998, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "thuthao.dang@quanlygym.vn", "Đặng Thu Thảo", "Nữ", 7, "0938112233", "PT", true, "Ca cố định: 06:00 - 14:00 (Thứ 2 - CN)" },
                    { 17, 15000000m, 2, new DateTime(1996, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "ngocanh.pham@quanlygym.vn", "Phạm Ngọc Ánh", "Nữ", 8, "0918776655", "PT", true, "Ca cố định: 06:00 - 14:00 (Thứ 2 - CN)" },
                    { 18, 16000000m, 2, new DateTime(1995, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "quocbao.tran@quanlygym.vn", "Trần Quốc Bảo", "Nam", 9, "0903445566", "PT", true, "Ca cố định: 08:00 - 16:00 (Thứ 2 - CN)" },
                    { 19, 15000000m, 3, new DateTime(1997, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "khanhlinh.do@quanlygym.vn", "Đỗ Khánh Linh", "Nữ", 10, "0922334455", "PT", true, "Ca cố định: 06:00 - 14:00 (Thứ 2 - CN)" },
                    { 20, 14500000m, 3, new DateTime(1996, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "tiendat.nguyen@quanlygym.vn", "Nguyễn Tiến Đạt", "Nam", 11, "0933557799", "PT", true, "Ca cố định: 14:00 - 22:00 (Thứ 2 - CN)" },
                    { 21, 14500000m, 3, new DateTime(1999, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "kimngan.hoang@quanlygym.vn", "Hoàng Kim Ngân", "Nữ", 12, "0944668800", "PT", true, "Ca cố định: 08:00 - 16:00 (Thứ 2 - CN)" }
                });

            migrationBuilder.InsertData(
                table: "ClassEnrollments",
                columns: new[] { "EnrollmentID", "AttendanceStatus", "ClassID", "EnrollmentDate", "MemberID", "Note" },
                values: new object[,]
                {
                    { 5, "Attended", 3, new DateTime(2026, 2, 3, 18, 30, 0, 0, DateTimeKind.Unspecified), 3, "Theo kịp các bước nhảy Latin vui vẻ" },
                    { 6, "Attended", 4, new DateTime(2026, 2, 10, 7, 0, 0, 0, DateTimeKind.Unspecified), 5, "Bài tập cơ bụng thon gọn cùng HLV Thảo" },
                    { 7, "Attended", 5, new DateTime(2026, 3, 9, 6, 30, 0, 0, DateTimeKind.Unspecified), 7, "Dẻo dai khớp vai và cột sống" },
                    { 10, "Attended", 8, new DateTime(2026, 4, 7, 7, 0, 0, 0, DateTimeKind.Unspecified), 7, "Chỉnh sửa tư thế vai gù hiệu quả" },
                    { 11, "Attended", 9, new DateTime(2026, 5, 11, 6, 30, 0, 0, DateTimeKind.Unspecified), 10, "Học viên nắm vững chuỗi bài vinyasa" },
                    { 13, "Absent", 11, new DateTime(2026, 6, 9, 18, 30, 0, 0, DateTimeKind.Unspecified), 10, "Nghỉ không phép buổi tập tối" },
                    { 14, "Attended", 12, new DateTime(2026, 6, 18, 7, 0, 0, 0, DateTimeKind.Unspecified), 9, "Tập trung siết cơ mông đùi" },
                    { 17, "Cancelled", 3, new DateTime(2026, 8, 4, 18, 30, 0, 0, DateTimeKind.Unspecified), 5, "Hủy lịch vì lý do sức khỏe cá nhân" },
                    { 19, "Attended", 4, new DateTime(2026, 9, 8, 7, 0, 0, 0, DateTimeKind.Unspecified), 3, "Cải thiện sức bền cơ bụng rãnh 11" },
                    { 20, "Attended", 5, new DateTime(2026, 9, 21, 6, 30, 0, 0, DateTimeKind.Unspecified), 8, "Thiền định tĩnh tâm cuối buổi tập" },
                    { 25, "Attended", 3, new DateTime(2026, 11, 24, 18, 30, 0, 0, DateTimeKind.Unspecified), 2, "Hội viên tham gia đầy đủ nhiệt tình" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassEnrollments_ClassID",
                table: "ClassEnrollments",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_ClassEnrollments_MemberID",
                table: "ClassEnrollments",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_BranchID",
                table: "Classes",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_PTID",
                table: "Classes",
                column: "PTID");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_RoomID",
                table: "Classes",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AccountID",
                table: "Notifications",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_BranchID",
                table: "Notifications",
                column: "BranchID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassEnrollments");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "CheckIns",
                keyColumn: "CheckInID",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Progresses",
                keyColumn: "ProgressID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Progresses",
                keyColumn: "ProgressID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Schedules",
                keyColumn: "ScheduleID",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "StaffID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "StaffID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "StaffID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "StaffID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "StaffID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "StaffID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Staffs",
                keyColumn: "StaffID",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 23);

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 1,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 500000m, "Bảo dưỡng định kỳ thảm chạy và tra dầu băng chuyền", 1, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 2,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 300000m, "Siết chặt ốc liên kết và kiểm tra thanh an toàn J-Hooks", 2, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 3,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 750000m, "Thay dây cáp thép bọc nylon và bôi trơn ròng rọc", 4, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 4,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 600000m, "Thay bàn đạp dây đai và cảm biến nhịp tim", 5, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 5,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 450000m, "Tra mỡ vòng bi ray trượt máy đạp đùi", 6, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 6,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 800000m, "Căn chỉnh lại hệ thống đối trọng tạ cáp bên phải", 7, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 7,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 900000m, "Bảo trì mô-tơ kháng lực máy trượt tuyết", 8, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 8,
                columns: new[] { "Cost", "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate" },
                values: new object[] { 650000m, "Kiểm tra động cơ 4.0 HP và căn chỉnh độ lệch băng tải", 1, new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "EquipmentMaintenances",
                keyColumn: "MaintenanceID",
                keyValue: 9,
                columns: new[] { "Description", "EquipmentID", "MaintenanceDate", "NextMaintenanceDate", "Status" },
                values: new object[] { "Thay thế chốt tạ selector pin bị kẹt và bảo dưỡng bánh xe dẫn hướng", 7, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Đang xử lý" });

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 9,
                columns: new[] { "Note", "ResolvedDate", "Status" },
                values: new object[] { "Đang chờ nhà cung cấp giao chốt tạ mới thay thế", null, "In Progress" });

            migrationBuilder.UpdateData(
                table: "Incidents",
                keyColumn: "IncidentID",
                keyValue: 10,
                columns: new[] { "Note", "ResolvedDate", "Status" },
                values: new object[] { "Đã giao kỹ thuật viên ca sáng kiểm tra", null, "New" });

            migrationBuilder.UpdateData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 1,
                column: "Specialization",
                value: "Tăng cơ Hypertrophy & Dinh dưỡng");

            migrationBuilder.UpdateData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 2,
                column: "Specialization",
                value: "Giảm mỡ Siết cơ & Pilates/Yoga");

            migrationBuilder.UpdateData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 3,
                column: "Specialization",
                value: "Powerlifting & Thể hình chuyên nghiệp");

            migrationBuilder.UpdateData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 4,
                column: "Specialization",
                value: "Cardio HIIT & Định hình vóc dáng");

            migrationBuilder.UpdateData(
                table: "PTs",
                keyColumn: "PTID",
                keyValue: 5,
                column: "Specialization",
                value: "Phục hồi chức năng & Vận động");
        }
    }
}
