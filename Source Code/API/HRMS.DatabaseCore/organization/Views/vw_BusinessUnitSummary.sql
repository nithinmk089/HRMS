
CREATE VIEW organization.vw_BusinessUnitSummary
AS
SELECT 
    bu.TenantID, bu.BusinessUnitID, bu.BusinessUnitCode, bu.BusinessUnitName,
    c.CompanyName,
    (SELECT COUNT(*) FROM organization.Department d WHERE d.BusinessUnitID = bu.BusinessUnitID AND d.IsDeleted = 0) AS DepartmentsCount
FROM organization.BusinessUnit bu
INNER JOIN security.Company c ON c.CompanyID = bu.CompanyID AND c.IsDeleted = 0
WHERE bu.IsDeleted = 0;