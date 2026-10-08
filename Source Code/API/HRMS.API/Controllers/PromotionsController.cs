using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/promotions")]
    [ApiController]
    [Authorize]
    public class PromotionsController : BaseApiController
    {
        private readonly IEmployeePromotionRepository _promotionRepository;

        public PromotionsController(IEmployeePromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Create([FromBody] CreateEmployeePromotionRequest r)
        {
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            r.CreatedBy = CurrentUserId;
            var id = await _promotionRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Promotion request initiated."));
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Approve(long id, [FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _promotionRepository.ApproveAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Promotion request approved."));
        }

        [HttpPut("{id}/complete")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Complete(long id, [FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _promotionRepository.CompleteAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Promotion request completed."));
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReport([FromQuery] long tenantId, [FromQuery] long? employeeId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var history = await _promotionRepository.GetPromotionHistoryReportAsync(effectiveTenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<EmployeePromotionDto>>.SuccessResult(history));
        }
    }
}
