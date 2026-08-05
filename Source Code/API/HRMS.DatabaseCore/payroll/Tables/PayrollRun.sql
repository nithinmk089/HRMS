CREATE TABLE [payroll].[PayrollRun] (
    [PayrollRunID]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]        BIGINT        NOT NULL,
    [PayrollPeriodID] BIGINT        NOT NULL,
    [RunDate]         DATETIME2 (7) NOT NULL,
    [RunStatus]       NVARCHAR (50) DEFAULT ('Pending') NOT NULL,
    [CreatedBy]       BIGINT        NOT NULL,
    [CreatedDate]     DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]      BIGINT        NULL,
    [ModifiedDate]    DATETIME2 (7) NULL,
    [DeletedBy]       BIGINT        NULL,
    [DeletedDate]     DATETIME2 (7) NULL,
    [IsDeleted]       BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]      ROWVERSION    NOT NULL,
    CONSTRAINT [PK_PayrollRun] PRIMARY KEY CLUSTERED ([PayrollRunID] ASC),
    CONSTRAINT [FK_PayrollRun_Period] FOREIGN KEY ([PayrollPeriodID]) REFERENCES [payroll].[PayrollPeriod] ([PayrollPeriodID]),
    CONSTRAINT [FK_PayrollRun_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_PayrollRun_Period]
    ON [payroll].[PayrollRun]([PayrollPeriodID] ASC) WHERE ([IsDeleted]=(0));

