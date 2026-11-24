-- ====================================================================================
-- SCRIPT CẬP NHẬT DATABASE QUẢN LÝ GYM THEO YÊU CẦU
-- LƯU Ý QUAN TRỌNG: Script KHÔNG tác động vào bảng __EFMigrationsHistory!
-- ====================================================================================

USE [QuanLyGymFullDB];
GO

-- 1. XÓA BẢNG WorkingSchedules NẾU TỒN TẠI (Yêu cầu 8)
IF OBJECT_ID('dbo.WorkingSchedules', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.WorkingSchedules;
    PRINT N'>> Đã xóa bảng WorkingSchedules.';
END
GO

-- 2. TẠO BẢNG Staffs (NHÂN VIÊN: PT, Tạp vụ, Thu ngân, Bảo vệ) (Yêu cầu 1)
IF OBJECT_ID('dbo.Staffs', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Staffs (
        StaffID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        FullName NVARCHAR(100) NOT NULL,
        Role NVARCHAR(50) NOT NULL, -- PT, Tạp vụ, Thu ngân, Bảo vệ
        Gender NVARCHAR(10) NULL,
        DateOfBirth DATETIME2(7) NOT NULL,
        Phone NVARCHAR(15) NULL,
        Email NVARCHAR(100) NULL,
        WorkShift NVARCHAR(100) NOT NULL, -- Ca làm việc (PT ca cố định 7 ngày)
        BaseSalary DECIMAL(18,2) NOT NULL, -- Lương cơ bản
        Status BIT NOT NULL DEFAULT 1,
        BranchID INT NULL FOREIGN KEY REFERENCES dbo.Branches(BranchID),
        PTID INT NULL FOREIGN KEY REFERENCES dbo.PTs(PTID) ON DELETE SET NULL
    );
    PRINT N'>> Đã tạo bảng Staffs.';
END
GO

-- 3. THÊM TRƯỜNG IsPTPackage VÀO BẢNG Packages (Yêu cầu 7)
IF COL_LENGTH('dbo.Packages', 'IsPTPackage') IS NULL
BEGIN
    ALTER TABLE dbo.Packages ADD IsPTPackage BIT NOT NULL DEFAULT 0;
    PRINT N'>> Đã thêm cột IsPTPackage vào bảng Packages.';
END
GO

-- 4. LOẠI BỎ CÁC CHỈ SỐ CƠ THỂ RA KHỎI BẢNG Members (Yêu cầu 2)
IF COL_LENGTH('dbo.Members', 'Height') IS NOT NULL
BEGIN
    ALTER TABLE dbo.Members DROP COLUMN Height;
    PRINT N'>> Đã bỏ cột Height khỏi Members.';
END
IF COL_LENGTH('dbo.Members', 'Weight') IS NOT NULL
BEGIN
    ALTER TABLE dbo.Members DROP COLUMN Weight;
    PRINT N'>> Đã bỏ cột Weight khỏi Members.';
END
IF COL_LENGTH('dbo.Members', 'BodyFat') IS NOT NULL
BEGIN
    ALTER TABLE dbo.Members DROP COLUMN BodyFat;
    PRINT N'>> Đã bỏ cột BodyFat khỏi Members.';
END
IF COL_LENGTH('dbo.Members', 'MuscleMass') IS NOT NULL
BEGIN
    ALTER TABLE dbo.Members DROP COLUMN MuscleMass;
    PRINT N'>> Đã bỏ cột MuscleMass khỏi Members.';
END
IF COL_LENGTH('dbo.Members', 'BMI') IS NOT NULL
BEGIN
    ALTER TABLE dbo.Members DROP COLUMN BMI;
    PRINT N'>> Đã bỏ cột BMI khỏi Members.';
END
GO

-- ====================================================================================
-- 5. XÓA DỮ LIỆU CŨ VÀ NẠP DỮ LIỆU MỚI (THEO ĐÚNG THỨ TỰ RÀNG BUỘC KHÓA NGOẠI)
-- ====================================================================================

-- Tắt tạm ràng buộc khóa ngoại để nạp data an toàn
EXEC sp_MSforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all";
GO

-- Xóa dữ liệu các bảng (trừ __EFMigrationsHistory)
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
-- A. BẢNG Branches: Chỉ có 3 chi nhánh ở TP.HCM (Yêu cầu 3)
-- ====================================================================================
SET IDENTITY_INSERT dbo.Branches ON;
INSERT INTO dbo.Branches (BranchID, BranchName, Address, Phone, Email, OpenTime, CloseTime, Status) VALUES
(1, N'QuanLyGym Quận 1', N'123 Nguyễn Thị Minh Khai, Phường Bến Thành, Quận 1, TP. HCM', '02838221122', 'quan1@quanlygym.vn', '06:00:00', '22:00:00', 1),
(2, N'QuanLyGym Bình Thạnh', N'456 Điện Biên Phủ, Phường 25, Quận Bình Thạnh, TP. HCM', '02838332233', 'binhthanh@quanlygym.vn', '06:00:00', '22:00:00', 1),
(3, N'QuanLyGym Quận 7', N'789 Nguyễn Lương Bằng, Phường Tân Phú, Quận 7, TP. HCM', '02838443344', 'quan7@quanlygym.vn', '06:00:00', '22:00:00', 1);
SET IDENTITY_INSERT dbo.Branches OFF;
GO

-- ====================================================================================
-- B. BẢNG Accounts: Chỉ có 3 loại là Admin, Member và PT (Yêu cầu 1)
-- ====================================================================================
SET IDENTITY_INSERT dbo.Accounts ON;
INSERT INTO dbo.Accounts (AccountID, Username, PasswordHash, Role, Status, CreatedAt) VALUES
-- 1 Admin
(1, 'admin', '123456', 'Admin', 1, '2026-01-01'),
-- 5 PTs
(2, 'pt_nam', '123456', 'PT', 1, '2026-01-01'),
(3, 'pt_bichtran', '123456', 'PT', 1, '2026-01-01'),
(4, 'pt_ducminh', '123456', 'PT', 1, '2026-01-01'),
(5, 'pt_thaophan', '123456', 'PT', 1, '2026-01-01'),
(6, 'pt_hoanglong', '123456', 'PT', 1, '2026-01-01'),
-- 10 Members
(7, 'member_hung', '123456', 'Member', 1, '2026-01-01'),
(8, 'member_lan', '123456', 'Member', 1, '2026-01-01'),
(9, 'member_hoa', '123456', 'Member', 1, '2026-01-01'),
(10, 'member_dung', '123456', 'Member', 1, '2026-01-01'),
(11, 'member_phuong', '123456', 'Member', 1, '2026-01-01'),
(12, 'member_tuan', '123456', 'Member', 1, '2026-01-01'),
(13, 'member_mai', '123456', 'Member', 1, '2026-01-01'),
(14, 'member_vy', '123456', 'Member', 1, '2026-01-01'),
(15, 'member_khoa', '123456', 'Member', 1, '2026-01-01'),
(16, 'member_tram', '123456', 'Member', 1, '2026-01-01');
SET IDENTITY_INSERT dbo.Accounts OFF;
GO

-- ====================================================================================
-- C. BẢNG PTs: 5 PT chia không đều cho 3 chi nhánh (Q1: 2, Bình Thạnh: 2, Q7: 1) (Yêu cầu 4)
-- ====================================================================================
SET IDENTITY_INSERT dbo.PTs ON;
INSERT INTO dbo.PTs (PTID, AccountID, FullName, Gender, DateOfBirth, Phone, Email, Specialization, Experience, Status, BranchID) VALUES
(1, 2, N'Nguyễn Văn Nam', N'Nam', '1995-05-20', '0988888888', 'nam.nguyen@quanlygym.vn', N'Tăng cơ Hypertrophy & Dinh dưỡng', 6, 1, 1),
(2, 3, N'Trần Thị Bích', N'Nữ', '1997-08-14', '0943210987', 'bich.tran@quanlygym.vn', N'Giảm mỡ Siết cơ & Pilates/Yoga', 5, 1, 1),
(3, 4, N'Nguyễn Đức Minh', N'Nam', '1994-03-25', '0987543210', 'minh.duc@quanlygym.vn', N'Powerlifting & Thể hình chuyên nghiệp', 7, 1, 2),
(4, 5, N'Phan Thị Thảo', N'Nữ', '1998-11-12', '0921098765', 'thao.phan@quanlygym.vn', N'Cardio HIIT & Định hình vóc dáng', 4, 1, 2),
(5, 6, N'Vũ Hoàng Long', N'Nam', '1996-09-08', '0909876543', 'long.vu@quanlygym.vn', N'Phục hồi chức năng & Vận động', 5, 1, 3);
SET IDENTITY_INSERT dbo.PTs OFF;
GO

-- ====================================================================================
-- D. BẢNG Members: 10 hội viên chia không đều cho 3 chi nhánh (Q1: 5, Bình Thạnh: 3, Q7: 2)
-- ĐÃ LOẠI BỎ CHỈ SỐ CƠ THỂ KHỎI BẢNG (Yêu cầu 2, 4)
-- ====================================================================================
SET IDENTITY_INSERT dbo.Members ON;
INSERT INTO dbo.Members (MemberID, AccountID, FullName, Gender, DateOfBirth, Phone, Email, Address, Avatar, CreatedAt, BranchID) VALUES
-- Branch 1: 5 hội viên
(1, 7, N'Trần Văn Hùng', N'Nam', '2000-10-10', '0977777777', 'hung.tran@gmail.com', N'15 Lê Lợi, Quận 1, TP. HCM', '', '2026-01-01', 1),
(2, 8, N'Lê Thị Lan', N'Nữ', '1999-04-15', '0912345678', 'lan.le@gmail.com', N'88 Pasteur, Quận 1, TP. HCM', '', '2026-01-05', 1),
(3, 9, N'Phạm Quỳnh Hoa', N'Nữ', '2001-08-22', '0934567890', 'hoa.pham@gmail.com', N'12 Nguyễn Huệ, Quận 1, TP. HCM', '', '2026-01-10', 1),
(4, 10, N'Hoàng Tiến Dũng', N'Nam', '1996-12-05', '0945678901', 'dung.hoang@gmail.com', N'204 Hai Bà Trưng, Quận 1, TP. HCM', '', '2026-01-15', 1),
(5, 11, N'Đỗ Thu Phương', N'Nữ', '1998-03-30', '0956789012', 'phuong.do@gmail.com', N'55 Nam Kỳ Khởi Nghĩa, Quận 1, TP. HCM', '', '2026-01-20', 1),
-- Branch 2: 3 hội viên
(6, 12, N'Ngô Anh Tuấn', N'Nam', '1995-07-18', '0967890123', 'tuan.ngo@gmail.com', N'32 Bạch Đằng, Quận Bình Thạnh, TP. HCM', '', '2026-02-01', 2),
(7, 13, N'Bùi Tuyết Mai', N'Nữ', '2002-02-14', '0978901234', 'mai.bui@gmail.com', N'150 Nơ Trang Long, Quận Bình Thạnh, TP. HCM', '', '2026-02-05', 2),
(8, 14, N'Trịnh Thảo Vy', N'Nữ', '1997-09-09', '0989012345', 'vy.trinh@gmail.com', N'82 Xô Viết Nghệ Tĩnh, Quận Bình Thạnh, TP. HCM', '', '2026-02-10', 2),
-- Branch 3: 2 hội viên
(9, 15, N'Đặng Đăng Khoa', N'Nam', '1993-11-28', '0990123456', 'khoa.dang@gmail.com', N'100 Huỳnh Tấn Phát, Quận 7, TP. HCM', '', '2026-02-15', 3),
(10, 16, N'Võ Bảo Trâm', N'Nữ', '2000-06-16', '0901234567', 'tram.vo@gmail.com', N'45 Nguyễn Thị Thập, Quận 7, TP. HCM', '', '2026-02-20', 3);
SET IDENTITY_INSERT dbo.Members OFF;
GO

-- ====================================================================================
-- E. BẢNG Packages: Chỉ có 4 gói tập (Classic, Silver, Gold, VIP Diamond)
-- IsPTPackage: 1 là Gói tập với PT, 0 là Gói tập tự do (Yêu cầu 7)
-- ====================================================================================
SET IDENTITY_INSERT dbo.Packages ON;
INSERT INTO dbo.Packages (PackageID, PackageName, Duration, Price, TotalSessions, IsPTPackage, Benefits, Description, Status, BranchID) VALUES
(1, N'Classic', 30, 500000, 30, 0, N'Tập tự do không giới hạn các khung giờ, tủ đồ cá nhân', N'Gói tập tự do tiêu chuẩn', 1, 1),
(2, N'Silver', 90, 2800000, 24, 1, N'Kèm PT 1-1 (12 buổi PT), kiểm tra chỉ số InBody định kỳ', N'Gói tập rèn luyện chuyên sâu cùng HLV cá nhân', 1, 1),
(3, N'Gold', 180, 5800000, 48, 1, N'Kèm PT 1-1 (36 buổi PT), đo InBody định kỳ, phòng xông hơi Sauna', N'Gói tập Gold cao cấp toàn diện', 1, 1),
(4, N'VIP Diamond', 365, 12500000, 100, 1, N'Kèm PT 1-1 không giới hạn, full dịch vụ VIP Sauna, khăn tập & locker riêng', N'Gói tập thượng lưu VIP Diamond đẳng cấp nhất', 1, 1);
SET IDENTITY_INSERT dbo.Packages OFF;
GO

-- ====================================================================================
-- F. BẢNG Memberships: 3 hội viên đăng ký Silver, Gold, VIP Diamond tập cùng PT; các hội viên khác Classic (Yêu cầu 7)
-- ====================================================================================
SET IDENTITY_INSERT dbo.Memberships ON;
INSERT INTO dbo.Memberships (MembershipID, MemberID, PackageID, RegisterDate, StartDate, EndDate, Price, UsedSessions, Status, CreatedAt, BranchID) VALUES
(1, 1, 4, '2026-01-01', '2026-01-01', '2027-01-01', 12500000, 25, 'Active', '2026-01-01', 1), -- Member 1 tập VIP Diamond với PT
(2, 2, 3, '2026-02-01', '2026-02-01', '2026-08-01', 5800000, 18, 'Active', '2026-02-01', 1),   -- Member 2 tập Gold với PT
(3, 3, 2, '2026-03-01', '2026-03-01', '2026-06-01', 2800000, 10, 'Active', '2026-03-01', 1),   -- Member 3 tập Silver với PT
(4, 4, 1, '2026-01-15', '2026-01-15', '2026-04-15', 500000, 14, 'Active', '2026-01-15', 1),    -- Member 4 Classic tự do
(5, 5, 1, '2026-02-01', '2026-02-01', '2026-05-01', 500000, 12, 'Active', '2026-02-01', 1),    -- Member 5 Classic tự do
(6, 6, 1, '2026-02-01', '2026-02-01', '2026-05-01', 500000, 8, 'Active', '2026-02-01', 2),     -- Member 6 Classic tự do
(7, 7, 1, '2026-02-10', '2026-02-10', '2026-05-10', 500000, 6, 'Active', '2026-02-10', 2),     -- Member 7 Classic tự do
(8, 8, 1, '2026-02-15', '2026-02-15', '2026-05-15', 500000, 15, 'Active', '2026-02-15', 2),    -- Member 8 Classic tự do
(9, 9, 1, '2026-02-20', '2026-02-20', '2026-05-20', 500000, 9, 'Active', '2026-02-20', 3),     -- Member 9 Classic tự do
(10, 10, 1, '2026-03-01', '2026-03-01', '2026-06-01', 500000, 5, 'Active', '2026-03-01', 3);   -- Member 10 Classic tự do
SET IDENTITY_INSERT dbo.Memberships OFF;
GO

-- ====================================================================================
-- G. BẢNG Staffs: 14 nhân viên (5 PT, 3 Thu ngân, 3 Tạp vụ, 3 Bảo vệ)
-- Có trường Ca làm việc, Lương cơ bản. PT có ca cố định 7 ngày (Yêu cầu 1, 8)
-- ====================================================================================
SET IDENTITY_INSERT dbo.Staffs ON;
INSERT INTO dbo.Staffs (StaffID, FullName, Role, Gender, DateOfBirth, Phone, Email, WorkShift, BaseSalary, Status, BranchID, PTID) VALUES
-- 5 PT ca cố định 7 ngày trong tuần
(1, N'Nguyễn Văn Nam', N'PT', N'Nam', '1995-05-20', '0988888888', 'nam.nguyen@quanlygym.vn', N'Ca cố định: 08:00 - 16:00 (Thứ 2 - CN)', 16000000, 1, 1, 1),
(2, N'Trần Thị Bích', N'PT', N'Nữ', '1997-08-14', '0943210987', 'bich.tran@quanlygym.vn', N'Ca cố định: 14:00 - 22:00 (Thứ 2 - CN)', 15500000, 1, 1, 2),
(3, N'Nguyễn Đức Minh', N'PT', N'Nam', '1994-03-25', '0987543210', 'minh.duc@quanlygym.vn', N'Ca cố định: 08:00 - 16:00 (Thứ 2 - CN)', 16500000, 1, 2, 3),
(4, N'Phan Thị Thảo', N'PT', N'Nữ', '1998-11-12', '0921098765', 'thao.phan@quanlygym.vn', N'Ca cố định: 14:00 - 22:00 (Thứ 2 - CN)', 14500000, 1, 2, 4),
(5, N'Vũ Hoàng Long', N'PT', N'Nam', '1996-09-08', '0909876543', 'long.vu@quanlygym.vn', N'Ca cố định: 09:00 - 17:00 (Thứ 2 - CN)', 15000000, 1, 3, 5),
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
-- H. BẢNG Rooms: 6 phòng tập phân bổ 3 chi nhánh (Yêu cầu 10)
-- ====================================================================================
SET IDENTITY_INSERT dbo.Rooms ON;
INSERT INTO dbo.Rooms (RoomID, RoomName, RoomType, Capacity, Description, Status, BranchID) VALUES
(1, N'Phòng Gym & Tạ Tầng 1', 'Gym', 60, N'Khu vực tập tạ tổng hợp và máy khối', 1, 1),
(2, N'Phòng Cardio Tầng 2', 'Cardio', 40, N'Máy chạy bộ, xe đạp và máy trượt tuyết', 1, 1),
(3, N'Phòng Gym Free Weight Tầng 1', 'Gym', 50, N'Khu tạ tự do và dàn Squat/Bench', 1, 2),
(4, N'Phòng Cardio & Functional Tầng 2', 'Functional', 35, N'Khu chức năng và tim mạch', 1, 2),
(5, N'Phòng Strength & Conditioning', 'Gym', 45, N'Khu tập thể lực và tạ khối', 1, 3),
(6, N'Phòng Studio & Cardio Tầng 2', 'Cardio', 30, N'Khu cardio và tập studio', 1, 3);
SET IDENTITY_INSERT dbo.Rooms OFF;
GO

-- ====================================================================================
-- I. BẢNG Schedules: Lịch tập cho 3 PT (id 1, 2, 3) trong 1 2 3 tuần tới (Yêu cầu 9)
-- Mốc hiện tại: 26/10/2026. Lịch kéo dài từ 26/10 đến 14/11/2026 với 26 buổi tập chi tiết
-- ====================================================================================
SET IDENTITY_INSERT dbo.Schedules ON;
INSERT INTO dbo.Schedules (ScheduleID, MemberID, PTID, RoomID, TrainingDate, StartTime, EndTime, TrainingType, Status, Note, BranchID) VALUES
-- PT 1: Tuần 1, 2, 3
(1, 1, 1, 1, '2026-10-26', '08:30:00', '10:00:00', N'Tập Ngực & Tay Sau (Hypertrophy)', 'Confirmed', N'Hội viên chuẩn bị thể lực tốt, mức tạ ngực 60kg', 1),
(2, 2, 1, 2, '2026-10-27', '14:00:00', '15:30:00', N'Cardio HIIT & Đốt Mỡ Chuyên Sâu', 'Confirmed', N'Đốt calo cường độ cao, theo dõi nhịp tim', 1),
(3, 1, 1, 1, '2026-10-29', '09:00:00', '10:30:00', N'Tập Lưng Xô & Tay Trước', 'Scheduled', N'Tập trung bài Lat Pulldown & Seated Row', 1),
(4, 3, 1, 1, '2026-10-31', '15:00:00', '16:30:00', N'Tập Chân Mông & Giãn Cơ', 'Scheduled', N'Khởi động kỹ khớp gối', 1),
(5, 1, 1, 1, '2026-11-02', '08:30:00', '10:00:00', N'Tập Vai & Bụng Core', 'Scheduled', N'Đẩy tạ vai 20kg mỗi bên', 1),
(6, 2, 1, 1, '2026-11-04', '14:00:00', '15:30:00', N'Tập Ngực & Tay Sau Nâng Cao', 'Scheduled', N'Giảm thời gian nghỉ giữa hiệp', 1),
(7, 1, 1, 1, '2026-11-06', '09:00:00', '10:30:00', N'Full Body Functional Training', 'Scheduled', N'Tập liên hoàn toàn thân', 1),
(8, 1, 1, 1, '2026-11-09', '08:30:00', '10:00:00', N'Kiểm Tra Tiến Độ & Strength Test', 'Scheduled', N'Đánh giá sức mạnh sau chu kỳ 1 tháng', 1),
(9, 3, 1, 1, '2026-11-11', '14:00:00', '15:30:00', N'Tập Lưng Xô & Bắp Tay', 'Scheduled', N'Dùng đai hỗ trợ lưng', 1),
(10, 2, 1, 2, '2026-11-13', '09:00:00', '10:30:00', N'Cardio Circuit & Core Stability', 'Scheduled', N'Bài tập sức bền và thăng bằng', 1),
-- PT 2: Tuần 1, 2, 3
(11, 3, 2, 1, '2026-10-26', '16:00:00', '17:30:00', N'Tư Thế Squat & Cơ Đùi Nữ', 'Confirmed', N'Chỉnh sửa góc đầu gối khi xuống tạ', 1),
(12, 2, 2, 1, '2026-10-28', '17:30:00', '19:00:00', N'Tập Mông Quả Đào & Thon Gọn Eo', 'Confirmed', N'Hip Thrust 40kg, kết hợp dây kháng lực MiniBand', 1),
(13, 3, 2, 2, '2026-10-30', '16:00:00', '17:30:00', N'Pilates Dẻo Dai & Cột Sống', 'Scheduled', N'Kéo giãn giảm mỏi cổ vai gáy', 1),
(14, 2, 2, 1, '2026-11-03', '17:00:00', '18:30:00', N'Cardio Giảm Cân & Bụng Dưới', 'Scheduled', N'Tập Tabata 20s-10s và Plank', 1),
(15, 3, 2, 1, '2026-11-05', '16:00:00', '17:30:00', N'Tập Vai Nữ & Lưng Tự Tin', 'Scheduled', N'Dumbbell Lateral Raise 3kg', 1),
(16, 2, 2, 2, '2026-11-07', '17:30:00', '19:00:00', N'Kéo Giãn Cơ & Thư Giãn Toàn Thân', 'Scheduled', N'Foam rolling giải phóng điểm căng cơ', 1),
(17, 3, 2, 1, '2026-11-10', '16:00:00', '17:30:00', N'Legs Day & Glute Bridge', 'Scheduled', N'Rèn luyện cơ đùi sau và mông', 1),
(18, 2, 2, 1, '2026-11-12', '17:00:00', '18:30:00', N'Đốt Mỡ Kháng Lực Toàn Thân', 'Scheduled', N'Tập với tạ Kettlebell và bóng Slam Ball', 1),
-- PT 3: Tuần 1, 2, 3
(19, 1, 3, 3, '2026-10-27', '08:00:00', '09:30:00', N'Powerlifting Deadlift & Lưng Dày', 'Confirmed', N'Deadlift kỹ thuật Sumo, mức tạ 100kg', 2),
(20, 2, 3, 3, '2026-10-29', '10:00:00', '11:30:00', N'Bench Press Chuẩn Thi Đấu', 'Scheduled', N'Tập kỹ thuật khóa khớp vai và ép xương bả vai', 2),
(21, 1, 3, 3, '2026-10-31', '08:30:00', '10:00:00', N'Squat Nặng & Phục Hồi Khớp', 'Scheduled', N'Back Squat 80kg x 5 reps x 5 sets', 2),
(22, 3, 3, 3, '2026-11-03', '09:00:00', '10:30:00', N'Kỹ Thuật Cử Tạ Olympic Cơ Bản', 'Scheduled', N'Clean & Press tạ kỹ thuật 15kg', 2),
(23, 1, 3, 4, '2026-11-06', '10:00:00', '11:30:00', N'Conditioning & Thể Lực Đỉnh Cao', 'Scheduled', N'Đẩy xe trượt Prowler Sled & quất dây thừng Battle Rope', 2),
(24, 2, 3, 3, '2026-11-10', '08:30:00', '10:00:00', N'Tập Xô Lưng Tăng Độ Dày', 'Scheduled', N'Barbell Bent Over Row & T-Bar Row', 2),
(25, 1, 3, 3, '2026-11-12', '10:00:00', '11:30:00', N'Thử Thách 1RM Bench & Squat', 'Scheduled', N'Chuẩn bị khởi động dài và bảo hiểm an toàn', 2),
(26, 3, 3, 4, '2026-11-14', '09:00:00', '10:30:00', N'HIIT & Sức Bền Tim Mạch', 'Scheduled', N'Máy chèo thuyền kết hợp máy trượt tuyết', 2);
SET IDENTITY_INSERT dbo.Schedules OFF;
GO

-- ====================================================================================
-- J. BẢNG CheckIns: 15 bản ghi (hơn 10 dữ liệu) (Yêu cầu 5)
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
(15, 1, 1, '2026-10-26 08:20:00', 'Approved', N'Check-in buổi tập đầu tuần cùng PT Nam', 1);
SET IDENTITY_INSERT dbo.CheckIns OFF;
GO

-- ====================================================================================
-- K. BẢNG CheckInImages: 15 bản ghi (hơn 10 dữ liệu) (Yêu cầu 5)
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
-- L. BẢNG Equipments: 10 dữ liệu (Yêu cầu 6)
-- ====================================================================================
SET IDENTITY_INSERT dbo.Equipments ON;
INSERT INTO dbo.Equipments (EquipmentID, RoomID, EquipmentName, EquipmentCode, PurchaseDate, Price, Status, Description) VALUES
(1, 2, N'Máy chạy bộ Technogym Excite Live', 'TM-01', '2025-06-15', 35000000, N'Tốt', N'Máy chạy bộ cao cấp màn hình cảm ứng'),
(2, 1, N'Dàn gánh tạ Squat Rack Olympic', 'SQ-01', '2025-06-20', 18000000, N'Tốt', N'Khung gánh tạ an toàn chịu tải 500kg'),
(3, 1, N'Ghế đẩy ngực Incline Bench Press', 'BP-01', '2025-07-05', 12000000, N'Tốt', N'Ghế dốc tập ngực trên chuẩn thi đấu'),
(4, 1, N'Máy kéo xô Lat Pulldown & Row', 'LP-01', '2025-07-10', 28000000, N'Tốt', N'Máy khối đôi kéo xô và chèo xô lưng'),
(5, 2, N'Xe đạp tập Cardio LifeFitness', 'BK-01', '2025-08-01', 16000000, N'Tốt', N'Xe đạp tĩnh kháng lực từ trường'),
(6, 3, N'Máy đạp đùi Leg Press 45 độ', 'LP-02', '2025-08-15', 32000000, N'Tốt', N'Máy đạp chân cơ đùi chịu tải trọng lớn'),
(7, 3, N'Dàn cáp đôi Dual Cable Crossover', 'CC-01', '2025-09-01', 42000000, N'Bảo trì', N'Dàn cáp đôi đa hướng ròng rọc nhôm'),
(8, 4, N'Máy trượt tuyết Elliptical Matrix', 'EL-01', '2025-09-15', 26000000, N'Tốt', N'Máy tập toàn thân bảo vệ khớp gối'),
(9, 5, N'Bộ tạ tay Dumbbell 5-40kg kèm giá', 'DB-01', '2025-10-01', 45000000, N'Tốt', N'Dàn tạ bọc urethene cao cấp 20 đôi'),
(10, 6, N'Máy chèo thuyền WaterRower', 'RW-01', '2025-10-15', 22000000, N'Tốt', N'Máy chèo thuyền cản nước bằng gỗ sồi');
SET IDENTITY_INSERT dbo.Equipments OFF;
GO

-- ====================================================================================
-- M. BẢNG EquipmentMaintenances: 10 dữ liệu (Yêu cầu 6)
-- ====================================================================================
SET IDENTITY_INSERT dbo.EquipmentMaintenances ON;
INSERT INTO dbo.EquipmentMaintenances (MaintenanceID, EquipmentID, MaintenanceDate, NextMaintenanceDate, Description, Cost, Status) VALUES
(1, 1, '2025-12-01', '2026-06-01', N'Bảo dưỡng định kỳ thảm chạy và tra dầu băng chuyền', 500000, N'Hoàn thành'),
(2, 2, '2025-12-15', '2026-06-15', N'Siết chặt ốc liên kết và kiểm tra thanh an toàn J-Hooks', 300000, N'Hoàn thành'),
(3, 4, '2026-01-10', '2026-07-10', N'Thay dây cáp thép bọc nylon và bôi trơn ròng rọc', 750000, N'Hoàn thành'),
(4, 5, '2026-02-05', '2026-08-05', N'Thay bàn đạp dây đai và cảm biến nhịp tim', 600000, N'Hoàn thành'),
(5, 6, '2026-02-20', '2026-08-20', N'Tra mỡ vòng bi ray trượt máy đạp đùi', 450000, N'Hoàn thành'),
(6, 7, '2026-03-05', '2026-09-05', N'Căn chỉnh lại hệ thống đối trọng tạ cáp bên phải', 800000, N'Hoàn thành'),
(7, 8, '2026-03-15', '2026-09-15', N'Bảo trì mô-tơ kháng lực máy trượt tuyết', 900000, N'Hoàn thành'),
(8, 1, '2026-06-01', '2026-12-01', N'Kiểm tra động cơ 4.0 HP và căn chỉnh độ lệch băng tải', 650000, N'Hoàn thành'),
(9, 7, '2026-10-15', '2026-11-15', N'Thay thế chốt tạ selector pin bị kẹt và bảo dưỡng bánh xe dẫn hướng', 400000, N'Đang xử lý'),
(10, 10, '2026-10-20', '2027-04-20', N'Thay nước bình chứa và làm sạch bánh đà máy chèo thuyền', 350000, N'Hoàn thành');
SET IDENTITY_INSERT dbo.EquipmentMaintenances OFF;
GO

-- ====================================================================================
-- N. BẢNG Incidents: 10 dữ liệu (Yêu cầu 6)
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
(9, 7, 4, '2026-10-22', N'Chốt chọn mức tạ bị cong nhẹ do hội viên thả tạ rơi tự do', 'In Progress', NULL, N'Đang chờ nhà cung cấp giao chốt tạ mới thay thế'),
(10, 10, 6, '2026-10-25', N'Dây kéo tay cầm máy chèo thuyền bị xoắn nhẹ cần căn lại', 'New', NULL, N'Đã giao kỹ thuật viên ca sáng kiểm tra');
SET IDENTITY_INSERT dbo.Incidents OFF;
GO

-- ====================================================================================
-- O. BẢNG Progresses: 10 dữ liệu theo dõi thể chất hội viên (Yêu cầu 2, 10)
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
(10, 6, 3, '2026-03-10', 178.0, 82.0, 24.0, 35.0, 25.9, 100.0, 88.0, 101.0, 35.0, 59.0, N'Tập tăng cơ giảm mỡ ban đầu');
SET IDENTITY_INSERT dbo.Progresses OFF;
GO

-- Bật lại kiểm tra ràng buộc khóa ngoại
EXEC sp_MSforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all";
GO

PRINT N'====================================================================================';
PRINT N'CẬP NHẬT DATABASE VÀ SEED DATA HOÀN TẤT THÀNH CÔNG! (BẢNG __EFMigrationsHistory NGUYÊN VẸN)';
PRINT N'====================================================================================';
GO
