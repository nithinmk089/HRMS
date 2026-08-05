using System;

namespace HRMS.Domain.Entities
{
    public class AssetMaintenance
    {
        public long AssetMaintenanceID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceType { get; set; } = string.Empty;
        public string? VendorName { get; set; }
        public decimal Cost { get; set; }
        public string MaintenanceStatus { get; set; } = "Scheduled";

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
