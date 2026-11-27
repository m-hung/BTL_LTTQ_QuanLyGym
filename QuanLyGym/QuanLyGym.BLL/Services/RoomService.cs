using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyGym.BLL.Models;
using QuanLyGym.DAL;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.BLL.Services
{
    public class RoomService
    {
        private static RoomService? _instance;
        public static RoomService Instance => _instance ??= new RoomService();

        private static bool _initialized = false;
        private static readonly object _initLock = new object();

        // Lưu trữ danh sách lớp học theo từng phòng (trong bộ nhớ / config tóm tắt)
        private static readonly Dictionary<string, List<string>> _roomClassesMap = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Phòng Cardio & Đạp Xe Trong Nhà (Spinning Studio)"] = new() { "Spinning RPM – Đua Xe Đạp Trong Nhà Đốt Mỡ Cấp Tốc" },
            ["Phòng Gym Trung Tâm (Khu Tạ & Kháng Lực)"] = new() { "BodyPump – Rèn Luyện Sức Bền Cơ Bắp Toàn Thân" },
            ["Phòng Huấn Luyện Boxing & Kickfit (Combat Zone)"] = new() { "Kickfit Tự Vệ & Cardio Boxing (Combat Zone)" },
            ["Phòng Huấn Luyện Cá Nhân VIP (PT Private Studio)"] = new() { },
            ["Phòng Tập Nhóm Yoga & Pilates Studio"] = new() { "Pilates Trị Liệu Tư Thế & Giảm Đau Cột Sống", "Hatha Yoga & Thiền Định Phục Hồi Năng Lượng" }
        };

        /// <summary>
        /// Khởi tạo và đồng bộ 5 phòng tập chuẩn theo đúng ảnh mẫu của người dùng
        /// </summary>
        public void EnsureInitialRooms()
        {
            if (_initialized) return;
            lock (_initLock)
            {
                if (_initialized) return;
                try
                {
                    using var db = new AppDbContext();

                    var branch = db.Branches.FirstOrDefault();
                    int branchId = branch?.BranchID ?? 1;

                    var standardRooms = new[]
                    {
                        new {
                            Name = "Phòng Cardio & Đạp Xe Trong Nhà (Spinning Studio)",
                            Location = "Tầng 2 – Khu B",
                            Capacity = 35,
                            Description = "Trang bị dàn máy chạy bộ thông minh, xe đạp địa hình spinning và máy trượt tuyết elliptical.",
                            TargetEquipments = 4
                        },
                        new {
                            Name = "Phòng Gym Trung Tâm (Khu Tạ & Kháng Lực)",
                            Location = "Tầng 1 – Khu A",
                            Capacity = 50,
                            Description = "Khu vực tập luyện chính gồm giàn tạ rời (Free Weights), rack gánh tạ và các máy tập khối chuyên dụng.",
                            TargetEquipments = 6
                        },
                        new {
                            Name = "Phòng Huấn Luyện Boxing & Kickfit (Combat Zone)",
                            Location = "Tầng 3 – Studio 2",
                            Capacity = 20,
                            Description = "Khu vực chuyên sâu boxing, kickfit với bao cát treo, thảm giảm chấn và găng đấm tiêu chuẩn thi đấu.",
                            TargetEquipments = 0
                        },
                        new {
                            Name = "Phòng Huấn Luyện Cá Nhân VIP (PT Private Studio)",
                            Location = "Tầng 2 – Phòng VIP",
                            Capacity = 10,
                            Description = "Không gian riêng tư cao cấp dành riêng cho học viên tập 1-1 cùng Huấn luyện viên thể hình chuyên nghiệp.",
                            TargetEquipments = 0
                        },
                        new {
                            Name = "Phòng Tập Nhóm Yoga & Pilates Studio",
                            Location = "Tầng 3 – Studio 1",
                            Capacity = 25,
                            Description = "Không gian yên tĩnh, sàn gỗ thông tự nhiên, gương ốp tường toàn cảnh và hệ thống thảm tập cao cấp.",
                            TargetEquipments = 0
                        }
                    };

                    var allDbRooms = db.Rooms.ToList();
                    for (int i = 0; i < standardRooms.Length; i++)
                    {
                        var sr = standardRooms[i];
                        if (i < allDbRooms.Count)
                        {
                            var r = allDbRooms[i];
                            r.RoomName = sr.Name;
                            r.RoomType = sr.Location;
                            r.Capacity = sr.Capacity;
                            r.Description = sr.Description;
                            r.Status = true;
                            if (r.BranchID == null) r.BranchID = branchId;
                        }
                        else
                        {
                            var newRoom = new Room
                            {
                                RoomName = sr.Name,
                                RoomType = sr.Location,
                                Capacity = sr.Capacity,
                                Description = sr.Description,
                                Status = true,
                                BranchID = branchId
                            };
                            db.Rooms.Add(newRoom);
                        }
                    }

                    // Nếu có phòng thừa ngoài 5 phòng chuẩn, chuyển thiết bị và lịch sang phòng chuẩn rồi dọn dẹp
                    if (allDbRooms.Count > standardRooms.Length)
                    {
                        var keepRooms = allDbRooms.Take(standardRooms.Length).ToList();
                        int fallbackRoomId = keepRooms[0].RoomID;
                        for (int i = standardRooms.Length; i < allDbRooms.Count; i++)
                        {
                            var extraRoom = allDbRooms[i];
                            // Chuyển thiết bị
                            var eqs = db.Equipments.Where(e => e.RoomID == extraRoom.RoomID).ToList();
                            foreach (var eq in eqs) eq.RoomID = fallbackRoomId;

                            // Chuyển lịch hẹn
                            var schs = db.Schedules.Where(s => s.RoomID == extraRoom.RoomID).ToList();
                            foreach (var sch in schs) sch.RoomID = fallbackRoomId;

                            db.Rooms.Remove(extraRoom);
                        }
                    }
                    db.SaveChanges();

                    // Phân bổ thiết bị trong Equipments vào các phòng (4 máy cho Cardio, 6 máy cho Phòng Tạ)
                    var currentRooms = db.Rooms.ToList();
                    var cardioRoom = currentRooms.FirstOrDefault(r => r.RoomName.Contains("Cardio"));
                    var gymRoom = currentRooms.FirstOrDefault(r => r.RoomName.Contains("Trung Tâm") || r.RoomName.Contains("Khu Tạ"));

                    var equipments = db.Equipments.ToList();
                    if (equipments.Count > 0)
                    {
                        // Reset toàn bộ thiết bị về 0 trước
                        foreach (var eq in equipments)
                        {
                            eq.RoomID = cardioRoom?.RoomID ?? 1;
                        }

                        // 4 máy cho phòng Cardio
                        if (cardioRoom != null)
                        {
                            var cardioEqs = equipments.Take(4).ToList();
                            foreach (var eq in cardioEqs)
                            {
                                eq.RoomID = cardioRoom.RoomID;
                            }
                        }

                        // 6 máy cho phòng Tạ & Kháng Lực
                        if (gymRoom != null)
                        {
                            var gymEqs = equipments.Skip(4).Take(6).ToList();
                            foreach (var eq in gymEqs)
                            {
                                eq.RoomID = gymRoom.RoomID;
                            }
                        }

                        db.SaveChanges();
                    }

                    _initialized = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[RoomService.EnsureInitialRooms] Error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Lấy danh sách các phòng tập kèm bộ lọc
        /// </summary>
        public List<RoomDto> GetRooms(RoomFilterDto? filter = null)
        {
            EnsureInitialRooms();

            using var db = new AppDbContext();
            var query = db.Rooms
                .Include(r => r.Branch)
                .AsNoTracking()
                .AsQueryable();

            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Keyword))
                {
                    string kw = filter.Keyword.Trim().ToLower();
                    query = query.Where(r =>
                        r.RoomName.ToLower().Contains(kw) ||
                        r.RoomType.ToLower().Contains(kw) ||
                        r.Description.ToLower().Contains(kw) ||
                        (r.Branch != null && r.Branch.BranchName.ToLower().Contains(kw))
                    );
                }

                if (!string.IsNullOrWhiteSpace(filter.Status) && filter.Status != "Tất cả")
                {
                    bool isActive = filter.Status == "Hoạt động";
                    query = query.Where(r => r.Status == isActive);
                }

                if (filter.BranchID.HasValue && filter.BranchID.Value > 0)
                {
                    query = query.Where(r => r.BranchID == filter.BranchID.Value);
                }
            }

            var rooms = query.OrderBy(r => r.RoomID).ToList();

            // Đếm số lượng máy tập theo từng phòng
            var eqCounts = db.Equipments
                .AsNoTracking()
                .GroupBy(e => e.RoomID)
                .Select(g => new { RoomID = g.Key, Count = g.Count() })
                .ToDictionary(x => x.RoomID, x => x.Count);

            var result = new List<RoomDto>();
            foreach (var r in rooms)
            {
                int eqCount = eqCounts.TryGetValue(r.RoomID, out int c) ? c : 0;

                // Lấy danh sách lớp học tương ứng
                var classes = new List<string>();
                if (_roomClassesMap.TryGetValue(r.RoomName, out var clsList))
                {
                    classes.AddRange(clsList);
                }
                else
                {
                    foreach (var pair in _roomClassesMap)
                    {
                        if (r.RoomName.Contains(pair.Key, StringComparison.OrdinalIgnoreCase) || pair.Key.Contains(r.RoomName, StringComparison.OrdinalIgnoreCase))
                        {
                            classes.AddRange(pair.Value);
                            break;
                        }
                    }
                }

                result.Add(new RoomDto
                {
                    RoomID = r.RoomID,
                    RoomName = r.RoomName,
                    Location = string.IsNullOrWhiteSpace(r.RoomType) ? "Tầng 1 – Khu A" : r.RoomType,
                    Capacity = r.Capacity > 0 ? r.Capacity : 30,
                    Description = r.Description,
                    Status = r.Status,
                    BranchID = r.BranchID,
                    BranchName = r.Branch?.BranchName ?? "Trụ sở chính",
                    EquipmentCount = eqCount,
                    Classes = classes
                });
            }

            return result;
        }

        /// <summary>
        /// Lấy thống kê KPI phòng tập
        /// </summary>
        public RoomSummaryDto GetSummary()
        {
            EnsureInitialRooms();

            using var db = new AppDbContext();
            var rooms = db.Rooms.AsNoTracking().ToList();
            int totalEquipments = db.Equipments.Count();

            return new RoomSummaryDto
            {
                TotalRooms = rooms.Count,
                ActiveRooms = rooms.Count(r => r.Status),
                InactiveRooms = rooms.Count(r => !r.Status),
                TotalCapacity = rooms.Sum(r => r.Capacity),
                TotalEquipments = totalEquipments
            };
        }

        /// <summary>
        /// Thêm mới phòng tập vào cơ sở dữ liệu
        /// </summary>
        public (bool Success, string Message, int RoomID) AddRoom(RoomDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RoomName))
                return (false, "Tên phòng tập không được để trống.", 0);

            try
            {
                using var db = new AppDbContext();

                var entity = new Room
                {
                    RoomName = dto.RoomName.Trim(),
                    RoomType = string.IsNullOrWhiteSpace(dto.Location) ? "Tầng 1 – Khu A" : dto.Location.Trim(),
                    Capacity = dto.Capacity > 0 ? dto.Capacity : 30,
                    Description = dto.Description?.Trim() ?? string.Empty,
                    Status = dto.Status,
                    BranchID = dto.BranchID > 0 ? dto.BranchID : db.Branches.Select(b => b.BranchID).FirstOrDefault()
                };

                db.Rooms.Add(entity);
                db.SaveChanges();

                if (dto.Classes.Count > 0)
                {
                    _roomClassesMap[entity.RoomName] = new List<string>(dto.Classes);
                }

                return (true, "Thêm phòng tập mới thành công!", entity.RoomID);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi thêm phòng tập: {ex.Message}", 0);
            }
        }

        /// <summary>
        /// Cập nhật thông tin phòng tập
        /// </summary>
        public (bool Success, string Message) UpdateRoom(RoomDto dto)
        {
            if (dto.RoomID <= 0)
                return (false, "Mã phòng tập không hợp lệ.");
            if (string.IsNullOrWhiteSpace(dto.RoomName))
                return (false, "Tên phòng tập không được để trống.");

            try
            {
                using var db = new AppDbContext();
                var entity = db.Rooms.Find(dto.RoomID);
                if (entity == null)
                    return (false, "Không tìm thấy phòng tập cần sửa.");

                string oldName = entity.RoomName;
                entity.RoomName = dto.RoomName.Trim();
                entity.RoomType = dto.Location?.Trim() ?? entity.RoomType;
                entity.Capacity = dto.Capacity;
                entity.Description = dto.Description?.Trim() ?? string.Empty;
                entity.Status = dto.Status;
                if (dto.BranchID.HasValue && dto.BranchID.Value > 0)
                {
                    entity.BranchID = dto.BranchID.Value;
                }

                db.SaveChanges();

                if (oldName != entity.RoomName && _roomClassesMap.ContainsKey(oldName))
                {
                    var classes = _roomClassesMap[oldName];
                    _roomClassesMap.Remove(oldName);
                    _roomClassesMap[entity.RoomName] = classes;
                }

                if (dto.Classes.Count > 0)
                {
                    _roomClassesMap[entity.RoomName] = new List<string>(dto.Classes);
                }

                return (true, "Cập nhật phòng tập thành công!");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi cập nhật: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật nhanh trạng thái phòng (Hoạt động / Tạm ngưng) trực tiếp từ Card
        /// </summary>
        public (bool Success, string Message) UpdateStatus(int roomId, bool newStatus)
        {
            try
            {
                using var db = new AppDbContext();
                var entity = db.Rooms.Find(roomId);
                if (entity == null)
                    return (false, "Không tìm thấy phòng tập.");

                entity.Status = newStatus;
                db.SaveChanges();
                return (true, $"Đã cập nhật trạng thái phòng thành '{(newStatus ? "Hoạt động" : "Tạm ngưng")}'!");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa phòng tập khỏi hệ thống
        /// </summary>
        public (bool Success, string Message) DeleteRoom(int roomId)
        {
            try
            {
                using var db = new AppDbContext();
                var entity = db.Rooms.Find(roomId);
                if (entity == null)
                    return (false, "Không tìm thấy phòng tập để xóa.");

                // Kiểm tra ràng buộc thiết bị
                bool hasEquipments = db.Equipments.Any(e => e.RoomID == roomId);
                bool hasSchedules = db.Schedules.Any(s => s.RoomID == roomId);

                if (hasEquipments || hasSchedules)
                {
                    // Chuyển sang tạm ngưng để đảm bảo toàn vẹn dữ liệu
                    entity.Status = false;
                    db.SaveChanges();
                    return (true, "Phòng tập đang chứa thiết bị hoặc lịch huấn luyện nên hệ thống đã chuyển trạng thái sang 'Tạm ngưng'!");
                }

                db.Rooms.Remove(entity);
                db.SaveChanges();
                _roomClassesMap.Remove(entity.RoomName);

                return (true, "Đã xóa phòng tập thành công!");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi xóa: {ex.Message}");
            }
        }
    }
}
