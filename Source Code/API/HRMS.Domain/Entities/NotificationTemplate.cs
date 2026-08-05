using System;

namespace HRMS.Domain.Entities
{
    public class NotificationTemplate
    {
        public long TemplateId { get; set; }
        public long TenantId { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string BusinessEvent { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string? SubjectTemplate { get; set; }
        public string BodyTemplate { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        // Audit Columns
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public long VersionNo { get; set; }
    }
}
