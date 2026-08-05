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
    public class SelfServiceController : ControllerBase
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

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile([FromQuery] long employeeId, [FromQuery] long tenantId)
        {
            var profile = await _employeeRepository.GetByIdAsync(employeeId, tenantId);
            if (profile == null) return NotFound(ApiResponse<EmployeeDto>.FailureResult("Profile not found."));
            return Ok(ApiResponse<EmployeeDto>.SuccessResult(profile));
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromQuery] long employeeId, [FromBody] UpdateEmployeeRequest r)
        {
            var ok = await _employeeRepository.UpdateProfileAsync(employeeId, r.TenantId, r.PreferredName, r.MaritalStatus, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Self profile details updated successfully."));
        }

        [HttpPut("contact-details")]
        public async Task<IActionResult> UpdateContactDetails([FromQuery] long employeeId, [FromBody] UpdateEmployeeRequest r)
        {
            var ok = await _employeeRepository.UpdateContactDetailsAsync(employeeId, r.TenantId, r.PersonalEmail, r.MobileNumber, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Self contact details updated successfully."));
        }

        [HttpGet("service-history")]
        public async Task<IActionResult> GetServiceHistory([FromQuery] long employeeId, [FromQuery] long tenantId)
        {
            var history = await _employeeRepository.GetServiceHistoryReportAsync(tenantId, employeeId);
            return Ok(ApiResponse<IEnumerable<EmployeeServiceHistoryReportDto>>.SuccessResult(history));
        }

        [HttpGet("documents")]
        public async Task<IActionResult> GetDocuments([FromQuery] long employeeId, [FromQuery] long tenantId, [FromQuery] string? docType)
        {
            var docs = await _documentRepository.SearchAsync(tenantId, employeeId, docType);
            return Ok(ApiResponse<IEnumerable<EmployeeDocumentDto>>.SuccessResult(docs));
        }

        [HttpPost("documents")]
        public async Task<IActionResult> UploadDocument([FromQuery] long employeeId, [FromBody] UploadEmployeeDocumentRequest r)
        {
            r.EmployeeId = employeeId;
            r.CreatedBy = 1;
            var id = await _documentRepository.UploadAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Self document uploaded successfully."));
        }

        [HttpGet("attendance")]
        public async Task<IActionResult> GetSelfAttendance([FromQuery] long employeeId, [FromQuery] long tenantId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var records = await _attendanceRepository.SearchAsync(tenantId, employeeId, startDate, endDate);
            return Ok(ApiResponse<IEnumerable<AttendanceDto>>.SuccessResult(records));
        }

        [HttpGet("leave-balances")]
        public async Task<IActionResult> GetSelfLeaveBalances([FromQuery] long employeeId, [FromQuery] long tenantId)
        {
            var balances = await _leaveRepository.GetBalancesAsync(tenantId, employeeId, null);
            return Ok(ApiResponse<IEnumerable<LeaveBalanceDto>>.SuccessResult(balances));
        }

        [HttpGet("leave-history")]
        public async Task<IActionResult> GetSelfLeaveHistory([FromQuery] long employeeId, [FromQuery] long tenantId)
        {
            var history = await _leaveRepository.SearchLeaveRequestsAsync(tenantId, employeeId, null);
            return Ok(ApiResponse<IEnumerable<LeaveRequestDto>>.SuccessResult(history));
        }

        [HttpPost("leave-request")]
        public async Task<IActionResult> SubmitSelfLeaveRequest([FromBody] CreateLeaveRequestRequest r)
        {
            r.CreatedBy = 1;
            var id = await _leaveRepository.CreateLeaveRequestAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Self leave request submitted successfully."));
        }

        [HttpPost("regularization")]
        public async Task<IActionResult> SubmitSelfRegularization([FromBody] CreateAttendanceRegularizationRequest r)
        {
            r.CreatedBy = 1;
            var id = await _attendanceRepository.CreateRegularizationAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Self regularization request submitted."));
        }
    }
}
