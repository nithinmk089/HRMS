CREATE TABLE [communication].[UserNotificationPreference] (
    [PreferenceID]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]      BIGINT        NOT NULL,
    [UserID]        BIGINT        NOT NULL,
    [BusinessEvent] VARCHAR (100) NOT NULL,
    [Channel]       VARCHAR (50)  NOT NULL,
    [Frequency]     VARCHAR (50)  NOT NULL,
    [IsEnabled]     BIT           DEFAULT ((1)) NOT NULL,
    [CreatedBy]     BIGINT        NOT NULL,
    [CreatedDate]   DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]    BIGINT        NULL,
    [ModifiedDate]  DATETIME      NULL,
    CONSTRAINT [PK_UserNotificationPreference] PRIMARY KEY CLUSTERED ([PreferenceID] ASC),
    CONSTRAINT [FK_UserNotificationPreference_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID]),
    CONSTRAINT [FK_UserNotificationPreference_User] FOREIGN KEY ([UserID]) REFERENCES [security].[User] ([UserID]),
    CONSTRAINT [UQ_UserNotificationPreference] UNIQUE NONCLUSTERED ([UserID] ASC, [BusinessEvent] ASC, [Channel] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_UserNotificationPreference_User]
    ON [communication].[UserNotificationPreference]([TenantID] ASC, [UserID] ASC);


GO

CREATE   TRIGGER communication.trg_UserNotificationPreference_Audit_Delete
ON communication.UserNotificationPreference
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'UserNotificationPreference',
        d.PreferenceID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        1
    FROM deleted d;
END;
GO

CREATE   TRIGGER communication.trg_UserNotificationPreference_Audit_Update
ON communication.UserNotificationPreference
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'UserNotificationPreference',
        i.PreferenceID,
        'Update',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.PreferenceID = d.PreferenceID;
END;
GO

-- USER NOTIFICATION PREFERENCE AUDIT TRIGGERS
CREATE   TRIGGER communication.trg_UserNotificationPreference_Audit_Insert
ON communication.UserNotificationPreference
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'UserNotificationPreference',
        i.PreferenceID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        i.CreatedBy
    FROM inserted i;
END;