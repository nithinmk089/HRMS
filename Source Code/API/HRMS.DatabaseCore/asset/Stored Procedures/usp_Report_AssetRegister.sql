
-- asset.usp_Report_AssetRegister.sql
CREATE PROCEDURE asset.usp_Report_AssetRegister
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT am.AssetID, am.AssetCode, am.AssetTag, am.AssetName, ac.CategoryName, am.Manufacturer, am.Model, am.SerialNumber, am.PurchaseDate, am.PurchaseCost, am.[Status]
    FROM asset.AssetMaster am
    LEFT JOIN asset.AssetCategory ac ON am.AssetCategoryID = ac.AssetCategoryID
    WHERE am.TenantID = @TenantID AND am.IsDeleted = 0;
END;