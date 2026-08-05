CREATE TABLE [security].[Permission] (
    [PermissionID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]       BIGINT        NOT NULL,
    [PermissionCode] VARCHAR (100) NOT NULL,
    [PermissionName] VARCHAR (200) NOT NULL,
    [ModuleCode]     VARCHAR (100) NOT NULL,
    [CreatedBy]      BIGINT        NOT NULL,
    [CreatedDate]    DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]     BIGINT        NULL,
    [ModifiedDate]   DATETIME      NULL,
    [DeletedBy]      BIGINT        NULL,
    [DeletedDate]    DATETIME      NULL,
    [IsDeleted]      BIT           DEFAULT ((0)) NOT NULL,
    [VersionNo]      BIGINT        DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([PermissionID] ASC),
    CONSTRAINT [FK_Permission_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO

CREATE   TRIGGER security.trg_Permission_Audit_Delete
ON security.[Permission]
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'Permission',
        d.PermissionID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        COALESCE(d.DeletedBy, 1)
    FROM deleted d;
END;
GO

CREATE   TRIGGER security.trg_Permission_Audit_Update
ON security.[Permission]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Permission',
        i.PermissionID,
        'Update',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.PermissionID = d.PermissionID;
END;
GO

-- PERMISSION AUDIT TRIGGERS
CREATE   TRIGGER security.trg_Permission_Audit_Insert
ON security.[Permission]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Permission',
        i.PermissionID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.CreatedBy, 1)
    FROM inserted i;
END;