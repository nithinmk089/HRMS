CREATE TABLE [security].[User] (
    [UserID]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]      BIGINT        NOT NULL,
    [EmployeeID]    BIGINT        NULL,
    [UserName]      VARCHAR (100) NOT NULL,
    [Email]         VARCHAR (200) NOT NULL,
    [PasswordHash]  VARCHAR (500) NOT NULL,
    [PasswordSalt]  VARCHAR (500) NOT NULL,
    [IsLocked]      BIT           DEFAULT ((0)) NOT NULL,
    [LastLoginDate] DATETIME      NULL,
    [CreatedBy]     BIGINT        NOT NULL,
    [CreatedDate]   DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]    BIGINT        NULL,
    [ModifiedDate]  DATETIME      NULL,
    [DeletedBy]     BIGINT        NULL,
    [DeletedDate]   DATETIME      NULL,
    [IsDeleted]     BIT           DEFAULT ((0)) NOT NULL,
    [VersionNo]     BIGINT        DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [FK_User_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID]),
    CONSTRAINT [UQ_User_Email] UNIQUE NONCLUSTERED ([Email] ASC),
    CONSTRAINT [UQ_User_UserName] UNIQUE NONCLUSTERED ([UserName] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_User_Email]
    ON [security].[User]([Email] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_User_UserName]
    ON [security].[User]([UserName] ASC) WHERE ([IsDeleted]=(0));


GO

-- 5. TRIGGERS
CREATE   TRIGGER security.trg_User_Email_Validation
ON security.[User]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1 
        FROM inserted 
        WHERE Email NOT LIKE '%_@__%.__%'
    )
    BEGIN
        RAISERROR ('Invalid email address format.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
GO

CREATE   TRIGGER security.trg_User_Audit_Delete
ON security.[User]
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'User',
        d.UserID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        COALESCE(d.DeletedBy, 1)
    FROM deleted d;
END;
GO

CREATE   TRIGGER security.trg_User_Audit_Update
ON security.[User]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'User',
        i.UserID,
        'Update',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.UserID = d.UserID;
END;
GO

CREATE   TRIGGER security.trg_User_Audit_Insert
ON security.[User]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'User',
        i.UserID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.CreatedBy, 1)
    FROM inserted i;
END;