CREATE TABLE [organization].[CostCenter] (
    [CostCenterID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]       BIGINT        NOT NULL,
    [CostCenterCode] VARCHAR (50)  NOT NULL,
    [CostCenterName] VARCHAR (200) NOT NULL,
    [CreatedBy]      BIGINT        NOT NULL,
    [CreatedDate]    DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]     BIGINT        NULL,
    [ModifiedDate]   DATETIME      NULL,
    [DeletedBy]      BIGINT        NULL,
    [DeletedDate]    DATETIME      NULL,
    [IsDeleted]      BIT           DEFAULT ((0)) NOT NULL,
    [VersionNo]      BIGINT        DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CostCenter] PRIMARY KEY CLUSTERED ([CostCenterID] ASC),
    CONSTRAINT [FK_CostCenter_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO

CREATE   TRIGGER organization.trg_CostCenter_Audit_Delete
ON organization.CostCenter
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'CostCenter',
        d.CostCenterID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        COALESCE(d.DeletedBy, 1)
    FROM deleted d;
END;
GO

CREATE   TRIGGER organization.trg_CostCenter_Audit_Update
ON organization.CostCenter
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'CostCenter',
        i.CostCenterID,
        'Update',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.CostCenterID = d.CostCenterID;
END;
GO

-- COST CENTER AUDIT TRIGGERS
CREATE   TRIGGER organization.trg_CostCenter_Audit_Insert
ON organization.CostCenter
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'CostCenter',
        i.CostCenterID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.CreatedBy, 1)
    FROM inserted i;
END;