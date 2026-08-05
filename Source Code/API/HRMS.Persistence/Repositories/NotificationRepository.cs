using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HRMS.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly string _connectionString;
        public NotificationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // --- Template CRUD ---
        public async Task<long> CreateTemplateAsync(CreateNotificationTemplateRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@TemplateName", request.TemplateName);
            p.Add("@BusinessEvent", request.BusinessEvent);
            p.Add("@Channel", request.Channel);
            p.Add("@SubjectTemplate", request.SubjectTemplate);
            p.Add("@BodyTemplate", request.BodyTemplate);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@TemplateID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("communication.usp_NotificationTemplate_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@TemplateID");
        }

        public async Task<bool> UpdateTemplateAsync(UpdateNotificationTemplateRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TemplateID", request.TemplateId);
            p.Add("@TenantID", request.TenantId);
            p.Add("@TemplateName", request.TemplateName);
            p.Add("@BusinessEvent", request.BusinessEvent);
            p.Add("@Channel", request.Channel);
            p.Add("@SubjectTemplate", request.SubjectTemplate);
            p.Add("@BodyTemplate", request.BodyTemplate);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("communication.usp_NotificationTemplate_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteTemplateAsync(long templateId, long tenantId, long deletedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TemplateID", templateId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", deletedBy);
            var affected = await conn.ExecuteAsync("communication.usp_NotificationTemplate_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<NotificationTemplateDto?> GetTemplateByIdAsync(long templateId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TemplateID", templateId);
            p.Add("@TenantID", tenantId);
            return await conn.QueryFirstOrDefaultAsync<NotificationTemplateDto>(
                "communication.usp_NotificationTemplate_GetById", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<NotificationTemplateDto>> SearchTemplatesAsync(long tenantId, string? searchText, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@SearchText", searchText);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("communication.usp_NotificationTemplate_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count
            return await multi.ReadAsync<NotificationTemplateDto>();
        }

        // --- Queue Actions ---
        public async Task<long> QueueNotificationAsync(SendNotificationRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@Sender", request.Sender);
            p.Add("@Recipient", request.Recipient);
            p.Add("@Channel", request.Channel);
            p.Add("@BusinessEvent", request.BusinessEvent);
            p.Add("@Subject", request.Subject);
            p.Add("@Body", request.Body);
            p.Add("@Status", "Queued");
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@NotificationQueueID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("communication.usp_NotificationQueue_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@NotificationQueueID");
        }

        public async Task<bool> UpdateNotificationStatusAsync(long queueId, long tenantId, string status, string? errorMessage = null, int? retryCount = null, DateTime? nextRunDate = null, DateTime? sentDate = null, DateTime? deliveredDate = null, DateTime? readDate = null, long? modifiedBy = null)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@NotificationQueueID", queueId);
            p.Add("@TenantID", tenantId);
            p.Add("@Status", status);
            p.Add("@ErrorMessage", errorMessage);
            p.Add("@RetryCount", retryCount);
            p.Add("@NextRunDate", nextRunDate);
            p.Add("@SentDate", sentDate);
            p.Add("@DeliveredDate", deliveredDate);
            p.Add("@ReadDate", readDate);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("communication.usp_NotificationQueue_UpdateStatus", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<NotificationQueueDto>> GetPendingNotificationsAsync(int maxBatchSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@MaxBatchSize", maxBatchSize);
            return await conn.QueryAsync<NotificationQueueDto>("communication.usp_NotificationQueue_GetPending", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<NotificationQueueDto>> SearchQueueAsync(long tenantId, string? channel, string? status, string? businessEvent, int page, int pageSize)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@Channel", channel);
            p.Add("@Status", status);
            p.Add("@BusinessEvent", businessEvent);
            p.Add("@PageNumber", page);
            p.Add("@PageSize", pageSize);
            using var multi = await conn.QueryMultipleAsync("communication.usp_NotificationQueue_Search", p, commandType: CommandType.StoredProcedure);
            await multi.ReadSingleAsync<dynamic>(); // total count
            return await multi.ReadAsync<NotificationQueueDto>();
        }

        public async Task<NotificationMetricsDto> GetMetricsAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            return await conn.QueryFirstOrDefaultAsync<NotificationMetricsDto>("communication.usp_NotificationQueue_GetMetrics", p, commandType: CommandType.StoredProcedure)
                ?? new NotificationMetricsDto();
        }

        // --- Escalation Rules ---
        public async Task<IEnumerable<NotificationQueueDto>> GetUnreadNotificationsForEscalationAsync(int hours)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@Hours", hours);
            return await conn.QueryAsync<NotificationQueueDto>("communication.usp_NotificationQueue_GetUnreadForEscalation", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> EscalateNotificationAsync(long queueId, long tenantId, string escalationRule, long modifiedBy)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@NotificationQueueID", queueId);
            p.Add("@TenantID", tenantId);
            p.Add("@EscalationRule", escalationRule);
            p.Add("@ModifiedBy", modifiedBy);
            var affected = await conn.ExecuteAsync("communication.usp_NotificationQueue_Escalate", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        // --- User Preferences ---
        public async Task<bool> SaveUserPreferenceAsync(SaveUserNotificationPreferenceRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantId);
            p.Add("@UserID", request.UserID);
            p.Add("@BusinessEvent", request.BusinessEvent);
            p.Add("@Channel", request.Channel);
            p.Add("@Frequency", request.Frequency);
            p.Add("@IsEnabled", request.IsEnabled);
            p.Add("@CreatedBy", request.CreatedBy);
            var affected = await conn.ExecuteAsync("communication.usp_UserNotificationPreference_Save", p, commandType: CommandType.StoredProcedure);
            return affected >= 0; // return true even if affected is 0 because merge/update could return 0 if no fields changed
        }

        public async Task<IEnumerable<UserNotificationPreferenceDto>> GetUserPreferencesAsync(long userId, long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@UserID", userId);
            p.Add("@TenantID", tenantId);
            return await conn.QueryAsync<UserNotificationPreferenceDto>("communication.usp_UserNotificationPreference_GetByUserId", p, commandType: CommandType.StoredProcedure);
        }
    }
}
