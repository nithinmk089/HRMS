using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Services
{
    public class EmailSettingsDto
    {
        public string Mode { get; set; } = "Development"; // "Development" or "Production"
        public string DevRecipient { get; set; } = "admin@hrms.com";
        public string SmtpHost { get; set; } = "smtp.mailtrap.io";
        public int SmtpPort { get; set; } = 587;
        public string SmtpUsername { get; set; } = "";
        public string SmtpPassword { get; set; } = "";
        public bool EnableSsl { get; set; } = true;
        public string FromAddress { get; set; } = "noreply@hrms.com";
        public string FromName { get; set; } = "Enterprise HRMS";
    }

    public class NotificationSendResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? DeliveredRecipient { get; set; }
        public string ModeUsed { get; set; } = "Production";
    }

    public interface IEmailNotificationService
    {
        Task<NotificationSendResult> SendEmailAsync(long tenantId, string recipient, string subject, string body);
        Task<EmailSettingsDto> GetEmailSettingsAsync(long tenantId);
        Task<bool> SaveEmailSettingsAsync(long tenantId, EmailSettingsDto settings, long modifiedBy);
        Task<NotificationSendResult> SendTestEmailAsync(long tenantId, string testRecipient);
    }
}
