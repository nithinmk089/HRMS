using System;

namespace HRMS.Domain.Entities
{
    public class AssetAssignment
    {
        public long AssetAssignmentID { get; set; }
        public long TenantID { get; set; }
        public long AssetID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public string AssignmentStatus { get; set; } = "Active";

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
