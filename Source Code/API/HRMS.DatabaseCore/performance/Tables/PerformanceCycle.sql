CREATE TABLE [performance].[PerformanceCycle] (
    [PerformanceCycleID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT         NOT NULL,
    [CycleCode]          NVARCHAR (50)  NOT NULL,
    [CycleName]          NVARCHAR (100) NOT NULL,
    [StartDate]          DATE           NOT NULL,
    [EndDate]            DATE           NOT NULL,
    [CycleStatus]        NVARCHAR (50)  DEFAULT ('Draft') NOT NULL,
    [CreatedBy]          BIGINT         NOT NULL,
    [CreatedDate]        DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT         NULL,
    [ModifiedDate]       DATETIME2 (7)  NULL,
    [DeletedBy]          BIGINT         NULL,
    [DeletedDate]        DATETIME2 (7)  NULL,
    [IsDeleted]          BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION     NOT NULL,
    CONSTRAINT [PK_PerformanceCycle] PRIMARY KEY CLUSTERED ([PerformanceCycleID] ASC),
    CONSTRAINT [FK_PerformanceCycle_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

