
CREATE PROCEDURE offboarding.usp_ClearanceRequest_Approve
    @ClearanceRequestID BIGINT,
    @TenantID           BIGINT,
    @ModifiedBy         BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE offboarding.ClearanceRequest
        SET ClearanceStatus = 'Approved',
            CompletedDate = CAST(GETUTCDATE() AS DATE),
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE ClearanceRequestID = @ClearanceRequestID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END