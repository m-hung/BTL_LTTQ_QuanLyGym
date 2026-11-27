using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyGym.BLL.Models;
using QuanLyGym.DAL;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.BLL.Services
{
    public class EquipmentService
    {
        private static EquipmentService? _instance;
        public static EquipmentService Instance => _instance ??= new EquipmentService();

        /// <summary>
        /// Chuẩn hóa trạng thái thiết bị thành 3 trạng thái chuẩn: "Tốt", "Cần bảo trì", "Hỏng hóc"
        /// </summary>
        public static string NormalizeStatus(string? status)
        {
            if (string.IsNullOrWhiteSpace(status)) return "Tốt";
            string s = status.Trim().ToLower();
            if (s.Contains("bảo trì") || s.Contains("maintenance")) return "Cần bảo trì";
            if (s.Contains("hỏng") || s.Contains("broken") || s.Contains("hư")) return "Hỏng hóc";
            return "Tốt";
        }

        /// <summary>
        /// Phân tách Nhà cung cấp và Ghi chú từ Description
        /// </summary>
        public static (string Supplier, string CleanDescription) ParseSupplierAndDescription(string? rawDescription, string equipmentName)
        {
            string raw = rawDescription?.Trim() ?? string.Empty;
            string supplier = string.Empty;
            string note = raw;

            if (raw.StartsWith("[NCC:") && raw.Contains("]"))
            {
                int endIdx = raw.IndexOf(']');
                supplier = raw.Substring(5, endIdx - 5).Trim();
                note = raw.Substring(endIdx + 1).Trim();
            }
            else if (raw.StartsWith("NCC:", StringComparison.OrdinalIgnoreCase))
            {
                int pipe = raw.IndexOf('|');
                if (pipe > 0)
                {
                    supplier = raw.Substring(4, pipe - 4).Trim();
                    note = raw.Substring(pipe + 1).Trim();
                }
                else
                {
                    supplier = raw.Substring(4).Trim();
                    note = string.Empty;
                }
            }
            else
            {
                // Tự động nhận diện nhà cung cấp nổi tiếng nếu chưa có
                if (equipmentName.Contains("Technogym", StringComparison.OrdinalIgnoreCase))
                    supplier = "Technogym Italy";
                else if (equipmentName.Contains("LifeFitness", StringComparison.OrdinalIgnoreCase))
                    supplier = "LifeFitness USA";
                else if (equipmentName.Contains("Matrix", StringComparison.OrdinalIgnoreCase))
                    supplier = "Matrix Fitness";
                else if (equipmentName.Contains("WaterRower", StringComparison.OrdinalIgnoreCase))
                    supplier = "WaterRower Inc";
                else if (equipmentName.Contains("Olympic", StringComparison.OrdinalIgnoreCase))
                    supplier = "Olympic Pro VN";
                else if (equipmentName.Contains("Squat", StringComparison.OrdinalIgnoreCase) || equipmentName.Contains("Bench", StringComparison.OrdinalIgnoreCase))
                    supplier = "Impulse Fitness";
                else
                    supplier = "Việt Nam Sport Co.";
            }

            return (supplier, note);
        }

        /// <summary>
        /// Định dạng chuỗi Description lưu trữ kèm nhà cung cấp
        /// </summary>
        public static string FormatDescriptionWithSupplier(string supplier, string note)
        {
            string cleanSupplier = supplier?.Trim() ?? string.Empty;
            string cleanNote = note?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(cleanSupplier))
                return cleanNote;

            if (string.IsNullOrWhiteSpace(cleanNote))
                return $"[NCC: {cleanSupplier}]";

            return $"[NCC: {cleanSupplier}] {cleanNote}";
        }

        /// <summary>
        /// Truy xuất danh sách thiết bị kèm phòng tập, chi nhánh và nhà cung cấp
        /// </summary>
        public List<EquipmentDto> GetEquipments(EquipmentFilterDto? filter = null)
        {
            try
            {
                using var db = new AppDbContext();

                var query = db.Equipments
                    .Include(e => e.Room)
                        .ThenInclude(r => r!.Branch)
                    .AsNoTracking();

                var list = query.ToList();

                var dtoList = new List<EquipmentDto>();
                foreach (var eq in list)
                {
                    var (supplier, cleanNote) = ParseSupplierAndDescription(eq.Description, eq.EquipmentName);
                    string status = NormalizeStatus(eq.Status);

                    var dto = new EquipmentDto
                    {
                        EquipmentID = eq.EquipmentID,
                        EquipmentCode = !string.IsNullOrWhiteSpace(eq.EquipmentCode) ? eq.EquipmentCode : $"TB-{eq.EquipmentID:D4}",
                        EquipmentName = eq.EquipmentName,
                        RoomID = eq.RoomID,
                        RoomName = eq.Room?.RoomName ?? "Chung",
                        BranchID = eq.Room?.BranchID,
                        BranchName = eq.Room?.Branch?.BranchName ?? "Chưa phân chi nhánh",
                        PurchaseDate = eq.PurchaseDate,
                        Price = eq.Price,
                        Supplier = supplier,
                        Status = status,
                        Description = cleanNote
                    };

                    dtoList.Add(dto);
                }

                // Áp dụng bộ lọc
                if (filter != null)
                {
                    if (filter.BranchID.HasValue && filter.BranchID.Value > 0)
                    {
                        dtoList = dtoList.Where(x => x.BranchID == filter.BranchID.Value).ToList();
                    }

                    if (filter.RoomID.HasValue && filter.RoomID.Value > 0)
                    {
                        dtoList = dtoList.Where(x => x.RoomID == filter.RoomID.Value).ToList();
                    }

                    if (!string.IsNullOrWhiteSpace(filter.Status) && filter.Status != "Tất cả")
                    {
                        string targetStatus = NormalizeStatus(filter.Status);
                        dtoList = dtoList.Where(x => x.Status == targetStatus).ToList();
                    }

                    if (!string.IsNullOrWhiteSpace(filter.SearchText))
                    {
                        string keyword = filter.SearchText.Trim().ToLower();
                        dtoList = dtoList.Where(x =>
                            x.EquipmentName.ToLower().Contains(keyword) ||
                            x.EquipmentCode.ToLower().Contains(keyword) ||
                            x.Supplier.ToLower().Contains(keyword) ||
                            x.RoomName.ToLower().Contains(keyword) ||
                            x.BranchName.ToLower().Contains(keyword)
                        ).ToList();
                    }
                }

                return dtoList.OrderByDescending(x => x.EquipmentID).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GetEquipments] Error: {ex.Message}");
                return new List<EquipmentDto>();
            }
        }

        /// <summary>
        /// Thống kê chỉ số thiết bị (Tổng, Tốt, Cần bảo trì, Hỏng hóc, Tổng giá trị)
        /// </summary>
        public EquipmentSummaryDto GetEquipmentSummary(int? branchId = null)
        {
            var filter = branchId.HasValue && branchId.Value > 0
                ? new EquipmentFilterDto { BranchID = branchId.Value }
                : null;

            var list = GetEquipments(filter);

            return new EquipmentSummaryDto
            {
                TotalCount = list.Count,
                GoodCount = list.Count(x => x.Status == "Tốt"),
                MaintenanceCount = list.Count(x => x.Status == "Cần bảo trì"),
                BrokenCount = list.Count(x => x.Status == "Hỏng hóc"),
                TotalValue = list.Sum(x => x.Price)
            };
        }

        /// <summary>
        /// Lấy chi tiết 1 thiết bị theo ID
        /// </summary>
        public EquipmentDto? GetEquipmentById(int id)
        {
            try
            {
                using var db = new AppDbContext();
                var eq = db.Equipments
                    .Include(e => e.Room)
                        .ThenInclude(r => r!.Branch)
                    .AsNoTracking()
                    .FirstOrDefault(e => e.EquipmentID == id);

                if (eq == null) return null;

                var (supplier, cleanNote) = ParseSupplierAndDescription(eq.Description, eq.EquipmentName);

                return new EquipmentDto
                {
                    EquipmentID = eq.EquipmentID,
                    EquipmentCode = !string.IsNullOrWhiteSpace(eq.EquipmentCode) ? eq.EquipmentCode : $"TB-{eq.EquipmentID:D4}",
                    EquipmentName = eq.EquipmentName,
                    RoomID = eq.RoomID,
                    RoomName = eq.Room?.RoomName ?? "Chung",
                    BranchID = eq.Room?.BranchID,
                    BranchName = eq.Room?.Branch?.BranchName ?? "Chưa phân chi nhánh",
                    PurchaseDate = eq.PurchaseDate,
                    Price = eq.Price,
                    Supplier = supplier,
                    Status = NormalizeStatus(eq.Status),
                    Description = cleanNote
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GetEquipmentById] Error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Thêm mới thiết bị vào Database
        /// </summary>
        public (bool Success, string Message, int NewId) AddEquipment(EquipmentDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.EquipmentName))
                    return (false, "Tên máy không được để trống.", 0);

                if (dto.RoomID <= 0)
                    return (false, "Vui lòng chọn phòng tập / chi nhánh đặt máy.", 0);

                if (dto.Price < 0)
                    return (false, "Đơn giá không thể là số âm.", 0);

                using var db = new AppDbContext();

                // Kiểm tra mã thiết bị
                string code = dto.EquipmentCode?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(code))
                {
                    int maxId = db.Equipments.Any() ? db.Equipments.Max(e => e.EquipmentID) : 0;
                    code = $"TB-{(maxId + 1):D4}";
                }
                else
                {
                    // Trùng mã?
                    bool exists = db.Equipments.Any(e => e.EquipmentCode == code);
                    if (exists)
                    {
                        return (false, $"Mã máy \"{code}\" đã tồn tại trong hệ thống.", 0);
                    }
                }

                string finalDescription = FormatDescriptionWithSupplier(dto.Supplier, dto.Description);
                string normalizedStatus = NormalizeStatus(dto.Status);

                var equipment = new Equipment
                {
                    EquipmentName = dto.EquipmentName.Trim(),
                    EquipmentCode = code,
                    RoomID = dto.RoomID,
                    PurchaseDate = dto.PurchaseDate == default ? DateTime.Now : dto.PurchaseDate,
                    Price = dto.Price,
                    Status = normalizedStatus,
                    Description = finalDescription
                };

                db.Equipments.Add(equipment);
                db.SaveChanges();

                return (true, "Thêm thiết bị mới thành công!", equipment.EquipmentID);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi lưu thiết bị: {ex.Message}", 0);
            }
        }

        /// <summary>
        /// Chỉnh sửa thông tin thiết bị
        /// </summary>
        public (bool Success, string Message) UpdateEquipment(EquipmentDto dto)
        {
            try
            {
                if (dto.EquipmentID <= 0)
                    return (false, "ID thiết bị không hợp lệ.");

                if (string.IsNullOrWhiteSpace(dto.EquipmentName))
                    return (false, "Tên máy không được để trống.");

                if (dto.RoomID <= 0)
                    return (false, "Vui lòng chọn phòng tập / chi nhánh đặt máy.");

                if (dto.Price < 0)
                    return (false, "Đơn giá không thể là số âm.");

                using var db = new AppDbContext();

                var equipment = db.Equipments.FirstOrDefault(e => e.EquipmentID == dto.EquipmentID);
                if (equipment == null)
                    return (false, "Không tìm thấy thiết bị cần cập nhật.");

                string code = dto.EquipmentCode?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(code))
                {
                    code = $"TB-{equipment.EquipmentID:D4}";
                }
                else
                {
                    bool exists = db.Equipments.Any(e => e.EquipmentCode == code && e.EquipmentID != dto.EquipmentID);
                    if (exists)
                    {
                        return (false, $"Mã máy \"{code}\" đã được sử dụng bởi thiết bị khác.");
                    }
                }

                equipment.EquipmentName = dto.EquipmentName.Trim();
                equipment.EquipmentCode = code;
                equipment.RoomID = dto.RoomID;
                equipment.PurchaseDate = dto.PurchaseDate;
                equipment.Price = dto.Price;
                equipment.Status = NormalizeStatus(dto.Status);
                equipment.Description = FormatDescriptionWithSupplier(dto.Supplier, dto.Description);

                db.SaveChanges();
                return (true, "Cập nhật thiết bị thành công!");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi cập nhật thiết bị: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa thiết bị khỏi Database (xử lý liên kết an toàn)
        /// </summary>
        public (bool Success, string Message) DeleteEquipment(int id)
        {
            try
            {
                using var db = new AppDbContext();
                var eq = db.Equipments.FirstOrDefault(e => e.EquipmentID == id);
                if (eq == null)
                    return (false, "Không tìm thấy thiết bị.");

                // Xóa lịch bảo trì nếu có
                var maintenances = db.EquipmentMaintenances.Where(m => m.EquipmentID == id).ToList();
                if (maintenances.Any())
                {
                    db.EquipmentMaintenances.RemoveRange(maintenances);
                }

                // Xóa sự cố liên quan nếu có
                var incidents = db.Incidents.Where(i => i.EquipmentID == id).ToList();
                if (incidents.Any())
                {
                    db.Incidents.RemoveRange(incidents);
                }

                db.Equipments.Remove(eq);
                db.SaveChanges();

                return (true, $"Đã xóa máy \"{eq.EquipmentName}\" thành công.");
            }
            catch (Exception ex)
            {
                return (false, $"Không thể xóa thiết bị: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách Chi nhánh và Phòng tập phục vụ lọc & nhập liệu
        /// </summary>
        public List<BranchLookupDto> GetBranchLookups()
        {
            try
            {
                using var db = new AppDbContext();

                var branches = db.Branches.AsNoTracking().ToList();
                var rooms = db.Rooms.AsNoTracking().ToList();

                var result = new List<BranchLookupDto>();

                foreach (var b in branches)
                {
                    var branchDto = new BranchLookupDto
                    {
                        BranchID = b.BranchID,
                        BranchName = b.BranchName
                    };

                    var branchRooms = rooms.Where(r => r.BranchID == b.BranchID).ToList();
                    foreach (var r in branchRooms)
                    {
                        branchDto.Rooms.Add(new RoomLookupDto
                        {
                            RoomID = r.RoomID,
                            RoomName = r.RoomName,
                            BranchID = b.BranchID,
                            BranchName = b.BranchName
                        });
                    }

                    result.Add(branchDto);
                }

                // Nếu có các phòng chưa gán chi nhánh
                var orphanRooms = rooms.Where(r => !r.BranchID.HasValue || !branches.Any(b => b.BranchID == r.BranchID.Value)).ToList();
                if (orphanRooms.Any())
                {
                    var orphanBranch = new BranchLookupDto
                    {
                        BranchID = 0,
                        BranchName = "Khu vực chung"
                    };
                    foreach (var r in orphanRooms)
                    {
                        orphanBranch.Rooms.Add(new RoomLookupDto
                        {
                            RoomID = r.RoomID,
                            RoomName = r.RoomName,
                            BranchID = 0,
                            BranchName = "Khu vực chung"
                        });
                    }
                    result.Add(orphanBranch);
                }

                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GetBranchLookups] Error: {ex.Message}");
                return new List<BranchLookupDto>();
            }
        }

        /// <summary>
        /// Lấy danh sách tất cả các phòng tập phẳng
        /// </summary>
        public List<RoomLookupDto> GetAllRooms()
        {
            try
            {
                using var db = new AppDbContext();
                return db.Rooms
                    .Include(r => r.Branch)
                    .AsNoTracking()
                    .Select(r => new RoomLookupDto
                    {
                        RoomID = r.RoomID,
                        RoomName = r.RoomName,
                        BranchID = r.BranchID,
                        BranchName = r.Branch != null ? r.Branch.BranchName : "Chung"
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GetAllRooms] Error: {ex.Message}");
                return new List<RoomLookupDto>();
            }
        }
    }
}
