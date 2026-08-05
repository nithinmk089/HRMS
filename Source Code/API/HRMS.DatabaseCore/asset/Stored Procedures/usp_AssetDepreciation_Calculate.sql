
-- asset.usp_AssetDepreciation_Calculate.sql
CREATE PROCEDURE asset.usp_AssetDepreciation_Calculate
    @TenantID BIGINT,
    @CreatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO asset.AssetDepreciation (TenantID, AssetID, DepreciationMethod, DepreciationRate, BookValue, CreatedBy)
        SELECT 
            am.TenantID,
            am.AssetID,
            'Straight-Line',
            10.00,
            am.PurchaseCost - asset.fn_CalculateDepreciation(am.AssetID, GETUTCDATE()),
            @CreatedBy
        FROM asset.AssetMaster am
        LEFT JOIN asset.AssetDepreciation ad ON am.AssetID = ad.AssetID
        WHERE am.TenantID = @TenantID AND am.IsDeleted = 0 AND ad.AssetDepreciationID IS NULL;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;