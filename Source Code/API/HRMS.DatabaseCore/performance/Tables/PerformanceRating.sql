CREATE TABLE [performance].[PerformanceRating] (
    [PerformanceRatingID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]            BIGINT         NOT NULL,
    [EmployeeID]          BIGINT         NOT NULL,
    [PerformanceCycleID]  BIGINT         NOT NULL,
    [RatingValue]         DECIMAL (5, 2) NOT NULL,
    [RatingRemarks]       NVARCHAR (500) NULL,
    [CreatedBy]           BIGINT         NOT NULL,
    [CreatedDate]         DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]          BIGINT         NULL,
    [ModifiedDate]        DATETIME2 (7)  NULL,
    [DeletedBy]           BIGINT         NULL,
    [DeletedDate]         DATETIME2 (7)  NULL,
    [IsDeleted]           BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]          ROWVERSION     NOT NULL,
    CONSTRAINT [PK_PerformanceRating] PRIMARY KEY CLUSTERED ([PerformanceRatingID] ASC),
    CONSTRAINT [FK_PerformanceRating_Cycle] FOREIGN KEY ([PerformanceCycleID]) REFERENCES [performance].[PerformanceCycle] ([PerformanceCycleID]),
    CONSTRAINT [FK_PerformanceRating_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_PerformanceRating_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

