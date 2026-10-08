using System;
using System.Security.Claims;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/import")]
    [ApiController]
    [Authorize]
    public class DataImportController : BaseApiController
    {
        private readonly IDataImportService _importService;

        public DataImportController(IDataImportService importService)
        {
            _importService = importService;
        }

        [HttpGet("template/{entityType}")]
        [AllowAnonymous]
        public IActionResult DownloadTemplate(string entityType)
        {
            var content = _importService.GenerateTemplateCsv(entityType);
            string fileName = $"{entityType.ToLower()}_import_template.csv";
            return File(content, "text/csv", fileName);
        }

        [HttpPost("employees")]
        public async Task<IActionResult> ImportEmployees([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(ApiResponse<ImportResultDto>.FailureResult("Please upload a valid CSV or Excel file."));
            }

            long tenantId = CurrentTenantId;
            long userId = CurrentUserId;

            using var stream = file.OpenReadStream();
            var result = await _importService.ImportEmployeesAsync(tenantId, userId, stream, file.FileName);
            return Ok(ApiResponse<ImportResultDto>.SuccessResult(result, result.Message));
        }

        [HttpPost("master-data/{entityType}")]
        public async Task<IActionResult> ImportMasterData(string entityType, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(ApiResponse<ImportResultDto>.FailureResult("Please upload a valid CSV or Excel file."));
            }

            long tenantId = CurrentTenantId;
            long userId = CurrentUserId;

            using var stream = file.OpenReadStream();
            var result = await _importService.ImportMasterDataAsync(tenantId, userId, entityType, stream, file.FileName);
            return Ok(ApiResponse<ImportResultDto>.SuccessResult(result, result.Message));
        }

        [HttpPost("operational/{entityType}")]
        public async Task<IActionResult> ImportOperationalData(string entityType, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(ApiResponse<ImportResultDto>.FailureResult("Please upload a valid CSV or Excel file."));
            }

            long tenantId = CurrentTenantId;
            long userId = CurrentUserId;

            using var stream = file.OpenReadStream();
            var result = await _importService.ImportOperationalDataAsync(tenantId, userId, entityType, stream, file.FileName);
            return Ok(ApiResponse<ImportResultDto>.SuccessResult(result, result.Message));
        }
    }
}
