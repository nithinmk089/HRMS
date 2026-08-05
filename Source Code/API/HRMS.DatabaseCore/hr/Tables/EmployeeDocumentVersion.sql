CREATE TABLE [hr].[EmployeeDocumentVersion] (
    [EmployeeDocumentVersionID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]                  BIGINT         NOT NULL,
    [EmployeeDocumentID]        BIGINT         NOT NULL,
    [VersionNumber]             INT            NOT NULL,
    [FileName]                  NVARCHAR (250) NOT NULL,
    [FilePath]                  NVARCHAR (500) NOT NULL,
    [MimeType]                  NVARCHAR (100) NOT NULL,
    [CreatedBy]                 BIGINT         NOT NULL,
    [CreatedDate]               DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]                BIGINT         NULL,
    [ModifiedDate]              DATETIME2 (7)  NULL,
    [DeletedBy]                 BIGINT         NULL,
    [DeletedDate]               DATETIME2 (7)  NULL,
    [IsDeleted]                 BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]                ROWVERSION     NOT NULL,
    CONSTRAINT [PK_EmployeeDocumentVersion] PRIMARY KEY CLUSTERED ([EmployeeDocumentVersionID] ASC),
    CONSTRAINT [FK_EmployeeDocumentVersion_Document] FOREIGN KEY ([EmployeeDocumentID]) REFERENCES [hr].[EmployeeDocument] ([EmployeeDocumentID]),
    CONSTRAINT [FK_EmployeeDocumentVersion_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_DocumentVersion_Document]
    ON [hr].[EmployeeDocumentVersion]([EmployeeDocumentID] ASC) WHERE ([IsDeleted]=(0));

