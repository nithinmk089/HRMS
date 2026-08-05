
-- payroll.usp_PayrollPeriod_Lock.sql
CREATE PROCEDURE payroll.usp_PayrollPeriod_Lock
    @PayrollPeriodID BIGINT,
    @TenantID BIGINT,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE payroll.PayrollPeriod
    SET ProcessingStatus = 'Locked', ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE()
    WHERE PayrollPeriodID = @PayrollPeriodID AND TenantID = @TenantID AND IsDeleted = 0;
END