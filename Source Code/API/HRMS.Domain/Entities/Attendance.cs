using System;

namespace HRMS.Domain.Entities
{
    public class HolidayCalendar
    {
        public long HolidayCalendarID { get; set; }
        public long TenantID { get; set; }
        public string CalendarCode { get; set; } = null!;
        public string CalendarName { get; set; } = null!;
        public string CountryCode { get; set; } = null!;

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

    public class Holiday
    {
        public long HolidayID { get; set; }
        public long TenantID { get; set; }
        public long HolidayCalendarID { get; set; }
        public DateTime HolidayDate { get; set; }
        public string HolidayName { get; set; } = null!;
        public string HolidayType { get; set; } = null!;

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

    public class Attendance
    {
        public long AttendanceID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public long ShiftID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public int? WorkingMinutes { get; set; }
        public string AttendanceStatus { get; set; } = null!;

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

    public class AttendanceAdjustment
    {
        public long AttendanceAdjustmentID { get; set; }
        public long TenantID { get; set; }
        public long AttendanceID { get; set; }
        public string AdjustmentReason { get; set; } = null!;
        public string? OriginalValue { get; set; }
        public string? NewValue { get; set; }
        public string ApprovalStatus { get; set; } = "Pending";

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

    public class AttendanceRegularization
    {
        public long AttendanceRegularizationID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Reason { get; set; } = null!;
        public string Status { get; set; } = "Pending";

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

    public class OvertimeRequest
    {
        public long OvertimeRequestID { get; set; }
        public long TenantID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime OvertimeDate { get; set; }
        public decimal RequestedHours { get; set; }
        public string Status { get; set; } = "Pending";

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

    public class OvertimeApproval
    {
        public long OvertimeApprovalID { get; set; }
        public long TenantID { get; set; }
        public long OvertimeRequestID { get; set; }
        public long ApproverID { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string? Remarks { get; set; }

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
