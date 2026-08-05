
-- asset.usp_AssetTransfer_Create.sql
CREATE PROCEDURE asset.usp_AssetTransfer_Create
    @TenantID BIGINT,
    @AssetID BIGINT,
    @FromEmployeeID BIGINT = NULL,
    @ToEmployeeID BIGINT,
    @TransferDate DATE,
    @TransferReason NVARCHAR(500) = NULL,
    @CreatedBy BIGINT,
    @AssetTransferID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO asset.AssetTransfer (TenantID, AssetID, FromEmployeeID, ToEmployeeID, TransferDate, TransferReason, TransferStatus, CreatedBy)
        VALUES (@TenantID, @AssetID, @FromEmployeeID, @ToEmployeeID, @TransferDate, @TransferReason, 'Pending', @CreatedBy);
        SET @AssetTransferID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;