using System;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace HRMS.API.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    [Authorize(Roles = "ADMIN,SYSADMIN")]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository) => _userRepository = userRepository;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest r)
        {
            try
            {
                r.TenantId = GetEffectiveTenantId(r.TenantId);
                r.CreatedBy = CurrentUserId;
                var id = await _userRepository.CreateAsync(r);
                return CreatedAtAction(nameof(GetById), new { id, tenantId = r.TenantId }, ApiResponse<long>.SuccessResult(id, "User created."));
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601 || ex.Message.Contains("UQ_User_Email") || ex.Message.Contains("UQ_User_UserName") || ex.Message.Contains("already"))
            {
                string msg = ex.Message.Contains("UQ_User_UserName") ? $"Username '{r.UserName}' is already registered." : $"Email address '{r.Email}' is already registered to another user account.";
                return Ok(ApiResponse<long>.FailureResult(msg));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<long>.FailureResult(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateUserRequest r)
        {
            try
            {
                r.UserId = id;
                r.ModifiedBy = CurrentUserId;
                var ok = await _userRepository.UpdateAsync(r);
                if (!ok)
                {
                    return Ok(ApiResponse<bool>.FailureResult("User account not found or profile could not be updated."));
                }
                return Ok(ApiResponse<bool>.SuccessResult(true, "User updated successfully."));
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601 || ex.Message.Contains("UQ_User_Email") || ex.Message.Contains("already"))
            {
                return Ok(ApiResponse<bool>.FailureResult($"Email address '{r.Email}' is already registered to another user account."));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<bool>.FailureResult(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id, [FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _userRepository.DeleteAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "User deleted."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id, [FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var user = await _userRepository.GetByIdAsync(id, effectiveTenantId);
            if (user == null) return NotFound(ApiResponse<ApplicationUserDto>.FailureResult("User not found."));
            return Ok(ApiResponse<ApplicationUserDto>.SuccessResult(user));
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] long tenantId = 0, [FromQuery] string? searchText = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var users = await _userRepository.SearchAsync(effectiveTenantId, searchText, page, pageSize);
            return Ok(ApiResponse<System.Collections.Generic.IEnumerable<ApplicationUserDto>>.SuccessResult(users));
        }

        [HttpPost("{id}/lock")]
        public async Task<IActionResult> Lock(long id, [FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _userRepository.LockAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "User locked."));
        }

        [HttpPost("{id}/unlock")]
        public async Task<IActionResult> Unlock(long id, [FromQuery] long tenantId = 0)
        {
            var effectiveTenantId = GetEffectiveTenantId(tenantId);
            var ok = await _userRepository.UnlockAsync(id, effectiveTenantId, CurrentUserId);
            return Ok(ApiResponse<bool>.SuccessResult(ok, "User unlocked."));
        }
    }
}
