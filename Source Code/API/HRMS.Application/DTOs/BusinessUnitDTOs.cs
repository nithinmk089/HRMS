namespace HRMS.Application.DTOs
{
    public class BusinessUnitDto
    {
        public long BusinessUnitId { get; set; }
        public long TenantId { get; set; }
        public long CompanyId { get; set; }
        public string BusinessUnitCode { get; set; } = string.Empty;
        public string BusinessUnitName { get; set; } = string.Empty;
        public long VersionNo { get; set; }
    }

    public class CreateBusinessUnitRequest
    {
        public long TenantId { get; set; }
        public long CompanyId { get; set; }
        public string BusinessUnitCode { get; set; } = string.Empty;
        public string BusinessUnitName { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    public class UpdateBusinessUnitRequest
    {
        public long BusinessUnitId { get; set; }
        public long TenantId { get; set; }
        public string BusinessUnitName { get; set; } = string.Empty;
        public long ModifiedBy { get; set; }
    }
}
