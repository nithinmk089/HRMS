
CREATE VIEW offboarding.vw_SettlementSummary
AS
SELECT 
    faf.FullAndFinalSettlementID,
    faf.TenantID,
    faf.EmployeeID,
    e.EmployeeCode,
    e.FirstName + ' ' + e.LastName AS EmployeeName,
    faf.SettlementAmount,
    faf.SettlementDate,
    faf.SettlementStatus
FROM offboarding.FullAndFinalSettlement faf
INNER JOIN hr.Employee e ON faf.EmployeeID = e.EmployeeID
WHERE faf.IsDeleted = 0;