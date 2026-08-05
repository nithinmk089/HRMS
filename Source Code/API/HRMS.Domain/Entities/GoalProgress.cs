using System;

namespace HRMS.Domain.Entities
{
    public class GoalProgress
    {
        public long GoalProgressID { get; set; }
        public long TenantID { get; set; }
        public long GoalID { get; set; }
        public DateTime ProgressDate { get; set; }
        public decimal ProgressPercentage { get; set; }
        public string? Remarks { get; set; }

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
