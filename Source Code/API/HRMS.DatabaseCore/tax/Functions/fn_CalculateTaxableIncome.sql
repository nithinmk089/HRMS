
-- tax.fn_CalculateTaxableIncome.sql
CREATE FUNCTION tax.fn_CalculateTaxableIncome
(
    @GrossPay DECIMAL(18,2),
    @Declarations DECIMAL(18,2)
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @Net DECIMAL(18,2) = @GrossPay - @Declarations;
    IF @Net < 0 SET @Net = 0;
    RETURN @Net;
END