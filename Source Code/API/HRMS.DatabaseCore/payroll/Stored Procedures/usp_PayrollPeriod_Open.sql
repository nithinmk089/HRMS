
-- ==========================================
-- 4. STORED PROCEDURES
-- ==========================================

-- payroll.usp_PayrollPeriod_Open.sql
CREATE PROCEDURE payroll.usp_PayrollPeriod_Open
    @PayrollPeriodID BIGINT,
    @TenantID BIGINT,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE payroll.PayrollPeriod
    SET ProcessingStatus = 'Open', ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE()
    WHERE PayrollPeriodID = @PayrollPeriodID AND TenantID = @TenantID AND IsDeleted = 0;
END