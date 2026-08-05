
CREATE PROCEDURE attendance.usp_Report_OvertimeSummary
    @TenantID BIGINT,
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM attendance.vw_OvertimeSummary
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND OvertimeDate >= @StartDate AND OvertimeDate <= @EndDate;
END;