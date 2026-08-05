
-- payroll.usp_LoanAdvance_Create.sql
CREATE PROCEDURE payroll.usp_LoanAdvance_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @LoanType NVARCHAR(50),
    @PrincipalAmount DECIMAL(18,2),
    @InterestRate DECIMAL(5,2),
    @TenureMonths INT,
    @CreatedBy BIGINT,
    @LoanAdvanceID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Installment DECIMAL(18,2) = @PrincipalAmount / @TenureMonths;
    
    INSERT INTO payroll.LoanAdvance (TenantID, EmployeeID, LoanType, PrincipalAmount, InterestRate, TenureMonths, MonthlyInstallment, Status, CreatedBy)
    VALUES (@TenantID, @EmployeeID, @LoanType, @PrincipalAmount, @InterestRate, @TenureMonths, @Installment, 'Pending', @CreatedBy);
    
    SET @LoanAdvanceID = SCOPE_IDENTITY();
END