
CREATE PROCEDURE leave.usp_LeaveRequest_Search
    @TenantID BIGINT,
    @EmployeeID BIGINT = NULL,
    @Status NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM leave.vw_LeaveHistory
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@EmployeeID IS NULL OR EmployeeID = @EmployeeID)
      AND (@Status IS NULL OR RequestStatus = @Status);
END;