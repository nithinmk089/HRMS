
-- asset.vw_AssetWarrantyStatus.sql
CREATE VIEW asset.vw_AssetWarrantyStatus
AS
SELECT 
    aw.AssetWarrantyID,
    aw.TenantID,
    aw.AssetID,
    am.AssetCode,
    am.AssetName,
    aw.WarrantyStartDate,
    aw.WarrantyEndDate,
    aw.WarrantyProvider,
    asset.fn_GetWarrantyStatus(aw.AssetID) AS WarrantyStatus
FROM asset.AssetWarranty aw
JOIN asset.AssetMaster am ON aw.AssetID = am.AssetID
WHERE aw.IsDeleted = 0;