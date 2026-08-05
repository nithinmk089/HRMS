
CREATE VIEW hr.vw_EmployeeTransferHistory
AS
SELECT 
    t.EmployeeTransferID,
    t.TenantID,
    t.EmployeeID,
    e.FirstName + ' ' + e.LastName AS EmployeeFullName,
    e.EmployeeCode,
    t.FromDepartmentID,
    fd.DepartmentName AS FromDepartmentName,
    t.ToDepartmentID,
    td.DepartmentName AS ToDepartmentName,
    t.FromLocationID,
    fl.LocationName AS FromLocationName,
    t.ToLocationID,
    tl.LocationName AS ToLocationName,
    t.EffectiveDate,
    t.Reason,
    t.[Status],
    t.CreatedDate
FROM hr.EmployeeTransfer t
INNER JOIN hr.Employee e ON t.EmployeeID = e.EmployeeID
INNER JOIN organization.Department fd ON t.FromDepartmentID = fd.DepartmentID
INNER JOIN organization.Department td ON t.ToDepartmentID = td.DepartmentID
INNER JOIN organization.Location fl ON t.FromLocationID = fl.LocationID
INNER JOIN organization.Location tl ON t.ToLocationID = tl.LocationID
WHERE t.IsDeleted = 0;