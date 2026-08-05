CREATE TABLE [organization].[Location] (
    [LocationID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]     BIGINT        NOT NULL,
    [LocationCode] VARCHAR (50)  NOT NULL,
    [LocationName] VARCHAR (200) NOT NULL,
    [CountryCode]  VARCHAR (10)  NULL,
    [StateCode]    VARCHAR (10)  NULL,
    [City]         VARCHAR (100) NULL,
    [CreatedBy]    BIGINT        NOT NULL,
    [CreatedDate]  DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]   BIGINT        NULL,
    [ModifiedDate] DATETIME      NULL,
    [DeletedBy]    BIGINT        NULL,
    [DeletedDate]  DATETIME      NULL,
    [IsDeleted]    BIT           DEFAULT ((0)) NOT NULL,
    [VersionNo]    BIGINT        DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED ([LocationID] ASC),
    CONSTRAINT [FK_Location_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO

-- LOCATION AUDIT TRIGGERS
CREATE   TRIGGER organization.trg_Location_Audit_Insert
ON organization.Location
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Location',
        i.LocationID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.CreatedBy, 1)
    FROM inserted i;
END;
GO

CREATE   TRIGGER organization.trg_Location_Audit_Delete
ON organization.Location
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'Location',
        d.LocationID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        COALESCE(d.DeletedBy, 1)
    FROM deleted d;
END;
GO

CREATE   TRIGGER organization.trg_Location_Audit_Update
ON organization.Location
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Location',
        i.LocationID,
        'Update',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.LocationID = d.LocationID;
END;