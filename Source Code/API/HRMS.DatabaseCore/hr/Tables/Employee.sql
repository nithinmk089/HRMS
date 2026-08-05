CREATE TABLE [hr].[Employee] (
    [EmployeeID]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]       BIGINT         NOT NULL,
    [EmployeeCode]   NVARCHAR (50)  NOT NULL,
    [EmployeeNumber] NVARCHAR (50)  NOT NULL,
    [FirstName]      NVARCHAR (100) NOT NULL,
    [MiddleName]     NVARCHAR (100) NULL,
    [LastName]       NVARCHAR (100) NOT NULL,
    [PreferredName]  NVARCHAR (100) NULL,
    [Gender]         NVARCHAR (20)  NULL,
    [DateOfBirth]    DATE           NULL,
    [MaritalStatus]  NVARCHAR (50)  NULL,
    [Nationality]    NVARCHAR (100) NULL,
    [PersonalEmail]  NVARCHAR (200) NULL,
    [MobileNumber]   NVARCHAR (50)  NULL,
    [Status]         NVARCHAR (50)  DEFAULT ('Active') NOT NULL,
    [CreatedBy]      BIGINT         NOT NULL,
    [CreatedDate]    DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]     BIGINT         NULL,
    [ModifiedDate]   DATETIME2 (7)  NULL,
    [DeletedBy]      BIGINT         NULL,
    [DeletedDate]    DATETIME2 (7)  NULL,
    [IsDeleted]      BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]     ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([EmployeeID] ASC),
    CONSTRAINT [FK_Employee_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Employee_Status]
    ON [hr].[Employee]([Status] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Employee_Email]
    ON [hr].[Employee]([PersonalEmail] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Employee_Name]
    ON [hr].[Employee]([FirstName] ASC, [LastName] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Employee_EmployeeNumber]
    ON [hr].[Employee]([TenantID] ASC, [EmployeeNumber] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Employee_EmployeeCode]
    ON [hr].[Employee]([TenantID] ASC, [EmployeeCode] ASC) WHERE ([IsDeleted]=(0));


GO

-- === 5. TRIGGERS ===
CREATE TRIGGER hr.trg_Employee_Audit
ON hr.Employee
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
            'Employee',
            i.EmployeeID,
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
            'Employee',
            i.EmployeeID,
            'Update',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            COALESCE(i.ModifiedBy, 1)
        FROM inserted i
        INNER JOIN deleted d ON i.EmployeeID = d.EmployeeID;
    END
    ELSE IF @Action = 'Delete'
    BEGIN
        INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
        SELECT 
            d.TenantID,
            'Employee',
            d.EmployeeID,
            'Delete',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            NULL,
            COALESCE(d.DeletedBy, 1)
        FROM deleted d;
    END
END;