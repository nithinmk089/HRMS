
CREATE   PROCEDURE communication.usp_NotificationQueue_Escalate
    @NotificationQueueID BIGINT,
    @TenantID BIGINT,
    @EscalationRule VARCHAR(50),
    @ModifiedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE communication.NotificationQueue
        SET EscalationStatus = @EscalationRule,
            ModifiedBy = @ModifiedBy,
            ModifiedDate = GETUTCDATE()
        WHERE NotificationQueueID = @NotificationQueueID AND TenantID = @TenantID;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace)
        VALUES ('communication.usp_NotificationQueue_Escalate', ERROR_MESSAGE(), CONVERT(VARCHAR, ERROR_STATE()));
        THROW;
    END CATCH
END;