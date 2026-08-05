using System;

namespace HRMS.Domain.Entities
{
    public class Payslip
    {
        public long PayslipID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long PayrollPeriodID { get; set; }
        public string PayslipNumber { get; set; } = string.Empty;
        public DateTime GeneratedDate { get; set; }

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
