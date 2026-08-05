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
    public class ConfigurationsController : ControllerBase
    {
        private readonly IConfigurationRepository _configurationRepository;
        public ConfigurationsController(IConfigurationRepository configurationRepository) => _configurationRepository = configurationRepository;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateConfigurationRequest r)
        {
            r.CreatedBy = 1;
            var id = await _configurationRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Configuration created."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateConfigurationRequest r)
        {
            r.ConfigurationId = id;
            r.ModifiedBy = 1;
            var ok = await _configurationRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Configuration updated."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long tenantId)
        {
            var ok = await _configurationRepository.DeleteAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Configuration deleted."));
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] long tenantId, [FromQuery] string? searchText, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var configs = await _configurationRepository.SearchAsync(tenantId, searchText, page, pageSize);
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<ConfigurationDto>>.SuccessResult(configs));
        }
    }
}
