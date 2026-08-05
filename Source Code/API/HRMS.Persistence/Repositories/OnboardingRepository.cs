using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HRMS.Persistence.Repositories
{
    public class OnboardingRepository : IOnboardingRepository
    {
        private readonly string _connectionString;
        public OnboardingRepository(IConfiguration configuration) => _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<long> CreateWorkflowAsync(CreateOnboardingWorkflowRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantID);
            p.Add("@EmployeeID", request.EmployeeID);
            p.Add("@WorkflowCode", request.WorkflowCode);
            p.Add("@WorkflowName", request.WorkflowName);
            p.Add("@StartDate", request.StartDate);
            p.Add("@TargetCompletionDate", request.TargetCompletionDate);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@OnboardingWorkflowID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("onboarding.usp_OnboardingWorkflow_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@OnboardingWorkflowID");
        }

        public async Task<bool> UpdateWorkflowAsync(UpdateOnboardingWorkflowRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@OnboardingWorkflowID", request.OnboardingWorkflowID);
            p.Add("@TenantID", request.TenantID);
            p.Add("@WorkflowCode", request.WorkflowCode);
            p.Add("@WorkflowName", request.WorkflowName);
            p.Add("@TargetCompletionDate", request.TargetCompletionDate);
            p.Add("@WorkflowStatus", request.WorkflowStatus);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("onboarding.usp_OnboardingWorkflow_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> StartWorkflowAsync(long workflowId, long tenantId, long userId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@OnboardingWorkflowID", workflowId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", userId);
            var affected = await conn.ExecuteAsync("onboarding.usp_OnboardingWorkflow_Start", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> CompleteWorkflowAsync(long workflowId, long tenantId, long userId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@OnboardingWorkflowID", workflowId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", userId);
            var affected = await conn.ExecuteAsync("onboarding.usp_OnboardingWorkflow_Complete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<IEnumerable<OnboardingWorkflowDto>> SearchWorkflowsAsync(long tenantId, long? employeeId, string? status)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            p.Add("@WorkflowStatus", status);
            return await conn.QueryAsync<OnboardingWorkflowDto>("onboarding.usp_OnboardingWorkflow_Search", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> CreateTaskAsync(CreateOnboardingTaskRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantID);
            p.Add("@TaskCode", request.TaskCode);
            p.Add("@TaskName", request.TaskName);
            p.Add("@TaskType", request.TaskType);
            p.Add("@DueDays", request.DueDays);
            p.Add("@SequenceNo", request.SequenceNo);
            p.Add("@IsMandatory", request.IsMandatory);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@OnboardingTaskID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("onboarding.usp_OnboardingTask_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@OnboardingTaskID");
        }

        public async Task<bool> UpdateTaskAsync(UpdateOnboardingTaskRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@OnboardingTaskID", request.OnboardingTaskID);
            p.Add("@TenantID", request.TenantID);
            p.Add("@TaskCode", request.TaskCode);
            p.Add("@TaskName", request.TaskName);
            p.Add("@TaskType", request.TaskType);
            p.Add("@DueDays", request.DueDays);
            p.Add("@SequenceNo", request.SequenceNo);
            p.Add("@IsMandatory", request.IsMandatory);
            p.Add("@ModifiedBy", request.ModifiedBy);
            var affected = await conn.ExecuteAsync("onboarding.usp_OnboardingTask_Update", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> DeleteTaskAsync(long taskId, long tenantId, long userId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@OnboardingTaskID", taskId);
            p.Add("@TenantID", tenantId);
            p.Add("@DeletedBy", userId);
            var affected = await conn.ExecuteAsync("onboarding.usp_OnboardingTask_Delete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<long> AssignTaskAsync(AssignOnboardingTaskRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantID);
            p.Add("@EmployeeID", request.EmployeeID);
            p.Add("@OnboardingTaskID", request.OnboardingTaskID);
            p.Add("@AssignedDate", request.AssignedDate);
            p.Add("@DueDate", request.DueDate);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@OnboardingTaskAssignmentID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("onboarding.usp_OnboardingTaskAssignment_Assign", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@OnboardingTaskAssignmentID");
        }

        public async Task<bool> CompleteTaskAssignmentAsync(long assignmentId, long tenantId, long userId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@OnboardingTaskAssignmentID", assignmentId);
            p.Add("@TenantID", tenantId);
            p.Add("@ModifiedBy", userId);
            var affected = await conn.ExecuteAsync("onboarding.usp_OnboardingTaskAssignment_Complete", p, commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<long> CreateDocumentSubmissionAsync(CreateDocumentSubmissionRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantID);
            p.Add("@EmployeeID", request.EmployeeID);
            p.Add("@DocumentType", request.DocumentType);
            p.Add("@FileName", request.FileName);
            p.Add("@FilePath", request.FilePath);
            p.Add("@MimeType", request.MimeType);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@EmployeeDocumentSubmissionID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("onboarding.usp_DocumentSubmission_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeDocumentSubmissionID");
        }

        public async Task<IEnumerable<DocumentSubmissionDto>> SearchDocumentSubmissionsAsync(long tenantId, long? employeeId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            p.Add("@EmployeeID", employeeId);
            return await conn.QueryAsync<DocumentSubmissionDto>("onboarding.usp_DocumentSubmission_Search", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> ApproveDocumentVerificationAsync(ApproveDocumentVerificationRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeDocumentSubmissionID", request.EmployeeDocumentSubmissionID);
            p.Add("@TenantID", request.TenantID);
            p.Add("@VerificationRemarks", request.VerificationRemarks);
            p.Add("@VerifiedBy", request.VerifiedBy);
            p.Add("@EmployeeDocumentVerificationID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("onboarding.usp_DocumentVerification_Approve", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeDocumentVerificationID");
        }

        public async Task<long> RejectDocumentVerificationAsync(RejectDocumentVerificationRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@EmployeeDocumentSubmissionID", request.EmployeeDocumentSubmissionID);
            p.Add("@TenantID", request.TenantID);
            p.Add("@VerificationRemarks", request.VerificationRemarks);
            p.Add("@VerifiedBy", request.VerifiedBy);
            p.Add("@EmployeeDocumentVerificationID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("onboarding.usp_DocumentVerification_Reject", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeDocumentVerificationID");
        }

        public async Task<long> RecordPolicyAcceptanceAsync(RecordPolicyAcceptanceRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantID);
            p.Add("@EmployeeID", request.EmployeeID);
            p.Add("@PolicyID", request.PolicyID);
            p.Add("@AcceptanceVersion", request.AcceptanceVersion);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@PolicyAcceptanceID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("onboarding.usp_PolicyAcceptance_Record", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@PolicyAcceptanceID");
        }

        public async Task<long> CreateESignatureAsync(CreateESignatureRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantID);
            p.Add("@EmployeeID", request.EmployeeID);
            p.Add("@SignatureFilePath", request.SignatureFilePath);
            p.Add("@SignatureHash", request.SignatureHash);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@EmployeeESignatureID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("onboarding.usp_ESignature_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EmployeeESignatureID");
        }

        public async Task<long> CreateEquipmentProvisioningAsync(CreateEquipmentProvisioningRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantID);
            p.Add("@EmployeeID", request.EmployeeID);
            p.Add("@AssetID", request.AssetID);
            p.Add("@ProvisionDate", request.ProvisionDate);
            p.Add("@ReturnRequired", request.ReturnRequired);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@EquipmentProvisioningID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("onboarding.usp_EquipmentProvisioning_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@EquipmentProvisioningID");
        }

        public async Task<long> CreateProbationReviewAsync(CreateProbationReviewRequest request)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", request.TenantID);
            p.Add("@EmployeeID", request.EmployeeID);
            p.Add("@ReviewDate", request.ReviewDate);
            p.Add("@ReviewStatus", request.ReviewStatus);
            p.Add("@ReviewerID", request.ReviewerID);
            p.Add("@Comments", request.Comments);
            p.Add("@CreatedBy", request.CreatedBy);
            p.Add("@ProbationReviewID", dbType: DbType.Int64, direction: ParameterDirection.Output);
            await conn.ExecuteAsync("onboarding.usp_ProbationReview_Create", p, commandType: CommandType.StoredProcedure);
            return p.Get<long>("@ProbationReviewID");
        }

        public async Task<IEnumerable<OnboardingStatusReportDto>> GetOnboardingStatusReportAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            return await conn.QueryAsync<OnboardingStatusReportDto>("onboarding.usp_Report_OnboardingStatus", p, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PendingTasksReportDto>> GetPendingTasksReportAsync(long tenantId)
        {
            using var conn = new SqlConnection(_connectionString);
            var p = new DynamicParameters();
            p.Add("@TenantID", tenantId);
            return await conn.QueryAsync<PendingTasksReportDto>("onboarding.usp_Report_PendingTasks", p, commandType: CommandType.StoredProcedure);
        }
    }
}
