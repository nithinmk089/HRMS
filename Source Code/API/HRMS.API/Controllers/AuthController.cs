using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HRMS.Application.DTOs;
using HRMS.Application.DTOs.Auth;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HRMS.API.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        [HttpGet("setup-status")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSetupStatus()
        {
            var status = await _authRepository.GetSetupStatusAsync();
            return Ok(ApiResponse<SetupStatusResponse>.SuccessResult(status));
        }

        [HttpPost("setup-admin")]
        [AllowAnonymous]
        public async Task<IActionResult> SetupAdmin([FromBody] InitialSystemSetupRequest request)
        {
            var status = await _authRepository.GetSetupStatusAsync();
            if (!status.NeedsSetup)
            {
                return BadRequest(ApiResponse<AuthResponse>.FailureResult("System initial administration setup has already been completed. Unauthorized initialization request rejected."));
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
            var browserInfo = Request.Headers["User-Agent"].ToString() ?? "Browser";

            var result = await _authRepository.PerformInitialSetupAsync(request, ipAddress, browserInfo);
            if (!result.Success)
            {
                return BadRequest(ApiResponse<AuthResponse>.FailureResult(result.Message));
            }

            result.AccessToken = GenerateJwtToken(result);
            result.RefreshToken = Guid.NewGuid().ToString("N");

            return Ok(ApiResponse<AuthResponse>.SuccessResult(result, "Initial system administration setup completed successfully."));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var browserInfo = Request.Headers["User-Agent"].ToString() ?? "unknown";

            var result = await _authRepository.LoginAsync(request.Email, request.Password, ipAddress, browserInfo);
            
            if (!result.Success)
            {
                return Unauthorized(ApiResponse<AuthResponse>.FailureResult(result.Message));
            }

            result.AccessToken = GenerateJwtToken(result);
            result.RefreshToken = Guid.NewGuid().ToString("N");

            return Ok(ApiResponse<AuthResponse>.SuccessResult(result, "Login successful."));
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public IActionResult Refresh()
        {
            var data = new { token = "sample-refreshed-jwt-token", refreshToken = "sample-refreshed-refresh-token" };
            return Ok(ApiResponse<object>.SuccessResult(data, "Token refreshed."));
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            return Ok(ApiResponse<string>.SuccessResult("Logged out successfully."));
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public IActionResult ForgotPassword([FromBody] string email)
        {
            return Ok(ApiResponse<string>.SuccessResult($"Password reset email sent to {email}."));
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public IActionResult ResetPassword([FromBody] string newPassword)
        {
            return Ok(ApiResponse<string>.SuccessResult("Password reset successfully."));
        }

        private string GenerateJwtToken(AuthResponse authResponse)
        {
            var key = Environment.GetEnvironmentVariable("HRMS_JWT_KEY") ?? _configuration["Jwt:Key"] ?? "HRMSDevSecretKey_MustBe32CharactersLong!!";
            var issuer = _configuration["Jwt:Issuer"] ?? "HRMS.API";
            var audience = _configuration["Jwt:Audience"] ?? "HRMS.Client";
            var expiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, authResponse.User?.UserId.ToString() ?? "0"),
                new Claim(ClaimTypes.Email, authResponse.User?.Email ?? ""),
                new Claim("tenantId", authResponse.User?.TenantId.ToString() ?? "0"),
                new Claim("firstName", authResponse.User?.FirstName ?? ""),
                new Claim("lastName", authResponse.User?.LastName ?? "")
            };

            if (authResponse.User?.EmployeeId.HasValue == true)
            {
                claims.Add(new Claim("employeeId", authResponse.User.EmployeeId.Value.ToString()));
            }

            if (authResponse.Roles != null)
            {
                foreach (var role in authResponse.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            if (authResponse.Permissions != null)
            {
                foreach (var perm in authResponse.Permissions)
                {
                    claims.Add(new Claim("permission", perm));
                }
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
