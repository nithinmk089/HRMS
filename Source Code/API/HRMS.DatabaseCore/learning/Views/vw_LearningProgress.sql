
-- learning.vw_LearningProgress.sql
CREATE VIEW learning.vw_LearningProgress
AS
SELECT e.EnrollmentID, e.TenantID, e.EmployeeID, e.CourseID, e.EnrollmentStatus,
       learning.fn_CalculateCompletionPercentage(e.EnrollmentID, e.TenantID) AS CompletionPercentage
FROM learning.Enrollment e
WHERE e.IsDeleted = 0;