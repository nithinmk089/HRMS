using System;

namespace HRMS.Domain.Entities
{
    public class UserNotificationPreference
    {
        public long PreferenceId { get; set; }
        public long TenantId { get; set; }
        public long UserID { get; set; }
        public string BusinessEvent { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }

        // Audit Columns
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
