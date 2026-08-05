CREATE TABLE [learning].[LearningAssignment] (
    [LearningAssignmentID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]             BIGINT        NOT NULL,
    [EmployeeID]           BIGINT        NOT NULL,
    [CourseID]             BIGINT        NOT NULL,
    [AssignmentType]       NVARCHAR (50) DEFAULT ('Mandatory') NOT NULL,
    [DueDate]              DATE          NULL,
    [AssignmentStatus]     NVARCHAR (50) DEFAULT ('Assigned') NOT NULL,
    [CreatedBy]            BIGINT        NOT NULL,
    [CreatedDate]          DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]           BIGINT        NULL,
    [ModifiedDate]         DATETIME2 (7) NULL,
    [DeletedBy]            BIGINT        NULL,
    [DeletedDate]          DATETIME2 (7) NULL,
    [IsDeleted]            BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]           ROWVERSION    NOT NULL,
    CONSTRAINT [PK_LearningAssignment] PRIMARY KEY CLUSTERED ([LearningAssignmentID] ASC),
    CONSTRAINT [FK_LearningAssignment_Course] FOREIGN KEY ([CourseID]) REFERENCES [learning].[Course] ([CourseID]),
    CONSTRAINT [FK_LearningAssignment_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_LearningAssignment_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Assignment_Employee]
    ON [learning].[LearningAssignment]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));

