using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasData(
                // Branch 1 (Quận 1): 5 hội viên
                new Member { MemberID = 1, AccountID = 7, FullName = "Trần Văn Hùng", Gender = "Nam", DateOfBirth = new DateTime(2000, 10, 10), Phone = "0977777777", Email = "hung.tran@gmail.com", Address = "15 Lê Lợi, Quận 1, TP. HCM", Avatar = "", CreatedAt = new DateTime(2026, 1, 1), BranchID = 1 },
                new Member { MemberID = 2, AccountID = 8, FullName = "Lê Thị Lan", Gender = "Nữ", DateOfBirth = new DateTime(1999, 4, 15), Phone = "0912345678", Email = "lan.le@gmail.com", Address = "88 Pasteur, Quận 1, TP. HCM", Avatar = "", CreatedAt = new DateTime(2026, 1, 5), BranchID = 1 },
                new Member { MemberID = 3, AccountID = 9, FullName = "Phạm Quỳnh Hoa", Gender = "Nữ", DateOfBirth = new DateTime(2001, 8, 22), Phone = "0934567890", Email = "hoa.pham@gmail.com", Address = "12 Nguyễn Huệ, Quận 1, TP. HCM", Avatar = "", CreatedAt = new DateTime(2026, 1, 10), BranchID = 1 },
                new Member { MemberID = 4, AccountID = 10, FullName = "Hoàng Tiến Dũng", Gender = "Nam", DateOfBirth = new DateTime(1996, 12, 5), Phone = "0945678901", Email = "dung.hoang@gmail.com", Address = "204 Hai Bà Trưng, Quận 1, TP. HCM", Avatar = "", CreatedAt = new DateTime(2026, 1, 15), BranchID = 1 },
                new Member { MemberID = 5, AccountID = 11, FullName = "Đỗ Thu Phương", Gender = "Nữ", DateOfBirth = new DateTime(1998, 3, 30), Phone = "0956789012", Email = "phuong.do@gmail.com", Address = "55 Nam Kỳ Khởi Nghĩa, Quận 1, TP. HCM", Avatar = "", CreatedAt = new DateTime(2026, 1, 20), BranchID = 1 },

                // Branch 2 (Bình Thạnh): 3 hội viên
                new Member { MemberID = 6, AccountID = 12, FullName = "Ngô Anh Tuấn", Gender = "Nam", DateOfBirth = new DateTime(1995, 7, 18), Phone = "0967890123", Email = "tuan.ngo@gmail.com", Address = "32 Bạch Đằng, Quận Bình Thạnh, TP. HCM", Avatar = "", CreatedAt = new DateTime(2026, 2, 1), BranchID = 2 },
                new Member { MemberID = 7, AccountID = 13, FullName = "Bùi Tuyết Mai", Gender = "Nữ", DateOfBirth = new DateTime(2002, 2, 14), Phone = "0978901234", Email = "mai.bui@gmail.com", Address = "150 Nơ Trang Long, Quận Bình Thạnh, TP. HCM", Avatar = "", CreatedAt = new DateTime(2026, 2, 5), BranchID = 2 },
                new Member { MemberID = 8, AccountID = 14, FullName = "Trịnh Thảo Vy", Gender = "Nữ", DateOfBirth = new DateTime(1997, 9, 9), Phone = "0989012345", Email = "vy.trinh@gmail.com", Address = "82 Xô Viết Nghệ Tĩnh, Quận Bình Thạnh, TP. HCM", Avatar = "", CreatedAt = new DateTime(2026, 2, 10), BranchID = 2 },

                // Branch 3 (Quận 7): 2 hội viên
                new Member { MemberID = 9, AccountID = 15, FullName = "Đặng Đăng Khoa", Gender = "Nam", DateOfBirth = new DateTime(1993, 11, 28), Phone = "0990123456", Email = "khoa.dang@gmail.com", Address = "100 Huỳnh Tấn Phát, Quận 7, TP. HCM", Avatar = "", CreatedAt = new DateTime(2026, 2, 15), BranchID = 3 },
                new Member { MemberID = 10, AccountID = 16, FullName = "Võ Bảo Trâm", Gender = "Nữ", DateOfBirth = new DateTime(2000, 6, 16), Phone = "0901234567", Email = "tram.vo@gmail.com", Address = "45 Nguyễn Thị Thập, Quận 7, TP. HCM", Avatar = "", CreatedAt = new DateTime(2026, 2, 20), BranchID = 3 }
            );
        }
    }
}
