CREATE TABLE [system].[AuditLog] (
    [AuditLogID]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]      BIGINT         NOT NULL,
    [TableName]     VARCHAR (250)  NOT NULL,
    [RecordID]      BIGINT         NOT NULL,
    [ActionType]    VARCHAR (50)   NOT NULL,
    [OldValueJSON]  NVARCHAR (MAX) NULL,
    [NewValueJSON]  NVARCHAR (MAX) NULL,
    [PerformedBy]   BIGINT         NOT NULL,
    [PerformedDate] DATETIME       DEFAULT (getutcdate()) NOT NULL,
    [IPAddress]     VARCHAR (100)  NULL,
    [BrowserInfo]   VARCHAR (500)  NULL,
    [CorrelationID] VARCHAR (100)  NULL,
    CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED ([AuditLogID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_AuditLog_Date]
    ON [system].[AuditLog]([PerformedDate] DESC);


GO
CREATE NONCLUSTERED INDEX [IX_AuditLog_PerformedBy]
    ON [system].[AuditLog]([PerformedBy] ASC, [PerformedDate] DESC);


GO
CREATE NONCLUSTERED INDEX [IX_AuditLog_Table_Record]
    ON [system].[AuditLog]([TableName] ASC, [RecordID] ASC);

