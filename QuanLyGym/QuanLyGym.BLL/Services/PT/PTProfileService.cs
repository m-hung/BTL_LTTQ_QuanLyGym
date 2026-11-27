using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using QuanLyGym.BLL.DTOs.PT;
using QuanLyGym.BLL.Models;

namespace QuanLyGym.BLL.Services.PT
{
    /// <summary>
    /// Xử lý nghiệp vụ Thông tin cá nhân của PT:
    /// tải hồ sơ, kiểm tra dữ liệu nhập, kiểm tra định dạng/magic-bytes ảnh đại diện, lưu trữ và xóa ảnh.
    /// </summary>
    public class PTProfileService
    {
        public const long MaxAvatarBytes = 5 * 1024 * 1024; // 5 MB

        private readonly PTDataStore _store = PTDataStore.Instance;

        public event Action? ProfileUpdated;

        public int GetPTIdByAccountId(int accountId)
        {
            if (accountId <= 0 || !_store.IsDatabaseAvailable())
                return 1;

            try
            {
                using var db = _store.CreateDbContext();
                int ptId = db.PTs.AsNoTracking()
                    .Where(p => p.AccountID == accountId)
                    .Select(p => p.PTID)
                    .FirstOrDefault();
                return ptId > 0 ? ptId : 1;
            }
            catch
            {
                return 1;
            }
        }

        public PTProfileDto GetProfile(int ptId = 1)
        {
            return _store.ExecuteLocked(() =>
            {
                PTProfileDto dto;

                if (_store.IsDatabaseAvailable())
                {
                    try
                    {
                        using var db = _store.CreateDbContext();
                        var pt = db.PTs.Include(p => p.Branch).AsNoTracking().FirstOrDefault(p => p.PTID == ptId);
                        if (pt != null)
                        {
                            var extras = _store.LoadExtras(ptId);
                            var schedules = db.Schedules.AsNoTracking().Where(s => s.PTID == ptId);
                            var today = DateTime.Today;
                            string? email = string.IsNullOrWhiteSpace(pt.Email)
                                ? db.Accounts.AsNoTracking().Where(a => a.AccountID == pt.AccountID).Select(a => a.Email).FirstOrDefault()
                                : pt.Email;

                            dto = new PTProfileDto
                            {
                                PTID = pt.PTID,
                                AccountID = pt.AccountID,
                                FullName = pt.FullName,
                                Email = email ?? string.Empty,
                                Phone = pt.Phone,
                                Gender = string.IsNullOrWhiteSpace(pt.Gender) ? "Nam" : pt.Gender,
                                DateOfBirth = pt.DateOfBirth == default ? new DateTime(1995, 5, 20) : pt.DateOfBirth,
                                Specialization = pt.Specialization,
                                Experience = pt.Experience,
                                Bio = extras.Bio,
                                AvatarPath = extras.AvatarPath,
                                BranchName = pt.Branch?.BranchName ?? "Titan Gym - Cơ sở chính",
                                Status = pt.Status,
                                MemberCount = schedules.Select(s => s.MemberID).Distinct().Count(),
                                CompletedSessions = schedules.Count(s => s.Status == "Completed"),
                                UpcomingSessions = schedules.Count(s => s.Status == "Approved" && s.TrainingDate >= today)
                            };
                            return dto;
                        }
                    }
                    catch { }
                }

                dto = _store.InMemoryProfile.Clone();
                var stats = ComputeInMemoryStats(ptId);
                dto.MemberCount = stats.MemberCount;
                dto.CompletedSessions = stats.CompletedSessions;
                dto.UpcomingSessions = stats.UpcomingSessions;
                return dto;
            });
        }

        public ServiceResult<string?> ValidateAvatarFile(string? filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return ServiceResult<string?>.Ok(null);

            if (!File.Exists(filePath))
                return ServiceResult<string?>.Fail("Không tìm thấy tệp ảnh đã chọn.");

            var info = new FileInfo(filePath);
            if (info.Length == 0)
                return ServiceResult<string?>.Fail("Tệp ảnh rỗng, vui lòng chọn ảnh hợp lệ.");

            if (info.Length > MaxAvatarBytes)
            {
                double mb = info.Length / 1048576.0;
                return ServiceResult<string?>.Fail($"Ảnh này nặng {mb:0.0} MB, tối đa 5 MB. Hãy chọn ảnh nhỏ hơn.");
            }

            string? ext = DetectImageExtension(filePath);
            if (ext == null)
                return ServiceResult<string?>.Fail("Chỉ nhận ảnh JPG, PNG hoặc WEBP hợp lệ.");

            return ServiceResult<string?>.Ok(ext);
        }

