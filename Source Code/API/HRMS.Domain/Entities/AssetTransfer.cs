using System;

namespace HRMS.Domain.Entities
{
    public class AssetTransfer
    {
        public long AssetTransferID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public long? FromEmployeeID { get; set; }
        public long ToEmployeeID { get; set; }
        public DateTime TransferDate { get; set; }
        public string? TransferReason { get; set; }
        public string TransferStatus { get; set; } = "Pending";

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
