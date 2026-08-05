
-- tax.usp_TaxDeclaration_Create.sql
CREATE PROCEDURE tax.usp_TaxDeclaration_Create
    @TenantID BIGINT,
    @EmployeeID BIGINT,
    @FinancialYear NVARCHAR(20),
    @DeclaredAmount DECIMAL(18,2),
    @CreatedBy BIGINT,
    @EmployeeTaxDeclarationID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tax.EmployeeTaxDeclaration (TenantID, EmployeeID, FinancialYear, DeclaredAmount, Status, CreatedBy)
    VALUES (@TenantID, @EmployeeID, @FinancialYear, @DeclaredAmount, 'Pending', @CreatedBy);
    SET @EmployeeTaxDeclarationID = SCOPE_IDENTITY();
END