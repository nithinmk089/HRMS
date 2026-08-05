using System;

namespace HRMS.Application.DTOs
{
    // === SHIFT DTOs ===
    public class ShiftDto
    {
        public long ShiftId { get; set; }
        public long TenantId { get; set; }
        public string ShiftCode { get; set; } = string.Empty;
        public string ShiftName { get; set; } = string.Empty;
        public string ShiftType { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int GraceInMinutes { get; set; }
        public int GraceOutMinutes { get; set; }
        public bool IsFlexible { get; set; }
    }

    public class CreateShiftRequest
    {
        public long TenantId { get; set; }
        public string ShiftCode { get; set; } = string.Empty;
        public string ShiftName { get; set; } = string.Empty;
        public string ShiftType { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int GraceInMinutes { get; set; }
        public int GraceOutMinutes { get; set; }
        public bool IsFlexible { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateShiftRequest
    {
        public long ShiftId { get; set; }
        public long TenantId { get; set; }
        public string ShiftCode { get; set; } = string.Empty;
        public string ShiftName { get; set; } = string.Empty;
        public string ShiftType { get; set; } = string.Empty;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int GraceInMinutes { get; set; }
        public int GraceOutMinutes { get; set; }
        public bool IsFlexible { get; set; }
        public long ModifiedBy { get; set; }
    }

    // === SHIFT ASSIGNMENT DTOs ===
    public class ShiftAssignmentDto
    {
        public long ShiftAssignmentId { get; set; }
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public long ShiftId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }

    public class CreateShiftAssignmentRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public long ShiftId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public long CreatedBy { get; set; }
    }

    // === ATTENDANCE DTOs ===
    public class AttendanceDto
    {
        public long AttendanceId { get; set; }
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public long ShiftId { get; set; }
        public string ShiftCode { get; set; } = string.Empty;
        public string ShiftName { get; set; } = string.Empty;
        public DateTime AttendanceDate { get; set; }
        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public int? WorkingMinutes { get; set; }
        public string AttendanceStatus { get; set; } = string.Empty;
    }

    public class CreateAttendanceRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public long ShiftId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime? ClockInTime { get; set; }
        public string AttendanceStatus { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    public class UpdateAttendanceRequest
    {
        public long AttendanceId { get; set; }
        public long TenantId { get; set; }
        public DateTime ClockOutTime { get; set; }
        public int WorkingMinutes { get; set; }
        public string AttendanceStatus { get; set; } = string.Empty;
        public long ModifiedBy { get; set; }
    }

    // === ATTENDANCE ADJUSTMENT DTOs ===
    public class AttendanceAdjustmentDto
    {
        public long AttendanceAdjustmentId { get; set; }
        public long TenantId { get; set; }
        public long AttendanceId { get; set; }
        public string AdjustmentReason { get; set; } = string.Empty;
        public string? OriginalValue { get; set; }
        public string? NewValue { get; set; }
        public string ApprovalStatus { get; set; } = "Pending";
    }

    public class CreateAttendanceAdjustmentRequest
    {
        public long TenantId { get; set; }
        public long AttendanceId { get; set; }
        public string AdjustmentReason { get; set; } = string.Empty;
        public string? OriginalValue { get; set; }
        public string? NewValue { get; set; }
        public long CreatedBy { get; set; }
    }

    // === REGULARIZATION DTOs ===
    public class AttendanceRegularizationDto
    {
        public long AttendanceRegularizationId { get; set; }
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
    }

    public class CreateAttendanceRegularizationRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    // === HOLIDAY DTOs ===
    public class HolidayCalendarDto
    {
        public long HolidayCalendarId { get; set; }
        public long TenantId { get; set; }
        public string CalendarCode { get; set; } = string.Empty;
        public string CalendarName { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
    }

    public class CreateHolidayCalendarRequest
    {
        public long TenantId { get; set; }
        public string CalendarCode { get; set; } = string.Empty;
        public string CalendarName { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    public class HolidayDto
    {
        public long HolidayId { get; set; }
        public long TenantId { get; set; }
        public long HolidayCalendarId { get; set; }
        public DateTime HolidayDate { get; set; }
        public string HolidayName { get; set; } = string.Empty;
        public string HolidayType { get; set; } = string.Empty;
    }

    public class CreateHolidayRequest
    {
        public long TenantId { get; set; }
        public long HolidayCalendarId { get; set; }
        public DateTime HolidayDate { get; set; }
        public string HolidayName { get; set; } = string.Empty;
        public string HolidayType { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    // === LEAVE TYPE DTOs ===
    public class LeaveTypeDto
    {
        public long LeaveTypeId { get; set; }
        public long TenantId { get; set; }
        public string LeaveCode { get; set; } = string.Empty;
        public string LeaveName { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
        public bool IsAccrualBased { get; set; }
    }

    public class CreateLeaveTypeRequest
    {
        public long TenantId { get; set; }
        public string LeaveCode { get; set; } = string.Empty;
        public string LeaveName { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
        public bool IsAccrualBased { get; set; }
        public long CreatedBy { get; set; }
    }

    // === LEAVE POLICY DTOs ===
    public class LeavePolicyDto
    {
        public long LeavePolicyId { get; set; }
        public long TenantId { get; set; }
        public string PolicyName { get; set; } = string.Empty;
        public long LeaveTypeId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }

    public class CreateLeavePolicyRequest
    {
        public long TenantId { get; set; }
        public string PolicyName { get; set; } = string.Empty;
        public long LeaveTypeId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public long CreatedBy { get; set; }
    }

    // === LEAVE BALANCE DTOs ===
    public class LeaveBalanceDto
    {
        public long LeaveBalanceId { get; set; }
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public long LeaveTypeId { get; set; }
        public string LeaveCode { get; set; } = string.Empty;
        public string LeaveName { get; set; } = string.Empty;
        public decimal OpeningBalance { get; set; }
        public decimal AccruedBalance { get; set; }
        public decimal ConsumedBalance { get; set; }
        public decimal AvailableBalance { get; set; }
    }

    // === LEAVE REQUEST DTOs ===
    public class LeaveRequestDto
    {
        public long LeaveRequestId { get; set; }
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public long LeaveTypeId { get; set; }
        public string LeaveCode { get; set; } = string.Empty;
        public string LeaveName { get; set; } = string.Empty;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalDays { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string RequestStatus { get; set; } = "Pending";
        public DateTime CreatedDate { get; set; }
    }

    public class CreateLeaveRequestRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalDays { get; set; }
        public string Reason { get; set; } = string.Empty;
        public long CreatedBy { get; set; }
    }

    // === LEAVE ENCASHMENT DTOs ===
    public class LeaveEncashmentDto
    {
        public long LeaveEncashmentId { get; set; }
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }
        public decimal EncashedDays { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending";
    }

    public class CreateLeaveEncashmentRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public long LeaveTypeId { get; set; }
        public decimal EncashedDays { get; set; }
        public decimal Amount { get; set; }
        public long CreatedBy { get; set; }
    }

    // === OVERTIME REQUEST DTOs ===
    public class OvertimeRequestDto
    {
        public long OvertimeRequestId { get; set; }
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime OvertimeDate { get; set; }
        public decimal RequestedHours { get; set; }
        public string Status { get; set; } = "Pending";
    }

    public class CreateOvertimeRequestRequest
    {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public DateTime OvertimeDate { get; set; }
        public decimal RequestedHours { get; set; }
        public long CreatedBy { get; set; }
    }
}
