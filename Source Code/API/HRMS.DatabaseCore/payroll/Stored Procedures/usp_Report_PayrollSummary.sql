
-- payroll.usp_Report_PayrollSummary.sql
CREATE PROCEDURE payroll.usp_Report_PayrollSummary
    @TenantID BIGINT,
    @PayrollPeriodID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM payroll.vw_PayrollSummary
    WHERE TenantID = @TenantID AND PayrollPeriodID = @PayrollPeriodID;
END