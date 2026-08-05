
CREATE PROCEDURE onboarding.usp_OnboardingTask_Delete
    @OnboardingTaskID BIGINT,
    @TenantID         BIGINT,
    @DeletedBy        BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE onboarding.OnboardingTask
        SET IsDeleted = 1,
            DeletedBy = @DeletedBy,
            DeletedDate = GETUTCDATE()
        WHERE OnboardingTaskID = @OnboardingTaskID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END