
CREATE VIEW offboarding.vw_ClearanceStatus
AS
SELECT 
    cr.ClearanceRequestID,
    cr.TenantID,
    cr.EmployeeID,
    e.FirstName + ' ' + e.LastName AS EmployeeName,
    cr.ClearanceStatus,
    cr.InitiatedDate,
    cr.CompletedDate,
    (SELECT COUNT(*) FROM offboarding.ClearanceTask ct WHERE ct.ClearanceRequestID = cr.ClearanceRequestID AND ct.IsDeleted = 0) AS TotalTasks,
    (SELECT COUNT(*) FROM offboarding.ClearanceTask ct WHERE ct.ClearanceRequestID = cr.ClearanceRequestID AND ct.Status = 'Approved' AND ct.IsDeleted = 0) AS ApprovedTasks
FROM offboarding.ClearanceRequest cr
INNER JOIN hr.Employee e ON cr.EmployeeID = e.EmployeeID
WHERE cr.IsDeleted = 0;