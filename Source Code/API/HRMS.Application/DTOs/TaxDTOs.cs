using System;

namespace HRMS.Application.DTOs
{
    // TAX REGIMES
    public class TaxRegimeDto
    {
        public long TaxRegimeID { get; set; }
        public long TenantID { get; set; }
        public string RegimeName { get; set; } = string.Empty;
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }

    public class CreateTaxRegimeRequest
    {
        public long TenantID { get; set; }
        public string RegimeName { get; set; } = string.Empty;
        public DateTime EffectiveFrom { get; set; }
        public long CreatedBy { get; set; }
    }

    // TAX SLABS
    public class TaxSlabDto
    {
        public long TaxSlabID { get; set; }
        public long TenantID { get; set; }
        public long TaxRegimeID { get; set; }
        public decimal IncomeFrom { get; set; }
        public decimal? IncomeTo { get; set; }
        public decimal TaxRate { get; set; }
    }

    public class CreateTaxSlabRequest
    {
        public long TenantID { get; set; }
        public long TaxRegimeID { get; set; }
        public decimal IncomeFrom { get; set; }
        public decimal? IncomeTo { get; set; }
        public decimal TaxRate { get; set; }
        public long CreatedBy { get; set; }
    }

    // TAX DECLARATIONS
    public class EmployeeTaxDeclarationDto
    {
        public long EmployeeTaxDeclarationID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public string FinancialYear { get; set; } = string.Empty;
        public decimal DeclaredAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CreateTaxDeclarationRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string FinancialYear { get; set; } = string.Empty;
        public decimal DeclaredAmount { get; set; }
        public long CreatedBy { get; set; }
    }

    // TAX COMPUTATIONS
    public class EmployeeTaxComputationDto
    {
        public long EmployeeTaxComputationID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string FinancialYear { get; set; } = string.Empty;
        public decimal TaxableIncome { get; set; }
        public decimal TaxAmount { get; set; }
    }

    // STATUTORY DEDUCTIONS
    public class StatutoryDeductionDto
    {
        public long StatutoryDeductionID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string DeductionType { get; set; } = string.Empty;
        public decimal DeductionAmount { get; set; }
    }
}
