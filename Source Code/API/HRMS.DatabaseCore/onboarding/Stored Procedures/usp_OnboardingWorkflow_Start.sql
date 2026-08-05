
CREATE PROCEDURE onboarding.usp_OnboardingWorkflow_Start
    @OnboardingWorkflowID BIGINT,
    @TenantID             BIGINT,
    @ModifiedBy           BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE onboarding.OnboardingWorkflow
        SET WorkflowStatus = 'In Progress',
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