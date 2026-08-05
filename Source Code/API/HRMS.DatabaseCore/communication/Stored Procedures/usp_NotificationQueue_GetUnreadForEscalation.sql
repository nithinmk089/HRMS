
CREATE   PROCEDURE communication.usp_NotificationQueue_GetUnreadForEscalation
    @Hours INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ThresholdDate DATETIME = DATEADD(HOUR, -@Hours, GETUTCDATE());
    SELECT NotificationQueueID, TenantID, Sender, Recipient, Channel, BusinessEvent, Subject, Body, [Status], ErrorMessage, RetryCount, MaxRetries, NextRunDate, SentDate, DeliveredDate, ReadDate, EscalationStatus, CreatedDate
    FROM communication.NotificationQueue
    WHERE [Status] IN ('Sent', 'Delivered')
      AND ReadDate IS NULL
      AND EscalationStatus = 'None'
      AND CreatedDate <= @ThresholdDate;
END;