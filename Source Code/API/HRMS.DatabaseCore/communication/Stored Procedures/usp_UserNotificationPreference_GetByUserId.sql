
CREATE   PROCEDURE communication.usp_UserNotificationPreference_GetByUserId
    @UserID BIGINT,
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PreferenceID, TenantID, UserID, BusinessEvent, Channel, Frequency, IsEnabled
    FROM communication.UserNotificationPreference
    WHERE UserID = @UserID AND TenantID = @TenantID;
END;