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
    public class PermissionsController : BaseApiController
    {
        private readonly IPermissionRepository _permissionRepository;
        public PermissionsController(IPermissionRepository permissionRepository) => _permissionRepository = permissionRepository;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionRequest r)
        {
            r.TenantId = GetEffectiveTenantId(r.TenantId);
            r.CreatedBy = CurrentUserId;
            var id = await _permissionRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Permission created."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdatePermissionRequest r)
        {
            r.PermissionId = id;
            r.ModifiedBy = CurrentUserId;
            var ok = await _permissionRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Permission updated."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _permissionRepository.DeleteAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Permission deleted."));
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] long tenantId = 0, [FromQuery] string? searchText = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var perms = await _permissionRepository.SearchAsync(effectiveTenantId, searchText, page, pageSize);
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<PermissionDto>>.SuccessResult(perms));
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromQuery] long tenantId = 0, [FromQuery] long roleId = 0, [FromQuery] long permissionId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _permissionRepository.AssignToRoleAsync(effectiveTenantId, roleId, permissionId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Permission assigned to role."));
        }

        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRole([FromQuery] long tenantId = 0, [FromQuery] long roleId = 0, [FromQuery] long permissionId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _permissionRepository.RemoveFromRoleAsync(effectiveTenantId, roleId, permissionId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Permission removed from role."));
        }

        [HttpPost("assign-user")]
        public async Task<IActionResult> AssignUser([FromQuery] long tenantId = 0, [FromQuery] long userId = 0, [FromQuery] long permissionId = 0, [FromQuery] bool isAllowed = true)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _permissionRepository.AssignToUserAsync(effectiveTenantId, userId, permissionId, isAllowed, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Permission assigned to user."));
        }

        [HttpPost("remove-user")]
        public async Task<IActionResult> RemoveUser([FromQuery] long tenantId = 0, [FromQuery] long userId = 0, [FromQuery] long permissionId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _permissionRepository.RemoveFromUserAsync(effectiveTenantId, userId, permissionId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Permission removed from user."));
        }
    }
}
