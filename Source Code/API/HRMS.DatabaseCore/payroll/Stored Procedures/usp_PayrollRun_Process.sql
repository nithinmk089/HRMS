
-- payroll.usp_PayrollRun_Process.sql
CREATE PROCEDURE payroll.usp_PayrollRun_Process
    @PayrollRunID BIGINT,
    @TenantID BIGINT,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    
    BEGIN TRANSACTION;
    
    DECLARE @PeriodDate DATE;
    SELECT @PeriodDate = pp.PeriodEndDate 
    FROM payroll.PayrollRun pr
    JOIN payroll.PayrollPeriod pp ON pr.PayrollPeriodID = pp.PayrollPeriodID
    WHERE pr.PayrollRunID = @PayrollRunID;
    
    -- Clear old transactions
    DELETE FROM payroll.PayrollTransaction WHERE PayrollRunID = @PayrollRunID AND TenantID = @TenantID;
    
    -- Process each employee in tenant
    INSERT INTO payroll.PayrollTransaction (TenantID, PayrollRunID, EmployeeID, GrossPay, Deductions, NetPay, CreatedBy)
    SELECT 
        @TenantID,
        @PayrollRunID,
        e.EmployeeID,
        payroll.fn_CalculateGrossPay(e.EmployeeID, @TenantID) AS Gross,
        payroll.fn_CalculateLoanDeduction(e.EmployeeID, @TenantID, @PeriodDate) AS Deductions,
        payroll.fn_CalculateGrossPay(e.EmployeeID, @TenantID) - payroll.fn_CalculateLoanDeduction(e.EmployeeID, @TenantID, @PeriodDate) AS Net,
        @ModifiedBy
    FROM hr.Employee e
    WHERE e.TenantID = @TenantID AND e.IsDeleted = 0;
    
    UPDATE payroll.PayrollRun
    SET RunStatus = 'Processed', ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE()
    WHERE PayrollRunID = @PayrollRunID AND TenantID = @TenantID;
    
    COMMIT TRANSACTION;
END