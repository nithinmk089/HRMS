CREATE TABLE [payroll].[PayrollTransaction] (
    [PayrollTransactionID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]             BIGINT          NOT NULL,
    [PayrollRunID]         BIGINT          NOT NULL,
    [EmployeeID]           BIGINT          NOT NULL,
    [GrossPay]             DECIMAL (18, 2) NOT NULL,
    [Deductions]           DECIMAL (18, 2) NOT NULL,
    [NetPay]               DECIMAL (18, 2) NOT NULL,
    [CreatedBy]            BIGINT          NOT NULL,
    [CreatedDate]          DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]           BIGINT          NULL,
    [ModifiedDate]         DATETIME2 (7)   NULL,
    [DeletedBy]            BIGINT          NULL,
    [DeletedDate]          DATETIME2 (7)   NULL,
    [IsDeleted]            BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]           ROWVERSION      NOT NULL,
    CONSTRAINT [PK_PayrollTransaction] PRIMARY KEY CLUSTERED ([PayrollTransactionID] ASC),
    CONSTRAINT [FK_PayrollTransaction_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_PayrollTransaction_Run] FOREIGN KEY ([PayrollRunID]) REFERENCES [payroll].[PayrollRun] ([PayrollRunID]),
    CONSTRAINT [FK_PayrollTransaction_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_PayrollTransaction_Run]
    ON [payroll].[PayrollTransaction]([PayrollRunID] ASC) WHERE ([IsDeleted]=(0));

