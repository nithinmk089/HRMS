import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Attendance, LeaveBalance, LeaveRequest, LeaveType } from '../../core/models/time-attendance.models';

@Component({
  selector: 'app-self-service-time',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './self-service-time.component.html',
  styleUrl: './self-service-time.component.scss'
})
export class SelfServiceTimeComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  // Constants / Session context mocks
  currentEmployeeId = 1; // Assuming employee id is 1 for self-service context mock
  currentTenantId = 1;

  attendanceRecords: Attendance[] = [];
  leaveBalances: LeaveBalance[] = [];
  leaveRequests: LeaveRequest[] = [];
  leaveTypes: LeaveType[] = [];

  leaveForm!: FormGroup;
  regularizationForm!: FormGroup;
  showLeaveModal = false;
  showRegModal = false;
  saving = false;
  loading = false;
  toastMessage = '';

  constructor() {
    this.initForms();
  }

  ngOnInit() {
    this.loadLeaveTypes();
    this.loadSelfData();
  }

  initForms() {
    this.leaveForm = this.fb.group({
      leaveTypeId: ['', [Validators.required]],
      fromDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      toDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      totalDays: [1, [Validators.required, Validators.min(0.5)]],
      reason: ['', [Validators.required, Validators.maxLength(500)]]
    });

    this.regularizationForm = this.fb.group({
      requestedDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      reason: ['', [Validators.required, Validators.maxLength(500)]]
    });
  }

  loadLeaveTypes() {
    this.api.getLeaveTypes(this.currentTenantId).subscribe({
      next: (res) => this.leaveTypes = res.data || []
    });
  }

  loadSelfData() {
    this.loading = true;
    this.api.getSelfAttendance(this.currentEmployeeId, this.currentTenantId).subscribe({
      next: (res) => this.attendanceRecords = res.data || []
    });

    this.api.getSelfLeaveBalances(this.currentEmployeeId, this.currentTenantId).subscribe({
      next: (res) => this.leaveBalances = res.data || []
    });

    this.api.getSelfLeaveHistory(this.currentEmployeeId, this.currentTenantId).subscribe({
      next: (res) => {
        this.leaveRequests = res.data || [];
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  markClockIn() {
    const payload = {
      tenantId: this.currentTenantId,
      employeeId: this.currentEmployeeId,
      shiftId: 1, // General shift default
      attendanceDate: new Date().toISOString().split('T')[0],
      clockInTime: new Date().toISOString(),
      attendanceStatus: 'Present'
    };

    this.api.clockIn(payload).subscribe({
      next: (res) => {
        if (res.success) {
          this.showToast('Clock in successful.');
          this.loadSelfData();
        }
      }
    });
  }

  markClockOut() {
    // Find today's clock in record if exists
    const todayStr = new Date().toISOString().split('T')[0];
    const todayRec = this.attendanceRecords.find(r => r.attendanceDate.toString().startsWith(todayStr));
    
    if (!todayRec) {
      alert('No clock in record found for today.');
      return;
    }

    const clockOutTime = new Date();
    const clockInTime = todayRec.clockInTime ? new Date(todayRec.clockInTime) : new Date();
    const workingMin = Math.round((clockOutTime.getTime() - clockInTime.getTime()) / 60000);

    const payload = {
      attendanceId: todayRec.attendanceId,
      tenantId: this.currentTenantId,
      clockOutTime: clockOutTime.toISOString(),
      workingMinutes: workingMin,
      attendanceStatus: 'Present'
    };

    this.api.clockOut(payload).subscribe({
      next: (res) => {
        if (res.success) {
          this.showToast('Clock out successful.');
          this.loadSelfData();
        }
      }
    });
  }

  openLeaveModal() {
    this.leaveForm.reset({
      fromDate: new Date().toISOString().split('T')[0],
      toDate: new Date().toISOString().split('T')[0],
      totalDays: 1
    });
    this.showLeaveModal = true;
  }

  closeLeaveModal() {
    this.showLeaveModal = false;
  }

  submitLeave() {
    if (this.leaveForm.invalid) return;
    this.saving = true;

    const payload = {
      ...this.leaveForm.value,
      employeeId: this.currentEmployeeId,
      tenantId: this.currentTenantId
    };

    this.api.submitSelfLeaveRequest(payload).subscribe({
      next: (res) => {
        this.saving = false;
        if (res.success) {
          this.showToast('Leave request submitted successfully.');
          this.closeLeaveModal();
          this.loadSelfData();
        }
      },
      error: () => this.saving = false
    });
  }

  openRegModal() {
    this.regularizationForm.reset({
      requestedDate: new Date().toISOString().split('T')[0]
    });
    this.showRegModal = true;
  }

  closeRegModal() {
    this.showRegModal = false;
  }

  submitRegularization() {
    if (this.regularizationForm.invalid) return;
    this.saving = true;

    const payload = {
      ...this.regularizationForm.value,
      employeeId: this.currentEmployeeId,
      tenantId: this.currentTenantId
    };

    this.api.submitSelfRegularization(payload).subscribe({
      next: (res) => {
        this.saving = false;
        if (res.success) {
          this.showToast('Regularization request submitted.');
          this.closeRegModal();
          this.loadSelfData();
        }
      },
      error: () => this.saving = false
    });
  }

  showToast(msg: string) {
    this.toastMessage = msg;
    setTimeout(() => this.toastMessage = '', 3000);
  }
}
