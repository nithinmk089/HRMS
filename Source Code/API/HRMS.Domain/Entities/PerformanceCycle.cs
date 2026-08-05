using System;

namespace HRMS.Domain.Entities
{
    public class PerformanceCycle
    {
        public long PerformanceCycleID { get; set; }
        public long TenantID { get; set; }
        public string CycleCode { get; set; } = string.Empty;
        public string CycleName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CycleStatus { get; set; } = "Draft";

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