        public ServiceResult<PTProfileDto> UpdateProfile(PTProfileDto input, string? newAvatarSourcePath = null, bool removeAvatar = false)
        {
            var errors = new Dictionary<string, string>();

            string fullName = (input.FullName ?? string.Empty).Trim();
            string phone = (input.Phone ?? string.Empty).Trim();
            string spec = (input.Specialization ?? string.Empty).Trim();
            string bio = (input.Bio ?? string.Empty).Trim();
            string gender = string.IsNullOrWhiteSpace(input.Gender) ? "Nam" : input.Gender.Trim();

            if (string.IsNullOrWhiteSpace(fullName))
                errors["FullName"] = "Vui lòng nhập họ và tên.";
            else if (fullName.Length > 100)
                errors["FullName"] = "Họ và tên tối đa 100 ký tự.";

            if (!string.IsNullOrWhiteSpace(phone))
            {
                string digitsOnly = Regex.Replace(phone, @"[\s\.\-\(\)]", "");
                if (phone.Length > 15)
                    errors["Phone"] = "Số điện thoại tối đa 15 ký tự.";
                else if (!Regex.IsMatch(digitsOnly, @"^(0|\+84)\d{8,11}$"))
                    errors["Phone"] = "Số điện thoại không hợp lệ (ví dụ: 0988123456).";
            }

            if (string.IsNullOrWhiteSpace(spec))
                errors["Specialization"] = "Vui lòng nhập chuyên môn huấn luyện.";
            else if (spec.Length > 200)
                errors["Specialization"] = "Chuyên môn tối đa 200 ký tự.";

            if (input.Experience < 0 || input.Experience > 50)
                errors["Experience"] = "Số năm kinh nghiệm phải từ 0 đến 50.";

            if (bio.Length > 1000)
                errors["Bio"] = "Giới thiệu bản thân tối đa 1000 ký tự.";

            if (input.DateOfBirth > DateTime.Today.AddYears(-16) || input.DateOfBirth < new DateTime(1940, 1, 1))
                errors["DateOfBirth"] = "Ngày sinh không hợp lệ (PT phải đủ 16 tuổi trở lên).";

            string? newAvatarExt = null;
            if (!string.IsNullOrWhiteSpace(newAvatarSourcePath))
            {
                var avatarCheck = ValidateAvatarFile(newAvatarSourcePath);
                if (!avatarCheck.Success)
                {
                    errors["Avatar"] = avatarCheck.Message;
                }
                else
                {
                    newAvatarExt = avatarCheck.Data;
                }
            }

            if (errors.Count > 0)
            {
                string summary = errors.Values.First();
                return ServiceResult<PTProfileDto>.Fail(summary, errors);
            }

            PTProfileDto updated = _store.ExecuteLocked(() =>
            {
                int ptId = input.PTID > 0 ? input.PTID : 1;
                var extras = _store.LoadExtras(ptId);
                extras.Bio = bio;

                if (removeAvatar)
                {
                    DeleteUploadedAvatars(ptId);
                    extras.AvatarPath = null;
                }
                else if (newAvatarExt != null && !string.IsNullOrWhiteSpace(newAvatarSourcePath))
                {
                    extras.AvatarPath = SaveAvatarFile(ptId, newAvatarSourcePath, newAvatarExt);
                }

                _store.SaveExtras(ptId, extras);

                if (_store.IsDatabaseAvailable())
                {
                    try
                    {
                        using var db = _store.CreateDbContext();
                        var pt = db.PTs.FirstOrDefault(p => p.PTID == ptId);
                        if (pt != null)
                        {
                            pt.FullName = fullName;
                            pt.Phone = phone;
                            pt.Gender = gender;
                            pt.DateOfBirth = input.DateOfBirth.Date;
                            pt.Specialization = spec;
                            pt.Experience = input.Experience;
                            db.SaveChanges();
                        }
                    }
                    catch { }
                }

                _store.InMemoryProfile.FullName = fullName;
                _store.InMemoryProfile.Phone = phone;
                _store.InMemoryProfile.Gender = gender;
                _store.InMemoryProfile.DateOfBirth = input.DateOfBirth.Date;
                _store.InMemoryProfile.Specialization = spec;
                _store.InMemoryProfile.Experience = input.Experience;
                _store.InMemoryProfile.Bio = bio;
                _store.InMemoryProfile.AvatarPath = extras.AvatarPath;

                return _store.InMemoryProfile.Clone();
            });

            FillProfileStats(updated);

            _store.NotifyChanged();
            ProfileUpdated?.Invoke();

            return ServiceResult<PTProfileDto>.Ok(updated, "Đã lưu thông tin cá nhân. Học viên sẽ thấy hồ sơ mới của bạn.");
        }

