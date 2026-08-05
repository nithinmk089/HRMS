CREATE TABLE [system].[Configuration] (
    [ConfigurationID]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT        NOT NULL,
    [ConfigurationKey]   VARCHAR (200) NOT NULL,
    [ConfigurationValue] VARCHAR (MAX) NULL,
    [DataType]           VARCHAR (50)  DEFAULT ('String') NOT NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [CreatedDate]        DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT        NULL,
    [ModifiedDate]       DATETIME      NULL,
    [DeletedBy]          BIGINT        NULL,
    [DeletedDate]        DATETIME      NULL,
    [IsDeleted]          BIT           DEFAULT ((0)) NOT NULL,
    [VersionNo]          BIGINT        DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED ([ConfigurationID] ASC),
    CONSTRAINT [FK_Configuration_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Configuration_Key]
    ON [system].[Configuration]([TenantID] ASC, [ConfigurationKey] ASC) WHERE ([IsDeleted]=(0));


GO

CREATE   TRIGGER system.trg_Configuration_Audit_Delete
ON system.Configuration
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'Configuration',
        d.ConfigurationID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        COALESCE(d.DeletedBy, 1)
    FROM deleted d;
END;
GO

CREATE   TRIGGER system.trg_Configuration_Audit_Update
ON system.Configuration
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Configuration',
        i.ConfigurationID,
        'Update',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.ConfigurationID = d.ConfigurationID;
END;
GO

-- CONFIGURATION AUDIT TRIGGERS
CREATE   TRIGGER system.trg_Configuration_Audit_Insert
ON system.Configuration
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Configuration',
        i.ConfigurationID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.CreatedBy, 1)
    FROM inserted i;
END;