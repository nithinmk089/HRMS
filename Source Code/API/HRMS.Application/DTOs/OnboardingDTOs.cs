using System;

namespace HRMS.Application.DTOs
{
    public class CreateOnboardingWorkflowRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string WorkflowCode { get; set; } = null!;
        public string WorkflowName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime TargetCompletionDate { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateOnboardingWorkflowRequest
    {
        public long OnboardingWorkflowID { get; set; }
        public long TenantID { get; set; }
        public string WorkflowCode { get; set; } = null!;
        public string WorkflowName { get; set; } = null!;
        public DateTime TargetCompletionDate { get; set; }
        public string WorkflowStatus { get; set; } = null!;
        public long ModifiedBy { get; set; }
    }

    public class OnboardingWorkflowDto
    {
        public long OnboardingWorkflowID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string WorkflowCode { get; set; } = null!;
        public string WorkflowName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime TargetCompletionDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string WorkflowStatus { get; set; } = null!;
    }

    public class CreateOnboardingTaskRequest
    {
        public long TenantID { get; set; }
        public string TaskCode { get; set; } = null!;
        public string TaskName { get; set; } = null!;
        public string TaskType { get; set; } = null!;
        public int DueDays { get; set; }
        public int SequenceNo { get; set; }
        public bool IsMandatory { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateOnboardingTaskRequest
    {
        public long OnboardingTaskID { get; set; }
        public long TenantID { get; set; }
        public string TaskCode { get; set; } = null!;
        public string TaskName { get; set; } = null!;
        public string TaskType { get; set; } = null!;
        public int DueDays { get; set; }
        public int SequenceNo { get; set; }
        public bool IsMandatory { get; set; }
        public long ModifiedBy { get; set; }
    }

    public class AssignOnboardingTaskRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long OnboardingTaskID { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public long CreatedBy { get; set; }
    }

    public class CreateDocumentSubmissionRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string DocumentType { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public long CreatedBy { get; set; }
    }

    public class DocumentSubmissionDto
    {
        public long EmployeeDocumentSubmissionID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string DocumentType { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public DateTime UploadedDate { get; set; }
    }

    public class ApproveDocumentVerificationRequest
    {
        public long EmployeeDocumentSubmissionID { get; set; }
        public long TenantID { get; set; }
        public string VerificationRemarks { get; set; } = null!;
        public long VerifiedBy { get; set; }
    }

    public class RejectDocumentVerificationRequest
    {
        public long EmployeeDocumentSubmissionID { get; set; }
        public long TenantID { get; set; }
        public string VerificationRemarks { get; set; } = null!;
        public long VerifiedBy { get; set; }
    }

    public class RecordPolicyAcceptanceRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long PolicyID { get; set; }
        public string AcceptanceVersion { get; set; } = null!;
        public long CreatedBy { get; set; }
    }

    public class CreateESignatureRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string? SignatureFilePath { get; set; }
        public string SignatureHash { get; set; } = null!;
        public long CreatedBy { get; set; }
    }

    public class CreateEquipmentProvisioningRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long AssetID { get; set; }
        public DateTime ProvisionDate { get; set; }
        public bool ReturnRequired { get; set; }
        public long CreatedBy { get; set; }
    }

    public class CreateProbationReviewRequest
    {
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime ReviewDate { get; set; }
        public string ReviewStatus { get; set; } = null!;
        public long ReviewerID { get; set; }
        public string? Comments { get; set; }
        public long CreatedBy { get; set; }
    }

    public class OnboardingStatusReportDto
    {
        public long OnboardingWorkflowID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeCode { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public string WorkflowCode { get; set; } = null!;
        public string WorkflowName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime TargetCompletionDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string WorkflowStatus { get; set; } = null!;
        public decimal CompletionPercentage { get; set; }
    }

    public class PendingTasksReportDto
    {
        public long OnboardingTaskAssignmentID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; } = null!;
        public long OnboardingTaskID { get; set; }
        public string TaskCode { get; set; } = null!;
        public string TaskName { get; set; } = null!;
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public string TaskStatus { get; set; } = null!;
    }
}
