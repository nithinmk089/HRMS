using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/travel")]
    [ApiController]
    [Authorize]
    public class TravelController : ControllerBase
    {
        private readonly ITravelRepository _travelRepository;
        public TravelController(ITravelRepository travelRepository) => _travelRepository = travelRepository;

        // --- Policies ---
        [HttpGet("policies")]
        public async Task<IActionResult> GetPolicies([FromQuery] long tenantId)
        {
            var res = await _travelRepository.GetPoliciesAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<TravelPolicyDto>>.SuccessResult(res));
        }

        [HttpPost("policies")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreatePolicy([FromBody] CreateTravelPolicyRequest r)
        {
            r.CreatedBy = 1;
            var id = await _travelRepository.CreatePolicyAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Travel policy created."));
        }

        // --- Requests ---
        [HttpGet("requests")]
        public async Task<IActionResult> GetRequests([FromQuery] long tenantId, [FromQuery] long? employeeId)
        {
            var res = await _travelRepository.GetRequestsAsync(tenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<TravelRequestDto>>.SuccessResult(res));
        }

        [HttpGet("requests/{id}")]
        public async Task<IActionResult> GetRequestById(long id, [FromQuery] long tenantId)
        {
            var res = await _travelRepository.GetRequestByIdAsync(id, tenantId);
            if (res == null) return NotFound(ApiResponse<object>.FailureResult("Travel request not found."));
            return Ok(ApiResponse<TravelRequestDto>.SuccessResult(res));
        }

        [HttpPost("requests")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateTravelRequest r)
        {
            r.CreatedBy = 1;
            var id = await _travelRepository.CreateRequestAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Travel request created."));
        }

        [HttpPost("requests/{id}/approve")]
        public async Task<IActionResult> ApproveRequest(long id, [FromBody] ApproveTravelRequestRequest r)
        {
            r.TravelRequestID = id;
            r.ModifiedBy = 1;
            await _travelRepository.ApproveRequestAsync(r);
            return Ok(ApiResponse<object>.SuccessResult(null, $"Travel request status updated to {r.ApprovalStatus}."));
        }

        [HttpPost("requests/{id}/cancel")]
        public async Task<IActionResult> CancelRequest(long id, [FromQuery] long tenantId)
        {
            await _travelRepository.CancelRequestAsync(id, tenantId, 1);
            return Ok(ApiResponse<object>.SuccessResult(null, "Travel request cancelled."));
        }

        // --- Advances ---
        [HttpGet("advances")]
        public async Task<IActionResult> GetAdvances([FromQuery] long tenantId, [FromQuery] long? employeeId)
        {
            var res = await _travelRepository.GetAdvancesAsync(tenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<TravelAdvanceDto>>.SuccessResult(res));
        }

        [HttpPost("advances")]
        public async Task<IActionResult> CreateAdvance([FromBody] CreateTravelAdvanceRequest r)
        {
            r.CreatedBy = 1;
            var id = await _travelRepository.CreateAdvanceAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Travel advance requested."));
        }

        [HttpPost("advances/{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN,FINANCE")]
        public async Task<IActionResult> ApproveAdvance(long id, [FromQuery] long tenantId)
        {
            await _travelRepository.ApproveAdvanceAsync(id, tenantId, 1, 1);
            return Ok(ApiResponse<object>.SuccessResult(null, "Travel advance approved."));
        }

        [HttpPost("advances/{id}/disburse")]
        [Authorize(Roles = "ADMIN,SYSADMIN,FINANCE")]
        public async Task<IActionResult> DisburseAdvance(long id, [FromQuery] long tenantId)
        {
            await _travelRepository.DisburseAdvanceAsync(id, tenantId, DateTime.UtcNow, 1);
            return Ok(ApiResponse<object>.SuccessResult(null, "Travel advance disbursed."));
        }

        // --- Categories ---
        [HttpGet("expense-categories")]
        public async Task<IActionResult> GetCategories([FromQuery] long tenantId)
        {
            var res = await _travelRepository.GetCategoriesAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<ExpenseCategoryDto>>.SuccessResult(res));
        }

        [HttpPost("expense-categories")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateExpenseCategoryRequest r)
        {
            r.CreatedBy = 1;
            var id = await _travelRepository.CreateCategoryAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Expense category created."));
        }

        // --- Claims ---
        [HttpGet("expense-claims")]
        public async Task<IActionResult> GetClaims([FromQuery] long tenantId, [FromQuery] long? employeeId)
        {
            var res = await _travelRepository.GetClaimsAsync(tenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<ExpenseClaimDto>>.SuccessResult(res));
        }

        [HttpGet("expense-claims/{id}")]
        public async Task<IActionResult> GetClaimById(long id, [FromQuery] long tenantId)
        {
            var res = await _travelRepository.GetClaimByIdAsync(id, tenantId);
            if (res == null) return NotFound(ApiResponse<object>.FailureResult("Expense claim not found."));
            return Ok(ApiResponse<ExpenseClaimDto>.SuccessResult(res));
        }

        [HttpPost("expense-claims")]
        public async Task<IActionResult> CreateClaim([FromBody] CreateExpenseClaimRequest r)
        {
            r.CreatedBy = 1;
            var id = await _travelRepository.CreateClaimAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Expense claim created."));
        }

        [HttpPost("expense-claims/{id}/submit")]
        public async Task<IActionResult> SubmitClaim(long id, [FromQuery] long tenantId)
        {
            await _travelRepository.SubmitClaimAsync(id, tenantId, 1);
            return Ok(ApiResponse<object>.SuccessResult(null, "Expense claim submitted."));
        }

        [HttpPost("expense-claims/{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN,FINANCE")]
        public async Task<IActionResult> ApproveClaim(long id, [FromBody] ApproveExpenseClaimRequest r)
        {
            r.ExpenseClaimID = id;
            r.ModifiedBy = 1;
            await _travelRepository.ApproveClaimAsync(r);
            return Ok(ApiResponse<object>.SuccessResult(null, $"Expense claim approved. Status updated to {r.ApprovalStatus}."));
        }

        // --- Settlements ---
        [HttpGet("settlements")]
        [Authorize(Roles = "ADMIN,SYSADMIN,FINANCE")]
        public async Task<IActionResult> GetSettlements([FromQuery] long tenantId)
        {
            var res = await _travelRepository.GetSettlementsAsync(tenantId);
            return Ok(ApiResponse<IEnumerable<ExpenseSettlementDto>>.SuccessResult(res));
        }

        [HttpPost("settlements/process")]
        [Authorize(Roles = "ADMIN,SYSADMIN,FINANCE")]
        public async Task<IActionResult> ProcessSettlement([FromBody] ProcessExpenseSettlementRequest r)
        {
            r.CreatedBy = 1;
            await _travelRepository.ProcessSettlementAsync(r);
            return Ok(ApiResponse<object>.SuccessResult(null, "Expense settlement processed successfully."));
        }

        // --- Receipts ---
        [HttpPost("receipts/upload")]
        public async Task<IActionResult> UploadReceipt([FromQuery] long itemId, [FromQuery] long tenantId, [FromQuery] string fileName)
        {
            // Simulate file storage by generating a mock server path
            var mockPath = $"/uploads/receipts/{Guid.NewGuid()}_{fileName}";
            var id = await _travelRepository.UploadReceiptAsync(itemId, tenantId, fileName, mockPath, 1);
            return Ok(ApiResponse<long>.SuccessResult(id, "Receipt uploaded successfully."));
        }

        [HttpDelete("receipts/{id}")]
        public async Task<IActionResult> DeleteReceipt(long id, [FromQuery] long tenantId)
        {
            await _travelRepository.DeleteReceiptAsync(id, tenantId, 1);
            return Ok(ApiResponse<object>.SuccessResult(null, "Receipt deleted."));
        }

        // --- Corporate Cards ---
        [HttpPost("corporate-cards/import")]
        [Authorize(Roles = "ADMIN,SYSADMIN,FINANCE")]
        public async Task<IActionResult> ImportTransaction([FromBody] ImportCorporateCardTransactionRequest r)
        {
            r.CreatedBy = 1;
            var id = await _travelRepository.ImportTransactionAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Corporate card transaction imported."));
        }

        [HttpPost("corporate-cards/reconcile")]
        [Authorize(Roles = "ADMIN,SYSADMIN,FINANCE")]
        public async Task<IActionResult> ReconcileTransaction([FromBody] ReconcileCorporateCardRequest r)
        {
            r.CreatedBy = 1;
            await _travelRepository.ReconcileTransactionAsync(r);
            return Ok(ApiResponse<object>.SuccessResult(null, "Corporate card transaction reconciled."));
        }

        [HttpGet("corporate-cards/transactions")]
        public async Task<IActionResult> GetTransactions([FromQuery] long tenantId, [FromQuery] long? employeeId)
        {
            var res = await _travelRepository.GetTransactionsAsync(tenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<CorporateCardTransactionDto>>.SuccessResult(res));
        }

        // --- Compliance ---
        [HttpGet("compliance")]
        public async Task<IActionResult> GetCompliance([FromQuery] long tenantId, [FromQuery] long? travelRequestId)
        {
            var res = await _travelRepository.GetComplianceAsync(tenantId, travelRequestId);
            return Ok(ApiResponse<IEnumerable<TravelComplianceDto>>.SuccessResult(res));
        }

        // --- Analytics ---
        [HttpGet("analytics/summary")]
        public async Task<IActionResult> GetAnalyticsSummary([FromQuery] long tenantId, [FromQuery] long? employeeId)
        {
            var res = await _travelRepository.GetAnalyticsAsync(tenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<TravelAnalyticsSummaryDto>>.SuccessResult(res));
        }
    }
}
