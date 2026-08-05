using System;
using System.Threading.Tasks;
using HRMS.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure.Services
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly ISignalRNotificationService _signalRService;
        private readonly ILogger<PushNotificationService> _logger;

        public PushNotificationService(ISignalRNotificationService signalRService, ILogger<PushNotificationService> logger)
        {
            _signalRService = signalRService;
            _logger = logger;
        }

        public async Task<NotificationSendResult> SendPushNotificationAsync(long tenantId, string recipient, string title, string message)
        {
            _logger.LogInformation("Sending Web/Mobile Push notification to {Recipient}. Title: '{Title}'", recipient, title);

            // Real-time push via SignalR websocket channel
            await _signalRService.SendNotificationToUserAsync(recipient, new
            {
                Type = "PushAlert",
                Title = title,
                Message = message,
                Recipient = recipient,
                Timestamp = DateTime.UtcNow
            });

            return new NotificationSendResult
            {
                Success = true,
                DeliveredRecipient = recipient,
                ModeUsed = "SignalR WebPush Dispatcher"
            };
        }
    }
}
