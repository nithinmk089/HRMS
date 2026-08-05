using System;

namespace HRMS.Domain.Entities
{
    public class Employee
    {
        public long EmployeeID { get; set; }
        public long TenantID { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? PreferredName { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Nationality { get; set; }
        public string? PersonalEmail { get; set; }
        public string? MobileNumber { get; set; }
        public string Status { get; set; } = "Active";

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