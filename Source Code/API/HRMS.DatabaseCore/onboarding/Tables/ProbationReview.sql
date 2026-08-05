CREATE TABLE [onboarding].[ProbationReview] (
    [ProbationReviewID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]          BIGINT          NOT NULL,
    [EmployeeID]        BIGINT          NOT NULL,
    [ReviewDate]        DATE            NOT NULL,
    [ReviewStatus]      NVARCHAR (50)   NOT NULL,
    [ReviewerID]        BIGINT          NOT NULL,
    [Comments]          NVARCHAR (1000) NULL,
    [CreatedBy]         BIGINT          NOT NULL,
    [CreatedDate]       DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        BIGINT          NULL,
    [ModifiedDate]      DATETIME2 (7)   NULL,
    [DeletedBy]         BIGINT          NULL,
    [DeletedDate]       DATETIME2 (7)   NULL,
    [IsDeleted]         BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]        ROWVERSION      NOT NULL,
    CONSTRAINT [PK_ProbationReview] PRIMARY KEY CLUSTERED ([ProbationReviewID] ASC),
    CONSTRAINT [FK_ProbationReview_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_ProbationReview_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

