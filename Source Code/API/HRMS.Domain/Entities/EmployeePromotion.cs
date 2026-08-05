using System;

namespace HRMS.Domain.Entities
{
    public class EmployeePromotion
    {
        public long EmployeePromotionID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long OldDesignationID { get; set; }
        public long NewDesignationID { get; set; }
        public string? OldGrade { get; set; }
        public string? NewGrade { get; set; }
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