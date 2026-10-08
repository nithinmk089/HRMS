using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/attendance")]
    [ApiController]
    [Authorize]
    public class AttendanceController : BaseApiController
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceController(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        [HttpPost("clock-in")]
        public async Task<IActionResult> ClockIn([FromBody] CreateAttendanceRequest r)
        {
            r.CreatedBy = CurrentUserId;
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            if (!IsAdmin && CurrentEmployeeId.HasValue)
            {
                r.EmployeeId = CurrentEmployeeId.Value;
            }
            // Enforce server-side timestamp to prevent client tampering
            r.ClockInTime = DateTime.UtcNow;
            if (r.AttendanceDate == default)
            {
                r.AttendanceDate = DateTime.UtcNow.Date;
            }

            var id = await _attendanceRepository.ClockInAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Clock in successful."));
        }

        [HttpPost("clock-out")]
        public async Task<IActionResult> ClockOut([FromBody] UpdateAttendanceRequest r)
        {
            r.ModifiedBy = CurrentUserId;
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            // Enforce server-side timestamp
            r.ClockOutTime = DateTime.UtcNow;

            var ok = await _attendanceRepository.ClockOutAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Clock out successful."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id, [FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var attendance = await _attendanceRepository.GetByIdAsync(id, effectiveTenantId);
            if (attendance == null) return NotFound(ApiResponse<AttendanceDto>.FailureResult("Attendance record not found."));
            return Ok(ApiResponse<AttendanceDto>.SuccessResult(attendance));
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] long tenantId = 0, [FromQuery] long? employeeId = null, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var resolvedTenantId = GetEffectiveTenantId(tenantId);
            if (!IsAdmin && !HasRole("MANAGER"))
            {
                if (CurrentEmployeeId.HasValue)
                {
                    employeeId = CurrentEmployeeId.Value;
                }
            }

            var records = await _attendanceRepository.SearchAsync(resolvedTenantId, employeeId, startDate, endDate);
            return Ok(ApiResponse<IEnumerable<AttendanceDto>>.SuccessResult(records));
        }

        [HttpPost("recalculate")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> Recalculate([FromQuery] long tenantId = 0, [FromQuery] long employeeId = 0, [FromQuery] DateTime attendanceDate = default)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _attendanceRepository.RecalculateAsync(effectiveTenantId, employeeId, attendanceDate, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Recalculation completed."));
        }

        [HttpPost("adjustments")]
        public async Task<IActionResult> CreateAdjustment([FromBody] CreateAttendanceAdjustmentRequest r)
        {
            r.CreatedBy = CurrentUserId;
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            var id = await _attendanceRepository.CreateAdjustmentAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Attendance adjustment request submitted."));
        }

        [HttpPost("adjustments/{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN,MANAGER")]
        public async Task<IActionResult> ApproveAdjustment(long id, [FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _attendanceRepository.ApproveAdjustmentAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Attendance adjustment approved."));
        }

        [HttpPost("adjustments/{id}/reject")]
        [Authorize(Roles = "ADMIN,SYSADMIN,MANAGER")]
        public async Task<IActionResult> RejectAdjustment(long id, [FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _attendanceRepository.RejectAdjustmentAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Attendance adjustment rejected."));
        }

        [HttpPost("regularizations")]
        public async Task<IActionResult> CreateRegularization([FromBody] CreateAttendanceRegularizationRequest r)
        {
            r.CreatedBy = CurrentUserId;
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            if (!IsAdmin && !HasRole("MANAGER") && CurrentEmployeeId.HasValue)
            {
                r.EmployeeId = CurrentEmployeeId.Value;
            }
            var id = await _attendanceRepository.CreateRegularizationAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Regularization request submitted."));
        }

        [HttpPost("regularizations/{id}/approve")]
        [Authorize(Roles = "ADMIN,SYSADMIN,MANAGER")]
        public async Task<IActionResult> ApproveRegularization(long id, [FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _attendanceRepository.ApproveRegularizationAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Regularization request approved."));
        }

        [HttpPost("regularizations/{id}/reject")]
        [Authorize(Roles = "ADMIN,SYSADMIN,MANAGER")]
        public async Task<IActionResult> RejectRegularization(long id, [FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _attendanceRepository.RejectRegularizationAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Regularization request rejected."));
        }
    }
}
