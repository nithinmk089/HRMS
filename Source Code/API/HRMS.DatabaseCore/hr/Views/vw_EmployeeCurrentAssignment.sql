
CREATE VIEW hr.vw_EmployeeCurrentAssignment
AS
SELECT 
    ee.EmployeeEmploymentID,
    ee.TenantID,
    ee.EmployeeID,
    e.EmployeeCode,
    e.FirstName + ' ' + e.LastName AS EmployeeFullName,
    ee.CompanyID,
    c.CompanyName,
    ee.BusinessUnitID,
    bu.BusinessUnitName,
    ee.DepartmentID,
    d.DepartmentName,
    ee.DesignationID,
    dg.DesignationName,
    ee.LocationID,
    l.LocationName,
    ee.CostCenterID,
    cc.CostCenterName,
    ee.EmploymentType,
    ee.JoiningDate,
    ee.EmploymentStatus
FROM hr.EmployeeEmployment ee
INNER JOIN hr.Employee e ON ee.EmployeeID = e.EmployeeID AND e.IsDeleted = 0 AND e.[Status] = 'Active'
LEFT JOIN security.Company c ON ee.CompanyID = c.CompanyID
LEFT JOIN organization.BusinessUnit bu ON ee.BusinessUnitID = bu.BusinessUnitID
LEFT JOIN organization.Department d ON ee.DepartmentID = d.DepartmentID
LEFT JOIN organization.Designation dg ON ee.DesignationID = dg.DesignationID
LEFT JOIN organization.Location l ON ee.LocationID = l.LocationID
LEFT JOIN organization.CostCenter cc ON ee.CostCenterID = cc.CostCenterID
WHERE ee.IsDeleted = 0 AND ee.EmploymentStatus = 'Active';