CREATE TABLE [performance].[ManagerAssessment] (
    [ManagerAssessmentID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]            BIGINT        NOT NULL,
    [EmployeeID]          BIGINT        NOT NULL,
    [ManagerID]           BIGINT        NOT NULL,
    [PerformanceCycleID]  BIGINT        NOT NULL,
    [AssessmentStatus]    NVARCHAR (50) DEFAULT ('Pending') NOT NULL,
    [CreatedBy]           BIGINT        NOT NULL,
    [CreatedDate]         DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]          BIGINT        NULL,
    [ModifiedDate]        DATETIME2 (7) NULL,
    [DeletedBy]           BIGINT        NULL,
    [DeletedDate]         DATETIME2 (7) NULL,
    [IsDeleted]           BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]          ROWVERSION    NOT NULL,
    CONSTRAINT [PK_ManagerAssessment] PRIMARY KEY CLUSTERED ([ManagerAssessmentID] ASC),
    CONSTRAINT [FK_ManagerAssessment_Cycle] FOREIGN KEY ([PerformanceCycleID]) REFERENCES [performance].[PerformanceCycle] ([PerformanceCycleID]),
    CONSTRAINT [FK_ManagerAssessment_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_ManagerAssessment_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

