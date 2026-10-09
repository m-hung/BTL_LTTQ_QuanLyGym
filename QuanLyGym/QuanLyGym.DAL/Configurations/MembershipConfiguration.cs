using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.HasData(
                new Membership { MembershipID = 1, MemberID = 1, PackageID = 2, RegisterDate = new DateTime(2026, 1, 1), StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 4, 1), Price = 3000000, UsedSessions = 5, Status = "Active", CreatedAt = new DateTime(2026, 1, 1), BranchID = 1 }
            );
        }
    }
}