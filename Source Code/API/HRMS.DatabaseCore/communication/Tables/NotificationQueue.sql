CREATE TABLE [communication].[NotificationQueue] (
    [NotificationQueueID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]            BIGINT        NOT NULL,
    [Sender]              VARCHAR (200) NULL,
    [Recipient]           VARCHAR (500) NOT NULL,
    [Channel]             VARCHAR (50)  NOT NULL,
    [BusinessEvent]       VARCHAR (100) NOT NULL,
    [Subject]             VARCHAR (200) NULL,
    [Body]                VARCHAR (MAX) NOT NULL,
    [Status]              VARCHAR (50)  NOT NULL,
    [ErrorMessage]        VARCHAR (MAX) NULL,
    [RetryCount]          INT           DEFAULT ((0)) NOT NULL,
    [MaxRetries]          INT           DEFAULT ((3)) NOT NULL,
    [NextRunDate]         DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [SentDate]            DATETIME      NULL,
    [DeliveredDate]       DATETIME      NULL,
    [ReadDate]            DATETIME      NULL,
    [EscalationStatus]    VARCHAR (50)  DEFAULT ('None') NOT NULL,
    [CreatedBy]           BIGINT        NOT NULL,
    [CreatedDate]         DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]          BIGINT        NULL,
    [ModifiedDate]        DATETIME      NULL,
    CONSTRAINT [PK_NotificationQueue] PRIMARY KEY CLUSTERED ([NotificationQueueID] ASC),
    CONSTRAINT [FK_NotificationQueue_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_NotificationQueue_NextRun]
    ON [communication].[NotificationQueue]([NextRunDate] ASC) WHERE ([Status] IN ('Queued', 'Failed'));


GO
CREATE NONCLUSTERED INDEX [IX_NotificationQueue_Tenant_Status]
    ON [communication].[NotificationQueue]([TenantID] ASC, [Status] ASC);


GO

CREATE   TRIGGER communication.trg_NotificationQueue_Audit_Delete
ON communication.NotificationQueue
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'NotificationQueue',
        d.NotificationQueueID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        1
    FROM deleted d;
END;
GO

CREATE   TRIGGER communication.trg_NotificationQueue_Audit_Update
ON communication.NotificationQueue
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'NotificationQueue',
        i.NotificationQueueID,
        'Update',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.NotificationQueueID = d.NotificationQueueID;
END;
GO

-- NOTIFICATION QUEUE AUDIT TRIGGERS
CREATE   TRIGGER communication.trg_NotificationQueue_Audit_Insert
ON communication.NotificationQueue
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'NotificationQueue',
        i.NotificationQueueID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        i.CreatedBy
    FROM inserted i;
END;