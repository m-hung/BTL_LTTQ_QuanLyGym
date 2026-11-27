using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLyGym.DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20261127110000_AddQrCheckIn")]
    public sealed class AddQrCheckIn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using var stream = typeof(AddQrCheckIn).Assembly.GetManifestResourceStream("QuanLyGym.DAL.UpdateQrCheckIn.sql")
                ?? throw new InvalidOperationException("Missing QR check-in schema script.");
            using var reader = new StreamReader(stream);
            migrationBuilder.Sql(reader.ReadToEnd());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Members_QrCardId", table: "Members");
            migrationBuilder.DropColumn(name: "QrCardId", table: "Members");
            migrationBuilder.DropColumn(name: "Method", table: "CheckIns");
        }
    }
}
