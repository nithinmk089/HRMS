namespace HRMS.Application.DTOs
{
    public class DesignationDto
    {
        public long DesignationId { get; set; }
        public long TenantId { get; set; }
        public string DesignationCode { get; set; } = string.Empty;
        public string DesignationName { get; set; } = string.Empty;
        public string? Grade { get; set; }
        public long VersionNo { get; set; }
    }

    public class CreateDesignationRequest
    {
        public long TenantId { get; set; }
        public string DesignationCode { get; set; } = string.Empty;
        public string DesignationName { get; set; } = string.Empty;
        public string? Grade { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateDesignationRequest
    {
        public long DesignationId { get; set; }
        public long TenantId { get; set; }
        public string DesignationName { get; set; } = string.Empty;
        public string? Grade { get; set; }
        public long ModifiedBy { get; set; }
    }
}
