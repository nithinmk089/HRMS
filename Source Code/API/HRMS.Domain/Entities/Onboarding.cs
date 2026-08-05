using System;

namespace HRMS.Domain.Entities
{
    public class OnboardingWorkflow
    {
        public long OnboardingWorkflowID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string WorkflowCode { get; set; } = null!;
        public string WorkflowName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime TargetCompletionDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string WorkflowStatus { get; set; } = "Pending";

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

    public class OnboardingTask
    {
        public long OnboardingTaskID { get; set; }
        public long TenantID { get; set; }
        public string TaskCode { get; set; } = null!;
        public string TaskName { get; set; } = null!;
        public string TaskType { get; set; } = null!;
        public int DueDays { get; set; }
        public int SequenceNo { get; set; }
        public bool IsMandatory { get; set; }

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

    public class OnboardingTaskAssignment
    {
        public long OnboardingTaskAssignmentID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long OnboardingTaskID { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string TaskStatus { get; set; } = "Pending";

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

    public class EmployeeDocumentSubmission
    {
        public long EmployeeDocumentSubmissionID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string DocumentType { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public DateTime UploadedDate { get; set; }

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

    public class EmployeeDocumentVerification
    {
        public long EmployeeDocumentVerificationID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeDocumentSubmissionID { get; set; }
        public string VerificationStatus { get; set; } = "Pending";
        public string? VerificationRemarks { get; set; }
        public long? VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }

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

    public class PolicyAcceptance
    {
        public long PolicyAcceptanceID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long PolicyID { get; set; }
        public DateTime AcceptedDate { get; set; }
        public string AcceptanceVersion { get; set; } = null!;

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

    public class EmployeeESignature
    {
        public long EmployeeESignatureID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? SignatureFilePath { get; set; }
        public string SignatureHash { get; set; } = null!;
        public DateTime SignatureDate { get; set; }

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

    public class EquipmentProvisioning
    {
        public long EquipmentProvisioningID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long AssetID { get; set; }
        public DateTime ProvisionDate { get; set; }
        public bool ReturnRequired { get; set; }

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

    public class ProbationReview
    {
        public long ProbationReviewID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ReviewStatus { get; set; } = null!;
        public long ReviewerID { get; set; }
        public string? Comments { get; set; }

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
