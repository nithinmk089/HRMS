CREATE TABLE [offboarding].[ExitRequest] (
    [ExitRequestID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]        BIGINT         NOT NULL,
    [EmployeeID]      BIGINT         NOT NULL,
    [ResignationDate] DATE           NOT NULL,
    [LastWorkingDate] DATE           NOT NULL,
    [ExitReason]      NVARCHAR (500) NOT NULL,
    [Status]          NVARCHAR (50)  DEFAULT ('Pending') NOT NULL,
    [CreatedBy]       BIGINT         NOT NULL,
    [CreatedDate]     DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]      BIGINT         NULL,
    [ModifiedDate]    DATETIME2 (7)  NULL,
    [DeletedBy]       BIGINT         NULL,
    [DeletedDate]     DATETIME2 (7)  NULL,
    [IsDeleted]       BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]      ROWVERSION     NOT NULL,
    CONSTRAINT [PK_ExitRequest] PRIMARY KEY CLUSTERED ([ExitRequestID] ASC),
    CONSTRAINT [FK_ExitRequest_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_ExitRequest_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_ExitRequest_Status]
    ON [offboarding].[ExitRequest]([Status] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_ExitRequest_Employee]
    ON [offboarding].[ExitRequest]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));


GO

CREATE TRIGGER offboarding.trg_ExitRequest_Audit
ON offboarding.ExitRequest
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Action NVARCHAR(50) = 'INSERT';
    IF EXISTS(SELECT * FROM deleted)
    BEGIN
        SET @Action = CASE WHEN EXISTS(SELECT * FROM inserted) THEN 'UPDATE' ELSE 'DELETE' END;
    END

    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, PerformedBy, PerformedDate, OldValueJSON, NewValueJSON)
    SELECT 
        COALESCE(i.TenantID, d.TenantID),
        'ExitRequest',
        COALESCE(i.ExitRequestID, d.ExitRequestID),
        @Action,
        COALESCE(i.ModifiedBy, i.CreatedBy, d.ModifiedBy, 1),
        GETUTCDATE(),
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
    FROM inserted i
    FULL OUTER JOIN deleted d ON i.ExitRequestID = d.ExitRequestID;
END