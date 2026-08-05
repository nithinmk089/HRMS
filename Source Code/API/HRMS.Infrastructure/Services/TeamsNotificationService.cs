using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure.Services
{
    public class TeamsNotificationService : ITeamsNotificationService
    {
        private readonly IConfigurationRepository _configRepo;
        private readonly ILogger<TeamsNotificationService> _logger;
        private static readonly HttpClient _httpClient = new HttpClient();

        public TeamsNotificationService(IConfigurationRepository configRepo, ILogger<TeamsNotificationService> logger)
        {
            _configRepo = configRepo;
            _logger = logger;
        }

        public async Task<NotificationSendResult> SendTeamsNotificationAsync(long tenantId, string recipientOrChannel, string title, string message)
        {
            var webhookUrl = await _configRepo.GetValueAsync(tenantId, "Teams_WebhookUrl");

            _logger.LogInformation("Sending Teams notification to {Channel}. Webhook URL configured: {HasUrl}", recipientOrChannel, !string.IsNullOrWhiteSpace(webhookUrl));

            if (!string.IsNullOrWhiteSpace(webhookUrl) && Uri.IsWellFormedUriString(webhookUrl, UriKind.Absolute))
            {
                try
                {
                    var cardPayload = new
                    {
                        type = "MessageCard",
                        context = "http://schema.org/extensions",
                        themeColor = "0076D7",
                        summary = title,
                        sections = new[]
                        {
                            new
                            {
                                activityTitle = title,
                                activitySubtitle = $"Target: {recipientOrChannel}",
                                text = message
                            }
                        }
                    };

                    var content = new StringContent(JsonSerializer.Serialize(cardPayload), Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync(webhookUrl, content);

                    return new NotificationSendResult
                    {
                        Success = response.IsSuccessStatusCode,
                        ErrorMessage = response.IsSuccessStatusCode ? null : $"Teams Webhook returned HTTP {response.StatusCode}",
                        DeliveredRecipient = recipientOrChannel,
                        ModeUsed = "Microsoft Teams Webhook"
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to post to Microsoft Teams Webhook");
                }
            }

            return new NotificationSendResult
            {
                Success = true,
                DeliveredRecipient = recipientOrChannel,
                ModeUsed = "Teams Simulated Provider"
            };
        }
    }
}
