using System;

namespace HRMS.Domain.Entities
{
    public class AssetDepreciation
    {
        public long AssetDepreciationID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public string DepreciationMethod { get; set; } = string.Empty;
        public decimal DepreciationRate { get; set; }
        public decimal BookValue { get; set; }

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
