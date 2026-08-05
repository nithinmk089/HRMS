
-- payroll.fn_CalculateIncentive.sql
CREATE FUNCTION payroll.fn_CalculateIncentive
(
    @EmployeeID BIGINT,
    @TenantID BIGINT,
    @Period NVARCHAR(50)
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @Amount DECIMAL(18,2) = 0;
    SELECT @Amount = SUM(IncentiveAmount)
    FROM payroll.Incentive
    WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND IncentivePeriod = @Period AND Status = 'Approved' AND IsDeleted = 0;
    RETURN ISNULL(@Amount, 0);
END