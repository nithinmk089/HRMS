CREATE TABLE [payroll].[SalaryComponent] (
    [SalaryComponentID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]          BIGINT         NOT NULL,
    [ComponentCode]     NVARCHAR (50)  NOT NULL,
    [ComponentName]     NVARCHAR (100) NOT NULL,
    [ComponentType]     NVARCHAR (50)  NOT NULL,
    [CalculationMethod] NVARCHAR (50)  NOT NULL,
    [TaxableFlag]       BIT            DEFAULT ((1)) NOT NULL,
    [CreatedBy]         BIGINT         NOT NULL,
    [CreatedDate]       DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        BIGINT         NULL,
    [ModifiedDate]      DATETIME2 (7)  NULL,
    [DeletedBy]         BIGINT         NULL,
    [DeletedDate]       DATETIME2 (7)  NULL,
    [IsDeleted]         BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL,
    CONSTRAINT [PK_SalaryComponent] PRIMARY KEY CLUSTERED ([SalaryComponentID] ASC),
    CONSTRAINT [FK_SalaryComponent_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

