
-- asset.usp_AssetDisposal_Close.sql
CREATE PROCEDURE asset.usp_AssetDisposal_Close
    @AssetDisposalID BIGINT,
    @TenantID BIGINT,
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE asset.AssetDisposal
        SET DisposalStatus = 'Closed', ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE()
        WHERE AssetDisposalID = @AssetDisposalID AND TenantID = @TenantID;

        DECLARE @AssetID BIGINT;
        SELECT @AssetID = AssetID FROM asset.AssetDisposal WHERE AssetDisposalID = @AssetDisposalID;
        UPDATE asset.AssetMaster SET [Status] = 'Disposed' WHERE AssetID = @AssetID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;