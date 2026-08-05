
CREATE PROCEDURE onboarding.usp_OnboardingTaskAssignment_Complete
    @OnboardingTaskAssignmentID BIGINT,
    @TenantID                   BIGINT,
    @ModifiedBy                 BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE onboarding.OnboardingTaskAssignment
        SET TaskStatus = 'Completed',
            CompletionDate = CAST(GETUTCDATE() AS DATE),
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE OnboardingTaskAssignmentID = @OnboardingTaskAssignmentID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END