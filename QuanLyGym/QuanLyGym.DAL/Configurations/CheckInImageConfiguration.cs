using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class CheckInImageConfiguration : IEntityTypeConfiguration<CheckInImage>
    {
        public void Configure(EntityTypeBuilder<CheckInImage> builder)
        {
            builder.HasData(
                new CheckInImage { ImageID = 1, CheckInID = 1, ImagePath = "uploads/checkin_001.jpg", UploadedAt = new DateTime(2026, 10, 20, 8, 26, 0), VerifiedBy = 1, VerificationNote = "Ảnh khuôn mặt rõ nét, khớp hồ sơ" },
                new CheckInImage { ImageID = 2, CheckInID = 2, ImagePath = "uploads/checkin_002.jpg", UploadedAt = new DateTime(2026, 10, 21, 13, 51, 0), VerifiedBy = 1, VerificationNote = "Nhận diện khuôn mặt hợp lệ" },
                new CheckInImage { ImageID = 3, CheckInID = 3, ImagePath = "uploads/checkin_003.jpg", UploadedAt = new DateTime(2026, 10, 21, 15, 56, 0), VerifiedBy = 2, VerificationNote = "Xác thực hội viên thành công" },
                new CheckInImage { ImageID = 4, CheckInID = 4, ImagePath = "uploads/checkin_004.jpg", UploadedAt = new DateTime(2026, 10, 22, 8, 31, 0), VerifiedBy = 1, VerificationNote = "Hợp lệ" },
                new CheckInImage { ImageID = 5, CheckInID = 5, ImagePath = "uploads/checkin_005.jpg", UploadedAt = new DateTime(2026, 10, 22, 17, 16, 0), VerifiedBy = 1, VerificationNote = "Check-in camera sảnh lễ tân" },
                new CheckInImage { ImageID = 6, CheckInID = 6, ImagePath = "uploads/checkin_006.jpg", UploadedAt = new DateTime(2026, 10, 23, 7, 6, 0), VerifiedBy = 1, VerificationNote = "Check-in camera sảnh lễ tân" },
                new CheckInImage { ImageID = 7, CheckInID = 7, ImagePath = "uploads/checkin_007.jpg", UploadedAt = new DateTime(2026, 10, 23, 17, 26, 0), VerifiedBy = 2, VerificationNote = "Hội viên Bích phụ trách" },
                new CheckInImage { ImageID = 8, CheckInID = 8, ImagePath = "uploads/checkin_008.jpg", UploadedAt = new DateTime(2026, 10, 24, 14, 56, 0), VerifiedBy = 1, VerificationNote = "Xác nhận đúng lịch hẹn" },
                new CheckInImage { ImageID = 9, CheckInID = 9, ImagePath = "uploads/checkin_009.jpg", UploadedAt = new DateTime(2026, 10, 24, 18, 1, 0), VerifiedBy = 3, VerificationNote = "Check-in cửa từ Bình Thạnh" },
                new CheckInImage { ImageID = 10, CheckInID = 10, ImagePath = "uploads/checkin_010.jpg", UploadedAt = new DateTime(2026, 10, 25, 7, 56, 0), VerifiedBy = 3, VerificationNote = "PT Minh duyệt check-in" },
                new CheckInImage { ImageID = 11, CheckInID = 11, ImagePath = "uploads/checkin_011.jpg", UploadedAt = new DateTime(2026, 10, 25, 16, 31, 0), VerifiedBy = 4, VerificationNote = "Cửa từ hợp lệ" },
                new CheckInImage { ImageID = 12, CheckInID = 12, ImagePath = "uploads/checkin_012.jpg", UploadedAt = new DateTime(2026, 10, 25, 19, 11, 0), VerifiedBy = 4, VerificationNote = "Cửa từ hợp lệ" },
                new CheckInImage { ImageID = 13, CheckInID = 13, ImagePath = "uploads/checkin_013.jpg", UploadedAt = new DateTime(2026, 10, 25, 6, 46, 0), VerifiedBy = 5, VerificationNote = "Lễ tân chi nhánh Q7 duyệt" },
                new CheckInImage { ImageID = 14, CheckInID = 14, ImagePath = "uploads/checkin_014.jpg", UploadedAt = new DateTime(2026, 10, 25, 18, 21, 0), VerifiedBy = 5, VerificationNote = "Cửa từ Q7 hợp lệ" },
                new CheckInImage { ImageID = 15, CheckInID = 15, ImagePath = "uploads/checkin_015.jpg", UploadedAt = new DateTime(2026, 10, 26, 8, 21, 0), VerifiedBy = 1, VerificationNote = "Ảnh selfie check-in tại quầy Q1" }
            );
        }
    }
}
