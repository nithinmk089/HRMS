CREATE TABLE [learning].[LearningCertification] (
    [LearningCertificationID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]                BIGINT         NOT NULL,
    [CourseID]                BIGINT         NOT NULL,
    [CertificationName]       NVARCHAR (200) NOT NULL,
    [ValidityMonths]          INT            DEFAULT ((12)) NOT NULL,
    [CreatedBy]               BIGINT         NOT NULL,
    [CreatedDate]             DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]              BIGINT         NULL,
    [ModifiedDate]            DATETIME2 (7)  NULL,
    [DeletedBy]               BIGINT         NULL,
    [DeletedDate]             DATETIME2 (7)  NULL,
    [IsDeleted]               BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]              ROWVERSION     NOT NULL,
    CONSTRAINT [PK_LearningCertification] PRIMARY KEY CLUSTERED ([LearningCertificationID] ASC),
    CONSTRAINT [FK_LearningCertification_Course] FOREIGN KEY ([CourseID]) REFERENCES [learning].[Course] ([CourseID]),
    CONSTRAINT [FK_LearningCertification_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

