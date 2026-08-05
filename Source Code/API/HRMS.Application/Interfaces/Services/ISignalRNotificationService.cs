using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Services
{
    public interface ISignalRNotificationService
    {
        Task SendNotificationToUserAsync(string userId, object notificationPayload);
        Task SendNotificationToTenantAsync(long tenantId, object notificationPayload);
        Task BroadcastNotificationAsync(object notificationPayload);
    }
}
