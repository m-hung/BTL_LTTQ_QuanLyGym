using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanLyGym.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePackageBenefits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 1,
                column: "Benefits",
                value: "Tập luyện không giới hạn khung giờ toàn hệ thống, Sử dụng đầy đủ hệ thống máy tạ & Cardio, Đo & phân tích chỉ số InBody định kỳ, Trang bị Smart Locker tủ đồ cá nhân, Miễn phí phòng tắm nóng lạnh & tiện ích");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 2,
                column: "Benefits",
                value: "12 buổi tập 1-1 với Huấn luyện viên, Thiết lập lộ trình cá nhân hóa, Kiểm tra chỉ số InBody hàng tháng, Luyện tập tự do toàn hệ thống Titan Fitness, Miễn phí tủ đồ Smart Locker & nước uống");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 3,
                column: "Benefits",
                value: "24 buổi tập 1-1 với Huấn luyện viên, Tư vấn dinh dưỡng chuyên sâu, Đo InBody định kỳ & theo sát vóc dáng, Sử dụng phòng xông hơi Sauna & Smart Locker riêng, Luyện tập tự do toàn hệ thống Titan Fitness, Miễn phí nước uống mỗi buổi tập");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 4,
                column: "Benefits",
                value: "36 buổi tập 1-1 cùng Huấn luyện viên cao cấp, Bác sĩ dinh dưỡng & HLV theo sát 24/7, Full dịch vụ Sauna VIP & VIP Locker riêng biệt, Miễn phí khăn tập & nước uống mỗi buổi, Luyện tập tự do toàn hệ thống Titan Fitness, Cam kết cải thiện thể hình bằng văn bản");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 1,
                column: "Benefits",
                value: "Tập không giới hạn khung giờ, Sử dụng đầy đủ máy tạ & Cardio, Tủ đồ cá nhân");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 2,
                column: "Benefits",
                value: "12 buổi tập 1-1 với HLV, Thiết lập lộ trình cá nhân hóa, Kiểm tra chỉ số InBody hàng tháng");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 3,
                column: "Benefits",
                value: "24 buổi tập 1-1 với HLV, Tư vấn dinh dưỡng chuyên sâu, Đo InBody định kỳ, Sử dụng phòng xông hơi Sauna");

            migrationBuilder.UpdateData(
                table: "Packages",
                keyColumn: "PackageID",
                keyValue: 4,
                column: "Benefits",
                value: "36 buổi tập 1-1 với HLV cao cấp, Full dịch vụ Sauna & VIP Locker, Miễn phí khăn tập & nước uống mỗi buổi");
        }
    }
}
