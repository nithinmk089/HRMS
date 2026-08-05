namespace HRMS.Application.DTOs
{
    public class LocationDto
    {
        public long LocationId { get; set; }
        public long TenantId { get; set; }
        public string LocationCode { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public string? CountryCode { get; set; }
        public string? StateCode { get; set; }
        public string? City { get; set; }
        public long VersionNo { get; set; }
    }

    public class CreateLocationRequest
    {
        public long TenantId { get; set; }
        public string LocationCode { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public string? CountryCode { get; set; }
        public string? StateCode { get; set; }
        public string? City { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateLocationRequest
    {
        public long LocationId { get; set; }
        public long TenantId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string? CountryCode { get; set; }
        public string? StateCode { get; set; }
        public string? City { get; set; }
        public long ModifiedBy { get; set; }
    }
}
