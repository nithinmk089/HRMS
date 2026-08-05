using System;

namespace HRMS.Domain.Entities
{
    public class Course
    {
        public long CourseID { get; set; }
        public long TenantID { get; set; }
        public string CourseCode { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public long CourseCategoryID { get; set; }
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public string CourseStatus { get; set; } = "Draft";
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