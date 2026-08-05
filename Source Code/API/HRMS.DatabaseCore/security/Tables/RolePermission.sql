CREATE TABLE [security].[RolePermission] (
    [RolePermissionID] BIGINT   IDENTITY (1, 1) NOT NULL,
    [TenantID]         BIGINT   NOT NULL,
    [RoleID]           BIGINT   NOT NULL,
    [PermissionID]     BIGINT   NOT NULL,
    [CreatedBy]        BIGINT   NOT NULL,
    [CreatedDate]      DATETIME DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]       BIGINT   NULL,
    [ModifiedDate]     DATETIME NULL,
    [DeletedBy]        BIGINT   NULL,
    [DeletedDate]      DATETIME NULL,
    [IsDeleted]        BIT      DEFAULT ((0)) NOT NULL,
    [VersionNo]        BIGINT   DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_RolePermission] PRIMARY KEY CLUSTERED ([RolePermissionID] ASC),
    CONSTRAINT [FK_RolePermission_Permission] FOREIGN KEY ([PermissionID]) REFERENCES [security].[Permission] ([PermissionID]),
    CONSTRAINT [FK_RolePermission_Role] FOREIGN KEY ([RoleID]) REFERENCES [security].[Role] ([RoleID]),
    CONSTRAINT [FK_RolePermission_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID]),
    CONSTRAINT [UQ_RolePermission] UNIQUE NONCLUSTERED ([RoleID] ASC, [PermissionID] ASC)
);


GO

-- ROLEPERMISSION AUDIT TRIGGERS
CREATE   TRIGGER security.trg_RolePermission_Audit_Insert
ON security.RolePermission
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'RolePermission',
        i.RolePermissionID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.CreatedBy, 1)
    FROM inserted i;
END;
GO

CREATE   TRIGGER security.trg_RolePermission_Audit_Delete
ON security.RolePermission
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'RolePermission',
        d.RolePermissionID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        COALESCE(d.DeletedBy, 1)
    FROM deleted d;
END;
GO

CREATE   TRIGGER security.trg_RolePermission_Audit_Update
ON security.RolePermission
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'RolePermission',
        i.RolePermissionID,
        'Update',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.RolePermissionID = d.RolePermissionID;
END;