
CREATE PROCEDURE onboarding.usp_OnboardingWorkflow_Search
    @TenantID       BIGINT,
    @EmployeeID     BIGINT = NULL,
    @WorkflowStatus NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        OnboardingWorkflowID,
        TenantID,
        EmployeeID,
        WorkflowCode,
        WorkflowName,
        StartDate,
        TargetCompletionDate,
        CompletionDate,
        WorkflowStatus
    FROM onboarding.OnboardingWorkflow
    WHERE TenantID = @TenantID
      AND (@EmployeeID IS NULL OR EmployeeID = @EmployeeID)
      AND (@WorkflowStatus IS NULL OR WorkflowStatus = @WorkflowStatus)
      AND IsDeleted = 0;
END