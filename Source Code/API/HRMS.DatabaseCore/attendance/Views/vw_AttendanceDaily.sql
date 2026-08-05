
-- === 4. VIEWS ===
CREATE VIEW attendance.vw_AttendanceDaily
AS
SELECT 
    a.AttendanceID,
    a.TenantID,
    a.EmployeeID,
    e.EmployeeCode,
    e.FirstName + ' ' + COALESCE(e.MiddleName + ' ', '') + e.LastName AS EmployeeName,
    a.ShiftID,
    s.ShiftCode,
    s.ShiftName,
    a.AttendanceDate,
    a.ClockInTime,
    a.ClockOutTime,
    a.WorkingMinutes,
    a.AttendanceStatus,
    a.IsDeleted
FROM attendance.Attendance a
INNER JOIN hr.Employee e ON a.EmployeeID = e.EmployeeID
INNER JOIN attendance.Shift s ON a.ShiftID = s.ShiftID;