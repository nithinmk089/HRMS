CREATE TABLE [security].[UserRole] (
    [UserRoleID]   BIGINT   IDENTITY (1, 1) NOT NULL,
    [TenantID]     BIGINT   NOT NULL,
    [UserID]       BIGINT   NOT NULL,
    [RoleID]       BIGINT   NOT NULL,
    [CreatedBy]    BIGINT   NOT NULL,
    [CreatedDate]  DATETIME DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]   BIGINT   NULL,
    [ModifiedDate] DATETIME NULL,
    [DeletedBy]    BIGINT   NULL,
    [DeletedDate]  DATETIME NULL,
    [IsDeleted]    BIT      DEFAULT ((0)) NOT NULL,
    [VersionNo]    BIGINT   DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([UserRoleID] ASC),
    CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleID]) REFERENCES [security].[Role] ([RoleID]),
    CONSTRAINT [FK_UserRole_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID]),
    CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserID]) REFERENCES [security].[User] ([UserID]),
    CONSTRAINT [UQ_UserRole] UNIQUE NONCLUSTERED ([UserID] ASC, [RoleID] ASC)
);


GO

CREATE   TRIGGER security.trg_UserRole_Audit_Delete
ON security.UserRole
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'UserRole',
        d.UserRoleID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        COALESCE(d.DeletedBy, 1)
    FROM deleted d;
END;
GO

CREATE   TRIGGER security.trg_UserRole_Audit_Update
ON security.UserRole
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'UserRole',
        i.UserRoleID,
        'Update',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.UserRoleID = d.UserRoleID;
END;
GO

-- USERROLE AUDIT TRIGGERS
CREATE   TRIGGER security.trg_UserRole_Audit_Insert
ON security.UserRole
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'UserRole',
        i.UserRoleID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.CreatedBy, 1)
    FROM inserted i;
END;