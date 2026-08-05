
-- asset.vw_AssetAssignments.sql
CREATE VIEW asset.vw_AssetAssignments
AS
SELECT 
    aa.AssetAssignmentID,
    aa.TenantID,
    aa.AssetID,
    am.AssetCode,
    am.AssetName,
    aa.EmployeeID,
    e.EmployeeCode,
    e.FirstName + ' ' + e.LastName AS EmployeeName,
    aa.AssignedDate,
    aa.ExpectedReturnDate,
    aa.ReturnedDate,
    aa.AssignmentStatus
FROM asset.AssetAssignment aa
JOIN asset.AssetMaster am ON aa.AssetID = am.AssetID
JOIN hr.Employee e ON aa.EmployeeID = e.EmployeeID
WHERE aa.IsDeleted = 0;