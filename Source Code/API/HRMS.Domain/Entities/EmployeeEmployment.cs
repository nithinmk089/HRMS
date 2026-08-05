using System;

namespace HRMS.Domain.Entities
{
    public class EmployeeEmployment
    {
        public long EmployeeEmploymentID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long CompanyID { get; set; }
        public long BusinessUnitID { get; set; }
        public long DepartmentID { get; set; }
        public long DesignationID { get; set; }
        public long LocationID { get; set; }
        public long CostCenterID { get; set; }
        public string EmploymentType { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? ProbationEndDate { get; set; }
        public int NoticePeriodDays { get; set; }
        public string EmploymentStatus { get; set; } = "Active";

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