using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using QuanLyGym.DAL;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.BLL.Services
{
    public record RegistrationBranch(int Id, string Name);

    public record MemberRegistration(string FullName, string Username, string Email, string Phone,
        DateTime DateOfBirth, string Gender, int BranchId, string Password, string ConfirmPassword);

    public class MemberRegistrationService
    {
        public List<RegistrationBranch> GetBranches()
        {
            using var db = new AppDbContext();
            return db.Branches.Where(branch => branch.Status)
                .OrderBy(branch => branch.BranchName)
                .Select(branch => new RegistrationBranch(branch.BranchID, branch.BranchName))
                .ToList();
        }

        public string? Register(MemberRegistration registration)
        {
            string fullName = registration.FullName.Trim();
            string username = registration.Username.Trim();
            string email = registration.Email.Trim().ToLowerInvariant();
            string phone = registration.Phone.Trim();

            if (fullName.Length is < 1 or > 100)
                return "Vui lòng nhập họ và tên (tối đa 100 ký tự).";
            if (!Regex.IsMatch(username, @"\A[A-Za-z0-9_]{3,50}\z"))
                return "Tên đăng nhập cần từ 3–50 ký tự, gồm chữ không dấu, số hoặc dấu gạch dưới.";
            if (email.Length > 100 || !MailAddress.TryCreate(email, out var address) || address.Address != email)
                return "Vui lòng nhập email hợp lệ.";
            if (!Regex.IsMatch(phone, @"\A\+?[0-9]{9,14}\z"))
                return "Số điện thoại cần từ 9–14 chữ số và có thể bắt đầu bằng dấu +.";
            if (registration.DateOfBirth.Date >= DateTime.Today)
                return "Ngày sinh phải trước ngày hôm nay.";
            if (registration.Gender is not ("Nam" or "Nữ" or "Khác"))
                return "Vui lòng chọn giới tính.";
            if (registration.Password.Length is < 8 or > 128 || string.IsNullOrWhiteSpace(registration.Password))
                return "Mật khẩu cần từ 8–128 ký tự.";
            if (registration.Password != registration.ConfirmPassword)
                return "Mật khẩu xác nhận không khớp.";

            string passwordHash = PasswordHasher.Hash(registration.Password);
            string usernameKey = username.ToLowerInvariant();

            using var db = new AppDbContext();
            using var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);

            if (db.Accounts.Any(account => account.Username.ToLower() == usernameKey))
                return "Tên đăng nhập đã được sử dụng.";
            if (db.Accounts.Any(account => account.Email != null && account.Email.ToLower() == email))
                return "Email đã được sử dụng.";
            if (!db.Branches.Any(branch => branch.BranchID == registration.BranchId && branch.Status))
                return "Vui lòng chọn chi nhánh đang hoạt động.";

            db.Members.Add(new Member
            {
                Account = new Account
                {
                    Username = username,
                    Email = email,
                    PasswordHash = passwordHash,
                    Role = "Member",
                    Status = true
                },
                FullName = fullName,
                Email = email,
                Phone = phone,
                DateOfBirth = registration.DateOfBirth.Date,
                Gender = registration.Gender,
                BranchID = registration.BranchId
            });

            db.SaveChanges();
            transaction.Commit();
            return null;
        }
    }
}
