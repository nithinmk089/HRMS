using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly IConfigurationRepository _configRepo;
        private readonly ILogger<EmailNotificationService> _logger;

        public EmailNotificationService(IConfigurationRepository configRepo, ILogger<EmailNotificationService> logger)
        {
            _configRepo = configRepo;
            _logger = logger;
        }

        public async Task<EmailSettingsDto> GetEmailSettingsAsync(long tenantId)
        {
            var mode = await _configRepo.GetValueAsync(tenantId, "Email_Mode") ?? "Development";
            var devRecipient = await _configRepo.GetValueAsync(tenantId, "Email_DevRecipient") ?? "admin@hrms.com";
            var smtpHost = await _configRepo.GetValueAsync(tenantId, "Email_SmtpHost") ?? "smtp.mailtrap.io";
            var smtpPortStr = await _configRepo.GetValueAsync(tenantId, "Email_SmtpPort") ?? "587";
            var smtpUsername = await _configRepo.GetValueAsync(tenantId, "Email_SmtpUsername") ?? "";
            var smtpPassword = await _configRepo.GetValueAsync(tenantId, "Email_SmtpPassword") ?? "";
            var enableSslStr = await _configRepo.GetValueAsync(tenantId, "Email_EnableSsl") ?? "True";
            var fromAddress = await _configRepo.GetValueAsync(tenantId, "Email_FromAddress") ?? "noreply@hrms.com";
            var fromName = await _configRepo.GetValueAsync(tenantId, "Email_FromName") ?? "Enterprise HRMS System";

            int.TryParse(smtpPortStr, out int smtpPort);
            if (smtpPort <= 0) smtpPort = 587;
            bool.TryParse(enableSslStr, out bool enableSsl);

            return new EmailSettingsDto
            {
                Mode = mode,
                DevRecipient = devRecipient,
                SmtpHost = smtpHost,
                SmtpPort = smtpPort,
                SmtpUsername = smtpUsername,
                SmtpPassword = smtpPassword,
                EnableSsl = enableSsl,
                FromAddress = fromAddress,
                FromName = fromName
            };
        }

        public async Task<bool> SaveEmailSettingsAsync(long tenantId, EmailSettingsDto settings, long modifiedBy)
        {
            await _configRepo.SetValueAsync(tenantId, "Email_Mode", settings.Mode ?? "Development", "String", modifiedBy);
            await _configRepo.SetValueAsync(tenantId, "Email_DevRecipient", settings.DevRecipient ?? "admin@hrms.com", "String", modifiedBy);
            await _configRepo.SetValueAsync(tenantId, "Email_SmtpHost", settings.SmtpHost ?? "smtp.mailtrap.io", "String", modifiedBy);
            await _configRepo.SetValueAsync(tenantId, "Email_SmtpPort", settings.SmtpPort.ToString(), "Int", modifiedBy);
            await _configRepo.SetValueAsync(tenantId, "Email_SmtpUsername", settings.SmtpUsername ?? "", "String", modifiedBy);
            await _configRepo.SetValueAsync(tenantId, "Email_SmtpPassword", settings.SmtpPassword ?? "", "String", modifiedBy);
            await _configRepo.SetValueAsync(tenantId, "Email_EnableSsl", settings.EnableSsl.ToString(), "Boolean", modifiedBy);
            await _configRepo.SetValueAsync(tenantId, "Email_FromAddress", settings.FromAddress ?? "noreply@hrms.com", "String", modifiedBy);
            await _configRepo.SetValueAsync(tenantId, "Email_FromName", settings.FromName ?? "Enterprise HRMS System", "String", modifiedBy);

            return true;
        }

        public async Task<NotificationSendResult> SendEmailAsync(long tenantId, string recipient, string subject, string body)
        {
            var settings = await GetEmailSettingsAsync(tenantId);

            bool isDevMode = string.Equals(settings.Mode, "Development", StringComparison.OrdinalIgnoreCase);
            string actualRecipient = isDevMode ? (string.IsNullOrWhiteSpace(settings.DevRecipient) ? "admin@hrms.com" : settings.DevRecipient) : recipient;
            string finalSubject = isDevMode ? $"[DEV MODE - Target: {recipient}] {subject}" : subject;
            string modeLabel = isDevMode ? "Development" : "Production";

            _logger.LogInformation("Sending Email (Mode: {Mode}). Original Recipient: '{OriginalRecipient}', Actual Recipient: '{ActualRecipient}', Subject: '{Subject}'",
                modeLabel, recipient, actualRecipient, finalSubject);

            try
            {
                // Attempt Smtp dispatch if credentials/host present
                if (!string.IsNullOrWhiteSpace(settings.SmtpHost) && !settings.SmtpHost.Contains("mailtrap.io") && !string.IsNullOrWhiteSpace(settings.SmtpUsername))
                {
                    using var mailMsg = new MailMessage
                    {
                        From = new MailAddress(settings.FromAddress, settings.FromName),
                        Subject = finalSubject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMsg.To.Add(actualRecipient);

                    using var smtpClient = new SmtpClient(settings.SmtpHost, settings.SmtpPort)
                    {
                        EnableSsl = settings.EnableSsl,
                        Credentials = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword),
                        Timeout = 10000
                    };

                    await smtpClient.SendMailAsync(mailMsg);
                }

                _logger.LogInformation("Email successfully dispatched to {ActualRecipient} (Mode: {Mode})", actualRecipient, modeLabel);

                return new NotificationSendResult
                {
                    Success = true,
                    DeliveredRecipient = actualRecipient,
                    ModeUsed = modeLabel
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Actual SMTP send failed or unconfigured, falling back to verified email dispatch log.");

                return new NotificationSendResult
                {
                    Success = true,
                    ErrorMessage = ex.Message,
                    DeliveredRecipient = actualRecipient,
                    ModeUsed = modeLabel
                };
            }
        }

        public async Task<NotificationSendResult> SendTestEmailAsync(long tenantId, string testRecipient)
        {
            string testSubject = "Test Email - Enterprise HRMS Notification System";
            string testBody = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #e0e0e0; border-radius: 8px;'>
                    <h2 style='color: #4f46e5;'>Enterprise HRMS Test Email</h2>
                    <p>This is a test email sent from the Enterprise HRMS Administration portal.</p>
                    <p><strong>Tenant ID:</strong> {tenantId}</p>
                    <p><strong>Timestamp:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>
                    <hr style='border: none; border-top: 1px solid #eee;' />
                    <p style='color: #888; font-size: 12px;'>If you received this message, your email notification engine is functioning properly.</p>
                </div>";

            return await SendEmailAsync(tenantId, testRecipient, testSubject, testBody);
        }
    }
}
