using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        // Template CRUD
        Task<long> CreateTemplateAsync(CreateNotificationTemplateRequest request);
        Task<bool> UpdateTemplateAsync(UpdateNotificationTemplateRequest request);
        Task<bool> DeleteTemplateAsync(long templateId, long tenantId, long deletedBy);
        Task<NotificationTemplateDto?> GetTemplateByIdAsync(long templateId, long tenantId);
        Task<IEnumerable<NotificationTemplateDto>> SearchTemplatesAsync(long tenantId, string? searchText, int page, int pageSize);

        // Queue Actions
        Task<long> QueueNotificationAsync(SendNotificationRequest request);
        Task<bool> UpdateNotificationStatusAsync(long queueId, long tenantId, string status, string? errorMessage = null, int? retryCount = null, DateTime? nextRunDate = null, DateTime? sentDate = null, DateTime? deliveredDate = null, DateTime? readDate = null, long? modifiedBy = null);
        Task<IEnumerable<NotificationQueueDto>> GetPendingNotificationsAsync(int maxBatchSize);
        Task<IEnumerable<NotificationQueueDto>> SearchQueueAsync(long tenantId, string? channel, string? status, string? businessEvent, int page, int pageSize);
        Task<NotificationMetricsDto> GetMetricsAsync(long tenantId);

        // Escalation Rules
        Task<IEnumerable<NotificationQueueDto>> GetUnreadNotificationsForEscalationAsync(int hours);
        Task<bool> EscalateNotificationAsync(long queueId, long tenantId, string escalationRule, long modifiedBy);

        // User Preferences
        Task<bool> SaveUserPreferenceAsync(SaveUserNotificationPreferenceRequest request);
        Task<IEnumerable<UserNotificationPreferenceDto>> GetUserPreferencesAsync(long userId, long tenantId);
    }
}
