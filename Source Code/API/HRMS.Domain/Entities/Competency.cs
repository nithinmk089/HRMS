using System;

namespace HRMS.Domain.Entities
{
    public class Competency
    {
        public long CompetencyID { get; set; }
        public long TenantID { get; set; }
        public long CompetencyFrameworkID { get; set; }
        public string CompetencyName { get; set; } = string.Empty;
        public string CompetencyLevel { get; set; } = string.Empty;

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
