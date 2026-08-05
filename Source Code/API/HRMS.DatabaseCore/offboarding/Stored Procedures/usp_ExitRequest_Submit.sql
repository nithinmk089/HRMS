
CREATE PROCEDURE offboarding.usp_ExitRequest_Submit
    @ExitRequestID BIGINT,
    @TenantID      BIGINT,
    @ModifiedBy    BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE offboarding.ExitRequest
        SET Status = 'Submitted',
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE ExitRequestID = @ExitRequestID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END