using System;
using System.Linq;
using FluentValidation;
using HRMS.Application.DTOs;

namespace HRMS.Application.Validators
{
    public class CreateNotificationTemplateRequestValidator : AbstractValidator<CreateNotificationTemplateRequest>
    {
        private static readonly string[] ValidChannels = { "In-App", "Email", "SMS", "Push", "Teams" };
        private static readonly string[] ValidEvents = { "Tenant Created", "Subscription Expiring", "Payment Failed", "License Threshold Reached" };

        public CreateNotificationTemplateRequestValidator()
        {
            RuleFor(x => x.TemplateName)
                .NotEmpty().WithMessage("Template Name is required.")
                .Length(3, 100).WithMessage("Template Name must be between 3 and 100 characters.");

            RuleFor(x => x.BusinessEvent)
                .NotEmpty().WithMessage("Business Event is required.")
                .Must(x => ValidEvents.Contains(x)).WithMessage($"Business Event must be one of: {string.Join(", ", ValidEvents)}");

            RuleFor(x => x.Channel)
                .NotEmpty().WithMessage("Channel is required.")
                .Must(x => ValidChannels.Contains(x)).WithMessage($"Channel must be one of: {string.Join(", ", ValidChannels)}");

            RuleFor(x => x.BodyTemplate)
                .NotEmpty().WithMessage("Body Template is required.");

            RuleFor(x => x.SubjectTemplate)
                .NotEmpty().WithMessage("Subject Template is required for Email and Teams channels.")
                .When(x => x.Channel == "Email" || x.Channel == "Teams");
        }
    }

    public class UpdateNotificationTemplateRequestValidator : AbstractValidator<UpdateNotificationTemplateRequest>
    {
        private static readonly string[] ValidChannels = { "In-App", "Email", "SMS", "Push", "Teams" };
        private static readonly string[] ValidEvents = { "Tenant Created", "Subscription Expiring", "Payment Failed", "License Threshold Reached" };

        public UpdateNotificationTemplateRequestValidator()
        {
            RuleFor(x => x.TemplateId).GreaterThan(0).WithMessage("Template ID must be greater than 0.");
            
            RuleFor(x => x.TemplateName)
                .NotEmpty().WithMessage("Template Name is required.")
                .Length(3, 100).WithMessage("Template Name must be between 3 and 100 characters.");

            RuleFor(x => x.BusinessEvent)
                .NotEmpty().WithMessage("Business Event is required.")
                .Must(x => ValidEvents.Contains(x)).WithMessage($"Business Event must be one of: {string.Join(", ", ValidEvents)}");

            RuleFor(x => x.Channel)
                .NotEmpty().WithMessage("Channel is required.")
                .Must(x => ValidChannels.Contains(x)).WithMessage($"Channel must be one of: {string.Join(", ", ValidChannels)}");

            RuleFor(x => x.BodyTemplate)
                .NotEmpty().WithMessage("Body Template is required.");

            RuleFor(x => x.SubjectTemplate)
                .NotEmpty().WithMessage("Subject Template is required for Email and Teams channels.")
                .When(x => x.Channel == "Email" || x.Channel == "Teams");
        }
    }

    public class SendNotificationRequestValidator : AbstractValidator<SendNotificationRequest>
    {
        private static readonly string[] ValidChannels = { "In-App", "Email", "SMS", "Push", "Teams" };
        private static readonly string[] ValidEvents = { "Tenant Created", "Subscription Expiring", "Payment Failed", "License Threshold Reached" };

        public SendNotificationRequestValidator()
        {
            RuleFor(x => x.Recipient)
                .NotEmpty().WithMessage("Recipient is required.");

            RuleFor(x => x.Recipient)
                .EmailAddress().WithMessage("Recipient must be a valid email address for Email channel.")
                .When(x => x.Channel == "Email");

            RuleFor(x => x.Channel)
                .NotEmpty().WithMessage("Channel is required.")
                .Must(x => ValidChannels.Contains(x)).WithMessage($"Channel must be one of: {string.Join(", ", ValidChannels)}");

            RuleFor(x => x.BusinessEvent)
                .NotEmpty().WithMessage("Business Event is required.")
                .Must(x => ValidEvents.Contains(x)).WithMessage($"Business Event must be one of: {string.Join(", ", ValidEvents)}");

            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Body is required.");

            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Subject is required for Email and Teams channels.")
                .When(x => x.Channel == "Email" || x.Channel == "Teams");
        }
    }

    public class SaveUserNotificationPreferenceRequestValidator : AbstractValidator<SaveUserNotificationPreferenceRequest>
    {
        private static readonly string[] ValidChannels = { "In-App", "Email", "SMS", "Push", "Teams" };
        private static readonly string[] ValidEvents = { "Tenant Created", "Subscription Expiring", "Payment Failed", "License Threshold Reached" };
        private static readonly string[] ValidFrequencies = { "Immediate", "Daily Digest", "Weekly Digest" };

        public SaveUserNotificationPreferenceRequestValidator()
        {
            RuleFor(x => x.UserID).GreaterThan(0).WithMessage("User ID must be greater than 0.");
            
            RuleFor(x => x.BusinessEvent)
                .NotEmpty().WithMessage("Business Event is required.")
                .Must(x => ValidEvents.Contains(x)).WithMessage($"Business Event must be one of: {string.Join(", ", ValidEvents)}");

            RuleFor(x => x.Channel)
                .NotEmpty().WithMessage("Channel is required.")
                .Must(x => ValidChannels.Contains(x)).WithMessage($"Channel must be one of: {string.Join(", ", ValidChannels)}");

            RuleFor(x => x.Frequency)
                .NotEmpty().WithMessage("Frequency is required.")
                .Must(x => ValidFrequencies.Contains(x)).WithMessage($"Frequency must be one of: {string.Join(", ", ValidFrequencies)}");
        }
    }
}
