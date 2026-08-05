CREATE TABLE [performance].[GoalProgress] (
    [GoalProgressID]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT         NOT NULL,
    [GoalID]             BIGINT         NOT NULL,
    [ProgressDate]       DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ProgressPercentage] DECIMAL (5, 2) NOT NULL,
    [Remarks]            NVARCHAR (500) NULL,
    [CreatedBy]          BIGINT         NOT NULL,
    [CreatedDate]        DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT         NULL,
    [ModifiedDate]       DATETIME2 (7)  NULL,
    [DeletedBy]          BIGINT         NULL,
    [DeletedDate]        DATETIME2 (7)  NULL,
    [IsDeleted]          BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION     NOT NULL,
    CONSTRAINT [PK_GoalProgress] PRIMARY KEY CLUSTERED ([GoalProgressID] ASC),
    CONSTRAINT [FK_GoalProgress_Goal] FOREIGN KEY ([GoalID]) REFERENCES [performance].[Goal] ([GoalID]),
    CONSTRAINT [FK_GoalProgress_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_GoalProgress_Goal]
    ON [performance].[GoalProgress]([GoalID] ASC) WHERE ([IsDeleted]=(0));

