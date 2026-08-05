CREATE TABLE [attendance].[Attendance] (
    [AttendanceID]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]         BIGINT        NOT NULL,
    [EmployeeID]       BIGINT        NOT NULL,
    [ShiftID]          BIGINT        NOT NULL,
    [AttendanceDate]   DATE          NOT NULL,
    [ClockInTime]      DATETIME2 (7) NULL,
    [ClockOutTime]     DATETIME2 (7) NULL,
    [WorkingMinutes]   INT           NULL,
    [AttendanceStatus] NVARCHAR (50) NOT NULL,
    [CreatedBy]        BIGINT        NOT NULL,
    [CreatedDate]      DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]       BIGINT        NULL,
    [ModifiedDate]     DATETIME2 (7) NULL,
    [DeletedBy]        BIGINT        NULL,
    [DeletedDate]      DATETIME2 (7) NULL,
    [IsDeleted]        BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]       ROWVERSION    NOT NULL,
    CONSTRAINT [PK_Attendance] PRIMARY KEY CLUSTERED ([AttendanceID] ASC),
    CONSTRAINT [FK_Attendance_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_Attendance_Shift] FOREIGN KEY ([ShiftID]) REFERENCES [attendance].[Shift] ([ShiftID]),
    CONSTRAINT [FK_Attendance_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Attendance_Date]
    ON [attendance].[Attendance]([TenantID] ASC, [AttendanceDate] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Attendance_Employee_Date]
    ON [attendance].[Attendance]([TenantID] ASC, [EmployeeID] ASC, [AttendanceDate] ASC) WHERE ([IsDeleted]=(0));


GO

-- === 5. TRIGGERS ===
CREATE TRIGGER attendance.trg_Attendance_Audit
ON attendance.Attendance
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Action VARCHAR(10);
    IF EXISTS(SELECT 1 FROM inserted) AND EXISTS(SELECT 1 FROM deleted)
        SET @Action = 'Update';
    ELSE IF EXISTS(SELECT 1 FROM inserted)
        SET @Action = 'Insert';
    ELSE IF EXISTS(SELECT 1 FROM deleted)
        SET @Action = 'Delete';

    IF @Action = 'Insert'
    BEGIN
        INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
        SELECT 
            i.TenantID,
            'Attendance',
            i.AttendanceID,
            'Insert',
            NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            COALESCE(i.CreatedBy, 1)
        FROM inserted i;
    END
    ELSE IF @Action = 'Update'
    BEGIN
        INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
        SELECT 
            i.TenantID,
            'Attendance',
            i.AttendanceID,
            'Update',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            COALESCE(i.ModifiedBy, 1)
        FROM inserted i
        INNER JOIN deleted d ON i.AttendanceID = d.AttendanceID;
    END
    ELSE IF @Action = 'Delete'
    BEGIN
        INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
        SELECT 
            d.TenantID,
            'Attendance',
            d.AttendanceID,
            'Delete',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            NULL,
            COALESCE(d.DeletedBy, 1)
        FROM deleted d;
    END
END;