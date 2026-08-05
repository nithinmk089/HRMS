CREATE TABLE [onboarding].[EmployeeDocumentVerification] (
    [EmployeeDocumentVerificationID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]                       BIGINT         NOT NULL,
    [EmployeeDocumentSubmissionID]   BIGINT         NOT NULL,
    [VerificationStatus]             NVARCHAR (50)  DEFAULT ('Pending') NOT NULL,
    [VerificationRemarks]            NVARCHAR (500) NULL,
    [VerifiedBy]                     BIGINT         NULL,
    [VerifiedDate]                   DATETIME2 (7)  NULL,
    [CreatedBy]                      BIGINT         NOT NULL,
    [CreatedDate]                    DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]                     BIGINT         NULL,
    [ModifiedDate]                   DATETIME2 (7)  NULL,
    [DeletedBy]                      BIGINT         NULL,
    [DeletedDate]                    DATETIME2 (7)  NULL,
    [IsDeleted]                      BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]                     ROWVERSION     NOT NULL,
    CONSTRAINT [PK_EmployeeDocumentVerification] PRIMARY KEY CLUSTERED ([EmployeeDocumentVerificationID] ASC),
    CONSTRAINT [FK_EmployeeDocumentVerification_Submission] FOREIGN KEY ([EmployeeDocumentSubmissionID]) REFERENCES [onboarding].[EmployeeDocumentSubmission] ([EmployeeDocumentSubmissionID]),
    CONSTRAINT [FK_EmployeeDocumentVerification_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

