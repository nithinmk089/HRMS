
-- asset.usp_Report_AssetAssignments.sql
CREATE PROCEDURE asset.usp_Report_AssetAssignments
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT aa.AssetAssignmentID, am.AssetCode, am.AssetName, e.EmployeeCode, e.FirstName + ' ' + e.LastName AS EmployeeName, aa.AssignedDate, aa.ExpectedReturnDate, aa.ReturnedDate, aa.AssignmentStatus
    FROM asset.AssetAssignment aa
    JOIN asset.AssetMaster am ON aa.AssetID = am.AssetID
    JOIN hr.Employee e ON aa.EmployeeID = e.EmployeeID
    WHERE aa.TenantID = @TenantID AND aa.IsDeleted = 0;
END;