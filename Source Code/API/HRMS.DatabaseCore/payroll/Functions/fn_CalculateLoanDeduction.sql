
-- payroll.fn_CalculateLoanDeduction.sql
CREATE FUNCTION payroll.fn_CalculateLoanDeduction
(
    @EmployeeID BIGINT,
    @TenantID BIGINT,
    @PeriodDate DATE
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @Deduction DECIMAL(18,2) = 0;
    SELECT @Deduction = SUM(lr.RepaymentAmount)
    FROM payroll.LoanRepayment lr
    JOIN payroll.LoanAdvance la ON lr.LoanAdvanceID = la.LoanAdvanceID
    WHERE la.EmployeeID = @EmployeeID 
      AND la.TenantID = @TenantID 
      AND lr.DueDate <= @PeriodDate 
      AND lr.Status = 'Unpaid' 
      AND lr.IsDeleted = 0 
      AND la.IsDeleted = 0;
    RETURN ISNULL(@Deduction, 0);
END