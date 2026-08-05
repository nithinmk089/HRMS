CREATE TABLE [tax].[StatutoryDeduction] (
    [StatutoryDeductionID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]             BIGINT          NOT NULL,
    [EmployeeID]           BIGINT          NOT NULL,
    [DeductionType]        NVARCHAR (50)   NOT NULL,
    [DeductionAmount]      DECIMAL (18, 2) NOT NULL,
    [CreatedBy]            BIGINT          NOT NULL,
    [CreatedDate]          DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]           BIGINT          NULL,
    [ModifiedDate]         DATETIME2 (7)   NULL,
    [DeletedBy]            BIGINT          NULL,
    [DeletedDate]          DATETIME2 (7)   NULL,
    [IsDeleted]            BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]           ROWVERSION      NOT NULL,
    CONSTRAINT [PK_StatutoryDeduction] PRIMARY KEY CLUSTERED ([StatutoryDeductionID] ASC),
    CONSTRAINT [FK_StatutoryDeduction_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_StatutoryDeduction_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

