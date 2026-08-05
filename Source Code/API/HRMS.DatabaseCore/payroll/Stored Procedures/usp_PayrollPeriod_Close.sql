
-- payroll.usp_PayrollPeriod_Close.sql
CREATE PROCEDURE payroll.usp_PayrollPeriod_Close
    @PayrollPeriodID BIGINT,
    @TenantID BIGINT,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE payroll.PayrollPeriod
    SET ProcessingStatus = 'Closed', ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE()
    WHERE PayrollPeriodID = @PayrollPeriodID AND TenantID = @TenantID AND IsDeleted = 0;
END