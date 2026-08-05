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
    public class PromotionsController : ControllerBase
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
            r.CreatedBy = 1;
            var id = await _promotionRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Promotion request initiated."));
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Approve(long id, [FromQuery] long tenantId)
        {
            var ok = await _promotionRepository.ApproveAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Promotion request approved."));
        }

        [HttpPut("{id}/complete")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Complete(long id, [FromQuery] long tenantId)
        {
            var ok = await _promotionRepository.CompleteAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Promotion request completed."));
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReport([FromQuery] long tenantId, [FromQuery] long? employeeId)
        {
            var history = await _promotionRepository.GetPromotionHistoryReportAsync(tenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<EmployeePromotionDto>>.SuccessResult(history));
        }
    }
}
