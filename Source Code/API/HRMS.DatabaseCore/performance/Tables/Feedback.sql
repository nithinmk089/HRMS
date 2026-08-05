CREATE TABLE [performance].[Feedback] (
    [FeedbackID]   BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]     BIGINT          NOT NULL,
    [EmployeeID]   BIGINT          NOT NULL,
    [FeedbackDate] DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [FeedbackText] NVARCHAR (2000) NOT NULL,
    [CreatedBy]    BIGINT          NOT NULL,
    [CreatedDate]  DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]   BIGINT          NULL,
    [ModifiedDate] DATETIME2 (7)   NULL,
    [DeletedBy]    BIGINT          NULL,
    [DeletedDate]  DATETIME2 (7)   NULL,
    [IsDeleted]    BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]   ROWVERSION      NOT NULL,
    CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED ([FeedbackID] ASC),
    CONSTRAINT [FK_Feedback_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_Feedback_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