        public ServiceResult<PTProfileDto> DeleteAvatar(int ptId = 1)
        {
            PTProfileDto updated = _store.ExecuteLocked(() =>
            {
                DeleteUploadedAvatars(ptId);
                var extras = _store.LoadExtras(ptId);
                extras.AvatarPath = null;
                _store.SaveExtras(ptId, extras);
                _store.InMemoryProfile.AvatarPath = null;
                return _store.InMemoryProfile.Clone();
            });

            FillProfileStats(updated);

            _store.NotifyChanged();
            ProfileUpdated?.Invoke();

            return ServiceResult<PTProfileDto>.Ok(updated, "Đã xoá ảnh đại diện. Hồ sơ sẽ hiện chữ viết tắt tên của bạn.");
        }

        private void FillProfileStats(PTProfileDto dto)
        {
            int ptId = dto.PTID > 0 ? dto.PTID : 1;

            if (_store.IsDatabaseAvailable())
            {
                try
                {
                    using var db = _store.CreateDbContext();
                    var schedules = db.Schedules.AsNoTracking().Where(s => s.PTID == ptId);
                    var today = DateTime.Today;
                    dto.MemberCount = schedules.Select(s => s.MemberID).Distinct().Count();
                    dto.CompletedSessions = schedules.Count(s => s.Status == "Completed");
                    dto.UpcomingSessions = schedules.Count(s => s.Status == "Approved" && s.TrainingDate >= today);
                    return;
                }
                catch { }
            }

            var stats = ComputeInMemoryStats(ptId);
            dto.MemberCount = stats.MemberCount;
            dto.CompletedSessions = stats.CompletedSessions;
            dto.UpcomingSessions = stats.UpcomingSessions;
        }

        private (int MemberCount, int CompletedSessions, int UpcomingSessions) ComputeInMemoryStats(int ptId)
        {
            var schedules = _store.InMemorySchedules.Where(s => s.PTID == ptId).ToList();
            var today = DateTime.Today;

            int baseMembers = 6;
            int baseCompleted = 24;
            int dynamicMembers = schedules.Select(s => s.MemberID).Distinct().Count();
            int completed = baseCompleted + schedules.Count(s => s.Status == "Completed");
            int upcoming = schedules.Count(s => s.Status == "Approved" && s.BookingDate.Date >= today);

            return (Math.Max(baseMembers, dynamicMembers), completed, upcoming);
        }

        /// <summary>
        /// Nhận diện định dạng ảnh dựa trên magic bytes thực tế (JPG, PNG, WEBP).
        /// </summary>
        private static string? DetectImageExtension(string filePath)
        {
            try
            {
                byte[] header = new byte[12];
                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                int read = fs.Read(header, 0, header.Length);

                if (read >= 3 && header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF)
                    return ".jpg";
                if (read >= 8 && header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47)
                    return ".png";
                if (read >= 12 && header[0] == 0x52 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x46
                    && header[8] == 0x57 && header[9] == 0x45 && header[10] == 0x42 && header[11] == 0x50)
                    return ".webp";
            }
            catch { }

            return null;
        }

        private string SaveAvatarFile(int ptId, string sourceFilePath, string ext)
        {
            Directory.CreateDirectory(_store.AvatarDirectory);
            DeleteUploadedAvatars(ptId);

            string fileName = $"trainer_{ptId}{ext}";
            string destPath = Path.Combine(_store.AvatarDirectory, fileName);
            File.Copy(sourceFilePath, destPath, overwrite: true);
            return destPath;
        }

        private void DeleteUploadedAvatars(int ptId)
        {
            if (!Directory.Exists(_store.AvatarDirectory)) return;
            foreach (var file in Directory.GetFiles(_store.AvatarDirectory, $"trainer_{ptId}.*"))
            {
                try { File.Delete(file); } catch { }
            }
        }
    }
}
