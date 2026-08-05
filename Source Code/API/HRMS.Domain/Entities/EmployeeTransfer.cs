using System;

namespace HRMS.Domain.Entities
{
    public class EmployeeTransfer
    {
        public long EmployeeTransferID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long FromDepartmentID { get; set; }
        public long ToDepartmentID { get; set; }
        public long FromLocationID { get; set; }
        public long ToLocationID { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = "Pending";

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