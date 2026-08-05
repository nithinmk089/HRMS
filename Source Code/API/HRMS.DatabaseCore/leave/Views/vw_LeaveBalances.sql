
CREATE VIEW leave.vw_LeaveBalances
AS
SELECT 
    lb.LeaveBalanceID,
    lb.TenantID,
    lb.EmployeeID,
    e.EmployeeCode,
    e.FirstName + ' ' + COALESCE(e.MiddleName + ' ', '') + e.LastName AS EmployeeName,
    lb.LeaveTypeID,
    lt.LeaveCode,
    lt.LeaveName,
    lb.OpeningBalance,
    lb.AccruedBalance,
    lb.ConsumedBalance,
    lb.AvailableBalance,
    lb.IsDeleted
FROM leave.LeaveBalance lb
INNER JOIN hr.Employee e ON lb.EmployeeID = e.EmployeeID
INNER JOIN leave.LeaveType lt ON lb.LeaveTypeID = lt.LeaveTypeID;