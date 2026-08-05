using FluentValidation;
using HRMS.Application.DTOs;

namespace HRMS.Application.Validators
{
    public class CreateTenantRequestValidator : AbstractValidator<CreateTenantRequest>
    {
        public CreateTenantRequestValidator()
        {
            RuleFor(x => x.TenantName)
                .NotEmpty().WithMessage("Tenant Name is required.")
                .Length(3, 200).WithMessage("Tenant Name must be between 3 and 200 characters.");

            RuleFor(x => x.TenantCode)
                .NotEmpty().WithMessage("Tenant Code is required.")
                .MinimumLength(3).WithMessage("Tenant Code must be at least 3 characters.")
                .Matches("^[A-Za-z0-9]+$").WithMessage("Tenant Code must be alphanumeric.");
        }
    }
}
