using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/departments")]
    [ApiController]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentsController(IDepartmentRepository departmentRepository) => _departmentRepository = departmentRepository;

        [HttpPost]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest r)
        {
            r.CreatedBy = 1;
            var id = await _departmentRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Department created."));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateDepartmentRequest r)
        {
            r.DepartmentId = id;
            r.ModifiedBy = 1;
            var ok = await _departmentRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Department updated."));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,SYSADMIN")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long tenantId)
        {
            var ok = await _departmentRepository.DeleteAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Department deleted."));
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] long tenantId, [FromQuery] long? businessUnitId, [FromQuery] string? searchText, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var depts = await _departmentRepository.SearchAsync(tenantId, businessUnitId, searchText, page, pageSize);
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<DepartmentDto>>.SuccessResult(depts));
        }
    }
}
