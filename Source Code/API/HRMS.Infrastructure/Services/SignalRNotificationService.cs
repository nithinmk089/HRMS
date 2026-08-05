using System;
using System.Threading.Tasks;
using HRMS.Application.Interfaces.Services;
using HRMS.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure.Services
{
    public class SignalRNotificationService : ISignalRNotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<SignalRNotificationService> _logger;

        public SignalRNotificationService(IHubContext<NotificationHub> hubContext, ILogger<SignalRNotificationService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task SendNotificationToUserAsync(string userId, object notificationPayload)
        {
            try
            {
                await _hubContext.Clients.Group($"user_{userId}").SendAsync("ReceiveNotification", notificationPayload);
                _logger.LogInformation("SignalR notification sent to user group user_{UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send SignalR notification to user_{UserId}", userId);
            }
        }

        public async Task SendNotificationToTenantAsync(long tenantId, object notificationPayload)
        {
            try
            {
                await _hubContext.Clients.Group($"tenant_{tenantId}").SendAsync("ReceiveNotification", notificationPayload);
                _logger.LogInformation("SignalR notification sent to tenant group tenant_{TenantId}", tenantId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send SignalR notification to tenant_{TenantId}", tenantId);
            }
        }

        public async Task BroadcastNotificationAsync(object notificationPayload)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", notificationPayload);
                _logger.LogInformation("SignalR notification broadcasted to all connected clients.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to broadcast SignalR notification.");
            }
        }
    }
}
