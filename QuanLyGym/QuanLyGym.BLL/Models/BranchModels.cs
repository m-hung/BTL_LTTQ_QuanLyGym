using System;

namespace QuanLyGym.BLL.Models
{
    public class BranchModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Manager { get; set; } = string.Empty;
        public string OpenTime { get; set; } = "06:00";
        public string CloseTime { get; set; } = "22:00";
        public int AreaSqM { get; set; } = 1000;
        public bool IsOnline { get; set; } = true;
        public string Note { get; set; } = string.Empty;
        public int MemberCount { get; set; } = 0;
        public int StaffCount { get; set; } = 0;
        public long MonthlyRevenue { get; set; } = 0;
    }

    public class BranchStaffItem
    {
        public string StaffId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = "PT";
        public string Phone { get; set; } = string.Empty;
        public string Shift { get; set; } = "Ca sáng (06:00 - 14:00)";
        public string JoinDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public bool IsWorking { get; set; } = true;
    }

    public class BranchMemberItem
    {
        public string MemberId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Package { get; set; } = "Gói Classic";
        public string AssignedPt { get; set; } = "Tự tập";
        public string StartDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
        public string ExpireDate { get; set; } = DateTime.Now.AddMonths(12).ToString("dd/MM/yyyy");
        public string Status { get; set; } = "Đang tập";
    }

    public class RevenueSummaryDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int TotalBranches { get; set; }
        public int ActiveBranches { get; set; }
        public int TotalStaff { get; set; }
    }

    public class MonthlyRevenueDto
    {
        public int Month { get; set; }
        public string Label { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public double RevenueInMillions => (double)Math.Round(Revenue / 1_000_000m, 2);
    }

    public class BranchRevenueDto
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public double RevenueInMillions => (double)Math.Round(Revenue / 1_000_000m, 2);
        public int MemberCount { get; set; }
        public int StaffCount { get; set; }
        public bool Status { get; set; }
    }

    public class ExpiringMembershipDto
    {
        public string MemberName { get; set; } = string.Empty;
        public string BranchName { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public DateTime EndDate { get; set; }
        public int DaysRemaining { get; set; }
    }

    public class MonthlyNewMembersDto
    {
        public int Month { get; set; }
        public string Label { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class BranchMonthlyRevenueDetailDto
    {
        public int Month { get; set; }
        public string MonthLabel { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public double RevenueInMillions => (double)Math.Round(Revenue / 1_000_000m, 2);
        public int MembershipCount { get; set; }
        public string TopPackage { get; set; } = "-";
        public double SharePercent { get; set; }
    }
}
