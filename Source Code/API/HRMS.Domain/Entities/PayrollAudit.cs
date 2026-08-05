using System;

namespace HRMS.Domain.Entities
{
    public class PayrollAudit
    {
        public long PayrollAuditID { get; set; }
        public long TenantID { get; set; }
        public long PayrollRunID { get; set; }
        public DateTime AuditDate { get; set; }
        public string? AuditRemarks { get; set; }

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
