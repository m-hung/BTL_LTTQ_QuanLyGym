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
                new CheckInImage { ImageID = 1, CheckInID = 1, ImagePath = "uploads/checkin_001.jpg", UploadedAt = new DateTime(2026, 3, 1, 16, 56, 0), VerifiedBy = 1, VerificationNote = "Ảnh hợp lệ" }
            );
        }
    }
}