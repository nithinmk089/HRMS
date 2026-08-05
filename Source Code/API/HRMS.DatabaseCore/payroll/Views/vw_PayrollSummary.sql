
-- payroll.vw_PayrollSummary.sql
CREATE VIEW payroll.vw_PayrollSummary
AS
SELECT 
    pr.PayrollRunID,
    pr.TenantID,
    pr.PayrollPeriodID,
    pp.PeriodCode,
    pr.RunDate,
    pr.RunStatus,
    COUNT(pt.EmployeeID) AS TotalEmployees,
    SUM(pt.GrossPay) AS TotalGrossPay,
    SUM(pt.Deductions) AS TotalDeductions,
    SUM(pt.NetPay) AS TotalNetPay
FROM payroll.PayrollRun pr
JOIN payroll.PayrollPeriod pp ON pr.PayrollPeriodID = pp.PayrollPeriodID
LEFT JOIN payroll.PayrollTransaction pt ON pr.PayrollRunID = pt.PayrollRunID
WHERE pr.IsDeleted = 0
GROUP BY pr.PayrollRunID, pr.TenantID, pr.PayrollPeriodID, pp.PeriodCode, pr.RunDate, pr.RunStatus;