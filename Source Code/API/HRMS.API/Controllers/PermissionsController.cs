using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/permissions")]
    [ApiController]
    [Authorize(Roles = "ADMIN,SYSADMIN")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;
        public PermissionsController(IPermissionRepository permissionRepository) => _permissionRepository = permissionRepository;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionRequest r)
        {
            r.CreatedBy = 1;
            var id = await _permissionRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Permission created."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdatePermissionRequest r)
        {
            r.PermissionId = id;
            r.ModifiedBy = 1;
            var ok = await _permissionRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Permission updated."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long tenantId)
        {
            var ok = await _permissionRepository.DeleteAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Permission deleted."));
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] long tenantId, [FromQuery] string? searchText, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var perms = await _permissionRepository.SearchAsync(tenantId, searchText, page, pageSize);
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<PermissionDto>>.SuccessResult(perms));
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromQuery] long tenantId, [FromQuery] long roleId, [FromQuery] long permissionId)
        {
            var ok = await _permissionRepository.AssignToRoleAsync(tenantId, roleId, permissionId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Permission assigned to role."));
        }

        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRole([FromQuery] long tenantId, [FromQuery] long roleId, [FromQuery] long permissionId)
        {
            var ok = await _permissionRepository.RemoveFromRoleAsync(tenantId, roleId, permissionId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Permission removed from role."));
        }

        [HttpPost("assign-user")]
        public async Task<IActionResult> AssignUser([FromQuery] long tenantId, [FromQuery] long userId, [FromQuery] long permissionId, [FromQuery] bool isAllowed = true)
        {
            var ok = await _permissionRepository.AssignToUserAsync(tenantId, userId, permissionId, isAllowed, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Permission assigned to user."));
        }

        [HttpPost("remove-user")]
        public async Task<IActionResult> RemoveUser([FromQuery] long tenantId, [FromQuery] long userId, [FromQuery] long permissionId)
        {
            var ok = await _permissionRepository.RemoveFromUserAsync(tenantId, userId, permissionId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Permission removed from user."));
        }
    }
}
