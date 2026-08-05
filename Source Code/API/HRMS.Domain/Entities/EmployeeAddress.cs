using System;

namespace HRMS.Domain.Entities
{
    public class EmployeeAddress
    {
        public long EmployeeAddressID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string AddressType { get; set; } = string.Empty; // Permanent, Current, Emergency
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string? State { get; set; }
        public string Country { get; set; } = string.Empty;
        public string? ZipCode { get; set; }

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