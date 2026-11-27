using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyGym.BLL.Models;
using QuanLyGym.DAL;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.BLL.Services
{
    public class MaintenanceService
    {
        private static MaintenanceService? _instance;
        public static MaintenanceService Instance => _instance ??= new MaintenanceService();

        private static bool _seedChecked = false;
        private static readonly object _seedLock = new object();

        /// <summary>
        /// Đảm bảo hệ thống có sẵn danh mục điều hòa và lịch bảo dưỡng thực tế
        /// </summary>
        public void EnsureSeedData()
        {
            if (_seedChecked) return;
            lock (_seedLock)
            {
                if (_seedChecked) return;
                try
                {
                    using var db = new AppDbContext();

                    // Kiểm tra xem đã có máy điều hòa trong bảng Equipments chưa
                    bool hasAC = db.Equipments.Any(e => e.EquipmentName.Contains("điều hòa") || e.EquipmentName.Contains("máy lạnh") || e.EquipmentCode.StartsWith("AC-"));
                    if (!hasAC)
                    {
                        var room1 = db.Rooms.FirstOrDefault(r => r.BranchID == 1) ?? db.Rooms.FirstOrDefault();
                        var room2 = db.Rooms.FirstOrDefault(r => r.BranchID == 2) ?? room1;

                        if (room1 != null)
                        {
                            var ac1 = new Equipment
                            {
                                EquipmentName = "Hệ thống điều hòa trung tâm Daikin VRV",
                                EquipmentCode = "AC-01",
                                RoomID = room1.RoomID,
                                PurchaseDate = new DateTime(2025, 6, 1),
                                Price = 38000000,
                                Status = "Tốt",
                                Description = "[NCC: Daikin Vietnam Co.] Hệ thống làm mát trung tâm biến tần Inverter tiết kiệm điện"
                            };
                            db.Equipments.Add(ac1);
                            db.SaveChanges();

                            // Thêm lịch bảo trì cho AC-01
                            db.EquipmentMaintenances.Add(new EquipmentMaintenance
                            {
                                EquipmentID = ac1.EquipmentID,
                                MaintenanceDate = new DateTime(2026, 4, 10),
                                NextMaintenanceDate = new DateTime(2026, 10, 10),
                                Description = "Vệ sinh lưới lọc bụi, đo áp suất gas R32, bảo dưỡng quạt dàn nóng ngoài trời",
                                Cost = 750000,
                                Status = "Hoàn thành"
                            });
                        }

                        if (room2 != null)
                        {
                            var ac2 = new Equipment
                            {
                                EquipmentName = "Máy lạnh âm trần Cassette Panasonic Inverter",
                                EquipmentCode = "AC-02",
                                RoomID = room2.RoomID,
                                PurchaseDate = new DateTime(2025, 7, 1),
                                Price = 26000000,
                                Status = "Cần bảo trì",
                                Description = "[NCC: Panasonic HVAC] Máy lạnh âm trần công suất 5HP kèm bơm thoát nước ngưng"
                            };
                            db.Equipments.Add(ac2);
                            db.SaveChanges();

                            // Thêm lịch bảo trì cho AC-02
                            db.EquipmentMaintenances.Add(new EquipmentMaintenance
                            {
                                EquipmentID = ac2.EquipmentID,
                                MaintenanceDate = new DateTime(2026, 11, 20),
                                NextMaintenanceDate = new DateTime(2026, 12, 20),
                                Description = "Thông tắc đường ống thoát nước ngưng, khử khuẩn dàn lạnh và nạp bổ sung gas lạnh",
                                Cost = 600000,
                                Status = "Đang xử lý"
                            });
                        }

                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[EnsureSeedData] Error: {ex.Message}");
                }
                finally
                {
                    _seedChecked = true;
                }
            }
        }

        /// <summary>
        /// Truy xuất danh sách lịch sử bảo trì - bảo dưỡng theo bộ lọc
        /// </summary>
        public List<EquipmentMaintenanceDto> GetMaintenances(MaintenanceFilterDto? filter = null)
        {
            EnsureSeedData();
            try
            {
                using var db = new AppDbContext();

                var query = db.EquipmentMaintenances
                    .Include(m => m.Equipment)
                        .ThenInclude(e => e!.Room)
                            .ThenInclude(r => r!.Branch)
                    .AsNoTracking();

                var list = query.ToList();

                var result = new List<EquipmentMaintenanceDto>();
                foreach (var item in list)
                {
                    var eq = item.Equipment;
                    string code = eq != null && !string.IsNullOrWhiteSpace(eq.EquipmentCode) ? eq.EquipmentCode : $"TB-{item.EquipmentID:D4}";
                    string name = eq?.EquipmentName ?? "Thiết bị không xác định";
                    int roomId = eq?.RoomID ?? 0;
                    string roomName = eq?.Room?.RoomName ?? "Chung";
                    int? branchId = eq?.Room?.BranchID;
                    string branchName = eq?.Room?.Branch?.BranchName ?? "Chưa phân chi nhánh";

                    string status = NormalizeStatus(item.Status);

                    result.Add(new EquipmentMaintenanceDto
                    {
                        MaintenanceID = item.MaintenanceID,
                        EquipmentID = item.EquipmentID,
                        EquipmentCode = code,
                        EquipmentName = name,
                        RoomID = roomId,
                        RoomName = roomName,
                        BranchID = branchId,
                        BranchName = branchName,
                        MaintenanceDate = item.MaintenanceDate,
                        NextMaintenanceDate = item.NextMaintenanceDate,
                        Description = item.Description,
                        Cost = item.Cost,
                        Status = status
                    });
                }

                // Áp dụng bộ lọc
                if (filter != null)
                {
                    if (filter.BranchID.HasValue && filter.BranchID.Value > 0)
                    {
                        result = result.Where(x => x.BranchID == filter.BranchID.Value).ToList();
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Status) && filter.Status != "Tất cả")
                    {
                        if (filter.Status == "Sắp đến hạn" || filter.Status.Contains("đến hạn"))
                        {
                            result = result.Where(x => x.IsDueSoon || x.IsOverdue).ToList();
                        }
                        else
                        {
                            string targetStatus = NormalizeStatus(filter.Status);
                            result = result.Where(x => x.Status == targetStatus).ToList();
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Category) && filter.Category != "Tất cả")
                    {
                        result = result.Where(x => x.EquipmentCategory == filter.Category).ToList();
                    }

                    if (filter.Year.HasValue && filter.Year.Value > 0)
                    {
                        result = result.Where(x => x.MaintenanceDate.Year == filter.Year.Value).ToList();
                    }

                    if (filter.Month.HasValue && filter.Month.Value > 0)
                    {
                        result = result.Where(x => x.MaintenanceDate.Month == filter.Month.Value).ToList();
                    }

                    if (!string.IsNullOrWhiteSpace(filter.SearchKeyword))
                    {
                        string kw = filter.SearchKeyword.Trim().ToLower();
                        result = result.Where(x =>
                            x.EquipmentName.ToLower().Contains(kw) ||
                            x.EquipmentCode.ToLower().Contains(kw) ||
                            x.Description.ToLower().Contains(kw) ||
                            x.RoomName.ToLower().Contains(kw) ||
                            x.BranchName.ToLower().Contains(kw)
                        ).ToList();
                    }
                }

                return result.OrderByDescending(x => x.MaintenanceDate).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GetMaintenances] Error: {ex.Message}");
                return new List<EquipmentMaintenanceDto>();
            }
        }

        public static string NormalizeStatus(string? status)
        {
            if (string.IsNullOrWhiteSpace(status)) return "Hoàn thành";
            string s = status.Trim().ToLower();
            if (s.Contains("hoàn thành") || s.Contains("done") || s.Contains("completed")) return "Hoàn thành";
            if (s.Contains("đang") || s.Contains("processing") || s.Contains("in progress")) return "Đang xử lý";
            if (s.Contains("chờ") || s.Contains("lên lịch") || s.Contains("pending") || s.Contains("scheduled")) return "Chờ bảo trì";
            return "Hoàn thành";
        }

        /// <summary>
        /// Lấy thống kê KPI và tổng chi phí bảo trì vận hành
        /// </summary>
        public MaintenanceSummaryDto GetSummary(int? branchId = null, int? year = null)
        {
            var filter = new MaintenanceFilterDto
            {
                BranchID = branchId,
                Year = year
            };

            var list = GetMaintenances(filter);
            DateTime now = DateTime.Now;

            return new MaintenanceSummaryDto
            {
                TotalMaintenances = list.Count,
                CompletedCount = list.Count(x => x.Status == "Hoàn thành"),
                ProcessingCount = list.Count(x => x.Status == "Đang xử lý"),
                ScheduledCount = list.Count(x => x.Status == "Chờ bảo trì"),
                DueSoonCount = list.Count(x => x.IsDueSoon || x.IsOverdue),
                TotalCost = list.Sum(x => x.Cost),
                CurrentMonthCost = list.Where(x => x.MaintenanceDate.Year == now.Year && x.MaintenanceDate.Month == now.Month).Sum(x => x.Cost),
                CurrentYearCost = list.Where(x => x.MaintenanceDate.Year == now.Year).Sum(x => x.Cost)
            };
        }

        /// <summary>
        /// Tính tổng chi phí bảo trì phục vụ tính chi phí vận hành (Operating Expense - OpEx)
        /// </summary>
        public decimal GetOperatingMaintenanceCost(int year, int? month = null, int? branchId = null)
        {
            var filter = new MaintenanceFilterDto
            {
                BranchID = branchId,
                Year = year,
                Month = month
            };

            var list = GetMaintenances(filter);
            return list.Sum(x => x.Cost);
        }

        /// <summary>
        /// Thêm mới phiếu bảo trì và đồng bộ trạng thái thiết bị
        /// </summary>
        public (bool Success, string Message, int NewId) AddMaintenance(EquipmentMaintenanceDto dto)
        {
            try
            {
                if (dto.EquipmentID <= 0)
                    return (false, "Vui lòng chọn thiết bị cần bảo trì.", 0);

                if (dto.Cost < 0)
                    return (false, "Chi phí bảo trì không thể là số âm.", 0);

                using var db = new AppDbContext();

                var eq = db.Equipments.FirstOrDefault(e => e.EquipmentID == dto.EquipmentID);
                if (eq == null)
                    return (false, "Không tìm thấy thiết bị tương ứng trong hệ thống.", 0);

                string status = NormalizeStatus(dto.Status);

                var record = new EquipmentMaintenance
                {
                    EquipmentID = dto.EquipmentID,
                    MaintenanceDate = dto.MaintenanceDate == default ? DateTime.Now : dto.MaintenanceDate,
                    NextMaintenanceDate = dto.NextMaintenanceDate == default ? DateTime.Now.AddMonths(6) : dto.NextMaintenanceDate,
                    Description = dto.Description?.Trim() ?? string.Empty,
                    Cost = dto.Cost,
                    Status = status
                };

                db.EquipmentMaintenances.Add(record);

                // Đồng bộ trạng thái thiết bị
                if (status == "Đang xử lý")
                {
                    eq.Status = "Cần bảo trì";
                }
                else if (status == "Hoàn thành")
                {
                    eq.Status = "Tốt";
                }

                db.SaveChanges();
                return (true, "Đã lên lịch bảo trì thành công!", record.MaintenanceID);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi lưu lịch bảo trì: {ex.Message}", 0);
            }
        }

        /// <summary>
        /// Cập nhật phiếu bảo trì và đồng bộ trạng thái thiết bị
        /// </summary>
        public (bool Success, string Message) UpdateMaintenance(EquipmentMaintenanceDto dto)
        {
            try
            {
                if (dto.MaintenanceID <= 0)
                    return (false, "ID phiếu bảo trì không hợp lệ.");

                if (dto.EquipmentID <= 0)
                    return (false, "Vui lòng chọn thiết bị cần bảo trì.");

                if (dto.Cost < 0)
                    return (false, "Chi phí bảo trì không thể là số âm.");

                using var db = new AppDbContext();

                var record = db.EquipmentMaintenances.FirstOrDefault(m => m.MaintenanceID == dto.MaintenanceID);
                if (record == null)
                    return (false, "Không tìm thấy phiếu bảo trì cần sửa.");

                var eq = db.Equipments.FirstOrDefault(e => e.EquipmentID == dto.EquipmentID);

                string status = NormalizeStatus(dto.Status);

                record.EquipmentID = dto.EquipmentID;
                record.MaintenanceDate = dto.MaintenanceDate;
                record.NextMaintenanceDate = dto.NextMaintenanceDate;
                record.Description = dto.Description?.Trim() ?? string.Empty;
                record.Cost = dto.Cost;
                record.Status = status;

                // Đồng bộ trạng thái thiết bị
                if (eq != null)
                {
                    if (status == "Đang xử lý")
                    {
                        eq.Status = "Cần bảo trì";
                    }
                    else if (status == "Hoàn thành")
                    {
                        eq.Status = "Tốt";
                    }
                }

                db.SaveChanges();
                return (true, "Cập nhật phiếu bảo trì thành công!");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi cập nhật phiếu bảo trì: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa phiếu bảo trì
        /// </summary>
        public (bool Success, string Message) DeleteMaintenance(int id)
        {
            try
            {
                using var db = new AppDbContext();
                var record = db.EquipmentMaintenances.FirstOrDefault(m => m.MaintenanceID == id);
                if (record == null)
                    return (false, "Không tìm thấy phiếu bảo trì.");

                db.EquipmentMaintenances.Remove(record);
                db.SaveChanges();

                return (true, "Đã xóa phiếu bảo trì thành công.");
            }
            catch (Exception ex)
            {
                return (false, $"Không thể xóa phiếu bảo trì: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách thiết bị nạp vào ComboBox chọn máy bảo trì
        /// </summary>
        public List<EquipmentLookupItem> GetEquipmentsForDropdown()
        {
            EnsureSeedData();
            try
            {
                using var db = new AppDbContext();
                var list = db.Equipments
                    .Include(e => e.Room)
                        .ThenInclude(r => r!.Branch)
                    .AsNoTracking()
                    .OrderBy(e => e.EquipmentID)
                    .ToList();

                var result = new List<EquipmentLookupItem>();
                foreach (var eq in list)
                {
                    string branch = eq.Room?.Branch?.BranchName ?? "Toàn chuỗi";
                    string room = eq.Room?.RoomName ?? "Chung";
                    string name = eq.EquipmentName;
                    string cat;
                    if (name.Contains("điều hòa") || name.Contains("máy lạnh") || eq.EquipmentCode.StartsWith("AC-"))
                        cat = "Hệ thống điều hòa";
                    else if (name.Contains("chạy") || name.Contains("đạp") || name.Contains("tuyết") || name.Contains("chèo"))
                        cat = "Dàn máy chạy & Cardio";
                    else
                        cat = "Giàn tạ & Thể lực";

                    result.Add(new EquipmentLookupItem
                    {
                        EquipmentID = eq.EquipmentID,
                        EquipmentCode = !string.IsNullOrWhiteSpace(eq.EquipmentCode) ? eq.EquipmentCode : $"TB-{eq.EquipmentID:D4}",
                        EquipmentName = eq.EquipmentName,
                        Location = $"{branch} • {room}",
                        Category = cat
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GetEquipmentsForDropdown] Error: {ex.Message}");
                return new List<EquipmentLookupItem>();
            }
        }
    }
}
