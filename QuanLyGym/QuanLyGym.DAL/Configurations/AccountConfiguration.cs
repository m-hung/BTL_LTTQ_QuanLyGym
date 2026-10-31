using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasData(
                // 1 Admin
                new Account { AccountID = 1, Username = "admin", PasswordHash = "123456", Role = "Admin", Status = true, CreatedAt = new DateTime(2026, 1, 1) },

                // 5 PTs
                new Account { AccountID = 2, Username = "pt_nam", PasswordHash = "123456", Role = "PT", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 3, Username = "pt_bichtran", PasswordHash = "123456", Role = "PT", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 4, Username = "pt_ducminh", PasswordHash = "123456", Role = "PT", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 5, Username = "pt_thaophan", PasswordHash = "123456", Role = "PT", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 6, Username = "pt_hoanglong", PasswordHash = "123456", Role = "PT", Status = true, CreatedAt = new DateTime(2026, 1, 1) },

                // 10 Members
                new Account { AccountID = 7, Username = "member_khanhhung", PasswordHash = "123456", Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 8, Username = "member_minhhung", PasswordHash = "123456", Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 9, Username = "member_hoa", PasswordHash = "123456", Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 10, Username = "member_dung", PasswordHash = "123456", Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 11, Username = "member_phuong", PasswordHash = "123456", Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 12, Username = "member_tuan", PasswordHash = "123456", Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 13, Username = "member_mai", PasswordHash = "123456", Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 14, Username = "member_vy", PasswordHash = "123456", Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 15, Username = "member_khoa", PasswordHash = "123456", Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 16, Username = "member_tram", PasswordHash = "123456", Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) }
            );
        }
    }
}
