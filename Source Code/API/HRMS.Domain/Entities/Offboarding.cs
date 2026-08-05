using System;

namespace HRMS.Domain.Entities
{
    public class ExitRequest
    {
        public long ExitRequestID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime ResignationDate { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public string ExitReason { get; set; } = null!;
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

    public class ExitApproval
    {
        public long ExitApprovalID { get; set; }
        public long TenantID { get; set; }
        public long ExitRequestID { get; set; }
        public long ApproverID { get; set; }
        public string ApprovalStatus { get; set; } = null!;
        public DateTime ApprovalDate { get; set; }
        public string? Remarks { get; set; }

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

    public class ClearanceRequest
    {
        public long ClearanceRequestID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string ClearanceStatus { get; set; } = "Pending";
        public DateTime InitiatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }

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

    public class ClearanceTask
    {
        public long ClearanceTaskID { get; set; }
        public long TenantID { get; set; }
        public long ClearanceRequestID { get; set; }
        public long DepartmentID { get; set; }
        public long AssignedTo { get; set; }
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

    public class AssetReturn
    {
        public long AssetReturnID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long AssetID { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? ReturnCondition { get; set; }

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

    public class KnowledgeTransfer
    {
        public long KnowledgeTransferID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long? SuccessorEmployeeID { get; set; }
        public DateTime? KTDate { get; set; }
        public string KTStatus { get; set; } = "Pending";

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

    public class ExperienceLetterRequest
    {
        public long ExperienceLetterRequestID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? GeneratedDate { get; set; }
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

    public class FullAndFinalSettlement
    {
        public long FullAndFinalSettlementID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public decimal SettlementAmount { get; set; }
        public DateTime? SettlementDate { get; set; }
        public string SettlementStatus { get; set; } = "Pending";

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
