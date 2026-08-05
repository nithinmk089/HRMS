
-- ==========================================
-- 6. ONBOARDING STORED PROCEDURES
-- ==========================================

CREATE PROCEDURE onboarding.usp_OnboardingWorkflow_Create
    @TenantID             BIGINT,
    @EmployeeID           BIGINT,
    @WorkflowCode         NVARCHAR(50),
    @WorkflowName         NVARCHAR(100),
    @StartDate            DATE,
    @TargetCompletionDate DATE,
    @CreatedBy            BIGINT,
    @OnboardingWorkflowID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO onboarding.OnboardingWorkflow (
            TenantID, EmployeeID, WorkflowCode, WorkflowName, StartDate, TargetCompletionDate, WorkflowStatus, CreatedBy
        )
        VALUES (
            @TenantID, @EmployeeID, @WorkflowCode, @WorkflowName, @StartDate, @TargetCompletionDate, 'Pending', @CreatedBy
        );

        SET @OnboardingWorkflowID = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END