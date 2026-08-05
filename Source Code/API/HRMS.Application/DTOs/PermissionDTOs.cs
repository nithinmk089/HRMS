namespace HRMS.Application.DTOs
{
    public class PermissionDto
    {
        public long PermissionId { get; set; }
        public long TenantId { get; set; }
        public string PermissionCode { get; set; } = string.Empty;
        public string PermissionName { get; set; } = string.Empty;
        public string ModuleCode { get; set; } = string.Empty;
        public long VersionNo { get; set; }
    }

    public class CreatePermissionRequest
    {
        public long TenantId { get; set; }
        public string PermissionCode { get; set; } = string.Empty;
        public string PermissionName { get; set; } = string.Empty;
        public string ModuleCode { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    public class UpdatePermissionRequest
    {
        public long PermissionId { get; set; }
        public long TenantId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public string ModuleCode { get; set; } = string.Empty;
        public long ModifiedBy { get; set; }
    }
}
