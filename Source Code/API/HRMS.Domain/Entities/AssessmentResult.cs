using System;

namespace HRMS.Domain.Entities
{
    public class AssessmentResult
    {
        public long AssessmentResultID { get; set; }
        public long TenantID { get; set; }
        public long AssessmentID { get; set; }
        public long EmployeeID { get; set; }
        public decimal Score { get; set; }
        public string ResultStatus { get; set; } = "Pending";
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