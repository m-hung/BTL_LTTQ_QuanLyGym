using System;

namespace QuanLyGym.BLL.DTOs.PT
{
    public class PTProfileDto
    {
        public int PTID { get; set; }
        public int AccountID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Gender { get; set; } = "Nam";
        public DateTime DateOfBirth { get; set; } = new DateTime(1995, 5, 20);
        public string Specialization { get; set; } = string.Empty;
        public int Experience { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string? AvatarPath { get; set; }
        public string BranchName { get; set; } = "Titan Gym - Cơ sở Cầu Giấy";
        public bool Status { get; set; } = true;

        // Thống kê hiển thị trên thẻ hồ sơ công khai
        public int MemberCount { get; set; }
        public int CompletedSessions { get; set; }
        public int UpcomingSessions { get; set; }

        public PTProfileDto Clone()
        {
            return (PTProfileDto)this.MemberwiseClone();
        }
    }
}
