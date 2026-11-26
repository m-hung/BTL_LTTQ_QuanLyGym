using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyGym.BLL.DTOs.PT;
using QuanLyGym.BLL.Models;

namespace QuanLyGym.BLL.Services.PT
{
    /// <summary>
    /// Xử lý nghiệp vụ Yêu cầu đặt lịch của PT:
    /// lọc danh sách yêu cầu chờ duyệt, kiểm tra quá giờ, chống trùng lịch khi duyệt, và từ chối kèm lý do.
    /// </summary>
    public class PTBookingApprovalService
    {
        private readonly PTDataStore _store = PTDataStore.Instance;

        public event Action? BookingsChanged;

        public List<BookingRequestDto> GetPendingRequests(int ptId = 1, string filter = "all", string? keyword = null)
        {
            var list = LoadPendingFromSource(ptId);

            if (string.Equals(filter, "overdue", StringComparison.OrdinalIgnoreCase))
            {
                list = list.Where(b => b.IsOverdue).ToList();
            }
            else if (string.Equals(filter, "upcoming", StringComparison.OrdinalIgnoreCase))
            {
                list = list.Where(b => !b.IsOverdue).ToList();
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string kw = keyword.Trim().ToLowerInvariant();
                list = list.Where(b =>
                    b.MemberName.ToLowerInvariant().Contains(kw) ||
                    b.MemberCode.ToLowerInvariant().Contains(kw) ||
                    b.Phone.ToLowerInvariant().Contains(kw) ||
                    b.PackageName.ToLowerInvariant().Contains(kw) ||
                    b.Notes.ToLowerInvariant().Contains(kw)).ToList();
            }

            return list.OrderBy(b => b.BookingDate).ThenBy(b => b.StartTime).ToList();
        }

        public int GetPendingCount(int ptId = 1)
        {
            return LoadPendingFromSource(ptId).Count;
        }

        public int GetLateCount(int ptId = 1)
        {
            return LoadPendingFromSource(ptId).Count(b => b.IsOverdue);
        }

        public List<BookingRequestDto> GetApprovedSchedules(int ptId = 1)
        {
            return _store.ExecuteLocked(() =>
            {
                if (_store.IsDatabaseAvailable())
                {
                    try
                    {
                        using var db = _store.CreateDbContext();
                        var rows = db.Schedules
                            .Include(s => s.Member)
                            .Include(s => s.Room)
                            .AsNoTracking()
                            .Where(s => s.PTID == ptId && s.Status == "Approved")
                            .OrderBy(s => s.TrainingDate)
                            .ThenBy(s => s.StartTime)
                            .ToList();

                        return rows.Select((s, idx) => new BookingRequestDto
                        {
                            ScheduleID = s.ScheduleID,
                            PTID = s.PTID,
                            MemberID = s.MemberID,
                            MemberCode = $"HV{s.MemberID:D3}",
                            MemberName = s.Member?.FullName ?? $"Hội viên #{s.MemberID}",
                            Phone = s.Member?.Phone ?? "",
                            Email = s.Member?.Email ?? "",
                            PackageName = s.TrainingType,
                            RoomName = s.Room?.RoomName ?? "Khu PT 1-1",
                            BookingDate = s.TrainingDate,
                            StartTime = s.StartTime,
                            EndTime = s.EndTime,
                            TrainingType = s.TrainingType,
                            Status = s.Status,
                            Notes = s.Note,
                            ColorIndex = idx % 6
                        }).ToList();
                    }
                    catch { }
                }

                return _store.InMemorySchedules
                    .Where(s => s.PTID == ptId && s.Status == "Approved")
                    .OrderBy(s => s.BookingDate)
                    .ThenBy(s => s.StartTime)
                    .Select(s => s.Clone())
                    .ToList();
            });
        }

