
-- payroll.fn_CalculateBonus.sql
CREATE FUNCTION payroll.fn_CalculateBonus
(
    @EmployeeID BIGINT,
    @TenantID BIGINT,
    @Period NVARCHAR(50)
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @Amount DECIMAL(18,2) = 0;
    SELECT @Amount = SUM(BonusAmount)
    FROM payroll.Bonus
    WHERE EmployeeID = @EmployeeID AND TenantID = @TenantID AND BonusPeriod = @Period AND Status = 'Approved' AND IsDeleted = 0;
    RETURN ISNULL(@Amount, 0);
END