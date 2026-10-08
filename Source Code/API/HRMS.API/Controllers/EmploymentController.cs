using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/employment")]
    [ApiController]
    [Authorize]
    public class EmploymentController : BaseApiController
    {
        private readonly IEmployeeEmploymentRepository _employmentRepository;

        public EmploymentController(IEmployeeEmploymentRepository employmentRepository)
        {
            _employmentRepository = employmentRepository;
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(long employeeId, [FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var emp = await _employmentRepository.GetByEmployeeIdAsync(employeeId, effectiveTenantId);
            return Ok(ApiResponse<EmployeeEmploymentDto>.SuccessResult(emp));
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeEmploymentRequest r)
        {
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            r.CreatedBy = CurrentUserId;
            var id = await _employmentRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Employment details created."));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateEmployeeEmploymentRequest r)
        {
            r.EmployeeEmploymentId = id;
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            r.ModifiedBy = CurrentUserId;
            var ok = await _employmentRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Employment details updated."));
        }
    }
}
