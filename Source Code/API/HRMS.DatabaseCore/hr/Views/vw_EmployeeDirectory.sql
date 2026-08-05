
CREATE VIEW hr.vw_EmployeeDirectory
AS
SELECT 
    e.EmployeeID,
    e.TenantID,
    e.EmployeeCode,
    e.EmployeeNumber,
    e.FirstName,
    e.MiddleName,
    e.LastName,
    e.PersonalEmail,
    e.MobileNumber,
    e.[Status],
    d.DepartmentName,
    dg.DesignationName,
    l.LocationName,
    ee.JoiningDate,
    ee.EmploymentType
FROM hr.Employee e
LEFT JOIN hr.EmployeeEmployment ee ON e.EmployeeID = ee.EmployeeID AND ee.IsDeleted = 0
LEFT JOIN organization.Department d ON ee.DepartmentID = d.DepartmentID
LEFT JOIN organization.Designation dg ON ee.DesignationID = dg.DesignationID
LEFT JOIN organization.Location l ON ee.LocationID = l.LocationID
WHERE e.IsDeleted = 0;