
CREATE PROCEDURE leave.usp_Report_LeaveHistory
    @TenantID BIGINT,
    @EmployeeID BIGINT = NULL,
    @StartDate DATE = NULL,
    @EndDate DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM leave.vw_LeaveHistory
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@EmployeeID IS NULL OR EmployeeID = @EmployeeID)
      AND (@StartDate IS NULL OR FromDate >= @StartDate)
      AND (@EndDate IS NULL OR ToDate <= @EndDate);
END;