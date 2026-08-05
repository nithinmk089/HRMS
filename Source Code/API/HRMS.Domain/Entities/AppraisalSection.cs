using System;

namespace HRMS.Domain.Entities
{
    public class AppraisalSection
    {
        public long AppraisalSectionID { get; set; }
        public long TenantID { get; set; }
        public long AppraisalTemplateID { get; set; }
        public string SectionName { get; set; } = string.Empty;
        public decimal Weightage { get; set; }
        public int SequenceNo { get; set; }

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
