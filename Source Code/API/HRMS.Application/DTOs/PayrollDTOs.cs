using System;

namespace HRMS.Application.DTOs
{
    // CALENDARS
    public class PayrollCalendarDto
    {
        public long PayrollCalendarID { get; set; }
        public long TenantID { get; set; }
        public string CalendarCode { get; set; } = string.Empty;
        public string CalendarName { get; set; } = string.Empty;
        public string FinancialYear { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class CreatePayrollCalendarRequest
    {
        public long TenantID { get; set; }
        public string CalendarCode { get; set; } = string.Empty;
        public string CalendarName { get; set; } = string.Empty;
        public string FinancialYear { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    public class UpdatePayrollCalendarRequest
    {
        public long PayrollCalendarID { get; set; }
        public long TenantID { get; set; }
        public string CalendarName { get; set; } = string.Empty;
        public long ModifiedBy { get; set; }
    }

    // PERIODS
    public class PayrollPeriodDto
    {
        public long PayrollPeriodID { get; set; }
        public long TenantID { get; set; }
        public long PayrollCalendarID { get; set; }
        public string PeriodCode { get; set; } = string.Empty;
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public string ProcessingStatus { get; set; } = string.Empty;
    }

    public class CreatePayrollPeriodRequest
    {
        public long TenantID { get; set; }
        public long PayrollCalendarID { get; set; }
        public string PeriodCode { get; set; } = string.Empty;
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public long CreatedBy { get; set; }
    }

    // SALARY STRUCTURES
    public class SalaryStructureDto
    {
        public long SalaryStructureID { get; set; }
        public long TenantID { get; set; }
        public string StructureCode { get; set; } = string.Empty;
        public string StructureName { get; set; } = string.Empty;
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }

    public class CreateSalaryStructureRequest
    {
        public long TenantID { get; set; }
        public string StructureCode { get; set; } = string.Empty;
        public string StructureName { get; set; } = string.Empty;
        public DateTime EffectiveFrom { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateSalaryStructureRequest
    {
        public long SalaryStructureID { get; set; }
        public long TenantID { get; set; }
        public string StructureName { get; set; } = string.Empty;
        public DateTime? EffectiveTo { get; set; }
        public long ModifiedBy { get; set; }
    }

    // COMPONENTS
    public class SalaryComponentDto
    {
        public long SalaryComponentID { get; set; }
        public long TenantID { get; set; }
        public string ComponentCode { get; set; } = string.Empty;
        public string ComponentName { get; set; } = string.Empty;
        public string ComponentType { get; set; } = string.Empty;
        public string CalculationMethod { get; set; } = string.Empty;
        public bool TaxableFlag { get; set; }
    }

    public class CreateSalaryComponentRequest
    {
        public long TenantID { get; set; }
        public string ComponentCode { get; set; } = string.Empty;
        public string ComponentName { get; set; } = string.Empty;
        public string ComponentType { get; set; } = string.Empty;
        public string CalculationMethod { get; set; } = string.Empty;
        public bool TaxableFlag { get; set; } = true;
        public long CreatedBy { get; set; }
    }

    // EMPLOYEE COMPENSATION
    public class EmployeeCompensationDto
    {
        public long EmployeeCompensationID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public long SalaryStructureID { get; set; }
        public string? StructureName { get; set; }
        public decimal GrossSalary { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }

    public class CreateEmployeeCompensationRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long SalaryStructureID { get; set; }
        public decimal GrossSalary { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public long CreatedBy { get; set; }
    }

    // RUNS
    public class PayrollRunDto
    {
        public long PayrollRunID { get; set; }
        public long TenantID { get; set; }
        public long PayrollPeriodID { get; set; }
        public string? PeriodCode { get; set; }
        public DateTime RunDate { get; set; }
        public string RunStatus { get; set; } = string.Empty;
        public int TotalEmployees { get; set; }
        public decimal TotalGrossPay { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TotalNetPay { get; set; }
    }

    public class CreatePayrollRunRequest
    {
        public long TenantID { get; set; }
        public long PayrollPeriodID { get; set; }
        public long CreatedBy { get; set; }
    }

    // TRANSACTIONS
    public class PayrollTransactionDto
    {
        public long PayrollTransactionID { get; set; }
        public long TenantID { get; set; }
        public long PayrollRunID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public decimal GrossPay { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetPay { get; set; }
    }

    // ADJUSTMENTS
    public class PayrollAdjustmentDto
    {
        public long PayrollAdjustmentID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public long PayrollPeriodID { get; set; }
        public string? PeriodCode { get; set; }
        public string AdjustmentType { get; set; } = string.Empty;
        public decimal AdjustmentAmount { get; set; }
        public string? Remarks { get; set; }
        public string AdjustmentStatus { get; set; } = string.Empty;
    }

    public class CreatePayrollAdjustmentRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long PayrollPeriodID { get; set; }
        public string AdjustmentType { get; set; } = string.Empty;
        public decimal AdjustmentAmount { get; set; }
        public string? Remarks { get; set; }
        public long CreatedBy { get; set; }
    }

    // ARREARS
    public class ArrearProcessingDto
    {
        public long ArrearProcessingID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public decimal ArrearAmount { get; set; }
        public DateTime EffectiveMonth { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreateArrearRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public decimal ArrearAmount { get; set; }
        public DateTime EffectiveMonth { get; set; }
        public long CreatedBy { get; set; }
    }

    // LOANS
    public class LoanAdvanceDto
    {
        public long LoanAdvanceID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public string LoanType { get; set; } = string.Empty;
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int TenureMonths { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreateLoanRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string LoanType { get; set; } = string.Empty;
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int TenureMonths { get; set; }
        public long CreatedBy { get; set; }
    }

    public class LoanRepaymentDto
    {
        public long LoanRepaymentID { get; set; }
        public long TenantID { get; set; }
        public long LoanAdvanceID { get; set; }
        public int InstallmentNo { get; set; }
        public decimal RepaymentAmount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // BONUS
    public class BonusDto
    {
        public long BonusID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public string BonusType { get; set; } = string.Empty;
        public decimal BonusAmount { get; set; }
        public string BonusPeriod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class CreateBonusRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string BonusType { get; set; } = string.Empty;
        public decimal BonusAmount { get; set; }
        public string BonusPeriod { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    // INCENTIVE
    public class IncentiveDto
    {
        public long IncentiveID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public string IncentiveType { get; set; } = string.Empty;
        public decimal IncentiveAmount { get; set; }
        public string IncentivePeriod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class CreateIncentiveRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string IncentiveType { get; set; } = string.Empty;
        public decimal IncentiveAmount { get; set; }
        public string IncentivePeriod { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    // PAYSLIP
    public class PayslipDto
    {
        public long PayslipID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public long PayrollPeriodID { get; set; }
        public string PeriodCode { get; set; } = string.Empty;
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public string PayslipNumber { get; set; } = string.Empty;
        public DateTime GeneratedDate { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetPay { get; set; }
    }
}
