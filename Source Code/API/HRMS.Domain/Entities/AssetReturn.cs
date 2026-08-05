using System;

namespace HRMS.Domain.Entities
{
    public class AssetReturnMaster
    {
        public long AssetReturnID { get; set; }
        public long TenantID { get; set; }
        public long AssetAssignmentID { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? ReturnCondition { get; set; }
        public string ReturnStatus { get; set; } = "Pending";

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
