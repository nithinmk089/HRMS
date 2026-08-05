
-- payroll.usp_SalaryStructure_Create.sql
CREATE PROCEDURE payroll.usp_SalaryStructure_Create
    @TenantID BIGINT,
    @StructureCode NVARCHAR(50),
    @StructureName NVARCHAR(100),
    @EffectiveFrom DATE,
    @CreatedBy BIGINT,
    @SalaryStructureID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO payroll.SalaryStructure (TenantID, StructureCode, StructureName, EffectiveFrom, CreatedBy)
    VALUES (@TenantID, @StructureCode, @StructureName, @EffectiveFrom, @CreatedBy);
    SET @SalaryStructureID = SCOPE_IDENTITY();
END