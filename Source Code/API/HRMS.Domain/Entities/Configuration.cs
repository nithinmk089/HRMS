using System;

namespace HRMS.Domain.Entities
{
    public class Configuration
    {
        public long ConfigurationId { get; set; }
        public long TenantId { get; set; }
        public string ConfigurationKey { get; set; } = string.Empty;
        public string? ConfigurationValue { get; set; }
        public string DataType { get; set; } = string.Empty;

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
