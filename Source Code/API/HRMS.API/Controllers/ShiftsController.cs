using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/shifts")]
    [ApiController]
    [Authorize]
    public class ShiftsController : BaseApiController
    {
        private readonly IShiftRepository _shiftRepository;

        public ShiftsController(IShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> Create([FromBody] CreateShiftRequest r)
        {
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            r.CreatedBy = CurrentUserId;
            var id = await _shiftRepository.CreateShiftAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Shift created successfully."));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateShiftRequest r)
        {
            r.ShiftId = id;
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            r.ModifiedBy = CurrentUserId;
            var ok = await _shiftRepository.UpdateShiftAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Shift updated successfully."));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _shiftRepository.DeleteShiftAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Shift deleted successfully."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id, [FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var shift = await _shiftRepository.GetShiftByIdAsync(id, effectiveTenantId);
            if (shift == null) return NotFound(ApiResponse<ShiftDto>.FailureResult("Shift not found."));
            return Ok(ApiResponse<ShiftDto>.SuccessResult(shift));
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] long tenantId, [FromQuery] string? searchTerm)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var shifts = await _shiftRepository.SearchShiftsAsync(effectiveTenantId, searchTerm);
            return Ok(ApiResponse<IEnumerable<ShiftDto>>.SuccessResult(shifts));
        }

        [HttpPost("assignments")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> Assign([FromBody] CreateShiftAssignmentRequest r)
        {
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            r.CreatedBy = CurrentUserId;
            var id = await _shiftRepository.CreateShiftAssignmentAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Shift assigned successfully."));
        }
    }
}
