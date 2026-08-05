
-- payroll.fn_CalculateGrossPay.sql
CREATE FUNCTION payroll.fn_CalculateGrossPay
(
    @EmployeeID BIGINT,
    @TenantID BIGINT
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @Gross DECIMAL(18,2) = 0;
    SELECT @Gross = GrossSalary 
    FROM payroll.EmployeeCompensation 
    WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IsDeleted = 0;
    RETURN ISNULL(@Gross, 0);
END