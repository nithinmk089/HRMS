namespace HRMS.Application.DTOs
{
    public class CostCenterDto
    {
        public long CostCenterId { get; set; }
        public long TenantId { get; set; }
        public string CostCenterCode { get; set; } = string.Empty;
        public string CostCenterName { get; set; } = string.Empty;
        public long VersionNo { get; set; }
    }

    public class CreateCostCenterRequest
    {
        public long TenantId { get; set; }
        public string CostCenterCode { get; set; } = string.Empty;
        public string CostCenterName { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    public class UpdateCostCenterRequest
    {
        public long CostCenterId { get; set; }
        public long TenantId { get; set; }
        public string CostCenterName { get; set; } = string.Empty;
        public long ModifiedBy { get; set; }
    }
}
