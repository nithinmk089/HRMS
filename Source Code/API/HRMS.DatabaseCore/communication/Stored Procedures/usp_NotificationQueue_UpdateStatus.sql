
CREATE   PROCEDURE communication.usp_NotificationQueue_UpdateStatus
    @NotificationQueueID BIGINT,
    @TenantID BIGINT,
    @Status VARCHAR(50),
    @ErrorMessage VARCHAR(MAX) = NULL,
    @RetryCount INT = NULL,
    @NextRunDate DATETIME = NULL,
    @SentDate DATETIME = NULL,
    @DeliveredDate DATETIME = NULL,
    @ReadDate DATETIME = NULL,
    @ModifiedBy BIGINT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE communication.NotificationQueue
        SET [Status] = @Status,
            ErrorMessage = COALESCE(@ErrorMessage, ErrorMessage),
            RetryCount = COALESCE(@RetryCount, RetryCount),
            NextRunDate = COALESCE(@NextRunDate, NextRunDate),
            SentDate = COALESCE(@SentDate, SentDate),
            DeliveredDate = COALESCE(@DeliveredDate, DeliveredDate),
            ReadDate = COALESCE(@ReadDate, ReadDate),
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE NotificationQueueID = @NotificationQueueID AND TenantID = @TenantID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace)
        VALUES ('communication.usp_NotificationQueue_UpdateStatus', ERROR_MESSAGE(), CONVERT(VARCHAR, ERROR_STATE()));
        THROW;
    END CATCH
END;