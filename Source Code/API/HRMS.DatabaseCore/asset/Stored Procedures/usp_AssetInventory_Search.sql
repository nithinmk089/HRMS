
-- asset.usp_AssetInventory_Search.sql
CREATE PROCEDURE asset.usp_AssetInventory_Search
    @TenantID BIGINT,
    @LocationID BIGINT = NULL,
    @Status NVARCHAR(50) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM asset.AssetInventory
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@LocationID IS NULL OR LocationID = @LocationID)
      AND (@Status IS NULL OR InventoryStatus = @Status);

    SELECT AssetInventoryID, TenantID, AssetID, LocationID, Quantity, InventoryStatus
    FROM asset.AssetInventory
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@LocationID IS NULL OR LocationID = @LocationID)
      AND (@Status IS NULL OR InventoryStatus = @Status)
    ORDER BY AssetID
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;