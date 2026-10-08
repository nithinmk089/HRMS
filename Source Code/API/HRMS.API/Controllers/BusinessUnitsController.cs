using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/business-units")]
    [ApiController]
    [Authorize]
    public class BusinessUnitsController : BaseApiController
    {
        private readonly IBusinessUnitRepository _businessUnitRepository;
        public BusinessUnitsController(IBusinessUnitRepository businessUnitRepository) => _businessUnitRepository = businessUnitRepository;

        [HttpPost]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Create([FromBody] CreateBusinessUnitRequest r)
        {
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            r.CreatedBy = CurrentUserId;
            var id = await _businessUnitRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Business Unit created."));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateBusinessUnitRequest r)
        {
            r.BusinessUnitId = id;
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            r.ModifiedBy = CurrentUserId;
            var ok = await _businessUnitRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Business Unit updated."));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _businessUnitRepository.DeleteAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Business Unit deleted."));
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] long tenantId, [FromQuery] long? companyId, [FromQuery] string? searchText, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var bus = await _businessUnitRepository.SearchAsync(effectiveTenantId, companyId, searchText, page, pageSize);
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<BusinessUnitDto>>.SuccessResult(bus));
        }
    }
}
