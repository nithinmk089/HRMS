
-- payroll.usp_EmployeeCompensation_Create.sql
CREATE PROCEDURE payroll.usp_EmployeeCompensation_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @SalaryStructureID BIGINT,
    @GrossSalary DECIMAL(18,2),
    @EffectiveFrom DATE,
    @CreatedBy BIGINT,
    @EmployeeCompensationID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO payroll.EmployeeCompensation (TenantID, EmployeeID, SalaryStructureID, GrossSalary, EffectiveFrom, CreatedBy)
    VALUES (@TenantID, @EmployeeID, @SalaryStructureID, @GrossSalary, @EffectiveFrom, @CreatedBy);
    SET @EmployeeCompensationID = SCOPE_IDENTITY();
END