using System;

namespace HRMS.Domain.Entities
{
    public class Incentive
    {
        public long IncentiveID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string IncentiveType { get; set; } = string.Empty;
        public decimal IncentiveAmount { get; set; }
        public string IncentivePeriod { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";

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
