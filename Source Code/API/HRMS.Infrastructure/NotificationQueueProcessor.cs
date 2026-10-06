using System;
using System.Threading;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure
{
    public class NotificationQueueProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationQueueProcessor> _logger;

        public NotificationQueueProcessor(IServiceProvider serviceProvider, ILogger<NotificationQueueProcessor> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Notification Queue Processor background service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessQueueAsync();
                    await CheckEscalationsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing notification queue/escalations.");
                }

                // Poll every 10 seconds
                await Task.Delay(10000, stoppingToken);
            }

            _logger.LogInformation("Notification Queue Processor background service stopped.");
        }

        private async Task ProcessQueueAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailNotificationService>();
            var signalRService = scope.ServiceProvider.GetRequiredService<ISignalRNotificationService>();
            var smsService = scope.ServiceProvider.GetRequiredService<ISmsNotificationService>();
            var pushService = scope.ServiceProvider.GetRequiredService<IPushNotificationService>();
            var teamsService = scope.ServiceProvider.GetRequiredService<ITeamsNotificationService>();

            var pending = await repo.GetPendingNotificationsAsync(10); // Batch size of 10

            foreach (var notif in pending)
            {
                _logger.LogInformation("Processing notification {NotificationQueueID} for recipient {Recipient} via channel {Channel}", notif.NotificationQueueId, notif.Recipient, notif.Channel);

                string decryptedBody = EncryptionHelper.Decrypt(notif.Body);

                bool success = true;
                string? error = null;

                try
                {
                    // Route based on channel
                    switch (notif.Channel?.Trim())
                    {
                        case "Email":
                            var emailResult = await emailService.SendEmailAsync(notif.TenantId, notif.Recipient, notif.Subject, decryptedBody);
                            success = emailResult.Success;
                            error = emailResult.ErrorMessage;
                            break;

                        case "In-App":
                            await signalRService.SendNotificationToUserAsync(notif.Recipient, new
                            {
                                NotificationId = notif.NotificationQueueId,
                                TenantId = notif.TenantId,
                                BusinessEvent = notif.BusinessEvent,
                                Subject = notif.Subject,
                                Body = decryptedBody,
                                Sender = notif.Sender,
                                CreatedDate = notif.CreatedDate
                            });
                            await signalRService.SendNotificationToTenantAsync(notif.TenantId, new
                            {
                                NotificationId = notif.NotificationQueueId,
                                BusinessEvent = notif.BusinessEvent,
                                Subject = notif.Subject,
                                Sender = notif.Sender
                            });
                            break;

                        case "SMS":
                            var smsResult = await smsService.SendSmsAsync(notif.TenantId, notif.Recipient, decryptedBody);
                            success = smsResult.Success;
                            error = smsResult.ErrorMessage;
                            break;

                        case "Push":
                            var pushResult = await pushService.SendPushNotificationAsync(notif.TenantId, notif.Recipient, notif.Subject, decryptedBody);
                            success = pushResult.Success;
                            error = pushResult.ErrorMessage;
                            break;

                        case "Teams":
                            var teamsResult = await teamsService.SendTeamsNotificationAsync(notif.TenantId, notif.Recipient, notif.Subject, decryptedBody);
                            success = teamsResult.Success;
                            error = teamsResult.ErrorMessage;
                            break;

                        default:
                            // Fallback simulation log for custom channels
                            _logger.LogInformation("[GENERIC CHANNEL SEND] Event: '{BusinessEvent}', Channel: {Channel}, Recipient: {Recipient}, Subject: '{Subject}'",
                                notif.BusinessEvent, notif.Channel, notif.Recipient, notif.Subject);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    error = ex.Message;
                }

                if (success)
                {
                    await repo.UpdateNotificationStatusAsync(
                        notif.NotificationQueueId,
                        notif.TenantId,
                        status: "Delivered",
                        sentDate: DateTime.UtcNow,
                        deliveredDate: DateTime.UtcNow
                    );
                    _logger.LogInformation("Notification {NotificationQueueID} successfully sent via {Channel}.", notif.NotificationQueueId, notif.Channel);
                }
                else
                {
                    int nextRetry = notif.RetryCount + 1;
                    string nextStatus = nextRetry >= notif.MaxRetries ? "Failed" : "Retry";

                    DateTime nextRun = DateTime.UtcNow.AddSeconds(Math.Pow(2, nextRetry) * 10);

                    await repo.UpdateNotificationStatusAsync(
                        notif.NotificationQueueId,
                        notif.TenantId,
                        status: nextStatus,
                        errorMessage: error,
                        retryCount: nextRetry,
                        nextRunDate: nextRun
                    );

                    _logger.LogWarning("Notification {NotificationQueueID} delivery failed. Attempt {Attempt}/{Max}. Next retry at {Next}",
                        notif.NotificationQueueId, nextRetry, notif.MaxRetries, nextRun);
                }
            }
        }

        private async Task CheckEscalationsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

            int[] hoursRules = { 24, 48, 72 };

            foreach (var hours in hoursRules)
            {
                var overdue = await repo.GetUnreadNotificationsForEscalationAsync(hours);
                foreach (var notif in overdue)
                {
                    string label = $"{hours} Hours";
                    _logger.LogWarning("Notification {NotificationQueueID} (Channel: {Channel}, Event: {Event}) for tenant {TenantID} has been unread for more than {Hours} hours. Escalating!",
                        notif.NotificationQueueId, notif.Channel, notif.BusinessEvent, notif.TenantId, hours);

                    await repo.EscalateNotificationAsync(notif.NotificationQueueId, notif.TenantId, label, modifiedBy: 1);

                    await repo.QueueNotificationAsync(new SendNotificationRequest
                    {
                        TenantId = notif.TenantId,
                        Sender = "System Escalation Service",
                        Recipient = "admin@hrms.com",
                        Channel = "Email",
                        BusinessEvent = notif.BusinessEvent,
                        Subject = $"[ESCALATION - {hours}H] Notification unread for recipient: {notif.Recipient}",
                        Body = EncryptionHelper.Encrypt($"The notification with ID {notif.NotificationQueueId} via {notif.Channel} channel has remained unread for more than {hours} hours. Please follow up. Target recipient: {notif.Recipient}"),
                        CreatedBy = 1
                    });
                }
            }
        }
    }
}
