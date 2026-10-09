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
                new Account { AccountID = 1, Username = "admin", PasswordHash = "123456", Role = "Admin", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 2, Username = "pt_nam", PasswordHash = "123456", Role = "PT", Status = true, CreatedAt = new DateTime(2026, 1, 1) },
                new Account { AccountID = 3, Username = "member_hung", PasswordHash = "123456", Role = "Member", Status = true, CreatedAt = new DateTime(2026, 1, 1) }
            );
        }
    }
}