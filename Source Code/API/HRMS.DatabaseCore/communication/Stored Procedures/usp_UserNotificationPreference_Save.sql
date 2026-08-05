
CREATE   PROCEDURE communication.usp_UserNotificationPreference_Save
    @TenantID BIGINT,
    @UserID BIGINT,
    @BusinessEvent VARCHAR(100),
    @Channel VARCHAR(50),
    @Frequency VARCHAR(50),
    @IsEnabled BIT,
    @CreatedBy BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        IF EXISTS (SELECT 1 FROM communication.UserNotificationPreference WHERE UserID = @UserID AND BusinessEvent = @BusinessEvent AND Channel = @Channel)
        BEGIN
            UPDATE communication.UserNotificationPreference
            SET Frequency = @Frequency,
                IsEnabled = @IsEnabled,
                ModifiedBy = @CreatedBy,
                ModifiedDate = GETUTCDATE()
            WHERE UserID = @UserID AND BusinessEvent = @BusinessEvent AND Channel = @Channel;
        END
        ELSE
        BEGIN
            INSERT INTO communication.UserNotificationPreference (TenantID, UserID, BusinessEvent, Channel, Frequency, IsEnabled, CreatedBy)
            VALUES (@TenantID, @UserID, @BusinessEvent, @Channel, @Frequency, @IsEnabled, @CreatedBy);
        END
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        INSERT INTO system.ErrorLog (ModuleName, ErrorMessage, StackTrace)
        VALUES ('communication.usp_UserNotificationPreference_Save', ERROR_MESSAGE(), CONVERT(VARCHAR, ERROR_STATE()));
        THROW;
    END CATCH
END;