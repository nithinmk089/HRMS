CREATE TABLE [hr].[EmployeeTransfer] (
    [EmployeeTransferID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]           BIGINT         NOT NULL,
    [EmployeeID]         BIGINT         NOT NULL,
    [FromDepartmentID]   BIGINT         NOT NULL,
    [ToDepartmentID]     BIGINT         NOT NULL,
    [FromLocationID]     BIGINT         NOT NULL,
    [ToLocationID]       BIGINT         NOT NULL,
    [EffectiveDate]      DATE           NOT NULL,
    [Reason]             NVARCHAR (500) NULL,
    [Status]             NVARCHAR (50)  DEFAULT ('Pending') NOT NULL,
    [CreatedBy]          BIGINT         NOT NULL,
    [CreatedDate]        DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]         BIGINT         NULL,
    [ModifiedDate]       DATETIME2 (7)  NULL,
    [DeletedBy]          BIGINT         NULL,
    [DeletedDate]        DATETIME2 (7)  NULL,
    [IsDeleted]          BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]         ROWVERSION     NOT NULL,
    CONSTRAINT [PK_EmployeeTransfer] PRIMARY KEY CLUSTERED ([EmployeeTransferID] ASC),
    CONSTRAINT [FK_EmployeeTransfer_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeTransfer_FromDept] FOREIGN KEY ([FromDepartmentID]) REFERENCES [organization].[Department] ([DepartmentID]),
    CONSTRAINT [FK_EmployeeTransfer_FromLoc] FOREIGN KEY ([FromLocationID]) REFERENCES [organization].[Location] ([LocationID]),
    CONSTRAINT [FK_EmployeeTransfer_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID]),
    CONSTRAINT [FK_EmployeeTransfer_ToDept] FOREIGN KEY ([ToDepartmentID]) REFERENCES [organization].[Department] ([DepartmentID]),
    CONSTRAINT [FK_EmployeeTransfer_ToLoc] FOREIGN KEY ([ToLocationID]) REFERENCES [organization].[Location] ([LocationID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Transfer_Status]
    ON [hr].[EmployeeTransfer]([Status] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Transfer_Employee]
    ON [hr].[EmployeeTransfer]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));


GO

CREATE TRIGGER hr.trg_EmployeeTransfer_Audit
ON hr.EmployeeTransfer
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
            'EmployeeTransfer',
            i.EmployeeTransferID,
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
            'EmployeeTransfer',
            i.EmployeeTransferID,
            'Update',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            COALESCE(i.ModifiedBy, 1)
        FROM inserted i
        INNER JOIN deleted d ON i.EmployeeTransferID = d.EmployeeTransferID;
    END
    ELSE IF @Action = 'Delete'
    BEGIN
        INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
        SELECT 
            d.TenantID,
            'EmployeeTransfer',
            d.EmployeeTransferID,
            'Delete',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            NULL,
            COALESCE(d.DeletedBy, 1)
        FROM deleted d;
    END
END;