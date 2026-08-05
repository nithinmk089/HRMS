CREATE TABLE [performance].[Goal] (
    [GoalID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT          NOT NULL,
    [EmployeeID]         BIGINT          NOT NULL,
    [PerformanceCycleID] BIGINT          NOT NULL,
    [GoalTitle]          NVARCHAR (200)  NOT NULL,
    [GoalDescription]    NVARCHAR (1000) NULL,
    [Weightage]          DECIMAL (5, 2)  NOT NULL,
    [TargetValue]        DECIMAL (18, 2) NOT NULL,
    [AchievementValue]   DECIMAL (18, 2) DEFAULT ((0.00)) NOT NULL,
    [GoalStatus]         NVARCHAR (50)   DEFAULT ('Pending') NOT NULL,
    [CreatedBy]          BIGINT          NOT NULL,
    [CreatedDate]        DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT          NULL,
    [ModifiedDate]       DATETIME2 (7)   NULL,
    [DeletedBy]          BIGINT          NULL,
    [DeletedDate]        DATETIME2 (7)   NULL,
    [IsDeleted]          BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION      NOT NULL,
    CONSTRAINT [PK_Goal] PRIMARY KEY CLUSTERED ([GoalID] ASC),
    CONSTRAINT [FK_Goal_Cycle] FOREIGN KEY ([PerformanceCycleID]) REFERENCES [performance].[PerformanceCycle] ([PerformanceCycleID]),
    CONSTRAINT [FK_Goal_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_Goal_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Goal_Cycle]
    ON [performance].[Goal]([PerformanceCycleID] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Goal_Employee]
    ON [performance].[Goal]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));

