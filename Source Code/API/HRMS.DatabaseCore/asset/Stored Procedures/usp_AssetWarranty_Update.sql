
-- asset.usp_AssetWarranty_Update.sql
CREATE PROCEDURE asset.usp_AssetWarranty_Update
    @AssetWarrantyID BIGINT,
    @TenantID BIGINT,
    @WarrantyStartDate DATE,
    @WarrantyEndDate DATE,
    @WarrantyProvider NVARCHAR(150),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE asset.AssetWarranty
        SET WarrantyStartDate = @WarrantyStartDate,
            WarrantyEndDate = @WarrantyEndDate,
            WarrantyProvider = @WarrantyProvider,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE AssetWarrantyID = @AssetWarrantyID AND TenantID = @TenantID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;