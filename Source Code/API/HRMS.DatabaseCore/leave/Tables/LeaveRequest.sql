CREATE TABLE [leave].[LeaveRequest] (
    [LeaveRequestID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]       BIGINT         NOT NULL,
    [EmployeeID]     BIGINT         NOT NULL,
    [LeaveTypeID]    BIGINT         NOT NULL,
    [FromDate]       DATE           NOT NULL,
    [ToDate]         DATE           NOT NULL,
    [TotalDays]      DECIMAL (5, 2) NOT NULL,
    [Reason]         NVARCHAR (500) NOT NULL,
    [Status]         NVARCHAR (50)  DEFAULT ('Pending') NOT NULL,
    [CreatedBy]      BIGINT         NOT NULL,
    [CreatedDate]    DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]     BIGINT         NULL,
    [ModifiedDate]   DATETIME2 (7)  NULL,
    [DeletedBy]      BIGINT         NULL,
    [DeletedDate]    DATETIME2 (7)  NULL,
    [IsDeleted]      BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]     ROWVERSION     NOT NULL,
    CONSTRAINT [PK_LeaveRequest] PRIMARY KEY CLUSTERED ([LeaveRequestID] ASC),
    CONSTRAINT [FK_LeaveRequest_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_LeaveRequest_LeaveType] FOREIGN KEY ([LeaveTypeID]) REFERENCES [leave].[LeaveType] ([LeaveTypeID]),
    CONSTRAINT [FK_LeaveRequest_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_LeaveRequest_Status]
    ON [leave].[LeaveRequest]([TenantID] ASC, [Status] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_LeaveRequest_Employee]
    ON [leave].[LeaveRequest]([TenantID] ASC, [EmployeeID] ASC) WHERE ([IsDeleted]=(0));


GO

CREATE TRIGGER leave.trg_LeaveRequest_Audit
ON leave.LeaveRequest
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
            'LeaveRequest',
            i.LeaveRequestID,
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
            'LeaveRequest',
            i.LeaveRequestID,
            'Update',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            COALESCE(i.ModifiedBy, 1)
        FROM inserted i
        INNER JOIN deleted d ON i.LeaveRequestID = d.LeaveRequestID;
    END
    ELSE IF @Action = 'Delete'
    BEGIN
        INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
        SELECT 
            d.TenantID,
            'LeaveRequest',
            d.LeaveRequestID,
            'Delete',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            NULL,
            COALESCE(d.DeletedBy, 1)
        FROM deleted d;
    END
END;