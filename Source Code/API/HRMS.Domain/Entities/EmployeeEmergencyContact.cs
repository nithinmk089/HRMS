using System;

namespace HRMS.Domain.Entities
{
    public class EmployeeEmergencyContact
    {
        public long EmployeeEmergencyContactID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string ContactName { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty; // Spouse, Parent, Child, Relative, Friend
        public string MobileNumber { get; set; } = string.Empty;
        public string? Email { get; set; }

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