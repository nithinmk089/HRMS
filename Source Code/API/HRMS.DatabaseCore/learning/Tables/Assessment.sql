CREATE TABLE [learning].[Assessment] (
    [AssessmentID]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]       BIGINT         NOT NULL,
    [CourseID]       BIGINT         NOT NULL,
    [AssessmentName] NVARCHAR (200) NOT NULL,
    [PassPercentage] DECIMAL (5, 2) DEFAULT ((60)) NOT NULL,
    [CreatedBy]      BIGINT         NOT NULL,
    [CreatedDate]    DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]     BIGINT         NULL,
    [ModifiedDate]   DATETIME2 (7)  NULL,
    [DeletedBy]      BIGINT         NULL,
    [DeletedDate]    DATETIME2 (7)  NULL,
    [IsDeleted]      BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]     ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Assessment] PRIMARY KEY CLUSTERED ([AssessmentID] ASC),
    CONSTRAINT [FK_Assessment_Course] FOREIGN KEY ([CourseID]) REFERENCES [learning].[Course] ([CourseID]),
    CONSTRAINT [FK_Assessment_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

