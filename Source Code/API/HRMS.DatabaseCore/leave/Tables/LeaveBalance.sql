CREATE TABLE [leave].[LeaveBalance] (
    [LeaveBalanceID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]         BIGINT         NOT NULL,
    [EmployeeID]       BIGINT         NOT NULL,
    [LeaveTypeID]      BIGINT         NOT NULL,
    [OpeningBalance]   DECIMAL (5, 2) DEFAULT ((0)) NOT NULL,
    [AccruedBalance]   DECIMAL (5, 2) DEFAULT ((0)) NOT NULL,
    [ConsumedBalance]  DECIMAL (5, 2) DEFAULT ((0)) NOT NULL,
    [AvailableBalance] DECIMAL (5, 2) DEFAULT ((0)) NOT NULL,
    [CreatedBy]        BIGINT         NOT NULL,
    [CreatedDate]      DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]       BIGINT         NULL,
    [ModifiedDate]     DATETIME2 (7)  NULL,
    [DeletedBy]        BIGINT         NULL,
    [DeletedDate]      DATETIME2 (7)  NULL,
    [IsDeleted]        BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]       ROWVERSION     NOT NULL,
    CONSTRAINT [PK_LeaveBalance] PRIMARY KEY CLUSTERED ([LeaveBalanceID] ASC),
    CONSTRAINT [FK_LeaveBalance_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_LeaveBalance_LeaveType] FOREIGN KEY ([LeaveTypeID]) REFERENCES [leave].[LeaveType] ([LeaveTypeID]),
    CONSTRAINT [FK_LeaveBalance_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_LeaveBalance_Employee]
    ON [leave].[LeaveBalance]([TenantID] ASC, [EmployeeID] ASC) WHERE ([IsDeleted]=(0));


GO

CREATE TRIGGER leave.trg_LeaveBalance_Audit
ON leave.LeaveBalance
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
            'LeaveBalance',
            i.LeaveBalanceID,
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
            'LeaveBalance',
            i.LeaveBalanceID,
            'Update',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            COALESCE(i.ModifiedBy, 1)
        FROM inserted i
        INNER JOIN deleted d ON i.LeaveBalanceID = d.LeaveBalanceID;
    END
    ELSE IF @Action = 'Delete'
    BEGIN
        INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
        SELECT 
            d.TenantID,
            'LeaveBalance',
            d.LeaveBalanceID,
            'Delete',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            NULL,
            COALESCE(d.DeletedBy, 1)
        FROM deleted d;
    END
END;