CREATE TABLE [hr].[EmployeePromotion] (
    [EmployeePromotionID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]            BIGINT         NOT NULL,
    [EmployeeID]          BIGINT         NOT NULL,
    [OldDesignationID]    BIGINT         NOT NULL,
    [NewDesignationID]    BIGINT         NOT NULL,
    [OldGrade]            NVARCHAR (50)  NULL,
    [NewGrade]            NVARCHAR (50)  NULL,
    [EffectiveDate]       DATE           NOT NULL,
    [Reason]              NVARCHAR (500) NULL,
    [Status]              NVARCHAR (50)  DEFAULT ('Pending') NOT NULL,
    [CreatedBy]           BIGINT         NOT NULL,
    [CreatedDate]         DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]          BIGINT         NULL,
    [ModifiedDate]        DATETIME2 (7)  NULL,
    [DeletedBy]           BIGINT         NULL,
    [DeletedDate]         DATETIME2 (7)  NULL,
    [IsDeleted]           BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]          ROWVERSION     NOT NULL,
    CONSTRAINT [PK_EmployeePromotion] PRIMARY KEY CLUSTERED ([EmployeePromotionID] ASC),
    CONSTRAINT [FK_EmployeePromotion_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeePromotion_NewDesg] FOREIGN KEY ([NewDesignationID]) REFERENCES [organization].[Designation] ([DesignationID]),
    CONSTRAINT [FK_EmployeePromotion_OldDesg] FOREIGN KEY ([OldDesignationID]) REFERENCES [organization].[Designation] ([DesignationID]),
    CONSTRAINT [FK_EmployeePromotion_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Promotion_Status]
    ON [hr].[EmployeePromotion]([Status] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Promotion_Employee]
    ON [hr].[EmployeePromotion]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));


GO

CREATE TRIGGER hr.trg_EmployeePromotion_Audit
ON hr.EmployeePromotion
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
            'EmployeePromotion',
            i.EmployeePromotionID,
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
            'EmployeePromotion',
            i.EmployeePromotionID,
            'Update',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            COALESCE(i.ModifiedBy, 1)
        FROM inserted i
        INNER JOIN deleted d ON i.EmployeePromotionID = d.EmployeePromotionID;
    END
    ELSE IF @Action = 'Delete'
    BEGIN
        INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, OldValueJSON, NewValueJSON, PerformedBy)
        SELECT 
            d.TenantID,
            'EmployeePromotion',
            d.EmployeePromotionID,
            'Delete',
            (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
            NULL,
            COALESCE(d.DeletedBy, 1)
        FROM deleted d;
    END
END;