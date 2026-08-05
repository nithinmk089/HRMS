using System;

namespace HRMS.Domain.Entities
{
    public class PayrollCalendar
    {
        public long PayrollCalendarID { get; set; }
        public long TenantID { get; set; }
        public string CalendarCode { get; set; } = string.Empty;
        public string CalendarName { get; set; } = string.Empty;
        public string FinancialYear { get; set; } = string.Empty;
        public bool IsActive { get; set; }

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
