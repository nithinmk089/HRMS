
CREATE PROCEDURE onboarding.usp_OnboardingTask_Create
    @TenantID         BIGINT,
    @TaskCode         NVARCHAR(50),
    @TaskName         NVARCHAR(100),
    @TaskType         NVARCHAR(50),
    @DueDays          INT,
    @SequenceNo       INT,
    @IsMandatory      BIT,
    @CreatedBy        BIGINT,
    @OnboardingTaskID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO onboarding.OnboardingTask (
            TenantID, TaskCode, TaskName, TaskType, DueDays, SequenceNo, IsMandatory, CreatedBy
        )
        VALUES (
            @TenantID, @TaskCode, @TaskName, @TaskType, @DueDays, @SequenceNo, @IsMandatory, @CreatedBy
        );

        SET @OnboardingTaskID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END