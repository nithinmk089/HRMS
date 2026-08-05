using System;

namespace HRMS.Application.DTOs
{
    public class TenantDto
    {
        public long TenantId { get; set; }
        public string TenantCode { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public long VersionNo { get; set; }
    }

    public class CreateTenantRequest
    {
        public string TenantCode { get; set; } = string.Empty;
        public string TenantName { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateTenantRequest
    {
        public long TenantId { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public long ModifiedBy { get; set; }
    }
}
