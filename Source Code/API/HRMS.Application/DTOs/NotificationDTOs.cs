using System;

namespace HRMS.Application.DTOs
{
    // --- Notification Template DTOs ---
    public class NotificationTemplateDto
    {
        public long TemplateId { get; set; }
        public long TenantId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string BusinessEvent { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string? SubjectTemplate { get; set; }
        public string BodyTemplate { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public long VersionNo { get; set; }
    }

    public class CreateNotificationTemplateRequest
    {
        public long TenantId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string BusinessEvent { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string? SubjectTemplate { get; set; }
        public string BodyTemplate { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    public class UpdateNotificationTemplateRequest
    {
        public long TemplateId { get; set; }
        public long TenantId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string BusinessEvent { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string? SubjectTemplate { get; set; }
        public string BodyTemplate { get; set; } = string.Empty;
        public long ModifiedBy { get; set; }
    }

    // --- Notification Queue DTOs ---
    public class NotificationQueueDto
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
        public DateTime CreatedDate { get; set; }
    }

    public class SendNotificationRequest
    {
        public long TenantId { get; set; }
        public string? Sender { get; set; }
        public string Recipient { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string BusinessEvent { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public string Body { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    // --- User Preference DTOs ---
    public class UserNotificationPreferenceDto
    {
        public long PreferenceId { get; set; }
        public long TenantId { get; set; }
        public long UserID { get; set; }
        public string BusinessEvent { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
    }

    public class SaveUserNotificationPreferenceRequest
    {
        public long TenantId { get; set; }
        public long UserID { get; set; }
        public string BusinessEvent { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
        public long CreatedBy { get; set; }
    }

    // --- Notification Metrics DTO ---
    public class NotificationMetricsDto
    {
        public int TotalProcessed { get; set; }
        public int SentCount { get; set; }
        public int DeliveredCount { get; set; }
        public int ReadCount { get; set; }
        public int FailedCount { get; set; }
        public decimal DeliveryRate { get; set; }
        public decimal FailureRate { get; set; }
        public decimal ReadRate { get; set; }
    }
}
