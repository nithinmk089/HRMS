CREATE TABLE [security].[UserPermission] (
    [UserPermissionID] BIGINT   IDENTITY (1, 1) NOT NULL,
    [TenantID]         BIGINT   NOT NULL,
    [UserID]           BIGINT   NOT NULL,
    [PermissionID]     BIGINT   NOT NULL,
    [IsAllowed]        BIT      DEFAULT ((1)) NOT NULL,
    [CreatedBy]        BIGINT   NOT NULL,
    [CreatedDate]      DATETIME DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]       BIGINT   NULL,
    [ModifiedDate]     DATETIME NULL,
    [DeletedBy]        BIGINT   NULL,
    [DeletedDate]      DATETIME NULL,
    [IsDeleted]        BIT      DEFAULT ((0)) NOT NULL,
    [VersionNo]        BIGINT   DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_UserPermission] PRIMARY KEY CLUSTERED ([UserPermissionID] ASC),
    CONSTRAINT [FK_UserPermission_Permission] FOREIGN KEY ([PermissionID]) REFERENCES [security].[Permission] ([PermissionID]),
    CONSTRAINT [FK_UserPermission_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID]),
    CONSTRAINT [FK_UserPermission_User] FOREIGN KEY ([UserID]) REFERENCES [security].[User] ([UserID]),
    CONSTRAINT [UQ_UserPermission] UNIQUE NONCLUSTERED ([UserID] ASC, [PermissionID] ASC)
);


GO

CREATE   TRIGGER security.trg_UserPermission_Audit_Delete
ON security.UserPermission
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'UserPermission',
        d.UserPermissionID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        COALESCE(d.DeletedBy, 1)
    FROM deleted d;
END;
GO

CREATE   TRIGGER security.trg_UserPermission_Audit_Update
ON security.UserPermission
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'UserPermission',
        i.UserPermissionID,
        'Update',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.UserPermissionID = d.UserPermissionID;
END;
GO

-- USERPERMISSION AUDIT TRIGGERS
CREATE   TRIGGER security.trg_UserPermission_Audit_Insert
ON security.UserPermission
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'UserPermission',
        i.UserPermissionID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.CreatedBy, 1)
    FROM inserted i;
END;