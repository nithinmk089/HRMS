
CREATE PROCEDURE onboarding.usp_OnboardingTaskAssignment_Assign
    @TenantID                   BIGINT,
    @EmployeeID                 BIGINT,
    @OnboardingTaskID           BIGINT,
    @AssignedDate               DATE,
    @DueDate                    DATE,
    @CreatedBy                  BIGINT,
    @OnboardingTaskAssignmentID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO onboarding.OnboardingTaskAssignment (
            TenantID, EmployeeID, OnboardingTaskID, AssignedDate, DueDate, TaskStatus, CreatedBy
        )
        VALUES (
            @TenantID, @EmployeeID, @OnboardingTaskID, @AssignedDate, @DueDate, 'Pending', @CreatedBy
        );

        SET @OnboardingTaskAssignmentID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END