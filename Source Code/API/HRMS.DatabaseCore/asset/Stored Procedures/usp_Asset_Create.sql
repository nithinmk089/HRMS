
-- asset.usp_Asset_Create.sql
CREATE PROCEDURE asset.usp_Asset_Create
    @TenantID BIGINT,
    @AssetCode NVARCHAR(50),
    @AssetTag NVARCHAR(50),
    @AssetName NVARCHAR(150),
    @AssetCategoryID BIGINT,
    @Manufacturer NVARCHAR(100) = NULL,
    @Model NVARCHAR(100) = NULL,
    @SerialNumber NVARCHAR(100),
    @PurchaseDate DATE,
    @PurchaseCost DECIMAL(18,2),
    @CurrentBookValue DECIMAL(18,2),
    @Status NVARCHAR(50) = 'Available',
    @CreatedBy BIGINT,
    @AssetID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO asset.AssetMaster (TenantID, AssetCode, AssetTag, AssetName, AssetCategoryID, Manufacturer, Model, SerialNumber, PurchaseDate, PurchaseCost, CurrentBookValue, [Status], CreatedBy)
        VALUES (@TenantID, @AssetCode, @AssetTag, @AssetName, @AssetCategoryID, @Manufacturer, @Model, @SerialNumber, @PurchaseDate, @PurchaseCost, @CurrentBookValue, @Status, @CreatedBy);
        SET @AssetID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;