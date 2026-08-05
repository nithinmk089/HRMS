CREATE TABLE [offboarding].[ClearanceRequest] (
    [ClearanceRequestID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT        NOT NULL,
    [EmployeeID]         BIGINT        NOT NULL,
    [ClearanceStatus]    NVARCHAR (50) DEFAULT ('Pending') NOT NULL,
    [InitiatedDate]      DATE          NOT NULL,
    [CompletedDate]      DATE          NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [CreatedDate]        DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT        NULL,
    [ModifiedDate]       DATETIME2 (7) NULL,
    [DeletedBy]          BIGINT        NULL,
    [DeletedDate]        DATETIME2 (7) NULL,
    [IsDeleted]          BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION    NOT NULL,
    CONSTRAINT [PK_ClearanceRequest] PRIMARY KEY CLUSTERED ([ClearanceRequestID] ASC),
    CONSTRAINT [FK_ClearanceRequest_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_ClearanceRequest_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO

CREATE TRIGGER offboarding.trg_ClearanceRequest_Audit
ON offboarding.ClearanceRequest
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
        'ClearanceRequest',
        COALESCE(i.ClearanceRequestID, d.ClearanceRequestID),
        @Action,
        COALESCE(i.ModifiedBy, i.CreatedBy, d.ModifiedBy, 1),
        GETUTCDATE(),
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
    FROM inserted i
    FULL OUTER JOIN deleted d ON i.ClearanceRequestID = d.ClearanceRequestID;
END