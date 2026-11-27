using System;
using System.Collections.Generic;

namespace QuanLyGym.BLL.Models
{
    public class EquipmentMaintenanceDto
    {
        public int MaintenanceID { get; set; }
        public int EquipmentID { get; set; }
        public string EquipmentCode { get; set; } = string.Empty;
        public string EquipmentName { get; set; } = string.Empty;
        public int RoomID { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public int? BranchID { get; set; }
        public string BranchName { get; set; } = string.Empty;

        public DateTime MaintenanceDate { get; set; }
        public DateTime NextMaintenanceDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public string Status { get; set; } = "Hoàn thành"; // "Hoàn thành", "Đang xử lý", "Chờ bảo trì"

        // Helpers
        public string DisplayCost => $"{Cost:N0} ₫";
        public string DisplayMaintenanceDate => MaintenanceDate.ToString("dd/MM/yyyy");
        public string DisplayNextMaintenanceDate => NextMaintenanceDate.ToString("dd/MM/yyyy");
        public string DisplayLocation => $"{BranchName} • {RoomName}";

        public int DaysUntilNext => (NextMaintenanceDate.Date - DateTime.Today).Days;
        public bool IsDueSoon => DaysUntilNext >= 0 && DaysUntilNext <= 30;
        public bool IsOverdue => DaysUntilNext < 0;

        /// <summary>
        /// Phân loại nhóm thiết bị: Máy chạy & Cardio, Giàn tạ & Thể lực, Hệ thống điều hòa
        /// </summary>
        public string EquipmentCategory
        {
            get
            {
                string name = EquipmentName.ToLower();
                if (name.Contains("điều hòa") || name.Contains("máy lạnh") || name.Contains("ac-") || name.Contains("daikin") || name.Contains("panasonic"))
                    return "Hệ thống điều hòa";
                if (name.Contains("chạy") || name.Contains("đạp") || name.Contains("tuyết") || name.Contains("chèo") || name.Contains("cardio"))
                    return "Dàn máy chạy & Cardio";
                return "Giàn tạ & Thể lực";
            }
        }
    }

    public class MaintenanceSummaryDto
    {
        public int TotalMaintenances { get; set; }
        public int CompletedCount { get; set; }
        public int ProcessingCount { get; set; }
        public int ScheduledCount { get; set; }
        public int DueSoonCount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal CurrentMonthCost { get; set; }
        public decimal CurrentYearCost { get; set; }
    }

    public class MaintenanceFilterDto
    {
        public int? BranchID { get; set; }
        public string? Status { get; set; } // "Tất cả", "Hoàn thành", "Đang xử lý", "Chờ bảo trì", "Sắp đến hạn"
        public string? Category { get; set; } // "Tất cả", "Dàn máy chạy & Cardio", "Giàn tạ & Thể lực", "Hệ thống điều hòa"
        public int? Year { get; set; }
        public int? Month { get; set; }
        public string? SearchKeyword { get; set; }
    }

    public class EquipmentLookupItem
    {
        public int EquipmentID { get; set; }
        public string EquipmentCode { get; set; } = string.Empty;
        public string EquipmentName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string DisplayText => $"[{EquipmentCode}] {EquipmentName} ({Location})";
    }
}
