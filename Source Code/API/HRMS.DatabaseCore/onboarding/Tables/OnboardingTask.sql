CREATE TABLE [onboarding].[OnboardingTask] (
    [OnboardingTaskID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]         BIGINT         NOT NULL,
    [TaskCode]         NVARCHAR (50)  NOT NULL,
    [TaskName]         NVARCHAR (100) NOT NULL,
    [TaskType]         NVARCHAR (50)  NOT NULL,
    [DueDays]          INT            DEFAULT ((0)) NOT NULL,
    [SequenceNo]       INT            DEFAULT ((0)) NOT NULL,
    [IsMandatory]      BIT            DEFAULT ((1)) NOT NULL,
    [CreatedBy]        BIGINT         NOT NULL,
    [CreatedDate]      DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]       BIGINT         NULL,
    [ModifiedDate]     DATETIME2 (7)  NULL,
    [DeletedBy]        BIGINT         NULL,
    [DeletedDate]      DATETIME2 (7)  NULL,
    [IsDeleted]        BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]       ROWVERSION     NOT NULL,
    CONSTRAINT [PK_OnboardingTask] PRIMARY KEY CLUSTERED ([OnboardingTaskID] ASC),
    CONSTRAINT [FK_OnboardingTask_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_OnboardingTask_Code]
    ON [onboarding].[OnboardingTask]([TenantID] ASC, [TaskCode] ASC) WHERE ([IsDeleted]=(0));


GO

CREATE TRIGGER onboarding.trg_OnboardingTask_Audit
ON onboarding.OnboardingTask
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
        'OnboardingTask',
        COALESCE(i.OnboardingTaskID, d.OnboardingTaskID),
        @Action,
        COALESCE(i.ModifiedBy, i.CreatedBy, d.ModifiedBy, 1),
        GETUTCDATE(),
        (SELECT d.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT i.* FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
    FROM inserted i
    FULL OUTER JOIN deleted d ON i.OnboardingTaskID = d.OnboardingTaskID;
END