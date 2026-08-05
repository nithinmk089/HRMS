
-- asset.usp_AssetTransfer_Approve.sql
CREATE PROCEDURE asset.usp_AssetTransfer_Approve
    @AssetTransferID BIGINT,
    @TenantID BIGINT,
    @ApprovedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE asset.AssetTransfer
        SET TransferStatus = 'Approved', ModifiedBy = @ApprovedBy, ModifiedDate = GETUTCDATE()
        WHERE AssetTransferID = @AssetTransferID AND TenantID = @TenantID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;