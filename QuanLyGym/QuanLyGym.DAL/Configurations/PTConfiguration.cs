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
                // Branch 1 (Quận 1): 4 PTs phụ trách 4 lớp học nhóm & kèm 1-1
                new PT { PTID = 1, AccountID = 2, FullName = "Nguyễn Văn Nam", Gender = "Nam", DateOfBirth = new DateTime(1995, 5, 20), Phone = "0988888888", Email = "nam.nguyen@quanlygym.vn", Specialization = "Boxing & Kickfit Cường độ cao", Experience = 6, Status = true, BranchID = 1 },
                new PT { PTID = 2, AccountID = 3, FullName = "Trần Thị Bích", Gender = "Nữ", DateOfBirth = new DateTime(1997, 8, 14), Phone = "0943210987", Email = "bich.tran@quanlygym.vn", Specialization = "Yoga Căn Bản & Thiền Định", Experience = 5, Status = true, BranchID = 1 },
                new PT { PTID = 6, AccountID = 17, FullName = "Lê Hoàng Nam", Gender = "Nam", DateOfBirth = new DateTime(1996, 3, 10), Phone = "0976123456", Email = "hoangnam.le@quanlygym.vn", Specialization = "Zumba Dance Fitness & Cardio", Experience = 5, Status = true, BranchID = 1 },
                new PT { PTID = 7, AccountID = 18, FullName = "Đặng Thu Thảo", Gender = "Nữ", DateOfBirth = new DateTime(1998, 10, 22), Phone = "0938112233", Email = "thuthao.dang@quanlygym.vn", Specialization = "Pilates Định Hình & Phục Hồi Cột Sống", Experience = 4, Status = true, BranchID = 1 },

                // Branch 2 (Bình Thạnh): 4 PTs phụ trách 4 lớp học nhóm & kèm 1-1
                new PT { PTID = 3, AccountID = 4, FullName = "Nguyễn Đức Minh", Gender = "Nam", DateOfBirth = new DateTime(1994, 3, 25), Phone = "0987543210", Email = "minh.duc@quanlygym.vn", Specialization = "Boxing & Kickfit Cường độ cao", Experience = 7, Status = true, BranchID = 2 },
                new PT { PTID = 4, AccountID = 5, FullName = "Phan Thị Thảo", Gender = "Nữ", DateOfBirth = new DateTime(1998, 11, 12), Phone = "0921098765", Email = "thao.phan@quanlygym.vn", Specialization = "Zumba Dance Fitness & Aerobic", Experience = 4, Status = true, BranchID = 2 },
                new PT { PTID = 8, AccountID = 19, FullName = "Phạm Ngọc Ánh", Gender = "Nữ", DateOfBirth = new DateTime(1996, 7, 19), Phone = "0918776655", Email = "ngocanh.pham@quanlygym.vn", Specialization = "Yoga Căn Bản & Thiền Định", Experience = 5, Status = true, BranchID = 2 },
                new PT { PTID = 9, AccountID = 20, FullName = "Trần Quốc Bảo", Gender = "Nam", DateOfBirth = new DateTime(1995, 12, 5), Phone = "0903445566", Email = "quocbao.tran@quanlygym.vn", Specialization = "Pilates Định Hình & Phục Hồi Cột Sống", Experience = 6, Status = true, BranchID = 2 },

                // Branch 3 (Quận 7): 4 PTs phụ trách 4 lớp học nhóm & kèm 1-1
                new PT { PTID = 5, AccountID = 6, FullName = "Vũ Hoàng Long", Gender = "Nam", DateOfBirth = new DateTime(1996, 9, 8), Phone = "0909876543", Email = "long.vu@quanlygym.vn", Specialization = "Boxing & Kickfit Cường độ cao", Experience = 5, Status = true, BranchID = 3 },
                new PT { PTID = 10, AccountID = 21, FullName = "Đỗ Khánh Linh", Gender = "Nữ", DateOfBirth = new DateTime(1997, 4, 15), Phone = "0922334455", Email = "khanhlinh.do@quanlygym.vn", Specialization = "Yoga Căn Bản & Thiền Định", Experience = 5, Status = true, BranchID = 3 },
                new PT { PTID = 11, AccountID = 22, FullName = "Nguyễn Tiến Đạt", Gender = "Nam", DateOfBirth = new DateTime(1996, 11, 30), Phone = "0933557799", Email = "tiendat.nguyen@quanlygym.vn", Specialization = "Zumba Dance Fitness & Cardio", Experience = 4, Status = true, BranchID = 3 },
                new PT { PTID = 12, AccountID = 23, FullName = "Hoàng Kim Ngân", Gender = "Nữ", DateOfBirth = new DateTime(1999, 8, 8), Phone = "0944668800", Email = "kimngan.hoang@quanlygym.vn", Specialization = "Pilates Định Hình & Phục Hồi Cột Sống", Experience = 4, Status = true, BranchID = 3 }
            );
        }
    }
}
