using FluentValidation;
using HRMS.Application.DTOs;

namespace HRMS.Application.Validators
{
    public class CreateCourseRequestValidator : AbstractValidator<CreateCourseRequest>
    {
        public CreateCourseRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0);
            RuleFor(x => x.CourseCode).NotEmpty().WithMessage("Course Code is required.");
            RuleFor(x => x.CourseName).NotEmpty().WithMessage("Course Name is required.");
            RuleFor(x => x.CourseCategoryID).GreaterThan(0).WithMessage("Course Category is required.");
        }
    }

    public class CreateLearningAssessmentRequestValidator : AbstractValidator<CreateLearningAssessmentRequest>
    {
        public CreateLearningAssessmentRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0);
            RuleFor(x => x.AssessmentName).NotEmpty().WithMessage("Assessment Name is required.");
            RuleFor(x => x.PassPercentage).InclusiveBetween(1, 100).WithMessage("Pass Percentage must be between 1 and 100.");
        }
    }

    public class SubmitAssessmentResultRequestValidator : AbstractValidator<SubmitAssessmentResultRequest>
    {
        public SubmitAssessmentResultRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0);
            RuleFor(x => x.AssessmentID).GreaterThan(0);
            RuleFor(x => x.EmployeeID).GreaterThan(0);
            RuleFor(x => x.Score).InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100.");
        }
    }
}
