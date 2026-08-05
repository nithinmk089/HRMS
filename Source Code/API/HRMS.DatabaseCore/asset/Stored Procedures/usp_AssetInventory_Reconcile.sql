
-- asset.usp_AssetInventory_Reconcile.sql
CREATE PROCEDURE asset.usp_AssetInventory_Reconcile
    @TenantID BIGINT,
    @AssetID BIGINT,
    @LocationID BIGINT,
    @Quantity INT,
    @InventoryStatus NVARCHAR(50),
    @CreatedBy BIGINT,
    @AssetInventoryID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @ExistingID BIGINT = NULL;
        SELECT TOP 1 @ExistingID = AssetInventoryID 
        FROM asset.AssetInventory 
        WHERE TenantID = @TenantID AND AssetID = @AssetID AND LocationID = @LocationID AND IsDeleted = 0;

        IF @ExistingID IS NOT NULL
        BEGIN
            UPDATE asset.AssetInventory
            SET Quantity = @Quantity,
                InventoryStatus = @InventoryStatus,
                ModifiedBy = @CreatedBy,
                ModifiedDate = GETUTCDATE()
            WHERE AssetInventoryID = @ExistingID;
            SET @AssetInventoryID = @ExistingID;
        END
        ELSE
        BEGIN
            INSERT INTO asset.AssetInventory (TenantID, AssetID, LocationID, Quantity, InventoryStatus, CreatedBy)
            VALUES (@TenantID, @AssetID, @LocationID, @Quantity, @InventoryStatus, @CreatedBy);
            SET @AssetInventoryID = SCOPE_IDENTITY();
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;