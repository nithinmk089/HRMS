using System;

namespace HRMS.Domain.Entities
{
    public class SelfAssessment
    {
        public long SelfAssessmentID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long PerformanceCycleID { get; set; }
        public string AssessmentStatus { get; set; } = "Pending";

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
