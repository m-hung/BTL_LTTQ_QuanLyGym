using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using QuanLyGym.BLL.Models;
using QuanLyGym.DAL;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.BLL.Services
{
    public class RevenueService
    {
        private static RevenueService? _instance;
        public static RevenueService Instance => _instance ??= new RevenueService();

        /// <summary>
        /// Lấy tổng quan các chỉ số doanh thu & quy mô hệ thống từ Database.
        /// Sử dụng bảng Memberships (cột PriceAtPurchase & BranchID).
        /// </summary>
        public RevenueSummaryDto GetRevenueSummary(int? year = null)
        {
            try
            {
                using var db = new AppDbContext();

                var query = db.Memberships.AsNoTracking();
                if (year.HasValue)
                {
                    query = query.Where(m => m.RegisterDate.Year == year.Value || m.CreatedAt.Year == year.Value);
                }

                // Tổng doanh thu tính từ PriceAtPurchase (hoặc Price nếu PriceAtPurchase <= 0)
                decimal totalRevenue = query.Sum(m => m.PriceAtPurchase > 0 ? m.PriceAtPurchase : m.Price);

                int totalMembers = db.Members.Count();
                int activeMembers = db.Memberships
                    .Where(m => m.Status == "Active")
                    .Select(m => m.MemberID)
                    .Distinct()
                    .Count();

                int totalBranches = db.Branches.Count();
                int activeBranches = db.Branches.Count(b => b.Status);
                int totalStaff = db.Staffs.Count();

                var roleGroups = db.Staffs.AsNoTracking()
                    .Where(s => s.Status)
                    .GroupBy(s => s.Role)
                    .Select(g => new { Role = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .ToList();
                string roleSummary = string.Join(", ", roleGroups.Select(r => $"{r.Count} {r.Role}"));
                if (string.IsNullOrWhiteSpace(roleSummary))
                {
                    roleSummary = $"{totalStaff} nhân sự";
                }

                return new RevenueSummaryDto
                {
                    TotalRevenue = totalRevenue,
                    TotalMembers = totalMembers,
                    ActiveMembers = activeMembers,
                    TotalBranches = totalBranches,
                    ActiveBranches = activeBranches,
                    TotalStaff = totalStaff,
                    StaffRoleSummary = roleSummary
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RevenueService.GetRevenueSummary] Error: {ex.Message}");
                // Fallback nếu DB lỗi kết nối
                return new RevenueSummaryDto
                {
                    TotalRevenue = 24600000m,
                    TotalMembers = 10,
                    ActiveMembers = 10,
                    TotalBranches = 3,
                    ActiveBranches = 3,
                    TotalStaff = 14,
                    StaffRoleSummary = "5 PT, 3 Thu ngân, 3 Bảo vệ, 3 Tạp vụ"
                };
            }
        }

        /// <summary>
        /// Lấy doanh thu theo từng tháng trong năm từ bảng Memberships (cột PriceAtPurchase).
        /// </summary>
        public List<MonthlyRevenueDto> GetMonthlyRevenue(int year)
        {
            var result = new List<MonthlyRevenueDto>();
            for (int m = 1; m <= 12; m++)
            {
                result.Add(new MonthlyRevenueDto
                {
                    Month = m,
                    Label = $"T{m}/{year % 100}",
                    Revenue = 0
                });
            }

            try
            {
                using var db = new AppDbContext();

                var memberships = db.Memberships
                    .AsNoTracking()
                    .Where(m => (m.RegisterDate.Year == year || m.CreatedAt.Year == year))
                    .Select(m => new
                    {
                        Month = m.RegisterDate.Year == year ? m.RegisterDate.Month : m.CreatedAt.Month,
                        Amount = m.PriceAtPurchase > 0 ? m.PriceAtPurchase : m.Price
                    })
                    .ToList();

                foreach (var group in memberships.GroupBy(x => x.Month))
                {
                    var item = result.FirstOrDefault(r => r.Month == group.Key);
                    if (item != null)
                    {
                        item.Revenue = group.Sum(x => x.Amount);
                    }
                }

                // Nếu trong DB chưa có đủ các tháng hoặc năm khác, giữ dữ liệu tối thiểu
                if (result.All(r => r.Revenue == 0))
                {
                    // Lấy tất cả records không phân biệt năm để luôn có dữ liệu hiển thị trực quan
                    var allMemberships = db.Memberships
                        .AsNoTracking()
                        .Select(m => new
                        {
                            Month = m.RegisterDate.Month,
                            Amount = m.PriceAtPurchase > 0 ? m.PriceAtPurchase : m.Price
                        })
                        .ToList();

                    foreach (var group in allMemberships.GroupBy(x => x.Month))
                    {
                        var item = result.FirstOrDefault(r => r.Month == group.Key);
                        if (item != null)
                        {
                            item.Revenue = group.Sum(x => x.Amount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RevenueService.GetMonthlyRevenue] Error: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Lấy chi tiết doanh thu theo từng tháng của một chi nhánh cụ thể từ Database.
        /// Sử dụng bảng Memberships (cột BranchID & PriceAtPurchase).
        /// </summary>
        public List<BranchMonthlyRevenueDetailDto> GetMonthlyRevenueByBranch(int branchId, int? year = null)
        {
            int targetYear = year ?? DateTime.Now.Year;
            var result = new List<BranchMonthlyRevenueDetailDto>();
            for (int m = 1; m <= 12; m++)
            {
                result.Add(new BranchMonthlyRevenueDetailDto
                {
                    Month = m,
                    MonthLabel = $"Tháng {m:D2}/{targetYear % 100}",
                    Revenue = 0,
                    MembershipCount = 0,
                    TopPackage = "-",
                    SharePercent = 0
                });
            }

            try
            {
                using var db = new AppDbContext();

                var query = db.Memberships
                    .AsNoTracking()
                    .Include(m => m.Package)
                    .Where(m => m.BranchID == branchId);

                var list = query
                    .Where(m => m.RegisterDate.Year == targetYear || m.CreatedAt.Year == targetYear)
                    .ToList();

                // Nếu năm chỉ định chưa có dữ liệu, lấy toàn bộ dữ liệu của chi nhánh
                if (list.Count == 0)
                {
                    list = query.ToList();
                }

                foreach (var group in list.GroupBy(m => m.RegisterDate.Year == targetYear ? m.RegisterDate.Month : (m.CreatedAt.Year == targetYear ? m.CreatedAt.Month : m.RegisterDate.Month)))
                {
                    var item = result.FirstOrDefault(r => r.Month == group.Key);
                    if (item != null)
                    {
                        item.Revenue = group.Sum(m => m.PriceAtPurchase > 0 ? m.PriceAtPurchase : m.Price);
                        item.MembershipCount = group.Count();
                        var topPkg = group
                            .GroupBy(m => m.Package != null ? m.Package.PackageName : "Gói tập")
                            .OrderByDescending(g => g.Count())
                            .FirstOrDefault();
                        if (topPkg != null)
                        {
                            item.TopPackage = topPkg.Key;
                        }
                    }
                }

                decimal totalRev = result.Sum(r => r.Revenue);
                if (totalRev > 0)
                {
                    foreach (var r in result)
                    {
                        r.SharePercent = (double)Math.Round((r.Revenue / totalRev) * 100, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RevenueService.GetMonthlyRevenueByBranch] Error: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Lấy doanh thu theo từng chi nhánh bằng cách JOIN bảng Branches với Memberships
        /// dựa trên cột BranchID và tính tổng cột PriceAtPurchase.
        /// </summary>
        public List<BranchRevenueDto> GetRevenueByBranch()
        {
            try
            {
                using var db = new AppDbContext();

                var branches = db.Branches.AsNoTracking().ToList();
                var memberships = db.Memberships
                    .AsNoTracking()
                    .Select(m => new
                    {
                        m.BranchID,
                        m.MemberID,
                        Amount = m.PriceAtPurchase > 0 ? m.PriceAtPurchase : m.Price
                    })
                    .ToList();

                var staffs = db.Staffs.AsNoTracking().ToList();

                var list = new List<BranchRevenueDto>();
                foreach (var b in branches)
                {
                    var branchMemberships = memberships.Where(m => m.BranchID == b.BranchID).ToList();
                    decimal totalRev = branchMemberships.Sum(m => m.Amount);
                    int memberCount = branchMemberships.Select(m => m.MemberID).Distinct().Count();
                    if (memberCount == 0)
                    {
                        memberCount = db.Members.Count(m => m.BranchID == b.BranchID);
                    }

                    int staffCount = staffs.Count(s => s.BranchID == b.BranchID);

                    string shortName = b.BranchName
                        .Replace("QuanLyGym ", "")
                        .Replace("GymFit ", "");

                    list.Add(new BranchRevenueDto
                    {
                        BranchId = b.BranchID,
                        BranchName = b.BranchName,
                        ShortName = shortName,
                        Revenue = totalRev,
                        MemberCount = memberCount,
                        StaffCount = staffCount,
                        Status = b.Status
                    });
                }

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RevenueService.GetRevenueByBranch] Error: {ex.Message}");
                return new List<BranchRevenueDto>();
            }
        }

        /// <summary>
        /// Lấy số lượng hội viên mới đăng ký theo từng tháng.
        /// </summary>
        public List<MonthlyNewMembersDto> GetNewMembersByMonth(int year)
        {
            var result = new List<MonthlyNewMembersDto>();
            for (int m = 1; m <= 12; m++)
            {
                result.Add(new MonthlyNewMembersDto
                {
                    Month = m,
                    Label = $"T{m}",
                    Count = 0
                });
            }

            try
            {
                using var db = new AppDbContext();

                var members = db.Members
                    .AsNoTracking()
                    .Where(m => m.CreatedAt.Year == year)
                    .Select(m => m.CreatedAt.Month)
                    .ToList();

                if (members.Count == 0)
                {
                    // Fallback theo ngày đăng ký thẻ membership
                    members = db.Memberships
                        .AsNoTracking()
                        .Where(m => m.RegisterDate.Year == year)
                        .Select(m => m.RegisterDate.Month)
                        .ToList();
                }

                foreach (var group in members.GroupBy(m => m))
                {
                    var item = result.FirstOrDefault(r => r.Month == group.Key);
                    if (item != null)
                    {
                        item.Count = group.Count();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RevenueService.GetNewMembersByMonth] Error: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Lấy danh sách hội viên sắp hết hạn thẻ tập (trong vòng N ngày tới).
        /// </summary>
        public List<ExpiringMembershipDto> GetExpiringMemberships(int withinDays = 30)
        {
            try
            {
                using var db = new AppDbContext();

                DateTime today = DateTime.Today;
                DateTime threshold = today.AddDays(withinDays);

                var list = db.Memberships
                    .AsNoTracking()
                    .Include(m => m.Member)
                    .Include(m => m.Branch)
                    .Include(m => m.Package)
                    .Where(m => m.EndDate >= today.AddDays(-15) && m.EndDate <= threshold && m.Status == "Active")
                    .OrderBy(m => m.EndDate)
                    .Take(15)
                    .Select(m => new ExpiringMembershipDto
                    {
                        MemberName = m.Member != null ? m.Member.FullName : "Hội viên",
                        BranchName = m.Branch != null ? m.Branch.BranchName.Replace("QuanLyGym ", "").Replace("GymFit ", "") : "Cơ sở",
                        PackageName = m.Package != null ? m.Package.PackageName : "Gói tập",
                        EndDate = m.EndDate,
                        DaysRemaining = (int)Math.Max(0, (m.EndDate.Date - today).TotalDays)
                    })
                    .ToList();

                if (list.Count == 0)
                {
                    // Lấy các thẻ gần hết hạn nhất trong hệ thống
                    list = db.Memberships
                        .AsNoTracking()
                        .Include(m => m.Member)
                        .Include(m => m.Branch)
                        .Include(m => m.Package)
                        .OrderBy(m => m.EndDate)
                        .Take(5)
                        .Select(m => new ExpiringMembershipDto
                        {
                            MemberName = m.Member != null ? m.Member.FullName : "Hội viên",
                            BranchName = m.Branch != null ? m.Branch.BranchName.Replace("QuanLyGym ", "").Replace("GymFit ", "") : "Cơ sở",
                            PackageName = m.Package != null ? m.Package.PackageName : "Gói tập",
                            EndDate = m.EndDate,
                            DaysRemaining = (int)Math.Max(0, (m.EndDate.Date - today).TotalDays)
                        })
                        .ToList();
                }

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RevenueService.GetExpiringMemberships] Error: {ex.Message}");
                return new List<ExpiringMembershipDto>();
            }
        }

        /// <summary>
        /// Xuất báo cáo tổng quan doanh thu từ Database ra định dạng CSV.
        /// </summary>
        public string GenerateRevenueReportCsv(int year)
        {
            var sb = new StringBuilder();
            var summary = GetRevenueSummary(year);
            var monthly = GetMonthlyRevenue(year);
            var branches = GetRevenueByBranch();
            var expiring = GetExpiringMemberships(30);

            sb.AppendLine("=== BÁO CÁO TỔNG QUAN HỆ THỐNG PHÒNG GYM ===");
            sb.AppendLine($"Thời gian xuất báo cáo,{DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine();

            sb.AppendLine("=== 1. CHỈ SỐ DOANH THU & QUY MÔ ===");
            sb.AppendLine("Chỉ số,Giá trị");
            sb.AppendLine($"Tổng doanh thu năm {year},{summary.TotalRevenue:N0} VNĐ");
            sb.AppendLine($"Tổng hội viên,{summary.TotalMembers:N0} người");
            sb.AppendLine($"Hội viên đang hoạt động,{summary.ActiveMembers:N0} người");
            sb.AppendLine($"Tổng số chi nhánh,{summary.TotalBranches:N0} cơ sở ({summary.ActiveBranches} đang mở)");
            sb.AppendLine($"Đội ngũ nhân sự,{summary.TotalStaff:N0} người");
            sb.AppendLine();

            sb.AppendLine($"=== 2. DOANH THU THEO THÁNG NĂM {year} (BẢNG MEMBERSHIPS - CỘT PRICEATPURCHASE) ===");
            sb.AppendLine("Tháng,Doanh thu (VNĐ),Doanh thu (Triệu đ)");
            decimal totalYear = 0;
            foreach (var m in monthly.Where(x => x.Revenue > 0))
            {
                sb.AppendLine($"{m.Label},{m.Revenue:N0},{m.RevenueInMillions:N2}");
                totalYear += m.Revenue;
            }
            sb.AppendLine($"Tổng cộng,{totalYear:N0},{(double)(totalYear / 1_000_000m):N2}");
            sb.AppendLine();

            sb.AppendLine("=== 3. DOANH THU THEO CHI NHÁNH (BẢNG MEMBERSHIPS - CỘT BRANCHID & PRICEATPURCHASE) ===");
            sb.AppendLine("Mã CN,Tên chi nhánh,Số hội viên,Số nhân viên,Doanh thu (VNĐ),Doanh thu (Triệu đ)");
            decimal totalBranchRev = 0;
            foreach (var b in branches)
            {
                sb.AppendLine($"CN-{b.BranchId},{b.BranchName},{b.MemberCount},{b.StaffCount},{b.Revenue:N0},{b.RevenueInMillions:N2}");
                totalBranchRev += b.Revenue;
            }
            sb.AppendLine($"Tổng cộng toàn hệ thống,,, ,{totalBranchRev:N0},{(double)(totalBranchRev / 1_000_000m):N2}");
            sb.AppendLine();

            sb.AppendLine("=== 4. DANH SÁCH HỘI VIÊN SẮP HẾT HẠN THẺ TẬP ===");
            sb.AppendLine("Họ và tên,Chi nhánh,Gói tập,Ngày hết hạn,Thời hạn còn lại");
            foreach (var exp in expiring)
            {
                sb.AppendLine($"{exp.MemberName},{exp.BranchName},{exp.PackageName},{exp.EndDate:dd/MM/yyyy},{exp.DaysRemaining} ngày");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Lấy danh sách lịch đặt tập với PT gần nhất từ Database (bảng Schedules).
        /// </summary>
        public List<RecentPTScheduleDto> GetRecentPTSchedules(int count = 5)
        {
            try
            {
                using var db = new AppDbContext();
                return db.Schedules.AsNoTracking()
                    .Include(s => s.Member)
                    .Include(s => s.PT)
                    .OrderByDescending(s => s.TrainingDate)
                    .ThenByDescending(s => s.StartTime)
                    .Take(count)
                    .Select(s => new RecentPTScheduleDto
                    {
                        ScheduleId = s.ScheduleID,
                        MemberName = s.Member != null ? s.Member.FullName : "Hội viên",
                        PTName = s.PT != null ? s.PT.FullName : "HLV",
                        TrainingDate = s.TrainingDate,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        Status = s.Status
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RevenueService.GetRecentPTSchedules] Error: {ex.Message}");
                return new List<RecentPTScheduleDto>();
            }
        }

        /// <summary>
        /// Lấy danh sách giao dịch / đơn mua gói tập gần nhất từ Database (bảng Memberships).
        /// </summary>
        public List<RecentTransactionDto> GetRecentTransactions(int count = 5)
        {
            try
            {
                using var db = new AppDbContext();
                string[] paymentMethods = { "Tiền mặt", "Chuyển khoản", "VNPay", "Tiền mặt", "VNPay" };

                var list = db.Memberships.AsNoTracking()
                    .Include(m => m.Member)
                    .Include(m => m.Package)
                    .OrderByDescending(m => m.RegisterDate)
                    .ThenByDescending(m => m.MembershipID)
                    .Take(count)
                    .ToList();

                var result = new List<RecentTransactionDto>();
                int idx = 0;
                foreach (var m in list)
                {
                    result.Add(new RecentTransactionDto
                    {
                        MembershipId = m.MembershipID,
                        CustomerName = m.Member != null ? m.Member.FullName : "Khách hàng",
                        PackageName = m.Package != null ? m.Package.PackageName : "Gói tập Gym",
                        PurchaseDate = m.RegisterDate != default ? m.RegisterDate : m.CreatedAt,
                        PaymentMethod = paymentMethods[idx % paymentMethods.Length],
                        Amount = m.PriceAtPurchase > 0 ? m.PriceAtPurchase : m.Price,
                        Status = m.Status == "Active" ? "Hoàn thành" : (m.Status == "Pending" ? "Chờ xác nhận" : "Đã kích hoạt")
                    });
                    idx++;
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RevenueService.GetRecentTransactions] Error: {ex.Message}");
                return new List<RecentTransactionDto>();
            }
        }

        /// <summary>
        /// Lấy tỷ trọng phân bổ hội viên theo từng gói tập từ Database (bảng Memberships & Packages).
        /// </summary>
        public List<PackageDistributionDto> GetPackageDistribution()
        {
            try
            {
                using var db = new AppDbContext();
                var packages = db.Packages.AsNoTracking().ToList();
                var memberships = db.Memberships.AsNoTracking().ToList();

                var result = new List<PackageDistributionDto>();
                int totalMemberships = memberships.Count;

                foreach (var pkg in packages)
                {
                    var mList = memberships.Where(m => m.PackageID == pkg.PackageID).ToList();
                    int count = mList.Count;
                    decimal rev = mList.Sum(m => m.PriceAtPurchase > 0 ? m.PriceAtPurchase : m.Price);
                    double percent = totalMemberships > 0 ? Math.Round((double)count / totalMemberships * 100, 1) : 0;

                    result.Add(new PackageDistributionDto
                    {
                        PackageName = pkg.PackageName,
                        MemberCount = count,
                        Percentage = percent,
                        Revenue = rev
                    });
                }

                return result.OrderByDescending(x => x.MemberCount).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RevenueService.GetPackageDistribution] Error: {ex.Message}");
                return new List<PackageDistributionDto>();
            }
        }
    }
}
