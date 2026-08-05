
-- asset.usp_Asset_Update.sql
CREATE PROCEDURE asset.usp_Asset_Update
    @AssetID BIGINT,
    @TenantID BIGINT,
    @AssetName NVARCHAR(150),
    @AssetCategoryID BIGINT,
    @Manufacturer NVARCHAR(100) = NULL,
    @Model NVARCHAR(100) = NULL,
    @SerialNumber NVARCHAR(100),
    @PurchaseDate DATE,
    @PurchaseCost DECIMAL(18,2),
    @CurrentBookValue DECIMAL(18,2),
    @Status NVARCHAR(50),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE asset.AssetMaster
        SET AssetName = @AssetName,
            AssetCategoryID = @AssetCategoryID,
            Manufacturer = @Manufacturer,
            Model = @Model,
            SerialNumber = @SerialNumber,
            PurchaseDate = @PurchaseDate,
            PurchaseCost = @PurchaseCost,
            CurrentBookValue = @CurrentBookValue,
            [Status] = @Status,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE AssetID = @AssetID AND TenantID = @TenantID AND IsDeleted = 0;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;