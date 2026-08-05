using System;

namespace HRMS.Domain.Entities
{
    public class Location
    {
        public long LocationId { get; set; }
        public long TenantId { get; set; }
        public string LocationCode { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public string? CountryCode { get; set; }
        public string? StateCode { get; set; }
        public string? City { get; set; }

        // Audit Columns
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public long VersionNo { get; set; }
    }
}
