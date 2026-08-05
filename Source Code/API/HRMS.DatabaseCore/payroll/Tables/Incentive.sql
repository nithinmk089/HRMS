CREATE TABLE [payroll].[Incentive] (
    [IncentiveID]     BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]        BIGINT          NOT NULL,
    [EmployeeID]      BIGINT          NOT NULL,
    [IncentiveType]   NVARCHAR (50)   NOT NULL,
    [IncentiveAmount] DECIMAL (18, 2) NOT NULL,
    [IncentivePeriod] NVARCHAR (50)   NOT NULL,
    [Status]          NVARCHAR (50)   DEFAULT ('Pending') NOT NULL,
    [CreatedBy]       BIGINT          NOT NULL,
    [CreatedDate]     DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]      BIGINT          NULL,
    [ModifiedDate]    DATETIME2 (7)   NULL,
    [DeletedBy]       BIGINT          NULL,
    [DeletedDate]     DATETIME2 (7)   NULL,
    [IsDeleted]       BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]      ROWVERSION      NOT NULL,
    CONSTRAINT [PK_Incentive] PRIMARY KEY CLUSTERED ([IncentiveID] ASC),
    CONSTRAINT [FK_Incentive_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_Incentive_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

