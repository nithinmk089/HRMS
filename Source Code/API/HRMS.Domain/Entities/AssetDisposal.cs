using System;

namespace HRMS.Domain.Entities
{
    public class AssetDisposal
    {
        public long AssetDisposalID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public DateTime DisposalDate { get; set; }
        public string DisposalMethod { get; set; } = string.Empty;
        public decimal DisposalValue { get; set; }
        public string DisposalStatus { get; set; } = "Pending";

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
