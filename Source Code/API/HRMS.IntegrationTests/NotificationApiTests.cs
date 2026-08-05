using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace HRMS.IntegrationTests
{
    public class NotificationApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<INotificationRepository> _notifRepoMock = new();

        public NotificationApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "TestScheme";
                        options.DefaultChallengeScheme = "TestScheme";
                    }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });

                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(INotificationRepository));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddScoped<INotificationRepository>(_ => _notifRepoMock.Object);
                });
            });
        }

        [Fact]
        public async Task GetMetrics_ReturnsOk_WithData()
        {
            // Arrange
            var metricsDto = new NotificationMetricsDto
            {
                TotalProcessed = 20,
                SentCount = 18,
                DeliveredCount = 17,
                ReadCount = 10,
                FailedCount = 2,
                DeliveryRate = 85.0m,
                FailureRate = 10.0m,
                ReadRate = 58.8m
            };
            _notifRepoMock.Setup(repo => repo.GetMetricsAsync(1L)).ReturnsAsync(metricsDto);

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/notifications/metrics?tenantId=1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<NotificationMetricsDto>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(85.0m, result.Data.DeliveryRate);
            Assert.Equal(2, result.Data.FailedCount);
        }

        [Fact]
        public async Task SendNotification_QueuesCorrectly_WhenValid()
        {
            // Arrange
            var request = new SendNotificationRequest
            {
                TenantId = 1,
                Recipient = "user@test.com",
                Channel = "Email",
                BusinessEvent = "Tenant Created",
                Subject = "Welcome",
                Body = "Welcome body content"
            };
            _notifRepoMock.Setup(repo => repo.QueueNotificationAsync(It.IsAny<SendNotificationRequest>())).ReturnsAsync(12L);

            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/notifications/send", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<long>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(12L, result.Data);
        }

        [Fact]
        public async Task GetPreferences_ReturnsUserPreferences()
        {
            // Arrange
            var preferencesList = new System.Collections.Generic.List<UserNotificationPreferenceDto>
            {
                new UserNotificationPreferenceDto { PreferenceId = 1, TenantId = 1, UserID = 10, BusinessEvent = "Tenant Created", Channel = "Email", Frequency = "Immediate", IsEnabled = true }
            };
            _notifRepoMock.Setup(repo => repo.GetUserPreferencesAsync(10L, 1L)).ReturnsAsync(preferencesList);

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/notifications/preferences/10?tenantId=1");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<System.Collections.Generic.IEnumerable<UserNotificationPreferenceDto>>>();
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Single(result.Data);
            Assert.Equal("Immediate", result.Data.First().Frequency);
        }
    }
}
