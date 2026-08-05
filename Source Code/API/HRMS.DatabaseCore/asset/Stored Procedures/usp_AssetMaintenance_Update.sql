
-- asset.usp_AssetMaintenance_Update.sql
CREATE PROCEDURE asset.usp_AssetMaintenance_Update
    @AssetMaintenanceID BIGINT,
    @TenantID BIGINT,
    @MaintenanceDate DATE,
    @MaintenanceType NVARCHAR(100),
    @VendorName NVARCHAR(150) = NULL,
    @Cost DECIMAL(18,2) = 0,
    @MaintenanceStatus NVARCHAR(50),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE asset.AssetMaintenance
        SET MaintenanceDate = @MaintenanceDate,
            MaintenanceType = @MaintenanceType,
            VendorName = @VendorName,
            Cost = @Cost,
            MaintenanceStatus = @MaintenanceStatus,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE AssetMaintenanceID = @AssetMaintenanceID AND TenantID = @TenantID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;