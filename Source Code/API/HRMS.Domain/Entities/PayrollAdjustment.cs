using System;

namespace HRMS.Domain.Entities
{
    public class PayrollAdjustment
    {
        public long PayrollAdjustmentID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long PayrollPeriodID { get; set; }
        public string AdjustmentType { get; set; } = string.Empty;
        public decimal AdjustmentAmount { get; set; }
        public string? Remarks { get; set; }
        public string AdjustmentStatus { get; set; } = "Pending";

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
