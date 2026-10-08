using System.Collections.Generic;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/tax")]
    [ApiController]
    [Authorize]
    public class TaxController : BaseApiController
    {
        private readonly ITaxRepository _taxRepository;
        public TaxController(ITaxRepository taxRepository) => _taxRepository = taxRepository;

        // --- Regimes ---
        [HttpGet("regimes")]
        public async Task<IActionResult> GetRegimes([FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _taxRepository.GetTaxRegimesAsync(effectiveTenantId);
            return Ok(ApiResponse<IEnumerable<TaxRegimeDto>>.SuccessResult(res));
        }

        [HttpPost("regimes")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateRegime([FromBody] CreateTaxRegimeRequest r)
        {
            r.TenantID = GetEffectiveTenantId(r.TenantID);
            r.CreatedBy = CurrentUserId;
            var id = await _taxRepository.CreateTaxRegimeAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Tax regime template added."));
        }

        // --- Slabs ---
        [HttpGet("slabs")]
        public async Task<IActionResult> GetSlabs([FromQuery] long regimeId, [FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _taxRepository.GetTaxSlabsAsync(regimeId, effectiveTenantId);
            return Ok(ApiResponse<IEnumerable<TaxSlabDto>>.SuccessResult(res));
        }

        [HttpPost("slabs")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> CreateSlab([FromBody] CreateTaxSlabRequest r)
        {
            r.TenantID = GetEffectiveTenantId(r.TenantID);
            r.CreatedBy = CurrentUserId;
            var id = await _taxRepository.CreateTaxSlabAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Tax slab criteria mapped."));
        }

        // --- Declarations ---
        [HttpGet("declarations")]
        public async Task<IActionResult> GetDeclarations([FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _taxRepository.GetTaxDeclarationsAsync(effectiveTenantId);
            return Ok(ApiResponse<IEnumerable<EmployeeTaxDeclarationDto>>.SuccessResult(res));
        }

        [HttpPost("declarations")]
        public async Task<IActionResult> CreateDeclaration([FromBody] CreateTaxDeclarationRequest r)
        {
            r.TenantID = GetEffectiveTenantId(r.TenantID);
            r.CreatedBy = CurrentUserId;
            var id = await _taxRepository.CreateTaxDeclarationAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Tax declaration submitted successfully."));
        }

        // --- Computations ---
        [HttpGet("computations")]
        public async Task<IActionResult> GetComputations([FromQuery] long tenantId, [FromQuery] string financialYear)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _taxRepository.GetTaxComputationsAsync(effectiveTenantId, financialYear);
            return Ok(ApiResponse<IEnumerable<EmployeeTaxComputationDto>>.SuccessResult(res));
        }

        // --- Statutory Deductions ---
        [HttpGet("statutory-deductions")]
        public async Task<IActionResult> GetStatutoryDeductions([FromQuery] long tenantId)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var res = await _taxRepository.GetStatutoryDeductionsAsync(effectiveTenantId);
            return Ok(ApiResponse<IEnumerable<StatutoryDeductionDto>>.SuccessResult(res));
        }
    }
}
