
CREATE VIEW organization.vw_DepartmentSummary
AS
SELECT 
    d.TenantID, d.DepartmentID, d.DepartmentCode, d.DepartmentName,
    bu.BusinessUnitName, c.CompanyName,
    (SELECT COUNT(*) FROM security.[User] u WHERE u.TenantID = d.TenantID AND u.IsDeleted = 0) AS ActiveUsersCount -- Dummy counter placeholder for employee mapping
FROM organization.Department d
INNER JOIN organization.BusinessUnit bu ON bu.BusinessUnitID = d.BusinessUnitID AND bu.IsDeleted = 0
INNER JOIN security.Company c ON c.CompanyID = bu.CompanyID AND c.IsDeleted = 0
WHERE d.IsDeleted = 0;