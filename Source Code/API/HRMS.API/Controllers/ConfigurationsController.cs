using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/configurations")]
    [ApiController]
    [Authorize(Roles = "ADMIN,SYSADMIN")]
    public class ConfigurationsController : BaseApiController
    {
        private readonly IConfigurationRepository _configurationRepository;
        public ConfigurationsController(IConfigurationRepository configurationRepository) => _configurationRepository = configurationRepository;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateConfigurationRequest r)
        {
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            r.CreatedBy = CurrentUserId;
            var id = await _configurationRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Configuration created."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateConfigurationRequest r)
        {
            r.ConfigurationId = id;
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            r.ModifiedBy = CurrentUserId;
            var ok = await _configurationRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Configuration updated."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _configurationRepository.DeleteAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Configuration deleted."));
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] long tenantId, [FromQuery] string? searchText, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var configs = await _configurationRepository.SearchAsync(effectiveTenantId, searchText, page, pageSize);
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<ConfigurationDto>>.SuccessResult(configs));
        }
    }
}
