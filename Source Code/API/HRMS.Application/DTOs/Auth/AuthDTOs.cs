using System.Collections.Generic;

namespace HRMS.Application.DTOs.Auth
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public UserDto User { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }
        public List<CompanyDto> Companies { get; set; }
    }

    public class UserDto
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long OrganizationId { get; set; }
        public long? TenantId { get; set; }
    }

    public class CompanyDto
    {
        public long CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public bool IsDefault { get; set; }
    }

    public class SetupStatusResponse
    {
        public bool NeedsSetup { get; set; }
        public int ExistingTenantsCount { get; set; }
        public int ExistingUsersCount { get; set; }
        public string SystemMessage { get; set; }
    }

    public class InitialSystemSetupRequest
    {
        // Tenant Info
        public string TenantCode { get; set; }
        public string TenantName { get; set; }

        // Primary Company Info
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string LegalName { get; set; }
        public string TaxNumber { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }

        // Initial Admin Employee & User Account
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AdminEmail { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
    }
}
