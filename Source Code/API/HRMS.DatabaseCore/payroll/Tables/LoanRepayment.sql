CREATE TABLE [payroll].[LoanRepayment] (
    [LoanRepaymentID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]        BIGINT          NOT NULL,
    [LoanAdvanceID]   BIGINT          NOT NULL,
    [InstallmentNo]   INT             NOT NULL,
    [RepaymentAmount] DECIMAL (18, 2) NOT NULL,
    [DueDate]         DATE            NOT NULL,
    [PaymentDate]     DATE            NULL,
    [Status]          NVARCHAR (50)   DEFAULT ('Unpaid') NOT NULL,
    [CreatedBy]       BIGINT          NOT NULL,
    [CreatedDate]     DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]      BIGINT          NULL,
    [ModifiedDate]    DATETIME2 (7)   NULL,
    [DeletedBy]       BIGINT          NULL,
    [DeletedDate]     DATETIME2 (7)   NULL,
    [IsDeleted]       BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]      ROWVERSION      NOT NULL,
    CONSTRAINT [PK_LoanRepayment] PRIMARY KEY CLUSTERED ([LoanRepaymentID] ASC),
    CONSTRAINT [FK_LoanRepayment_Loan] FOREIGN KEY ([LoanAdvanceID]) REFERENCES [payroll].[LoanAdvance] ([LoanAdvanceID]),
    CONSTRAINT [FK_LoanRepayment_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

