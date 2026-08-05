
-- asset.usp_Report_AssetAudit.sql
CREATE PROCEDURE asset.usp_Report_AssetAudit
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT au.AssetAuditID, am.AssetCode, am.AssetName, au.AuditDate, e.FirstName + ' ' + e.LastName AS Auditor, au.AuditStatus, au.Findings
    FROM asset.AssetAudit au
    JOIN asset.AssetMaster am ON au.AssetID = am.AssetID
    JOIN hr.Employee e ON au.AuditorID = e.EmployeeID
    WHERE au.TenantID = @TenantID AND au.IsDeleted = 0;
END;