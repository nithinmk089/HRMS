
-- ==========================================
-- 2. FUNCTIONS
-- ==========================================
-- payroll.fn_CalculateNetPay.sql
CREATE FUNCTION payroll.fn_CalculateNetPay
(
    @GrossPay DECIMAL(18,2),
    @Deductions DECIMAL(18,2)
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    RETURN ISNULL(@GrossPay, 0) - ISNULL(@Deductions, 0);
END