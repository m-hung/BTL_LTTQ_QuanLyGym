using System;
using QuanLyGym.BLL.Helpers;

namespace QuanLyGym.BLL.DTOs.PT
{
    public class BookingRequestDto
    {
        public int ScheduleID { get; set; }
        public int PTID { get; set; }
        public int MemberID { get; set; }
        public string MemberCode { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string TrainingType { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Pending, Approved, Cancelled, Completed
        public string Notes { get; set; } = string.Empty;
        public int ColorIndex { get; set; }

        public DateTime StartAt => BookingDate.Date + StartTime;
        public DateTime EndAt => BookingDate.Date + EndTime;
        public bool IsOverdue => Status == "Pending" && EndAt < DateTime.Now;
        public string TimeSlotText => PTDisplayHelper.SlotText(StartTime, EndTime);
        public string MemberInitials => PTDisplayHelper.Initials(MemberName);
        public string DayShort => PTDisplayHelper.DayShort(BookingDate);
        public string DayLong => PTDisplayHelper.DayLong(BookingDate);
        public string RelativeDayText => IsOverdue ? "Đã quá giờ tập" : PTDisplayHelper.RelativeDay(BookingDate);
        public string FullDateTimeText => $"{DayLong}, {BookingDate:dd/MM/yyyy} ({TimeSlotText})";

        public BookingRequestDto Clone()
        {
            return (BookingRequestDto)this.MemberwiseClone();
        }
    }
}
