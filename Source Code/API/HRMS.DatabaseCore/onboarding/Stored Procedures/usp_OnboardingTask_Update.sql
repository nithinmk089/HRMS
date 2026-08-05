
CREATE PROCEDURE onboarding.usp_OnboardingTask_Update
    @OnboardingTaskID BIGINT,
    @TenantID         BIGINT,
    @TaskCode         NVARCHAR(50),
    @TaskName         NVARCHAR(100),
    @TaskType         NVARCHAR(50),
    @DueDays          INT,
    @SequenceNo       INT,
    @IsMandatory      BIT,
    @ModifiedBy       BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE onboarding.OnboardingTask
        SET TaskCode = @TaskCode,
            TaskName = @TaskName,
            TaskType = @TaskType,
            DueDays = @DueDays,
            SequenceNo = @SequenceNo,
            IsMandatory = @IsMandatory,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE OnboardingTaskID = @OnboardingTaskID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END