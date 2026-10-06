using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/tenants")]
    [ApiController]
    [Authorize]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantRepository _tenantRepository;
        public TenantsController(ITenantRepository tenantRepository) => _tenantRepository = tenantRepository;

        [HttpPost]
        [Authorize(Roles = "SYSADMIN")]
        public async Task<IActionResult> Create([FromBody] CreateTenantRequest r)
        {
            r.CreatedBy = 1;
            var id = await _tenantRepository.CreateAsync(r);
            return CreatedAtAction(nameof(GetById), new { id }, ApiResponse<long>.SuccessResult(id, "Tenant created successfully."));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SYSADMIN")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateTenantRequest r)
        {
            r.TenantId = id;
            r.ModifiedBy = 1;
            var ok = await _tenantRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Tenant updated successfully."));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SYSADMIN")]
        public async Task<IActionResult> Delete(long id)
        {
            var ok = await _tenantRepository.DeleteAsync(id, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Tenant soft-deleted successfully."));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "SYSADMIN")]
        public async Task<IActionResult> GetById(long id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null) return NotFound(ApiResponse<TenantDto>.FailureResult("Tenant not found."));
            return Ok(ApiResponse<TenantDto>.SuccessResult(tenant));
        }

        [HttpGet]
        [Authorize(Roles = "SYSADMIN")]
        public async Task<IActionResult> Search([FromQuery] string? searchText, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var tenants = await _tenantRepository.SearchAsync(searchText, status, page, pageSize);
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<TenantDto>>.SuccessResult(tenants));
        }

        [HttpPost("{id}/activate")]
        [Authorize(Roles = "SYSADMIN")]
        public async Task<IActionResult> Activate(long id)
        {
            var ok = await _tenantRepository.ActivateAsync(id, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Tenant activated."));
        }

        [HttpPost("{id}/deactivate")]
        [Authorize(Roles = "SYSADMIN")]
        public async Task<IActionResult> Deactivate(long id)
        {
            var ok = await _tenantRepository.DeactivateAsync(id, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Tenant deactivated."));
        }
    }
}
