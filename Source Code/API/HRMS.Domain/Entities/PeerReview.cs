using System;

namespace HRMS.Domain.Entities
{
    public class PeerReview
    {
        public long PeerReviewID { get; set; }
        public long TenantID { get; set; }
        public long ReviewerID { get; set; }
        public long EmployeeID { get; set; }
        public string ReviewText { get; set; } = string.Empty;

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
