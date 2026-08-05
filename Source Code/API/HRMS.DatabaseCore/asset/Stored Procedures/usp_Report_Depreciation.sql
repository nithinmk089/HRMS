
-- asset.usp_Report_Depreciation.sql
CREATE PROCEDURE asset.usp_Report_Depreciation
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ad.AssetDepreciationID, am.AssetCode, am.AssetName, ad.DepreciationMethod, ad.DepreciationRate, am.PurchaseCost, ad.BookValue
    FROM asset.AssetDepreciation ad
    JOIN asset.AssetMaster am ON ad.AssetID = am.AssetID
    WHERE ad.TenantID = @TenantID AND ad.IsDeleted = 0;
END;