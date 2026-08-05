CREATE TABLE [learning].[AssessmentResult] (
    [AssessmentResultID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT         NOT NULL,
    [AssessmentID]       BIGINT         NOT NULL,
    [EmployeeID]         BIGINT         NOT NULL,
    [Score]              DECIMAL (5, 2) NOT NULL,
    [ResultStatus]       NVARCHAR (50)  DEFAULT ('Pending') NOT NULL,
    [CreatedBy]          BIGINT         NOT NULL,
    [CreatedDate]        DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT         NULL,
    [ModifiedDate]       DATETIME2 (7)  NULL,
    [DeletedBy]          BIGINT         NULL,
    [DeletedDate]        DATETIME2 (7)  NULL,
    [IsDeleted]          BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION     NOT NULL,
    CONSTRAINT [PK_AssessmentResult] PRIMARY KEY CLUSTERED ([AssessmentResultID] ASC),
    CONSTRAINT [FK_AssessmentResult_Assessment] FOREIGN KEY ([AssessmentID]) REFERENCES [learning].[Assessment] ([AssessmentID]),
    CONSTRAINT [FK_AssessmentResult_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_AssessmentResult_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

