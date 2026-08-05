using System;

namespace HRMS.Domain.Entities
{
    public class AssetRepair
    {
        public long AssetRepairID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public DateTime RepairDate { get; set; }
        public string? RepairReason { get; set; }
        public decimal RepairCost { get; set; }
        public string RepairStatus { get; set; } = "Pending";

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
