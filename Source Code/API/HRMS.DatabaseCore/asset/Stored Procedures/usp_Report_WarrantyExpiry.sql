
-- asset.usp_Report_WarrantyExpiry.sql
CREATE PROCEDURE asset.usp_Report_WarrantyExpiry
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT aw.AssetWarrantyID, am.AssetCode, am.AssetName, aw.WarrantyStartDate, aw.WarrantyEndDate, aw.WarrantyProvider
    FROM asset.AssetWarranty aw
    JOIN asset.AssetMaster am ON aw.AssetID = am.AssetID
    WHERE aw.TenantID = @TenantID AND aw.IsDeleted = 0;
END;