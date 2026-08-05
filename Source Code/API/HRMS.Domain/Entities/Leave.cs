using System;

namespace HRMS.Domain.Entities
{
    public class LeaveType
    {
        public long LeaveTypeID { get; set; }
        public long TenantID { get; set; }
        public string LeaveCode { get; set; } = null!;
        public string LeaveName { get; set; } = null!;
        public bool IsPaid { get; set; }
        public bool IsAccrualBased { get; set; }

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

    public class LeavePolicy
    {
        public long LeavePolicyID { get; set; }
        public long TenantID { get; set; }
        public string PolicyName { get; set; } = null!;
        public long LeaveTypeID { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

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

    public class LeaveAccrualRule
    {
        public long LeaveAccrualRuleID { get; set; }
        public long TenantID { get; set; }
        public long LeavePolicyID { get; set; }
        public string AccrualFrequency { get; set; } = null!;
        public decimal AccrualAmount { get; set; }

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

    public class LeaveBalance
    {
        public long LeaveBalanceID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long LeaveTypeID { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal AccruedBalance { get; set; }
        public decimal ConsumedBalance { get; set; }
        public decimal AvailableBalance { get; set; }

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

    public class LeaveRequest
    {
        public long LeaveRequestID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long LeaveTypeID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalDays { get; set; }
        public string Reason { get; set; } = null!;
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

    public class LeaveApproval
    {
        public long LeaveApprovalID { get; set; }
        public long TenantID { get; set; }
        public long LeaveRequestID { get; set; }
        public long ApproverID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string? ApprovalRemarks { get; set; }

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

    public class LeaveEncashment
    {
        public long LeaveEncashmentID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long LeaveTypeID { get; set; }
        public decimal EncashedDays { get; set; }
        public decimal Amount { get; set; }
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
