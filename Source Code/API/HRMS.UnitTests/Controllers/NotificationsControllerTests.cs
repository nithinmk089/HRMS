using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.API.Controllers;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace HRMS.UnitTests.Controllers
{
    public class NotificationsControllerTests
    {
        private readonly Mock<INotificationRepository> _repoMock;
        private readonly Mock<IEmailNotificationService> _emailServiceMock;
        private readonly Mock<ISignalRNotificationService> _signalRServiceMock;
        private readonly NotificationsController _controller;

        public NotificationsControllerTests()
        {
            _repoMock = new Mock<INotificationRepository>();
            _emailServiceMock = new Mock<IEmailNotificationService>();
            _signalRServiceMock = new Mock<ISignalRNotificationService>();
            _controller = new NotificationsController(_repoMock.Object, _emailServiceMock.Object, _signalRServiceMock.Object);
        }

        [Fact]
        public async Task CreateTemplate_ReturnsCreatedAtAction_WhenValid()
        {
            // Arrange
            var request = new CreateNotificationTemplateRequest
            {
                TenantId = 1,
                TemplateName = "Welcome Email",
                BusinessEvent = "Tenant Created",
                Channel = "Email",
                SubjectTemplate = "Welcome to {Company}",
                BodyTemplate = "Hello {FirstName}"
            };
            _repoMock.Setup(r => r.CreateTemplateAsync(It.IsAny<CreateNotificationTemplateRequest>())).ReturnsAsync(10L);

            // Act
            var result = await _controller.CreateTemplate(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(createdResult.Value);
            Assert.True(response.Success);
            Assert.Equal(10L, response.Data);
        }

        [Fact]
        public async Task CreateTemplate_ReturnsBadRequest_WhenInvalid()
        {
            // Arrange
            var request = new CreateNotificationTemplateRequest
            {
                TemplateName = "" // Invalid
            };

            // Act
            var result = await _controller.CreateTemplate(request);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(badResult.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task GetTemplateById_ReturnsOk_WhenFound()
        {
            // Arrange
            var templateDto = new NotificationTemplateDto
            {
                TemplateId = 99L,
                TenantId = 1L,
                TemplateName = "Test Template",
                BusinessEvent = "Tenant Created",
                Channel = "In-App",
                SubjectTemplate = "Subject",
                BodyTemplate = "Body",
                IsActive = true,
                VersionNo = 1
            };
            _repoMock.Setup(r => r.GetTemplateByIdAsync(99L, 1L)).ReturnsAsync(templateDto);

            // Act
            var result = await _controller.GetTemplateById(99L, 1L);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<NotificationTemplateDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(99L, response.Data.TemplateId);
        }

        [Fact]
        public async Task GetTemplateById_ReturnsNotFound_WhenNull()
        {
            // Arrange
            _repoMock.Setup(r => r.GetTemplateByIdAsync(99L, 1L)).ReturnsAsync((NotificationTemplateDto?)null);

            // Act
            var result = await _controller.GetTemplateById(99L, 1L);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<NotificationTemplateDto>>(notFoundResult.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task SendNotification_ReturnsOk_WhenValid()
        {
            // Arrange
            var request = new SendNotificationRequest
            {
                TenantId = 1,
                Sender = "System",
                Recipient = "user@test.com",
                Channel = "Email",
                BusinessEvent = "Tenant Created",
                Subject = "Test Subject",
                Body = "Test Body"
            };
            _repoMock.Setup(r => r.QueueNotificationAsync(It.IsAny<SendNotificationRequest>())).ReturnsAsync(101L);

            // Act
            var result = await _controller.SendNotification(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<long>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(101L, response.Data);
        }

        [Fact]
        public async Task RetryNotification_ReturnsOk()
        {
            // Arrange
            _repoMock.Setup(r => r.UpdateNotificationStatusAsync(5L, 1L, "Queued", null, 0, It.IsAny<DateTime>(), null, null, null, 1L)).ReturnsAsync(true);

            // Act
            var result = await _controller.RetryNotification(5L, 1L);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task CancelNotification_ReturnsOk()
        {
            // Arrange
            _repoMock.Setup(r => r.UpdateNotificationStatusAsync(5L, 1L, "Cancelled", null, null, null, null, null, null, 1L)).ReturnsAsync(true);

            // Act
            var result = await _controller.CancelNotification(5L, 1L);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task GetMetrics_ReturnsOk()
        {
            // Arrange
            var metrics = new NotificationMetricsDto { TotalProcessed = 10, DeliveryRate = 90.0m };
            _repoMock.Setup(r => r.GetMetricsAsync(1L)).ReturnsAsync(metrics);

            // Act
            var result = await _controller.GetMetrics(1L);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<NotificationMetricsDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(90.0m, response.Data!.DeliveryRate);
        }

        [Fact]
        public async Task GetEmailSettings_ReturnsOk()
        {
            // Arrange
            var settings = new EmailSettingsDto { Mode = "Development", DevRecipient = "dev@test.com" };
            _emailServiceMock.Setup(s => s.GetEmailSettingsAsync(1L)).ReturnsAsync(settings);

            // Act
            var result = await _controller.GetEmailSettings(1L);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<EmailSettingsDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Development", response.Data!.Mode);
        }
    }
}
