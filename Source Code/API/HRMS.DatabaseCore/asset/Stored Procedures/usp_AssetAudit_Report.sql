
-- asset.usp_AssetAudit_Report.sql
CREATE PROCEDURE asset.usp_AssetAudit_Report
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT au.AssetAuditID, au.AssetID, am.AssetCode, am.AssetName, au.AuditDate, au.AuditorID, au.AuditStatus, au.Findings
    FROM asset.AssetAudit au
    JOIN asset.AssetMaster am ON au.AssetID = am.AssetID
    WHERE au.TenantID = @TenantID AND au.IsDeleted = 0;
END;