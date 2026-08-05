
CREATE VIEW attendance.vw_OvertimeSummary
AS
SELECT 
    o.OvertimeRequestID,
    o.TenantID,
    o.EmployeeID,
    e.EmployeeCode,
    e.FirstName + ' ' + COALESCE(e.MiddleName + ' ', '') + e.LastName AS EmployeeName,
    o.OvertimeDate,
    o.RequestedHours,
    o.[Status],
    o.IsDeleted
FROM attendance.OvertimeRequest o
INNER JOIN hr.Employee e ON o.EmployeeID = e.EmployeeID;