using System;
using System.Collections.Generic;

namespace QuanLyGym.BLL.Models
{
    public class RoomDto
    {
        public int RoomID { get; set; }
        public string RoomCode => $"#ROOM-{RoomID}";

        public string RoomName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty; // Lưu vào RoomType trong DB (ví dụ: Tầng 2 – Khu B)
        public int Capacity { get; set; } = 30;
        public string CapacityText => $"{Capacity} người";

        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public string StatusText => Status ? "Hoạt động" : "Tạm ngưng";

        public int? BranchID { get; set; }
        public string BranchName { get; set; } = string.Empty;

        public int EquipmentCount { get; set; }
        public string EquipmentText => $"{EquipmentCount} máy";

        public int ClassCount => Classes.Count;
        public string ClassText => $"{ClassCount} lớp";

        public List<string> Classes { get; set; } = new();
        public string ClassesCombined
        {
            get => string.Join(" | ", Classes);
            set
            {
                Classes.Clear();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var items = value.Split(new[] { '|', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in items)
                    {
                        string trimmed = item.Trim();
                        if (!string.IsNullOrEmpty(trimmed)) Classes.Add(trimmed);
                    }
                }
            }
        }
    }

    public class RoomSummaryDto
    {
        public int TotalRooms { get; set; }
        public int ActiveRooms { get; set; }
        public int InactiveRooms { get; set; }
        public int TotalCapacity { get; set; }
        public int TotalEquipments { get; set; }
    }

    public class RoomFilterDto
    {
        public string Keyword { get; set; } = string.Empty;
        public string Status { get; set; } = "Tất cả"; // "Tất cả", "Hoạt động", "Tạm ngưng"
        public int? BranchID { get; set; }
    }
}
