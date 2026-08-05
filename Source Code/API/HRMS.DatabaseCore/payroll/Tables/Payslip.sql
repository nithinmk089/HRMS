CREATE TABLE [payroll].[Payslip] (
    [PayslipID]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]        BIGINT         NOT NULL,
    [EmployeeID]      BIGINT         NOT NULL,
    [PayrollPeriodID] BIGINT         NOT NULL,
    [PayslipNumber]   NVARCHAR (100) NOT NULL,
    [GeneratedDate]   DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       BIGINT         NOT NULL,
    [CreatedDate]     DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]      BIGINT         NULL,
    [ModifiedDate]    DATETIME2 (7)  NULL,
    [DeletedBy]       BIGINT         NULL,
    [DeletedDate]     DATETIME2 (7)  NULL,
    [IsDeleted]       BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]      ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Payslip] PRIMARY KEY CLUSTERED ([PayslipID] ASC),
    CONSTRAINT [FK_Payslip_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_Payslip_Period] FOREIGN KEY ([PayrollPeriodID]) REFERENCES [payroll].[PayrollPeriod] ([PayrollPeriodID]),
    CONSTRAINT [FK_Payslip_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Payslip_Period]
    ON [payroll].[Payslip]([PayrollPeriodID] ASC) WHERE ([IsDeleted]=(0));

