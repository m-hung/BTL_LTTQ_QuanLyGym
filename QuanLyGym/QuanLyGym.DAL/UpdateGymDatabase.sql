-- ====================================================================================
-- SCRIPT CẬP NHẬT DATABASE QUẢN LÝ GYM THEO YÊU CẦU
-- LƯU Ý QUAN TRỌNG: Script KHÔNG tác động vào bảng __EFMigrationsHistory!
-- ====================================================================================

USE [QuanLyGymFullDB];
GO

-- 1. XÓA BẢNG WorkingSchedules NẾU TỒN TẠI
IF OBJECT_ID('dbo.WorkingSchedules', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.WorkingSchedules;
    PRINT N'>> Đã xóa bảng WorkingSchedules.';
END
GO

-- 2. TẠO BẢNG Staffs (NHÂN VIÊN: PT, Tạp vụ, Thu ngân, Bảo vệ)
IF OBJECT_ID('dbo.Staffs', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Staffs (
        StaffID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        FullName NVARCHAR(100) NOT NULL,
        Role NVARCHAR(50) NOT NULL,
        Gender NVARCHAR(10) NULL,
        DateOfBirth DATETIME2(7) NOT NULL,
        Phone NVARCHAR(15) NULL,
        Email NVARCHAR(100) NULL,
        WorkShift NVARCHAR(100) NOT NULL,
        BaseSalary DECIMAL(18,2) NOT NULL,
        Status BIT NOT NULL DEFAULT 1,
        BranchID INT NULL FOREIGN KEY REFERENCES dbo.Branches(BranchID),
        PTID INT NULL FOREIGN KEY REFERENCES dbo.PTs(PTID) ON DELETE SET NULL
    );
    PRINT N'>> Đã tạo bảng Staffs.';
END
GO

-- 3. TẠO BẢNG Classes (LỚP HỌC NHÓM)
IF OBJECT_ID('dbo.Classes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Classes (
        ClassID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        ClassName NVARCHAR(100) NOT NULL,
        ClassType NVARCHAR(50) NOT NULL,
        BranchID INT NOT NULL FOREIGN KEY REFERENCES dbo.Branches(BranchID),
        RoomID INT NOT NULL FOREIGN KEY REFERENCES dbo.Rooms(RoomID),
        PTID INT NOT NULL FOREIGN KEY REFERENCES dbo.PTs(PTID),
        DayOfWeek NVARCHAR(50) NOT NULL,
        StartTime TIME(7) NOT NULL,
        EndTime TIME(7) NOT NULL,
        MaxCapacity INT NOT NULL DEFAULT 25,
        Description NVARCHAR(500) NULL,
        Status BIT NOT NULL DEFAULT 1
    );
    PRINT N'>> Đã tạo bảng Classes.';
END
GO

-- 4. TẠO BẢNG ClassEnrollments (ĐIỂM DANH / ĐĂNG KÝ LỚP HỌC)
IF OBJECT_ID('dbo.ClassEnrollments', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ClassEnrollments (
        EnrollmentID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        ClassID INT NOT NULL FOREIGN KEY REFERENCES dbo.Classes(ClassID) ON DELETE CASCADE,
        MemberID INT NOT NULL FOREIGN KEY REFERENCES dbo.Members(MemberID),
        EnrollmentDate DATETIME2(7) NOT NULL,
        AttendanceStatus NVARCHAR(20) NOT NULL DEFAULT 'Attended'
            CHECK (AttendanceStatus IN ('Attended', 'Absent', 'Cancelled')),
        Note NVARCHAR(255) NULL
    );
    PRINT N'>> Đã tạo bảng ClassEnrollments.';
END
GO

-- 5. TẠO BẢNG Notifications (THÔNG BÁO HỆ THỐNG)
IF OBJECT_ID('dbo.Notifications', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Notifications (
        NotificationID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        AccountID INT NULL FOREIGN KEY REFERENCES dbo.Accounts(AccountID) ON DELETE CASCADE,
        Title NVARCHAR(150) NOT NULL,
        Content NVARCHAR(500) NOT NULL,
        NotificationType NVARCHAR(50) NOT NULL DEFAULT 'System',
        IsRead BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2(7) NOT NULL DEFAULT GETDATE(),
        BranchID INT NULL FOREIGN KEY REFERENCES dbo.Branches(BranchID)
    );
    PRINT N'>> Đã tạo bảng Notifications.';
END
GO

-- 6. KIỂM TRA VÀ THÊM CỘT IsPTPackage TRONG BẢNG Packages NẾU THIẾU
IF COL_LENGTH('dbo.Packages', 'IsPTPackage') IS NULL
BEGIN
    ALTER TABLE dbo.Packages ADD IsPTPackage BIT NOT NULL DEFAULT 0;
    PRINT N'>> Đã thêm cột IsPTPackage vào bảng Packages.';
END
GO

-- 7. XÓA BỎ CHỈ SỐ CƠ THỂ KHỎI BẢNG Members NẾU CÒN TỒN TẠI
IF COL_LENGTH('dbo.Members', 'Height') IS NOT NULL ALTER TABLE dbo.Members DROP COLUMN Height;
IF COL_LENGTH('dbo.Members', 'Weight') IS NOT NULL ALTER TABLE dbo.Members DROP COLUMN Weight;
IF COL_LENGTH('dbo.Members', 'BodyFat') IS NOT NULL ALTER TABLE dbo.Members DROP COLUMN BodyFat;
IF COL_LENGTH('dbo.Members', 'MuscleMass') IS NOT NULL ALTER TABLE dbo.Members DROP COLUMN MuscleMass;
IF COL_LENGTH('dbo.Members', 'BMI') IS NOT NULL ALTER TABLE dbo.Members DROP COLUMN BMI;
GO

-- ====================================================================================
-- 8. XÓA DỮ LIỆU CŨ VÀ NẠP DỮ LIỆU MỚI (THEO ĐÚNG THỨ TỰ RÀNG BUỘC KHÓA NGOẠI)
-- ====================================================================================

EXEC sp_MSforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all";
GO

DELETE FROM dbo.ClassEnrollments;
DELETE FROM dbo.Classes;
DELETE FROM dbo.Notifications;
DELETE FROM dbo.CheckInImages;
DELETE FROM dbo.CheckIns;
DELETE FROM dbo.Schedules;
DELETE FROM dbo.Progresses;
DELETE FROM dbo.EquipmentMaintenances;
DELETE FROM dbo.Incidents;
DELETE FROM dbo.Equipments;
DELETE FROM dbo.Rooms;
DELETE FROM dbo.Memberships;
DELETE FROM dbo.Staffs;
DELETE FROM dbo.PTs;
DELETE FROM dbo.Members;
DELETE FROM dbo.Packages;
DELETE FROM dbo.Accounts;
DELETE FROM dbo.Branches;
GO

-- ====================================================================================
-- A. BẢNG Branches: 3 chi nhánh tại TP.HCM
-- ====================================================================================
SET IDENTITY_INSERT dbo.Branches ON;
INSERT INTO dbo.Branches (BranchID, BranchName, Address, Phone, Email, OpenTime, CloseTime, Status) VALUES
(1, N'QuanLyGym Quận 1', N'123 Nguyễn Thị Minh Khai, Phường Bến Thành, Quận 1, TP. HCM', '02838221122', 'quan1@quanlygym.vn', '06:00:00', '22:00:00', 1),
(2, N'QuanLyGym Bình Thạnh', N'456 Điện Biên Phủ, Phường 25, Quận Bình Thạnh, TP. HCM', '02838332233', 'binhthanh@quanlygym.vn', '06:00:00', '22:00:00', 1),
(3, N'QuanLyGym Quận 7', N'789 Nguyễn Lương Bằng, Phường Tân Phú, Quận 7, TP. HCM', '02838443344', 'quan7@quanlygym.vn', '06:00:00', '22:00:00', 1);
SET IDENTITY_INSERT dbo.Branches OFF;
GO

-- ====================================================================================
-- B. BẢNG Accounts: Admin, 12 PTs, 10 Members
-- Mật khẩu mặc định: 123456 (PBKDF2 băm chuẩn AuthService)
-- ====================================================================================
SET IDENTITY_INSERT dbo.Accounts ON;
INSERT INTO dbo.Accounts (AccountID, Username, Email, PasswordHash, Role, Status, CreatedAt) VALUES
-- 1 Admin
(1, 'admin', 'admingym5@gmail.com', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'Admin', 1, '2026-01-01'),

-- 12 PTs (4 PT mỗi chi nhánh)
(2, 'pt_nam', 'nam.nguyen@quanlygym.vn', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'PT', 1, '2026-01-01'),
(3, 'pt_bichtran', 'bich.tran@quanlygym.vn', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'PT', 1, '2026-01-01'),
(4, 'pt_ducminh', 'minh.duc@quanlygym.vn', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'PT', 1, '2026-01-01'),
(5, 'pt_thaophan', 'thao.phan@quanlygym.vn', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'PT', 1, '2026-01-01'),
(6, 'pt_hoanglong', 'long.vu@quanlygym.vn', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'PT', 1, '2026-01-01'),
(17, 'pt_hoangnam', 'hoangnam.le@quanlygym.vn', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'PT', 1, '2026-01-01'),
(18, 'pt_thuthao', 'thuthao.dang@quanlygym.vn', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'PT', 1, '2026-01-01'),
(19, 'pt_ngocanh', 'ngocanh.pham@quanlygym.vn', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'PT', 1, '2026-01-01'),
(20, 'pt_quocbao', 'quocbao.tran@quanlygym.vn', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'PT', 1, '2026-01-01'),
(21, 'pt_khanhlinh', 'khanhlinh.do@quanlygym.vn', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'PT', 1, '2026-01-01'),
(22, 'pt_tiendat', 'tiendat.nguyen@quanlygym.vn', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'PT', 1, '2026-01-01'),
(23, 'pt_kimngan', 'kimngan.hoang@quanlygym.vn', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'PT', 1, '2026-01-01'),

-- 10 Members
(7, 'member_khanhhung', 'nguyenkhanhhung2512@gmail.com', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'Member', 1, '2026-01-01'),
(8, 'member_minhhung', 'minhhungthai7@gmail.com', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'Member', 1, '2026-01-01'),
(9, 'member_hoa', 'hoa.pham@gmail.com', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'Member', 1, '2026-01-01'),
(10, 'member_dung', 'dung.hoang@gmail.com', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'Member', 1, '2026-01-01'),
(11, 'member_phuong', 'phuong.do@gmail.com', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'Member', 1, '2026-01-01'),
(12, 'member_tuan', 'tuan.ngo@gmail.com', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'Member', 1, '2026-01-01'),
(13, 'member_mai', 'mai.bui@gmail.com', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'Member', 1, '2026-01-01'),
(14, 'member_vy', 'vy.trinh@gmail.com', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'Member', 1, '2026-01-01'),
(15, 'member_khoa', 'khoa.dang@gmail.com', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'Member', 1, '2026-01-01'),
(16, 'member_tram', 'tram.vo@gmail.com', 'PBKDF2$100000$6nqYawHporBDCQpOTdXZdA==$HJcSl22pVMBm2pEqQMq0C/h8wthaCgMd1P2n7IfzI7o=', 'Member', 1, '2026-01-01');
SET IDENTITY_INSERT dbo.Accounts OFF;
GO

-- ====================================================================================
-- C. BẢNG PTs: 12 PT (Mỗi chi nhánh 4 PT riêng phụ trách từng lớp học nhóm)
-- ====================================================================================
SET IDENTITY_INSERT dbo.PTs ON;
INSERT INTO dbo.PTs (PTID, AccountID, FullName, Gender, DateOfBirth, Phone, Email, Specialization, Experience, Status, BranchID) VALUES
-- Branch 1 (Quận 1): 4 PTs
(1, 2, N'Nguyễn Văn Nam', N'Nam', '1995-05-20', '0988888888', 'nam.nguyen@quanlygym.vn', N'Boxing & Kickfit Cường độ cao', 6, 1, 1),
(2, 3, N'Trần Thị Bích', N'Nữ', '1997-08-14', '0943210987', 'bich.tran@quanlygym.vn', N'Yoga Căn Bản & Thiền Định', 5, 1, 1),
(6, 17, N'Lê Hoàng Nam', N'Nam', '1996-03-10', '0976123456', 'hoangnam.le@quanlygym.vn', N'Zumba Dance Fitness & Cardio', 5, 1, 1),
(7, 18, N'Đặng Thu Thảo', N'Nữ', '1998-10-22', '0938112233', 'thuthao.dang@quanlygym.vn', N'Pilates Định Hình & Phục Hồi Cột Sống', 4, 1, 1),

-- Branch 2 (Bình Thạnh): 4 PTs
(3, 4, N'Nguyễn Đức Minh', N'Nam', '1994-03-25', '0987543210', 'minh.duc@quanlygym.vn', N'Boxing & Kickfit Cường độ cao', 7, 1, 2),
(4, 5, N'Phan Thị Thảo', N'Nữ', '1998-11-12', '0921098765', 'thao.phan@quanlygym.vn', N'Zumba Dance Fitness & Aerobic', 4, 1, 2),
(8, 19, N'Phạm Ngọc Ánh', N'Nữ', '1996-07-19', '0918776655', 'ngocanh.pham@quanlygym.vn', N'Yoga Căn Bản & Thiền Định', 5, 1, 2),
(9, 20, N'Trần Quốc Bảo', N'Nam', '1995-12-05', '0903445566', 'quocbao.tran@quanlygym.vn', N'Pilates Định Hình & Phục Hồi Cột Sống', 6, 1, 2),

-- Branch 3 (Quận 7): 4 PTs
(5, 6, N'Vũ Hoàng Long', N'Nam', '1996-09-08', '0909876543', 'long.vu@quanlygym.vn', N'Boxing & Kickfit Cường độ cao', 5, 1, 3),
(10, 21, N'Đỗ Khánh Linh', N'Nữ', '1997-04-15', '0922334455', 'khanhlinh.do@quanlygym.vn', N'Yoga Căn Bản & Thiền Định', 5, 1, 3),
(11, 22, N'Nguyễn Tiến Đạt', N'Nam', '1996-11-30', '0933557799', 'tiendat.nguyen@quanlygym.vn', N'Zumba Dance Fitness & Cardio', 4, 1, 3),
(12, 23, N'Hoàng Kim Ngân', N'Nữ', '1999-08-08', '0944668800', 'kimngan.hoang@quanlygym.vn', N'Pilates Định Hình & Phục Hồi Cột Sống', 4, 1, 3);
SET IDENTITY_INSERT dbo.PTs OFF;
GO

-- ====================================================================================
-- D. BẢNG Members: 10 hội viên tại 3 chi nhánh
-- ====================================================================================
SET IDENTITY_INSERT dbo.Members ON;
INSERT INTO dbo.Members (MemberID, AccountID, FullName, Gender, DateOfBirth, Phone, Email, Address, Avatar, CreatedAt, BranchID) VALUES
(1, 7, N'Nguyễn Khánh Hưng', N'Nam', '2000-10-10', '0977777777', 'nguyenkhanhhung2512@gmail.com', N'15 Lê Lợi, Quận 1, TP. HCM', '', '2026-01-01', 1),
(2, 8, N'Thái Trần Minh Hưng', N'Nam', '1999-04-15', '0912345678', 'minhhungthai7@gmail.com', N'88 Pasteur, Quận 1, TP. HCM', '', '2026-01-05', 1),
(3, 9, N'Phạm Quỳnh Hoa', N'Nữ', '2001-08-22', '0934567890', 'hoa.pham@gmail.com', N'12 Nguyễn Huệ, Quận 1, TP. HCM', '', '2026-01-10', 1),
(4, 10, N'Hoàng Tiến Dũng', N'Nam', '1996-12-05', '0945678901', 'dung.hoang@gmail.com', N'204 Hai Bà Trưng, Quận 1, TP. HCM', '', '2026-01-15', 1),
(5, 11, N'Đỗ Thu Phương', N'Nữ', '1998-03-30', '0956789012', 'phuong.do@gmail.com', N'55 Nam Kỳ Khởi Nghĩa, Quận 1, TP. HCM', '', '2026-01-20', 1),
(6, 12, N'Ngô Anh Tuấn', N'Nam', '1995-07-18', '0967890123', 'tuan.ngo@gmail.com', N'32 Bạch Đằng, Quận Bình Thạnh, TP. HCM', '', '2026-02-01', 2),
(7, 13, N'Bùi Tuyết Mai', N'Nữ', '2002-02-14', '0978901234', 'mai.bui@gmail.com', N'150 Nơ Trang Long, Quận Bình Thạnh, TP. HCM', '', '2026-02-05', 2),
(8, 14, N'Trịnh Thảo Vy', N'Nữ', '1997-09-09', '0989012345', 'vy.trinh@gmail.com', N'82 Xô Viết Nghệ Tĩnh, Quận Bình Thạnh, TP. HCM', '', '2026-02-10', 2),
(9, 15, N'Đặng Đăng Khoa', N'Nam', '1993-11-28', '0990123456', 'khoa.dang@gmail.com', N'100 Huỳnh Tấn Phát, Quận 7, TP. HCM', '', '2026-02-15', 3),
(10, 16, N'Võ Bảo Trâm', N'Nữ', '2000-06-16', '0901234567', 'tram.vo@gmail.com', N'45 Nguyễn Thị Thập, Quận 7, TP. HCM', '', '2026-02-20', 3);
SET IDENTITY_INSERT dbo.Members OFF;
GO

-- ====================================================================================
-- E. BẢNG Packages: 4 gói tập tiêu chuẩn
-- ====================================================================================
SET IDENTITY_INSERT dbo.Packages ON;
INSERT INTO dbo.Packages (PackageID, PackageName, Duration, Price, TotalPTSessions, IsPTPackage, Benefits, Description, Status, BranchID) VALUES
(1, N'Classic', 30, 500000, 0, 0, N'Tập tự do không giới hạn khung giờ, tủ đồ cá nhân tiêu chuẩn', N'Gói tập tự do tiêu chuẩn cho người bận rộn', 1, 1),
(2, N'Silver', 90, 2800000, 12, 1, N'Kèm PT 1-1 (12 buổi PT), kiểm tra chỉ số InBody định kỳ hàng tháng', N'Gói tập rèn luyện chuyên sâu cùng HLV cá nhân', 1, 1),
(3, N'Gold', 180, 5800000, 36, 1, N'Kèm PT 1-1 (36 buổi PT), đo InBody định kỳ, phòng xông hơi Sauna thảo dược', N'Gói tập Gold cao cấp toàn diện phục hồi thể lực', 1, 1),
(4, N'VIP Diamond', 365, 12500000, 72, 1, N'Kèm PT 1-1 không giới hạn, full dịch vụ VIP Sauna, khăn tập & locker riêng cao cấp', N'Gói tập thượng lưu VIP Diamond đẳng cấp nhất', 1, 1);
SET IDENTITY_INSERT dbo.Packages OFF;
GO

-- ====================================================================================
-- F. BẢNG Memberships: Doanh thu đăng ký FULL 12 THÁNG NĂM 2026 (3 chi nhánh)
-- ====================================================================================
SET IDENTITY_INSERT dbo.Memberships ON;
INSERT INTO dbo.Memberships (MembershipID, MemberID, PackageID, RegisterDate, StartDate, EndDate, Price, PriceAtPurchase, UsedPTSessions, Status, CreatedAt, BranchID) VALUES
-- Tháng 1/2026
(1, 1, 4, '2026-01-01', '2026-01-01', '2027-01-01', 12500000, 12500000, 25, 'Active', '2026-01-01', 1),
(4, 4, 1, '2026-01-15', '2026-01-15', '2026-04-15', 500000, 500000, 14, 'Active', '2026-01-15', 1),

-- Tháng 2/2026
(2, 2, 3, '2026-02-01', '2026-02-01', '2026-08-01', 5800000, 5800000, 18, 'Active', '2026-02-01', 1),
(5, 5, 1, '2026-02-01', '2026-02-01', '2026-05-01', 500000, 500000, 12, 'Active', '2026-02-01', 1),
(6, 6, 1, '2026-02-01', '2026-02-01', '2026-05-01', 500000, 500000, 8, 'Active', '2026-02-01', 2),
(7, 7, 1, '2026-02-10', '2026-02-10', '2026-05-10', 500000, 500000, 6, 'Active', '2026-02-10', 2),
(8, 8, 1, '2026-02-15', '2026-02-15', '2026-05-15', 500000, 500000, 15, 'Active', '2026-02-15', 2),
(9, 9, 1, '2026-02-20', '2026-02-20', '2026-05-20', 500000, 500000, 9, 'Active', '2026-02-20', 3),

-- Tháng 3/2026
(3, 3, 2, '2026-03-01', '2026-03-01', '2026-06-01', 2800000, 2800000, 10, 'Active', '2026-03-01', 1),
(10, 10, 1, '2026-03-01', '2026-03-01', '2026-06-01', 500000, 500000, 5, 'Active', '2026-03-01', 3),
(11, 6, 3, '2026-03-15', '2026-03-15', '2026-09-15', 5800000, 5800000, 14, 'Active', '2026-03-15', 2),

-- Tháng 4/2026
(12, 4, 2, '2026-04-16', '2026-04-16', '2026-07-16', 2800000, 2800000, 8, 'Active', '2026-04-16', 1),
(13, 7, 1, '2026-04-20', '2026-04-20', '2026-07-20', 500000, 500000, 10, 'Active', '2026-04-20', 2),

-- Tháng 5/2026
(14, 5, 2, '2026-05-05', '2026-05-05', '2026-08-05', 2800000, 2800000, 11, 'Active', '2026-05-05', 1),
(15, 9, 3, '2026-05-22', '2026-05-22', '2026-11-22', 5800000, 5800000, 20, 'Active', '2026-05-22', 3),

-- Tháng 6/2026
(16, 8, 2, '2026-06-10', '2026-06-10', '2026-09-10', 2800000, 2800000, 9, 'Active', '2026-06-10', 2),
(17, 10, 1, '2026-06-15', '2026-06-15', '2026-09-15', 500000, 500000, 7, 'Active', '2026-06-15', 3),

-- Tháng 7/2026
(18, 2, 4, '2026-07-01', '2026-07-01', '2027-07-01', 12500000, 12500000, 22, 'Active', '2026-07-01', 1),
(19, 3, 3, '2026-07-15', '2026-07-15', '2027-01-15', 5800000, 5800000, 16, 'Active', '2026-07-15', 1),

-- Tháng 8/2026
(20, 6, 2, '2026-08-05', '2026-08-05', '2026-11-05', 2800000, 2800000, 8, 'Active', '2026-08-05', 2),
(21, 10, 2, '2026-08-20', '2026-08-20', '2026-11-20', 2800000, 2800000, 6, 'Active', '2026-08-20', 3),

-- Tháng 9/2026
(22, 7, 3, '2026-09-10', '2026-09-10', '2027-03-10', 5800000, 5800000, 12, 'Active', '2026-09-10', 2),
(23, 4, 1, '2026-09-25', '2026-09-25', '2026-12-25', 500000, 500000, 10, 'Active', '2026-09-25', 1),

-- Tháng 10/2026
(24, 9, 4, '2026-10-05', '2026-10-05', '2027-10-05', 12500000, 12500000, 15, 'Active', '2026-10-05', 3),
(25, 5, 1, '2026-10-18', '2026-10-18', '2027-01-18', 500000, 500000, 5, 'Active', '2026-10-18', 1),

-- Tháng 11/2026
(26, 8, 3, '2026-11-02', '2026-11-02', '2027-05-02', 5800000, 5800000, 4, 'Active', '2026-11-02', 2),
(27, 1, 3, '2026-11-15', '2026-11-15', '2027-05-15', 5800000, 5800000, 2, 'Active', '2026-11-15', 1),
(28, 3, 2, '2026-11-22', '2026-11-22', '2027-02-22', 2800000, 2800000, 1, 'Active', '2026-11-22', 1),

-- Tháng 12/2026
(29, 2, 3, '2026-12-05', '2026-12-05', '2027-06-05', 5800000, 5800000, 0, 'Active', '2026-12-05', 1),
(30, 6, 4, '2026-12-12', '2026-12-12', '2027-12-12', 12500000, 12500000, 0, 'Active', '2026-12-12', 2),
(31, 10, 2, '2026-12-20', '2026-12-20', '2027-03-20', 2800000, 2800000, 0, 'Active', '2026-12-20', 3);
SET IDENTITY_INSERT dbo.Memberships OFF;
GO

-- ====================================================================================
-- G. BẢNG Staffs: 21 nhân viên (12 PT, 3 Thu ngân, 3 Tạp vụ, 3 Bảo vệ)
-- ====================================================================================
SET IDENTITY_INSERT dbo.Staffs ON;
INSERT INTO dbo.Staffs (StaffID, FullName, Role, Gender, DateOfBirth, Phone, Email, WorkShift, BaseSalary, Status, BranchID, PTID) VALUES
-- 12 PTs
(1, N'Nguyễn Văn Nam', N'PT', N'Nam', '1995-05-20', '0988888888', 'nam.nguyen@quanlygym.vn', N'Ca cố định: 08:00 - 16:00 (Thứ 2 - CN)', 16000000, 1, 1, 1),
(2, N'Trần Thị Bích', N'PT', N'Nữ', '1997-08-14', '0943210987', 'bich.tran@quanlygym.vn', N'Ca cố định: 14:00 - 22:00 (Thứ 2 - CN)', 15500000, 1, 1, 2),
(15, N'Lê Hoàng Nam', N'PT', N'Nam', '1996-03-10', '0976123456', 'hoangnam.le@quanlygym.vn', N'Ca cố định: 14:00 - 22:00 (Thứ 2 - CN)', 15000000, 1, 1, 6),
(16, N'Đặng Thu Thảo', N'PT', N'Nữ', '1998-10-22', '0938112233', 'thuthao.dang@quanlygym.vn', N'Ca cố định: 06:00 - 14:00 (Thứ 2 - CN)', 14500000, 1, 1, 7),

(3, N'Nguyễn Đức Minh', N'PT', N'Nam', '1994-03-25', '0987543210', 'minh.duc@quanlygym.vn', N'Ca cố định: 08:00 - 16:00 (Thứ 2 - CN)', 16500000, 1, 2, 3),
(4, N'Phan Thị Thảo', N'PT', N'Nữ', '1998-11-12', '0921098765', 'thao.phan@quanlygym.vn', N'Ca cố định: 14:00 - 22:00 (Thứ 2 - CN)', 14500000, 1, 2, 4),
(17, N'Phạm Ngọc Ánh', N'PT', N'Nữ', '1996-07-19', '0918776655', 'ngocanh.pham@quanlygym.vn', N'Ca cố định: 06:00 - 14:00 (Thứ 2 - CN)', 15000000, 1, 2, 8),
(18, N'Trần Quốc Bảo', N'PT', N'Nam', '1995-12-05', '0903445566', 'quocbao.tran@quanlygym.vn', N'Ca cố định: 08:00 - 16:00 (Thứ 2 - CN)', 16000000, 1, 2, 9),

(5, N'Vũ Hoàng Long', N'PT', N'Nam', '1996-09-08', '0909876543', 'long.vu@quanlygym.vn', N'Ca cố định: 09:00 - 17:00 (Thứ 2 - CN)', 15000000, 1, 3, 5),
(19, N'Đỗ Khánh Linh', N'PT', N'Nữ', '1997-04-15', '0922334455', 'khanhlinh.do@quanlygym.vn', N'Ca cố định: 06:00 - 14:00 (Thứ 2 - CN)', 15000000, 1, 3, 10),
(20, N'Nguyễn Tiến Đạt', N'PT', N'Nam', '1996-11-30', '0933557799', 'tiendat.nguyen@quanlygym.vn', N'Ca cố định: 14:00 - 22:00 (Thứ 2 - CN)', 14500000, 1, 3, 11),
(21, N'Hoàng Kim Ngân', N'PT', N'Nữ', '1999-08-08', '0944668800', 'kimngan.hoang@quanlygym.vn', N'Ca cố định: 08:00 - 16:00 (Thứ 2 - CN)', 14500000, 1, 3, 12),

-- 3 Thu ngân
(6, N'Nguyễn Thị Lan Anh', N'Thu ngân', N'Nữ', '2000-03-10', '0912345679', 'lananh.tn@quanlygym.vn', N'Ca sáng (06:00 - 14:00)', 9000000, 1, 1, NULL),
(7, N'Trần Quang Huy', N'Thu ngân', N'Nam', '1999-12-04', '0934567891', 'quanghuy.tn@quanlygym.vn', N'Ca chiều (14:00 - 22:00)', 9000000, 1, 2, NULL),
(8, N'Đặng Minh Châu', N'Thu ngân', N'Nữ', '2001-05-18', '0966222333', 'minhchau.tn@quanlygym.vn', N'Ca xoay (08:00 - 17:00)', 9500000, 1, 3, NULL),

-- 3 Tạp vụ
(9, N'Lê Thị Mai', N'Tạp vụ', N'Nữ', '1980-08-20', '0905111222', 'maile.tv@quanlygym.vn', N'Ca sáng (06:00 - 14:00)', 7500000, 1, 1, NULL),
(10, N'Nguyễn Thị Sáu', N'Tạp vụ', N'Nữ', '1982-10-15', '0932333444', 'saunguyen.tv@quanlygym.vn', N'Ca chiều (14:00 - 22:00)', 7500000, 1, 2, NULL),
(11, N'Hoàng Thị Nga', N'Tạp vụ', N'Nữ', '1985-04-12', '0988444555', 'ngahoang.tv@quanlygym.vn', N'Ca sáng (06:00 - 14:00)', 7500000, 1, 3, NULL),

-- 3 Bảo vệ
(12, N'Bùi Văn Dũng', N'Bảo vệ', N'Nam', '1978-01-25', '0911777888', 'dungbui.bv@quanlygym.vn', N'Ca ngày (06:00 - 18:00)', 8500000, 1, 1, NULL),
(13, N'Ngô Quang Hải', N'Bảo vệ', N'Nam', '1983-06-30', '0944888999', 'haingo.bv@quanlygym.vn', N'Ca đêm (18:00 - 06:00)', 9000000, 1, 2, NULL),
(14, N'Đỗ Văn Tuấn', N'Bảo vệ', N'Nam', '1986-09-17', '0977999000', 'tuando.bv@quanlygym.vn', N'Ca ngày (06:00 - 18:00)', 8500000, 1, 3, NULL);
SET IDENTITY_INSERT dbo.Staffs OFF;
GO

-- ====================================================================================
-- H. BẢNG Rooms: 6 phòng tập tại 3 chi nhánh
-- ====================================================================================
SET IDENTITY_INSERT dbo.Rooms ON;
INSERT INTO dbo.Rooms (RoomID, RoomName, RoomType, Capacity, Description, Status, BranchID) VALUES
(1, N'Phòng Gym & Tạ Tầng 1', 'Gym', 60, N'Khu vực tập tạ tổng hợp và máy khối', 1, 1),
(2, N'Phòng Cardio & Studio Tầng 2', 'Cardio', 40, N'Máy chạy bộ, xe đạp và sàn tập Yoga/Zumba', 1, 1),
(3, N'Phòng Gym Free Weight Tầng 1', 'Gym', 50, N'Khu tạ tự do và dàn Squat/Bench', 1, 2),
(4, N'Phòng Cardio & Functional Tầng 2', 'Functional', 35, N'Khu chức năng và sàn tập Studio', 1, 2),
(5, N'Phòng Strength & Conditioning', 'Gym', 45, N'Khu tập thể lực và tạ khối', 1, 3),
(6, N'Phòng Studio & Cardio Tầng 2', 'Cardio', 30, N'Khu cardio và sàn tập nhóm Studio', 1, 3);
SET IDENTITY_INSERT dbo.Rooms OFF;
GO

-- ====================================================================================
-- I. BẢNG Classes: 12 lớp học nhóm (3 chi nhánh đều có 4 lớp học đó, 12 PT riêng)
-- ====================================================================================
SET IDENTITY_INSERT dbo.Classes ON;
INSERT INTO dbo.Classes (ClassID, ClassName, ClassType, BranchID, RoomID, PTID, DayOfWeek, StartTime, EndTime, MaxCapacity, Description, Status) VALUES
-- Chi nhánh 1 (Quận 1): 4 lớp học
(1, N'Lớp Yoga Căn Bản & Thiền Định', 'Yoga', 1, 2, 2, N'Thứ 2 - 4 - 6', '06:30:00', '08:00:00', 20, N'Lớp Yoga asana kết hợp kỹ thuật thở Pranayama và thiền tịnh tâm, giúp giảm stress và tăng độ dẻo dai.', 1),
(2, N'Lớp Boxing & Kickfit Cường Độ Cao', 'Boxing', 1, 1, 1, N'Thứ 2 - 4 - 6', '18:00:00', '19:30:00', 25, N'Kỹ thuật đấm đá cơ bản, di chuyển đối kháng và rèn luyện sức bền tim mạch, tự vệ thực chiến.', 1),
(3, N'Lớp Zumba Dance Đốt Cháy Calo', 'Zumba', 1, 2, 6, N'Thứ 3 - 5 - 7', '18:30:00', '19:45:00', 30, N'Vũ điệu Zumba Latin sôi động trên nền nhạc lôi cuốn, hỗ trợ đốt cháy 600-800 kcal mỗi buổi tập.', 1),
(4, N'Lớp Pilates Định Hình & Cột Sống', 'Pilates', 1, 2, 7, N'Thứ 3 - 5 - 7', '07:00:00', '08:15:00', 18, N'Bài tập thảm Pilates tăng cường cơ lõi Core, chỉnh sửa tư thế gù lưng và thon gọn vòng eo.', 1),

-- Chi nhánh 2 (Bình Thạnh): 4 lớp học
(5, N'Lớp Yoga Căn Bản & Thiền Định', 'Yoga', 2, 4, 8, N'Thứ 2 - 4 - 6', '06:30:00', '08:00:00', 20, N'Lớp Yoga asana kết hợp kỹ thuật thở Pranayama và thiền tịnh tâm, giúp giảm stress và tăng độ dẻo dai.', 1),
(6, N'Lớp Boxing & Kickfit Cường Độ Cao', 'Boxing', 2, 3, 3, N'Thứ 2 - 4 - 6', '18:00:00', '19:30:00', 25, N'Kỹ thuật đấm đá cơ bản, di chuyển đối kháng và rèn luyện sức bền tim mạch, tự vệ thực chiến.', 1),
(7, N'Lớp Zumba Dance Đốt Cháy Calo', 'Zumba', 2, 4, 4, N'Thứ 3 - 5 - 7', '18:30:00', '19:45:00', 30, N'Vũ điệu Zumba Latin sôi động trên nền nhạc lôi cuốn, hỗ trợ đốt cháy 600-800 kcal mỗi buổi tập.', 1),
(8, N'Lớp Pilates Định Hình & Cột Sống', 'Pilates', 2, 4, 9, N'Thứ 3 - 5 - 7', '07:00:00', '08:15:00', 18, N'Bài tập thảm Pilates tăng cường cơ lõi Core, chỉnh sửa tư thế gù lưng và thon gọn vòng eo.', 1),

-- Chi nhánh 3 (Quận 7): 4 lớp học
(9, N'Lớp Yoga Căn Bản & Thiền Định', 'Yoga', 3, 6, 10, N'Thứ 2 - 4 - 6', '06:30:00', '08:00:00', 20, N'Lớp Yoga asana kết hợp kỹ thuật thở Pranayama và thiền tịnh tâm, giúp giảm stress và tăng độ dẻo dai.', 1),
(10, N'Lớp Boxing & Kickfit Cường Độ Cao', 'Boxing', 3, 5, 5, N'Thứ 2 - 4 - 6', '18:00:00', '19:30:00', 25, N'Kỹ thuật đấm đá cơ bản, di chuyển đối kháng và rèn luyện sức bền tim mạch, tự vệ thực chiến.', 1),
(11, N'Lớp Zumba Dance Đốt Cháy Calo', 'Zumba', 3, 6, 11, N'Thứ 3 - 5 - 7', '18:30:00', '19:45:00', 30, N'Vũ điệu Zumba Latin sôi động trên nền nhạc lôi cuốn, hỗ trợ đốt cháy 600-800 kcal mỗi buổi tập.', 1),
(12, N'Lớp Pilates Định Hình & Cột Sống', 'Pilates', 3, 6, 12, N'Thứ 3 - 5 - 7', '07:00:00', '08:15:00', 18, N'Bài tập thảm Pilates tăng cường cơ lõi Core, chỉnh sửa tư thế gù lưng và thon gọn vòng eo.', 1);
SET IDENTITY_INSERT dbo.Classes OFF;
GO

-- ====================================================================================
-- J. BẢNG ClassEnrollments: 25 bản ghi đăng ký / điểm danh lớp nhóm năm 2026
-- RÀNG BUỘC: AttendanceStatus CHỈ CÓ 'Attended', 'Absent', 'Cancelled'
-- ====================================================================================
SET IDENTITY_INSERT dbo.ClassEnrollments ON;
INSERT INTO dbo.ClassEnrollments (EnrollmentID, ClassID, MemberID, EnrollmentDate, AttendanceStatus, Note) VALUES
(1, 1, 2, '2026-01-12 06:30:00', 'Attended', N'Hoàn thành tốt các tư thế Asana chào mặt trời'),
(2, 1, 3, '2026-01-14 06:30:00', 'Attended', N'Tập trung nhịp thở sâu rất tốt'),
(3, 2, 1, '2026-01-19 18:00:00', 'Attended', N'Rèn luyện thể lực đấm bao cát cùng HLV Nam'),
(4, 2, 4, '2026-01-21 18:00:00', 'Absent', N'Hội viên bận công việc đột xuất chưa đến lớp'),
(5, 3, 3, '2026-02-03 18:30:00', 'Attended', N'Theo kịp các bước nhảy Latin vui vẻ'),
(6, 4, 5, '2026-02-10 07:00:00', 'Attended', N'Bài tập cơ bụng thon gọn cùng HLV Thảo'),
(7, 5, 7, '2026-03-09 06:30:00', 'Attended', N'Dẻo dai khớp vai và cột sống'),
(8, 6, 6, '2026-03-16 18:00:00', 'Attended', N'Lực đấm và bước di chuyển tiến bộ rõ rệt'),
(9, 7, 8, '2026-03-24 18:30:00', 'Cancelled', N'Hội viên xin hủy lịch trước 1 ngày do đi công tác'),
(10, 8, 7, '2026-04-07 07:00:00', 'Attended', N'Chỉnh sửa tư thế vai gù hiệu quả'),
(11, 9, 10, '2026-05-11 06:30:00', 'Attended', N'Học viên nắm vững chuỗi bài vinyasa'),
(12, 10, 9, '2026-05-20 18:00:00', 'Attended', N'Tập cường độ cao với HLV Hoàng Long'),
(13, 11, 10, '2026-06-09 18:30:00', 'Absent', N'Nghỉ không phép buổi tập tối'),
(14, 12, 9, '2026-06-18 07:00:00', 'Attended', N'Tập trung siết cơ mông đùi'),
(15, 1, 2, '2026-07-06 06:30:00', 'Attended', N'Độ dẻo dai cơ gân kheo cải thiện'),
(16, 2, 1, '2026-07-15 18:00:00', 'Attended', N'Tập đấm đá phản xạ chân mông'),
(17, 3, 5, '2026-08-04 18:30:00', 'Cancelled', N'Hủy lịch vì lý do sức khỏe cá nhân'),
(18, 6, 6, '2026-08-17 18:00:00', 'Attended', N'Buổi tập Kickfit cường độ tối đa'),
(19, 4, 3, '2026-09-08 07:00:00', 'Attended', N'Cải thiện sức bền cơ bụng rãnh 11'),
(20, 5, 8, '2026-09-21 06:30:00', 'Attended', N'Thiền định tĩnh tâm cuối buổi tập'),
(21, 7, 7, '2026-10-06 18:30:00', 'Attended', N'Không khí lớp học sôi nổi nhiệt huyết'),
(22, 10, 9, '2026-10-19 18:00:00', 'Absent', N'Vắng mặt buổi tập đối kháng'),
(23, 1, 3, '2026-11-09 06:30:00', 'Attended', N'Tập thở Pranayama nâng cao'),
(24, 2, 1, '2026-11-16 18:00:00', 'Attended', N'Kiểm tra kỹ thuật gạt đòn và phản công'),
(25, 3, 2, '2026-11-24 18:30:00', 'Attended', N'Hội viên tham gia đầy đủ nhiệt tình');
SET IDENTITY_INSERT dbo.ClassEnrollments OFF;
GO

-- ====================================================================================
-- K. BẢNG Notifications: 11 thông báo hệ thống và cá nhân trải dài năm 2026
-- ====================================================================================
SET IDENTITY_INSERT dbo.Notifications ON;
INSERT INTO dbo.Notifications (NotificationID, AccountID, Title, Content, NotificationType, IsRead, CreatedAt, BranchID) VALUES
(1, NULL, N'Chúc Mừng Năm Mới 2026 - Lịch Hoạt Động Tết', N'Hệ thống QuanLyGym trân trọng gửi lời chúc mừng năm mới 2026 đến toàn thể quý Hội viên và HLV. Hệ thống sẽ mở cửa xuyên suốt phục vụ quý khách.', 'System', 1, '2026-01-01 08:00:00', NULL),
(2, 7, N'Xác Nhận Kích Hoạt Gói VIP Diamond', N'Chào bạn Khánh Hưng! Hợp đồng gói tập VIP Diamond của bạn đã được kích hoạt thành công. HLV cá nhân Nguyễn Văn Nam sẽ đồng hành cùng bạn.', 'Reminder', 1, '2026-01-01 09:30:00', 1),
(3, NULL, N'Khai Trương Studio GroupX & Lớp Học Nhóm Mới', N'QuanLyGym chính thức mở các lớp học nhóm: Yoga, Zumba, Boxing và Pilates tại cả 3 chi nhánh với đội ngũ HLV hàng đầu. Đăng ký ngay tại quầy lễ tân!', 'Promotion', 1, '2026-01-10 10:00:00', NULL),
(4, NULL, N'Ưu Đãi Đặc Biệt Quốc Tế Phụ Nữ 8/3', N'Tặng ngay 20% khi đăng ký hoặc nâng cấp gói tập và giảm 50% phí thuê HLV kèm riêng cho phái đẹp trong tuần lễ từ 01/03 đến 08/03/2026.', 'Promotion', 1, '2026-03-01 08:00:00', NULL),
(5, 8, N'Nhắc Nhở Đo Lại Chỉ Số Thể Chất InBody Định Kỳ', N'Đã 3 tháng kể từ lần kiểm tra thể chất gần nhất. Mời bạn đến phòng đo InBody tại Chi nhánh 1 để HLV Nam đánh giá lại tiến độ tăng cơ giảm mỡ.', 'Reminder', 1, '2026-04-15 14:00:00', 1),
(6, NULL, N'Hoàn Tất Bảo Trì & Nâng Cấp Hệ Thống Máy Chạy Bộ', N'Hệ thống máy chạy bộ Matrix và dàn tạ khối tại Chi nhánh Quận 1 & Bình Thạnh đã được bảo dưỡng toàn diện, sẵn sàng phục vụ quý hội viên.', 'Maintenance', 1, '2026-06-02 09:00:00', 1),
(7, NULL, N'Chào Hè Sôi Động 2026 - Thử Thách Đốt Mỡ 30 Ngày', N'Tham gia giải chạy và thử thách Fitness 30 ngày để nhận gói tập VIP 6 tháng và phần thưởng giá trị từ Ban quản lý QuanLyGym.', 'System', 1, '2026-07-01 08:30:00', NULL),
(8, 9, N'Thông Báo Sắp Hết Hạn Thẻ Tập', N'Thẻ tập của bạn sắp hết hạn trong 7 ngày tới. Hãy liên hệ bộ phận Lễ tân hoặc gia hạn trên ứng dụng để nhận ngay ưu đãi 10% gói tập mới.', 'Reminder', 0, '2026-09-25 10:00:00', 1),
(9, NULL, N'Đại Tiệc Siêu Sale Black Friday Tháng 11/2026', N'Cơ hội duy nhất trong năm: Mua 1 năm tặng 3 tháng, giảm giá cực sốc toàn bộ các gói Classic, Silver, Gold và VIP Diamond!', 'Promotion', 0, '2026-11-20 08:00:00', NULL),
(10, 2, N'Thông Báo Lịch Đặt Hẹn PT Mới Tháng 12', N'Bạn có 15 lịch hẹn tập luyện mới từ hội viên trải dài suốt tháng 12/2026. Vui lòng kiểm tra mục Lịch tập và xác nhận các ca chờ duyệt.', 'Reminder', 0, '2026-11-27 08:00:00', 1),
(11, NULL, N'Kế Hoạch Tổng Vệ Sinh & Kiểm Tra An Toàn Cuối Năm', N'Chi nhánh Quận 7 và Bình Thạnh sẽ thực hiện kiểm tra an toàn thiết bị phòng tập định kỳ vào ngày 15/12/2026 từ 22:00 đến 24:00.', 'Maintenance', 0, '2026-11-27 09:00:00', 3);
SET IDENTITY_INSERT dbo.Notifications OFF;
GO

-- ====================================================================================
-- L. BẢNG Schedules: 60 lịch tập, CHÚ TRỌNG VÀO PT NAM TỪ 27/11 ĐẾN HẾT THÁNG 12/2026
-- ====================================================================================
SET IDENTITY_INSERT dbo.Schedules ON;
INSERT INTO dbo.Schedules (ScheduleID, MemberID, PTID, RoomID, TrainingDate, StartTime, EndTime, TrainingType, Status, Note, BranchID) VALUES
-- Lịch tập quá khứ của PT 1, 2, 3 (Tháng 10 - Tháng 11)
(1, 1, 1, 1, '2026-10-26', '08:30:00', '10:00:00', N'Tập Ngực & Tay Sau (Hypertrophy)', 'Completed', N'Hội viên chuẩn bị thể lực tốt, mức tạ ngực 60kg', 1),
(2, 2, 1, 2, '2026-10-27', '14:00:00', '15:30:00', N'Cardio HIIT & Đốt Mỡ Chuyên Sâu', 'Completed', N'Đốt calo cường độ cao, đã hoàn thành', 1),
(3, 1, 1, 1, '2026-10-29', '09:00:00', '10:30:00', N'Tập Lưng Xô & Tay Trước', 'Completed', N'Tập trung bài Lat Pulldown & Seated Row', 1),
(4, 3, 1, 1, '2026-10-31', '15:00:00', '16:30:00', N'Tập Chân Mông & Giãn Cơ', 'Confirmed', N'Khởi động kỹ khớp gối', 1),
(5, 1, 1, 1, '2026-11-02', '08:30:00', '10:00:00', N'Tập Vai & Bụng Core', 'Confirmed', N'Đẩy tạ vai 20kg mỗi bên', 1),
(6, 2, 1, 1, '2026-11-04', '14:00:00', '15:30:00', N'Tập Ngực & Tay Sau Nâng Cao', 'Confirmed', N'Giảm thời gian nghỉ giữa hiệp', 1),
(7, 1, 1, 1, '2026-11-06', '09:00:00', '10:30:00', N'Full Body Functional Training', 'Confirmed', N'Tập liên hoàn toàn thân', 1),
(8, 1, 1, 1, '2026-11-09', '08:30:00', '10:00:00', N'Kiểm Tra Tiến Độ & Strength Test', 'Scheduled', N'Đánh giá sức mạnh sau chu kỳ 1 tháng', 1),
(9, 3, 1, 1, '2026-11-11', '14:00:00', '15:30:00', N'Tập Lưng Xô & Bắp Tay', 'Cancelled', N'Hội viên báo bận lịch tác nghiệp công tác', 1),
(10, 2, 1, 2, '2026-11-13', '09:00:00', '10:30:00', N'Cardio Circuit & Core Stability', 'Scheduled', N'Bài tập sức bền và thăng bằng', 1),
(27, 1, 1, 1, '2026-11-16', '08:30:00', '10:00:00', N'Tập Ngực Chuyên Sâu & Flyes', 'Scheduled', N'Tập trung ép ngực với tạ đơn', 1),
(28, 2, 1, 2, '2026-11-18', '14:00:00', '15:30:00', N'HIIT Tabata & Bụng Dưới', 'Scheduled', N'Chú ý kiểm soát nhịp tim', 1),
(29, 1, 1, 1, '2026-11-20', '09:00:00', '10:30:00', N'Tập Lưng Dày & Deadlift', 'Scheduled', N'Kiểm tra kỹ thuật siết lưng', 1),
(30, 1, 1, 1, '2026-11-23', '08:30:00', '10:00:00', N'Tập Vai & Tay Sau (Super-set)', 'Pending', N'Hội viên mới gửi yêu cầu đặt lịch', 1),
(31, 3, 1, 1, '2026-11-25', '14:00:00', '15:30:00', N'Tập Chân & Đùi Trước', 'Scheduled', N'Leg Press & Leg Extension', 1),
(32, 1, 1, 1, '2026-11-27', '09:00:00', '10:30:00', N'Tổng Kết Tiến Độ & Giãn Cơ Sauna', 'Pending', N'Đo lại các vòng và chỉ số mỡ', 1),

-- PT 2 Lịch cũ
(11, 3, 2, 1, '2026-10-26', '16:00:00', '17:30:00', N'Tư Thế Squat & Cơ Đùi Nữ', 'Completed', N'Chỉnh sửa góc đầu gối khi xuống tạ', 1),
(12, 2, 2, 1, '2026-10-28', '17:30:00', '19:00:00', N'Tập Mông Quả Đào & Thon Gọn Eo', 'Completed', N'Hip Thrust 40kg, kết hợp dây kháng lực MiniBand', 1),
(13, 3, 2, 2, '2026-10-30', '16:00:00', '17:30:00', N'Pilates Dẻo Dai & Cột Sống', 'Completed', N'Kéo giãn giảm mỏi cổ vai gáy', 1),
(14, 2, 2, 1, '2026-11-03', '17:00:00', '18:30:00', N'Cardio Giảm Cân & Bụng Dưới', 'Confirmed', N'Tập Tabata 20s-10s và Plank', 1),
(15, 3, 2, 1, '2026-11-05', '16:00:00', '17:30:00', N'Tập Vai Nữ & Lưng Tự Tin', 'Confirmed', N'Dumbbell Lateral Raise 3kg', 1),
(16, 2, 2, 2, '2026-11-07', '17:30:00', '19:00:00', N'Kéo Giãn Cơ & Thư Giãn Toàn Thân', 'Confirmed', N'Foam rolling giải phóng điểm căng cơ', 1),
(17, 3, 2, 1, '2026-11-10', '16:00:00', '17:30:00', N'Legs Day & Glute Bridge', 'Scheduled', N'Rèn luyện cơ đùi sau và mông', 1),
(18, 2, 2, 1, '2026-11-12', '17:00:00', '18:30:00', N'Đốt Mỡ Kháng Lực Toàn Thân', 'Scheduled', N'Tập với tạ Kettlebell và bóng Slam Ball', 1),
(33, 3, 2, 1, '2026-11-17', '16:00:00', '17:30:00', N'Tập Mông & Đùi Trong Nữ', 'Scheduled', N'Sumo Squat & Cable Abduction', 1),
(34, 2, 2, 2, '2026-11-19', '17:00:00', '18:30:00', N'Tập Cốt Lõi (Core & Abs)', 'Scheduled', N'Chú ý nhịp thở khi gập bụng', 1),
(35, 3, 2, 1, '2026-11-24', '16:00:00', '17:30:00', N'Pilates Thể Lực & Dáng Đẹp', 'Pending', N'Đang chờ HLV xác nhận ca', 1),
(36, 2, 2, 1, '2026-11-26', '17:00:00', '18:30:00', N'Kéo Giãn Sâu & Yoga Gym', 'Scheduled', N'Thư giãn các cơ sau chu kỳ tập nặng', 1),

-- PT 3 Lịch cũ
(19, 1, 3, 3, '2026-10-27', '08:00:00', '09:30:00', N'Powerlifting Deadlift & Lưng Dày', 'Completed', N'Deadlift kỹ thuật Sumo, mức tạ 100kg', 2),
(20, 2, 3, 3, '2026-10-29', '10:00:00', '11:30:00', N'Bench Press Chuẩn Thi Đấu', 'Completed', N'Tập kỹ thuật khóa khớp vai và ép xương bả vai', 2),
(21, 1, 3, 3, '2026-10-31', '08:30:00', '10:00:00', N'Squat Nặng & Phục Hồi Khớp', 'Confirmed', N'Back Squat 80kg x 5 reps x 5 sets', 2),
(22, 3, 3, 3, '2026-11-03', '09:00:00', '10:30:00', N'Kỹ Thuật Cử Tạ Olympic Cơ Bản', 'Confirmed', N'Clean & Press tạ kỹ thuật 15kg', 2),
(23, 1, 3, 4, '2026-11-06', '10:00:00', '11:30:00', N'Conditioning & Thể Lực Đỉnh Cao', 'Confirmed', N'Đẩy xe trượt Prowler Sled & quất dây thừng Battle Rope', 2),
(24, 2, 3, 3, '2026-11-10', '08:30:00', '10:00:00', N'Tập Xô Lưng Tăng Độ Dày', 'Scheduled', N'Barbell Bent Over Row & T-Bar Row', 2),
(25, 1, 3, 3, '2026-11-12', '10:00:00', '11:30:00', N'Thử Thách 1RM Bench & Squat', 'Scheduled', N'Chuẩn bị khởi động dài và bảo hiểm an toàn', 2),
(26, 3, 3, 4, '2026-11-14', '09:00:00', '10:30:00', N'HIIT & Sức Bền Tim Mạch', 'Scheduled', N'Máy chèo thuyền kết hợp máy trượt tuyết', 2),
(37, 1, 3, 3, '2026-11-17', '08:30:00', '10:00:00', N'Deadlift Sumo & Kỹ Thuật Siết Lưng', 'Scheduled', N'Nâng mức tạ thử nghiệm 110kg', 2),
(38, 2, 3, 4, '2026-11-19', '10:00:00', '11:30:00', N'Overhead Press & Khớp Vai', 'Scheduled', N'Tập vai đẩy thanh đòn đứng', 2),
(39, 1, 3, 3, '2026-11-24', '08:30:00', '10:00:00', N'Powerlifting Complete Day', 'Pending', N'Lịch đăng ký mới cuối tháng', 2),
(40, 3, 3, 4, '2026-11-28', '09:00:00', '10:30:00', N'Cardio Thể Lực & Phục Hồi Co Cơ', 'Scheduled', N'Bài tập giải mỏi toàn thân', 2),

-- LỊCH TẬP PT NAM TỪ 27/11 ĐẾN 31/12/2026
(41, 2, 1, 2, '2026-11-28', '14:00:00', '15:30:00', N'Cardio HIIT & Siết Cơ Bụng', 'Confirmed', N'Kiểm tra nhịp tim và sức bền tim mạch', 1),
(42, 4, 1, 1, '2026-11-30', '09:00:00', '10:30:00', N'Tập Ngực & Tay Trước Kháng Lực', 'Confirmed', N'Hội viên bắt đầu lộ trình tăng cơ', 1),
(43, 1, 1, 1, '2026-12-02', '08:30:00', '10:00:00', N'Tập Chân Mông & Squat Nặng', 'Scheduled', N'Khởi động kỹ khớp gối và cơ đùi trước', 1),
(44, 2, 1, 1, '2026-12-04', '14:00:00', '15:30:00', N'Tập Lưng Xô & Kéo Xà Trợ Lực', 'Scheduled', N'Tập trung bài Lat Pulldown & Seated Cable Row', 1),
(45, 1, 1, 1, '2026-12-07', '08:30:00', '10:00:00', N'Deadlift Sumo & Lưng Dày', 'Scheduled', N'Tăng mức tạ lên 90kg, kiểm tra đai lưng', 1),
(46, 3, 1, 2, '2026-12-09', '15:00:00', '16:30:00', N'Kickfit Đốt Mỡ Cường Độ Cao', 'Pending', N'Rèn luyện phản xạ và đòn đấm thẳng', 1),
(47, 4, 1, 1, '2026-12-11', '09:00:00', '10:30:00', N'Tập Vai & Tay Sau Chuyên Sâu', 'Scheduled', N'Overhead Dumbbell Press & Dips', 1),
(48, 1, 1, 1, '2026-12-14', '08:30:00', '10:00:00', N'Leg Day - Leg Press & Hamstring Curl', 'Scheduled', N'Kéo giãn cơ đùi sau sau buổi tập', 1),
(49, 2, 1, 2, '2026-12-16', '14:00:00', '15:30:00', N'Functional Training & Thể Lực Core', 'Scheduled', N'Tập với bóng Slam Ball và dây thừng Battle Rope', 1),
(50, 5, 1, 1, '2026-12-18', '09:00:00', '10:30:00', N'Tập Toàn Thân Full Body & Giảm Mỡ', 'Pending', N'Lịch hẹn mới đăng ký chờ xác nhận', 1),
(51, 1, 1, 1, '2026-12-21', '08:30:00', '10:00:00', N'Bench Press Nặng & Ngực Trên Incline', 'Scheduled', N'Thử thách mức tạ 80kg có HLV bảo hiểm', 1),
(52, 3, 1, 2, '2026-12-23', '15:00:00', '16:30:00', N'Boxing Đón Giáng Sinh - Đấm Bao Cát', 'Scheduled', N'Tập tổ hợp đòn 1-2 Hook Uppercut', 1),
(53, 2, 1, 2, '2026-12-25', '09:00:00', '10:30:00', N'Kéo Giãn Cơ Chuyên Sâu & Thư Giãn Noel', 'Confirmed', N'Foam Rolling và giãn cơ tĩnh toàn thân', 1),
(54, 1, 1, 1, '2026-12-28', '08:30:00', '10:00:00', N'Đánh Giá Chỉ Số Thể Lực & Test Sức Mạnh', 'Scheduled', N'Kiểm tra 1RM và chụp ảnh form dáng cuối năm', 1),
(55, 4, 1, 1, '2026-12-30', '14:00:00', '15:30:00', N'Tổng Kết Chu Kỳ Tập Luyện Năm 2026', 'Scheduled', N'Đo đạc chỉ số và lên giáo án năm mới 2027', 1),
(56, 1, 1, 1, '2026-12-31', '09:00:00', '10:30:00', N'Buổi Tập Cuối Năm - Full Body Pump', 'Scheduled', N'Tập nhẹ nhàng xả stress sẵn sàng đón năm mới', 1),

-- Lịch tháng 12 của các PT khác
(57, 3, 2, 2, '2026-12-05', '16:00:00', '17:30:00', N'Pilates & Dáng Đẹp Cuối Năm', 'Scheduled', N'Kéo giãn cột sống', 1),
(58, 6, 3, 3, '2026-12-12', '09:00:00', '10:30:00', N'Powerlifting Squat Heavy Day', 'Scheduled', N'Mức tạ mục tiêu 120kg', 2),
(59, 7, 4, 4, '2026-12-19', '15:00:00', '16:30:00', N'Zumba Cardio Đốt Mỡ Noel', 'Scheduled', N'Nhạc Giáng Sinh sôi động', 2),
(60, 9, 5, 5, '2026-12-26', '10:00:00', '11:30:00', N'Kickfit Thể Lực & Phản Xạ Q7', 'Scheduled', N'Tập đối kháng có giáp bảo hộ', 3);
SET IDENTITY_INSERT dbo.Schedules OFF;
GO

-- ====================================================================================
-- M. BẢNG CheckIns: 24 lượt check-in
-- ====================================================================================
SET IDENTITY_INSERT dbo.CheckIns ON;
INSERT INTO dbo.CheckIns (CheckInID, MemberID, PTID, CheckInTime, Status, Note, BranchID) VALUES
(1, 1, 1, '2026-10-20 08:25:00', 'Approved', N'Check-in tập buổi ngực cùng PT Nam', 1),
(2, 2, 1, '2026-10-21 13:50:00', 'Approved', N'Check-in tập Cardio cùng PT Nam', 1),
(3, 3, 2, '2026-10-21 15:55:00', 'Approved', N'Check-in tập Pilates cùng PT Bích', 1),
(4, 1, 1, '2026-10-22 08:30:00', 'Approved', N'Check-in tập Lưng cùng PT Nam', 1),
(5, 4, NULL, '2026-10-22 17:15:00', 'Approved', N'Hội viên Classic tập tự do chiều', 1),
(6, 5, NULL, '2026-10-23 07:05:00', 'Approved', N'Hội viên Classic tập tự do sáng', 1),
(7, 2, 2, '2026-10-23 17:25:00', 'Approved', N'Check-in tập Mông & Đùi cùng PT Bích', 1),
(8, 3, 1, '2026-10-24 14:55:00', 'Approved', N'Check-in tập Chân cùng PT Nam', 1),
(9, 6, NULL, '2026-10-24 18:00:00', 'Approved', N'Hội viên Classic tập tự do chi nhánh Bình Thạnh', 2),
(10, 1, 3, '2026-10-25 07:55:00', 'Approved', N'Check-in tập Powerlifting cùng PT Minh', 2),
(11, 7, NULL, '2026-10-25 16:30:00', 'Approved', N'Hội viên Classic tập Cardio tự do', 2),
(12, 8, NULL, '2026-10-25 19:10:00', 'Approved', N'Hội viên Classic tập tối', 2),
(13, 9, NULL, '2026-10-25 06:45:00', 'Approved', N'Hội viên Classic tập tự do chi nhánh Q7', 3),
(14, 10, NULL, '2026-10-25 18:20:00', 'Approved', N'Hội viên Classic tập tối chi nhánh Q7', 3),
(15, 1, 1, '2026-10-26 08:20:00', 'Approved', N'Check-in buổi tập đầu tuần cùng PT Nam', 1),
(16, 1, 1, '2026-11-02 08:25:00', 'Approved', N'Check-in tập Vai & Bụng Core cùng PT Nam', 1),
(17, 2, 1, '2026-11-04 13:55:00', 'Approved', N'Check-in tập Ngực nâng cao cùng PT Nam', 1),
(18, 3, 2, '2026-11-05 15:55:00', 'Approved', N'Check-in tập Vai Nữ cùng PT Bích', 1),
(19, 1, 1, '2026-11-06 08:55:00', 'Approved', N'Check-in Full Body Functional cùng PT Nam', 1),
(20, 6, 3, '2026-11-10 08:20:00', 'Approved', N'Check-in Powerlifting cùng PT Minh', 2),
(21, 2, 1, '2026-11-18 13:50:00', 'Approved', N'Check-in HIIT Tabata cùng PT Nam', 1),
(22, 1, 1, '2026-11-20 08:55:00', 'Approved', N'Check-in Deadlift Lưng Dày cùng PT Nam', 1),
(23, 7, 4, '2026-11-24 18:25:00', 'Approved', N'Check-in lớp Zumba Dance cùng PT Thảo', 2),
(24, 1, 1, '2026-11-27 08:20:00', 'Approved', N'Check-in buổi tập sáng cùng PT Nam', 1);
SET IDENTITY_INSERT dbo.CheckIns OFF;
GO

-- ====================================================================================
-- N. BẢNG CheckInImages: 15 bản ghi
-- ====================================================================================
SET IDENTITY_INSERT dbo.CheckInImages ON;
INSERT INTO dbo.CheckInImages (ImageID, CheckInID, ImagePath, UploadedAt, VerifiedBy, VerificationNote) VALUES
(1, 1, 'uploads/checkin_001.jpg', '2026-10-20 08:26:00', 1, N'Ảnh khuôn mặt rõ nét, khớp hồ sơ'),
(2, 2, 'uploads/checkin_002.jpg', '2026-10-21 13:51:00', 1, N'Nhận diện khuôn mặt hợp lệ'),
(3, 3, 'uploads/checkin_003.jpg', '2026-10-21 15:56:00', 2, N'Xác thực hội viên thành công'),
(4, 4, 'uploads/checkin_004.jpg', '2026-10-22 08:31:00', 1, N'Hợp lệ'),
(5, 5, 'uploads/checkin_005.jpg', '2026-10-22 17:16:00', 1, N'Check-in camera sảnh lễ tân'),
(6, 6, 'uploads/checkin_006.jpg', '2026-10-23 07:06:00', 1, N'Check-in camera sảnh lễ tân'),
(7, 7, 'uploads/checkin_007.jpg', '2026-10-23 17:26:00', 2, N'Hội viên Bích phụ trách'),
(8, 8, 'uploads/checkin_008.jpg', '2026-10-24 14:56:00', 1, N'Xác nhận đúng lịch hẹn'),
(9, 9, 'uploads/checkin_009.jpg', '2026-10-24 18:01:00', 3, N'Check-in cửa từ Bình Thạnh'),
(10, 10, 'uploads/checkin_010.jpg', '2026-10-25 07:56:00', 3, N'PT Minh duyệt check-in'),
(11, 11, 'uploads/checkin_011.jpg', '2026-10-25 16:31:00', 4, N'Cửa từ hợp lệ'),
(12, 12, 'uploads/checkin_012.jpg', '2026-10-25 19:11:00', 4, N'Cửa từ hợp lệ'),
(13, 13, 'uploads/checkin_013.jpg', '2026-10-25 06:46:00', 5, N'Lễ tân chi nhánh Q7 duyệt'),
(14, 14, 'uploads/checkin_014.jpg', '2026-10-25 18:21:00', 5, N'Cửa từ Q7 hợp lệ'),
(15, 15, 'uploads/checkin_015.jpg', '2026-10-26 08:21:00', 1, N'Ảnh selfie check-in tại quầy Q1');
SET IDENTITY_INSERT dbo.CheckInImages OFF;
GO

-- ====================================================================================
-- O. BẢNG Equipments: 10 thiết bị máy tập
-- ====================================================================================
SET IDENTITY_INSERT dbo.Equipments ON;
INSERT INTO dbo.Equipments (EquipmentID, RoomID, EquipmentName, EquipmentCode, PurchaseDate, Price, Status, Description) VALUES
(1, 2, N'Máy chạy bộ Technogym Excite Live', 'TM-01', '2025-06-15', 35000000, N'Tốt', N'Máy chạy bộ cao cấp màn hình cảm ứng'),
(2, 1, N'Dàn gánh tạ Squat Rack Olympic', 'SQ-01', '2025-06-20', 18000000, N'Tốt', N'Khung gánh tạ an toàn chịu tải 500kg'),
(3, 1, N'Ghế đẩy ngực Incline Bench Press', 'BP-01', '2025-07-05', 12000000, N'Tốt', N'Ghế dốc tập ngực trên chuẩn thi đấu'),
(4, 1, N'Máy kéo xô Lat Pulldown & Row', 'LP-01', '2025-07-10', 28000000, N'Tốt', N'Máy khối đôi kéo xô và chèo xô lưng'),
(5, 2, N'Xe đạp tập Cardio LifeFitness', 'BK-01', '2025-08-01', 16000000, N'Tốt', N'Xe đạp tĩnh kháng lực từ trường'),
(6, 3, N'Máy đạp đùi Leg Press 45 độ', 'LP-02', '2025-08-15', 32000000, N'Tốt', N'Máy đạp chân cơ đùi chịu tải trọng lớn'),
(7, 3, N'Dàn cáp đôi Dual Cable Crossover', 'CC-01', '2025-09-01', 42000000, N'Tốt', N'Dàn cáp đôi đa hướng ròng rọc nhôm'),
(8, 4, N'Máy trượt tuyết Elliptical Matrix', 'EL-01', '2025-09-15', 26000000, N'Tốt', N'Máy tập toàn thân bảo vệ khớp gối'),
(9, 5, N'Bộ tạ tay Dumbbell 5-40kg kèm giá', 'DB-01', '2025-10-01', 45000000, N'Tốt', N'Dàn tạ bọc urethene cao cấp 20 đôi'),
(10, 6, N'Máy chèo thuyền WaterRower', 'RW-01', '2025-10-15', 22000000, N'Tốt', N'Máy chèo thuyền cản nước bằng gỗ sồi');
SET IDENTITY_INSERT dbo.Equipments OFF;
GO

-- ====================================================================================
-- P. BẢNG EquipmentMaintenances: Bảo trì thiết bị FULL 12 THÁNG NĂM 2026
-- ====================================================================================
SET IDENTITY_INSERT dbo.EquipmentMaintenances ON;
INSERT INTO dbo.EquipmentMaintenances (MaintenanceID, EquipmentID, MaintenanceDate, NextMaintenanceDate, Description, Cost, Status) VALUES
(1, 4, '2026-01-10', '2026-07-10', N'Thay dây cáp thép bọc nylon và bôi trơn ròng rọc giàn kéo xô', 750000, N'Hoàn thành'),
(2, 5, '2026-02-15', '2026-08-15', N'Thay bàn đạp dây đai và nối lại cảm biến nhịp tim xe đạp tập', 600000, N'Hoàn thành'),
(3, 7, '2026-03-12', '2026-09-12', N'Căn chỉnh lại hệ thống đối trọng tạ cáp bên phải dàn cáp đôi', 800000, N'Hoàn thành'),
(4, 6, '2026-04-18', '2026-10-18', N'Tra mỡ vòng bi ray trượt và chỉnh chốt an toàn máy Leg Press', 450000, N'Hoàn thành'),
(5, 8, '2026-05-20', '2026-11-20', N'Bảo trì mô-tơ kháng lực từ tính máy trượt tuyết Elliptical', 900000, N'Hoàn thành'),
(6, 1, '2026-06-08', '2026-12-08', N'Kiểm tra động cơ 4.0 HP và căn chỉnh độ lệch băng tải máy chạy bộ', 650000, N'Hoàn thành'),
(7, 2, '2026-07-14', '2027-01-14', N'Bọc lại đệm mút simili cao cấp và siết ốc dàn khung giàn Squat', 550000, N'Hoàn thành'),
(8, 3, '2026-08-22', '2027-02-22', N'Cân chỉnh độ dốc ghế đa năng và kiểm tra khóa an toàn J-Hooks', 350000, N'Hoàn thành'),
(9, 9, '2026-09-15', '2027-03-15', N'Sơn phủ chống rỉ và dùng keo khóa ren siết chặt đầu tạ tay cao su', 400000, N'Hoàn thành'),
(10, 10, '2026-10-20', '2027-04-20', N'Thay nước bình chứa và làm sạch bánh đà máy chèo thuyền', 350000, N'Hoàn thành'),
(11, 7, '2026-11-15', '2027-05-15', N'Thay thế chốt tạ selector pin bị kẹt và bảo dưỡng bánh xe dẫn hướng', 500000, N'Hoàn thành'),
(12, 1, '2026-12-10', '2027-06-10', N'Bảo dưỡng tổng thể động cơ, thay chổi than và tra dầu silicon máy chạy', 850000, N'Hoàn thành');
SET IDENTITY_INSERT dbo.EquipmentMaintenances OFF;
GO

-- ====================================================================================
-- Q. BẢNG Incidents: 12 sự cố thiết bị trải dài năm 2026
-- ====================================================================================
SET IDENTITY_INSERT dbo.Incidents ON;
INSERT INTO dbo.Incidents (IncidentID, EquipmentID, ReportedBy, ReportDate, Description, Status, ResolvedDate, Note) VALUES
(1, 1, 1, '2026-02-10', N'Máy chạy bộ phát ra tiếng kêu lạ ở băng tải khi đạt tốc độ trên 12km/h', 'Resolved', '2026-02-11', N'Đã siết lại ốc và căn chỉnh tâm băng chuyền'),
(2, 3, 2, '2026-03-12', N'Đệm da ghế đẩy ngực bị rách nhẹ ở góc mép', 'Resolved', '2026-03-14', N'Đã bọc lại lớp da simili cao cấp mới'),
(3, 4, 2, '2026-04-05', N'Cáp kéo xô có hiện tượng sờn vỏ nhựa bảo vệ', 'Resolved', '2026-04-06', N'Đã thay cáp mới an toàn tuyệt đối'),
(4, 5, 3, '2026-05-20', N'Màn hình xe đạp không nhận tín hiệu nhịp tim từ tay cầm', 'Resolved', '2026-05-21', N'Đã nối lại dây cảm biến bị tuột bên trong'),
(5, 6, 4, '2026-06-18', N'Khóa chốt an toàn máy Leg Press bị rít khó gạt', 'Resolved', '2026-06-19', N'Đã tra dầu bôi trơn và vệ sinh trục chốt'),
(6, 7, 4, '2026-07-22', N'Ròng rọc dàn cáp đôi xoay không đều gây cọ xát', 'Resolved', '2026-07-23', N'Đã thay vòng bi ròng rọc bên trái'),
(7, 8, 5, '2026-08-30', N'Bàn đạp máy trượt tuyết phát ra tiếng cót két khi đạp nhanh', 'Resolved', '2026-08-31', N'Đã siết ốc trục cam và tra mỡ chịu nhiệt'),
(8, 9, 1, '2026-09-14', N'Quả tạ 15kg bị lỏng ốc đầu ren cao su', 'Resolved', '2026-09-15', N'Đã dùng keo khóa ren vặn chặt cố định'),
(9, 7, 4, '2026-10-22', N'Chốt chọn mức tạ bị cong nhẹ do hội viên thả tạ rơi tự do', 'Resolved', '2026-11-15', N'Đã thay chốt tạ selector pin mới chính hãng'),
(10, 10, 6, '2026-10-25', N'Dây kéo tay cầm máy chèo thuyền bị xoắn nhẹ cần căn lại', 'Resolved', '2026-10-26', N'Đã tháo căn chỉnh và tra dầu mượt mà'),
(11, 5, 2, '2026-11-20', N'Yên xe đạp tập xoay bị lỏng ốc vít nâng hạ', 'Resolved', '2026-11-21', N'Kỹ thuật viên đã thay núm vặn khóa chốt mới'),
(12, 1, 1, '2026-11-26', N'Thảm máy chạy bộ 1 bị trượt nhẹ khi chạy tốc độ cao', 'In Progress', NULL, N'Đang lên lịch bảo dưỡng tổng thể trong tháng 12');
SET IDENTITY_INSERT dbo.Incidents OFF;
GO

-- ====================================================================================
-- R. BẢNG Progresses: 12 dữ liệu thể chất InBody hội viên
-- ====================================================================================
SET IDENTITY_INSERT dbo.Progresses ON;
INSERT INTO dbo.Progresses (ProgressID, MemberID, PTID, RecordDate, Height, Weight, BodyFat, MuscleMass, BMI, Chest, Waist, Hip, Arm, Thigh, Note) VALUES
(1, 1, 1, '2026-01-05', 175.0, 78.0, 22.0, 33.5, 25.5, 96.0, 84.0, 98.0, 34.0, 56.0, N'Chỉ số ban đầu bắt đầu tập với PT Nam'),
(2, 1, 1, '2026-03-05', 175.0, 76.0, 19.5, 34.2, 24.8, 98.0, 81.0, 97.0, 35.5, 57.0, N'Giảm 2kg mỡ, tăng cơ tay và ngực rõ rệt'),
(3, 1, 1, '2026-06-05', 175.0, 74.0, 17.0, 35.0, 24.2, 101.0, 78.0, 96.0, 36.5, 58.0, N'Thể trạng săn chắc, form siết nét'),
(4, 1, 1, '2026-10-15', 175.0, 72.5, 15.5, 35.8, 23.7, 103.0, 76.0, 95.0, 37.5, 58.5, N'Đạt mục tiêu thể hình Lean Muscle mong muốn'),
(5, 2, 1, '2026-02-05', 162.0, 58.0, 28.0, 22.0, 22.1, 86.0, 72.0, 92.0, 26.0, 52.0, N'Bắt đầu lộ trình giảm mỡ toàn thân'),
(6, 2, 1, '2026-05-10', 162.0, 54.5, 24.5, 23.0, 20.8, 85.0, 67.0, 90.0, 25.0, 50.0, N'Vòng eo giảm 5cm, sức bền tăng tốt'),
(7, 2, 2, '2026-09-20', 162.0, 52.0, 21.0, 23.8, 19.8, 85.0, 64.0, 89.0, 24.5, 49.0, N'Body cân đối, cơ bụng bắt đầu hiện rãnh 11'),
(8, 3, 2, '2026-03-05', 168.0, 62.0, 26.0, 25.0, 22.0, 88.0, 70.0, 94.0, 27.0, 53.0, N'Bắt đầu tập định hình vóc dáng cùng PT Bích'),
(9, 3, 2, '2026-07-15', 168.0, 58.5, 22.5, 26.2, 20.7, 89.0, 66.0, 94.0, 26.5, 52.0, N'Độ dẻo dai và thể lực cải thiện vượt bậc'),
(10, 6, 3, '2026-03-10', 178.0, 82.0, 24.0, 35.0, 25.9, 100.0, 88.0, 101.0, 35.0, 59.0, N'Tập tăng cơ giảm mỡ ban đầu'),
(11, 1, 1, '2026-11-20', 175.0, 72.0, 14.8, 36.2, 23.5, 104.0, 75.0, 95.0, 38.0, 59.0, N'Chỉ số thể hình đạt chuẩn vận động viên, cơ bắp sắc nét'),
(12, 2, 1, '2026-11-22', 162.0, 51.5, 20.0, 24.0, 19.6, 85.0, 63.0, 89.0, 24.0, 48.5, N'Vòng eo đạt chuẩn thon gọn lý tưởng, thể lực rất dẻo dai');
SET IDENTITY_INSERT dbo.Progresses OFF;
GO

-- ====================================================================================
-- BẬT LẠI RÀNG BUỘC KHÓA NGOẠI
-- ====================================================================================
EXEC sp_MSforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all";
GO

PRINT N'====================================================================================';
PRINT N'CẬP NHẬT DATABASE VÀ SEED DATA HOÀN TẤT THÀNH CÔNG! (BẢNG __EFMigrationsHistory NGUYÊN VẸN)';
PRINT N'====================================================================================';
GO
