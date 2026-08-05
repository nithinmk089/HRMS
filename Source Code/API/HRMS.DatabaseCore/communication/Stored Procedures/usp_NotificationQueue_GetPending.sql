
CREATE   PROCEDURE communication.usp_NotificationQueue_GetPending
    @MaxBatchSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@MaxBatchSize) NotificationQueueID, TenantID, Sender, Recipient, Channel, BusinessEvent, Subject, Body, [Status], ErrorMessage, RetryCount, MaxRetries, NextRunDate, SentDate, DeliveredDate, ReadDate, EscalationStatus
    FROM communication.NotificationQueue
    WHERE [Status] = 'Queued'
       OR ([Status] = 'Failed' AND RetryCount < MaxRetries AND NextRunDate <= GETUTCDATE())
    ORDER BY NextRunDate ASC;
END;