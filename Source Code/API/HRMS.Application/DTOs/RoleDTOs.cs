namespace HRMS.Application.DTOs
{
    public class ApplicationRoleDto
    {
        public long RoleId { get; set; }
        public long TenantId { get; set; }
        public string RoleCode { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long VersionNo { get; set; }
    }

    public class CreateRoleRequest
    {
        public long TenantId { get; set; }
        public string RoleCode { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateRoleRequest
    {
        public long RoleId { get; set; }
        public long TenantId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long ModifiedBy { get; set; }
    }
}
