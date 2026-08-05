
CREATE VIEW onboarding.vw_PendingTasks
AS
SELECT 
    ta.OnboardingTaskAssignmentID,
    ta.TenantID,
    ta.EmployeeID,
    e.FirstName + ' ' + e.LastName AS EmployeeName,
    ta.OnboardingTaskID,
    t.TaskCode,
    t.TaskName,
    ta.AssignedDate,
    ta.DueDate,
    ta.TaskStatus
FROM onboarding.OnboardingTaskAssignment ta
INNER JOIN onboarding.OnboardingTask t ON ta.OnboardingTaskID = t.OnboardingTaskID
INNER JOIN hr.Employee e ON ta.EmployeeID = e.EmployeeID
WHERE ta.TaskStatus <> 'Completed' AND ta.IsDeleted = 0;