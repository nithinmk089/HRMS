
CREATE PROCEDURE leave.usp_LeaveBalance_Get
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @LeaveTypeID BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM leave.vw_LeaveBalances
    WHERE TenantID = @TenantID AND EmployeeID = @EmployeeID AND IsDeleted = 0
      AND (@LeaveTypeID IS NULL OR LeaveTypeID = @LeaveTypeID);
END;