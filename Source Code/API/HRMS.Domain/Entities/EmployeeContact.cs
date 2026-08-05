using System;

namespace HRMS.Domain.Entities
{
    public class EmployeeContact
    {
        public long EmployeeContactID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string ContactType { get; set; } = string.Empty; // Mobile, Alternate Mobile, Work Phone, Personal Email, Work Email
        public string ContactValue { get; set; } = string.Empty;

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