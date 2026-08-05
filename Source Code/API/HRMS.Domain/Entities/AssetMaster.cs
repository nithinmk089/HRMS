using System;

namespace HRMS.Domain.Entities
{
    public class AssetMaster
    {
        public long AssetID { get; set; }
        public long TenantID { get; set; }
        public string AssetCode { get; set; } = string.Empty;
        public string AssetTag { get; set; } = string.Empty;
        public string AssetName { get; set; } = string.Empty;
        public long AssetCategoryID { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public decimal PurchaseCost { get; set; }
        public decimal CurrentBookValue { get; set; }
        public string Status { get; set; } = "Available";

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
