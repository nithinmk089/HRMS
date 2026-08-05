using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/leaves")]
    [ApiController]
    [Authorize]
    public class LeavesController : ControllerBase
    {
        private readonly ILeaveRepository _leaveRepository;

        public LeavesController(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        [HttpPost("types")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateLeaveType([FromBody] CreateLeaveTypeRequest r)
        {
            r.CreatedBy = 1;
            var id = await _leaveRepository.CreateLeaveTypeAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Leave type created successfully."));
        }

        [HttpGet("types")]
        public async Task<IActionResult> SearchLeaveTypes([FromQuery] long tenantId, [FromQuery] string? searchTerm)
        {
            var types = await _leaveRepository.SearchLeaveTypesAsync(tenantId, searchTerm);
            return Ok(ApiResponse<IEnumerable<LeaveTypeDto>>.SuccessResult(types));
        }

        [HttpPost("policies")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateLeavePolicy([FromBody] CreateLeavePolicyRequest r)
        {
            r.CreatedBy = 1;
            var id = await _leaveRepository.CreateLeavePolicyAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Leave policy created successfully."));
        }

        [HttpGet("balances/{employeeId}")]
        public async Task<IActionResult> GetBalances(long employeeId, [FromQuery] long tenantId, [FromQuery] long? leaveTypeId)
        {
            var balances = await _leaveRepository.GetBalancesAsync(tenantId, employeeId, leaveTypeId);
            return Ok(ApiResponse<IEnumerable<LeaveBalanceDto>>.SuccessResult(balances));
        }

        [HttpPost("requests")]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] CreateLeaveRequestRequest r)
        {
            r.CreatedBy = 1;
            var id = await _leaveRepository.CreateLeaveRequestAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Leave request submitted successfully."));
        }

        [HttpGet("requests")]
        public async Task<IActionResult> SearchLeaveRequests([FromQuery] long tenantId, [FromQuery] long? employeeId, [FromQuery] string? status)
        {
            var requests = await _leaveRepository.SearchLeaveRequestsAsync(tenantId, employeeId, status);
            return Ok(ApiResponse<IEnumerable<LeaveRequestDto>>.SuccessResult(requests));
        }

        [HttpPost("requests/{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN,MANAGER")]
        public async Task<IActionResult> ApproveLeaveRequest(long id, [FromQuery] long tenantId, [FromQuery] string? remarks)
        {
            var ok = await _leaveRepository.ApproveLeaveRequestAsync(id, tenantId, 1, remarks);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Leave request approved."));
        }

        [HttpPost("requests/{id}/reject")]
        [Authorize(Roles = "ADMIN,SYSADMIN,MANAGER")]
        public async Task<IActionResult> RejectLeaveRequest(long id, [FromQuery] long tenantId, [FromQuery] string? remarks)
        {
            var ok = await _leaveRepository.RejectLeaveRequestAsync(id, tenantId, 1, remarks);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Leave request rejected."));
        }

        [HttpPost("requests/{id}/cancel")]
        public async Task<IActionResult> CancelLeaveRequest(long id, [FromQuery] long tenantId)
        {
            var ok = await _leaveRepository.CancelLeaveRequestAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Leave request cancelled."));
        }

        [HttpPost("encashments")]
        public async Task<IActionResult> CreateLeaveEncashment([FromBody] CreateLeaveEncashmentRequest r)
        {
            r.CreatedBy = 1;
            var id = await _leaveRepository.CreateLeaveEncashmentAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Leave encashment request submitted."));
        }

        [HttpPost("encashments/{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> ApproveLeaveEncashment(long id, [FromQuery] long tenantId)
        {
            var ok = await _leaveRepository.ApproveLeaveEncashmentAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Leave encashment request approved."));
        }
    }
}
