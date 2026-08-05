CREATE TABLE [onboarding].[OnboardingTaskAssignment] (
    [OnboardingTaskAssignmentID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]                   BIGINT        NOT NULL,
    [EmployeeID]                 BIGINT        NOT NULL,
    [OnboardingTaskID]           BIGINT        NOT NULL,
    [AssignedDate]               DATE          NOT NULL,
    [DueDate]                    DATE          NOT NULL,
    [CompletionDate]             DATE          NULL,
    [TaskStatus]                 NVARCHAR (50) DEFAULT ('Pending') NOT NULL,
    [CreatedBy]                  BIGINT        NOT NULL,
    [CreatedDate]                DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]                 BIGINT        NULL,
    [ModifiedDate]               DATETIME2 (7) NULL,
    [DeletedBy]                  BIGINT        NULL,
    [DeletedDate]                DATETIME2 (7) NULL,
    [IsDeleted]                  BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]                 ROWVERSION    NOT NULL,
    CONSTRAINT [PK_OnboardingTaskAssignment] PRIMARY KEY CLUSTERED ([OnboardingTaskAssignmentID] ASC),
    CONSTRAINT [FK_OnboardingTaskAssignment_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_OnboardingTaskAssignment_Task] FOREIGN KEY ([OnboardingTaskID]) REFERENCES [onboarding].[OnboardingTask] ([OnboardingTaskID]),
    CONSTRAINT [FK_OnboardingTaskAssignment_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_OnboardingTaskAssignment_Status]
    ON [onboarding].[OnboardingTaskAssignment]([TaskStatus] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_OnboardingTaskAssignment_Employee]
    ON [onboarding].[OnboardingTaskAssignment]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));

