CREATE TABLE [organization].[Department] (
    [DepartmentID]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT        NOT NULL,
    [BusinessUnitID]     BIGINT        NOT NULL,
    [DepartmentCode]     VARCHAR (50)  NOT NULL,
    [DepartmentName]     VARCHAR (200) NOT NULL,
    [ParentDepartmentID] BIGINT        NULL,
    [CreatedBy]          BIGINT        NOT NULL,
    [CreatedDate]        DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT        NULL,
    [ModifiedDate]       DATETIME      NULL,
    [DeletedBy]          BIGINT        NULL,
    [DeletedDate]        DATETIME      NULL,
    [IsDeleted]          BIT           DEFAULT ((0)) NOT NULL,
    [VersionNo]          BIGINT        DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED ([DepartmentID] ASC),
    CONSTRAINT [FK_Department_BusinessUnit] FOREIGN KEY ([BusinessUnitID]) REFERENCES [organization].[BusinessUnit] ([BusinessUnitID]),
    CONSTRAINT [FK_Department_Parent] FOREIGN KEY ([ParentDepartmentID]) REFERENCES [organization].[Department] ([DepartmentID]),
    CONSTRAINT [FK_Department_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Department_Parent]
    ON [organization].[Department]([ParentDepartmentID] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Department_Name]
    ON [organization].[Department]([DepartmentName] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Department_Code]
    ON [organization].[Department]([DepartmentCode] ASC) WHERE ([IsDeleted]=(0));


GO

CREATE   TRIGGER organization.trg_Department_Audit_Delete
ON organization.Department
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        d.TenantID,
        'Department',
        d.DepartmentID,
        'Delete',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        NULL,
        COALESCE(d.DeletedBy, 1)
    FROM deleted d;
END;
GO

CREATE   TRIGGER organization.trg_Department_Audit_Update
ON organization.Department
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Department',
        i.DepartmentID,
        'Update',
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.ModifiedBy, 1)
    FROM inserted i
    INNER JOIN deleted d ON i.DepartmentID = d.DepartmentID;
END;
GO

CREATE   TRIGGER organization.trg_Department_Audit_Insert
ON organization.Department
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
    SELECT 
        i.TenantID,
        'Department',
        i.DepartmentID,
        'Insert',
        NULL,
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        COALESCE(i.CreatedBy, 1)
    FROM inserted i;
END;