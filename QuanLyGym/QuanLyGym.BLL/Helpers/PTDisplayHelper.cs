using System;

namespace QuanLyGym.BLL.Helpers
{
    /// <summary>
    /// Tiện ích xử lý hiển thị ngày giờ, khung giờ và chữ viết tắt cho phân hệ PT.
    /// </summary>
    public static class PTDisplayHelper
    {
        public static string Initials(string? fullName)
        {
            var words = (fullName ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0) return "PT";
            if (words.Length == 1) return words[0][..1].ToUpperInvariant();
            return (words[^2][..1] + words[^1][..1]).ToUpperInvariant();
        }

        public static string SlotText(TimeSpan start, TimeSpan end)
        {
            return $"{start:hh\\:mm}–{end:hh\\:mm}";
        }

        public static string DayShort(DateTime d) => d.DayOfWeek switch
        {
            DayOfWeek.Sunday => "CN",
            _ => "T" + ((int)d.DayOfWeek + 1)
        };

        public static string DayLong(DateTime d) => d.DayOfWeek switch
        {
            DayOfWeek.Sunday => "Chủ nhật",
            DayOfWeek.Monday => "Thứ Hai",
            DayOfWeek.Tuesday => "Thứ Ba",
            DayOfWeek.Wednesday => "Thứ Tư",
            DayOfWeek.Thursday => "Thứ Năm",
            DayOfWeek.Friday => "Thứ Sáu",
            _ => "Thứ Bảy"
        };

        public static string RelativeDay(DateTime day)
        {
            var diff = (day.Date - DateTime.Today).Days;
            return diff switch
            {
                0 => "Hôm nay",
                1 => "Ngày mai",
                -1 => "Hôm qua",
                > 1 => $"Còn {diff} ngày",
                _ => $"{-diff} ngày trước"
            };
        }
    }
}
