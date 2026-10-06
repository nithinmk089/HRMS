using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            var tenantId = user?.FindFirst("tenantId")?.Value;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _logger.LogInformation("SignalR client connected: {ConnectionId} (User: {UserId}, Tenant: {TenantId})",
                Context.ConnectionId, userId, tenantId);

            // Automatically join verified groups based on claims
            if (!string.IsNullOrEmpty(tenantId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"tenant_{tenantId}");
            }
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("SignalR client disconnected: {ConnectionId}", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinTenantGroup(string tenantId)
        {
            var currentTenant = Context.User?.FindFirst("tenantId")?.Value;
            bool isSysAdmin = Context.User?.IsInRole("SYSADMIN") == true;

            // Only allow subscribing to user's own tenant unless sysadmin
            if (isSysAdmin || (!string.IsNullOrEmpty(currentTenant) && currentTenant == tenantId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"tenant_{tenantId}");
                _logger.LogInformation("Connection {ConnectionId} joined tenant group: tenant_{TenantId}", Context.ConnectionId, tenantId);
            }
            else
            {
                _logger.LogWarning("Unauthorized attempt by connection {ConnectionId} to join tenant group: tenant_{TenantId}", Context.ConnectionId, tenantId);
            }
        }

        public async Task JoinUserGroup(string userId)
        {
            var currentUserId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool isSysAdmin = Context.User?.IsInRole("SYSADMIN") == true;

            // Only allow subscribing to user's own user group unless sysadmin
            if (isSysAdmin || (!string.IsNullOrEmpty(currentUserId) && currentUserId == userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
                _logger.LogInformation("Connection {ConnectionId} joined user group: user_{UserId}", Context.ConnectionId, userId);
            }
            else
            {
                _logger.LogWarning("Unauthorized attempt by connection {ConnectionId} to join user group: user_{UserId}", Context.ConnectionId, userId);
            }
        }

        public async Task LeaveUserGroup(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
            }
        }
    }
}
