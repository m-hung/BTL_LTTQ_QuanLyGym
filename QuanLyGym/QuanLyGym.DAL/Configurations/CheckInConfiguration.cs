using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class CheckInConfiguration : IEntityTypeConfiguration<CheckIn>
    {
        public void Configure(EntityTypeBuilder<CheckIn> builder)
        {
            builder.HasData(
                new CheckIn { CheckInID = 1, MemberID = 1, PTID = 1, CheckInTime = new DateTime(2026, 10, 20, 8, 25, 0), Status = "Approved", Note = "Check-in tập buổi ngực cùng PT Nam", BranchID = 1 },
                new CheckIn { CheckInID = 2, MemberID = 2, PTID = 1, CheckInTime = new DateTime(2026, 10, 21, 13, 50, 0), Status = "Approved", Note = "Check-in tập Cardio cùng PT Nam", BranchID = 1 },
                new CheckIn { CheckInID = 3, MemberID = 3, PTID = 2, CheckInTime = new DateTime(2026, 10, 21, 15, 55, 0), Status = "Approved", Note = "Check-in tập Pilates cùng PT Bích", BranchID = 1 },
                new CheckIn { CheckInID = 4, MemberID = 1, PTID = 1, CheckInTime = new DateTime(2026, 10, 22, 8, 30, 0), Status = "Approved", Note = "Check-in tập Lưng cùng PT Nam", BranchID = 1 },
                new CheckIn { CheckInID = 5, MemberID = 4, PTID = null, CheckInTime = new DateTime(2026, 10, 22, 17, 15, 0), Status = "Approved", Note = "Hội viên Classic tập tự do chiều", BranchID = 1 },
                new CheckIn { CheckInID = 6, MemberID = 5, PTID = null, CheckInTime = new DateTime(2026, 10, 23, 7, 5, 0), Status = "Approved", Note = "Hội viên Classic tập tự do sáng", BranchID = 1 },
                new CheckIn { CheckInID = 7, MemberID = 2, PTID = 2, CheckInTime = new DateTime(2026, 10, 23, 17, 25, 0), Status = "Approved", Note = "Check-in tập Mông & Đùi cùng PT Bích", BranchID = 1 },
                new CheckIn { CheckInID = 8, MemberID = 3, PTID = 1, CheckInTime = new DateTime(2026, 10, 24, 14, 55, 0), Status = "Approved", Note = "Check-in tập Chân cùng PT Nam", BranchID = 1 },
                new CheckIn { CheckInID = 9, MemberID = 6, PTID = null, CheckInTime = new DateTime(2026, 10, 24, 18, 0, 0), Status = "Approved", Note = "Hội viên Classic tập tự do chi nhánh Bình Thạnh", BranchID = 2 },
                new CheckIn { CheckInID = 10, MemberID = 1, PTID = 3, CheckInTime = new DateTime(2026, 10, 25, 7, 55, 0), Status = "Approved", Note = "Check-in tập Powerlifting cùng PT Minh", BranchID = 2 },
                new CheckIn { CheckInID = 11, MemberID = 7, PTID = null, CheckInTime = new DateTime(2026, 10, 25, 16, 30, 0), Status = "Approved", Note = "Hội viên Classic tập Cardio tự do", BranchID = 2 },
                new CheckIn { CheckInID = 12, MemberID = 8, PTID = null, CheckInTime = new DateTime(2026, 10, 25, 19, 10, 0), Status = "Approved", Note = "Hội viên Classic tập tối", BranchID = 2 },
                new CheckIn { CheckInID = 13, MemberID = 9, PTID = null, CheckInTime = new DateTime(2026, 10, 25, 6, 45, 0), Status = "Approved", Note = "Hội viên Classic tập tự do chi nhánh Q7", BranchID = 3 },
                new CheckIn { CheckInID = 14, MemberID = 10, PTID = null, CheckInTime = new DateTime(2026, 10, 25, 18, 20, 0), Status = "Approved", Note = "Hội viên Classic tập tối chi nhánh Q7", BranchID = 3 },
                new CheckIn { CheckInID = 15, MemberID = 1, PTID = 1, CheckInTime = new DateTime(2026, 10, 26, 8, 20, 0), Status = "Approved", Note = "Check-in buổi tập đầu tuần cùng PT Nam", BranchID = 1 }
            );
        }
    }
}
