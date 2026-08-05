
-- asset.usp_AssetDepreciation_Report.sql
CREATE PROCEDURE asset.usp_AssetDepreciation_Report
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ad.AssetDepreciationID, ad.AssetID, am.AssetCode, am.AssetName, ad.DepreciationMethod, ad.DepreciationRate, am.PurchaseCost, ad.BookValue
    FROM asset.AssetDepreciation ad
    JOIN asset.AssetMaster am ON ad.AssetID = am.AssetID
    WHERE ad.TenantID = @TenantID AND ad.IsDeleted = 0;
END;