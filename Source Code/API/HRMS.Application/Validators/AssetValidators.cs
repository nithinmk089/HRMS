using FluentValidation;
using HRMS.Application.DTOs;

namespace HRMS.Application.Validators
{
    public class CreateAssetCategoryRequestValidator : AbstractValidator<CreateAssetCategoryRequest>
    {
        public CreateAssetCategoryRequestValidator()
        {
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.CategoryCode).NotEmpty().WithMessage("Category Code is required.").MaximumLength(50).WithMessage("Category Code must not exceed 50 characters.");
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Category Name is required.").MaximumLength(100).WithMessage("Category Name must not exceed 100 characters.");
        }
    }

    public class CreateAssetRequestValidator : AbstractValidator<CreateAssetRequest>
    {
        public CreateAssetRequestValidator()
        {
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.AssetCode).NotEmpty().WithMessage("Asset Code is required.").MaximumLength(50).WithMessage("Asset Code must not exceed 50 characters.");
            RuleFor(x => x.AssetTag).NotEmpty().WithMessage("Asset Tag is required.").MaximumLength(50).WithMessage("Asset Tag must not exceed 50 characters.");
            RuleFor(x => x.AssetName).NotEmpty().WithMessage("Asset Name is required.").MaximumLength(150).WithMessage("Asset Name must not exceed 150 characters.");
            RuleFor(x => x.AssetCategoryID).GreaterThan(0).WithMessage("Asset Category ID must be greater than 0.");
            RuleFor(x => x.SerialNumber).NotEmpty().WithMessage("Serial Number is required.").MaximumLength(100).WithMessage("Serial Number must not exceed 100 characters.");
            RuleFor(x => x.PurchaseDate).NotEmpty().WithMessage("Purchase Date is required.");
            RuleFor(x => x.PurchaseCost).GreaterThanOrEqualTo(0).WithMessage("Purchase Cost must be non-negative.");
        }
    }

    public class CreateAssetAssignmentRequestValidator : AbstractValidator<CreateAssetAssignmentRequest>
    {
        public CreateAssetAssignmentRequestValidator()
        {
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.AssetID).GreaterThan(0).WithMessage("Asset ID is required.");
            RuleFor(x => x.EmployeeID).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.AssignedDate).NotEmpty().WithMessage("Assigned Date is required.");
        }
    }

    public class CreateAssetTransferRequestValidator : AbstractValidator<CreateAssetTransferRequest>
    {
        public CreateAssetTransferRequestValidator()
        {
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.AssetID).GreaterThan(0).WithMessage("Asset ID is required.");
            RuleFor(x => x.ToEmployeeID).GreaterThan(0).WithMessage("To Employee ID is required.");
            RuleFor(x => x.TransferDate).NotEmpty().WithMessage("Transfer Date is required.");
        }
    }
}
