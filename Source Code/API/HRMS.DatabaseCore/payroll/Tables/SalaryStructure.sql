CREATE TABLE [payroll].[SalaryStructure] (
    [SalaryStructureID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]          BIGINT         NOT NULL,
    [StructureCode]     NVARCHAR (50)  NOT NULL,
    [StructureName]     NVARCHAR (100) NOT NULL,
    [EffectiveFrom]     DATE           NOT NULL,
    [EffectiveTo]       DATE           NULL,
    [CreatedBy]         BIGINT         NOT NULL,
    [CreatedDate]       DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        BIGINT         NULL,
    [ModifiedDate]      DATETIME2 (7)  NULL,
    [DeletedBy]         BIGINT         NULL,
    [DeletedDate]       DATETIME2 (7)  NULL,
    [IsDeleted]         BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]        ROWVERSION     NOT NULL,
    CONSTRAINT [PK_SalaryStructure] PRIMARY KEY CLUSTERED ([SalaryStructureID] ASC),
    CONSTRAINT [FK_SalaryStructure_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

