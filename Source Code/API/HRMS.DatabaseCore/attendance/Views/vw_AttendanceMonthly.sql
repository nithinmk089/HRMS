
CREATE VIEW attendance.vw_AttendanceMonthly
AS
SELECT 
    TenantID,
    EmployeeID,
    YEAR(AttendanceDate) AS YearNum,
    MONTH(AttendanceDate) AS MonthNum,
    SUM(CASE WHEN AttendanceStatus = 'Present' THEN 1 ELSE 0 END) AS PresentDays,
    SUM(CASE WHEN AttendanceStatus = 'Absent' THEN 1 ELSE 0 END) AS AbsentDays,
    SUM(CASE WHEN AttendanceStatus LIKE '%Late%' THEN 1 ELSE 0 END) AS LateDays,
    SUM(CASE WHEN AttendanceStatus LIKE '%EarlyOut%' THEN 1 ELSE 0 END) AS EarlyOutDays,
    SUM(CASE WHEN AttendanceStatus = 'OnLeave' THEN 1 ELSE 0 END) AS LeaveDays,
    COUNT(*) AS TotalDaysChecked
FROM attendance.Attendance
WHERE IsDeleted = 0
GROUP BY TenantID, EmployeeID, YEAR(AttendanceDate), MONTH(AttendanceDate);