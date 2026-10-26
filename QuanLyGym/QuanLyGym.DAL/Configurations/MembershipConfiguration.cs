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
                // 3 hội viên đăng ký gói có PT (VIP Diamond, Gold, Silver)
                new Membership { MembershipID = 1, MemberID = 1, PackageID = 4, RegisterDate = new DateTime(2026, 1, 1), StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2027, 1, 1), Price = 12500000, UsedSessions = 25, Status = "Active", CreatedAt = new DateTime(2026, 1, 1), BranchID = 1 },
                new Membership { MembershipID = 2, MemberID = 2, PackageID = 3, RegisterDate = new DateTime(2026, 2, 1), StartDate = new DateTime(2026, 2, 1), EndDate = new DateTime(2026, 8, 1), Price = 5800000, UsedSessions = 18, Status = "Active", CreatedAt = new DateTime(2026, 2, 1), BranchID = 1 },
                new Membership { MembershipID = 3, MemberID = 3, PackageID = 2, RegisterDate = new DateTime(2026, 3, 1), StartDate = new DateTime(2026, 3, 1), EndDate = new DateTime(2026, 6, 1), Price = 2800000, UsedSessions = 10, Status = "Active", CreatedAt = new DateTime(2026, 3, 1), BranchID = 1 },

                // Các hội viên còn lại đăng ký gói Classic (Gói tập tự do, không tập với PT)
                new Membership { MembershipID = 4, MemberID = 4, PackageID = 1, RegisterDate = new DateTime(2026, 1, 15), StartDate = new DateTime(2026, 1, 15), EndDate = new DateTime(2026, 4, 15), Price = 500000, UsedSessions = 14, Status = "Active", CreatedAt = new DateTime(2026, 1, 15), BranchID = 1 },
                new Membership { MembershipID = 5, MemberID = 5, PackageID = 1, RegisterDate = new DateTime(2026, 2, 1), StartDate = new DateTime(2026, 2, 1), EndDate = new DateTime(2026, 5, 1), Price = 500000, UsedSessions = 12, Status = "Active", CreatedAt = new DateTime(2026, 2, 1), BranchID = 1 },
                new Membership { MembershipID = 6, MemberID = 6, PackageID = 1, RegisterDate = new DateTime(2026, 2, 1), StartDate = new DateTime(2026, 2, 1), EndDate = new DateTime(2026, 5, 1), Price = 500000, UsedSessions = 8, Status = "Active", CreatedAt = new DateTime(2026, 2, 1), BranchID = 2 },
                new Membership { MembershipID = 7, MemberID = 7, PackageID = 1, RegisterDate = new DateTime(2026, 2, 10), StartDate = new DateTime(2026, 2, 10), EndDate = new DateTime(2026, 5, 10), Price = 500000, UsedSessions = 6, Status = "Active", CreatedAt = new DateTime(2026, 2, 10), BranchID = 2 },
                new Membership { MembershipID = 8, MemberID = 8, PackageID = 1, RegisterDate = new DateTime(2026, 2, 15), StartDate = new DateTime(2026, 2, 15), EndDate = new DateTime(2026, 5, 15), Price = 500000, UsedSessions = 15, Status = "Active", CreatedAt = new DateTime(2026, 2, 15), BranchID = 2 },
                new Membership { MembershipID = 9, MemberID = 9, PackageID = 1, RegisterDate = new DateTime(2026, 2, 20), StartDate = new DateTime(2026, 2, 20), EndDate = new DateTime(2026, 5, 20), Price = 500000, UsedSessions = 9, Status = "Active", CreatedAt = new DateTime(2026, 2, 20), BranchID = 3 },
                new Membership { MembershipID = 10, MemberID = 10, PackageID = 1, RegisterDate = new DateTime(2026, 3, 1), StartDate = new DateTime(2026, 3, 1), EndDate = new DateTime(2026, 6, 1), Price = 500000, UsedSessions = 5, Status = "Active", CreatedAt = new DateTime(2026, 3, 1), BranchID = 3 }
            );
        }
    }
}
