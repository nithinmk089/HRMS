CREATE TABLE [organization].[Designation] (
    [DesignationID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]        BIGINT        NOT NULL,
    [DesignationCode] VARCHAR (50)  NOT NULL,
    [DesignationName] VARCHAR (200) NOT NULL,
    [Grade]           VARCHAR (50)  NULL,
    [CreatedBy]       BIGINT        NOT NULL,
    [CreatedDate]     DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]      BIGINT        NULL,
    [ModifiedDate]    DATETIME      NULL,
    [DeletedBy]       BIGINT        NULL,
    [DeletedDate]     DATETIME      NULL,
    [IsDeleted]       BIT           DEFAULT ((0)) NOT NULL,
    [VersionNo]       BIGINT        DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Designation] PRIMARY KEY CLUSTERED ([DesignationID] ASC),
    CONSTRAINT [FK_Designation_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO

CREATE   TRIGGER organization.trg_Designation_Audit_Delete
ON organization.Designation
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'Designation',
        d.DesignationID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        COALESCE(d.DeletedBy, 1)
    FROM deleted d;
END;
GO

CREATE   TRIGGER organization.trg_Designation_Audit_Update
ON organization.Designation
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Designation',
        i.DesignationID,
        'Update',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.DesignationID = d.DesignationID;
END;
GO

-- DESIGNATION AUDIT TRIGGERS
CREATE   TRIGGER organization.trg_Designation_Audit_Insert
ON organization.Designation
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Designation',
        i.DesignationID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.CreatedBy, 1)
    FROM inserted i;
END;