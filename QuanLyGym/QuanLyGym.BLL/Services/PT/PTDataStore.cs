using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using QuanLyGym.BLL.DTOs.PT;
using QuanLyGym.DAL;

namespace QuanLyGym.BLL.Services.PT
{
    internal class PTExtrasMetadata
    {
        public string Bio { get; set; } = string.Empty;
        public string? AvatarPath { get; set; }
    }

    /// <summary>
    /// Kho dữ liệu dùng chung cho phân hệ PT:
    /// tự động kết nối SQL Server qua AppDbContext nếu khả dụng, hoặc fallback mượt mà sang dữ liệu nội bộ.
    /// </summary>
    internal sealed class PTDataStore
    {
        private static readonly Lazy<PTDataStore> _lazy = new Lazy<PTDataStore>(() => new PTDataStore());
        public static PTDataStore Instance => _lazy.Value;

        private readonly object _lock = new object();
        private readonly string _appDataDir;
        private readonly string _avatarDir;
        private bool? _isDbAvailable;

        public PTProfileDto InMemoryProfile { get; private set; }
        public List<BookingRequestDto> InMemorySchedules { get; private set; }

        public event Action? DataChanged;

        private PTDataStore()
        {
            _appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QuanLyGym");
            _avatarDir = Path.Combine(_appDataDir, "Avatars");
            Directory.CreateDirectory(_appDataDir);
            Directory.CreateDirectory(_avatarDir);

            InMemoryProfile = CreateDefaultProfile();
            InMemorySchedules = CreateDefaultSchedules();
            LoadExtrasFromDisk(InMemoryProfile);
        }

        public string AvatarDirectory => _avatarDir;

        public void NotifyChanged()
        {
            DataChanged?.Invoke();
        }

        public bool IsDatabaseAvailable()
        {
            if (_isDbAvailable.HasValue) return _isDbAvailable.Value;

            try
            {
                string connStr = AppDbContext.GetResolvedConnectionString();
                if (!connStr.Contains("Connect Timeout", StringComparison.OrdinalIgnoreCase))
                {
                    connStr = connStr.TrimEnd(';') + ";Connect Timeout=2;";
                }

                using var conn = new Microsoft.Data.SqlClient.SqlConnection(connStr);
                conn.Open();
                _isDbAvailable = true;
            }
            catch
            {
                _isDbAvailable = false;
            }

            return _isDbAvailable.Value;
        }

        public AppDbContext CreateDbContext()
        {
            return new AppDbContext();
        }

        public PTExtrasMetadata LoadExtras(int ptId)
        {
            string path = Path.Combine(_appDataDir, $"pt_extras_{ptId}.json");
            if (File.Exists(path))
            {
                try
                {
                    var json = File.ReadAllText(path);
                    var data = JsonSerializer.Deserialize<PTExtrasMetadata>(json);
                    if (data != null)
                    {
                        if (!string.IsNullOrWhiteSpace(data.AvatarPath) && !File.Exists(data.AvatarPath))
                        {
                            data.AvatarPath = null;
                        }
                        return data;
                    }
                }
                catch { }
            }

            // Tự động tìm file ảnh đã lưu theo quy ước trainer_{id}.*
            string? existingAvatar = FindSavedAvatar(ptId);
            return new PTExtrasMetadata
            {
                Bio = "5 năm kinh nghiệm huấn luyện cá nhân (PT 1-1), chứng chỉ NASM-CPT, chuyên lộ trình tăng cơ giảm mỡ bền vững và chuẩn kỹ thuật tập luyện.",
                AvatarPath = existingAvatar
            };
        }

