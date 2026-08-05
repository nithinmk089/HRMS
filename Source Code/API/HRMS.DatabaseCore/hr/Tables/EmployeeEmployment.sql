CREATE TABLE [hr].[EmployeeEmployment] (
    [EmployeeEmploymentID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]             BIGINT        NOT NULL,
    [EmployeeID]           BIGINT        NOT NULL,
    [CompanyID]            BIGINT        NOT NULL,
    [BusinessUnitID]       BIGINT        NOT NULL,
    [DepartmentID]         BIGINT        NOT NULL,
    [DesignationID]        BIGINT        NOT NULL,
    [LocationID]           BIGINT        NOT NULL,
    [CostCenterID]         BIGINT        NOT NULL,
    [EmploymentType]       NVARCHAR (50) NOT NULL,
    [JoiningDate]          DATE          NOT NULL,
    [ConfirmationDate]     DATE          NULL,
    [ProbationEndDate]     DATE          NULL,
    [NoticePeriodDays]     INT           DEFAULT ((0)) NOT NULL,
    [EmploymentStatus]     NVARCHAR (50) DEFAULT ('Active') NOT NULL,
    [CreatedBy]            BIGINT        NOT NULL,
    [CreatedDate]          DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]           BIGINT        NULL,
    [ModifiedDate]         DATETIME2 (7) NULL,
    [DeletedBy]            BIGINT        NULL,
    [DeletedDate]          DATETIME2 (7) NULL,
    [IsDeleted]            BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]           ROWVERSION    NOT NULL,
    CONSTRAINT [PK_EmployeeEmployment] PRIMARY KEY CLUSTERED ([EmployeeEmploymentID] ASC),
    CONSTRAINT [FK_EmployeeEmployment_BusinessUnit] FOREIGN KEY ([BusinessUnitID]) REFERENCES [organization].[BusinessUnit] ([BusinessUnitID]),
    CONSTRAINT [FK_EmployeeEmployment_Company] FOREIGN KEY ([CompanyID]) REFERENCES [security].[Company] ([CompanyID]),
    CONSTRAINT [FK_EmployeeEmployment_CostCenter] FOREIGN KEY ([CostCenterID]) REFERENCES [organization].[CostCenter] ([CostCenterID]),
    CONSTRAINT [FK_EmployeeEmployment_Department] FOREIGN KEY ([DepartmentID]) REFERENCES [organization].[Department] ([DepartmentID]),
    CONSTRAINT [FK_EmployeeEmployment_Designation] FOREIGN KEY ([DesignationID]) REFERENCES [organization].[Designation] ([DesignationID]),
    CONSTRAINT [FK_EmployeeEmployment_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeEmployment_Location] FOREIGN KEY ([LocationID]) REFERENCES [organization].[Location] ([LocationID]),
    CONSTRAINT [FK_EmployeeEmployment_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Employment_Status]
    ON [hr].[EmployeeEmployment]([EmploymentStatus] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Employment_Department]
    ON [hr].[EmployeeEmployment]([DepartmentID] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Employment_Employee]
    ON [hr].[EmployeeEmployment]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));


GO

CREATE TRIGGER hr.trg_EmployeeEmployment_Audit
ON hr.EmployeeEmployment
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
            'EmployeeEmployment',
            i.EmployeeEmploymentID,
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
            'EmployeeEmployment',
            i.EmployeeEmploymentID,
            'Update',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            COALESCE(i.ModifiedBy, 1)
        FROM inserted i
        INNER JOIN deleted d ON i.EmployeeEmploymentID = d.EmployeeEmploymentID;
    END
    ELSE IF @Action = 'Delete'
    BEGIN
        INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
        SELECT 
            d.TenantID,
            'EmployeeEmployment',
            d.EmployeeEmploymentID,
            'Delete',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            NULL,
            COALESCE(d.DeletedBy, 1)
        FROM deleted d;
    END
END;