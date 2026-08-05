
CREATE PROCEDURE onboarding.usp_OnboardingWorkflow_Update
    @OnboardingWorkflowID BIGINT,
    @TenantID             BIGINT,
    @WorkflowCode         NVARCHAR(50),
    @WorkflowName         NVARCHAR(100),
    @TargetCompletionDate DATE,
    @WorkflowStatus       NVARCHAR(50),
    @ModifiedBy           BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE onboarding.OnboardingWorkflow
        SET WorkflowCode = @WorkflowCode,
            WorkflowName = @WorkflowName,
            TargetCompletionDate = @TargetCompletionDate,
            WorkflowStatus = @WorkflowStatus,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE OnboardingWorkflowID = @OnboardingWorkflowID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END