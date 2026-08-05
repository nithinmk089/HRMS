
CREATE FUNCTION leave.fn_GetLeaveBalance
(
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @LeaveTypeID BIGINT
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @Balance DECIMAL(5,2);
    SELECT @Balance = AvailableBalance
    FROM leave.LeaveBalance
    WHERE TenantID = @TenantID 
      AND EmployeeID = @EmployeeID 
      AND LeaveTypeID = @LeaveTypeID 
      AND IsDeleted = 0;
      
    RETURN COALESCE(@Balance, 0);
END;