
-- asset.usp_AssetCategory_Create.sql
CREATE PROCEDURE asset.usp_AssetCategory_Create
    @TenantID BIGINT,
    @CategoryCode NVARCHAR(50),
    @CategoryName NVARCHAR(100),
    @ParentCategoryID BIGINT = NULL,
    @Description NVARCHAR(500) = NULL,
    @IsDepreciable BIT = 1,
    @CreatedBy BIGINT,
    @AssetCategoryID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO asset.AssetCategory (TenantID, CategoryCode, CategoryName, ParentCategoryID, [Description], IsDepreciable, CreatedBy)
        VALUES (@TenantID, @CategoryCode, @CategoryName, @ParentCategoryID, @Description, @IsDepreciable, @CreatedBy);
        SET @AssetCategoryID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;