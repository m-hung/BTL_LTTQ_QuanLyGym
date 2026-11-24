using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyGym.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailToAccountsAndPasswordResets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Accounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PasswordResets",
                columns: table => new
                {
                    PasswordResetID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    CodeHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FailedAttempts = table.Column<int>(type: "int", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResets", x => x.PasswordResetID);
                    table.ForeignKey(
                        name: "FK_PasswordResets_Accounts_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 1,
                column: "Email",
                value: "admingym5@gmail.com");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 2,
                column: "Email",
                value: "nam.nguyen@quanlygym.vn");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 3,
                column: "Email",
                value: "bich.tran@quanlygym.vn");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 4,
                column: "Email",
                value: "minh.duc@quanlygym.vn");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 5,
                column: "Email",
                value: "thao.phan@quanlygym.vn");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 6,
                column: "Email",
                value: "long.vu@quanlygym.vn");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 7,
                column: "Email",
                value: "nguyenkhanhhung2512@gmail.com");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 8,
                column: "Email",
                value: "minhhungthai7@gmail.com");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 9,
                column: "Email",
                value: "hoa.pham@gmail.com");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 10,
                column: "Email",
                value: "dung.hoang@gmail.com");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 11,
                column: "Email",
                value: "phuong.do@gmail.com");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 12,
                column: "Email",
                value: "tuan.ngo@gmail.com");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 13,
                column: "Email",
                value: "mai.bui@gmail.com");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 14,
                column: "Email",
                value: "vy.trinh@gmail.com");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 15,
                column: "Email",
                value: "khoa.dang@gmail.com");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "AccountID",
                keyValue: 16,
                column: "Email",
                value: "tram.vo@gmail.com");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                table: "Accounts",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResets_AccountID_CreatedAt",
                table: "PasswordResets",
                columns: new[] { "AccountID", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordResets");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_Email",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Accounts");
        }
    }
}
