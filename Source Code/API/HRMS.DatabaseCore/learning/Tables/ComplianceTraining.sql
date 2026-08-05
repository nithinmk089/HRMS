CREATE TABLE [learning].[ComplianceTraining] (
    [ComplianceTrainingID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]             BIGINT         NOT NULL,
    [CourseID]             BIGINT         NOT NULL,
    [ComplianceType]       NVARCHAR (100) NOT NULL,
    [MandatoryFlag]        BIT            DEFAULT ((1)) NOT NULL,
    [CreatedBy]            BIGINT         NOT NULL,
    [CreatedDate]          DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]           BIGINT         NULL,
    [ModifiedDate]         DATETIME2 (7)  NULL,
    [DeletedBy]            BIGINT         NULL,
    [DeletedDate]          DATETIME2 (7)  NULL,
    [IsDeleted]            BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]           ROWVERSION     NOT NULL,
    CONSTRAINT [PK_ComplianceTraining] PRIMARY KEY CLUSTERED ([ComplianceTrainingID] ASC),
    CONSTRAINT [FK_ComplianceTraining_Course] FOREIGN KEY ([CourseID]) REFERENCES [learning].[Course] ([CourseID]),
    CONSTRAINT [FK_ComplianceTraining_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

