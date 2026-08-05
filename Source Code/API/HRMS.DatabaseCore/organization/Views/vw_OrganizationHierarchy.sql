
CREATE VIEW organization.vw_OrganizationHierarchy
AS
SELECT 
    t.TenantID, t.TenantName,
    c.CompanyID, c.CompanyCode, c.CompanyName,
    bu.BusinessUnitID, bu.BusinessUnitCode, bu.BusinessUnitName,
    d.DepartmentID, d.DepartmentCode, d.DepartmentName, d.ParentDepartmentID
FROM security.Tenant t
INNER JOIN security.Company c ON c.TenantID = t.TenantID AND c.IsDeleted = 0
INNER JOIN organization.BusinessUnit bu ON bu.CompanyID = c.CompanyID AND bu.IsDeleted = 0
LEFT JOIN organization.Department d ON d.BusinessUnitID = bu.BusinessUnitID AND d.IsDeleted = 0
WHERE t.IsDeleted = 0;