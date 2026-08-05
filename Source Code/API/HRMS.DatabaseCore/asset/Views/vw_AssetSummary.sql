
-- asset.vw_AssetSummary.sql
CREATE VIEW asset.vw_AssetSummary
AS
SELECT 
    am.AssetID,
    am.TenantID,
    am.AssetCode,
    am.AssetTag,
    am.AssetName,
    am.AssetCategoryID,
    ac.CategoryName,
    am.Manufacturer,
    am.Model,
    am.SerialNumber,
    am.PurchaseDate,
    am.PurchaseCost,
    am.CurrentBookValue,
    am.Status,
    asset.fn_GetCurrentHolder(am.AssetID) AS CurrentHolder,
    asset.fn_GetWarrantyStatus(am.AssetID) AS WarrantyStatus
FROM asset.AssetMaster am
LEFT JOIN asset.AssetCategory ac ON am.AssetCategoryID = ac.AssetCategoryID
WHERE am.IsDeleted = 0;