using System;

namespace HRMS.Domain.Entities
{
    public class NotificationQueue
    {
        public long NotificationQueueId { get; set; }
        public long TenantId { get; set; }
        public string? Sender { get; set; }
        public string Recipient { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string BusinessEvent { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public string Body { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
        public int RetryCount { get; set; }
        public int MaxRetries { get; set; }
        public DateTime NextRunDate { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public string EscalationStatus { get; set; } = "None";

        // Audit Columns
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
