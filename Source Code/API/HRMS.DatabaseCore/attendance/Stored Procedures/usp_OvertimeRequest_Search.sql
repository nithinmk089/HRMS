
CREATE PROCEDURE attendance.usp_OvertimeRequest_Search
    @TenantID BIGINT,
    @EmployeeID BIGINT = NULL,
    @Status NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM attendance.vw_OvertimeSummary
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@EmployeeID IS NULL OR EmployeeID = @EmployeeID)
      AND (@Status IS NULL OR [Status] = @Status);
END;