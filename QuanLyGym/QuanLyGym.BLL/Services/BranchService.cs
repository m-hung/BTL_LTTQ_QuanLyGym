using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyGym.BLL.Models;
using QuanLyGym.DAL;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.BLL.Services
{
    public class BranchService
    {
        private static BranchService? _instance;
        public static BranchService Instance => _instance ??= new BranchService();

        /// <summary>
        /// Truy xuất toàn bộ danh sách Chi nhánh từ Database (Bảng Branches, Staffs, Memberships).
        /// Doanh thu được tính trực tiếp từ cột PriceAtPurchase trong bảng Memberships theo từng BranchID.
        /// </summary>
        public List<BranchModel> GetAllBranches()
        {
            try
            {
                using var db = new AppDbContext();

                var dbBranches = db.Branches.AsNoTracking().ToList();
                var dbStaffs = db.Staffs.AsNoTracking().ToList();
                var dbMemberships = db.Memberships
                    .AsNoTracking()
                    .Select(m => new
                    {
                        m.BranchID,
                        m.MemberID,
                        Amount = m.PriceAtPurchase > 0 ? m.PriceAtPurchase : m.Price
                    })
                    .ToList();

                var dbMembers = db.Members.AsNoTracking().Select(m => new { m.MemberID, m.BranchID }).ToList();

                var result = new List<BranchModel>();

                foreach (var b in dbBranches)
                {
                    // Doanh thu từ bảng Memberships theo BranchID và PriceAtPurchase
                    var branchMemberships = dbMemberships.Where(m => m.BranchID == b.BranchID).ToList();
                    long branchRev = (long)branchMemberships.Sum(m => m.Amount);

                    // Đếm số hội viên thuộc chi nhánh
                    int memberCount = branchMemberships.Select(m => m.MemberID).Distinct().Count();
                    if (memberCount == 0)
                    {
                        memberCount = dbMembers.Count(m => m.BranchID == b.BranchID);
                    }

                    // Đếm số nhân sự thuộc chi nhánh
                    int staffCount = dbStaffs.Count(s => s.BranchID == b.BranchID);

                    // Tìm quản lý chi nhánh
                    var managerStaff = dbStaffs.FirstOrDefault(s => s.BranchID == b.BranchID && (s.Role.Contains("Quản lý") || s.Role.Contains("PT")));
                    string managerName = managerStaff != null ? managerStaff.FullName : "Trưởng cơ sở";

                    string code = $"CN-{(b.BranchID < 10 ? "0" : "")}{b.BranchID}";

                    result.Add(new BranchModel
                    {
                        Id = b.BranchID,
                        Code = code,
                        Name = b.BranchName,
                        Address = b.Address,
                        Phone = b.Phone,
                        Email = b.Email,
                        Manager = managerName,
                        OpenTime = b.OpenTime.ToString(@"hh\:mm"),
                        CloseTime = b.CloseTime.ToString(@"hh\:mm"),
                        AreaSqM = 1000 + (b.BranchID * 200),
                        IsOnline = b.Status,
                        Note = b.Status ? "Cơ sở đang mở cửa đón khách ổn định." : "Cơ sở tạm đóng bảo dưỡng thiết bị.",
                        MemberCount = memberCount,
                        StaffCount = staffCount,
                        MonthlyRevenue = branchRev
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BranchService.GetAllBranches] Error: {ex.Message}");
                // Fallback nếu kết nối DB lỗi
                return GetFallbackBranches();
            }
        }

        /// <summary>
        /// Thêm mới một Chi nhánh vào Database.
        /// </summary>
        public bool AddBranch(BranchModel model)
        {
            try
            {
                using var db = new AppDbContext();

                var branch = new Branch
                {
                    BranchName = model.Name,
                    Address = model.Address,
                    Phone = model.Phone,
                    Email = model.Email,
                    OpenTime = TimeSpan.TryParse(model.OpenTime, out var ot) ? ot : new TimeSpan(6, 0, 0),
                    CloseTime = TimeSpan.TryParse(model.CloseTime, out var ct) ? ct : new TimeSpan(22, 0, 0),
                    Status = model.IsOnline
                };

                db.Branches.Add(branch);
                db.SaveChanges();

                model.Id = branch.BranchID;
                model.Code = $"CN-{(branch.BranchID < 10 ? "0" : "")}{branch.BranchID}";
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BranchService.AddBranch] Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Cập nhật thông tin Chi nhánh trong Database.
        /// </summary>
        public bool UpdateBranch(BranchModel model)
        {
            try
            {
                using var db = new AppDbContext();
                var branch = db.Branches.Find(model.Id);
                if (branch == null) return false;

                branch.BranchName = model.Name;
                branch.Address = model.Address;
                branch.Phone = model.Phone;
                branch.Email = model.Email;

                if (TimeSpan.TryParse(model.OpenTime, out var ot)) branch.OpenTime = ot;
                if (TimeSpan.TryParse(model.CloseTime, out var ct)) branch.CloseTime = ct;

                branch.Status = model.IsOnline;

                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BranchService.UpdateBranch] Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Bật / Tắt trạng thái hoạt động (Online/Offline) của chi nhánh trong Database.
        /// </summary>
        public bool ToggleBranchStatus(int branchId)
        {
            try
            {
                using var db = new AppDbContext();
                var branch = db.Branches.Find(branchId);
                if (branch == null) return false;

                branch.Status = !branch.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BranchService.ToggleBranchStatus] Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Xóa vĩnh viễn một Chi nhánh khỏi Database.
        /// </summary>
        public bool DeleteBranch(int branchId)
        {
            try
            {
                using var db = new AppDbContext();
                var branch = db.Branches.Find(branchId);
                if (branch == null) return false;

                // Cập nhật gỡ bỏ liên kết khóa ngoại trước khi xóa
                var staffs = db.Staffs.Where(s => s.BranchID == branchId).ToList();
                foreach (var s in staffs) s.BranchID = null;

                var members = db.Members.Where(m => m.BranchID == branchId).ToList();
                foreach (var m in members) m.BranchID = null;

                var memberships = db.Memberships.Where(ms => ms.BranchID == branchId).ToList();
                foreach (var ms in memberships) ms.BranchID = null;

                var packages = db.Packages.Where(p => p.BranchID == branchId).ToList();
                foreach (var p in packages) p.BranchID = null;

                var rooms = db.Rooms.Where(r => r.BranchID == branchId).ToList();
                foreach (var r in rooms) r.BranchID = null;

                var schedules = db.Schedules.Where(sc => sc.BranchID == branchId).ToList();
                foreach (var sc in schedules) sc.BranchID = null;

                var checkins = db.CheckIns.Where(c => c.BranchID == branchId).ToList();
                foreach (var c in checkins) c.BranchID = null;

                db.Branches.Remove(branch);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BranchService.DeleteBranch] Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Lấy danh sách nhân sự thuộc chi nhánh từ bảng Staffs trong Database.
        /// </summary>
        public List<BranchStaffItem> GetBranchStaff(int branchId)
        {
            try
            {
                using var db = new AppDbContext();
                var staffs = db.Staffs.AsNoTracking().Where(s => s.BranchID == branchId).ToList();

                return staffs.Select(s => new BranchStaffItem
                {
                    StaffId = $"NV-{s.StaffID:D2}",
                    Name = s.FullName,
                    Role = s.Role,
                    Phone = s.Phone ?? "",
                    Shift = string.IsNullOrWhiteSpace(s.WorkShift) ? "Ca sáng (06:00 - 14:00)" : s.WorkShift,
                    JoinDate = s.DateOfBirth.ToString("dd/MM/yyyy"),
                    IsWorking = s.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BranchService.GetBranchStaff] Error: {ex.Message}");
                return new List<BranchStaffItem>();
            }
        }

        /// <summary>
        /// Thêm nhân sự vào chi nhánh trong Database.
        /// </summary>
        public bool AddStaffToBranch(int branchId, BranchStaffItem item)
        {
            try
            {
                using var db = new AppDbContext();

                decimal salary = item.Role switch
                {
                    "PT" => 16000000m,
                    "Thu ngân" => 9000000m,
                    "Tạp vụ" => 7500000m,
                    _ => 8500000m
                };

                var staff = new Staff
                {
                    FullName = item.Name,
                    Role = item.Role,
                    Phone = item.Phone,
                    Email = $"{item.Phone}@quanlygym.vn",
                    WorkShift = item.Shift,
                    BaseSalary = salary,
                    Status = item.IsWorking,
                    BranchID = branchId,
                    DateOfBirth = DateTime.Today.AddYears(-25),
                    Gender = "Nam"
                };

                db.Staffs.Add(staff);
                db.SaveChanges();
                item.StaffId = $"NV-{staff.StaffID:D2}";
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BranchService.AddStaffToBranch] Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Xóa nhân viên khỏi chi nhánh trong Database.
        /// </summary>
        public bool RemoveStaffFromBranch(string staffIdStr, int branchId)
        {
            try
            {
                using var db = new AppDbContext();

                // Lấy ID số từ mã "NV-01" hoặc số nguyên
                string cleanId = staffIdStr.Replace("NV-", "").Trim();
                if (int.TryParse(cleanId, out int staffId))
                {
                    var staff = db.Staffs.Find(staffId);
                    if (staff != null)
                    {
                        db.Staffs.Remove(staff);
                        db.SaveChanges();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BranchService.RemoveStaffFromBranch] Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Lấy danh sách hội viên thuộc chi nhánh từ bảng Memberships & Members trong Database.
        /// </summary>
        public List<BranchMemberItem> GetBranchMembers(int branchId)
        {
            try
            {
                using var db = new AppDbContext();

                var memberships = db.Memberships
                    .AsNoTracking()
                    .Include(m => m.Member)
                    .Include(m => m.Package)
                    .Where(m => m.BranchID == branchId)
                    .ToList();

                var list = new List<BranchMemberItem>();
                foreach (var ms in memberships)
                {
                    string assignedPt = "Tự tập";
                    if (ms.Package != null && ms.Package.IsPTPackage)
                    {
                        // Tìm PT từ Schedule gần nhất của hội viên này
                        var ptName = db.Schedules
                            .AsNoTracking()
                            .Include(s => s.PT)
                            .Where(s => s.MemberID == ms.MemberID && s.PT != null)
                            .Select(s => s.PT!.FullName)
                            .FirstOrDefault();

                        if (!string.IsNullOrEmpty(ptName)) assignedPt = ptName;
                    }

                    list.Add(new BranchMemberItem
                    {
                        MemberId = $"HV-{ms.MemberID:D2}",
                        Name = ms.Member?.FullName ?? "Hội viên",
                        Phone = ms.Member?.Phone ?? "",
                        Package = ms.Package?.PackageName ?? "Gói Standard",
                        AssignedPt = assignedPt,
                        StartDate = ms.StartDate.ToString("dd/MM/yyyy"),
                        ExpireDate = ms.EndDate.ToString("dd/MM/yyyy"),
                        Status = ms.Status == "Active" ? "Đang tập" : "Đã hết hạn"
                    });
                }

                // Nếu trong Memberships chưa có đủ, lấy thêm từ bảng Members
                if (list.Count == 0)
                {
                    var members = db.Members.AsNoTracking().Where(m => m.BranchID == branchId).ToList();
                    foreach (var m in members)
                    {
                        list.Add(new BranchMemberItem
                        {
                            MemberId = $"HV-{m.MemberID:D2}",
                            Name = m.FullName,
                            Phone = m.Phone,
                            Package = "Gói Classic",
                            AssignedPt = "Tự tập",
                            StartDate = m.CreatedAt.ToString("dd/MM/yyyy"),
                            ExpireDate = m.CreatedAt.AddMonths(3).ToString("dd/MM/yyyy"),
                            Status = "Đang tập"
                        });
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BranchService.GetBranchMembers] Error: {ex.Message}");
                return new List<BranchMemberItem>();
            }
        }

        /// <summary>
        /// Đăng ký thêm học viên vào chi nhánh trong Database, tạo bản ghi Member và Membership có cột BranchID & PriceAtPurchase.
        /// </summary>
        public bool AddMemberToBranch(int branchId, BranchMemberItem item, int durationMonths = 12)
        {
            try
            {
                using var db = new AppDbContext();

                // 1. Tạo tài khoản đăng nhập cho hội viên
                string username = "member_" + DateTime.Now.Ticks.ToString().Substring(10);
                var account = new Account
                {
                    Username = username,
                    PasswordHash = "123456",
                    Role = "Member",
                    Status = true,
                    CreatedAt = DateTime.Now
                };
                db.Accounts.Add(account);
                db.SaveChanges();

                // 2. Tạo bản ghi Member
                var member = new Member
                {
                    AccountID = account.AccountID,
                    FullName = item.Name,
                    Phone = item.Phone,
                    Email = $"{item.Phone}@gmail.com",
                    Address = "TP. Hồ Chí Minh",
                    CreatedAt = DateTime.Now,
                    BranchID = branchId,
                    Gender = "Nam",
                    DateOfBirth = DateTime.Today.AddYears(-22)
                };
                db.Members.Add(member);
                db.SaveChanges();

                // 3. Tìm Package tương ứng
                var package = db.Packages.FirstOrDefault(p => p.PackageName.Contains(item.Package))
                              ?? db.Packages.FirstOrDefault()
                              ?? new Package { PackageID = 1, PackageName = "Classic", Price = 500000 };

                decimal purchasePrice = package.Price > 0 ? package.Price : 500000m;

                // 4. Tạo bản ghi Membership lưu trữ PriceAtPurchase và BranchID
                var membership = new Membership
                {
                    MemberID = member.MemberID,
                    PackageID = package.PackageID,
                    BranchID = branchId,
                    PriceAtPurchase = purchasePrice,
                    Price = purchasePrice,
                    RegisterDate = DateTime.Now,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(durationMonths),
                    UsedPTSessions = 0,
                    Status = "Active",
                    CreatedAt = DateTime.Now
                };

                db.Memberships.Add(membership);
                db.SaveChanges();

                item.MemberId = $"HV-{member.MemberID:D2}";
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BranchService.AddMemberToBranch] Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Xóa học viên khỏi chi nhánh trong Database.
        /// </summary>
        public bool RemoveMemberFromBranch(string memberIdStr, int branchId)
        {
            try
            {
                using var db = new AppDbContext();

                string cleanId = memberIdStr.Replace("HV-", "").Trim();
                if (int.TryParse(cleanId, out int memberId))
                {
                    // Đổi trạng thái thẻ Membership thành Expired hoặc gỡ bỏ
                    var memberships = db.Memberships.Where(m => m.MemberID == memberId && m.BranchID == branchId).ToList();
                    foreach (var ms in memberships)
                    {
                        ms.Status = "Expired";
                    }

                    var member = db.Members.Find(memberId);
                    if (member != null && member.BranchID == branchId)
                    {
                        member.BranchID = null;
                    }

                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BranchService.RemoveMemberFromBranch] Error: {ex.Message}");
                return false;
            }
        }

        private List<BranchModel> GetFallbackBranches()
        {
            return new List<BranchModel>
            {
                new BranchModel { Id = 1, Code = "CN-01", Name = "QuanLyGym Quận 1", Address = "123 Nguyễn Thị Minh Khai, P. Bến Thành, Quận 1, TP. HCM", Phone = "02838221122", Email = "quan1@quanlygym.vn", Manager = "Nguyễn Văn Nam", OpenTime = "06:00", CloseTime = "22:00", AreaSqM = 1500, IsOnline = true, MemberCount = 5, StaffCount = 5, MonthlyRevenue = 22100000 },
                new BranchModel { Id = 2, Code = "CN-02", Name = "QuanLyGym Bình Thạnh", Address = "456 Điện Biên Phủ, P. 25, Quận Bình Thạnh, TP. HCM", Phone = "02838332233", Email = "binhthanh@quanlygym.vn", Manager = "Nguyễn Đức Minh", OpenTime = "06:00", CloseTime = "22:00", AreaSqM = 1200, IsOnline = true, MemberCount = 3, StaffCount = 5, MonthlyRevenue = 1500000 },
                new BranchModel { Id = 3, Code = "CN-03", Name = "QuanLyGym Quận 7", Address = "789 Nguyễn Lương Bằng, P. Tân Phú, Quận 7, TP. HCM", Phone = "02838443344", Email = "quan7@quanlygym.vn", Manager = "Vũ Hoàng Long", OpenTime = "06:00", CloseTime = "22:00", AreaSqM = 1000, IsOnline = true, MemberCount = 2, StaffCount = 4, MonthlyRevenue = 1000000 }
            };
        }
    }
}
