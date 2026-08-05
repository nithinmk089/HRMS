using System;

namespace HRMS.Domain.Entities
{
    public class PayrollPeriod
    {
        public long PayrollPeriodID { get; set; }
        public long TenantID { get; set; }
        public long PayrollCalendarID { get; set; }
        public string PeriodCode { get; set; } = string.Empty;
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public string ProcessingStatus { get; set; } = "Open";

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
