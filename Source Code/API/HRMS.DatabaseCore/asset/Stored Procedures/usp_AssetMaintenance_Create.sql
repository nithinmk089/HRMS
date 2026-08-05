
-- asset.usp_AssetMaintenance_Create.sql
CREATE PROCEDURE asset.usp_AssetMaintenance_Create
    @TenantID BIGINT,
    @AssetID BIGINT,
    @MaintenanceDate DATE,
    @MaintenanceType NVARCHAR(100),
    @VendorName NVARCHAR(150) = NULL,
    @Cost DECIMAL(18,2) = 0,
    @CreatedBy BIGINT,
    @AssetMaintenanceID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO asset.AssetMaintenance (TenantID, AssetID, MaintenanceDate, MaintenanceType, VendorName, Cost, MaintenanceStatus, CreatedBy)
        VALUES (@TenantID, @AssetID, @MaintenanceDate, @MaintenanceType, @VendorName, @Cost, 'Scheduled', @CreatedBy);
        SET @AssetMaintenanceID = SCOPE_IDENTITY();
        
        UPDATE asset.AssetMaster SET [Status] = 'Maintenance' WHERE AssetID = @AssetID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;