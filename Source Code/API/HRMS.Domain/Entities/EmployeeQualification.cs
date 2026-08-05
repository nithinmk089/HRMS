using System;

namespace HRMS.Domain.Entities
{
    public class EmployeeQualification
    {
        public long EmployeeQualificationID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string QualificationType { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public string? University { get; set; }
        public int YearOfPassing { get; set; }
        public decimal? Percentage { get; set; }

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