using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class PTConfiguration : IEntityTypeConfiguration<PT>
    {
        public void Configure(EntityTypeBuilder<PT> builder)
        {
            builder.HasData(
                // Branch 1 (Quận 1): 2 PTs
                new PT { PTID = 1, AccountID = 2, FullName = "Nguyễn Văn Nam", Gender = "Nam", DateOfBirth = new DateTime(1995, 5, 20), Phone = "0988888888", Email = "nam.nguyen@quanlygym.vn", Specialization = "Tăng cơ Hypertrophy & Dinh dưỡng", Experience = 6, Status = true, BranchID = 1 },
                new PT { PTID = 2, AccountID = 3, FullName = "Trần Thị Bích", Gender = "Nữ", DateOfBirth = new DateTime(1997, 8, 14), Phone = "0943210987", Email = "bich.tran@quanlygym.vn", Specialization = "Giảm mỡ Siết cơ & Pilates/Yoga", Experience = 5, Status = true, BranchID = 1 },

                // Branch 2 (Bình Thạnh): 2 PTs
                new PT { PTID = 3, AccountID = 4, FullName = "Nguyễn Đức Minh", Gender = "Nam", DateOfBirth = new DateTime(1994, 3, 25), Phone = "0987543210", Email = "minh.duc@quanlygym.vn", Specialization = "Powerlifting & Thể hình chuyên nghiệp", Experience = 7, Status = true, BranchID = 2 },
                new PT { PTID = 4, AccountID = 5, FullName = "Phan Thị Thảo", Gender = "Nữ", DateOfBirth = new DateTime(1998, 11, 12), Phone = "0921098765", Email = "thao.phan@quanlygym.vn", Specialization = "Cardio HIIT & Định hình vóc dáng", Experience = 4, Status = true, BranchID = 2 },

                // Branch 3 (Quận 7): 1 PT
                new PT { PTID = 5, AccountID = 6, FullName = "Vũ Hoàng Long", Gender = "Nam", DateOfBirth = new DateTime(1996, 9, 8), Phone = "0909876543", Email = "long.vu@quanlygym.vn", Specialization = "Phục hồi chức năng & Vận động", Experience = 5, Status = true, BranchID = 3 }
            );
        }
    }
}
