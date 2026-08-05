using FluentValidation.TestHelper;
using HRMS.Application.DTOs;
using HRMS.Application.Validators;
using Xunit;

namespace HRMS.UnitTests.Validators
{
    public class NotificationValidatorsTests
    {
        private readonly CreateNotificationTemplateRequestValidator _createTemplateValidator;
        private readonly UpdateNotificationTemplateRequestValidator _updateTemplateValidator;
        private readonly SendNotificationRequestValidator _sendNotificationValidator;
        private readonly SaveUserNotificationPreferenceRequestValidator _preferenceValidator;

        public NotificationValidatorsTests()
        {
            _createTemplateValidator = new CreateNotificationTemplateRequestValidator();
            _updateTemplateValidator = new UpdateNotificationTemplateRequestValidator();
            _sendNotificationValidator = new SendNotificationRequestValidator();
            _preferenceValidator = new SaveUserNotificationPreferenceRequestValidator();
        }

        [Fact]
        public void CreateTemplate_ShouldHaveError_WhenNameIsEmpty()
        {
            var request = new CreateNotificationTemplateRequest { TemplateName = "" };
            var result = _createTemplateValidator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.TemplateName);
        }

        [Fact]
        public void CreateTemplate_ShouldHaveError_WhenChannelIsInvalid()
        {
            var request = new CreateNotificationTemplateRequest { Channel = "Carrier Pigeon" };
            var result = _createTemplateValidator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Channel);
        }

        [Fact]
        public void CreateTemplate_ShouldNotHaveError_WhenRequestIsValid()
        {
            var request = new CreateNotificationTemplateRequest
            {
                TemplateName = "Default Welcome",
                BusinessEvent = "Tenant Created",
                Channel = "Email",
                SubjectTemplate = "Welcome to HRM",
                BodyTemplate = "Hi {TenantName}"
            };
            var result = _createTemplateValidator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void SendNotification_ShouldHaveError_WhenEmailIsInvalid()
        {
            var request = new SendNotificationRequest
            {
                Channel = "Email",
                Recipient = "invalid_email_format",
                BusinessEvent = "Tenant Created",
                Subject = "Hello",
                Body = "Welcome"
            };
            var result = _sendNotificationValidator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Recipient);
        }

        [Fact]
        public void Preferences_ShouldHaveError_WhenFrequencyIsInvalid()
        {
            var request = new SaveUserNotificationPreferenceRequest
            {
                UserID = 1,
                BusinessEvent = "Tenant Created",
                Channel = "Email",
                Frequency = "Bi-Weekly" // Invalid Frequency
            };
            var result = _preferenceValidator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Frequency);
        }
    }
}
