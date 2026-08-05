CREATE TABLE [hr].[EmployeeDocument] (
    [EmployeeDocumentID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT         NOT NULL,
    [EmployeeID]         BIGINT         NOT NULL,
    [DocumentType]       NVARCHAR (50)  NOT NULL,
    [FileName]           NVARCHAR (250) NOT NULL,
    [FilePath]           NVARCHAR (500) NOT NULL,
    [MimeType]           NVARCHAR (100) NOT NULL,
    [VersionNumber]      INT            DEFAULT ((1)) NOT NULL,
    [CreatedBy]          BIGINT         NOT NULL,
    [CreatedDate]        DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT         NULL,
    [ModifiedDate]       DATETIME2 (7)  NULL,
    [DeletedBy]          BIGINT         NULL,
    [DeletedDate]        DATETIME2 (7)  NULL,
    [IsDeleted]          BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION     NOT NULL,
    CONSTRAINT [PK_EmployeeDocument] PRIMARY KEY CLUSTERED ([EmployeeDocumentID] ASC),
    CONSTRAINT [FK_EmployeeDocument_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeDocument_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Document_Employee]
    ON [hr].[EmployeeDocument]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));


GO

CREATE TRIGGER hr.trg_EmployeeDocument_Audit
ON hr.EmployeeDocument
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Action VARCHAR(10);
    IF EXISTS(SELECT 1 FROM inserted) AND EXISTS(SELECT 1 FROM deleted)
        SET @Action = 'Update';
    ELSE IF EXISTS(SELECT 1 FROM inserted)
        SET @Action = 'Insert';
    ELSE IF EXISTS(SELECT 1 FROM deleted)
        SET @Action = 'Delete';

    IF @Action = 'Insert'
    BEGIN
        INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
        SELECT 
            i.TenantID,
            'EmployeeDocument',
            i.EmployeeDocumentID,
            'Insert',
            NULL,
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            COALESCE(i.CreatedBy, 1)
        FROM inserted i;
    END
    ELSE IF @Action = 'Update'
    BEGIN
        INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
        SELECT 
            i.TenantID,
            'EmployeeDocument',
            i.EmployeeDocumentID,
            'Update',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            COALESCE(i.ModifiedBy, 1)
        FROM inserted i
        INNER JOIN deleted d ON i.EmployeeDocumentID = d.EmployeeDocumentID;
    END
    ELSE IF @Action = 'Delete'
    BEGIN
        INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
        SELECT 
            d.TenantID,
            'EmployeeDocument',
            d.EmployeeDocumentID,
            'Delete',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            NULL,
            COALESCE(d.DeletedBy, 1)
        FROM deleted d;
    END
END;