CREATE TABLE [payroll].[PayrollAdjustment] (
    [PayrollAdjustmentID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]            BIGINT          NOT NULL,
    [EmployeeID]          BIGINT          NOT NULL,
    [PayrollPeriodID]     BIGINT          NOT NULL,
    [AdjustmentType]      NVARCHAR (50)   NOT NULL,
    [AdjustmentAmount]    DECIMAL (18, 2) NOT NULL,
    [Remarks]             NVARCHAR (500)  NULL,
    [AdjustmentStatus]    NVARCHAR (50)   DEFAULT ('Pending') NOT NULL,
    [CreatedBy]           BIGINT          NOT NULL,
    [CreatedDate]         DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]          BIGINT          NULL,
    [ModifiedDate]        DATETIME2 (7)   NULL,
    [DeletedBy]           BIGINT          NULL,
    [DeletedDate]         DATETIME2 (7)   NULL,
    [IsDeleted]           BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]          ROWVERSION      NOT NULL,
    CONSTRAINT [PK_PayrollAdjustment] PRIMARY KEY CLUSTERED ([PayrollAdjustmentID] ASC),
    CONSTRAINT [FK_PayrollAdjustment_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_PayrollAdjustment_Period] FOREIGN KEY ([PayrollPeriodID]) REFERENCES [payroll].[PayrollPeriod] ([PayrollPeriodID]),
    CONSTRAINT [FK_PayrollAdjustment_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

