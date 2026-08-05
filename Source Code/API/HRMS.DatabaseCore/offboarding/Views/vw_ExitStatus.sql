
CREATE VIEW offboarding.vw_ExitStatus
AS
SELECT 
    er.ExitRequestID,
    er.TenantID,
    er.EmployeeID,
    e.EmployeeCode,
    e.FirstName + ' ' + e.LastName AS EmployeeName,
    er.ResignationDate,
    er.LastWorkingDate,
    er.ExitReason,
    er.Status,
    offboarding.fn_GetClearanceStatus(er.EmployeeID, er.TenantID) AS ClearanceStatus,
    offboarding.fn_GetSettlementStatus(er.EmployeeID, er.TenantID) AS SettlementStatus
FROM offboarding.ExitRequest er
INNER JOIN hr.Employee e ON er.EmployeeID = e.EmployeeID
WHERE er.IsDeleted = 0;