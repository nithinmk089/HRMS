
-- asset.usp_AssetMaintenance_Close.sql
CREATE PROCEDURE asset.usp_AssetMaintenance_Close
    @AssetMaintenanceID BIGINT,
    @TenantID BIGINT,
    @ModifiedBy BIGINT,
    @Remarks NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE asset.AssetMaintenance
        SET MaintenanceStatus = 'Closed', ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE()
        WHERE AssetMaintenanceID = @AssetMaintenanceID AND TenantID = @TenantID;

        DECLARE @AssetID BIGINT;
        SELECT @AssetID = AssetID FROM asset.AssetMaintenance WHERE AssetMaintenanceID = @AssetMaintenanceID;
        UPDATE asset.AssetMaster SET [Status] = 'Available' WHERE AssetID = @AssetID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;