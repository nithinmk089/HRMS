
-- asset.usp_Asset_Search.sql
CREATE PROCEDURE asset.usp_Asset_Search
    @TenantID BIGINT,
    @SearchText NVARCHAR(250) = NULL,
    @Status NVARCHAR(50) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM asset.AssetMaster
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@Status IS NULL OR [Status] = @Status)
      AND (@SearchText IS NULL OR AssetName LIKE '%' + @SearchText + '%' OR AssetCode LIKE '%' + @SearchText + '%' OR SerialNumber LIKE '%' + @SearchText + '%');

    SELECT AssetID, TenantID, AssetCode, AssetTag, AssetName, AssetCategoryID, Manufacturer, Model, SerialNumber, PurchaseDate, PurchaseCost, CurrentBookValue, [Status]
    FROM asset.AssetMaster
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@Status IS NULL OR [Status] = @Status)
      AND (@SearchText IS NULL OR AssetName LIKE '%' + @SearchText + '%' OR AssetCode LIKE '%' + @SearchText + '%' OR SerialNumber LIKE '%' + @SearchText + '%')
    ORDER BY AssetName
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;