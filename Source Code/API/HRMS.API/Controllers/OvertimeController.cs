using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/overtime")]
    [ApiController]
    [Authorize]
    public class OvertimeController : BaseApiController
    {
        private readonly IOvertimeRepository _overtimeRepository;

        public OvertimeController(IOvertimeRepository overtimeRepository)
        {
            _overtimeRepository = overtimeRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOvertimeRequestRequest r)
        {
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            r.CreatedBy = CurrentUserId;
            var id = await _overtimeRepository.CreateOvertimeRequestAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Overtime request submitted."));
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] long tenantId, [FromQuery] long? employeeId, [FromQuery] string? status)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var requests = await _overtimeRepository.SearchOvertimeRequestsAsync(effectiveTenantId, employeeId, status);
            return Ok(ApiResponse<IEnumerable<OvertimeRequestDto>>.SuccessResult(requests));
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN,MANAGER")]
        public async Task<IActionResult> Approve(long id, [FromQuery] long tenantId, [FromQuery] string? remarks)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _overtimeRepository.ApproveOvertimeRequestAsync(id, effectiveTenantId, CurrentUserId, remarks);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Overtime request approved."));
        }

        [HttpPost("{id}/reject")]
        [Authorize(Roles = "ADMIN,SYSADMIN,MANAGER")]
        public async Task<IActionResult> Reject(long id, [FromQuery] long tenantId, [FromQuery] string? remarks)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _overtimeRepository.RejectOvertimeRequestAsync(id, effectiveTenantId, CurrentUserId, remarks);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Overtime request rejected."));
        }
    }
}
