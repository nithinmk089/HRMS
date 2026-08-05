CREATE TABLE [security].[Tenant] (
    [TenantID]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantCode]    VARCHAR (50)  NOT NULL,
    [TenantName]    VARCHAR (200) NOT NULL,
    [Status]        VARCHAR (20)  NOT NULL,
    [EffectiveFrom] DATETIME      NOT NULL,
    [EffectiveTo]   DATETIME      NULL,
    [CreatedBy]     BIGINT        NOT NULL,
    [CreatedDate]   DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]    BIGINT        NULL,
    [ModifiedDate]  DATETIME      NULL,
    [DeletedBy]     BIGINT        NULL,
    [DeletedDate]   DATETIME      NULL,
    [IsDeleted]     BIT           DEFAULT ((0)) NOT NULL,
    [VersionNo]     BIGINT        DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Tenant] PRIMARY KEY CLUSTERED ([TenantID] ASC),
    CONSTRAINT [UQ_Tenant_Code] UNIQUE NONCLUSTERED ([TenantCode] ASC),
    CONSTRAINT [UQ_Tenant_Name] UNIQUE NONCLUSTERED ([TenantName] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Tenant_Status]
    ON [security].[Tenant]([Status] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Tenant_Name]
    ON [security].[Tenant]([TenantName] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Tenant_Code]
    ON [security].[Tenant]([TenantCode] ASC) WHERE ([IsDeleted]=(0));


GO

CREATE   TRIGGER security.trg_Tenant_Code_Validation
ON security.Tenant
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1 
        FROM inserted 
        WHERE LEN(TenantCode) < 3 OR TenantCode LIKE '%[^A-Z0-9]%'
    )
    BEGIN
        RAISERROR ('Tenant Code must be alphanumeric and at least 3 characters long.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
GO

CREATE   TRIGGER security.trg_Tenant_Audit_Delete
ON security.Tenant
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'Tenant',
        d.TenantID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        COALESCE(d.DeletedBy, 1)
    FROM deleted d;
END;
GO

CREATE   TRIGGER security.trg_Tenant_Audit_Update
ON security.Tenant
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Tenant',
        i.TenantID,
        CASE WHEN i.IsDeleted = 1 AND d.IsDeleted = 0 THEN 'Delete' ELSE 'Update' END,
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, i.DeletedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.TenantID = d.TenantID;
END;
GO

-- TENANT AUDIT TRIGGERS
CREATE   TRIGGER security.trg_Tenant_Audit_Insert
ON security.Tenant
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Tenant',
        i.TenantID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        i.CreatedBy
    FROM inserted i;
END;