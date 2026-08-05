CREATE TABLE [payroll].[EmployeeCompensation] (
    [EmployeeCompensationID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]               BIGINT          NOT NULL,
    [EmployeeID]             BIGINT          NOT NULL,
    [SalaryStructureID]      BIGINT          NOT NULL,
    [GrossSalary]            DECIMAL (18, 2) NOT NULL,
    [EffectiveFrom]          DATE            NOT NULL,
    [EffectiveTo]            DATE            NULL,
    [CreatedBy]              BIGINT          NOT NULL,
    [CreatedDate]            DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]             BIGINT          NULL,
    [ModifiedDate]           DATETIME2 (7)   NULL,
    [DeletedBy]              BIGINT          NULL,
    [DeletedDate]            DATETIME2 (7)   NULL,
    [IsDeleted]              BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]             ROWVERSION      NOT NULL,
    CONSTRAINT [PK_EmployeeCompensation] PRIMARY KEY CLUSTERED ([EmployeeCompensationID] ASC),
    CONSTRAINT [FK_EmployeeCompensation_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeCompensation_Structure] FOREIGN KEY ([SalaryStructureID]) REFERENCES [payroll].[SalaryStructure] ([SalaryStructureID]),
    CONSTRAINT [FK_EmployeeCompensation_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Compensation_Employee]
    ON [payroll].[EmployeeCompensation]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));

