using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyGym.BLL.Models;
using QuanLyGym.DAL;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.BLL.Services
{
    public class ScheduleService
    {
        private static ScheduleService? _instance;
        public static ScheduleService Instance => _instance ??= new ScheduleService();

        private static bool _initialized = false;
        private static readonly object _initLock = new object();

        /// <summary>
        /// Đồng bộ và đảm bảo dữ liệu lịch tập 1-1 mẫu theo đúng hình ảnh người dùng yêu cầu
        /// </summary>
        public void EnsureInitialSchedules()
        {
            if (_initialized) return;
            lock (_initLock)
            {
                if (_initialized) return;
                try
                {
                    using var db = new AppDbContext();

                    // Đảm bảo có ít nhất 1 chi nhánh và 1 phòng tập
                    var branch = db.Branches.FirstOrDefault();
                    if (branch == null)
                    {
                        branch = new Branch
                        {
                            BranchName = "WebGym Central Flagship",
                            Address = "Số 1 Đại Cồ Việt, Hai Bà Trưng, Hà Nội",
                            Phone = "02438686868",
                            Status = true,
                            OpenTime = new TimeSpan(6, 0, 0),
                            CloseTime = new TimeSpan(22, 0, 0)
                        };
                        db.Branches.Add(branch);
                        db.SaveChanges();
                    }

                    var room = db.Rooms.FirstOrDefault(r => r.BranchID == branch.BranchID) ?? db.Rooms.FirstOrDefault();
                    if (room == null)
                    {
                        room = new Room
                        {
                            RoomName = "Phòng PT Zone 1",
                            RoomType = "1-1 Coaching Zone",
                            Capacity = 10,
                            Description = "Khu tập tạ và huấn luyện cá nhân cao cấp",
                            Status = true,
                            BranchID = branch.BranchID
                        };
                        db.Rooms.Add(room);
                        db.SaveChanges();
                    }

                    // Đảm bảo HLV Trần Minh Nhật tồn tại
                    var pt = db.PTs.FirstOrDefault(p => p.FullName.Contains("Trần Minh Nhật"));
                    if (pt == null)
                    {
                        // Kiểm tra xem có tài khoản PT không hoặc dùng tài khoản đầu tiên
                        var acc = db.Accounts.FirstOrDefault(a => a.Role == "PT") ?? db.Accounts.FirstOrDefault();
                        int accId = acc?.AccountID ?? 1;

                        pt = new PT
                        {
                            AccountID = accId,
                            FullName = "Trần Minh Nhật",
                            Gender = "Nam",
                            DateOfBirth = new DateTime(1995, 8, 15),
                            Phone = "0912345678",
                            Email = "minhnhat.pt@webgym.vn",
                            Specialization = "Tăng cơ, Giảm mỡ nâng cao & Thể hình chuyên nghiệp (Bodybuilding)",
                            Experience = 6,
                            Status = true,
                            BranchID = branch.BranchID
                        };
                        db.PTs.Add(pt);
                        db.SaveChanges();
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(pt.Specialization))
                        {
                            pt.Specialization = "Tăng cơ, Giảm mỡ nâng cao & Thể hình chuyên nghiệp (Bodybuilding)";
                            db.SaveChanges();
                        }
                    }

                    // Danh sách hội viên mẫu theo ảnh người dùng
                    var sampleMembers = new[]
                    {
                        new { Name = "Nguyễn Hữu Đức", Phone = "0987654329", Gender = "Nam" },
                        new { Name = "Bùi Phương Anh", Phone = "0987654324", Gender = "Nữ" },
                        new { Name = "Trần Bảo Ngọc", Phone = "0987654326", Gender = "Nữ" },
                        new { Name = "Nguyễn Quang Hải", Phone = "0987654321", Gender = "Nam" },
                        new { Name = "Đỗ Mai Linh", Phone = "0987654328", Gender = "Nữ" },
                        new { Name = "Vũ Đức Thắng", Phone = "0987654323", Gender = "Nam" },
                        new { Name = "Đặng Thanh Hương", Phone = "0987654322", Gender = "Nữ" },
                        new { Name = "Đào Tuấn Anh", Phone = "0987654325", Gender = "Nam" },
                        new { Name = "Phạm Hoàng Nam", Phone = "0987654327", Gender = "Nam" }
                    };

                    var memberDict = new Dictionary<string, Member>();
                    foreach (var sm in sampleMembers)
                    {
                        var m = db.Members.FirstOrDefault(x => x.FullName == sm.Name || x.Phone == sm.Phone);
                        if (m == null)
                        {
                            var acc = db.Accounts.FirstOrDefault(a => a.Role == "Member") ?? db.Accounts.FirstOrDefault();
                            m = new Member
                            {
                                AccountID = acc?.AccountID ?? 1,
                                FullName = sm.Name,
                                Phone = sm.Phone,
                                Gender = sm.Gender,
                                DateOfBirth = new DateTime(1998, 5, 20),
                                Email = $"{sm.Phone}@gmail.com",
                                Address = "Hà Nội",
                                BranchID = branch.BranchID,
                                CreatedAt = DateTime.Now
                            };
                            db.Members.Add(m);
                            db.SaveChanges();
                        }
                        memberDict[sm.Name] = m;
                    }

                    // 1. Chuẩn hóa các trạng thái tiếng Anh cũ nếu có (Scheduled -> Đã duyệt, Pending -> Chờ duyệt)
                    var existingSchedules = db.Schedules.ToList();
                    bool hasChanged = false;
                    foreach (var s in existingSchedules)
                    {
                        string norm = NormalizeStatus(s.Status);
                        if (s.Status != norm)
                        {
                            s.Status = norm;
                            hasChanged = true;
                        }
                    }
                    if (hasChanged) db.SaveChanges();

                    // 2. Kiểm tra nếu chưa có các buổi tập mẫu của HLV Trần Minh Nhật theo ảnh người dùng
                    bool hasNhatSessions = db.Schedules.Any(s => s.PTID == pt.PTID && s.TrainingType.Contains("Overhead Press"));
                    if (!hasNhatSessions)
                    {
                        var sessionsData = new[]
                        {
                            new {
                                MemberName = "Nguyễn Hữu Đức",
                                Date = new DateTime(2026, 11, 26),
                                Start = new TimeSpan(7, 0, 0),
                                End = new TimeSpan(8, 30, 0),
                                Status = "Hoàn thành",
                                Workout = "Overhead Press & Pull-up",
                                Note = "Hệ thống tự động xác nhận hoàn thành sau 24h"
                            },
                            new {
                                MemberName = "Bùi Phương Anh",
                                Date = new DateTime(2026, 11, 26),
                                Start = new TimeSpan(14, 0, 0),
                                End = new TimeSpan(15, 30, 0),
                                Status = "Hoàn thành",
                                Workout = "Đốt mỡ thân dưới",
                                Note = "Hệ thống tự động xác nhận hoàn thành sau 24h"
                            },
                            new {
                                MemberName = "Trần Bảo Ngọc",
                                Date = new DateTime(2026, 11, 26),
                                Start = new TimeSpan(17, 30, 0),
                                End = new TimeSpan(19, 0, 0),
                                Status = "Hoàn thành",
                                Workout = "Thân trên & siết bụng",
                                Note = "Hệ thống tự động xác nhận hoàn thành sau 24h"
                            },
                            new {
                                MemberName = "Nguyễn Quang Hải",
                                Date = new DateTime(2026, 11, 25),
                                Start = new TimeSpan(8, 0, 0),
                                End = new TimeSpan(9, 30, 0),
                                Status = "Hoàn thành",
                                Workout = "Deadlift & Lưng dày",
                                Note = "Hệ thống tự động xác nhận hoàn thành sau 24h"
                            },
                            new {
                                MemberName = "Đỗ Mai Linh",
                                Date = new DateTime(2026, 11, 25),
                                Start = new TimeSpan(15, 30, 0),
                                End = new TimeSpan(17, 0, 0),
                                Status = "Hoàn thành",
                                Workout = "Tập Core & phục hồi vai",
                                Note = "Hệ thống tự động xác nhận hoàn thành sau 24h"
                            },
                            new {
                                MemberName = "Vũ Đức Thắng",
                                Date = new DateTime(2026, 11, 25),
                                Start = new TimeSpan(18, 30, 0),
                                End = new TimeSpan(20, 0, 0),
                                Status = "Hoàn thành",
                                Workout = "Squat kỹ thuật & bắp chân",
                                Note = "Hệ thống tự động xác nhận hoàn thành sau 24h"
                            },
                            new {
                                MemberName = "Nguyễn Hữu Đức",
                                Date = new DateTime(2026, 11, 24),
                                Start = new TimeSpan(7, 0, 0),
                                End = new TimeSpan(8, 30, 0),
                                Status = "Hoàn thành",
                                Workout = "Full Body Functional",
                                Note = "Hệ thống tự động xác nhận hoàn thành sau 24h"
                            },
                            new {
                                MemberName = "Đặng Thanh Hương",
                                Date = new DateTime(2026, 11, 24),
                                Start = new TimeSpan(10, 0, 0),
                                End = new TimeSpan(11, 30, 0),
                                Status = "Hoàn thành",
                                Workout = "Đo InBody & chỉnh bài tập eo",
                                Note = "Hệ thống tự động xác nhận hoàn thành sau 24h"
                            },
                            new {
                                MemberName = "Đào Tuấn Anh",
                                Date = new DateTime(2026, 11, 28),
                                Start = new TimeSpan(17, 0, 0),
                                End = new TimeSpan(18, 30, 0),
                                Status = "Chờ duyệt",
                                Workout = "Ngực & Tay sau tăng tiến",
                                Note = "Hội viên yêu cầu HLV hướng dẫn tạ đơn nặng"
                            },
                            new {
                                MemberName = "Đỗ Mai Linh",
                                Date = new DateTime(2026, 11, 28),
                                Start = new TimeSpan(15, 0, 0),
                                End = new TimeSpan(16, 30, 0),
                                Status = "Đã duyệt",
                                Workout = "HIIT & Cardio thon gọn",
                                Note = "Đã chuẩn bị thảm và tạ ấm kettlebell"
                            },
                            new {
                                MemberName = "Vũ Đức Thắng",
                                Date = new DateTime(2026, 11, 28),
                                Start = new TimeSpan(8, 30, 0),
                                End = new TimeSpan(10, 0, 0),
                                Status = "Đã duyệt",
                                Workout = "Chân đùi & Bật nhảy Plyometrics",
                                Note = "Buổi tập chuyên sâu tăng sức bền"
                            },
                            new {
                                MemberName = "Phạm Hoàng Nam",
                                Date = new DateTime(2026, 11, 29),
                                Start = new TimeSpan(16, 0, 0),
                                End = new TimeSpan(17, 30, 0),
                                Status = "Đã duyệt",
                                Workout = "Lưng xô & Cẳng tay",
                                Note = "Tập trung cải thiện kéo xà Pull-up"
                            },
                            new {
                                MemberName = "Nguyễn Hữu Đức",
                                Date = new DateTime(2026, 11, 29),
                                Start = new TimeSpan(9, 0, 0),
                                End = new TimeSpan(10, 30, 0),
                                Status = "Đã duyệt",
                                Workout = "Bench Press & Vai trước",
                                Note = "Kiểm tra mức tạ PR 100kg"
                            }
                        };

                        foreach (var s in sessionsData)
                        {
                            if (memberDict.TryGetValue(s.MemberName, out var m))
                            {
                                db.Schedules.Add(new Schedule
                                {
                                    MemberID = m.MemberID,
                                    PTID = pt.PTID,
                                    RoomID = room.RoomID,
                                    BranchID = branch.BranchID,
                                    TrainingDate = s.Date,
                                    StartTime = s.Start,
                                    EndTime = s.End,
                                    TrainingType = s.Workout,
                                    Status = s.Status,
                                    Note = s.Note
                                });
                            }
                        }
                        db.SaveChanges();
                    }

                    _initialized = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ScheduleService.EnsureInitialSchedules] Error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Lấy danh sách lịch hẹn 1-1 theo các tiêu chí lọc
        /// </summary>
        public List<ScheduleDto> GetSchedules(ScheduleFilterDto? filter = null)
        {
            EnsureInitialSchedules();

            using var db = new AppDbContext();
            var query = db.Schedules
                .Include(s => s.Member)
                .Include(s => s.PT)
                .Include(s => s.Room)
                .Include(s => s.Branch)
                .AsNoTracking()
                .AsQueryable();

            if (filter != null)
            {
                // Lọc theo từ khóa (Tên hội viên, SĐT, Tên HLV, Nội dung bài tập, Ghi chú)
                if (!string.IsNullOrWhiteSpace(filter.Keyword))
                {
                    string kw = filter.Keyword.Trim().ToLower();
                    query = query.Where(s =>
                        (s.Member != null && s.Member.FullName.ToLower().Contains(kw)) ||
                        (s.Member != null && s.Member.Phone.ToLower().Contains(kw)) ||
                        (s.PT != null && s.PT.FullName.ToLower().Contains(kw)) ||
                        s.TrainingType.ToLower().Contains(kw) ||
                        s.Note.ToLower().Contains(kw) ||
                        s.ScheduleID.ToString().Contains(kw)
                    );
                }

                // Lọc theo PT
                if (filter.PTID.HasValue && filter.PTID.Value > 0)
                {
                    query = query.Where(s => s.PTID == filter.PTID.Value);
                }

                // Lọc theo trạng thái
                if (!string.IsNullOrWhiteSpace(filter.Status) && filter.Status != "Tất cả")
                {
                    query = query.Where(s => s.Status == filter.Status);
                }

                // Lọc theo thời gian
                if (filter.SpecificDate.HasValue)
                {
                    var d = filter.SpecificDate.Value.Date;
                    query = query.Where(s => s.TrainingDate.Date == d);
                }
                else if (!string.IsNullOrWhiteSpace(filter.DateFilter) && filter.DateFilter != "Tất cả")
                {
                    var today = DateTime.Today;
                    if (filter.DateFilter == "Hôm nay")
                    {
                        query = query.Where(s => s.TrainingDate.Date == today);
                    }
                    else if (filter.DateFilter == "Tuần này")
                    {
                        int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                        var startOfWeek = today.AddDays(-diff).Date;
                        var endOfWeek = startOfWeek.AddDays(7);
                        query = query.Where(s => s.TrainingDate.Date >= startOfWeek && s.TrainingDate.Date < endOfWeek);
                    }
                    else if (filter.DateFilter == "Tháng này")
                    {
                        var startOfMonth = new DateTime(today.Year, today.Month, 1);
                        var endOfMonth = startOfMonth.AddMonths(1);
                        query = query.Where(s => s.TrainingDate.Date >= startOfMonth && s.TrainingDate.Date < endOfMonth);
                    }
                }
            }

            // Sắp xếp: Ngày mới nhất lên đầu, sau đó theo giờ bắt đầu
            var rawList = query
                .OrderByDescending(s => s.TrainingDate)
                .ThenByDescending(s => s.StartTime)
                .ToList();

            var result = new List<ScheduleDto>();
            foreach (var s in rawList)
            {
                result.Add(new ScheduleDto
                {
                    ScheduleID = s.ScheduleID,
                    MemberID = s.MemberID,
                    MemberName = s.Member?.FullName ?? "Hội viên ẩn danh",
                    MemberPhone = s.Member?.Phone ?? "Chưa có SĐT",
                    PTID = s.PTID,
                    PTName = s.PT?.FullName ?? "HLV Gym",
                    PTSpecialization = s.PT?.Specialization ?? "HLV Thể hình chuyên nghiệp",
                    RoomID = s.RoomID,
                    RoomName = s.Room?.RoomName ?? "Phòng PT 1-1",
                    BranchID = s.BranchID,
                    BranchName = s.Branch?.BranchName ?? "Trụ sở chính",
                    TrainingDate = s.TrainingDate,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    TrainingType = s.TrainingType,
                    Status = NormalizeStatus(s.Status),
                    Note = s.Note
                });
            }

            return result;
        }

        public static string NormalizeStatus(string? status)
        {
            if (string.IsNullOrWhiteSpace(status)) return "Chờ duyệt";
            string s = status.Trim();
            if (s.Equals("Scheduled", StringComparison.OrdinalIgnoreCase) || s.Equals("Confirmed", StringComparison.OrdinalIgnoreCase))
                return "Đã duyệt";
            if (s.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                return "Chờ duyệt";
            if (s.Equals("Completed", StringComparison.OrdinalIgnoreCase) || s.Equals("Finished", StringComparison.OrdinalIgnoreCase))
                return "Hoàn thành";
            if (s.Equals("Cancelled", StringComparison.OrdinalIgnoreCase) || s.Equals("Canceled", StringComparison.OrdinalIgnoreCase))
                return "Đã hủy";
            return s;
        }

        /// <summary>
        /// Lấy thống kê tổng hợp phục vụ hiển thị các thẻ KPI
        /// </summary>
        public ScheduleSummaryDto GetSummary()
        {
            EnsureInitialSchedules();

            using var db = new AppDbContext();
            var all = db.Schedules.AsNoTracking().ToList();
            var today = DateTime.Today;

            return new ScheduleSummaryDto
            {
                TotalSchedules = all.Count,
                CompletedCount = all.Count(s => NormalizeStatus(s.Status) == "Hoàn thành"),
                ApprovedCount = all.Count(s => NormalizeStatus(s.Status) == "Đã duyệt"),
                PendingCount = all.Count(s => NormalizeStatus(s.Status) == "Chờ duyệt"),
                CancelledCount = all.Count(s => NormalizeStatus(s.Status) == "Đã hủy"),
                TodayCount = all.Count(s => s.TrainingDate.Date == today),
                ActivePTCount = db.PTs.Count(p => p.Status)
            };
        }

        /// <summary>
        /// Đặt lịch tập 1-1 mới có kiểm tra trùng lịch của Huấn luyện viên
        /// </summary>
        public (bool Success, string Message, int ScheduleID) AddSchedule(ScheduleDto dto)
        {
            if (dto.MemberID <= 0)
                return (false, "Vui lòng chọn hội viên tham gia buổi tập.", 0);
            if (dto.PTID <= 0)
                return (false, "Vui lòng chọn huấn luyện viên phụ trách (PT).", 0);
            if (dto.StartTime >= dto.EndTime)
                return (false, "Giờ bắt đầu phải sớm hơn giờ kết thúc buổi tập.", 0);

            try
            {
                using var db = new AppDbContext();

                // Kiểm tra xung đột lịch của PT
                var date = dto.TrainingDate.Date;
                var conflictPT = db.Schedules.FirstOrDefault(s =>
                    s.PTID == dto.PTID &&
                    s.TrainingDate.Date == date &&
                    s.Status != "Đã hủy" &&
                    s.StartTime < dto.EndTime &&
                    s.EndTime > dto.StartTime);

                if (conflictPT != null)
                {
                    return (false, $"Huấn luyện viên đã có lịch huấn luyện khác vào khung giờ ({conflictPT.StartTime:hh\\:mm} - {conflictPT.EndTime:hh\\:mm}) trong ngày {dto.TrainingDateDisplay}!", 0);
                }

                // Kiểm tra xung đột lịch của Hội viên
                var conflictMember = db.Schedules.FirstOrDefault(s =>
                    s.MemberID == dto.MemberID &&
                    s.TrainingDate.Date == date &&
                    s.Status != "Đã hủy" &&
                    s.StartTime < dto.EndTime &&
                    s.EndTime > dto.StartTime);

                if (conflictMember != null)
                {
                    return (false, $"Hội viên này đã có lịch tập khác vào khung giờ ({conflictMember.StartTime:hh\\:mm} - {conflictMember.EndTime:hh\\:mm}) trong ngày {dto.TrainingDateDisplay}!", 0);
                }

                // Đảm bảo có RoomID và BranchID
                int roomId = dto.RoomID;
                if (roomId <= 0)
                {
                    var defRoom = db.Rooms.FirstOrDefault();
                    roomId = defRoom?.RoomID ?? 1;
                }

                int? branchId = dto.BranchID;
                if (!branchId.HasValue || branchId.Value <= 0)
                {
                    var pt = db.PTs.Find(dto.PTID);
                    branchId = pt?.BranchID ?? db.Branches.Select(b => b.BranchID).FirstOrDefault();
                }

                var entity = new Schedule
                {
                    MemberID = dto.MemberID,
                    PTID = dto.PTID,
                    RoomID = roomId,
                    BranchID = branchId,
                    TrainingDate = dto.TrainingDate.Date,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    TrainingType = string.IsNullOrWhiteSpace(dto.TrainingType) ? "Huấn luyện 1-1 Cá nhân" : dto.TrainingType.Trim(),
                    Status = string.IsNullOrWhiteSpace(dto.Status) ? "Chờ duyệt" : dto.Status.Trim(),
                    Note = dto.Note?.Trim() ?? string.Empty
                };

                db.Schedules.Add(entity);
                db.SaveChanges();

                return (true, "Đặt lịch tập 1-1 với PT thành công!", entity.ScheduleID);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi hệ thống khi đặt lịch: {ex.Message}", 0);
            }
        }

        /// <summary>
        /// Cập nhật thông tin lịch hẹn 1-1
        /// </summary>
        public (bool Success, string Message) UpdateSchedule(ScheduleDto dto)
        {
            if (dto.ScheduleID <= 0)
                return (false, "Mã lịch hẹn không hợp lệ.");
            if (dto.MemberID <= 0)
                return (false, "Vui lòng chọn hội viên.");
            if (dto.PTID <= 0)
                return (false, "Vui lòng chọn HLV (PT).");
            if (dto.StartTime >= dto.EndTime)
                return (false, "Giờ bắt đầu phải trước giờ kết thúc.");

            try
            {
                using var db = new AppDbContext();
                var entity = db.Schedules.Find(dto.ScheduleID);
                if (entity == null)
                    return (false, "Không tìm thấy lịch hẹn trong cơ sở dữ liệu.");

                // Kiểm tra xung đột lịch của PT (trừ chính lịch hẹn đang sửa)
                var date = dto.TrainingDate.Date;
                var conflictPT = db.Schedules.FirstOrDefault(s =>
                    s.ScheduleID != dto.ScheduleID &&
                    s.PTID == dto.PTID &&
                    s.TrainingDate.Date == date &&
                    s.Status != "Đã hủy" &&
                    s.StartTime < dto.EndTime &&
                    s.EndTime > dto.StartTime);

                if (conflictPT != null)
                {
                    return (false, $"Huấn luyện viên đã có lịch khác trong khung giờ ({conflictPT.StartTime:hh\\:mm} - {conflictPT.EndTime:hh\\:mm})!");
                }

                entity.MemberID = dto.MemberID;
                entity.PTID = dto.PTID;
                if (dto.RoomID > 0) entity.RoomID = dto.RoomID;
                if (dto.BranchID.HasValue && dto.BranchID.Value > 0) entity.BranchID = dto.BranchID;
                entity.TrainingDate = dto.TrainingDate.Date;
                entity.StartTime = dto.StartTime;
                entity.EndTime = dto.EndTime;
                entity.TrainingType = dto.TrainingType?.Trim() ?? "Huấn luyện 1-1";
                entity.Status = dto.Status?.Trim() ?? "Đã duyệt";
                entity.Note = dto.Note?.Trim() ?? string.Empty;

                db.SaveChanges();
                return (true, "Cập nhật lịch hẹn thành công!");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi cập nhật: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật nhanh trạng thái lịch hẹn (Hoàn thành, Đã duyệt, Đã hủy)
        /// </summary>
        public (bool Success, string Message) UpdateStatus(int scheduleId, string newStatus)
        {
            try
            {
                using var db = new AppDbContext();
                var entity = db.Schedules.Find(scheduleId);
                if (entity == null)
                    return (false, "Không tìm thấy lịch hẹn.");

                entity.Status = newStatus;
                db.SaveChanges();
                return (true, $"Đã cập nhật trạng thái thành '{newStatus}'!");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa lịch hẹn khỏi hệ thống
        /// </summary>
        public (bool Success, string Message) DeleteSchedule(int scheduleId)
        {
            try
            {
                using var db = new AppDbContext();
                var entity = db.Schedules.Find(scheduleId);
                if (entity == null)
                    return (false, "Không tìm thấy lịch hẹn để xóa.");

                db.Schedules.Remove(entity);
                db.SaveChanges();
                return (true, "Đã xóa lịch hẹn thành công!");
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi xóa lịch hẹn: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách hội viên cho ComboBox đặt lịch
        /// </summary>
        public List<MemberLookupDto> GetMembersForBooking()
        {
            using var db = new AppDbContext();
            return db.Members
                .AsNoTracking()
                .OrderBy(m => m.FullName)
                .Select(m => new MemberLookupDto
                {
                    MemberID = m.MemberID,
                    FullName = m.FullName,
                    Phone = m.Phone
                })
                .ToList();
        }

        /// <summary>
        /// Lấy danh sách Huấn luyện viên cho ComboBox đặt lịch
        /// </summary>
        public List<PTLookupDto> GetPTsForBooking()
        {
            using var db = new AppDbContext();
            return db.PTs
                .AsNoTracking()
                .Where(p => p.Status)
                .OrderBy(p => p.FullName)
                .Select(p => new PTLookupDto
                {
                    PTID = p.PTID,
                    FullName = p.FullName,
                    Specialization = p.Specialization
                })
                .ToList();
        }

        /// <summary>
        /// Lấy danh sách Phòng tập cho ComboBox đặt lịch
        /// </summary>
        public List<RoomLookupDto> GetRoomsForBooking()
        {
            using var db = new AppDbContext();
            return db.Rooms
                .Include(r => r.Branch)
                .AsNoTracking()
                .Where(r => r.Status)
                .OrderBy(r => r.RoomName)
                .Select(r => new RoomLookupDto
                {
                    RoomID = r.RoomID,
                    RoomName = r.RoomName,
                    BranchID = r.BranchID,
                    BranchName = r.Branch != null ? r.Branch.BranchName : ""
                })
                .ToList();
        }
    }
}
