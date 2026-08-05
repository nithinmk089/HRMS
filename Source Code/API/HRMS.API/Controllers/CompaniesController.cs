using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/companies")]
    [ApiController]
    [Authorize(Roles = "ADMIN,SYSADMIN,HRADMIN")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        public CompaniesController(ICompanyRepository companyRepository) => _companyRepository = companyRepository;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyRequest r)
        {
            r.CreatedBy = 1;
            var id = await _companyRepository.CreateAsync(r);
            return CreatedAtAction(nameof(GetById), new { id }, ApiResponse<long>.SuccessResult(id, "Company created."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateCompanyRequest r)
        {
            r.CompanyId = id;
            r.ModifiedBy = 1;
            var ok = await _companyRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Company updated."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long tenantId)
        {
            var ok = await _companyRepository.DeleteAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Company deleted."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id, [FromQuery] long tenantId)
        {
            var company = await _companyRepository.GetByIdAsync(id, tenantId);
            if (company == null) return NotFound(ApiResponse<CompanyDto>.FailureResult("Company not found."));
            return Ok(ApiResponse<CompanyDto>.SuccessResult(company));
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] long tenantId, [FromQuery] string? searchText, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var companies = await _companyRepository.SearchAsync(tenantId, searchText, page, pageSize);
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<CompanyDto>>.SuccessResult(companies));
        }
    }
}
