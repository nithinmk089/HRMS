using System;

namespace HRMS.Domain.Entities
{
    public class BusinessUnit
    {
        public long BusinessUnitId { get; set; }
        public long TenantId { get; set; }
        public long CompanyId { get; set; }
        public string BusinessUnitCode { get; set; } = string.Empty;
        public string BusinessUnitName { get; set; } = string.Empty;

        // Audit Columns
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public long VersionNo { get; set; }
    }
}
