
CREATE VIEW attendance.vw_AttendanceSummary
AS
SELECT 
    TenantID,
    AttendanceDate,
    SUM(CASE WHEN AttendanceStatus = 'Present' THEN 1 ELSE 0 END) AS TotalPresent,
    SUM(CASE WHEN AttendanceStatus = 'Absent' THEN 1 ELSE 0 END) AS TotalAbsent,
    SUM(CASE WHEN AttendanceStatus LIKE '%Late%' THEN 1 ELSE 0 END) AS TotalLate,
    SUM(CASE WHEN AttendanceStatus = 'OnLeave' THEN 1 ELSE 0 END) AS TotalOnLeave
FROM attendance.Attendance
WHERE IsDeleted = 0
GROUP BY TenantID, AttendanceDate;