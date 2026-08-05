
-- payroll.usp_Payslip_Generate.sql
CREATE PROCEDURE payroll.usp_Payslip_Generate
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @PayrollPeriodID BIGINT,
    @CreatedBy BIGINT,
    @PayslipID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Num NVARCHAR(100) = 'PAY-' + CAST(@EmployeeID AS NVARCHAR) + '-' + CAST(@PayrollPeriodID AS NVARCHAR);
    
    INSERT INTO payroll.Payslip (TenantID, EmployeeID, PayrollPeriodID, PayslipNumber, GeneratedDate, CreatedBy)
    VALUES (@TenantID, @EmployeeID, @PayrollPeriodID, @Num, GETUTCDATE(), @CreatedBy);
    
    SET @PayslipID = SCOPE_IDENTITY();
END