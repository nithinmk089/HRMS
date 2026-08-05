
-- learning.vw_AssessmentResults.sql
CREATE VIEW learning.vw_AssessmentResults
AS
SELECT ar.AssessmentResultID, ar.TenantID, ar.AssessmentID, ar.EmployeeID, ar.Score, ar.ResultStatus,
       a.AssessmentName, a.PassPercentage
FROM learning.AssessmentResult ar
INNER JOIN learning.Assessment a ON ar.AssessmentID = a.AssessmentID
WHERE ar.IsDeleted = 0;