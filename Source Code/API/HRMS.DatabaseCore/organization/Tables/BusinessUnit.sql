CREATE TABLE [organization].[BusinessUnit] (
    [BusinessUnitID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]         BIGINT        NOT NULL,
    [CompanyID]        BIGINT        NOT NULL,
    [BusinessUnitCode] VARCHAR (50)  NOT NULL,
    [BusinessUnitName] VARCHAR (200) NOT NULL,
    [CreatedBy]        BIGINT        NOT NULL,
    [CreatedDate]      DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]       BIGINT        NULL,
    [ModifiedDate]     DATETIME      NULL,
    [DeletedBy]        BIGINT        NULL,
    [DeletedDate]      DATETIME      NULL,
    [IsDeleted]        BIT           DEFAULT ((0)) NOT NULL,
    [VersionNo]        BIGINT        DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_BusinessUnit] PRIMARY KEY CLUSTERED ([BusinessUnitID] ASC),
    CONSTRAINT [FK_BusinessUnit_Company] FOREIGN KEY ([CompanyID]) REFERENCES [security].[Company] ([CompanyID]),
    CONSTRAINT [FK_BusinessUnit_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_BusinessUnit_Name]
    ON [organization].[BusinessUnit]([BusinessUnitName] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_BusinessUnit_Code]
    ON [organization].[BusinessUnit]([BusinessUnitCode] ASC) WHERE ([IsDeleted]=(0));


GO

CREATE   TRIGGER organization.trg_BusinessUnit_Audit_Delete
ON organization.BusinessUnit
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'BusinessUnit',
        d.BusinessUnitID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        COALESCE(d.DeletedBy, 1)
    FROM deleted d;
END;
GO

CREATE   TRIGGER organization.trg_BusinessUnit_Audit_Update
ON organization.BusinessUnit
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'BusinessUnit',
        i.BusinessUnitID,
        'Update',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.BusinessUnitID = d.BusinessUnitID;
END;
GO

-- BUSINESS UNIT AUDIT TRIGGERS
CREATE   TRIGGER organization.trg_BusinessUnit_Audit_Insert
ON organization.BusinessUnit
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'BusinessUnit',
        i.BusinessUnitID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.CreatedBy, 1)
    FROM inserted i;
END;