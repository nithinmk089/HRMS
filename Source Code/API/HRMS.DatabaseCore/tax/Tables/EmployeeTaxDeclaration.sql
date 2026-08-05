CREATE TABLE [tax].[EmployeeTaxDeclaration] (
    [EmployeeTaxDeclarationID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]                 BIGINT          NOT NULL,
    [EmployeeID]               BIGINT          NOT NULL,
    [FinancialYear]            NVARCHAR (20)   NOT NULL,
    [DeclaredAmount]           DECIMAL (18, 2) NOT NULL,
    [Status]                   NVARCHAR (50)   DEFAULT ('Pending') NOT NULL,
    [CreatedBy]                BIGINT          NOT NULL,
    [CreatedDate]              DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]               BIGINT          NULL,
    [ModifiedDate]             DATETIME2 (7)   NULL,
    [DeletedBy]                BIGINT          NULL,
    [DeletedDate]              DATETIME2 (7)   NULL,
    [IsDeleted]                BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]               ROWVERSION      NOT NULL,
    CONSTRAINT [PK_EmployeeTaxDeclaration] PRIMARY KEY CLUSTERED ([EmployeeTaxDeclarationID] ASC),
    CONSTRAINT [FK_EmployeeTaxDeclaration_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeTaxDeclaration_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

