
-- asset.usp_AssetDepreciation_Recalculate.sql
CREATE PROCEDURE asset.usp_AssetDepreciation_Recalculate
    @TenantID BIGINT,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE ad
        SET ad.BookValue = am.PurchaseCost - asset.fn_CalculateDepreciation(am.AssetID, GETUTCDATE()),
            ad.ModifiedBy = @ModifiedBy,
            ad.ModifiedDate = GETUTCDATE()
        FROM asset.AssetDepreciation ad
        JOIN asset.AssetMaster am ON ad.AssetID = am.AssetID
        WHERE ad.TenantID = @TenantID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;