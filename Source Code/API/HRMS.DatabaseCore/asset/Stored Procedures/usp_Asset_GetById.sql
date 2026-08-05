
-- asset.usp_Asset_GetById.sql
CREATE PROCEDURE asset.usp_Asset_GetById
    @AssetID BIGINT,
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AssetID, TenantID, AssetCode, AssetTag, AssetName, AssetCategoryID, Manufacturer, Model, SerialNumber, PurchaseDate, PurchaseCost, CurrentBookValue, [Status]
    FROM asset.AssetMaster
    WHERE AssetID = @AssetID AND TenantID = @TenantID AND IsDeleted = 0;
END;