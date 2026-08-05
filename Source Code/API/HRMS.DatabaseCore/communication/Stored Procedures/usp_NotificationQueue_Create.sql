
CREATE   PROCEDURE communication.usp_NotificationQueue_Create
    @TenantID BIGINT,
    @Sender VARCHAR(200),
    @Recipient VARCHAR(500),
    @Channel VARCHAR(50),
    @BusinessEvent VARCHAR(100),
    @Subject VARCHAR(200),
    @Body VARCHAR(MAX),
    @Status VARCHAR(50),
    @CreatedBy BIGINT,
    @NotificationQueueID BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO communication.NotificationQueue (TenantID, Sender, Recipient, Channel, BusinessEvent, Subject, Body, [Status], CreatedBy)
        VALUES (@TenantID, @Sender, @Recipient, @Channel, @BusinessEvent, @Subject, @Body, @Status, @CreatedBy);
        SET @NotificationQueueID = SCOPE_IDENTITY();
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace)
        VALUES ('communication.usp_NotificationQueue_Create', ERROR_MESSAGE(), CONVERT(VARCHAR, ERROR_STATE()));
        THROW;
    END CATCH
END;