        public ServiceResult<BookingRequestDto> ApproveRequest(int scheduleId, int ptId = 1)
        {
            ServiceResult<BookingRequestDto> result = _store.ExecuteLocked(() =>
            {
                if (_store.IsDatabaseAvailable())
                {
                    try
                    {
                        using var db = _store.CreateDbContext();
                        var booking = db.Schedules
                            .Include(s => s.Member)
                            .Include(s => s.Room)
                            .FirstOrDefault(s => s.ScheduleID == scheduleId && s.PTID == ptId);

                        if (booking == null)
                            return ServiceResult<BookingRequestDto>.Fail("Không tìm thấy ca tập hoặc bạn không có quyền duyệt.");

                        if (booking.Status != "Pending")
                            return ServiceResult<BookingRequestDto>.Fail("Chỉ có thể duyệt các ca tập đang ở trạng thái chờ duyệt.");

                        // Chống trùng lịch: Kiểm tra HLV đã có ca tập Approved nào giao nhau trong cùng ngày chưa
                        bool hasConflict = db.Schedules.Any(s =>
                            s.ScheduleID != scheduleId &&
                            s.PTID == booking.PTID &&
                            s.TrainingDate.Date == booking.TrainingDate.Date &&
                            s.Status == "Approved" &&
                            s.StartTime < booking.EndTime &&
                            booking.StartTime < s.EndTime);

                        if (hasConflict)
                        {
                            string slot = $"{booking.StartTime:hh\\:mm}–{booking.EndTime:hh\\:mm}";
                            return ServiceResult<BookingRequestDto>.Fail(
                                $"Bạn đã duyệt một ca dạy khác vào khung giờ {slot} ngày {booking.TrainingDate:dd/MM/yyyy} rồi. Không thể duyệt trùng.");
                        }

                        booking.Status = "Approved";
                        db.SaveChanges();

                        var dto = MapEntityToDto(booking, 0);
                        return ServiceResult<BookingRequestDto>.Ok(dto, $"Đã duyệt lịch hẹn tập của {dto.MemberName} thành công!");
                    }
                    catch { }
                }

                var memBooking = _store.InMemorySchedules.FirstOrDefault(b => b.ScheduleID == scheduleId && b.PTID == ptId);
                if (memBooking == null)
                    return ServiceResult<BookingRequestDto>.Fail("Không tìm thấy ca tập hoặc bạn không có quyền duyệt.");

                if (memBooking.Status != "Pending")
                    return ServiceResult<BookingRequestDto>.Fail("Chỉ có thể duyệt các ca tập đang ở trạng thái chờ duyệt.");

                bool memConflict = _store.InMemorySchedules.Any(b =>
                    b.ScheduleID != scheduleId &&
                    b.PTID == memBooking.PTID &&
                    b.BookingDate.Date == memBooking.BookingDate.Date &&
                    b.Status == "Approved" &&
                    b.StartTime < memBooking.EndTime &&
                    memBooking.StartTime < b.EndTime);

                if (memConflict)
                {
                    return ServiceResult<BookingRequestDto>.Fail(
                        $"Bạn đã duyệt một ca dạy khác vào khung giờ {memBooking.TimeSlotText} ngày {memBooking.BookingDate:dd/MM/yyyy} rồi. Không thể duyệt trùng.");
                }

                memBooking.Status = "Approved";
                return ServiceResult<BookingRequestDto>.Ok(
                    memBooking.Clone(),
                    $"Đã duyệt lịch hẹn tập của {memBooking.MemberName} ({memBooking.BookingDate:dd/MM/yyyy} {memBooking.TimeSlotText}) thành công!");
            });

            if (result.Success)
            {
                _store.NotifyChanged();
                BookingsChanged?.Invoke();
            }

            return result;
        }

