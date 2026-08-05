using System;

namespace HRMS.Domain.Entities
{
    public class LearningAnalytics
    {
        public long LearningAnalyticsID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public int CompletedCourses { get; set; }
        public int ActiveCourses { get; set; }
        public int CertificationsEarned { get; set; }
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