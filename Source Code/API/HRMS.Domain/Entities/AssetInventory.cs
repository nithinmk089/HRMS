using System;

namespace HRMS.Domain.Entities
{
    public class AssetInventory
    {
        public long AssetInventoryID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public long LocationID { get; set; }
        public int Quantity { get; set; }
        public string InventoryStatus { get; set; } = "InStock";

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
