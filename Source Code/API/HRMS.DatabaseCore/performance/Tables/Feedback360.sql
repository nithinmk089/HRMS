CREATE TABLE [performance].[Feedback360] (
    [Feedback360ID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]      BIGINT         NOT NULL,
    [EmployeeID]    BIGINT         NOT NULL,
    [ReviewerID]    BIGINT         NOT NULL,
    [FeedbackScore] DECIMAL (5, 2) NOT NULL,
    [CreatedBy]     BIGINT         NOT NULL,
    [CreatedDate]   DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]    BIGINT         NULL,
    [ModifiedDate]  DATETIME2 (7)  NULL,
    [DeletedBy]     BIGINT         NULL,
    [DeletedDate]   DATETIME2 (7)  NULL,
    [IsDeleted]     BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]    ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Feedback360] PRIMARY KEY CLUSTERED ([Feedback360ID] ASC),
    CONSTRAINT [FK_Feedback360_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_Feedback360_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

