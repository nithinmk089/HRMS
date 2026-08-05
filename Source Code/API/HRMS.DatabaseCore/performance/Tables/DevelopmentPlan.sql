CREATE TABLE [performance].[DevelopmentPlan] (
    [DevelopmentPlanID]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]             BIGINT         NOT NULL,
    [EmployeeID]           BIGINT         NOT NULL,
    [PlanTitle]            NVARCHAR (200) NOT NULL,
    [TargetCompletionDate] DATE           NOT NULL,
    [CreatedBy]            BIGINT         NOT NULL,
    [CreatedDate]          DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]           BIGINT         NULL,
    [ModifiedDate]         DATETIME2 (7)  NULL,
    [DeletedBy]            BIGINT         NULL,
    [DeletedDate]          DATETIME2 (7)  NULL,
    [IsDeleted]            BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]           ROWVERSION     NOT NULL,
    CONSTRAINT [PK_DevelopmentPlan] PRIMARY KEY CLUSTERED ([DevelopmentPlanID] ASC),
    CONSTRAINT [FK_DevelopmentPlan_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_DevelopmentPlan_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

