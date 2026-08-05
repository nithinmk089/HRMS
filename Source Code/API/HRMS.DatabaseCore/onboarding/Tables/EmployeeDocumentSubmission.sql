CREATE TABLE [onboarding].[EmployeeDocumentSubmission] (
    [EmployeeDocumentSubmissionID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]                     BIGINT         NOT NULL,
    [EmployeeID]                   BIGINT         NOT NULL,
    [DocumentType]                 NVARCHAR (100) NOT NULL,
    [FileName]                     NVARCHAR (255) NOT NULL,
    [FilePath]                     NVARCHAR (500) NOT NULL,
    [MimeType]                     NVARCHAR (100) NOT NULL,
    [UploadedDate]                 DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]                    BIGINT         NOT NULL,
    [CreatedDate]                  DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]                   BIGINT         NULL,
    [ModifiedDate]                 DATETIME2 (7)  NULL,
    [DeletedBy]                    BIGINT         NULL,
    [DeletedDate]                  DATETIME2 (7)  NULL,
    [IsDeleted]                    BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]                   ROWVERSION     NOT NULL,
    CONSTRAINT [PK_EmployeeDocumentSubmission] PRIMARY KEY CLUSTERED ([EmployeeDocumentSubmissionID] ASC),
    CONSTRAINT [FK_EmployeeDocumentSubmission_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeDocumentSubmission_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_DocumentSubmission_Employee]
    ON [onboarding].[EmployeeDocumentSubmission]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));

