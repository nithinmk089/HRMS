using FluentValidation;
using HRMS.Application.DTOs;

namespace HRMS.Application.Validators
{
    public class CreatePerformanceCycleRequestValidator : AbstractValidator<CreatePerformanceCycleRequest>
    {
        public CreatePerformanceCycleRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.CycleCode).NotEmpty().WithMessage("Cycle Code is required.");
            RuleFor(x => x.CycleName).NotEmpty().WithMessage("Cycle Name is required.");
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start Date is required.");
            RuleFor(x => x.EndDate).NotEmpty().WithMessage("End Date is required.");
        }
    }

    public class CreateGoalRequestValidator : AbstractValidator<CreateGoalRequest>
    {
        public CreateGoalRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.EmployeeID).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.PerformanceCycleID).GreaterThan(0).WithMessage("Performance Cycle ID is required.");
            RuleFor(x => x.GoalTitle).NotEmpty().WithMessage("Goal Title is required.");
            RuleFor(x => x.Weightage).InclusiveBetween(1, 100).WithMessage("Weightage must be between 1 and 100.");
        }
    }

    public class CreateGoalProgressRequestValidator : AbstractValidator<CreateGoalProgressRequest>
    {
        public CreateGoalProgressRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.GoalID).GreaterThan(0).WithMessage("Goal ID is required.");
            RuleFor(x => x.ProgressPercentage).InclusiveBetween(0, 100).WithMessage("Progress Percentage must be between 0 and 100.");
        }
    }

    public class CreateFeedbackRequestValidator : AbstractValidator<CreateFeedbackRequest>
    {
        public CreateFeedbackRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.EmployeeID).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.FeedbackText).NotEmpty().WithMessage("Feedback Text is required.");
        }
    }
}
