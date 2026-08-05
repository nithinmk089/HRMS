using System;

namespace HRMS.Application.DTOs
{
    public class ApplicationUserDto
    {
        public long UserId { get; set; }
        public long TenantId { get; set; }
        public long? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsLocked { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public long VersionNo { get; set; }

        // Backward-compatibility aliases for Dapper/Serialization
        public long UserID { get => UserId; set => UserId = value; }
        public long TenantID { get => TenantId; set => TenantId = value; }
        public long? EmployeeID { get => EmployeeId; set => EmployeeId = value; }
    }

    public class CreateUserRequest
    {
        public long TenantId { get; set; }
        public long? EmployeeId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    public class UpdateUserRequest
    {
        public long UserId { get; set; }
        public long TenantId { get; set; }
        public long? EmployeeId { get; set; }
        public string Email { get; set; } = string.Empty;
        public long ModifiedBy { get; set; }
    }
}
