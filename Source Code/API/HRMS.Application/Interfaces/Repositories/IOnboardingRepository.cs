using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;

namespace HRMS.Application.Interfaces.Repositories
{
    public interface IOnboardingRepository
    {
        Task<long> CreateWorkflowAsync(CreateOnboardingWorkflowRequest request);
        Task<bool> UpdateWorkflowAsync(UpdateOnboardingWorkflowRequest request);
        Task<bool> StartWorkflowAsync(long workflowId, long tenantId, long userId);
        Task<bool> CompleteWorkflowAsync(long workflowId, long tenantId, long userId);
        Task<IEnumerable<OnboardingWorkflowDto>> SearchWorkflowsAsync(long tenantId, long? employeeId, string? status);

        Task<long> CreateTaskAsync(CreateOnboardingTaskRequest request);
        Task<bool> UpdateTaskAsync(UpdateOnboardingTaskRequest request);
        Task<bool> DeleteTaskAsync(long taskId, long tenantId, long userId);
        Task<long> AssignTaskAsync(AssignOnboardingTaskRequest request);
        Task<bool> CompleteTaskAssignmentAsync(long assignmentId, long tenantId, long userId);

        Task<long> CreateDocumentSubmissionAsync(CreateDocumentSubmissionRequest request);
        Task<IEnumerable<DocumentSubmissionDto>> SearchDocumentSubmissionsAsync(long tenantId, long? employeeId);
        Task<long> ApproveDocumentVerificationAsync(ApproveDocumentVerificationRequest request);
        Task<long> RejectDocumentVerificationAsync(RejectDocumentVerificationRequest request);

        Task<long> RecordPolicyAcceptanceAsync(RecordPolicyAcceptanceRequest request);
        Task<long> CreateESignatureAsync(CreateESignatureRequest request);
        Task<long> CreateEquipmentProvisioningAsync(CreateEquipmentProvisioningRequest request);
        Task<long> CreateProbationReviewAsync(CreateProbationReviewRequest request);

        Task<IEnumerable<OnboardingStatusReportDto>> GetOnboardingStatusReportAsync(long tenantId);
        Task<IEnumerable<PendingTasksReportDto>> GetPendingTasksReportAsync(long tenantId);
    }
}
