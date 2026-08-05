
CREATE PROCEDURE hr.usp_Report_EmployeePromotions
    @TenantID BIGINT,
    @StartDate DATE = NULL,
    @EndDate DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * 
    FROM hr.vw_EmployeePromotionHistory
    WHERE TenantID = @TenantID
      AND (@StartDate IS NULL OR EffectiveDate >= @StartDate)
      AND (@EndDate IS NULL OR EffectiveDate <= @EndDate)
    ORDER BY EffectiveDate DESC;
END;