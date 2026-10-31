using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyGym.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceAtPurchaseToMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceAtPurchase",
                table: "Memberships",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 1,
                column: "PriceAtPurchase",
                value: 12500000m);

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 2,
                column: "PriceAtPurchase",
                value: 5800000m);

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 3,
                column: "PriceAtPurchase",
                value: 2800000m);

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 4,
                column: "PriceAtPurchase",
                value: 500000m);

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 5,
                column: "PriceAtPurchase",
                value: 500000m);

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 6,
                column: "PriceAtPurchase",
                value: 500000m);

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 7,
                column: "PriceAtPurchase",
                value: 500000m);

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 8,
                column: "PriceAtPurchase",
                value: 500000m);

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 9,
                column: "PriceAtPurchase",
                value: 500000m);

            migrationBuilder.UpdateData(
                table: "Memberships",
                keyColumn: "MembershipID",
                keyValue: 10,
                column: "PriceAtPurchase",
                value: 500000m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceAtPurchase",
                table: "Memberships");
        }
    }
}
