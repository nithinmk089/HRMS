using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/offboarding")]
    [ApiController]
    [Authorize]
    public class OffboardingController : ControllerBase
    {
        private readonly IOffboardingRepository _offboardingRepository;

        public OffboardingController(IOffboardingRepository offboardingRepository)
        {
            _offboardingRepository = offboardingRepository;
        }

        [HttpPost("exit-requests")]
        public async Task<IActionResult> CreateExitRequest([FromBody] CreateExitRequest request)
        {
            request.CreatedBy = 1;
            var id = await _offboardingRepository.CreateExitRequestAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(id, "Resignation request submitted successfully."));
        }

        [HttpPut("exit-requests/{id}")]
        public async Task<IActionResult> UpdateExitRequest(long id, [FromBody] UpdateExitRequest request)
        {
            request.ExitRequestID = id;
            request.ModifiedBy = 1;
            var success = await _offboardingRepository.UpdateExitRequestAsync(request);
            return Ok(ApiResponse<bool>.SuccessResult(success, "Resignation request updated successfully."));
        }

        [HttpPost("exit-requests/{id}/submit")]
        public async Task<IActionResult> SubmitExitRequest(long id, [FromQuery] long tenantId)
        {
            var success = await _offboardingRepository.SubmitExitRequestAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(success, "Resignation request officially submitted."));
        }

        [HttpPost("exit-requests/{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN,MANAGER")]
        public async Task<IActionResult> ApproveExitRequest(long id, [FromBody] ApproveExitRequest request)
        {
            request.ExitRequestID = id;
            request.ApproverID = 1;
            var approvalId = await _offboardingRepository.ApproveExitRequestAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(approvalId, "Resignation request approved."));
        }

        [HttpPost("exit-requests/{id}/reject")]
        [Authorize(Roles = "ADMIN,SYSADMIN,MANAGER")]
        public async Task<IActionResult> RejectExitRequest(long id, [FromBody] RejectExitRequest request)
        {
            request.ExitRequestID = id;
            request.ApproverID = 1;
            var approvalId = await _offboardingRepository.RejectExitRequestAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(approvalId, "Resignation request rejected."));
        }

        [HttpPost("clearances")]
        public async Task<IActionResult> CreateClearanceRequest([FromBody] CreateClearanceRequest request)
        {
            request.CreatedBy = 1;
            var id = await _offboardingRepository.CreateClearanceRequestAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(id, "Clearance request initiated."));
        }

        [HttpPost("clearances/{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN,MANAGER")]
        public async Task<IActionResult> ApproveClearanceRequest(long id, [FromQuery] long tenantId)
        {
            var success = await _offboardingRepository.ApproveClearanceRequestAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(success, "Clearance request approved and completed."));
        }

        [HttpPost("clearances/tasks/{id}/complete")]
        public async Task<IActionResult> CompleteClearanceTask(long id, [FromQuery] long tenantId)
        {
            var success = await _offboardingRepository.CompleteClearanceTaskAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(success, "Clearance department task marked as completed."));
        }

        [HttpPost("asset-returns")]
        public async Task<IActionResult> CreateAssetReturn([FromBody] CreateAssetReturnRequest request)
        {
            request.CreatedBy = 1;
            var id = await _offboardingRepository.CreateAssetReturnAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(id, "Asset return logged."));
        }

        [HttpPost("asset-returns/{id}/verify")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> VerifyAssetReturn(long id, [FromBody] VerifyAssetReturnRequest request)
        {
            request.AssetReturnID = id;
            request.ModifiedBy = 1;
            var success = await _offboardingRepository.VerifyAssetReturnAsync(request);
            return Ok(ApiResponse<bool>.SuccessResult(success, "Asset condition and return verified."));
        }

        [HttpPost("knowledge-transfers")]
        public async Task<IActionResult> CreateKnowledgeTransfer([FromBody] CreateKnowledgeTransferRequest request)
        {
            request.CreatedBy = 1;
            var id = await _offboardingRepository.CreateKnowledgeTransferAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(id, "KT handover details logged."));
        }

        [HttpPost("knowledge-transfers/{id}/complete")]
        public async Task<IActionResult> CompleteKnowledgeTransfer(long id, [FromQuery] long tenantId)
        {
            var success = await _offboardingRepository.CompleteKnowledgeTransferAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(success, "KT handover confirmed as completed."));
        }

        [HttpPost("experience-letters/generate")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> GenerateExperienceLetter([FromBody] GenerateExperienceLetterRequest request)
        {
            request.CreatedBy = 1;
            var id = await _offboardingRepository.GenerateExperienceLetterAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(id, "Experience letter generated successfully."));
        }

        [HttpPost("settlements/calculate")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CalculateFullAndFinalSettlement([FromBody] CalculateFFSRequest request)
        {
            request.CreatedBy = 1;
            var id = await _offboardingRepository.CalculateFullAndFinalSettlementAsync(request);
            return Ok(ApiResponse<long>.SuccessResult(id, "Full and final settlement calculated."));
        }

        [HttpPost("settlements/{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> ApproveFullAndFinalSettlement(long id, [FromQuery] long tenantId)
        {
            var success = await _offboardingRepository.ApproveFullAndFinalSettlementAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(success, "Full and final settlement approved."));
        }

        [HttpPost("settlements/{id}/close")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CloseFullAndFinalSettlement(long id, [FromQuery] long tenantId)
        {
            var success = await _offboardingRepository.CloseFullAndFinalSettlementAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(success, "Full and final settlement closed and paid."));
        }

        [HttpGet("reports/exit-status")]
        public async Task<IActionResult> GetExitStatusReport([FromQuery] long tenantId)
        {
            var report = await _offboardingRepository.GetExitStatusReportAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<ExitStatusReportDto>>.SuccessResult(report));
        }

        [HttpGet("reports/clearance-status")]
        public async Task<IActionResult> GetClearanceStatusReport([FromQuery] long tenantId)
        {
            var report = await _offboardingRepository.GetClearanceStatusReportAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<ClearanceStatusReportDto>>.SuccessResult(report));
        }

        [HttpGet("reports/settlement-summary")]
        public async Task<IActionResult> GetFullAndFinalSummaryReport([FromQuery] long tenantId)
        {
            var report = await _offboardingRepository.GetFullAndFinalSummaryReportAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<FullAndFinalSummaryReportDto>>.SuccessResult(report));
        }
    }
}
