CREATE TABLE [security].[Company] (
    [CompanyID]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]     BIGINT        NOT NULL,
    [CompanyCode]  VARCHAR (50)  NOT NULL,
    [CompanyName]  VARCHAR (200) NOT NULL,
    [LegalName]    VARCHAR (300) NULL,
    [TaxNumber]    VARCHAR (100) NULL,
    [Email]        VARCHAR (200) NULL,
    [Phone]        VARCHAR (50)  NULL,
    [Website]      VARCHAR (200) NULL,
    [CreatedBy]    BIGINT        NOT NULL,
    [CreatedDate]  DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]   BIGINT        NULL,
    [ModifiedDate] DATETIME      NULL,
    [DeletedBy]    BIGINT        NULL,
    [DeletedDate]  DATETIME      NULL,
    [IsDeleted]    BIT           DEFAULT ((0)) NOT NULL,
    [VersionNo]    BIGINT        DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([CompanyID] ASC),
    CONSTRAINT [FK_Company_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID]),
    CONSTRAINT [UQ_Company_Code] UNIQUE NONCLUSTERED ([CompanyCode] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Company_Name]
    ON [security].[Company]([CompanyName] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Company_Code]
    ON [security].[Company]([CompanyCode] ASC) WHERE ([IsDeleted]=(0));


GO

-- COMPANY AUDIT TRIGGERS
CREATE   TRIGGER security.trg_Company_Audit_Insert
ON security.Company
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Company',
        i.CompanyID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        i.CreatedBy
    FROM inserted i;
END;
GO

CREATE   TRIGGER security.trg_Company_Audit_Delete
ON security.Company
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'Company',
        d.CompanyID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        COALESCE(d.DeletedBy, 1)
    FROM deleted d;
END;
GO

CREATE   TRIGGER security.trg_Company_Audit_Update
ON security.Company
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Company',
        i.CompanyID,
        CASE WHEN i.IsDeleted = 1 AND d.IsDeleted = 0 THEN 'Delete' ELSE 'Update' END,
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, i.DeletedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.CompanyID = d.CompanyID;
END;