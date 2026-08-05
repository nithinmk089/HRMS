using System;

namespace HRMS.Domain.Entities
{
    public class CourseContent
    {
        public long CourseContentID { get; set; }
        public long TenantID { get; set; }
        public long CourseID { get; set; }
        public string ContentTitle { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string? ContentUrl { get; set; }
        public int SequenceNo { get; set; }
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