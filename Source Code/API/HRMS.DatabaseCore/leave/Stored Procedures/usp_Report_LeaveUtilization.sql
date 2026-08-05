
CREATE PROCEDURE leave.usp_Report_LeaveUtilization
    @TenantID BIGINT,
    @LeaveTypeID BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM leave.vw_LeaveUtilization
    WHERE TenantID = @TenantID
      AND (@LeaveTypeID IS NULL OR LeaveTypeID = @LeaveTypeID);
END;