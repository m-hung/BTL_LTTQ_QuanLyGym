using System;
using System.Collections.Generic;

namespace QuanLyGym.BLL.Models
{
    public class PackageDto
    {
        public int PackageID { get; set; }
        public string PackageCode => $"#PKG-{PackageID}";
        public string PackageName { get; set; } = string.Empty;
        public int Duration { get; set; } // Ngày
        public decimal Price { get; set; } // VNĐ
        public int TotalPTSessions { get; set; }
        public bool IsPTPackage { get; set; }
        public string Benefits { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public int? BranchID { get; set; }
        public string BranchName { get; set; } = "Toàn hệ thống";

        // Thống kê liên kết trực tiếp từ Memberships
        public int SubscriberCount { get; set; }
        public decimal TotalRevenue { get; set; }

        // Format helpers
        public string DisplayPrice => $"{Price:N0} VNĐ";
        public string DisplayDuration => $"{Duration} ngày";
        public string DisplayPTService => IsPTPackage
            ? (TotalPTSessions > 0 ? $"✔ Có HLV riêng ({TotalPTSessions} buổi)" : "✔ Có HLV riêng")
            : "Không có PT";
        public string DisplaySubscriberCount => $"{SubscriberCount} hội viên";
        public string DisplayStatus => Status ? "Đang mở bán" : "Tạm dừng";

        /// <summary>
        /// Tên hiển thị chuẩn đẹp tương thích với ảnh mẫu của user
        /// </summary>
        public string DisplayTitle
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(PackageName))
                {
                    if (PackageName.Equals("Classic", StringComparison.OrdinalIgnoreCase))
                        return "Gói Đồng (Basic 1 Tháng)";
                    if (PackageName.Equals("Silver", StringComparison.OrdinalIgnoreCase))
                        return "Gói Bạc (Standard 3 Tháng)";
                    if (PackageName.Equals("Gold", StringComparison.OrdinalIgnoreCase))
                        return "Gói Vàng (VIP 6 Tháng kèm PT)";
                    if (PackageName.Equals("VIP Diamond", StringComparison.OrdinalIgnoreCase))
                        return "Gói Kim Cương (VVIP 1 Năm kèm PT)";
                    return PackageName;
                }
                return $"Gói tập #{PackageID}";
            }
        }
    }

    public class PackageSummaryDto
    {
        public int TotalPackages { get; set; }
        public int ActivePackages { get; set; }
        public int PTPackagesCount { get; set; }
        public int TotalSubscribers { get; set; }
        public decimal TotalRevenue { get; set; }
        public string MostPopularPackage { get; set; } = "-";
        public int MostPopularSubscribers { get; set; }
    }

    public class PackageFilterDto
    {
        public string? SearchKeyword { get; set; }
        public string? ServiceType { get; set; } // "Tất cả", "Tự tập", "Kèm PT"
        public bool? Status { get; set; }
    }
}
