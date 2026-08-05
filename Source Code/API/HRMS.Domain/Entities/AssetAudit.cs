using System;

namespace HRMS.Domain.Entities
{
    public class AssetAudit
    {
        public long AssetAuditID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public DateTime AuditDate { get; set; }
        public long AuditorID { get; set; }
        public string AuditStatus { get; set; } = "Scheduled";
        public string? Findings { get; set; }

        // Audit Columns
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}
