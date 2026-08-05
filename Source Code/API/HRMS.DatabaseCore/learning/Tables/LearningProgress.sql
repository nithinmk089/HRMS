CREATE TABLE [learning].[LearningProgress] (
    [LearningProgressID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]             BIGINT         NOT NULL,
    [EnrollmentID]         BIGINT         NOT NULL,
    [CompletionPercentage] DECIMAL (5, 2) DEFAULT ((0)) NOT NULL,
    [LastAccessedDate]     DATETIME2 (7)  NULL,
    [CreatedBy]            BIGINT         NOT NULL,
    [CreatedDate]          DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]           BIGINT         NULL,
    [ModifiedDate]         DATETIME2 (7)  NULL,
    [DeletedBy]            BIGINT         NULL,
    [DeletedDate]          DATETIME2 (7)  NULL,
    [IsDeleted]            BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]           ROWVERSION     NOT NULL,
    CONSTRAINT [PK_LearningProgress] PRIMARY KEY CLUSTERED ([LearningProgressID] ASC),
    CONSTRAINT [FK_LearningProgress_Enrollment] FOREIGN KEY ([EnrollmentID]) REFERENCES [learning].[Enrollment] ([EnrollmentID]),
    CONSTRAINT [FK_LearningProgress_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

