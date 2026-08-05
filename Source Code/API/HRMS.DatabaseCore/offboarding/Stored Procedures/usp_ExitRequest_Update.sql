
CREATE PROCEDURE offboarding.usp_ExitRequest_Update
    @ExitRequestID   BIGINT,
    @TenantID        BIGINT,
    @LastWorkingDate DATE,
    @ExitReason      NVARCHAR(500),
    @ModifiedBy      BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE offboarding.ExitRequest
        SET LastWorkingDate = @LastWorkingDate,
            ExitReason = @ExitReason,
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