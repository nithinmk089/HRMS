CREATE TABLE [security].[Role] (
    [RoleID]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]     BIGINT        NOT NULL,
    [RoleCode]     VARCHAR (50)  NOT NULL,
    [RoleName]     VARCHAR (200) NOT NULL,
    [Description]  VARCHAR (500) NULL,
    [CreatedBy]    BIGINT        NOT NULL,
    [CreatedDate]  DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]   BIGINT        NULL,
    [ModifiedDate] DATETIME      NULL,
    [DeletedBy]    BIGINT        NULL,
    [DeletedDate]  DATETIME      NULL,
    [IsDeleted]    BIT           DEFAULT ((0)) NOT NULL,
    [VersionNo]    BIGINT        DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([RoleID] ASC),
    CONSTRAINT [FK_Role_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO

CREATE   TRIGGER security.trg_Role_Audit_Delete
ON security.[Role]
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'Role',
        d.RoleID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        COALESCE(d.DeletedBy, 1)
    FROM deleted d;
END;
GO

CREATE   TRIGGER security.trg_Role_Audit_Update
ON security.[Role]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Role',
        i.RoleID,
        'Update',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.RoleID = d.RoleID;
END;
GO

CREATE   TRIGGER security.trg_Role_Audit_Insert
ON security.[Role]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Role',
        i.RoleID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.CreatedBy, 1)
    FROM inserted i;
END;