CREATE TABLE [onboarding].[OnboardingWorkflow] (
    [OnboardingWorkflowID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]             BIGINT         NOT NULL,
    [EmployeeID]           BIGINT         NOT NULL,
    [WorkflowCode]         NVARCHAR (50)  NOT NULL,
    [WorkflowName]         NVARCHAR (100) NOT NULL,
    [StartDate]            DATE           NOT NULL,
    [TargetCompletionDate] DATE           NOT NULL,
    [CompletionDate]       DATE           NULL,
    [WorkflowStatus]       NVARCHAR (50)  DEFAULT ('Pending') NOT NULL,
    [CreatedBy]            BIGINT         NOT NULL,
    [CreatedDate]          DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]           BIGINT         NULL,
    [ModifiedDate]         DATETIME2 (7)  NULL,
    [DeletedBy]            BIGINT         NULL,
    [DeletedDate]          DATETIME2 (7)  NULL,
    [IsDeleted]            BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]           ROWVERSION     NOT NULL,
    CONSTRAINT [PK_OnboardingWorkflow] PRIMARY KEY CLUSTERED ([OnboardingWorkflowID] ASC),
    CONSTRAINT [FK_OnboardingWorkflow_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_OnboardingWorkflow_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_OnboardingWorkflow_Status]
    ON [onboarding].[OnboardingWorkflow]([WorkflowStatus] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_OnboardingWorkflow_Employee]
    ON [onboarding].[OnboardingWorkflow]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));


GO

-- ==========================================
-- 5. TRIGGERS
-- ==========================================

CREATE TRIGGER onboarding.trg_OnboardingWorkflow_Audit
ON onboarding.OnboardingWorkflow
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Action NVARCHAR(50) = 'INSERT';
    IF EXISTS(SELECT * FROM deleted)
    BEGIN
        SET @Action = CASE WHEN EXISTS(SELECT * FROM inserted) THEN 'UPDATE' ELSE 'DELETE' END;
    END

    INSERT INTO system.AuditLog (TenantID, TableName, RecordID, ActionType, PerformedBy, PerformedDate, OldValueJSON, NewValueJSON)
    SELECT 
        COALESCE(i.TenantID, d.TenantID),
        'OnboardingWorkflow',
        COALESCE(i.OnboardingWorkflowID, d.OnboardingWorkflowID),
        @Action,
        COALESCE(i.ModifiedBy, i.CreatedBy, d.ModifiedBy, 1),
        GETUTCDATE(),
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
    FROM inserted i
    FULL OUTER JOIN deleted d ON i.OnboardingWorkflowID = d.OnboardingWorkflowID;
END