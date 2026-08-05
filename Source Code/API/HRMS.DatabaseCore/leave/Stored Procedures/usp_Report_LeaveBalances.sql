
CREATE PROCEDURE leave.usp_Report_LeaveBalances
    @TenantID BIGINT,
    @EmployeeID BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM leave.vw_LeaveBalances
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@EmployeeID IS NULL OR EmployeeID = @EmployeeID);
END;