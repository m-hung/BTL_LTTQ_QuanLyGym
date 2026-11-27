using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.DAL.Configurations
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.HasData(
                // 12 PT - Ca làm việc cố định tại 3 chi nhánh
                new Staff { StaffID = 1, FullName = "Nguyễn Văn Nam", Role = "PT", Gender = "Nam", DateOfBirth = new DateTime(1995, 5, 20), Phone = "0988888888", Email = "nam.nguyen@quanlygym.vn", WorkShift = "Ca cố định: 08:00 - 16:00 (Thứ 2 - CN)", BaseSalary = 16000000, Status = true, BranchID = 1, PTID = 1 },
                new Staff { StaffID = 2, FullName = "Trần Thị Bích", Role = "PT", Gender = "Nữ", DateOfBirth = new DateTime(1997, 8, 14), Phone = "0943210987", Email = "bich.tran@quanlygym.vn", WorkShift = "Ca cố định: 14:00 - 22:00 (Thứ 2 - CN)", BaseSalary = 15500000, Status = true, BranchID = 1, PTID = 2 },
                new Staff { StaffID = 15, FullName = "Lê Hoàng Nam", Role = "PT", Gender = "Nam", DateOfBirth = new DateTime(1996, 3, 10), Phone = "0976123456", Email = "hoangnam.le@quanlygym.vn", WorkShift = "Ca cố định: 14:00 - 22:00 (Thứ 2 - CN)", BaseSalary = 15000000, Status = true, BranchID = 1, PTID = 6 },
                new Staff { StaffID = 16, FullName = "Đặng Thu Thảo", Role = "PT", Gender = "Nữ", DateOfBirth = new DateTime(1998, 10, 22), Phone = "0938112233", Email = "thuthao.dang@quanlygym.vn", WorkShift = "Ca cố định: 06:00 - 14:00 (Thứ 2 - CN)", BaseSalary = 14500000, Status = true, BranchID = 1, PTID = 7 },

                new Staff { StaffID = 3, FullName = "Nguyễn Đức Minh", Role = "PT", Gender = "Nam", DateOfBirth = new DateTime(1994, 3, 25), Phone = "0987543210", Email = "minh.duc@quanlygym.vn", WorkShift = "Ca cố định: 08:00 - 16:00 (Thứ 2 - CN)", BaseSalary = 16500000, Status = true, BranchID = 2, PTID = 3 },
                new Staff { StaffID = 4, FullName = "Phan Thị Thảo", Role = "PT", Gender = "Nữ", DateOfBirth = new DateTime(1998, 11, 12), Phone = "0921098765", Email = "thao.phan@quanlygym.vn", WorkShift = "Ca cố định: 14:00 - 22:00 (Thứ 2 - CN)", BaseSalary = 14500000, Status = true, BranchID = 2, PTID = 4 },
                new Staff { StaffID = 17, FullName = "Phạm Ngọc Ánh", Role = "PT", Gender = "Nữ", DateOfBirth = new DateTime(1996, 7, 19), Phone = "0918776655", Email = "ngocanh.pham@quanlygym.vn", WorkShift = "Ca cố định: 06:00 - 14:00 (Thứ 2 - CN)", BaseSalary = 15000000, Status = true, BranchID = 2, PTID = 8 },
                new Staff { StaffID = 18, FullName = "Trần Quốc Bảo", Role = "PT", Gender = "Nam", DateOfBirth = new DateTime(1995, 12, 5), Phone = "0903445566", Email = "quocbao.tran@quanlygym.vn", WorkShift = "Ca cố định: 08:00 - 16:00 (Thứ 2 - CN)", BaseSalary = 16000000, Status = true, BranchID = 2, PTID = 9 },

                new Staff { StaffID = 5, FullName = "Vũ Hoàng Long", Role = "PT", Gender = "Nam", DateOfBirth = new DateTime(1996, 9, 8), Phone = "0909876543", Email = "long.vu@quanlygym.vn", WorkShift = "Ca cố định: 09:00 - 17:00 (Thứ 2 - CN)", BaseSalary = 15000000, Status = true, BranchID = 3, PTID = 5 },
                new Staff { StaffID = 19, FullName = "Đỗ Khánh Linh", Role = "PT", Gender = "Nữ", DateOfBirth = new DateTime(1997, 4, 15), Phone = "0922334455", Email = "khanhlinh.do@quanlygym.vn", WorkShift = "Ca cố định: 06:00 - 14:00 (Thứ 2 - CN)", BaseSalary = 15000000, Status = true, BranchID = 3, PTID = 10 },
                new Staff { StaffID = 20, FullName = "Nguyễn Tiến Đạt", Role = "PT", Gender = "Nam", DateOfBirth = new DateTime(1996, 11, 30), Phone = "0933557799", Email = "tiendat.nguyen@quanlygym.vn", WorkShift = "Ca cố định: 14:00 - 22:00 (Thứ 2 - CN)", BaseSalary = 14500000, Status = true, BranchID = 3, PTID = 11 },
                new Staff { StaffID = 21, FullName = "Hoàng Kim Ngân", Role = "PT", Gender = "Nữ", DateOfBirth = new DateTime(1999, 8, 8), Phone = "0944668800", Email = "kimngan.hoang@quanlygym.vn", WorkShift = "Ca cố định: 08:00 - 16:00 (Thứ 2 - CN)", BaseSalary = 14500000, Status = true, BranchID = 3, PTID = 12 },

                // Thu ngân
                new Staff { StaffID = 6, FullName = "Nguyễn Thị Lan Anh", Role = "Thu ngân", Gender = "Nữ", DateOfBirth = new DateTime(2000, 3, 10), Phone = "0912345679", Email = "lananh.tn@quanlygym.vn", WorkShift = "Ca sáng (06:00 - 14:00)", BaseSalary = 9000000, Status = true, BranchID = 1, PTID = null },
                new Staff { StaffID = 7, FullName = "Trần Quang Huy", Role = "Thu ngân", Gender = "Nam", DateOfBirth = new DateTime(1999, 12, 4), Phone = "0934567891", Email = "quanghuy.tn@quanlygym.vn", WorkShift = "Ca chiều (14:00 - 22:00)", BaseSalary = 9000000, Status = true, BranchID = 2, PTID = null },
                new Staff { StaffID = 8, FullName = "Đặng Minh Châu", Role = "Thu ngân", Gender = "Nữ", DateOfBirth = new DateTime(2001, 5, 18), Phone = "0966222333", Email = "minhchau.tn@quanlygym.vn", WorkShift = "Ca xoay (08:00 - 17:00)", BaseSalary = 9500000, Status = true, BranchID = 3, PTID = null },

                // Tạp vụ
                new Staff { StaffID = 9, FullName = "Lê Thị Mai", Role = "Tạp vụ", Gender = "Nữ", DateOfBirth = new DateTime(1980, 8, 20), Phone = "0905111222", Email = "maile.tv@quanlygym.vn", WorkShift = "Ca sáng (06:00 - 14:00)", BaseSalary = 7500000, Status = true, BranchID = 1, PTID = null },
                new Staff { StaffID = 10, FullName = "Nguyễn Thị Sáu", Role = "Tạp vụ", Gender = "Nữ", DateOfBirth = new DateTime(1982, 10, 15), Phone = "0932333444", Email = "saunguyen.tv@quanlygym.vn", WorkShift = "Ca chiều (14:00 - 22:00)", BaseSalary = 7500000, Status = true, BranchID = 2, PTID = null },
                new Staff { StaffID = 11, FullName = "Hoàng Thị Nga", Role = "Tạp vụ", Gender = "Nữ", DateOfBirth = new DateTime(1985, 4, 12), Phone = "0988444555", Email = "ngahoang.tv@quanlygym.vn", WorkShift = "Ca sáng (06:00 - 14:00)", BaseSalary = 7500000, Status = true, BranchID = 3, PTID = null },

                // Bảo vệ
                new Staff { StaffID = 12, FullName = "Bùi Văn Dũng", Role = "Bảo vệ", Gender = "Nam", DateOfBirth = new DateTime(1978, 1, 25), Phone = "0911777888", Email = "dungbui.bv@quanlygym.vn", WorkShift = "Ca ngày (06:00 - 18:00)", BaseSalary = 8500000, Status = true, BranchID = 1, PTID = null },
                new Staff { StaffID = 13, FullName = "Ngô Quang Hải", Role = "Bảo vệ", Gender = "Nam", DateOfBirth = new DateTime(1983, 6, 30), Phone = "0944888999", Email = "haingo.bv@quanlygym.vn", WorkShift = "Ca đêm (18:00 - 06:00)", BaseSalary = 9000000, Status = true, BranchID = 2, PTID = null },
                new Staff { StaffID = 14, FullName = "Đỗ Văn Tuấn", Role = "Bảo vệ", Gender = "Nam", DateOfBirth = new DateTime(1986, 9, 17), Phone = "0977999000", Email = "tuando.bv@quanlygym.vn", WorkShift = "Ca ngày (06:00 - 18:00)", BaseSalary = 8500000, Status = true, BranchID = 3, PTID = null }
            );
        }
    }
}
