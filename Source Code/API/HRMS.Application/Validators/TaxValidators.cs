using FluentValidation;
using HRMS.Application.DTOs;

namespace HRMS.Application.Validators
{
    public class CreateTaxRegimeRequestValidator : AbstractValidator<CreateTaxRegimeRequest>
    {
        public CreateTaxRegimeRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.RegimeName).NotEmpty().WithMessage("Regime Name is required.");
            RuleFor(x => x.EffectiveFrom).NotEmpty().WithMessage("Effective From date is required.");
        }
    }

    public class CreateTaxSlabRequestValidator : AbstractValidator<CreateTaxSlabRequest>
    {
        public CreateTaxSlabRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.TaxRegimeID).GreaterThan(0).WithMessage("Tax Regime ID is required.");
            RuleFor(x => x.IncomeFrom).GreaterThanOrEqualTo(0).WithMessage("Income From must be non-negative.");
            RuleFor(x => x.TaxRate).GreaterThanOrEqualTo(0).WithMessage("Tax Rate must be non-negative.");
        }
    }

    public class CreateTaxDeclarationRequestValidator : AbstractValidator<CreateTaxDeclarationRequest>
    {
        public CreateTaxDeclarationRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.EmployeeID).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.FinancialYear).NotEmpty().WithMessage("Financial Year is required.");
            RuleFor(x => x.DeclaredAmount).GreaterThanOrEqualTo(0).WithMessage("Declared Amount must be non-negative.");
        }
    }
}
