using System;

namespace HRMS.Domain.Entities
{
    public class ComplianceTraining
    {
        public long ComplianceTrainingID { get; set; }
        public long TenantID { get; set; }
        public long CourseID { get; set; }
        public string ComplianceType { get; set; } = string.Empty;
        public bool MandatoryFlag { get; set; }
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