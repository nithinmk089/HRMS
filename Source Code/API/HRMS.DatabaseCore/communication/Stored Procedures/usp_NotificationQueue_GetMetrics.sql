
CREATE   PROCEDURE communication.usp_NotificationQueue_GetMetrics
    @TenantID BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @TotalProcessed INT;
    DECLARE @Sent INT;
    DECLARE @Delivered INT;
    DECLARE @Read INT;
    DECLARE @Failed INT;
    SELECT 
        @TotalProcessed = COUNT(*),
        @Sent = SUM(CASE WHEN [Status] IN ('Sent', 'Delivered', 'Read') THEN 1 ELSE 0 END),
        @Delivered = SUM(CASE WHEN [Status] IN ('Delivered', 'Read') THEN 1 ELSE 0 END),
        @Read = SUM(CASE WHEN [Status] = 'Read' THEN 1 ELSE 0 END),
        @Failed = SUM(CASE WHEN [Status] = 'Failed' THEN 1 ELSE 0 END)
    FROM communication.NotificationQueue
    WHERE TenantID = @TenantID AND [Status] != 'Cancelled';
    SELECT 
        ISNULL(@TotalProcessed, 0) AS TotalProcessed,
        ISNULL(@Sent, 0) AS SentCount,
        ISNULL(@Delivered, 0) AS DeliveredCount,
        ISNULL(@Read, 0) AS ReadCount,
        ISNULL(@Failed, 0) AS FailedCount,
        CASE WHEN ISNULL(@TotalProcessed, 0) > 0 THEN (ISNULL(@Delivered, 0) * 100.0) / @TotalProcessed ELSE 0.0 END AS DeliveryRate,
        CASE WHEN ISNULL(@TotalProcessed, 0) > 0 THEN (ISNULL(@Failed, 0) * 100.0) / @TotalProcessed ELSE 0.0 END AS FailureRate,
        CASE WHEN ISNULL(@Delivered, 0) > 0 THEN (ISNULL(@Read, 0) * 100.0) / @Delivered ELSE 0.0 END AS ReadRate;
END;