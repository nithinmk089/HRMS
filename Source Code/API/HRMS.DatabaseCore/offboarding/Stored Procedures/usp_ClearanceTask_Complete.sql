
CREATE PROCEDURE offboarding.usp_ClearanceTask_Complete
    @ClearanceTaskID BIGINT,
    @TenantID        BIGINT,
    @ModifiedBy      BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE offboarding.ClearanceTask
        SET Status = 'Completed',
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE ClearanceTaskID = @ClearanceTaskID AND TenantID = @TenantID AND IsDeleted = 0;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END