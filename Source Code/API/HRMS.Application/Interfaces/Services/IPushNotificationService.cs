using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Services
{
    public interface IPushNotificationService
    {
        Task<NotificationSendResult> SendPushNotificationAsync(long tenantId, string recipient, string title, string message);
    }
}
