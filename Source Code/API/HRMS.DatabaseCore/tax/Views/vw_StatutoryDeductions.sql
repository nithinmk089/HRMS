
-- tax.vw_StatutoryDeductions.sql
CREATE VIEW tax.vw_StatutoryDeductions
AS
SELECT 
    sd.StatutoryDeductionID,
    sd.TenantID,
    sd.EmployeeID,
    e.EmployeeCode,
    CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName,
    sd.DeductionType,
    sd.DeductionAmount
FROM tax.StatutoryDeduction sd
JOIN hr.Employee e ON sd.EmployeeID = e.EmployeeID
WHERE sd.IsDeleted = 0;