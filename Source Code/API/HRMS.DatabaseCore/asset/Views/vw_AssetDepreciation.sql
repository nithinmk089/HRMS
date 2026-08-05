
-- asset.vw_AssetDepreciation.sql
CREATE VIEW asset.vw_AssetDepreciation
AS
SELECT 
    ad.AssetDepreciationID,
    ad.TenantID,
    ad.AssetID,
    am.AssetCode,
    am.AssetName,
    ad.DepreciationMethod,
    ad.DepreciationRate,
    ad.BookValue
FROM asset.AssetDepreciation ad
JOIN asset.AssetMaster am ON ad.AssetID = am.AssetID
WHERE ad.IsDeleted = 0;