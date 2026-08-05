
-- tax.vw_TaxComputation.sql
CREATE VIEW tax.vw_TaxComputation
AS
SELECT 
    tc.EmployeeTaxComputationID,
    tc.TenantID,
    tc.EmployeeID,
    e.EmployeeCode,
    CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName,
    tc.FinancialYear,
    tc.TaxableIncome,
    tc.TaxAmount
FROM tax.EmployeeTaxComputation tc
JOIN hr.Employee e ON tc.EmployeeID = e.EmployeeID
WHERE tc.IsDeleted = 0;