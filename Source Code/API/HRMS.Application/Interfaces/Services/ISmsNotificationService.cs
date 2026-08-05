using System.Threading.Tasks;

namespace HRMS.Application.Interfaces.Services
{
    public interface ISmsNotificationService
    {
        Task<NotificationSendResult> SendSmsAsync(long tenantId, string recipientPhone, string message);
    }
}
