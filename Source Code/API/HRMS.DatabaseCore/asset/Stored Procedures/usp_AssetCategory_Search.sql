
-- asset.usp_AssetCategory_Search.sql
CREATE PROCEDURE asset.usp_AssetCategory_Search
    @TenantID BIGINT,
    @SearchText NVARCHAR(250) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM asset.AssetCategory
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR CategoryName LIKE '%' + @SearchText + '%' OR CategoryCode LIKE '%' + @SearchText + '%');

    SELECT AssetCategoryID, TenantID, CategoryCode, CategoryName, ParentCategoryID, [Description], IsDepreciable
    FROM asset.AssetCategory
    WHERE TenantID = @TenantID AND IsDeleted = 0
      AND (@SearchText IS NULL OR CategoryName LIKE '%' + @SearchText + '%' OR CategoryCode LIKE '%' + @SearchText + '%')
    ORDER BY CategoryName
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;