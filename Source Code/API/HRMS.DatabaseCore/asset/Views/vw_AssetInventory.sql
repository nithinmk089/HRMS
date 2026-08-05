
-- ==========================================
-- 3. VIEWS
-- ==========================================

-- asset.vw_AssetInventory.sql
CREATE VIEW asset.vw_AssetInventory
AS
SELECT 
    ai.AssetInventoryID,
    ai.TenantID,
    ai.AssetID,
    am.AssetCode,
    am.AssetName,
    ai.LocationID,
    loc.LocationName,
    ai.Quantity,
    ai.InventoryStatus
FROM asset.AssetInventory ai
JOIN asset.AssetMaster am ON ai.AssetID = am.AssetID
JOIN organization.Location loc ON ai.LocationID = loc.LocationID
WHERE ai.IsDeleted = 0;