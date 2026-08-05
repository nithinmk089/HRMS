using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [Route("api/v1/roles")]
    [ApiController]
    [Authorize(Roles = "ADMIN,SYSADMIN")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        public RolesController(IRoleRepository roleRepository) => _roleRepository = roleRepository;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest r)
        {
            r.CreatedBy = 1;
            var id = await _roleRepository.CreateAsync(r);
            return Ok(ApiResponse<long>.SuccessResult(id, "Role created."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateRoleRequest r)
        {
            r.RoleId = id;
            r.ModifiedBy = 1;
            var ok = await _roleRepository.UpdateAsync(r);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Role updated."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long tenantId)
        {
            var ok = await _roleRepository.DeleteAsync(id, tenantId, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Role deleted."));
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] long tenantId, [FromQuery] string? searchText, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var roles = await _roleRepository.SearchAsync(tenantId, searchText, page, pageSize);
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<ApplicationRoleDto>>.SuccessResult(roles));
        }

        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetRoleUsers(long id, [FromQuery] long tenantId)
        {
            var userIds = await _roleRepository.GetUserIdsForRoleAsync(id, tenantId);
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<long>>.SuccessResult(userIds));
        }

        [HttpPost("{id}/assign-user")]
        public async Task<IActionResult> AssignUser(long id, [FromQuery] long userId, [FromQuery] long tenantId)
        {
            var ok = await _roleRepository.AssignToUserAsync(tenantId, userId, id, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Role assigned to user."));
        }

        [HttpPost("{id}/remove-user")]
        public async Task<IActionResult> RemoveUser(long id, [FromQuery] long userId, [FromQuery] long tenantId)
        {
            var ok = await _roleRepository.RemoveFromUserAsync(tenantId, userId, id, 1);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "Role removed from user."));
        }
    }
}
