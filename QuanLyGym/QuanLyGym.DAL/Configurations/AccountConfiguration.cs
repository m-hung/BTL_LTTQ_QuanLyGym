using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        // Chuỗi băm PBKDF2 của mật khẩu mẫu "123456" (salt cố định để seed không đổi giữa các lần build,
        // tránh EF báo "pending model changes"). Định dạng khớp với QuanLyGym.BLL.Services.PasswordHasher.
        private const string SeedPasswordHash = "PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=";

        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasIndex(a => a.Email).IsUnique();

            builder.HasData(
                // 1 Admin
                new Account { AccountID = 1, Username = "admin", Email = "admingym5@gmail.com", PasswordHash = SeedPasswordHash, Role = "Admin", Status = true, CreatedAt = new DateTime(2026, 1, 1) },

                // 5 PTs
                new Account { AccountID = 2, Username = "pt_nam", Email = "nam.nguyen@quanlygym.vn", PasswordHash = SeedPasswordHash, Role = "PT", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 3, Username = "pt_bichtran", Email = "bich.tran@quanlygym.vn", PasswordHash = SeedPasswordHash, Role = "PT", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 4, Username = "pt_ducminh", Email = "minh.duc@quanlygym.vn", PasswordHash = SeedPasswordHash, Role = "PT", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 5, Username = "pt_thaophan", Email = "thao.phan@quanlygym.vn", PasswordHash = SeedPasswordHash, Role = "PT", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 6, Username = "pt_hoanglong", Email = "long.vu@quanlygym.vn", PasswordHash = SeedPasswordHash, Role = "PT", Status = true, CreatedAt = new DateTime(2026, 1, 1) },

                // 10 Members
                new Account { AccountID = 7, Username = "member_khanhhung", Email = "nguyenkhanhhung2512@gmail.com", PasswordHash = SeedPasswordHash, Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 8, Username = "member_minhhung", Email = "minhhungthai7@gmail.com", PasswordHash = SeedPasswordHash, Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 9, Username = "member_hoa", Email = "hoa.pham@gmail.com", PasswordHash = SeedPasswordHash, Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 10, Username = "member_dung", Email = "dung.hoang@gmail.com", PasswordHash = SeedPasswordHash, Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 11, Username = "member_phuong", Email = "phuong.do@gmail.com", PasswordHash = SeedPasswordHash, Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 12, Username = "member_tuan", Email = "tuan.ngo@gmail.com", PasswordHash = SeedPasswordHash, Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 13, Username = "member_mai", Email = "mai.bui@gmail.com", PasswordHash = SeedPasswordHash, Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 14, Username = "member_vy", Email = "vy.trinh@gmail.com", PasswordHash = SeedPasswordHash, Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 15, Username = "member_khoa", Email = "khoa.dang@gmail.com", PasswordHash = SeedPasswordHash, Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 16, Username = "member_tram", Email = "tram.vo@gmail.com", PasswordHash = SeedPasswordHash, Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) }
            );
        }
    }
}
