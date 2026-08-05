namespace HRMS.Application.DTOs
{
    public class DepartmentDto
    {
        public long DepartmentId { get; set; }
        public long TenantId { get; set; }
        public long BusinessUnitId { get; set; }
        public string DepartmentCode { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public long? ParentDepartmentId { get; set; }
        public long VersionNo { get; set; }
    }

    public class CreateDepartmentRequest
    {
        public long TenantId { get; set; }
        public long BusinessUnitId { get; set; }
        public string DepartmentCode { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public long? ParentDepartmentId { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateDepartmentRequest
    {
        public long DepartmentId { get; set; }
        public long TenantId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public long? ParentDepartmentId { get; set; }
        public long ModifiedBy { get; set; }
    }
}
