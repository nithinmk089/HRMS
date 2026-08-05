CREATE TABLE [learning].[AssessmentQuestion] (
    [AssessmentQuestionID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]             BIGINT          NOT NULL,
    [AssessmentID]         BIGINT          NOT NULL,
    [QuestionText]         NVARCHAR (1000) NOT NULL,
    [QuestionType]         NVARCHAR (50)   DEFAULT ('MCQ') NOT NULL,
    [CreatedBy]            BIGINT          NOT NULL,
    [CreatedDate]          DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]           BIGINT          NULL,
    [ModifiedDate]         DATETIME2 (7)   NULL,
    [DeletedBy]            BIGINT          NULL,
    [DeletedDate]          DATETIME2 (7)   NULL,
    [IsDeleted]            BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]           ROWVERSION      NOT NULL,
    CONSTRAINT [PK_AssessmentQuestion] PRIMARY KEY CLUSTERED ([AssessmentQuestionID] ASC),
    CONSTRAINT [FK_AssessmentQuestion_Assessment] FOREIGN KEY ([AssessmentID]) REFERENCES [learning].[Assessment] ([AssessmentID]),
    CONSTRAINT [FK_AssessmentQuestion_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

