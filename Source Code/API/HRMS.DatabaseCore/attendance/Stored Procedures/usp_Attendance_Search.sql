
CREATE PROCEDURE attendance.usp_Attendance_Search
    @TenantID BIGINT,
    @EmployeeID BIGINT = NULL,
    @StartDate DATE = NULL,
    @EndDate DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM attendance.vw_AttendanceDaily
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@EmployeeID IS NULL OR EmployeeID = @EmployeeID)
      AND (@StartDate IS NULL OR AttendanceDate >= @StartDate)
      AND (@EndDate IS NULL OR AttendanceDate <= @EndDate);
END;