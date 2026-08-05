CREATE TABLE [communication].[NotificationTemplate] (
    [TemplateID]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]        BIGINT        NOT NULL,
    [TemplateName]    VARCHAR (100) NOT NULL,
    [BusinessEvent]   VARCHAR (100) NOT NULL,
    [Channel]         VARCHAR (50)  NOT NULL,
    [SubjectTemplate] VARCHAR (200) NULL,
    [BodyTemplate]    VARCHAR (MAX) NOT NULL,
    [IsActive]        BIT           DEFAULT ((1)) NOT NULL,
    [CreatedBy]       BIGINT        NOT NULL,
    [CreatedDate]     DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]      BIGINT        NULL,
    [ModifiedDate]    DATETIME      NULL,
    [DeletedBy]       BIGINT        NULL,
    [DeletedDate]     DATETIME      NULL,
    [IsDeleted]       BIT           DEFAULT ((0)) NOT NULL,
    [VersionNo]       BIGINT        DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_NotificationTemplate] PRIMARY KEY CLUSTERED ([TemplateID] ASC),
    CONSTRAINT [FK_NotificationTemplate_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID]),
    CONSTRAINT [UQ_NotificationTemplate] UNIQUE NONCLUSTERED ([TenantID] ASC, [BusinessEvent] ASC, [Channel] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_NotificationTemplate_Tenant]
    ON [communication].[NotificationTemplate]([TenantID] ASC) WHERE ([IsDeleted]=(0));


GO

CREATE   TRIGGER communication.trg_NotificationTemplate_Audit_Delete
ON communication.NotificationTemplate
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'NotificationTemplate',
        d.TemplateID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        COALESCE(d.DeletedBy, 1)
    FROM deleted d;
END;
GO

CREATE   TRIGGER communication.trg_NotificationTemplate_Audit_Update
ON communication.NotificationTemplate
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'NotificationTemplate',
        i.TemplateID,
        CASE WHEN i.IsDeleted = 1 AND d.IsDeleted = 0 THEN 'Delete' ELSE 'Update' END,
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, i.DeletedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.TemplateID = d.TemplateID;
END;
GO

-- NOTIFICATION TEMPLATE AUDIT TRIGGERS
CREATE   TRIGGER communication.trg_NotificationTemplate_Audit_Insert
ON communication.NotificationTemplate
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'NotificationTemplate',
        i.TemplateID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        i.CreatedBy
    FROM inserted i;
END;