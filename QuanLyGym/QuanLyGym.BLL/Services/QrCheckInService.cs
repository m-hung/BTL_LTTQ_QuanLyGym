using Microsoft.EntityFrameworkCore;
using QuanLyGym.DAL;
using QuanLyGym.DAL.Entities;

namespace QuanLyGym.BLL.Services
{
    public sealed record QrCheckInBranch(int Id, string Name)
    {
        public override string ToString() => Name;
    }

    public sealed record QrCheckInVisit(string Name, string Code, string Package, DateTime Time, string Method);

    public enum QrCheckInStatus { Accepted, InvalidCode, InvalidAccount, InvalidBranch, InvalidMembership, WrongBranch, Duplicate }

    public sealed record QrCheckInResult(QrCheckInStatus Status, string Detail, QrCheckInVisit? Visit = null);

    public sealed class QrCheckInService
    {
        private const string Prefix = "GYM-";
        private const string CameraMethod = "Camera QR";
        private const string ManualMethod = "Nhập mã / USB";

        public static string FormatCode(Guid code) => $"{Prefix}{code:N}".ToUpperInvariant();

        public async Task<List<QrCheckInBranch>> GetBranchesAsync()
        {
            using var db = new AppDbContext();
            return await db.Branches.AsNoTracking().Where(branch => branch.Status)
                .OrderBy(branch => branch.BranchName)
                .Select(branch => new QrCheckInBranch(branch.BranchID, branch.BranchName)).ToListAsync();
        }

        public async Task<List<QrCheckInVisit>> GetTodayAsync(int branchId)
        {
            using var db = new AppDbContext();
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            var rows = await db.CheckIns.AsNoTracking()
                .Where(row => row.BranchID == branchId && row.CheckInTime >= today && row.CheckInTime < tomorrow &&
                    (row.Method == CameraMethod || row.Method == ManualMethod))
                .OrderByDescending(row => row.CheckInTime).ThenByDescending(row => row.CheckInID)
                .Select(row => new { row.Member!.FullName, row.Member.QrCardId, row.Note, row.CheckInTime, row.Method })
                .ToListAsync();
            return rows.Select(row => new QrCheckInVisit(row.FullName, FormatCode(row.QrCardId), row.Note,
                row.CheckInTime, row.Method!)).ToList();
        }

        public async Task<QrCheckInResult> CheckInAsync(int adminId, int branchId, string input, bool fromCamera)
        {
            string code = input.Trim();
            if (!code.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase) ||
                !Guid.TryParseExact(code[Prefix.Length..], "N", out Guid cardId) || cardId == Guid.Empty)
            {
                return new(QrCheckInStatus.InvalidCode, "Dùng mã QR hoặc mã thẻ trên thẻ tập của hội viên.");
            }

            using var db = new AppDbContext();
            var lookup = await (
                from admin in db.Accounts
                where admin.AccountID == adminId && admin.Role == "Admin" && admin.Status
                from branch in db.Branches.Where(item => item.BranchID == branchId && item.Status).DefaultIfEmpty()
                from member in db.Members.Where(item => item.QrCardId == cardId).DefaultIfEmpty()
                select new
                {
                    BranchName = branch == null ? null : branch.BranchName,
                    MemberId = member == null ? (int?)null : member.MemberID,
                    MemberName = member == null ? null : member.FullName,
                    MemberActive = member != null && member.Account != null && member.Account.Status
                }).SingleOrDefaultAsync();
            if (lookup == null)
                return new(QrCheckInStatus.InvalidAccount, "Tài khoản hiện tại không có quyền check-in tại quầy.");
            if (lookup.BranchName == null)
                return new(QrCheckInStatus.InvalidBranch, "Chọn một chi nhánh đang hoạt động.");
            if (lookup.MemberId is not int memberId)
                return new(QrCheckInStatus.InvalidCode, "Không tìm thấy thẻ hội viên.");
            if (!lookup.MemberActive)
                return new(QrCheckInStatus.InvalidAccount, $"{lookup.MemberName} · Tài khoản hội viên đã bị khóa.");


            await using var transaction = await db.Database.BeginTransactionAsync();
            string lockKey = $"QuanLyGym:QrCheckIn:{memberId}";
            await db.Database.ExecuteSqlInterpolatedAsync($@"
DECLARE @result int;
EXEC @result = sys.sp_getapplock @Resource = {lockKey}, @LockMode = 'Exclusive', @LockOwner = 'Transaction', @LockTimeout = 5000;
IF @result < 0 THROW 51001, 'Could not lock check-in.', 1;");

            DateTime now = DateTime.Now;
            DateTime today = now.Date;
            DateTime earliest = now.AddMinutes(-5);
            var membership = await db.Memberships
                .Where(item => item.MemberID == memberId && item.Status == "Active" &&
                    item.StartDate <= now && item.EndDate >= today)
                .Select(item => new
                {
                    item.MembershipID,
                    item.EndDate,
                    PackageName = item.Package!.PackageName,
                    AtBranch = item.Package != null && (item.Package.BranchID == null || item.Package.BranchID == branchId),
                    Previous = db.CheckIns.Where(visit => visit.MemberID == memberId && visit.Status == "Approved" &&
                        visit.PTID == null && visit.CheckInTime >= earliest && visit.CheckInTime <= now)
                        .Max(visit => (DateTime?)visit.CheckInTime)
                })
                .OrderByDescending(item => item.AtBranch)
                .ThenByDescending(item => item.EndDate).ThenByDescending(item => item.MembershipID)
                .FirstOrDefaultAsync();
            if (membership == null)
                return new(QrCheckInStatus.InvalidMembership, $"{lookup.MemberName} · Không có gói tập đang trong thời hạn.");
            if (!membership.AtBranch)
                return new(QrCheckInStatus.WrongBranch, $"{lookup.MemberName} · Gói tập không dùng tại {lookup.BranchName}.");
            if (membership.Previous is DateTime recent)
                return new(QrCheckInStatus.Duplicate, $"{lookup.MemberName} · Đã vào cửa lúc {recent:HH:mm:ss}. Vui lòng đợi đủ 5 phút.");

            string packageName = membership.PackageName;
            string method = fromCamera ? CameraMethod : ManualMethod;

            db.CheckIns.Add(new CheckIn
            {
                MemberID = memberId,
                BranchID = branchId,
                CheckInTime = now,
                Status = "Approved",
                Method = method,
                Note = packageName
            });
            await db.SaveChangesAsync();
            await transaction.CommitAsync();
            var visit = new QrCheckInVisit(lookup.MemberName!, FormatCode(cardId), packageName, now, method);
            return new(QrCheckInStatus.Accepted, $"{visit.Name} · {packageName}\r\n{lookup.BranchName} · {now:HH:mm:ss}", visit);
        }
    }
}
