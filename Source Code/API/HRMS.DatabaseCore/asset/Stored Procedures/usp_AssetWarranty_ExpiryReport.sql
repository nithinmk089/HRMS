
-- asset.usp_AssetWarranty_ExpiryReport.sql
CREATE PROCEDURE asset.usp_AssetWarranty_ExpiryReport
    @TenantID BIGINT,
    @WithinDays INT = 30
AS
BEGIN
    SET NOCOUNT ON;
    SELECT aw.AssetWarrantyID, aw.AssetID, am.AssetCode, am.AssetName, aw.WarrantyEndDate, aw.WarrantyProvider
    FROM asset.AssetWarranty aw
    JOIN asset.AssetMaster am ON aw.AssetID = am.AssetID
    WHERE aw.TenantID = @TenantID AND aw.IsDeleted = 0
      AND aw.WarrantyEndDate <= DATEADD(day, @WithinDays, GETUTCDATE())
    ORDER BY aw.WarrantyEndDate;
END;