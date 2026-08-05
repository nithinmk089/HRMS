
CREATE   PROCEDURE communication.usp_NotificationQueue_Search
    @TenantID BIGINT,
    @Channel VARCHAR(50) = NULL,
    @Status VARCHAR(50) = NULL,
    @BusinessEvent VARCHAR(100) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    SELECT COUNT(*) AS TotalCount
    FROM communication.NotificationQueue
    WHERE TenantID = @TenantID
      AND (@Channel IS NULL OR Channel = @Channel)
      AND (@Status IS NULL OR [Status] = @Status)
      AND (@BusinessEvent IS NULL OR BusinessEvent = @BusinessEvent);
    SELECT NotificationQueueID, TenantID, Sender, Recipient, Channel, BusinessEvent, Subject, Body, [Status], ErrorMessage, RetryCount, MaxRetries, NextRunDate, SentDate, DeliveredDate, ReadDate, EscalationStatus, CreatedDate
    FROM communication.NotificationQueue
    WHERE TenantID = @TenantID
      AND (@Channel IS NULL OR Channel = @Channel)
      AND (@Status IS NULL OR [Status] = @Status)
      AND (@BusinessEvent IS NULL OR BusinessEvent = @BusinessEvent)
    ORDER BY NotificationQueueID DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;