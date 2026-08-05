CREATE TABLE [learning].[LearningAnalytics] (
    [LearningAnalyticsID]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]             BIGINT        NOT NULL,
    [EmployeeID]           BIGINT        NOT NULL,
    [CompletedCourses]     INT           DEFAULT ((0)) NOT NULL,
    [ActiveCourses]        INT           DEFAULT ((0)) NOT NULL,
    [CertificationsEarned] INT           DEFAULT ((0)) NOT NULL,
    [CreatedBy]            BIGINT        NOT NULL,
    [CreatedDate]          DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]           BIGINT        NULL,
    [ModifiedDate]         DATETIME2 (7) NULL,
    [DeletedBy]            BIGINT        NULL,
    [DeletedDate]          DATETIME2 (7) NULL,
    [IsDeleted]            BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]           ROWVERSION    NOT NULL,
    CONSTRAINT [PK_LearningAnalytics] PRIMARY KEY CLUSTERED ([LearningAnalyticsID] ASC),
    CONSTRAINT [FK_LearningAnalytics_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_LearningAnalytics_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

