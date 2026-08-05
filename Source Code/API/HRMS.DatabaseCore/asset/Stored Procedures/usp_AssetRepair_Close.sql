
-- asset.usp_AssetRepair_Close.sql
CREATE PROCEDURE asset.usp_AssetRepair_Close
    @AssetRepairID BIGINT,
    @TenantID BIGINT,
    @ModifiedBy BIGINT,
    @Remarks NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE asset.AssetRepair
        SET RepairStatus = 'Closed', ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE()
        WHERE AssetRepairID = @AssetRepairID AND TenantID = @TenantID;

        DECLARE @AssetID BIGINT;
        SELECT @AssetID = AssetID FROM asset.AssetRepair WHERE AssetRepairID = @AssetRepairID;
        UPDATE asset.AssetMaster SET [Status] = 'Available' WHERE AssetID = @AssetID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;