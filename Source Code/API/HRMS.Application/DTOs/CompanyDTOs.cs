namespace HRMS.Application.DTOs
{
    public class CompanyDto
    {
        public long CompanyId { get; set; }
        public long TenantId { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string? LegalName { get; set; }
        public string? TaxNumber { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public long VersionNo { get; set; }
    }

    public class CreateCompanyRequest
    {
        public long TenantId { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string? LegalName { get; set; }
        public string? TaxNumber { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateCompanyRequest
    {
        public long CompanyId { get; set; }
        public long TenantId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string? LegalName { get; set; }
        public string? TaxNumber { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public long ModifiedBy { get; set; }
    }
}
