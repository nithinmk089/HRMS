
-- payroll.usp_SalaryStructure_Update.sql
CREATE PROCEDURE payroll.usp_SalaryStructure_Update
    @SalaryStructureID BIGINT,
    @TenantID BIGINT,
    @StructureName NVARCHAR(100),
    @EffectiveTo DATE = NULL,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE payroll.SalaryStructure
    SET StructureName = @StructureName, EffectiveTo = @EffectiveTo, ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE()
    WHERE SalaryStructureID = @SalaryStructureID AND TenantID = @TenantID AND IsDeleted = 0;
END