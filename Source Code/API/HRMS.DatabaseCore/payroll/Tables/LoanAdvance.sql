CREATE TABLE [payroll].[LoanAdvance] (
    [LoanAdvanceID]      BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT          NOT NULL,
    [EmployeeID]         BIGINT          NOT NULL,
    [LoanType]           NVARCHAR (50)   NOT NULL,
    [PrincipalAmount]    DECIMAL (18, 2) NOT NULL,
    [InterestRate]       DECIMAL (5, 2)  DEFAULT ((0.00)) NOT NULL,
    [TenureMonths]       INT             NOT NULL,
    [MonthlyInstallment] DECIMAL (18, 2) NOT NULL,
    [Status]             NVARCHAR (50)   DEFAULT ('Pending') NOT NULL,
    [CreatedBy]          BIGINT          NOT NULL,
    [CreatedDate]        DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT          NULL,
    [ModifiedDate]       DATETIME2 (7)   NULL,
    [DeletedBy]          BIGINT          NULL,
    [DeletedDate]        DATETIME2 (7)   NULL,
    [IsDeleted]          BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION      NOT NULL,
    CONSTRAINT [PK_LoanAdvance] PRIMARY KEY CLUSTERED ([LoanAdvanceID] ASC),
    CONSTRAINT [FK_LoanAdvance_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_LoanAdvance_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

