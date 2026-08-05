
-- asset.usp_AssetCategory_GetById.sql
CREATE PROCEDURE asset.usp_AssetCategory_GetById
    @AssetCategoryID BIGINT,
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AssetCategoryID, TenantID, CategoryCode, CategoryName, ParentCategoryID, [Description], IsDepreciable
    FROM asset.AssetCategory
    WHERE AssetCategoryID = @AssetCategoryID AND TenantID = @TenantID AND IsDeleted = 0;
END;