
CREATE PROCEDURE offboarding.usp_KnowledgeTransfer_Complete
    @KnowledgeTransferID BIGINT,
    @TenantID            BIGINT,
    @ModifiedBy          BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE offboarding.KnowledgeTransfer
        SET KTStatus = 'Completed',
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE KnowledgeTransferID = @KnowledgeTransferID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END