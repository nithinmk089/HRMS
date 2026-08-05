
-- asset.usp_AssetRepair_Update.sql
CREATE PROCEDURE asset.usp_AssetRepair_Update
    @AssetRepairID BIGINT,
    @TenantID BIGINT,
    @RepairDate DATE,
    @RepairReason NVARCHAR(500) = NULL,
    @RepairCost DECIMAL(18,2) = 0,
    @RepairStatus NVARCHAR(50),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE asset.AssetRepair
        SET RepairDate = @RepairDate,
            RepairReason = @RepairReason,
            RepairCost = @RepairCost,
            RepairStatus = @RepairStatus,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE AssetRepairID = @AssetRepairID AND TenantID = @TenantID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;