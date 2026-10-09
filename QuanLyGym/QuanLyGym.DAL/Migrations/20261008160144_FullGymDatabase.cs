using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyGym.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FullGymDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountID);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    BranchID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.BranchID);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Height = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    BodyFat = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MuscleMass = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    BMI = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberID);
                    table.ForeignKey(
                        name: "FK_Members_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "BranchID");
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    PackageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalSessions = table.Column<int>(type: "int", nullable: false),
                    Benefits = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.PackageID);
                    table.ForeignKey(
                        name: "FK_Packages_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "BranchID");
                });

            migrationBuilder.CreateTable(
                name: "PTs",
                columns: table => new
                {
                    PTID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Experience = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PTs", x => x.PTID);
                    table.ForeignKey(
                        name: "FK_PTs_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PTs_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "BranchID");
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RoomType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomID);
                    table.ForeignKey(
                        name: "FK_Rooms_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "BranchID");
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    MembershipID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    PackageID = table.Column<int>(type: "int", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsedSessions = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.MembershipID);
                    table.ForeignKey(
                        name: "FK_Memberships_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "BranchID");
                    table.ForeignKey(
                        name: "FK_Memberships_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Memberships_Packages_PackageID",
                        column: x => x.PackageID,
                        principalTable: "Packages",
                        principalColumn: "PackageID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckIns",
                columns: table => new
                {
                    CheckInID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    PTID = table.Column<int>(type: "int", nullable: true),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckIns", x => x.CheckInID);
                    table.ForeignKey(
                        name: "FK_CheckIns_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "BranchID");
                    table.ForeignKey(
                        name: "FK_CheckIns_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckIns_PTs_PTID",
                        column: x => x.PTID,
                        principalTable: "PTs",
                        principalColumn: "PTID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Progresses",
                columns: table => new
                {
                    ProgressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    PTID = table.Column<int>(type: "int", nullable: false),
                    RecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    BodyFat = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    MuscleMass = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    BMI = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Chest = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Waist = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Hip = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Arm = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Thigh = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Progresses", x => x.ProgressID);
                    table.ForeignKey(
                        name: "FK_Progresses_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Progresses_PTs_PTID",
                        column: x => x.PTID,
                        principalTable: "PTs",
                        principalColumn: "PTID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkingSchedules",
                columns: table => new
                {
                    WorkingScheduleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PTID = table.Column<int>(type: "int", nullable: false),
                    WorkDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    EquipmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    EquipmentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EquipmentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.EquipmentID);
                    table.ForeignKey(
                        name: "FK_Equipments_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ScheduleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    PTID = table.Column<int>(type: "int", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    TrainingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    TrainingType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ScheduleID);
                    table.ForeignKey(
                        name: "FK_Schedules_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "BranchID");
                    table.ForeignKey(
                        name: "FK_Schedules_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_PTs_PTID",
                        column: x => x.PTID,
                        principalTable: "PTs",
                        principalColumn: "PTID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schedules_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckInImages",
                columns: table => new
                {
                    ImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckInID = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerifiedBy = table.Column<int>(type: "int", nullable: true),
                    VerificationNote = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckInImages", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK_CheckInImages_CheckIns_CheckInID",
                        column: x => x.CheckInID,
                        principalTable: "CheckIns",
                        principalColumn: "CheckInID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckInImages_PTs_VerifiedBy",
                        column: x => x.VerifiedBy,
                        principalTable: "PTs",
                        principalColumn: "PTID");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMaintenances",
                columns: table => new
                {
                    MaintenanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentID = table.Column<int>(type: "int", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextMaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMaintenances", x => x.MaintenanceID);
                    table.ForeignKey(
                        name: "FK_EquipmentMaintenances_Equipments_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    IncidentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentID = table.Column<int>(type: "int", nullable: false),
                    ReportedBy = table.Column<int>(type: "int", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ResolvedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.IncidentID);
                    table.ForeignKey(
                        name: "FK_Incidents_Accounts_ReportedBy",
                        column: x => x.ReportedBy,
                        principalTable: "Accounts",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidents_Equipments_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "Equipments",
                        principalColumn: "EquipmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckInImages_CheckInID",
                table: "CheckInImages",
                column: "CheckInID");

            migrationBuilder.CreateIndex(
                name: "IX_CheckInImages_VerifiedBy",
                table: "CheckInImages",
                column: "VerifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_BranchID",
                table: "CheckIns",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_MemberID",
                table: "CheckIns",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_PTID",
                table: "CheckIns",
                column: "PTID");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaintenances_EquipmentID",
                table: "EquipmentMaintenances",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_RoomID",
                table: "Equipments",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_EquipmentID",
                table: "Incidents",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_ReportedBy",
                table: "Incidents",
                column: "ReportedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Members_AccountID",
                table: "Members",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_BranchID",
                table: "Members",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_BranchID",
                table: "Memberships",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_MemberID",
                table: "Memberships",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_PackageID",
                table: "Memberships",
                column: "PackageID");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_BranchID",
                table: "Packages",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Progresses_MemberID",
                table: "Progresses",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Progresses_PTID",
                table: "Progresses",
                column: "PTID");

            migrationBuilder.CreateIndex(
                name: "IX_PTs_AccountID",
                table: "PTs",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_PTs_BranchID",
                table: "PTs",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BranchID",
                table: "Rooms",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_BranchID",
                table: "Schedules",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_MemberID",
                table: "Schedules",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_PTID",
                table: "Schedules",
                column: "PTID");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_RoomID",
                table: "Schedules",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingSchedules_BranchID",
                table: "WorkingSchedules",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingSchedules_PTID",
                table: "WorkingSchedules",
                column: "PTID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckInImages");

            migrationBuilder.DropTable(
                name: "EquipmentMaintenances");

            migrationBuilder.DropTable(
                name: "Incidents");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "Progresses");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "WorkingSchedules");

            migrationBuilder.DropTable(
                name: "CheckIns");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "PTs");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Branches");
        }
    }
}
