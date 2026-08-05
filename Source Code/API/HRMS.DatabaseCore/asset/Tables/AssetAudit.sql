CREATE TABLE [asset].[AssetAudit] (
    [AssetAuditID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]     BIGINT          NOT NULL,
    [AssetID]      BIGINT          NOT NULL,
    [AuditDate]    DATE            NOT NULL,
    [AuditorID]    BIGINT          NOT NULL,
    [AuditStatus]  NVARCHAR (50)   DEFAULT ('Scheduled') NOT NULL,
    [Findings]     NVARCHAR (1000) NULL,
    [CreatedBy]    BIGINT          NOT NULL,
    [CreatedDate]  DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]   BIGINT          NULL,
    [ModifiedDate] DATETIME2 (7)   NULL,
    [DeletedBy]    BIGINT          NULL,
    [DeletedDate]  DATETIME2 (7)   NULL,
    [IsDeleted]    BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]   ROWVERSION      NOT NULL,
    CONSTRAINT [PK_AssetAudit] PRIMARY KEY CLUSTERED ([AssetAuditID] ASC),
    CONSTRAINT [FK_AssetAudit_Asset] FOREIGN KEY ([AssetID]) REFERENCES [asset].[AssetMaster] ([AssetID]),
    CONSTRAINT [FK_AssetAudit_Auditor] FOREIGN KEY ([AuditorID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_AssetAudit_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_AssetAudit_Asset]
    ON [asset].[AssetAudit]([AssetID] ASC) WHERE ([IsDeleted]=(0));

