
-- asset.usp_AssetTransfer_Complete.sql
CREATE PROCEDURE asset.usp_AssetTransfer_Complete
    @AssetTransferID BIGINT,
    @TenantID BIGINT,
    @CompletedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE asset.AssetTransfer
        SET TransferStatus = 'Completed', ModifiedBy = @CompletedBy, ModifiedDate = GETUTCDATE()
        WHERE AssetTransferID = @AssetTransferID AND TenantID = @TenantID;

        DECLARE @AssetID BIGINT, @FromEmployeeID BIGINT, @ToEmployeeID BIGINT, @TransferDate DATE;
        SELECT @AssetID = AssetID, @FromEmployeeID = FromEmployeeID, @ToEmployeeID = ToEmployeeID, @TransferDate = TransferDate
        FROM asset.AssetTransfer WHERE AssetTransferID = @AssetTransferID;

        UPDATE asset.AssetAssignment
        SET ReturnedDate = @TransferDate, AssignmentStatus = 'Transferred', ModifiedBy = @CompletedBy, ModifiedDate = GETUTCDATE()
        WHERE AssetID = @AssetID AND EmployeeID = @FromEmployeeID AND AssignmentStatus = 'Active';

        INSERT INTO asset.AssetAssignment (TenantID, AssetID, EmployeeID, AssignedDate, AssignmentStatus, CreatedBy)
        VALUES (@TenantID, @AssetID, @ToEmployeeID, @TransferDate, 'Active', @CompletedBy);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;