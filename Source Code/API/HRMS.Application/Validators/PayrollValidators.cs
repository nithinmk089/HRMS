using FluentValidation;
using HRMS.Application.DTOs;

namespace HRMS.Application.Validators
{
    public class CreatePayrollCalendarRequestValidator : AbstractValidator<CreatePayrollCalendarRequest>
    {
        public CreatePayrollCalendarRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.CalendarCode).NotEmpty().WithMessage("Calendar Code is required.");
            RuleFor(x => x.CalendarName).NotEmpty().WithMessage("Calendar Name is required.");
            RuleFor(x => x.FinancialYear).NotEmpty().WithMessage("Financial Year is required.");
        }
    }

    public class CreatePayrollPeriodRequestValidator : AbstractValidator<CreatePayrollPeriodRequest>
    {
        public CreatePayrollPeriodRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.PayrollCalendarID).GreaterThan(0).WithMessage("Payroll Calendar ID is required.");
            RuleFor(x => x.PeriodCode).NotEmpty().WithMessage("Period Code is required.");
            RuleFor(x => x.PeriodStartDate).NotEmpty().WithMessage("Period Start Date is required.");
            RuleFor(x => x.PeriodEndDate).NotEmpty().WithMessage("Period End Date is required.");
        }
    }

    public class CreateSalaryStructureRequestValidator : AbstractValidator<CreateSalaryStructureRequest>
    {
        public CreateSalaryStructureRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.StructureCode).NotEmpty().WithMessage("Structure Code is required.");
            RuleFor(x => x.StructureName).NotEmpty().WithMessage("Structure Name is required.");
            RuleFor(x => x.EffectiveFrom).NotEmpty().WithMessage("Effective From date is required.");
        }
    }

    public class CreateEmployeeCompensationRequestValidator : AbstractValidator<CreateEmployeeCompensationRequest>
    {
        public CreateEmployeeCompensationRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.EmployeeID).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.SalaryStructureID).GreaterThan(0).WithMessage("Salary Structure ID is required.");
            RuleFor(x => x.GrossSalary).GreaterThan(0).WithMessage("Gross Salary must be greater than 0.");
            RuleFor(x => x.EffectiveFrom).NotEmpty().WithMessage("Effective From date is required.");
        }
    }

    public class CreatePayrollRunRequestValidator : AbstractValidator<CreatePayrollRunRequest>
    {
        public CreatePayrollRunRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.PayrollPeriodID).GreaterThan(0).WithMessage("Payroll Period ID is required.");
        }
    }

    public class CreatePayrollAdjustmentRequestValidator : AbstractValidator<CreatePayrollAdjustmentRequest>
    {
        public CreatePayrollAdjustmentRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.EmployeeID).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.PayrollPeriodID).GreaterThan(0).WithMessage("Payroll Period ID is required.");
            RuleFor(x => x.AdjustmentAmount).NotEqual(0).WithMessage("Adjustment Amount cannot be 0.");
            RuleFor(x => x.AdjustmentType).NotEmpty().WithMessage("Adjustment Type is required.");
        }
    }

    public class CreateLoanRequestValidator : AbstractValidator<CreateLoanRequest>
    {
        public CreateLoanRequestValidator()
        {
            RuleFor(x => x.TenantID).GreaterThan(0).WithMessage("Tenant ID must be greater than 0.");
            RuleFor(x => x.EmployeeID).GreaterThan(0).WithMessage("Employee ID is required.");
            RuleFor(x => x.LoanType).NotEmpty().WithMessage("Loan Type is required.");
            RuleFor(x => x.PrincipalAmount).GreaterThan(0).WithMessage("Principal Amount must be greater than 0.");
            RuleFor(x => x.TenureMonths).GreaterThan(0).WithMessage("Tenure Months must be greater than 0.");
        }
    }
}
