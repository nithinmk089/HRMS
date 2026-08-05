namespace HRMS.Application.DTOs
{
    public class ConfigurationDto
    {
        public long ConfigurationId { get; set; }
        public long TenantId { get; set; }
        public string ConfigurationKey { get; set; } = string.Empty;
        public string? ConfigurationValue { get; set; }
        public string DataType { get; set; } = string.Empty;
        public long VersionNo { get; set; }
    }

    public class CreateConfigurationRequest
    {
        public long TenantId { get; set; }
        public string ConfigurationKey { get; set; } = string.Empty;
        public string? ConfigurationValue { get; set; }
        public string DataType { get; set; } = "String";
        public long CreatedBy { get; set; }
    }

    public class UpdateConfigurationRequest
    {
        public long ConfigurationId { get; set; }
        public long TenantId { get; set; }
        public string? ConfigurationValue { get; set; }
        public long ModifiedBy { get; set; }
    }
}