        public void SaveExtras(int ptId, PTExtrasMetadata meta)
        {
            string path = Path.Combine(_appDataDir, $"pt_extras_{ptId}.json");
            try
            {
                var json = JsonSerializer.Serialize(meta, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
            catch { }
        }

        public string? FindSavedAvatar(int ptId)
        {
            if (!Directory.Exists(_avatarDir)) return null;
            return Directory.GetFiles(_avatarDir, $"trainer_{ptId}.*").FirstOrDefault();
        }

        private void LoadExtrasFromDisk(PTProfileDto profile)
        {
            var meta = LoadExtras(profile.PTID);
            if (!string.IsNullOrWhiteSpace(meta.Bio))
            {
                profile.Bio = meta.Bio;
            }
            if (!string.IsNullOrWhiteSpace(meta.AvatarPath) && File.Exists(meta.AvatarPath))
            {
                profile.AvatarPath = meta.AvatarPath;
            }
        }

        private static PTProfileDto CreateDefaultProfile()
        {
            return new PTProfileDto
            {
                PTID = 1,
                AccountID = 2,
                FullName = "Tuấn Minh",
                Email = "tuanminh@titangym.vn",
                Phone = "0988.888.888",
                Gender = "Nam",
                DateOfBirth = new DateTime(1995, 5, 20),
                Specialization = "Tăng cơ, Giảm mỡ, Hypertrophy",
                Experience = 5,
                Bio = "5 năm kinh nghiệm huấn luyện cá nhân (PT 1-1), chứng chỉ NASM-CPT, chuyên lộ trình tăng cơ giảm mỡ bền vững và chuẩn kỹ thuật tập luyện.",
                BranchName = "Titan Gym - Cơ sở Cầu Giấy",
                Status = true,
                MemberCount = 6,
                CompletedSessions = 24,
                UpcomingSessions = 4
            };
        }

        private static List<BookingRequestDto> CreateDefaultSchedules()
        {
            var today = DateTime.Today;
            return new List<BookingRequestDto>
            {
                // Các yêu cầu chờ duyệt (Pending)
                new BookingRequestDto
                {
                    ScheduleID = 101,
                    PTID = 1,
                    MemberID = 1,
                    MemberCode = "HV001",
                    MemberName = "Nguyễn Đức An",
                    Phone = "0912.345.678",
                    Email = "ducan@gmail.com",
                    PackageName = "Tăng Cơ Hypertrophy",
                    RoomName = "Zone A - Khu Tạ Tự Do",
                    BookingDate = today.AddDays(1),
                    StartTime = new TimeSpan(8, 30, 0),
                    EndTime = new TimeSpan(10, 0, 0),
                    TrainingType = "Huấn luyện 1-1",
                    Status = "Pending",
                    Notes = "Tập ngực & tay sau, test mức tạ Bench Press mới.",
                    ColorIndex = 0
                },
                new BookingRequestDto
                {
                    ScheduleID = 102,
                    PTID = 1,
                    MemberID = 2,
                    MemberCode = "HV002",
                    MemberName = "Phạm Mai Linh",
                    Phone = "0988.765.432",
                    Email = "mailinh@gmail.com",
                    PackageName = "Body Transformation Siết Mỡ",
                    RoomName = "Zone B - Cardio & HIIT",
                    BookingDate = today.AddDays(2),
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(10, 30, 0),
                    TrainingType = "Huấn luyện 1-1",
                    Status = "Pending",
                    Notes = "Kiểm tra lại chỉ số InBody và cardio kháng lực cường độ cao.",
                    ColorIndex = 1
                },
                new BookingRequestDto
                {
                    ScheduleID = 103,
                    PTID = 1,
                    MemberID = 3,
                    MemberCode = "HV003",
                    MemberName = "Lê Hoàng Nam",
                    Phone = "0903.112.233",
                    Email = "hoangnam@gmail.com",
                    PackageName = "Sức Bền & Thể Lực",
                    RoomName = "Zone C - Functional",
                    BookingDate = today.AddDays(2),
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(10, 30, 0),
                    TrainingType = "Huấn luyện 1-1",
                    Status = "Pending",
                    Notes = "Luyện bài tập chân và nhóm cơ core hỗ trợ chạy bán marathon.",
                    ColorIndex = 2
                },
                new BookingRequestDto
                {
                    ScheduleID = 104,
                    PTID = 1,
                    MemberID = 5,
                    MemberCode = "HV005",
                    MemberName = "Trần Quốc Huy",
                    Phone = "0934.556.677",
                    Email = "quochuy@gmail.com",
                    PackageName = "Giảm Cân Cấp Tốc HIIT",
                    RoomName = "Zone B - Cardio & HIIT",
                    BookingDate = today.AddDays(-1),
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(15, 30, 0),
                    TrainingType = "Huấn luyện 1-1",
                    Status = "Pending",
                    Notes = "",
                    ColorIndex = 3
                },

                // Các lịch đã duyệt sẵn để kiểm tra thống kê & chống trùng lịch
                new BookingRequestDto
                {
                    ScheduleID = 201,
                    PTID = 1,
                    MemberID = 4,
                    MemberCode = "HV004",
                    MemberName = "Đỗ Bảo Trâm",
                    Phone = "0977.889.900",
                    Email = "baotram@gmail.com",
                    PackageName = "Tập Nữ Cơ Bản 1-1",
                    RoomName = "Zone A - Khu Tạ Tự Do",
                    BookingDate = today,
                    StartTime = new TimeSpan(17, 0, 0),
                    EndTime = new TimeSpan(18, 30, 0),
                    TrainingType = "Tập Lưng & Vai",
                    Status = "Approved",
                    Notes = "Cải thiện tư thế lưng gù.",
                    ColorIndex = 4
                },
                new BookingRequestDto
                {
                    ScheduleID = 202,
                    PTID = 1,
                    MemberID = 6,
                    MemberCode = "HV006",
                    MemberName = "Vũ Thị Hương",
                    Phone = "0965.443.322",
                    Email = "thihuong@gmail.com",
                    PackageName = "Dãn Cơ & Phục Hồi",
                    RoomName = "Zone C - Functional",
                    BookingDate = today.AddDays(3),
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(15, 30, 0),
                    TrainingType = "Phục hồi cột sống",
                    Status = "Approved",
                    Notes = "Giảm đau mỏi vùng thắt lưng L4-L5.",
                    ColorIndex = 5
                }
            };
        }

        public T ExecuteLocked<T>(Func<T> action)
        {
            lock (_lock)
            {
                return action();
            }
        }
    }
}