        public ServiceResult<BookingRequestDto> RejectRequest(int scheduleId, string? reason, int ptId = 1)
        {
            string cleanReason = string.IsNullOrWhiteSpace(reason)
                ? "HLV bận vào khung giờ này."
                : reason.Trim();

            if (cleanReason.Length > 250)
            {
                return ServiceResult<BookingRequestDto>.Fail("Lý do từ chối tối đa 250 ký tự.");
            }

            ServiceResult<BookingRequestDto> result = _store.ExecuteLocked(() =>
            {
                if (_store.IsDatabaseAvailable())
                {
                    try
                    {
                        using var db = _store.CreateDbContext();
                        var booking = db.Schedules
                            .Include(s => s.Member)
                            .Include(s => s.Room)
                            .FirstOrDefault(s => s.ScheduleID == scheduleId && s.PTID == ptId);

                        if (booking == null)
                            return ServiceResult<BookingRequestDto>.Fail("Không tìm thấy ca tập.");

                        if (booking.Status != "Pending")
                            return ServiceResult<BookingRequestDto>.Fail("Chỉ có thể từ chối các yêu cầu đang ở trạng thái chờ duyệt.");

                        booking.Status = "Cancelled";
                        booking.Note = cleanReason;
                        db.SaveChanges();

                        var dto = MapEntityToDto(booking, 0);
                        return ServiceResult<BookingRequestDto>.Ok(dto, "Đã từ chối lịch hẹn và gửi phản hồi tới học viên.");
                    }
                    catch { }
                }

                var memBooking = _store.InMemorySchedules.FirstOrDefault(b => b.ScheduleID == scheduleId && b.PTID == ptId);
                if (memBooking == null)
                    return ServiceResult<BookingRequestDto>.Fail("Không tìm thấy ca tập.");

                if (memBooking.Status != "Pending")
                    return ServiceResult<BookingRequestDto>.Fail("Chỉ có thể từ chối các yêu cầu đang ở trạng thái chờ duyệt.");

                memBooking.Status = "Cancelled";
                memBooking.Notes = cleanReason;

                return ServiceResult<BookingRequestDto>.Ok(
                    memBooking.Clone(),
                    $"Đã từ chối lịch hẹn của {memBooking.MemberName} với lý do: \"{cleanReason}\"");
            });

            if (result.Success)
            {
                _store.NotifyChanged();
                BookingsChanged?.Invoke();
            }

            return result;
        }

        private List<BookingRequestDto> LoadPendingFromSource(int ptId)
        {
            return _store.ExecuteLocked(() =>
            {
                if (_store.IsDatabaseAvailable())
                {
                    try
                    {
                        using var db = _store.CreateDbContext();
                        var rows = db.Schedules
                            .Include(s => s.Member)
                            .Include(s => s.Room)
                            .AsNoTracking()
                            .Where(s => s.PTID == ptId && s.Status == "Pending")
                            .OrderBy(s => s.TrainingDate)
                            .ThenBy(s => s.StartTime)
                            .ToList();

                        return rows.Select((s, idx) => MapEntityToDto(s, idx)).ToList();
                    }
                    catch { }
                }

                return _store.InMemorySchedules
                    .Where(s => s.PTID == ptId && s.Status == "Pending")
                    .OrderBy(s => s.BookingDate)
                    .ThenBy(s => s.StartTime)
                    .Select(s => s.Clone())
                    .ToList();
            });
        }

        private static BookingRequestDto MapEntityToDto(DAL.Entities.Schedule s, int idx)
        {
            return new BookingRequestDto
            {
                ScheduleID = s.ScheduleID,
                PTID = s.PTID,
                MemberID = s.MemberID,
                MemberCode = $"HV{s.MemberID:D3}",
                MemberName = s.Member?.FullName ?? $"Hội viên #{s.MemberID}",
                Phone = s.Member?.Phone ?? "",
                Email = s.Member?.Email ?? "",
                PackageName = string.IsNullOrWhiteSpace(s.TrainingType) ? "Huấn luyện 1-1" : s.TrainingType,
                RoomName = s.Room?.RoomName ?? "Khu PT 1-1",
                BookingDate = s.TrainingDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                TrainingType = s.TrainingType,
                Status = s.Status,
                Notes = s.Note ?? string.Empty,
                ColorIndex = idx % 6
            };
        }
    }
}
