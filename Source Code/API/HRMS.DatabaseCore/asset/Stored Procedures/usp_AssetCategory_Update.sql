
-- asset.usp_AssetCategory_Update.sql
CREATE PROCEDURE asset.usp_AssetCategory_Update
    @AssetCategoryID BIGINT,
    @TenantID BIGINT,
    @CategoryName NVARCHAR(100),
    @ParentCategoryID BIGINT = NULL,
    @Description NVARCHAR(500) = NULL,
    @IsDepreciable BIT = 1,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE asset.AssetCategory
        SET CategoryName = @CategoryName,
            ParentCategoryID = @ParentCategoryID,
            [Description] = @Description,
            IsDepreciable = @IsDepreciable,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE AssetCategoryID = @AssetCategoryID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;