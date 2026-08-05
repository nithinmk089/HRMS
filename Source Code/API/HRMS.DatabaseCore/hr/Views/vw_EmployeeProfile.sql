
-- === 4. VIEWS ===
CREATE VIEW hr.vw_EmployeeProfile
AS
SELECT 
    e.EmployeeID,
    e.TenantID,
    e.EmployeeCode,
    e.EmployeeNumber,
    e.FirstName,
    e.MiddleName,
    e.LastName,
    e.PreferredName,
    e.Gender,
    e.DateOfBirth,
    e.MaritalStatus,
    e.Nationality,
    e.PersonalEmail,
    e.MobileNumber,
    e.[Status] AS EmployeeStatus,
    ee.EmploymentType,
    ee.JoiningDate,
    ee.ConfirmationDate,
    ee.ProbationEndDate,
    ee.NoticePeriodDays,
    ee.EmploymentStatus,
    ee.CompanyID,
    c.CompanyName,
    ee.BusinessUnitID,
    bu.BusinessUnitName,
    ee.DepartmentID,
    d.DepartmentName,
    ee.DesignationID,
    dg.DesignationName,
    dg.Grade,
    ee.LocationID,
    l.LocationName,
    ee.CostCenterID,
    cc.CostCenterName,
    m.ManagerID,
    (SELECT ISNULL(memp.FirstName, '') + CASE WHEN memp.MiddleName IS NOT NULL THEN ' ' + memp.MiddleName ELSE '' END + ' ' + ISNULL(memp.LastName, '') FROM hr.Employee memp WHERE memp.EmployeeID = m.ManagerID) AS ManagerFullName
FROM hr.Employee e
LEFT JOIN hr.EmployeeEmployment ee ON e.EmployeeID = ee.EmployeeID AND ee.IsDeleted = 0
LEFT JOIN security.Company c ON ee.CompanyID = c.CompanyID
LEFT JOIN organization.BusinessUnit bu ON ee.BusinessUnitID = bu.BusinessUnitID
LEFT JOIN organization.Department d ON ee.DepartmentID = d.DepartmentID
LEFT JOIN organization.Designation dg ON ee.DesignationID = dg.DesignationID
LEFT JOIN organization.Location l ON ee.LocationID = l.LocationID
LEFT JOIN organization.CostCenter cc ON ee.CostCenterID = cc.CostCenterID
LEFT JOIN hr.EmployeeManager m ON e.EmployeeID = m.EmployeeID AND m.IsDeleted = 0 AND (m.EffectiveTo IS NULL OR m.EffectiveTo >= CAST(GETUTCDATE() AS DATE))
WHERE e.IsDeleted = 0;