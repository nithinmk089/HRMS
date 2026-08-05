
CREATE VIEW leave.vw_LeaveUtilization
AS
SELECT 
    lr.TenantID,
    lr.LeaveTypeID,
    lt.LeaveCode,
    lt.LeaveName,
    SUM(lr.TotalDays) AS TotalDaysUtilized,
    COUNT(lr.LeaveRequestID) AS TotalRequestsApproved
FROM leave.LeaveRequest lr
INNER JOIN leave.LeaveType lt ON lr.LeaveTypeID = lt.LeaveTypeID
WHERE lr.[Status] = 'Approved' AND lr.IsDeleted = 0
GROUP BY lr.TenantID, lr.LeaveTypeID, lt.LeaveCode, lt.LeaveName;