CREATE TABLE [payroll].[PayrollAudit] (
    [PayrollAuditID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]       BIGINT          NOT NULL,
    [PayrollRunID]   BIGINT          NOT NULL,
    [AuditDate]      DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [AuditRemarks]   NVARCHAR (1000) NULL,
    [CreatedBy]      BIGINT          NOT NULL,
    [CreatedDate]    DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]     BIGINT          NULL,
    [ModifiedDate]   DATETIME2 (7)   NULL,
    [DeletedBy]      BIGINT          NULL,
    [DeletedDate]    DATETIME2 (7)   NULL,
    [IsDeleted]      BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]     ROWVERSION      NOT NULL,
    CONSTRAINT [PK_PayrollAudit] PRIMARY KEY CLUSTERED ([PayrollAuditID] ASC),
    CONSTRAINT [FK_PayrollAudit_Run] FOREIGN KEY ([PayrollRunID]) REFERENCES [payroll].[PayrollRun] ([PayrollRunID]),
    CONSTRAINT [FK_PayrollAudit_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

