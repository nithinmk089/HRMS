
-- asset.vw_AssetMaintenance.sql
CREATE VIEW asset.vw_AssetMaintenance
AS
SELECT 
    am.AssetMaintenanceID,
    am.TenantID,
    am.AssetID,
    ast.AssetCode,
    ast.AssetName,
    am.MaintenanceDate,
    am.MaintenanceType,
    am.VendorName,
    am.Cost,
    am.MaintenanceStatus
FROM asset.AssetMaintenance am
JOIN asset.AssetMaster ast ON am.AssetID = ast.AssetID
WHERE am.IsDeleted = 0;