
CREATE VIEW leave.vw_LeaveHistory
AS
SELECT 
    lr.LeaveRequestID,
    lr.TenantID,
    lr.EmployeeID,
    e.EmployeeCode,
    e.FirstName + ' ' + COALESCE(e.MiddleName + ' ', '') + e.LastName AS EmployeeName,
    lr.LeaveTypeID,
    lt.LeaveCode,
    lt.LeaveName,
    lr.FromDate,
    lr.ToDate,
    lr.TotalDays,
    lr.Reason,
    lr.[Status] AS RequestStatus,
    lr.CreatedDate,
    lr.IsDeleted
FROM leave.LeaveRequest lr
INNER JOIN hr.Employee e ON lr.EmployeeID = e.EmployeeID
INNER JOIN leave.LeaveType lt ON lr.LeaveTypeID = lt.LeaveTypeID;