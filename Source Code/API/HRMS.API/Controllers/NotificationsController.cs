using System;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using HRMS.Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/notifications")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly ISignalRNotificationService _signalRNotificationService;

        public NotificationsController(
            INotificationRepository notificationRepository,
            IEmailNotificationService emailNotificationService,
            ISignalRNotificationService signalRNotificationService)
        {
            _notificationRepository = notificationRepository;
            _emailNotificationService = emailNotificationService;
            _signalRNotificationService = signalRNotificationService;
        }

        // --- Template Endpoints ---
        [HttpPost("templates")]
        public async Task<IActionResult> CreateTemplate([FromBody] CreateNotificationTemplateRequest r)
        {
            var validator = new CreateNotificationTemplateRequestValidator();
            var valResult = await validator.ValidateAsync(r);
            if (!valResult.IsValid)
            {
                return BadRequest(ApiResponse<long>.FailureResult(string.Join(" ", valResult.Errors.Select(e => e.ErrorMessage))));
            }

            r.CreatedBy = 1;
            var id = await _notificationRepository.CreateTemplateAsync(r);
            return CreatedAtAction(nameof(GetTemplateById), new { id, tenantId = r.TenantId }, ApiResponse<long>.SuccessResult(id, "Notification template created."));
        }

        [HttpPut("templates/{id}")]
        public async Task<IActionResult> UpdateTemplate(long id, [FromBody] UpdateNotificationTemplateRequest r)
        {
            r.TemplateId = id;
            var validator = new UpdateNotificationTemplateRequestValidator();
            var valResult = await validator.ValidateAsync(r);
            if (!valResult.IsValid)
            {
                return BadRequest(ApiResponse<bool>.FailureResult(string.Join(" ", valResult.Errors.Select(e => e.ErrorMessage))));
            }

            r.ModifiedBy = 1;
            var ok = await _notificationRepository.UpdateTemplateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Notification template updated."));
        }

        [HttpDelete("templates/{id}")]
        public async Task<IActionResult> DeleteTemplate(long id, [FromQuery] long tenantId)
        {
            var ok = await _notificationRepository.DeleteTemplateAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Notification template deleted."));
        }

        [HttpGet("templates/{id}")]
        public async Task<IActionResult> GetTemplateById(long id, [FromQuery] long tenantId)
        {
            var template = await _notificationRepository.GetTemplateByIdAsync(id, tenantId);
            if (template == null) return NotFound(ApiResponse<NotificationTemplateDto>.FailureResult("Template not found."));
            return Ok(ApiResponse<NotificationTemplateDto>.SuccessResult(template));
        }

        [HttpGet("templates")]
        public async Task<IActionResult> SearchTemplates([FromQuery] long tenantId, [FromQuery] string? searchText, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var templates = await _notificationRepository.SearchTemplatesAsync(tenantId, searchText, page, pageSize);
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<NotificationTemplateDto>>.SuccessResult(templates));
        }

        // --- Queue Endpoints ---
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationRequest r)
        {
            var validator = new SendNotificationRequestValidator();
            var valResult = await validator.ValidateAsync(r);
            if (!valResult.IsValid)
            {
                return BadRequest(ApiResponse<long>.FailureResult(string.Join(" ", valResult.Errors.Select(e => e.ErrorMessage))));
            }

            // Encrypt body payload before saving
            r.CreatedBy = 1;
            r.Body = HRMS.Infrastructure.EncryptionHelper.Encrypt(r.Body);

            var id = await _notificationRepository.QueueNotificationAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Notification successfully queued."));
        }

        [HttpPost("{id}/retry")]
        public async Task<IActionResult> RetryNotification(long id, [FromQuery] long tenantId)
        {
            var ok = await _notificationRepository.UpdateNotificationStatusAsync(
                id, tenantId, status: "Queued", retryCount: 0, nextRunDate: DateTime.UtcNow, modifiedBy: 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Notification reset and queued for retry."));
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelNotification(long id, [FromQuery] long tenantId)
        {
            var ok = await _notificationRepository.UpdateNotificationStatusAsync(
                id, tenantId, status: "Cancelled", modifiedBy: 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Notification successfully cancelled."));
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> ReadNotification(long id, [FromQuery] long tenantId)
        {
            var ok = await _notificationRepository.UpdateNotificationStatusAsync(
                id, tenantId, status: "Read", readDate: DateTime.UtcNow, modifiedBy: 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Notification marked as read."));
        }

        [HttpGet]
        public async Task<IActionResult> SearchQueue([FromQuery] long tenantId, [FromQuery] string? channel, [FromQuery] string? status, [FromQuery] string? businessEvent, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var queue = await _notificationRepository.SearchQueueAsync(tenantId, channel, status, businessEvent, page, pageSize);
            
            // Decrypt bodies for viewing in history
            foreach (var item in queue)
            {
                item.Body = HRMS.Infrastructure.EncryptionHelper.Decrypt(item.Body);
            }
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<NotificationQueueDto>>.SuccessResult(queue));
        }

        [HttpGet("metrics")]
        public async Task<IActionResult> GetMetrics([FromQuery] long tenantId)
        {
            var metrics = await _notificationRepository.GetMetricsAsync(tenantId);
            return Ok(ApiResponse<NotificationMetricsDto>.SuccessResult(metrics));
        }

        // --- Preferences Endpoints ---
        [HttpGet("preferences/{userId}")]
        public async Task<IActionResult> GetPreferences(long userId, [FromQuery] long tenantId)
        {
            var preferences = await _notificationRepository.GetUserPreferencesAsync(userId, tenantId);
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<UserNotificationPreferenceDto>>.SuccessResult(preferences));
        }

        [HttpPost("preferences")]
        public async Task<IActionResult> SavePreference([FromBody] SaveUserNotificationPreferenceRequest r)
        {
            var validator = new SaveUserNotificationPreferenceRequestValidator();
            var valResult = await validator.ValidateAsync(r);
            if (!valResult.IsValid)
            {
                return BadRequest(ApiResponse<bool>.FailureResult(string.Join(" ", valResult.Errors.Select(e => e.ErrorMessage))));
            }

            r.CreatedBy = 1;
            var ok = await _notificationRepository.SaveUserPreferenceAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Preferences updated."));
        }

        // --- Administration Email & Channel Settings Endpoints ---
        [HttpGet("email-settings")]
        public async Task<IActionResult> GetEmailSettings([FromQuery] long tenantId)
        {
            var settings = await _emailNotificationService.GetEmailSettingsAsync(tenantId);
            return Ok(ApiResponse<EmailSettingsDto>.SuccessResult(settings));
        }

        [HttpPost("email-settings")]
        public async Task<IActionResult> SaveEmailSettings([FromQuery] long tenantId, [FromBody] EmailSettingsDto settings)
        {
            var ok = await _emailNotificationService.SaveEmailSettingsAsync(tenantId, settings, modifiedBy: 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Email notification settings saved successfully."));
        }

        [HttpPost("test-email")]
        public async Task<IActionResult> SendTestEmail([FromQuery] long tenantId, [FromQuery] string recipient)
        {
            if (string.IsNullOrWhiteSpace(recipient)) return BadRequest(ApiResponse<NotificationSendResult>.FailureResult("Recipient email address is required."));
            var result = await _emailNotificationService.SendTestEmailAsync(tenantId, recipient);
            return Ok(ApiResponse<NotificationSendResult>.SuccessResult(result, "Test email dispatch complete."));
        }

        // --- SignalR Broadcast Testing Endpoint ---
        [HttpPost("broadcast-live")]
        public async Task<IActionResult> BroadcastLiveNotification([FromQuery] long tenantId, [FromQuery] string subject, [FromQuery] string body)
        {
            var payload = new
            {
                NotificationId = DateTime.UtcNow.Ticks,
                TenantId = tenantId,
                BusinessEvent = "SYSTEM_TEST",
                Subject = string.IsNullOrWhiteSpace(subject) ? "Live System Alert" : subject,
                Body = string.IsNullOrWhiteSpace(body) ? "This is a real-time SignalR notification broadcast." : body,
                Sender = "System Admin",
                CreatedDate = DateTime.UtcNow
            };

            await _signalRNotificationService.BroadcastNotificationAsync(payload);
            return Ok(ApiResponse<bool>.SuccessResult(true, "Live SignalR notification broadcasted to all connected clients."));
        }
    }
}
