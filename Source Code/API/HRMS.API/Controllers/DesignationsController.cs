using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/designations")]
    [ApiController]
    [Authorize]
    public class DesignationsController : BaseApiController
    {
        private readonly IDesignationRepository _designationRepository;
        public DesignationsController(IDesignationRepository designationRepository) => _designationRepository = designationRepository;

        [HttpPost]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Create([FromBody] CreateDesignationRequest r)
        {
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            r.CreatedBy = CurrentUserId;
            var id = await _designationRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Designation created."));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateDesignationRequest r)
        {
            r.DesignationId = id;
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            r.ModifiedBy = CurrentUserId;
            var ok = await _designationRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Designation updated."));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _designationRepository.DeleteAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Designation deleted."));
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] long tenantId, [FromQuery] string? searchText, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var desgs = await _designationRepository.SearchAsync(effectiveTenantId, searchText, page, pageSize);
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<DesignationDto>>.SuccessResult(desgs));
        }
    }
}
