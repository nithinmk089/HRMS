
CREATE VIEW hr.vw_EmployeeHierarchy
AS
SELECT 
    em.EmployeeManagerID,
    em.TenantID,
    em.EmployeeID,
    e.FirstName + ' ' + e.LastName AS EmployeeFullName,
    e.EmployeeCode,
    em.ManagerID,
    m.FirstName + ' ' + m.LastName AS ManagerFullName,
    m.EmployeeCode AS ManagerCode,
    em.EffectiveFrom,
    em.EffectiveTo
FROM hr.EmployeeManager em
INNER JOIN hr.Employee e ON em.EmployeeID = e.EmployeeID
INNER JOIN hr.Employee m ON em.ManagerID = m.EmployeeID
WHERE em.IsDeleted = 0;