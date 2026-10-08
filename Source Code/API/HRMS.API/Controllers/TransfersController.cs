using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/transfers")]
    [ApiController]
    [Authorize]
    public class TransfersController : BaseApiController
    {
        private readonly IEmployeeTransferRepository _transferRepository;

        public TransfersController(IEmployeeTransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeTransferRequest r)
        {
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            r.CreatedBy = CurrentUserId;
            var id = await _transferRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Transfer request initiated."));
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Approve(long id, [FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _transferRepository.ApproveAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Transfer request approved."));
        }

        [HttpPut("{id}/complete")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Complete(long id, [FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _transferRepository.CompleteAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Transfer request completed."));
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReport([FromQuery] long tenantId, [FromQuery] long? employeeId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var history = await _transferRepository.GetTransferHistoryReportAsync(effectiveTenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<EmployeeTransferDto>>.SuccessResult(history));
        }
    }
}
