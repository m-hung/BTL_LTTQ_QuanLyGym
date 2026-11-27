using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyGym.BLL.Models;
using QuanLyGym.DAL;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.BLL.Services
{
    public class PackageService
    {
        private static PackageService? _instance;
        public static PackageService Instance => _instance ??= new PackageService();

        private static bool _initialized = false;
        private static readonly object _initLock = new object();

        /// <summary>
        /// Khởi tạo và đồng bộ dữ liệu các gói tập chuẩn theo ảnh mẫu của người dùng
        /// </summary>
        public void EnsurePackageData()
        {
            if (_initialized) return;
            lock (_initLock)
            {
                if (_initialized) return;
                try
                {
                    using var db = new AppDbContext();

                    // Cập nhật hoặc bổ sung các gói tập tiêu chuẩn nếu cần
                    var packages = db.Packages.ToList();

                    // 1. Gói Đồng (Basic 1 Tháng)
                    var p1 = packages.FirstOrDefault(p => p.PackageID == 1 || p.PackageName.Contains("Đồng") || p.PackageName.Equals("Classic", StringComparison.OrdinalIgnoreCase));
                    if (p1 != null)
                    {
                        p1.PackageName = "Gói Đồng (Basic 1 Tháng)";
                        p1.Duration = 30;
                        p1.Price = 500000;
                        p1.IsPTPackage = false;
                        p1.TotalPTSessions = 0;
                        if (string.IsNullOrWhiteSpace(p1.Description) || p1.Description == "Gói cơ bản")
                            p1.Description = "Gói tập cơ bản 30 ngày, sử dụng toàn bộ trang thiết bị Gym hiện đại, tủ locker và phòng tắm nước nóng.";
                        p1.Status = true;
                    }

                    // 2. Gói Bạc (Standard 3 Tháng)
                    var p2 = packages.FirstOrDefault(p => p.PackageID == 2 || p.PackageName.Contains("Bạc") || p.PackageName.Equals("Silver", StringComparison.OrdinalIgnoreCase));
                    if (p2 != null)
                    {
                        p2.PackageName = "Gói Bạc (Standard 3 Tháng)";
                        p2.Duration = 90;
                        p2.Price = 1300000;
                        p2.IsPTPackage = false;
                        p2.TotalPTSessions = 0;
                        if (string.IsNullOrWhiteSpace(p2.Description) || p2.Description == "Gói cao cấp")
                            p2.Description = "Gói tập 90 ngày tiết kiệm, không giới hạn khung giờ tập, miễn phí đo chỉ số cơ thể định kỳ (InBody).";
                        p2.Status = true;
                    }

                    // 3. Gói Vàng (VIP 6 Tháng kèm PT)
                    var p3 = packages.FirstOrDefault(p => p.PackageID == 3 || p.PackageName.Contains("Vàng") || p.PackageName.Equals("Gold", StringComparison.OrdinalIgnoreCase));
                    if (p3 != null)
                    {
                        p3.PackageName = "Gói Vàng (VIP 6 Tháng kèm PT)";
                        p3.Duration = 180;
                        p3.Price = 4500000;
                        p3.IsPTPackage = true;
                        p3.TotalPTSessions = 24;
                        if (string.IsNullOrWhiteSpace(p3.Description))
                            p3.Description = "Gói tập 180 ngày cao cấp, tập kèm PT không giới hạn, sử dụng phòng xông hơi Sauna & bể bơi nước ấm.";
                        p3.Status = true;
                    }

                    // 4. Gói Kim Cương (VVIP 1 Năm kèm PT)
                    var p4 = packages.FirstOrDefault(p => p.PackageID == 4 || p.PackageName.Contains("Kim Cương") || p.PackageName.Equals("VIP Diamond", StringComparison.OrdinalIgnoreCase));
                    if (p4 != null)
                    {
                        p4.PackageName = "Gói Kim Cương (VVIP 1 Năm kèm PT)";
                        p4.Duration = 365;
                        p4.Price = 8000000;
                        p4.IsPTPackage = true;
                        p4.TotalPTSessions = 48;
                        if (string.IsNullOrWhiteSpace(p4.Description))
                            p4.Description = "Gói tập trọn gói 365 ngày đặc quyền tối cao, tập kèm PT không giới hạn, tư vấn dinh dưỡng AI và khăn nước riêng.";
                        p4.Status = true;
                    }

                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[EnsurePackageData] Error: {ex.Message}");
                }
                finally
                {
                    _initialized = true;
                }
            }
        }

        /// <summary>
        /// Lấy toàn bộ danh sách gói tập kèm số lượng khách đăng ký và doanh thu từ database
        /// </summary>
        public List<PackageDto> GetPackages(PackageFilterDto? filter = null)
        {
            EnsurePackageData();
            try
            {
                using var db = new AppDbContext();

                var packages = db.Packages
                    .Include(p => p.Branch)
                    .AsNoTracking()
                    .ToList();

                var memberships = db.Memberships
                    .AsNoTracking()
                    .ToList();

                var result = new List<PackageDto>();

                foreach (var pkg in packages)
                {
                    var mList = memberships.Where(m => m.PackageID == pkg.PackageID).ToList();
                    int subCount = mList.Count;
                    decimal rev = mList.Sum(m => m.PriceAtPurchase > 0 ? m.PriceAtPurchase : m.Price);

                    result.Add(new PackageDto
                    {
                        PackageID = pkg.PackageID,
                        PackageName = pkg.PackageName,
                        Duration = pkg.Duration,
                        Price = pkg.Price,
                        TotalPTSessions = pkg.TotalPTSessions,
                        IsPTPackage = pkg.IsPTPackage,
                        Benefits = pkg.Benefits ?? string.Empty,
                        Description = pkg.Description ?? string.Empty,
                        Status = pkg.Status,
                        BranchID = pkg.BranchID,
                        BranchName = pkg.Branch != null ? pkg.Branch.BranchName : "Toàn hệ thống",
                        SubscriberCount = subCount,
                        TotalRevenue = rev
                    });
                }

                // Áp dụng bộ lọc
                if (filter != null)
                {
                    if (!string.IsNullOrWhiteSpace(filter.ServiceType) && filter.ServiceType != "Tất cả")
                    {
                        if (filter.ServiceType == "Tự tập" || filter.ServiceType.Contains("Tự tập"))
                            result = result.Where(x => !x.IsPTPackage).ToList();
                        else if (filter.ServiceType == "Kèm PT" || filter.ServiceType.Contains("PT"))
                            result = result.Where(x => x.IsPTPackage).ToList();
                    }

                    if (filter.Status.HasValue)
                    {
                        result = result.Where(x => x.Status == filter.Status.Value).ToList();
                    }

                    if (!string.IsNullOrWhiteSpace(filter.SearchKeyword))
                    {
                        string kw = filter.SearchKeyword.Trim().ToLower();
                        result = result.Where(x =>
                            x.PackageName.ToLower().Contains(kw) ||
                            x.Description.ToLower().Contains(kw) ||
                            x.Benefits.ToLower().Contains(kw) ||
                            x.PackageCode.ToLower().Contains(kw)
                        ).ToList();
                    }
                }

                return result.OrderBy(x => x.PackageID).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GetPackages] Error: {ex.Message}");
                return new List<PackageDto>();
            }
        }

        /// <summary>
        /// Thống kê chỉ số KPI gói tập
        /// </summary>
        public PackageSummaryDto GetSummary()
        {
            var list = GetPackages(null);

            var topPkg = list.OrderByDescending(x => x.SubscriberCount).FirstOrDefault();

            return new PackageSummaryDto
            {
                TotalPackages = list.Count,
                ActivePackages = list.Count(x => x.Status),
                PTPackagesCount = list.Count(x => x.IsPTPackage),
                TotalSubscribers = list.Sum(x => x.SubscriberCount),
                TotalRevenue = list.Sum(x => x.TotalRevenue),
                MostPopularPackage = topPkg != null ? topPkg.PackageName : "-",
                MostPopularSubscribers = topPkg?.SubscriberCount ?? 0
            };
        }

        /// <summary>
        /// Thêm mới gói tập vào Database
        /// </summary>
        public (bool Success, string Message, int NewId) AddPackage(PackageDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.PackageName))
                    return (false, "Tên gói tập không được để trống.", 0);

                if (dto.Price <= 0)
                    return (false, "Giá niêm yết phải lớn hơn 0.", 0);

                if (dto.Duration <= 0)
                    return (false, "Thời hạn gói tập phải lớn hơn 0 ngày.", 0);

                using var db = new AppDbContext();

                var pkg = new Package
                {
                    PackageName = dto.PackageName.Trim(),
                    Duration = dto.Duration,
                    Price = dto.Price,
                    TotalPTSessions = dto.TotalPTSessions,
                    IsPTPackage = dto.IsPTPackage,
                    Benefits = dto.Benefits?.Trim() ?? string.Empty,
                    Description = dto.Description?.Trim() ?? string.Empty,
                    Status = dto.Status,
                    BranchID = dto.BranchID > 0 ? dto.BranchID : null
                };

                db.Packages.Add(pkg);
                db.SaveChanges();

                return (true, "Thêm gói tập mới thành công!", pkg.PackageID);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi lưu gói tập: {ex.Message}", 0);
            }
        }

        /// <summary>
        /// Cập nhật thông tin gói tập
        /// </summary>
        public (bool Success, string Message) UpdatePackage(PackageDto dto)
        {
            try
            {
                if (dto.PackageID <= 0)
                    return (false, "ID gói tập không hợp lệ.");

                if (string.IsNullOrWhiteSpace(dto.PackageName))
                    return (false, "Tên gói tập không được để trống.");

                if (dto.Price <= 0)
                    return (false, "Giá niêm yết phải lớn hơn 0.");

                if (dto.Duration <= 0)
                    return (false, "Thời hạn gói tập phải lớn hơn 0 ngày.");

                using var db = new AppDbContext();

                var pkg = db.Packages.FirstOrDefault(p => p.PackageID == dto.PackageID);
                if (pkg == null)
                    return (false, "Không tìm thấy gói tập cần cập nhật.");

                pkg.PackageName = dto.PackageName.Trim();
                pkg.Duration = dto.Duration;
                pkg.Price = dto.Price;
                pkg.TotalPTSessions = dto.TotalPTSessions;
                pkg.IsPTPackage = dto.IsPTPackage;
                pkg.Benefits = dto.Benefits?.Trim() ?? string.Empty;
                pkg.Description = dto.Description?.Trim() ?? string.Empty;
                pkg.Status = dto.Status;
                pkg.BranchID = dto.BranchID > 0 ? dto.BranchID : null;

                db.SaveChanges();
                return (true, "Cập nhật gói tập thành công!");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi cập nhật gói tập: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa gói tập khỏi Database (hoặc ngừng mở bán nếu có hội viên đang sử dụng)
        /// </summary>
        public (bool Success, string Message) DeletePackage(int id)
        {
            try
            {
                using var db = new AppDbContext();
                var pkg = db.Packages.FirstOrDefault(p => p.PackageID == id);
                if (pkg == null)
                    return (false, "Không tìm thấy gói tập.");

                bool inUse = db.Memberships.Any(m => m.PackageID == id);
                if (inUse)
                {
                    // Chuyển sang tạm dừng bán để bảo đảm toàn vẹn dữ liệu hợp đồng hội viên
                    pkg.Status = false;
                    db.SaveChanges();
                    return (true, $"Gói \"{pkg.PackageName}\" đang có hội viên đăng ký nên đã được chuyển sang trạng thái [Tạm dừng mở bán].");
                }

                db.Packages.Remove(pkg);
                db.SaveChanges();
                return (true, $"Đã xóa gói tập \"{pkg.PackageName}\" thành công.");
            }
            catch (Exception ex)
            {
                return (false, $"Không thể xóa gói tập: {ex.Message}");
            }
        }
    }
}
