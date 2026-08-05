
-- payroll.vw_Payslip.sql
CREATE VIEW payroll.vw_Payslip
AS
SELECT 
    p.PayslipID,
    p.TenantID,
    p.EmployeeID,
    e.EmployeeCode,
    CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName,
    p.PayrollPeriodID,
    pp.PeriodCode,
    pp.PeriodStartDate,
    pp.PeriodEndDate,
    p.PayslipNumber,
    p.GeneratedDate,
    ec.GrossSalary,
    pt.NetPay,
    pt.Deductions
FROM payroll.Payslip p
JOIN hr.Employee e ON p.EmployeeID = e.EmployeeID
JOIN payroll.PayrollPeriod pp ON p.PayrollPeriodID = pp.PayrollPeriodID
LEFT JOIN payroll.EmployeeCompensation ec ON e.EmployeeID = ec.EmployeeID AND ec.IsDeleted = 0
LEFT JOIN payroll.PayrollRun pr ON pr.PayrollPeriodID = pp.PayrollPeriodID AND pr.IsDeleted = 0
LEFT JOIN payroll.PayrollTransaction pt ON pt.PayrollRunID = pr.PayrollRunID AND pt.EmployeeID = e.EmployeeID
WHERE p.IsDeleted = 0;