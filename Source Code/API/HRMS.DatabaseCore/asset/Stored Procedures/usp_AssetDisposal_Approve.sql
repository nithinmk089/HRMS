
-- asset.usp_AssetDisposal_Approve.sql
CREATE PROCEDURE asset.usp_AssetDisposal_Approve
    @AssetDisposalID BIGINT,
    @TenantID BIGINT,
    @ApprovedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE asset.AssetDisposal
        SET DisposalStatus = 'Approved', ModifiedBy = @ApprovedBy, ModifiedDate = GETUTCDATE()
        WHERE AssetDisposalID = @AssetDisposalID AND TenantID = @TenantID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;