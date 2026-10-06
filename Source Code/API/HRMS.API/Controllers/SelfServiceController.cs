using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/self")]
    [ApiController]
    [Authorize]
    public class SelfServiceController : BaseApiController
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeDocumentRepository _documentRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ILeaveRepository _leaveRepository;

        public SelfServiceController(
            IEmployeeRepository employeeRepository, 
            IEmployeeDocumentRepository documentRepository,
            IAttendanceRepository attendanceRepository,
            ILeaveRepository leaveRepository)
        {
            _employeeRepository = employeeRepository;
            _documentRepository = documentRepository;
            _attendanceRepository = attendanceRepository;
            _leaveRepository = leaveRepository;
        }

        private long GetEffectiveEmployeeId(long requestedEmployeeId)
        {
            // If caller is not admin, strictly enforce caller's own employee id
            if (!IsAdmin && CurrentEmployeeId.HasValue)
            {
                return CurrentEmployeeId.Value;
            }
            if (requestedEmployeeId > 0) return requestedEmployeeId;
            return CurrentEmployeeId ?? 1;
        }

        private long GetEffectiveTenantId(long requestedTenantId)
        {
            if (!IsAdmin || requestedTenantId <= 0)
            {
                return CurrentTenantId;
            }
            return requestedTenantId;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile([FromQuery] long employeeId, [FromQuery] long tenantId)
        {
            var empId = GetEffectiveEmployeeId(employeeId);
            var tId = GetEffectiveTenantId(tenantId);
            var profile = await _employeeRepository.GetByIdAsync(empId, tId);
            if (profile == null) return NotFound(ApiResponse<EmployeeDto>.FailureResult("Profile not found."));
            return Ok(ApiResponse<EmployeeDto>.SuccessResult(profile));
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromQuery] long employeeId, [FromBody] UpdateEmployeeRequest r)
        {
            var empId = GetEffectiveEmployeeId(employeeId > 0 ? employeeId : r.EmployeeId);
            var tId = GetEffectiveTenantId(r.TenantId);
            var ok = await _employeeRepository.UpdateProfileAsync(empId, tId, r.PreferredName, r.MaritalStatus, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Self profile details updated successfully."));
        }

        [HttpPut("contact-details")]
        public async Task<IActionResult> UpdateContactDetails([FromQuery] long employeeId, [FromBody] UpdateEmployeeRequest r)
        {
            var empId = GetEffectiveEmployeeId(employeeId > 0 ? employeeId : r.EmployeeId);
            var tId = GetEffectiveTenantId(r.TenantId);
            var ok = await _employeeRepository.UpdateContactDetailsAsync(empId, tId, r.PersonalEmail, r.MobileNumber, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Self contact details updated successfully."));
        }

        [HttpGet("service-history")]
        public async Task<IActionResult> GetServiceHistory([FromQuery] long employeeId, [FromQuery] long tenantId)
        {
            var empId = GetEffectiveEmployeeId(employeeId);
            var tId = GetEffectiveTenantId(tenantId);
            var history = await _employeeRepository.GetServiceHistoryReportAsync(tId, empId);
            return Ok(ApiResponse<IEnumerable<EmployeeServiceHistoryReportDto>>.SuccessResult(history));
        }

        [HttpGet("documents")]
        public async Task<IActionResult> GetDocuments([FromQuery] long employeeId, [FromQuery] long tenantId, [FromQuery] string? docType)
        {
            var empId = GetEffectiveEmployeeId(employeeId);
            var tId = GetEffectiveTenantId(tenantId);
            var docs = await _documentRepository.SearchAsync(tId, empId, docType);
            return Ok(ApiResponse<IEnumerable<EmployeeDocumentDto>>.SuccessResult(docs));
        }

        [HttpPost("documents")]
        public async Task<IActionResult> UploadDocument([FromQuery] long employeeId, [FromBody] UploadEmployeeDocumentRequest r)
        {
            var empId = GetEffectiveEmployeeId(employeeId > 0 ? employeeId : r.EmployeeId);
            var tId = GetEffectiveTenantId(r.TenantId);
            r.EmployeeId = empId;
            r.TenantId = tId;
            r.CreatedBy = CurrentUserId;
            var id = await _documentRepository.UploadAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Self document uploaded successfully."));
        }

        [HttpGet("attendance")]
        public async Task<IActionResult> GetSelfAttendance([FromQuery] long employeeId, [FromQuery] long tenantId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var empId = GetEffectiveEmployeeId(employeeId);
            var tId = GetEffectiveTenantId(tenantId);
            var records = await _attendanceRepository.SearchAsync(tId, empId, startDate, endDate);
            return Ok(ApiResponse<IEnumerable<AttendanceDto>>.SuccessResult(records));
        }

        [HttpGet("leave-balances")]
        public async Task<IActionResult> GetSelfLeaveBalances([FromQuery] long employeeId, [FromQuery] long tenantId)
        {
            var empId = GetEffectiveEmployeeId(employeeId);
            var tId = GetEffectiveTenantId(tenantId);
            var balances = await _leaveRepository.GetBalancesAsync(tId, empId, null);
            return Ok(ApiResponse<IEnumerable<LeaveBalanceDto>>.SuccessResult(balances));
        }

        [HttpGet("leave-history")]
        public async Task<IActionResult> GetSelfLeaveHistory([FromQuery] long employeeId, [FromQuery] long tenantId)
        {
            var empId = GetEffectiveEmployeeId(employeeId);
            var tId = GetEffectiveTenantId(tenantId);
            var history = await _leaveRepository.SearchLeaveRequestsAsync(tId, empId, null);
            return Ok(ApiResponse<IEnumerable<LeaveRequestDto>>.SuccessResult(history));
        }

        [HttpPost("leave-request")]
        public async Task<IActionResult> SubmitSelfLeaveRequest([FromBody] CreateLeaveRequestRequest r)
        {
            var empId = GetEffectiveEmployeeId(r.EmployeeId);
            var tId = GetEffectiveTenantId(r.TenantId);
            r.EmployeeId = empId;
            r.TenantId = tId;
            r.CreatedBy = CurrentUserId;
            var id = await _leaveRepository.CreateLeaveRequestAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Self leave request submitted successfully."));
        }

        [HttpPost("regularization")]
        public async Task<IActionResult> SubmitSelfRegularization([FromBody] CreateAttendanceRegularizationRequest r)
        {
            var empId = GetEffectiveEmployeeId(r.EmployeeId);
            var tId = GetEffectiveTenantId(r.TenantId);
            r.EmployeeId = empId;
            r.TenantId = tId;
            r.CreatedBy = CurrentUserId;
            var id = await _attendanceRepository.CreateRegularizationAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Self regularization request submitted."));
        }
    }
}
