
-- payroll.vw_SalaryRegister.sql
CREATE VIEW payroll.vw_SalaryRegister
AS
SELECT 
    pt.PayrollTransactionID,
    pt.TenantID,
    pt.EmployeeID,
    e.EmployeeCode,
    CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName,
    pr.PayrollPeriodID,
    pt.GrossPay,
    pt.Deductions,
    pt.NetPay
FROM payroll.PayrollTransaction pt
JOIN payroll.PayrollRun pr ON pt.PayrollRunID = pr.PayrollRunID
JOIN hr.Employee e ON pt.EmployeeID = e.EmployeeID
WHERE pt.IsDeleted = 0;