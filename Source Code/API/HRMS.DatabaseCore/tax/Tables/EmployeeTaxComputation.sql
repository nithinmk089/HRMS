CREATE TABLE [tax].[EmployeeTaxComputation] (
    [EmployeeTaxComputationID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]                 BIGINT          NOT NULL,
    [EmployeeID]               BIGINT          NOT NULL,
    [FinancialYear]            NVARCHAR (20)   NOT NULL,
    [TaxableIncome]            DECIMAL (18, 2) NOT NULL,
    [TaxAmount]                DECIMAL (18, 2) NOT NULL,
    [CreatedBy]                BIGINT          NOT NULL,
    [CreatedDate]              DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]               BIGINT          NULL,
    [ModifiedDate]             DATETIME2 (7)   NULL,
    [DeletedBy]                BIGINT          NULL,
    [DeletedDate]              DATETIME2 (7)   NULL,
    [IsDeleted]                BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]               ROWVERSION      NOT NULL,
    CONSTRAINT [PK_EmployeeTaxComputation] PRIMARY KEY CLUSTERED ([EmployeeTaxComputationID] ASC),
    CONSTRAINT [FK_EmployeeTaxComputation_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeTaxComputation_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

