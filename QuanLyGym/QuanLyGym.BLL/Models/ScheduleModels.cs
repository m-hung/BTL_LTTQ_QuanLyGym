using System;

namespace QuanLyGym.BLL.Models
{
    public class ScheduleDto
    {
        public int ScheduleID { get; set; }
        public string ScheduleCode => $"#{ScheduleID}";

        public int MemberID { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string MemberPhone { get; set; } = string.Empty;

        public int PTID { get; set; }
        public string PTName { get; set; } = string.Empty;
        public string PTSpecialization { get; set; } = string.Empty;

        public int RoomID { get; set; }
        public string RoomName { get; set; } = string.Empty;

        public int? BranchID { get; set; }
        public string BranchName { get; set; } = string.Empty;

        public DateTime TrainingDate { get; set; } = DateTime.Today;
        public string TrainingDateDisplay => TrainingDate.ToString("dd/MM/yyyy");
        public string DayOfWeekDisplay
        {
            get
            {
                switch (TrainingDate.DayOfWeek)
                {
                    case DayOfWeek.Monday: return "Thứ Hai";
                    case DayOfWeek.Tuesday: return "Thứ Ba";
                    case DayOfWeek.Wednesday: return "Thứ Tư";
                    case DayOfWeek.Thursday: return "Thứ Năm";
                    case DayOfWeek.Friday: return "Thứ Sáu";
                    case DayOfWeek.Saturday: return "Thứ Bảy";
                    case DayOfWeek.Sunday: return "Chủ Nhật";
                    default: return "";
                }
            }
        }

        public TimeSpan StartTime { get; set; } = new TimeSpan(7, 0, 0);
        public TimeSpan EndTime { get; set; } = new TimeSpan(8, 30, 0);
        public string TimeSlotDisplay => $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";

        public string TrainingType { get; set; } = "Huấn luyện 1-1 Cá nhân";
        public string Status { get; set; } = "Chờ duyệt"; // "Hoàn thành", "Đã duyệt", "Chờ duyệt", "Đã hủy"
        public string Note { get; set; } = string.Empty;

        // Mô tả hiển thị kết hợp (Bài tập + Ghi chú hệ thống)
        public string FullDescription
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Note))
                    return TrainingType;
                if (string.IsNullOrWhiteSpace(TrainingType))
                    return Note;
                return $"{TrainingType} | {Note}";
            }
        }
    }

    public class ScheduleSummaryDto
    {
        public int TotalSchedules { get; set; }
        public int CompletedCount { get; set; }
        public int ApprovedCount { get; set; }
        public int PendingCount { get; set; }
        public int CancelledCount { get; set; }
        public int TodayCount { get; set; }
        public int ActivePTCount { get; set; }
    }

    public class ScheduleFilterDto
    {
        public string Keyword { get; set; } = string.Empty;
        public int? PTID { get; set; }
        public string Status { get; set; } = "Tất cả"; // "Tất cả", "Hoàn thành", "Đã duyệt", "Chờ duyệt", "Đã hủy"
        public string DateFilter { get; set; } = "Tất cả"; // "Tất cả", "Hôm nay", "Tuần này", "Tháng này"
        public DateTime? SpecificDate { get; set; }
    }

    public class MemberLookupDto
    {
        public int MemberID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string DisplayText => string.IsNullOrWhiteSpace(Phone) ? FullName : $"{FullName} - {Phone}";
    }

    public class PTLookupDto
    {
        public int PTID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string DisplayText => string.IsNullOrWhiteSpace(Specialization) ? FullName : $"{FullName} ({Specialization})";
    }


}
