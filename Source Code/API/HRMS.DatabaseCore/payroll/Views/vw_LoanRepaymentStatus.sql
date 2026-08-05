
-- ==========================================
-- 3. VIEWS
-- ==========================================

-- payroll.vw_LoanRepaymentStatus.sql
CREATE VIEW payroll.vw_LoanRepaymentStatus
AS
SELECT 
    la.LoanAdvanceID,
    la.TenantID,
    la.EmployeeID,
    la.LoanType,
    la.PrincipalAmount,
    la.TenureMonths,
    COUNT(lr.LoanRepaymentID) AS TotalInstallments,
    SUM(CASE WHEN lr.Status = 'Paid' THEN 1 ELSE 0 END) AS PaidInstallments,
    SUM(CASE WHEN lr.Status = 'Unpaid' THEN 1 ELSE 0 END) AS UnpaidInstallments,
    SUM(CASE WHEN lr.Status = 'Paid' THEN lr.RepaymentAmount ELSE 0 END) AS TotalRepaidAmount
FROM payroll.LoanAdvance la
LEFT JOIN payroll.LoanRepayment lr ON la.LoanAdvanceID = lr.LoanAdvanceID AND lr.IsDeleted = 0
WHERE la.IsDeleted = 0
GROUP BY la.LoanAdvanceID, la.TenantID, la.EmployeeID, la.LoanType, la.PrincipalAmount, la.TenureMonths;