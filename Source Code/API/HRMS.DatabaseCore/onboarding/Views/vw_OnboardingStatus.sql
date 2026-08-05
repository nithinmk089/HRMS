
-- ==========================================
-- 4. VIEWS
-- ==========================================

CREATE VIEW onboarding.vw_OnboardingStatus
AS
SELECT 
    ow.OnboardingWorkflowID,
    ow.TenantID,
    ow.EmployeeID,
    e.EmployeeCode,
    e.FirstName + ' ' + e.LastName AS EmployeeName,
    ow.WorkflowCode,
    ow.WorkflowName,
    ow.StartDate,
    ow.TargetCompletionDate,
    ow.CompletionDate,
    ow.WorkflowStatus,
    onboarding.fn_GetCompletionPercentage(ow.EmployeeID, ow.TenantID) AS CompletionPercentage
FROM onboarding.OnboardingWorkflow ow
INNER JOIN hr.Employee e ON ow.EmployeeID = e.EmployeeID
WHERE ow.IsDeleted = 0;