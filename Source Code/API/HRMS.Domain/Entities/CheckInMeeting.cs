using System;

namespace HRMS.Domain.Entities
{
    public class CheckInMeeting
    {
        public long CheckInMeetingID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long ManagerID { get; set; }
        public DateTime MeetingDate { get; set; }
        public string? Notes { get; set; }

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
