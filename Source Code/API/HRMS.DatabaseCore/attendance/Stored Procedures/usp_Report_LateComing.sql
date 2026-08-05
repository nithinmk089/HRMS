
CREATE PROCEDURE attendance.usp_Report_LateComing
    @TenantID BIGINT,
    @StartDate DATE,
    @EndDate DATE,
    @EmployeeID BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM attendance.vw_AttendanceDaily
    WHERE TenantID = @TenantID 
      AND IsDeleted = 0
      AND AttendanceStatus LIKE '%Late%'
      AND AttendanceDate >= @StartDate 
      AND AttendanceDate <= @EndDate
      AND (@EmployeeID IS NULL OR EmployeeID = @EmployeeID);
END;