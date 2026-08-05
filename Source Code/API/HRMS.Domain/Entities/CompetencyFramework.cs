using System;

namespace HRMS.Domain.Entities
{
    public class CompetencyFramework
    {
        public long CompetencyFrameworkID { get; set; }
        public long TenantID { get; set; }
        public string FrameworkName { get; set; } = string.Empty;
        public string? Description { get; set; }

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
