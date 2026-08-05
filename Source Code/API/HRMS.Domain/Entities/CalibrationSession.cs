using System;

namespace HRMS.Domain.Entities
{
    public class CalibrationSession
    {
        public long CalibrationSessionID { get; set; }
        public long TenantID { get; set; }
        public DateTime SessionDate { get; set; }
        public long FacilitatorID { get; set; }
        public string SessionStatus { get; set; } = "Draft";

        // Audit Columns
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}
