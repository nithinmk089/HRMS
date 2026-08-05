using System;
using FluentValidation;
using HRMS.Application.DTOs;

namespace HRMS.Application.Validators
{
    public class CreateEmployeeTransferRequestValidator : AbstractValidator<CreateEmployeeTransferRequest>
    {
        public CreateEmployeeTransferRequestValidator()
        {
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.FromDepartmentId).GreaterThan(0).WithMessage("From Department ID is required.");
            RuleFor(x => x.ToDepartmentId).GreaterThan(0).WithMessage("To Department ID is required.");
            RuleFor(x => x.FromLocationId).GreaterThan(0).WithMessage("From Location ID is required.");
            RuleFor(x => x.ToLocationId).GreaterThan(0).WithMessage("To Location ID is required.");
            RuleFor(x => x.EffectiveDate).NotEmpty().WithMessage("Effective Date is required.");
            RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("Created By is required.");
        }
    }

    public class CreateEmployeePromotionRequestValidator : AbstractValidator<CreateEmployeePromotionRequest>
    {
        public CreateEmployeePromotionRequestValidator()
        {
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.OldDesignationId).GreaterThan(0).WithMessage("Old Designation ID is required.");
            RuleFor(x => x.NewDesignationId).GreaterThan(0).WithMessage("New Designation ID is required.");
            RuleFor(x => x.EffectiveDate).NotEmpty().WithMessage("Effective Date is required.");
            RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("Created By is required.");
        }
    }
}
