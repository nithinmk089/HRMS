
-- asset.usp_AssetWarranty_Create.sql
CREATE PROCEDURE asset.usp_AssetWarranty_Create
    @TenantID BIGINT,
    @AssetID BIGINT,
    @WarrantyStartDate DATE,
    @WarrantyEndDate DATE,
    @WarrantyProvider NVARCHAR(150),
    @CreatedBy BIGINT,
    @AssetWarrantyID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO asset.AssetWarranty (TenantID, AssetID, WarrantyStartDate, WarrantyEndDate, WarrantyProvider, CreatedBy)
        VALUES (@TenantID, @AssetID, @WarrantyStartDate, @WarrantyEndDate, @WarrantyProvider, @CreatedBy);
        SET @AssetWarrantyID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;