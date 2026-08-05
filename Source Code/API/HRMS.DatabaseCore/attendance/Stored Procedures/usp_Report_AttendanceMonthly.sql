
CREATE PROCEDURE attendance.usp_Report_AttendanceMonthly
    @TenantID BIGINT,
    @YearNum INT,
    @MonthNum INT,
    @EmployeeID BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT m.*, e.FirstName + ' ' + COALESCE(e.MiddleName + ' ', '') + e.LastName AS EmployeeName, e.EmployeeCode
    FROM attendance.vw_AttendanceMonthly m
    INNER JOIN hr.Employee e ON m.EmployeeID = e.EmployeeID
    WHERE m.TenantID = @TenantID 
      AND m.YearNum = @YearNum 
      AND m.MonthNum = @MonthNum
      AND (@EmployeeID IS NULL OR m.EmployeeID = @EmployeeID);
END;