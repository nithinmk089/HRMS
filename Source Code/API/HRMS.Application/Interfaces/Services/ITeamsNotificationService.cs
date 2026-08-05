using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Services
{
    public interface ITeamsNotificationService
    {
        Task<NotificationSendResult> SendTeamsNotificationAsync(long tenantId, string recipientOrChannel, string title, string message);
    }
}
