using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/onboarding")]
    [ApiController]
    [Authorize]
    public class OnboardingController : BaseApiController
    {
        private readonly IOnboardingRepository _onboardingRepository;

        public OnboardingController(IOnboardingRepository onboardingRepository)
        {
            _onboardingRepository = onboardingRepository;
        }

        [HttpPost("workflows")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateWorkflow([FromBody] CreateOnboardingWorkflowRequest request)
        {
            request.TenantID = GetEffectiveTenantId(request.TenantID);
            request.CreatedBy = CurrentUserId;
            var id = await _onboardingRepository.CreateWorkflowAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(id, "Onboarding workflow created successfully."));
        }

        [HttpPut("workflows/{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> UpdateWorkflow(long id, [FromBody] UpdateOnboardingWorkflowRequest request)
        {
            request.OnboardingWorkflowID = id;
            request.TenantID = GetEffectiveTenantId(request.TenantID);
            request.ModifiedBy = CurrentUserId;
            var success = await _onboardingRepository.UpdateWorkflowAsync(request);
            return Ok(ApiResponse<bool>.SuccessResult(success, "Onboarding workflow updated successfully."));
        }

        [HttpPost("workflows/{id}/start")]
        public async Task<IActionResult> StartWorkflow(long id, [FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var success = await _onboardingRepository.StartWorkflowAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(success, "Onboarding workflow started."));
        }

        [HttpPost("workflows/{id}/complete")]
        public async Task<IActionResult> CompleteWorkflow(long id, [FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var success = await _onboardingRepository.CompleteWorkflowAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(success, "Onboarding workflow completed."));
        }

        [HttpGet("workflows")]
        public async Task<IActionResult> SearchWorkflows([FromQuery] long tenantId, [FromQuery] long? employeeId, [FromQuery] string? status)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var workflows = await _onboardingRepository.SearchWorkflowsAsync(effectiveTenantId, employeeId, status);
            return Ok(ApiResponse<IEnumerable<OnboardingWorkflowDto>>.SuccessResult(workflows));
        }

        [HttpPost("tasks")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateTask([FromBody] CreateOnboardingTaskRequest request)
        {
            request.TenantID = GetEffectiveTenantId(request.TenantID);
            request.CreatedBy = CurrentUserId;
            var id = await _onboardingRepository.CreateTaskAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(id, "Onboarding task dictionary definition created."));
        }

        [HttpPut("tasks/{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> UpdateTask(long id, [FromBody] UpdateOnboardingTaskRequest request)
        {
            request.OnboardingTaskID = id;
            request.TenantID = GetEffectiveTenantId(request.TenantID);
            request.ModifiedBy = CurrentUserId;
            var success = await _onboardingRepository.UpdateTaskAsync(request);
            return Ok(ApiResponse<bool>.SuccessResult(success, "Onboarding task dictionary definition updated."));
        }

        [HttpDelete("tasks/{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> DeleteTask(long id, [FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var success = await _onboardingRepository.DeleteTaskAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(success, "Onboarding task definition deleted."));
        }

        [HttpPost("tasks/{id}/assign")]
        public async Task<IActionResult> AssignTask(long id, [FromBody] AssignOnboardingTaskRequest request)
        {
            request.OnboardingTaskID = id;
            request.TenantID = GetEffectiveTenantId(request.TenantID);
            request.CreatedBy = CurrentUserId;
            var assignmentId = await _onboardingRepository.AssignTaskAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(assignmentId, "Onboarding task assigned to employee."));
        }

        [HttpPost("tasks/{id}/complete")]
        public async Task<IActionResult> CompleteTaskAssignment(long id, [FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var success = await _onboardingRepository.CompleteTaskAssignmentAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(success, "Onboarding task marked as completed."));
        }

        [HttpPost("documents")]
        public async Task<IActionResult> CreateDocumentSubmission([FromBody] CreateDocumentSubmissionRequest request)
        {
            request.TenantID = GetEffectiveTenantId(request.TenantID);
            request.CreatedBy = CurrentUserId;
            var id = await _onboardingRepository.CreateDocumentSubmissionAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(id, "Document submission uploaded successfully."));
        }

        [HttpGet("documents")]
        public async Task<IActionResult> SearchDocumentSubmissions([FromQuery] long tenantId, [FromQuery] long? employeeId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var documents = await _onboardingRepository.SearchDocumentSubmissionsAsync(effectiveTenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<DocumentSubmissionDto>>.SuccessResult(documents));
        }

        [HttpPost("documents/{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN,MANAGER")]
        public async Task<IActionResult> ApproveDocumentVerification(long id, [FromBody] ApproveDocumentVerificationRequest request)
        {
            request.EmployeeDocumentSubmissionID = id;
            request.TenantID = GetEffectiveTenantId(request.TenantID);
            request.VerifiedBy = CurrentUserId;
            var verificationId = await _onboardingRepository.ApproveDocumentVerificationAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(verificationId, "Document verification approved."));
        }

        [HttpPost("documents/{id}/reject")]
        [Authorize(Roles = "ADMIN,SYSADMIN,MANAGER")]
        public async Task<IActionResult> RejectDocumentVerification(long id, [FromBody] RejectDocumentVerificationRequest request)
        {
            request.EmployeeDocumentSubmissionID = id;
            request.TenantID = GetEffectiveTenantId(request.TenantID);
            request.VerifiedBy = CurrentUserId;
            var verificationId = await _onboardingRepository.RejectDocumentVerificationAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(verificationId, "Document verification rejected."));
        }

        [HttpPost("policies/{id}/accept")]
        public async Task<IActionResult> RecordPolicyAcceptance(long id, [FromBody] RecordPolicyAcceptanceRequest request)
        {
            request.PolicyID = id;
            request.TenantID = GetEffectiveTenantId(request.TenantID);
            request.CreatedBy = CurrentUserId;
            var idAccepted = await _onboardingRepository.RecordPolicyAcceptanceAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(idAccepted, "Policy acceptance recorded."));
        }

        [HttpPost("e-signatures")]
        public async Task<IActionResult> CreateESignature([FromBody] CreateESignatureRequest request)
        {
            request.TenantID = GetEffectiveTenantId(request.TenantID);
            request.CreatedBy = CurrentUserId;
            var id = await _onboardingRepository.CreateESignatureAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(id, "E-signature saved successfully."));
        }

        [HttpPost("equipment")]
        public async Task<IActionResult> CreateEquipmentProvisioning([FromBody] CreateEquipmentProvisioningRequest request)
        {
            request.TenantID = GetEffectiveTenantId(request.TenantID);
            request.CreatedBy = CurrentUserId;
            var id = await _onboardingRepository.CreateEquipmentProvisioningAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(id, "Equipment provisioning item recorded."));
        }

        [HttpPost("probation-reviews")]
        public async Task<IActionResult> CreateProbationReview([FromBody] CreateProbationReviewRequest request)
        {
            request.TenantID = GetEffectiveTenantId(request.TenantID);
            request.CreatedBy = CurrentUserId;
            var id = await _onboardingRepository.CreateProbationReviewAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(id, "Probation review decision saved."));
        }

        [HttpGet("reports/status")]
        public async Task<IActionResult> GetOnboardingStatusReport([FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var report = await _onboardingRepository.GetOnboardingStatusReportAsync(effectiveTenantId);
            return Ok(ApiResponse<IEnumerable<OnboardingStatusReportDto>>.SuccessResult(report));
        }

        [HttpGet("reports/pending-tasks")]
        public async Task<IActionResult> GetPendingTasksReport([FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var report = await _onboardingRepository.GetPendingTasksReportAsync(effectiveTenantId);
            return Ok(ApiResponse<IEnumerable<PendingTasksReportDto>>.SuccessResult(report));
        }
    }
}
