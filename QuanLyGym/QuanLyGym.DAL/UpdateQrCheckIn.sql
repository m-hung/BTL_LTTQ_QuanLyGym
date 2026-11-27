-- Run against the database used by QuanLyGym. Safe to run again.
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID(N'dbo.Members', N'U') IS NULL OR OBJECT_ID(N'dbo.CheckIns', N'U') IS NULL
        THROW 51002, 'Select the QuanLyGym database before applying this update.', 1;

    IF COL_LENGTH(N'dbo.Members', N'QrCardId') IS NULL
        ALTER TABLE dbo.Members ADD QrCardId uniqueidentifier NOT NULL
            CONSTRAINT DF_Members_QrCardId DEFAULT NEWID();

    IF NOT EXISTS (SELECT 1 FROM sys.indexes
                   WHERE object_id = OBJECT_ID(N'dbo.Members') AND name = N'IX_Members_QrCardId')
        EXEC(N'CREATE UNIQUE INDEX IX_Members_QrCardId ON dbo.Members(QrCardId);');

    IF COL_LENGTH(N'dbo.CheckIns', N'Method') IS NULL
        ALTER TABLE dbo.CheckIns ADD Method nvarchar(24) NULL;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
