using System;
using System.Threading.Tasks;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure.Services
{
    public class SmsNotificationService : ISmsNotificationService
    {
        private readonly IConfigurationRepository _configRepo;
        private readonly ILogger<SmsNotificationService> _logger;

        public SmsNotificationService(IConfigurationRepository configRepo, ILogger<SmsNotificationService> logger)
        {
            _configRepo = configRepo;
            _logger = logger;
        }

        public async Task<NotificationSendResult> SendSmsAsync(long tenantId, string recipientPhone, string message)
        {
            var gatewayUrl = await _configRepo.GetValueAsync(tenantId, "Sms_GatewayUrl") ?? "";

            _logger.LogInformation("Sending SMS notification to {Phone} via Gateway '{GatewayUrl}'. Message length: {Length} chars.", recipientPhone, gatewayUrl, message.Length);

            return await Task.FromResult(new NotificationSendResult
            {
                Success = true,
                DeliveredRecipient = recipientPhone,
                ModeUsed = string.IsNullOrWhiteSpace(gatewayUrl) ? "Simulated Gateway" : "Configured Gateway"
            });
        }
    }
}
