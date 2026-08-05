using System;
using System.Linq;
using FluentValidation;
using HRMS.Application.DTOs;

namespace HRMS.Application.Validators
{
    // --- Employee ---
    public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeRequestValidator()
        {
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID must be valid.");
            RuleFor(x => x.EmployeeCode)
                .NotEmpty().WithMessage("Employee Code is required.")
                .MaximumLength(50).WithMessage("Employee Code cannot exceed 50 characters.")
                .Matches("^[A-Za-z0-9-]+$").WithMessage("Employee Code must be alphanumeric or contain dashes.");

            RuleFor(x => x.EmployeeNumber)
                .NotEmpty().WithMessage("Employee Number is required.")
                .MaximumLength(50).WithMessage("Employee Number cannot exceed 50 characters.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(100).WithMessage("First Name cannot exceed 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(100).WithMessage("Last Name cannot exceed 100 characters.");

            RuleFor(x => x.DateOfBirth)
                .Must(dob => dob == null || dob < DateTime.UtcNow.AddYears(-15))
                .WithMessage("Employee must be at least 15 years old.");

            RuleFor(x => x.PersonalEmail)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.PersonalEmail))
                .WithMessage("Personal Email must be a valid email address.");

            RuleFor(x => x.MobileNumber)
                .MaximumLength(20).WithMessage("Mobile number cannot exceed 20 characters.");

            RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("Created By is required.");
        }
    }

    public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
    {
        public UpdateEmployeeRequestValidator()
        {
            RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(100).WithMessage("First Name cannot exceed 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(100).WithMessage("Last Name cannot exceed 100 characters.");

            RuleFor(x => x.DateOfBirth)
                .Must(dob => dob == null || dob < DateTime.UtcNow.AddYears(-15))
                .WithMessage("Employee must be at least 15 years old.");

            RuleFor(x => x.PersonalEmail)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.PersonalEmail))
                .WithMessage("Personal Email must be a valid email address.");

            RuleFor(x => x.MobileNumber)
                .MaximumLength(20).WithMessage("Mobile number cannot exceed 20 characters.");

            RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage("Modified By is required.");
        }
    }

    // --- Employment ---
    public class CreateEmployeeEmploymentRequestValidator : AbstractValidator<CreateEmployeeEmploymentRequest>
    {
        public CreateEmployeeEmploymentRequestValidator()
        {
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.CompanyId).GreaterThan(0).WithMessage("Company ID is required.");
            RuleFor(x => x.BusinessUnitId).GreaterThan(0).WithMessage("Business Unit ID is required.");
            RuleFor(x => x.DepartmentId).GreaterThan(0).WithMessage("Department ID is required.");
            RuleFor(x => x.DesignationId).GreaterThan(0).WithMessage("Designation ID is required.");
            RuleFor(x => x.LocationId).GreaterThan(0).WithMessage("Location ID is required.");
            RuleFor(x => x.CostCenterId).GreaterThan(0).WithMessage("Cost Center ID is required.");

            RuleFor(x => x.EmploymentType)
                .NotEmpty().WithMessage("Employment Type is required.")
                .MaximumLength(50).WithMessage("Employment Type cannot exceed 50 characters.");

            RuleFor(x => x.JoiningDate)
                .NotEmpty().WithMessage("Joining Date is required.");

            RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("Created By is required.");
        }
    }

    public class UpdateEmployeeEmploymentRequestValidator : AbstractValidator<UpdateEmployeeEmploymentRequest>
    {
        public UpdateEmployeeEmploymentRequestValidator()
        {
            RuleFor(x => x.EmployeeEmploymentId).GreaterThan(0).WithMessage("Employment ID is required.");
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.CompanyId).GreaterThan(0).WithMessage("Company ID is required.");
            RuleFor(x => x.BusinessUnitId).GreaterThan(0).WithMessage("Business Unit ID is required.");
            RuleFor(x => x.DepartmentId).GreaterThan(0).WithMessage("Department ID is required.");
            RuleFor(x => x.DesignationId).GreaterThan(0).WithMessage("Designation ID is required.");
            RuleFor(x => x.LocationId).GreaterThan(0).WithMessage("Location ID is required.");
            RuleFor(x => x.CostCenterId).GreaterThan(0).WithMessage("Cost Center ID is required.");

            RuleFor(x => x.EmploymentType)
                .NotEmpty().WithMessage("Employment Type is required.")
                .MaximumLength(50).WithMessage("Employment Type cannot exceed 50 characters.");

            RuleFor(x => x.JoiningDate)
                .NotEmpty().WithMessage("Joining Date is required.");

            RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage("Modified By is required.");
        }
    }

    // --- Address ---
    public class CreateEmployeeAddressRequestValidator : AbstractValidator<CreateEmployeeAddressRequest>
    {
        public CreateEmployeeAddressRequestValidator()
        {
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.AddressType)
                .NotEmpty().WithMessage("Address Type is required.")
                .MaximumLength(50).WithMessage("Address Type cannot exceed 50 characters.");

            RuleFor(x => x.AddressLine1)
                .NotEmpty().WithMessage("Address Line 1 is required.")
                .MaximumLength(250).WithMessage("Address Line 1 cannot exceed 250 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(100).WithMessage("Country cannot exceed 100 characters.");

            RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("Created By is required.");
        }
    }

    public class UpdateEmployeeAddressRequestValidator : AbstractValidator<UpdateEmployeeAddressRequest>
    {
        public UpdateEmployeeAddressRequestValidator()
        {
            RuleFor(x => x.EmployeeAddressId).GreaterThan(0).WithMessage("Address ID is required.");
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.AddressType)
                .NotEmpty().WithMessage("Address Type is required.")
                .MaximumLength(50).WithMessage("Address Type cannot exceed 50 characters.");

            RuleFor(x => x.AddressLine1)
                .NotEmpty().WithMessage("Address Line 1 is required.")
                .MaximumLength(250).WithMessage("Address Line 1 cannot exceed 250 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(100).WithMessage("Country cannot exceed 100 characters.");

            RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage("Modified By is required.");
        }
    }

    // --- Contact ---
    public class CreateEmployeeContactRequestValidator : AbstractValidator<CreateEmployeeContactRequest>
    {
        public CreateEmployeeContactRequestValidator()
        {
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.ContactType)
                .NotEmpty().WithMessage("Contact Type is required.")
                .MaximumLength(50).WithMessage("Contact Type cannot exceed 50 characters.");

            RuleFor(x => x.ContactValue)
                .NotEmpty().WithMessage("Contact Value is required.")
                .MaximumLength(200).WithMessage("Contact Value cannot exceed 200 characters.");

            RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("Created By is required.");
        }
    }

    public class UpdateEmployeeContactRequestValidator : AbstractValidator<UpdateEmployeeContactRequest>
    {
        public UpdateEmployeeContactRequestValidator()
        {
            RuleFor(x => x.EmployeeContactId).GreaterThan(0).WithMessage("Contact ID is required.");
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.ContactType)
                .NotEmpty().WithMessage("Contact Type is required.")
                .MaximumLength(50).WithMessage("Contact Type cannot exceed 50 characters.");

            RuleFor(x => x.ContactValue)
                .NotEmpty().WithMessage("Contact Value is required.")
                .MaximumLength(200).WithMessage("Contact Value cannot exceed 200 characters.");

            RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage("Modified By is required.");
        }
    }

    // --- Emergency Contact ---
    public class CreateEmployeeEmergencyContactRequestValidator : AbstractValidator<CreateEmployeeEmergencyContactRequest>
    {
        public CreateEmployeeEmergencyContactRequestValidator()
        {
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.ContactName)
                .NotEmpty().WithMessage("Contact Name is required.")
                .MaximumLength(150).WithMessage("Contact Name cannot exceed 150 characters.");

            RuleFor(x => x.Relationship)
                .NotEmpty().WithMessage("Relationship is required.")
                .MaximumLength(50).WithMessage("Relationship cannot exceed 50 characters.");

            RuleFor(x => x.MobileNumber)
                .NotEmpty().WithMessage("Mobile Number is required.")
                .MaximumLength(50).WithMessage("Mobile Number cannot exceed 50 characters.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Email must be a valid email address.");

            RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("Created By is required.");
        }
    }

    public class UpdateEmployeeEmergencyContactRequestValidator : AbstractValidator<UpdateEmployeeEmergencyContactRequest>
    {
        public UpdateEmployeeEmergencyContactRequestValidator()
        {
            RuleFor(x => x.EmployeeEmergencyContactId).GreaterThan(0).WithMessage("Emergency Contact ID is required.");
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.ContactName)
                .NotEmpty().WithMessage("Contact Name is required.")
                .MaximumLength(150).WithMessage("Contact Name cannot exceed 150 characters.");

            RuleFor(x => x.Relationship)
                .NotEmpty().WithMessage("Relationship is required.")
                .MaximumLength(50).WithMessage("Relationship cannot exceed 50 characters.");

            RuleFor(x => x.MobileNumber)
                .NotEmpty().WithMessage("Mobile Number is required.")
                .MaximumLength(50).WithMessage("Mobile Number cannot exceed 50 characters.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Email must be a valid email address.");

            RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage("Modified By is required.");
        }
    }

    // --- Qualification ---
    public class CreateEmployeeQualificationRequestValidator : AbstractValidator<CreateEmployeeQualificationRequest>
    {
        public CreateEmployeeQualificationRequestValidator()
        {
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.QualificationType)
                .NotEmpty().WithMessage("Qualification Type is required.")
                .MaximumLength(100).WithMessage("Qualification Type cannot exceed 100 characters.");

            RuleFor(x => x.Institution)
                .NotEmpty().WithMessage("Institution is required.")
                .MaximumLength(200).WithMessage("Institution cannot exceed 200 characters.");

            RuleFor(x => x.YearOfPassing)
                .InclusiveBetween(1900, DateTime.UtcNow.Year + 5).WithMessage("Invalid Year of Passing.");

            RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("Created By is required.");
        }
    }

    public class UpdateEmployeeQualificationRequestValidator : AbstractValidator<UpdateEmployeeQualificationRequest>
    {
        public UpdateEmployeeQualificationRequestValidator()
        {
            RuleFor(x => x.EmployeeQualificationId).GreaterThan(0).WithMessage("Qualification ID is required.");
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.QualificationType)
                .NotEmpty().WithMessage("Qualification Type is required.")
                .MaximumLength(100).WithMessage("Qualification Type cannot exceed 100 characters.");

            RuleFor(x => x.Institution)
                .NotEmpty().WithMessage("Institution is required.")
                .MaximumLength(200).WithMessage("Institution cannot exceed 200 characters.");

            RuleFor(x => x.YearOfPassing)
                .InclusiveBetween(1900, DateTime.UtcNow.Year + 5).WithMessage("Invalid Year of Passing.");

            RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage("Modified By is required.");
        }
    }

    // --- Certification ---
    public class CreateEmployeeCertificationRequestValidator : AbstractValidator<CreateEmployeeCertificationRequest>
    {
        public CreateEmployeeCertificationRequestValidator()
        {
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.CertificationName)
                .NotEmpty().WithMessage("Certification Name is required.")
                .MaximumLength(150).WithMessage("Certification Name cannot exceed 150 characters.");

            RuleFor(x => x.CertificationAuthority)
                .NotEmpty().WithMessage("Certification Authority is required.")
                .MaximumLength(150).WithMessage("Certification Authority cannot exceed 150 characters.");

            RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("Created By is required.");
        }
    }

    public class UpdateEmployeeCertificationRequestValidator : AbstractValidator<UpdateEmployeeCertificationRequest>
    {
        public UpdateEmployeeCertificationRequestValidator()
        {
            RuleFor(x => x.EmployeeCertificationId).GreaterThan(0).WithMessage("Certification ID is required.");
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.CertificationName)
                .NotEmpty().WithMessage("Certification Name is required.")
                .MaximumLength(150).WithMessage("Certification Name cannot exceed 150 characters.");

            RuleFor(x => x.CertificationAuthority)
                .NotEmpty().WithMessage("Certification Authority is required.")
                .MaximumLength(150).WithMessage("Certification Authority cannot exceed 150 characters.");

            RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage("Modified By is required.");
        }
    }

    // --- Document ---
    public class UploadEmployeeDocumentRequestValidator : AbstractValidator<UploadEmployeeDocumentRequest>
    {
        public UploadEmployeeDocumentRequestValidator()
        {
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.DocumentType)
                .NotEmpty().WithMessage("Document Type is required.")
                .MaximumLength(100).WithMessage("Document Type cannot exceed 100 characters.");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("File Name is required.")
                .MaximumLength(250).WithMessage("File Name cannot exceed 250 characters.");

            RuleFor(x => x.FilePath)
                .NotEmpty().WithMessage("File Path is required.");

            RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("Created By is required.");
        }
    }

    public class UpdateEmployeeDocumentRequestValidator : AbstractValidator<UpdateEmployeeDocumentRequest>
    {
        public UpdateEmployeeDocumentRequestValidator()
        {
            RuleFor(x => x.EmployeeDocumentId).GreaterThan(0).WithMessage("Document ID is required.");
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("File Name is required.")
                .MaximumLength(250).WithMessage("File Name cannot exceed 250 characters.");

            RuleFor(x => x.FilePath)
                .NotEmpty().WithMessage("File Path is required.");

            RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage("Modified By is required.");
        }
    }

    // --- Manager ---
    public class AssignManagerRequestValidator : AbstractValidator<AssignManagerRequest>
    {
        public AssignManagerRequestValidator()
        {
            RuleFor(x => x.TenantId).GreaterThan(0).WithMessage("Tenant ID is required.");
            RuleFor(x => x.EmployeeId).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.ManagerId).GreaterThan(0).WithMessage("Manager ID is required.");
            RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("Created By is required.");
            RuleFor(x => x.EffectiveFrom).NotEmpty().WithMessage("Effective From Date is required.");
        }
    }
}
