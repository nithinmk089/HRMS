export interface Shift {
  shiftId: number;
  tenantId: number;
  shiftCode: string;
  shiftName: string;
  shiftType: string;
  startTime: string;
  endTime: string;
  graceInMinutes: number;
  graceOutMinutes: number;
  isFlexible: boolean;
}

export interface ShiftAssignment {
  shiftAssignmentId: number;
  tenantId: number;
  employeeId: number;
  shiftId: number;
  effectiveFrom: string;
  effectiveTo?: string;
}

export interface Attendance {
  attendanceId: number;
  tenantId: number;
  employeeId: number;
  employeeCode: string;
  employeeName: string;
  shiftId: number;
  shiftCode: string;
  shiftName: string;
  attendanceDate: string;
  clockInTime?: string;
  clockOutTime?: string;
  workingMinutes?: number;
  attendanceStatus: string;
}

export interface AttendanceAdjustment {
  attendanceAdjustmentId: number;
  tenantId: number;
  attendanceId: number;
  adjustmentReason: string;
  originalValue?: string;
  newValue?: string;
  approvalStatus: string;
}

export interface AttendanceRegularization {
  attendanceRegularizationId: number;
  tenantId: number;
  employeeId: number;
  requestedDate: string;
  reason: string;
  status: string;
}

export interface HolidayCalendar {
  holidayCalendarId: number;
  tenantId: number;
  calendarCode: string;
  calendarName: string;
  countryCode: string;
}

export interface Holiday {
  holidayId: number;
  tenantId: number;
  holidayCalendarId: number;
  holidayDate: string;
  holidayName: string;
  holidayType: string;
}

export interface LeaveType {
  leaveTypeId: number;
  tenantId: number;
  leaveCode: string;
  leaveName: string;
  isPaid: boolean;
  isAccrualBased: boolean;
}

export interface LeavePolicy {
  leavePolicyId: number;
  tenantId: number;
  policyName: string;
  leaveTypeId: number;
  effectiveFrom: string;
  effectiveTo?: string;
}

export interface LeaveBalance {
  leaveBalanceId: number;
  tenantId: number;
  employeeId: number;
  employeeCode: string;
  employeeName: string;
  leaveTypeId: number;
  leaveCode: string;
  leaveName: string;
  openingBalance: number;
  accruedBalance: number;
  consumedBalance: number;
  availableBalance: number;
}

export interface LeaveRequest {
  leaveRequestId: number;
  tenantId: number;
  employeeId: number;
  employeeCode: string;
  employeeName: string;
  leaveTypeId: number;
  leaveCode: string;
  leaveName: string;
  fromDate: string;
  toDate: string;
  totalDays: number;
  reason: string;
  requestStatus: string;
  createdDate: string;
}

export interface LeaveEncashment {
  leaveEncashmentId: number;
  tenantId: number;
  employeeId: number;
  leaveTypeId: number;
  encashedDays: number;
  amount: number;
  status: string;
}

export interface OvertimeRequest {
  overtimeRequestId: number;
  tenantId: number;
  employeeId: number;
  employeeCode: string;
  employeeName: string;
  overtimeDate: string;
  requestedHours: number;
  status: string;
}
