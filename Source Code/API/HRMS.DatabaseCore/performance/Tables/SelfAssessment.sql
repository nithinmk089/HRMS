CREATE TABLE [performance].[SelfAssessment] (
    [SelfAssessmentID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT        NOT NULL,
    [EmployeeID]         BIGINT        NOT NULL,
    [PerformanceCycleID] BIGINT        NOT NULL,
    [AssessmentStatus]   NVARCHAR (50) DEFAULT ('Pending') NOT NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [CreatedDate]        DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT        NULL,
    [ModifiedDate]       DATETIME2 (7) NULL,
    [DeletedBy]          BIGINT        NULL,
    [DeletedDate]        DATETIME2 (7) NULL,
    [IsDeleted]          BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION    NOT NULL,
    CONSTRAINT [PK_SelfAssessment] PRIMARY KEY CLUSTERED ([SelfAssessmentID] ASC),
    CONSTRAINT [FK_SelfAssessment_Cycle] FOREIGN KEY ([PerformanceCycleID]) REFERENCES [performance].[PerformanceCycle] ([PerformanceCycleID]),
    CONSTRAINT [FK_SelfAssessment_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_SelfAssessment_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

