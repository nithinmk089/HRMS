using System;

namespace HRMS.Domain.Entities
{
    public class PayrollRun
    {
        public long PayrollRunID { get; set; }
        public long TenantID { get; set; }
        public long PayrollPeriodID { get; set; }
        public DateTime RunDate { get; set; }
        public string RunStatus { get; set; } = "Pending";

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
