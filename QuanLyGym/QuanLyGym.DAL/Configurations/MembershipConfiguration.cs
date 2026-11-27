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
                // =========================================================================
                // Doanh thu đăng ký thẻ tập full 12 tháng năm 2026 tại 3 chi nhánh
                // =========================================================================

                // Tháng 1/2026
                new Membership { MembershipID = 1, MemberID = 1, PackageID = 4, RegisterDate = new DateTime(2026, 1, 1), StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2027, 1, 1), Price = 12500000, PriceAtPurchase = 12500000, UsedPTSessions = 25, Status = "Active", CreatedAt = new DateTime(2026, 1, 1), BranchID = 1 },
                new Membership { MembershipID = 4, MemberID = 4, PackageID = 1, RegisterDate = new DateTime(2026, 1, 15), StartDate = new DateTime(2026, 1, 15), EndDate = new DateTime(2026, 4, 15), Price = 500000, PriceAtPurchase = 500000, UsedPTSessions = 14, Status = "Active", CreatedAt = new DateTime(2026, 1, 15), BranchID = 1 },

                // Tháng 2/2026
                new Membership { MembershipID = 2, MemberID = 2, PackageID = 3, RegisterDate = new DateTime(2026, 2, 1), StartDate = new DateTime(2026, 2, 1), EndDate = new DateTime(2026, 8, 1), Price = 5800000, PriceAtPurchase = 5800000, UsedPTSessions = 18, Status = "Active", CreatedAt = new DateTime(2026, 2, 1), BranchID = 1 },
                new Membership { MembershipID = 5, MemberID = 5, PackageID = 1, RegisterDate = new DateTime(2026, 2, 1), StartDate = new DateTime(2026, 2, 1), EndDate = new DateTime(2026, 5, 1), Price = 500000, PriceAtPurchase = 500000, UsedPTSessions = 12, Status = "Active", CreatedAt = new DateTime(2026, 2, 1), BranchID = 1 },
                new Membership { MembershipID = 6, MemberID = 6, PackageID = 1, RegisterDate = new DateTime(2026, 2, 1), StartDate = new DateTime(2026, 2, 1), EndDate = new DateTime(2026, 5, 1), Price = 500000, PriceAtPurchase = 500000, UsedPTSessions = 8, Status = "Active", CreatedAt = new DateTime(2026, 2, 1), BranchID = 2 },
                new Membership { MembershipID = 7, MemberID = 7, PackageID = 1, RegisterDate = new DateTime(2026, 2, 10), StartDate = new DateTime(2026, 2, 10), EndDate = new DateTime(2026, 5, 10), Price = 500000, PriceAtPurchase = 500000, UsedPTSessions = 6, Status = "Active", CreatedAt = new DateTime(2026, 2, 10), BranchID = 2 },
                new Membership { MembershipID = 8, MemberID = 8, PackageID = 1, RegisterDate = new DateTime(2026, 2, 15), StartDate = new DateTime(2026, 2, 15), EndDate = new DateTime(2026, 5, 15), Price = 500000, PriceAtPurchase = 500000, UsedPTSessions = 15, Status = "Active", CreatedAt = new DateTime(2026, 2, 15), BranchID = 2 },
                new Membership { MembershipID = 9, MemberID = 9, PackageID = 1, RegisterDate = new DateTime(2026, 2, 20), StartDate = new DateTime(2026, 2, 20), EndDate = new DateTime(2026, 5, 20), Price = 500000, PriceAtPurchase = 500000, UsedPTSessions = 9, Status = "Active", CreatedAt = new DateTime(2026, 2, 20), BranchID = 3 },

                // Tháng 3/2026
                new Membership { MembershipID = 3, MemberID = 3, PackageID = 2, RegisterDate = new DateTime(2026, 3, 1), StartDate = new DateTime(2026, 3, 1), EndDate = new DateTime(2026, 6, 1), Price = 2800000, PriceAtPurchase = 2800000, UsedPTSessions = 10, Status = "Active", CreatedAt = new DateTime(2026, 3, 1), BranchID = 1 },
                new Membership { MembershipID = 10, MemberID = 10, PackageID = 1, RegisterDate = new DateTime(2026, 3, 1), StartDate = new DateTime(2026, 3, 1), EndDate = new DateTime(2026, 6, 1), Price = 500000, PriceAtPurchase = 500000, UsedPTSessions = 5, Status = "Active", CreatedAt = new DateTime(2026, 3, 1), BranchID = 3 },
                new Membership { MembershipID = 11, MemberID = 6, PackageID = 3, RegisterDate = new DateTime(2026, 3, 15), StartDate = new DateTime(2026, 3, 15), EndDate = new DateTime(2026, 9, 15), Price = 5800000, PriceAtPurchase = 5800000, UsedPTSessions = 14, Status = "Active", CreatedAt = new DateTime(2026, 3, 15), BranchID = 2 },

                // Tháng 4/2026
                new Membership { MembershipID = 12, MemberID = 4, PackageID = 2, RegisterDate = new DateTime(2026, 4, 16), StartDate = new DateTime(2026, 4, 16), EndDate = new DateTime(2026, 7, 16), Price = 2800000, PriceAtPurchase = 2800000, UsedPTSessions = 8, Status = "Active", CreatedAt = new DateTime(2026, 4, 16), BranchID = 1 },
                new Membership { MembershipID = 13, MemberID = 7, PackageID = 1, RegisterDate = new DateTime(2026, 4, 20), StartDate = new DateTime(2026, 4, 20), EndDate = new DateTime(2026, 7, 20), Price = 500000, PriceAtPurchase = 500000, UsedPTSessions = 10, Status = "Active", CreatedAt = new DateTime(2026, 4, 20), BranchID = 2 },

                // Tháng 5/2026
                new Membership { MembershipID = 14, MemberID = 5, PackageID = 2, RegisterDate = new DateTime(2026, 5, 5), StartDate = new DateTime(2026, 5, 5), EndDate = new DateTime(2026, 8, 5), Price = 2800000, PriceAtPurchase = 2800000, UsedPTSessions = 11, Status = "Active", CreatedAt = new DateTime(2026, 5, 5), BranchID = 1 },
                new Membership { MembershipID = 15, MemberID = 9, PackageID = 3, RegisterDate = new DateTime(2026, 5, 22), StartDate = new DateTime(2026, 5, 22), EndDate = new DateTime(2026, 11, 22), Price = 5800000, PriceAtPurchase = 5800000, UsedPTSessions = 20, Status = "Active", CreatedAt = new DateTime(2026, 5, 22), BranchID = 3 },

                // Tháng 6/2026
                new Membership { MembershipID = 16, MemberID = 8, PackageID = 2, RegisterDate = new DateTime(2026, 6, 10), StartDate = new DateTime(2026, 6, 10), EndDate = new DateTime(2026, 9, 10), Price = 2800000, PriceAtPurchase = 2800000, UsedPTSessions = 9, Status = "Active", CreatedAt = new DateTime(2026, 6, 10), BranchID = 2 },
                new Membership { MembershipID = 17, MemberID = 10, PackageID = 1, RegisterDate = new DateTime(2026, 6, 15), StartDate = new DateTime(2026, 6, 15), EndDate = new DateTime(2026, 9, 15), Price = 500000, PriceAtPurchase = 500000, UsedPTSessions = 7, Status = "Active", CreatedAt = new DateTime(2026, 6, 15), BranchID = 3 },

                // Tháng 7/2026
                new Membership { MembershipID = 18, MemberID = 2, PackageID = 4, RegisterDate = new DateTime(2026, 7, 1), StartDate = new DateTime(2026, 7, 1), EndDate = new DateTime(2027, 7, 1), Price = 12500000, PriceAtPurchase = 12500000, UsedPTSessions = 22, Status = "Active", CreatedAt = new DateTime(2026, 7, 1), BranchID = 1 },
                new Membership { MembershipID = 19, MemberID = 3, PackageID = 3, RegisterDate = new DateTime(2026, 7, 15), StartDate = new DateTime(2026, 7, 15), EndDate = new DateTime(2027, 1, 15), Price = 5800000, PriceAtPurchase = 5800000, UsedPTSessions = 16, Status = "Active", CreatedAt = new DateTime(2026, 7, 15), BranchID = 1 },

                // Tháng 8/2026
                new Membership { MembershipID = 20, MemberID = 6, PackageID = 2, RegisterDate = new DateTime(2026, 8, 5), StartDate = new DateTime(2026, 8, 5), EndDate = new DateTime(2026, 11, 5), Price = 2800000, PriceAtPurchase = 2800000, UsedPTSessions = 8, Status = "Active", CreatedAt = new DateTime(2026, 8, 5), BranchID = 2 },
                new Membership { MembershipID = 21, MemberID = 10, PackageID = 2, RegisterDate = new DateTime(2026, 8, 20), StartDate = new DateTime(2026, 8, 20), EndDate = new DateTime(2026, 11, 20), Price = 2800000, PriceAtPurchase = 2800000, UsedPTSessions = 6, Status = "Active", CreatedAt = new DateTime(2026, 8, 20), BranchID = 3 },

                // Tháng 9/2026
                new Membership { MembershipID = 22, MemberID = 7, PackageID = 3, RegisterDate = new DateTime(2026, 9, 10), StartDate = new DateTime(2026, 9, 10), EndDate = new DateTime(2027, 3, 10), Price = 5800000, PriceAtPurchase = 5800000, UsedPTSessions = 12, Status = "Active", CreatedAt = new DateTime(2026, 9, 10), BranchID = 2 },
                new Membership { MembershipID = 23, MemberID = 4, PackageID = 1, RegisterDate = new DateTime(2026, 9, 25), StartDate = new DateTime(2026, 9, 25), EndDate = new DateTime(2026, 12, 25), Price = 500000, PriceAtPurchase = 500000, UsedPTSessions = 10, Status = "Active", CreatedAt = new DateTime(2026, 9, 25), BranchID = 1 },

                // Tháng 10/2026
                new Membership { MembershipID = 24, MemberID = 9, PackageID = 4, RegisterDate = new DateTime(2026, 10, 5), StartDate = new DateTime(2026, 10, 5), EndDate = new DateTime(2027, 10, 5), Price = 12500000, PriceAtPurchase = 12500000, UsedPTSessions = 15, Status = "Active", CreatedAt = new DateTime(2026, 10, 5), BranchID = 3 },
                new Membership { MembershipID = 25, MemberID = 5, PackageID = 1, RegisterDate = new DateTime(2026, 10, 18), StartDate = new DateTime(2026, 10, 18), EndDate = new DateTime(2027, 1, 18), Price = 500000, PriceAtPurchase = 500000, UsedPTSessions = 5, Status = "Active", CreatedAt = new DateTime(2026, 10, 18), BranchID = 1 },

                // Tháng 11/2026
                new Membership { MembershipID = 26, MemberID = 8, PackageID = 3, RegisterDate = new DateTime(2026, 11, 2), StartDate = new DateTime(2026, 11, 2), EndDate = new DateTime(2027, 5, 2), Price = 5800000, PriceAtPurchase = 5800000, UsedPTSessions = 4, Status = "Active", CreatedAt = new DateTime(2026, 11, 2), BranchID = 2 },
                new Membership { MembershipID = 27, MemberID = 1, PackageID = 3, RegisterDate = new DateTime(2026, 11, 15), StartDate = new DateTime(2026, 11, 15), EndDate = new DateTime(2027, 5, 15), Price = 5800000, PriceAtPurchase = 5800000, UsedPTSessions = 2, Status = "Active", CreatedAt = new DateTime(2026, 11, 15), BranchID = 1 },
                new Membership { MembershipID = 28, MemberID = 3, PackageID = 2, RegisterDate = new DateTime(2026, 11, 22), StartDate = new DateTime(2026, 11, 22), EndDate = new DateTime(2027, 2, 22), Price = 2800000, PriceAtPurchase = 2800000, UsedPTSessions = 1, Status = "Active", CreatedAt = new DateTime(2026, 11, 22), BranchID = 1 },

                // Tháng 12/2026
                new Membership { MembershipID = 29, MemberID = 2, PackageID = 3, RegisterDate = new DateTime(2026, 12, 5), StartDate = new DateTime(2026, 12, 5), EndDate = new DateTime(2027, 6, 5), Price = 5800000, PriceAtPurchase = 5800000, UsedPTSessions = 0, Status = "Active", CreatedAt = new DateTime(2026, 12, 5), BranchID = 1 },
                new Membership { MembershipID = 30, MemberID = 6, PackageID = 4, RegisterDate = new DateTime(2026, 12, 12), StartDate = new DateTime(2026, 12, 12), EndDate = new DateTime(2027, 12, 12), Price = 12500000, PriceAtPurchase = 12500000, UsedPTSessions = 0, Status = "Active", CreatedAt = new DateTime(2026, 12, 12), BranchID = 2 },
                new Membership { MembershipID = 31, MemberID = 10, PackageID = 2, RegisterDate = new DateTime(2026, 12, 20), StartDate = new DateTime(2026, 12, 20), EndDate = new DateTime(2027, 3, 20), Price = 2800000, PriceAtPurchase = 2800000, UsedPTSessions = 0, Status = "Active", CreatedAt = new DateTime(2026, 12, 20), BranchID = 3 }
            );
        }
    }
}
