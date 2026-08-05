
-- payroll.usp_SalaryComponent_Create.sql
CREATE PROCEDURE payroll.usp_SalaryComponent_Create
    @TenantID BIGINT,
    @ComponentCode NVARCHAR(50),
    @ComponentName NVARCHAR(100),
    @ComponentType NVARCHAR(50),
    @CalculationMethod NVARCHAR(50),
    @TaxableFlag BIT,
    @CreatedBy BIGINT,
    @SalaryComponentID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO payroll.SalaryComponent (TenantID, ComponentCode, ComponentName, ComponentType, CalculationMethod, TaxableFlag, CreatedBy)
    VALUES (@TenantID, @ComponentCode, @ComponentName, @ComponentType, @CalculationMethod, @TaxableFlag, @CreatedBy);
    SET @SalaryComponentID = SCOPE_IDENTITY();
END