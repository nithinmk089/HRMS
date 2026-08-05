using System;

namespace HRMS.Domain.Entities
{
    public class LoanRepayment
    {
        public long LoanRepaymentID { get; set; }
        public long TenantID { get; set; }
        public long LoanAdvanceID { get; set; }
        public int InstallmentNo { get; set; }
        public decimal RepaymentAmount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Status { get; set; } = "Unpaid";

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
