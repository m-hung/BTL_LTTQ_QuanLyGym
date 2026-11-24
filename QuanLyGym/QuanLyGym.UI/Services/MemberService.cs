using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyGym.DAL;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.UI.Services
{
    public class CheckInMonthlyStats
    {
        public int TotalCount { get; set; }
        public int ApprovedCount { get; set; }
        public int PendingCount { get; set; }
        public int DistinctDaysCount { get; set; }
        public int StreakDays { get; set; }
    }

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
            try
            {
                using var db = new AppDbContext();
                var memberId = db.Members
                    .Where(m => m.AccountID == accountId)
                    .Select(m => (int?)m.MemberID)
                    .FirstOrDefault();

                CurrentMemberId = memberId ?? 1;
                return memberId.HasValue;
            }
            catch
            {
                CurrentMemberId = 1;
                return false;
            }
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
                EnsurePackageBenefitsUpdated();
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

        public bool IsClassicMember()
        {
            try
            {
                var membership = GetActiveMembership();
                if (membership?.Package != null)
                {
                    return !membership.Package.IsPTPackage || membership.Package.TotalPTSessions == 0;
                }
                return true;
            }
            catch
            {
                return true;
            }
        }

        public List<Membership> GetMembershipHistory()
        {
            try
            {
                using var db = new AppDbContext();
                return db.Memberships
                    .Include(m => m.Package)
                    .Include(m => m.Branch)
                    .Where(m => m.MemberID == CurrentMemberId)
                    .OrderByDescending(m => m.RegisterDate)
                    .ToList();
            }
            catch
            {
                return new List<Membership>();
            }
        }

        /// <summary>
        /// Lấy lịch tập sắp tới với HLV (Bảng Schedules dành riêng cho tập cùng PT).
        /// </summary>
        public List<Schedule> GetUpcomingSchedules()
        {
            try
            {
                using var db = new AppDbContext();
                return db.Schedules
                    .Include(s => s.PT)
                    .Include(s => s.Room)
                    .Include(s => s.Branch)
                    .Where(s => s.MemberID == CurrentMemberId && s.Status != "Cancelled")
                    .OrderBy(s => s.TrainingDate)
                    .ThenBy(s => s.StartTime)
                    .ToList();
            }
            catch
            {
                return new List<Schedule>();
            }
        }

        /// <summary>
        /// Lấy toàn bộ lịch tập (cả đã tập, đã xác nhận, chờ duyệt, đã hủy) từ Schedules.
        /// </summary>
        public List<Schedule> GetAllSchedules()
        {
            try
            {
                using var db = new AppDbContext();
                return db.Schedules
                    .Include(s => s.PT)
                    .Include(s => s.Room)
                    .Include(s => s.Branch)
                    .Where(s => s.MemberID == CurrentMemberId)
                    .OrderByDescending(s => s.TrainingDate)
                    .ThenByDescending(s => s.StartTime)
                    .ToList();
            }
            catch
            {
                return new List<Schedule>();
            }
        }

        /// <summary>
        /// Lấy buổi tập kế tiếp gần nhất với HLV từ bảng Schedules.
        /// </summary>
        public Schedule? GetNextSchedule()
        {
            try
            {
                using var db = new AppDbContext();
                return db.Schedules
                    .Include(s => s.PT)
                    .Include(s => s.Room)
                    .Include(s => s.Branch)
                    .Where(s => s.MemberID == CurrentMemberId && s.TrainingDate >= DateTime.Today && s.Status != "Cancelled")
                    .OrderBy(s => s.TrainingDate)
                    .ThenBy(s => s.StartTime)
                    .FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy toàn bộ nhật ký điểm danh vào cửa (Bảng CheckIns - Cả tập tự do và tập có PT).
        /// </summary>
        public List<CheckIn> GetRecentCheckIns()
        {
            try
            {
                using var db = new AppDbContext();
                return db.CheckIns
                    .Include(c => c.PT)
                    .Include(c => c.Branch)
                    .Where(c => c.MemberID == CurrentMemberId)
                    .OrderByDescending(c => c.CheckInTime)
                    .ToList();
            }
            catch
            {
                return new List<CheckIn>();
            }
        }

        /// <summary>
        /// Lấy lượt check-in gần nhất của hội viên từ bảng CheckIns.
        /// </summary>
        public CheckIn? GetLatestCheckIn()
        {
            try
            {
                using var db = new AppDbContext();
                return db.CheckIns
                    .Include(c => c.Branch)
                    .Include(c => c.PT)
                    .Where(c => c.MemberID == CurrentMemberId)
                    .OrderByDescending(c => c.CheckInTime)
                    .FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách điểm danh theo tháng chỉ định từ bảng CheckIns.
        /// </summary>
        public List<CheckIn> GetMonthlyCheckIns(int year, int month)
        {
            try
            {
                using var db = new AppDbContext();
                return db.CheckIns
                    .Include(c => c.Branch)
                    .Include(c => c.PT)
                    .Where(c => c.MemberID == CurrentMemberId && c.CheckInTime.Year == year && c.CheckInTime.Month == month)
                    .OrderByDescending(c => c.CheckInTime)
                    .ToList();
            }
            catch
            {
                return new List<CheckIn>();
            }
        }

        /// <summary>
        /// Thống kê điểm danh trong tháng (tổng số lần, đã duyệt, chờ duyệt, số ngày đã tập, chuỗi ngày liên tiếp).
        /// </summary>
        public CheckInMonthlyStats GetMonthlyCheckInStats(int year, int month)
        {
            try
            {
                var list = GetMonthlyCheckIns(year, month);
                var distinctDates = list.Select(c => c.CheckInTime.Date).Distinct().OrderByDescending(d => d).ToList();

                int streak = 0;
                DateTime checkDate = DateTime.Today;
                if (!distinctDates.Contains(checkDate))
                {
                    checkDate = checkDate.AddDays(-1);
                }
                while (distinctDates.Contains(checkDate))
                {
                    streak++;
                    checkDate = checkDate.AddDays(-1);
                }

                return new CheckInMonthlyStats
                {
                    TotalCount = list.Count,
                    ApprovedCount = list.Count(c => c.Status == "Approved"),
                    PendingCount = list.Count(c => c.Status == "Pending"),
                    DistinctDaysCount = distinctDates.Count,
                    StreakDays = streak
                };
            }
            catch
            {
                return new CheckInMonthlyStats();
            }
        }

        /// <summary>
        /// Lấy lần đo chỉ số InBody mới nhất.
        /// </summary>
        public Progress? GetLatestProgress()
        {
            try
            {
                using var db = new AppDbContext();
                return db.Progresses
                    .Include(p => p.PT)
                    .Where(p => p.MemberID == CurrentMemberId)
                    .OrderByDescending(p => p.RecordDate)
                    .ThenByDescending(p => p.ProgressID)
                    .FirstOrDefault();
            }
            catch
            {
                return null;
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
                    .ThenByDescending(p => p.ProgressID)
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
                var curMember = db.Members.Find(CurrentMemberId);
                int branchId = curMember?.BranchID ?? 1;

                var checkIn = new CheckIn
                {
                    MemberID = CurrentMemberId,
                    PTID = ptId,
                    CheckInTime = DateTime.Now,
                    Status = ptId.HasValue ? "Pending" : "Approved", // Tự do thì tự động Approved, có PT kèm thì chờ PT duyệt
                    Note = note,
                    BranchID = branchId
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

        public bool BookSchedule(DateTime date, TimeSpan start, TimeSpan end, string type, int ptId, int branchId, string note)
        {
            try
            {
                using var db = new AppDbContext();
                // Tìm phòng mặc định cho chi nhánh
                var room = db.Rooms.FirstOrDefault(r => r.BranchID == branchId) ?? db.Rooms.FirstOrDefault();
                int roomId = room?.RoomID ?? 1;

                var schedule = new Schedule
                {
                    MemberID = CurrentMemberId,
                    PTID = ptId,
                    RoomID = roomId,
                    TrainingDate = date,
                    StartTime = start,
                    EndTime = end,
                    TrainingType = type,
                    Status = "Pending",
                    Note = note,
                    BranchID = branchId
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

        public bool CancelSchedule(int scheduleId, out string message)
        {
            try
            {
                using var db = new AppDbContext();
                var sched = db.Schedules.FirstOrDefault(s => s.ScheduleID == scheduleId && s.MemberID == CurrentMemberId);
                if (sched != null)
                {
                    sched.Status = "Cancelled";
                    db.SaveChanges();
                    message = "Đã hủy lịch tập thành công!";
                    return true;
                }
                message = "Không tìm thấy lịch tập cần hủy.";
                return false;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi hủy lịch: " + ex.Message;
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
                    PTID = ptId > 0 ? ptId : 1,
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

        public bool ChangePassword(string oldPass, string newPass, out string message)
        {
            try
            {
                using var db = new AppDbContext();
                var member = db.Members.Include(m => m.Account).FirstOrDefault(m => m.MemberID == CurrentMemberId);
                if (member?.Account == null)
                {
                    message = "Không tìm thấy tài khoản người dùng.";
                    return false;
                }
                if (member.Account.PasswordHash != oldPass)
                {
                    message = "Mật khẩu hiện tại không đúng!";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(newPass) || newPass.Length < 6)
                {
                    message = "Mật khẩu mới phải có ít nhất 6 ký tự!";
                    return false;
                }
                member.Account.PasswordHash = newPass;
                db.SaveChanges();
                message = "Đổi mật khẩu thành công!";
                return true;
            }
            catch (Exception ex)
            {
                message = "Lỗi khi đổi mật khẩu: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Truy vấn danh sách toàn bộ Chi nhánh đang hoạt động trong hệ thống.
        /// </summary>
        public List<Branch> GetAllBranches()
        {
            try
            {
                using var db = new AppDbContext();
                return db.Branches
                    .Where(b => b.Status)
                    .OrderBy(b => b.BranchID)
                    .ToList();
            }
            catch
            {
                return new List<Branch>();
            }
        }

        /// <summary>
        /// Truy vấn danh sách toàn bộ Huấn luyện viên (PT) đang hoạt động trong hệ thống.
        /// </summary>
        public List<PT> GetAllPTs()
        {
            try
            {
                using var db = new AppDbContext();
                var list = db.PTs
                    .Include(p => p.Branch)
                    .Where(p => p.Status)
                    .OrderBy(p => p.BranchID)
                    .ThenBy(p => p.FullName)
                    .ToList();

                if (list != null && list.Count > 0)
                    return list;
            }
            catch
            {
            }

            return GetDefaultPTs();
        }

        private List<PT> GetDefaultPTs()
        {
            return new List<PT>
            {
                new PT { PTID = 1, FullName = "Nguyễn Văn Nam", Gender = "Nam", DateOfBirth = new DateTime(1995, 5, 20), Phone = "0988.888.888", Email = "nam.nguyen@quanlygym.vn", Specialization = "Tăng cơ Hypertrophy & Dinh dưỡng", Experience = 6, Status = true, Branch = new Branch { BranchID = 1, BranchName = "Chi nhánh Cầu Giấy" } },
                new PT { PTID = 2, FullName = "Trần Thị Bích", Gender = "Nữ", DateOfBirth = new DateTime(1997, 8, 14), Phone = "0943.210.987", Email = "bich.tran@quanlygym.vn", Specialization = "Giảm mỡ Siết cơ & Pilates/Yoga", Experience = 5, Status = true, Branch = new Branch { BranchID = 1, BranchName = "Chi nhánh Cầu Giấy" } },
                new PT { PTID = 3, FullName = "Nguyễn Đức Minh", Gender = "Nam", DateOfBirth = new DateTime(1994, 3, 25), Phone = "0987.543.210", Email = "minh.duc@quanlygym.vn", Specialization = "Powerlifting & Thể hình chuyên nghiệp", Experience = 7, Status = true, Branch = new Branch { BranchID = 2, BranchName = "Chi nhánh Đống Đa" } },
                new PT { PTID = 4, FullName = "Phan Thị Thảo", Gender = "Nữ", DateOfBirth = new DateTime(1998, 11, 12), Phone = "0921.098.765", Email = "thao.phan@quanlygym.vn", Specialization = "Cardio HIIT & Định hình vóc dáng", Experience = 4, Status = true, Branch = new Branch { BranchID = 2, BranchName = "Chi nhánh Đống Đa" } },
                new PT { PTID = 5, FullName = "Vũ Hoàng Long", Gender = "Nam", DateOfBirth = new DateTime(1996, 9, 8), Phone = "0909.876.543", Email = "long.vu@quanlygym.vn", Specialization = "Phục hồi chức năng & Vận động", Experience = 5, Status = true, Branch = new Branch { BranchID = 3, BranchName = "Chi nhánh Quận 7" } }
            };
        }

        private static bool _benefitsChecked = false;

        /// <summary>
        /// Đảm bảo cột Benefits trong database luôn có đầy đủ các quyền lợi phong phú và quyền lợi chung.
        /// </summary>
        public void EnsurePackageBenefitsUpdated()
        {
            if (_benefitsChecked) return;
            try
            {
                using var db = new AppDbContext();
                bool modified = false;

                var p1 = db.Packages.Find(1);
                if (p1 != null && (!p1.Benefits.Contains("không giới hạn khung giờ toàn hệ thống") || !p1.Benefits.Contains("InBody")))
                {
                    p1.Benefits = "Tập luyện không giới hạn khung giờ toàn hệ thống, Sử dụng đầy đủ hệ thống máy tạ & Cardio, Đo & phân tích chỉ số InBody định kỳ, Trang bị Smart Locker tủ đồ cá nhân, Miễn phí phòng tắm nóng lạnh & tiện ích, Quẹt thẻ thông minh vào cửa tự động";
                    modified = true;
                }

                var p2 = db.Packages.Find(2);
                if (p2 != null && (!p2.Benefits.Contains("Huấn luyện viên") || !p2.Benefits.Contains("Luyện tập tự do")))
                {
                    p2.Benefits = "12 buổi tập 1-1 với Huấn luyện viên, Thiết lập lộ trình cá nhân hóa, Kiểm tra chỉ số InBody hàng tháng, Luyện tập tự do toàn hệ thống Titan Fitness, Miễn phí tủ đồ Smart Locker & nước uống";
                    modified = true;
                }

                var p3 = db.Packages.Find(3);
                if (p3 != null && (!p3.Benefits.Contains("Huấn luyện viên") || !p3.Benefits.Contains("Luyện tập tự do")))
                {
                    p3.Benefits = "24 buổi tập 1-1 với Huấn luyện viên, Tư vấn dinh dưỡng chuyên sâu, Đo InBody định kỳ & theo sát vóc dáng, Sử dụng phòng xông hơi Sauna & Smart Locker riêng, Luyện tập tự do toàn hệ thống Titan Fitness, Miễn phí nước uống mỗi buổi tập";
                    modified = true;
                }

                var p4 = db.Packages.Find(4);
                if (p4 != null && (!p4.Benefits.Contains("Bác sĩ dinh dưỡng") || !p4.Benefits.Contains("Luyện tập tự do")))
                {
                    p4.Benefits = "36 buổi tập 1-1 cùng Huấn luyện viên cao cấp, Bác sĩ dinh dưỡng & HLV theo sát 24/7, Full dịch vụ Sauna VIP & VIP Locker riêng biệt, Miễn phí khăn tập & nước uống mỗi buổi, Luyện tập tự do toàn hệ thống Titan Fitness, Cam kết cải thiện thể hình bằng văn bản";
                    modified = true;
                }

                if (modified)
                {
                    db.SaveChanges();
                }
                _benefitsChecked = true;
            }
            catch
            {
                // Bỏ qua nếu lỗi kết nối
            }
        }

        /// <summary>
        /// Truy vấn động danh sách các gói tập có HLV cá nhân (PT) đang hoạt động từ CSDL.
        /// </summary>
        public List<Package> GetPTPackages()
        {
            try
            {
                EnsurePackageBenefitsUpdated();
                using var db = new AppDbContext();
                var list = db.Packages
                    .Where(p => p.IsPTPackage && p.Status)
                    .OrderBy(p => p.Price)
                    .ToList();

                if (list != null && list.Count > 0)
                    return list;
            }
            catch
            {
            }

            return GetDefaultPTPackages();
        }

        /// <summary>
        /// Truy vấn động toàn bộ các gói tập đang mở (bao gồm cả Classic và gói PT).
        /// </summary>
        public List<Package> GetAllPackages()
        {
            try
            {
                EnsurePackageBenefitsUpdated();
                using var db = new AppDbContext();
                var list = db.Packages
                    .Where(p => p.Status)
                    .OrderBy(p => p.Price)
                    .ToList();

                if (list != null && list.Count > 0)
                    return list;
            }
            catch
            {
            }

            return GetDefaultAllPackages();
        }

        private List<Package> GetDefaultPTPackages()
        {
            return new List<Package>
            {
                new Package
                {
                    PackageID = 2,
                    PackageName = "Silver",
                    Duration = 90,
                    Price = 2800000,
                    TotalPTSessions = 12,
                    IsPTPackage = true,
                    Benefits = "12 buổi tập 1-1 với Huấn luyện viên, Thiết lập lộ trình cá nhân hóa, Kiểm tra chỉ số InBody hàng tháng, Luyện tập tự do toàn hệ thống Titan Fitness, Miễn phí tủ đồ Smart Locker & nước uống",
                    Description = "Gói rèn luyện chuyên sâu 3 tháng cùng HLV, phù hợp người mới bắt đầu cần định hướng.",
                    Status = true
                },
                new Package
                {
                    PackageID = 3,
                    PackageName = "Gold",
                    Duration = 90,
                    Price = 5800000,
                    TotalPTSessions = 24,
                    IsPTPackage = true,
                    Benefits = "24 buổi tập 1-1 với Huấn luyện viên, Tư vấn dinh dưỡng chuyên sâu, Đo InBody định kỳ & theo sát vóc dáng, Sử dụng phòng xông hơi Sauna & Smart Locker riêng, Luyện tập tự do toàn hệ thống Titan Fitness, Miễn phí nước uống mỗi buổi tập",
                    Description = "Gói nâng cao toàn diện giúp cải thiện thể hình nhanh chóng và thư giãn sau giờ tập.",
                    Status = true
                },
                new Package
                {
                    PackageID = 4,
                    PackageName = "VIP Diamond",
                    Duration = 90,
                    Price = 12500000,
                    TotalPTSessions = 36,
                    IsPTPackage = true,
                    Benefits = "36 buổi tập 1-1 cùng Huấn luyện viên cao cấp, Bác sĩ dinh dưỡng & HLV theo sát 24/7, Full dịch vụ Sauna VIP & VIP Locker riêng biệt, Miễn phí khăn tập & nước uống mỗi buổi, Luyện tập tự do toàn hệ thống Titan Fitness, Cam kết cải thiện thể hình bằng văn bản",
                    Description = "Gói tập thượng lưu cao cấp nhất, cam kết kết quả và trải nghiệm dịch vụ chuẩn 5 sao.",
                    Status = true
                }
            };
        }

        private List<Package> GetDefaultAllPackages()
        {
            var list = new List<Package>
            {
                new Package
                {
                    PackageID = 1,
                    PackageName = "Classic",
                    Duration = 30,
                    Price = 500000,
                    TotalPTSessions = 0,
                    IsPTPackage = false,
                    Benefits = "Tập luyện không giới hạn khung giờ toàn hệ thống, Sử dụng đầy đủ hệ thống máy tạ & Cardio, Đo & phân tích chỉ số InBody định kỳ, Trang bị Smart Locker tủ đồ cá nhân, Miễn phí phòng tắm nóng lạnh & tiện ích, Quẹt thẻ thông minh vào cửa tự động",
                    Description = "Gói tập tự do tiêu chuẩn dành cho hội viên muốn duy trì rèn luyện hàng ngày.",
                    Status = true
                }
            };
            list.AddRange(GetDefaultPTPackages());
            return list;
        }

        /// <summary>
        /// Tạo yêu cầu nâng cấp gói tập trên CSDL (ghi nhận vào bảng Memberships với trạng thái Pending).
        /// </summary>
        public bool RequestUpgradePackage(int packageId, out string message)
        {
            try
            {
                using var db = new AppDbContext();
                var pkg = db.Packages.Find(packageId);
                if (pkg == null)
                {
                    message = "Gói tập không tồn tại trong hệ thống.";
                    return false;
                }

                var member = db.Members.Find(CurrentMemberId);
                int branchId = member?.BranchID ?? 1;

                var newMembership = new Membership
                {
                    MemberID = CurrentMemberId,
                    PackageID = packageId,
                    PriceAtPurchase = pkg.Price,
                    Price = pkg.Price,
                    RegisterDate = DateTime.Now,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(pkg.Duration > 0 ? pkg.Duration : 90),
                    UsedPTSessions = 0,
                    Status = "Pending",
                    BranchID = branchId,
                    CreatedAt = DateTime.Now
                };

                db.Memberships.Add(newMembership);
                db.SaveChanges();

                message = $"Yêu cầu nâng cấp lên gói {pkg.PackageName} ({pkg.Price:N0} đ) đã được ghi nhận thành công trên hệ thống!\n\nLễ tân và Huấn luyện viên trưởng sẽ liên hệ với bạn trong vòng 15 phút.";
                return true;
            }
            catch (Exception ex)
            {
                message = "Đã xảy ra lỗi khi gửi yêu cầu nâng cấp: " + ex.Message;
                return false;
            }
        }
    }
}
