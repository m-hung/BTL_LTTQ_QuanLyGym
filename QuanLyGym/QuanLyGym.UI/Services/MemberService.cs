using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyGym.DAL;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.UI.Services
{
    public class MemberService
    {
        private static MemberService? _instance;
        public static MemberService Instance => _instance ??= new MemberService();

        public int CurrentMemberId { get; set; } = 1; // Default demo member

        /// <summary>
        /// Gán CurrentMemberId theo tài khoản đang đăng nhập (AccountID -> MemberID).
        /// Trả về false nếu tài khoản chưa có hồ sơ hội viên.
        /// </summary>
        public bool LoadCurrentMemberByAccount(int accountId)
        {
            using var db = new AppDbContext();
            var memberId = db.Members
                .Where(m => m.AccountID == accountId)
                .Select(m => (int?)m.MemberID)
                .FirstOrDefault();

            CurrentMemberId = memberId ?? 0;
            return memberId.HasValue;
        }

        public Member? GetCurrentMember()
        {
            try
            {
                using var db = new AppDbContext();
                return db.Members
                    .Include(m => m.Branch)
                    .Include(m => m.Account)
                    .FirstOrDefault(m => m.MemberID == CurrentMemberId);
            }
            catch
            {
                return null;
            }
        }

        public Membership? GetActiveMembership()
        {
            try
            {
                using var db = new AppDbContext();
                return db.Memberships
                    .Include(m => m.Package)
                    .Include(m => m.Branch)
                    .OrderByDescending(m => m.MembershipID)
                    .FirstOrDefault(m => m.MemberID == CurrentMemberId && m.Status == "Active");
            }
            catch
            {
                return null;
            }
        }

        public List<Membership> GetMembershipHistory()
        {
            try
            {
                using var db = new AppDbContext();
                return db.Memberships
                    .Include(m => m.Package)
                    .Where(m => m.MemberID == CurrentMemberId)
                    .OrderByDescending(m => m.RegisterDate)
                    .ToList();
            }
            catch
            {
                return new List<Membership>();
            }
        }

        public List<Schedule> GetUpcomingSchedules()
        {
            try
            {
                using var db = new AppDbContext();
                return db.Schedules
                    .Include(s => s.PT)
                    .Include(s => s.Room)
                    .Where(s => s.MemberID == CurrentMemberId)
                    .OrderBy(s => s.TrainingDate)
                    .ThenBy(s => s.StartTime)
                    .ToList();
            }
            catch
            {
                return new List<Schedule>();
            }
        }

        public List<CheckIn> GetRecentCheckIns()
        {
            try
            {
                using var db = new AppDbContext();
                return db.CheckIns
                    .Include(c => c.PT)
                    .Where(c => c.MemberID == CurrentMemberId)
                    .OrderByDescending(c => c.CheckInTime)
                    .Take(15)
                    .ToList();
            }
            catch
            {
                return new List<CheckIn>();
            }
        }

        public List<Progress> GetProgressHistory()
        {
            try
            {
                using var db = new AppDbContext();
                return db.Progresses
                    .Include(p => p.PT)
                    .Where(p => p.MemberID == CurrentMemberId)
                    .OrderByDescending(p => p.RecordDate)
                    .ToList();
            }
            catch
            {
                return new List<Progress>();
            }
        }

        public bool SaveCheckIn(int? ptId, string note, string? imagePath)
        {
            try
            {
                using var db = new AppDbContext();
                var checkIn = new CheckIn
                {
                    MemberID = CurrentMemberId,
                    PTID = ptId,
                    CheckInTime = DateTime.Now,
                    Status = "Pending",
                    Note = note,
                    BranchID = 1
                };
                db.CheckIns.Add(checkIn);
                db.SaveChanges();

                if (!string.IsNullOrEmpty(imagePath))
                {
                    var img = new CheckInImage
                    {
                        CheckInID = checkIn.CheckInID,
                        ImagePath = imagePath
                    };
                    db.CheckInImages.Add(img);
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool BookSchedule(DateTime date, TimeSpan start, TimeSpan end, string type, int ptId, int roomId, string note)
        {
            try
            {
                using var db = new AppDbContext();
                var schedule = new Schedule
                {
                    MemberID = CurrentMemberId,
                    PTID = ptId,
                    RoomID = roomId,
                    TrainingDate = date,
                    StartTime = start,
                    EndTime = end,
                    TrainingType = type,
                    Status = "Chờ duyệt",
                    Note = note,
                    BranchID = 1
                };
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SaveProgress(decimal weight, decimal height, decimal bodyFat, decimal muscle, decimal chest, decimal waist, decimal hip, decimal arm, decimal thigh, string note, int ptId = 1)
        {
            try
            {
                using var db = new AppDbContext();
                decimal bmi = height > 0 ? Math.Round(weight / ((height / 100) * (height / 100)), 1) : 0;
                var progress = new Progress
                {
                    MemberID = CurrentMemberId,
                    PTID = ptId,
                    RecordDate = DateTime.Now,
                    Height = height,
                    Weight = weight,
                    BodyFat = bodyFat,
                    MuscleMass = muscle,
                    BMI = bmi,
                    Chest = chest,
                    Waist = waist,
                    Hip = hip,
                    Arm = arm,
                    Thigh = thigh,
                    Note = note
                };
                db.Progresses.Add(progress);

                // Also update current member record
                var member = db.Members.Find(CurrentMemberId);
                if (member != null)
                {
                    member.Height = height;
                    member.Weight = weight;
                    member.BodyFat = bodyFat;
                    member.MuscleMass = muscle;
                    member.BMI = bmi;
                }

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateProfile(string fullName, string gender, DateTime dob, string phone, string email, string address)
        {
            try
            {
                using var db = new AppDbContext();
                var member = db.Members.Find(CurrentMemberId);
                if (member != null)
                {
                    member.FullName = fullName;
                    member.Gender = gender;
                    member.DateOfBirth = dob;
                    member.Phone = phone;
                    member.Email = email;
                    member.Address = address;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
