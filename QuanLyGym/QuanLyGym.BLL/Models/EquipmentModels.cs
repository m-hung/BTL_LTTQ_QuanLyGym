using System;
using System.Collections.Generic;

namespace QuanLyGym.BLL.Models
{
    public class EquipmentDto
    {
        public int EquipmentID { get; set; }
        public string EquipmentCode { get; set; } = string.Empty;
        public string EquipmentName { get; set; } = string.Empty;
        public int RoomID { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public int? BranchID { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public decimal Price { get; set; }
        public string Supplier { get; set; } = string.Empty;
        public string Status { get; set; } = "Tốt";
        public string Description { get; set; } = string.Empty;

        // Formatted display helpers
        public string DisplayCode => !string.IsNullOrWhiteSpace(EquipmentCode)
            ? EquipmentCode
            : $"TB-{EquipmentID:D4}";

        public string DisplayBranch => !string.IsNullOrWhiteSpace(BranchName) ? BranchName : "Toàn hệ thống";
        public string DisplayRoom => !string.IsNullOrWhiteSpace(RoomName) ? RoomName : "Chung";
        public string DisplayLocation => $"{DisplayBranch} • {DisplayRoom}";
        public string DisplayPrice => $"{Price:N0} ₫";
        public string DisplayPurchaseDate => PurchaseDate.ToString("dd/MM/yyyy");
    }

    public class EquipmentSummaryDto
    {
        public int TotalCount { get; set; }
        public int GoodCount { get; set; }
        public int MaintenanceCount { get; set; }
        public int BrokenCount { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class EquipmentFilterDto
    {
        public string? SearchText { get; set; }
        public int? BranchID { get; set; }
        public int? RoomID { get; set; }
        public string? Status { get; set; } // "Tất cả", "Tốt", "Cần bảo trì", "Hỏng hóc"
    }

    public class BranchLookupDto
    {
        public int BranchID { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public List<RoomLookupDto> Rooms { get; set; } = new();
    }

    public class RoomLookupDto
    {
        public int RoomID { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public int? BranchID { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string DisplayText => string.IsNullOrWhiteSpace(BranchName) ? RoomName : $"{RoomName} - {BranchName}";
    }
}
