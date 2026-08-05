
-- payroll.usp_PayrollRun_Create.sql
CREATE PROCEDURE payroll.usp_PayrollRun_Create
    @TenantID BIGINT,
    @PayrollPeriodID BIGINT,
    @CreatedBy BIGINT,
    @PayrollRunID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO payroll.PayrollRun (TenantID, PayrollPeriodID, RunDate, RunStatus, CreatedBy)
    VALUES (@TenantID, @PayrollPeriodID, GETUTCDATE(), 'Pending', @CreatedBy);
    SET @PayrollRunID = SCOPE_IDENTITY();
END