
-- asset.usp_AssetRepair_Create.sql
CREATE PROCEDURE asset.usp_AssetRepair_Create
    @TenantID BIGINT,
    @AssetID BIGINT,
    @RepairDate DATE,
    @RepairReason NVARCHAR(500) = NULL,
    @RepairCost DECIMAL(18,2) = 0,
    @CreatedBy BIGINT,
    @AssetRepairID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO asset.AssetRepair (TenantID, AssetID, RepairDate, RepairReason, RepairCost, RepairStatus, CreatedBy)
        VALUES (@TenantID, @AssetID, @RepairDate, @RepairReason, @RepairCost, 'Pending', @CreatedBy);
        SET @AssetRepairID = SCOPE_IDENTITY();

        UPDATE asset.AssetMaster SET [Status] = 'Repair' WHERE AssetID = @AssetID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;