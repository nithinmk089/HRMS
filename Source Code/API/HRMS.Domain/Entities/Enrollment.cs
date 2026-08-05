using System;

namespace HRMS.Domain.Entities
{
    public class Enrollment
    {
        public long EnrollmentID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long CourseID { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string EnrollmentStatus { get; set; } = "Active";
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