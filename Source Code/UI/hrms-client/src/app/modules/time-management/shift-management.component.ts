import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Tenant } from '../../core/models/phase01.models';
import { Shift, ShiftAssignment } from '../../core/models/time-attendance.models';
import { EmployeeDirectoryDto } from '../../core/models/employee.models';

@Component({
  selector: 'app-shift-management',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './shift-management.component.html',
  styleUrl: './shift-management.component.scss'
})
export class ShiftManagementComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  tenants: Tenant[] = [];
  shifts: Shift[] = [];
  employees: EmployeeDirectoryDto[] = [];
  selectedTenantId = 0;

  shiftForm!: FormGroup;
  assignmentForm!: FormGroup;
  showShiftModal = false;
  showAssignModal = false;
  editingShift: Shift | null = null;
  saving = false;
  loading = false;
  errorMessage = '';
  toastMessage = '';

  constructor() {
    this.initForms();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForms() {
    this.shiftForm = this.fb.group({
      shiftCode: ['', [Validators.required, Validators.maxLength(50)]],
      shiftName: ['', [Validators.required, Validators.maxLength(100)]],
      shiftType: ['General', [Validators.required]],
      startTime: ['09:00:00', [Validators.required]],
      endTime: ['17:00:00', [Validators.required]],
      graceInMinutes: [0, [Validators.required, Validators.min(0)]],
      graceOutMinutes: [0, [Validators.required, Validators.min(0)]],
      isFlexible: [false]
    });

    this.assignmentForm = this.fb.group({
      employeeId: ['', [Validators.required]],
      shiftId: ['', [Validators.required]],
      effectiveFrom: [new Date().toISOString().split('T')[0], [Validators.required]],
      effectiveTo: ['']
    });
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.selectedTenantId = this.tenants[0].tenantId;
          this.loadShifts();
          this.loadEmployees();
        }
      }
    });
  }

  loadShifts() {
    if (!this.selectedTenantId) return;
    this.loading = true;
    this.api.getShifts(this.selectedTenantId).subscribe({
      next: (res) => {
        this.shifts = res.data || [];
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  loadEmployees() {
    if (!this.selectedTenantId) return;
    this.api.getEmployees(this.selectedTenantId, undefined, 'Active', 1, 1000).subscribe({
      next: (res) => this.employees = res.data || []
    });
  }

  onTenantChange() {
    this.loadShifts();
    this.loadEmployees();
  }

  openShiftModal(shift?: Shift) {
    this.errorMessage = '';
    this.editingShift = shift || null;
    if (shift) {
      this.shiftForm.patchValue({
        shiftCode: shift.shiftCode,
        shiftName: shift.shiftName,
        shiftType: shift.shiftType,
        startTime: shift.startTime,
        endTime: shift.endTime,
        graceInMinutes: shift.graceInMinutes,
        graceOutMinutes: shift.graceOutMinutes,
        isFlexible: shift.isFlexible
      });
    } else {
      this.shiftForm.reset({
        shiftType: 'General',
        startTime: '09:00:00',
        endTime: '17:00:00',
        graceInMinutes: 0,
        graceOutMinutes: 0,
        isFlexible: false
      });
    }
    this.showShiftModal = true;
  }

  closeShiftModal() {
    this.showShiftModal = false;
  }

  saveShift() {
    if (this.shiftForm.invalid) return;
    this.saving = true;
    this.errorMessage = '';

    const payload = {
      ...this.shiftForm.value,
      tenantId: this.selectedTenantId
    };

    if (this.editingShift) {
      this.api.updateShift(this.editingShift.shiftId, payload).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Shift updated successfully.');
            this.closeShiftModal();
            this.loadShifts();
          } else {
            this.errorMessage = res.message || 'Failed to update shift.';
          }
        },
        error: () => this.saving = false
      });
    } else {
      this.api.createShift(payload).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Shift created successfully.');
            this.closeShiftModal();
            this.loadShifts();
          } else {
            this.errorMessage = res.message || 'Failed to create shift.';
          }
        },
        error: () => this.saving = false
      });
    }
  }

  deleteShift(shift: Shift) {
    if (confirm(`Are you sure you want to delete shift ${shift.shiftName}?`)) {
      this.api.deleteShift(shift.shiftId, this.selectedTenantId).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Shift deleted.');
            this.loadShifts();
          }
        }
      });
    }
  }

  openAssignModal(shift: Shift) {
    this.assignmentForm.reset({
      shiftId: shift.shiftId,
      effectiveFrom: new Date().toISOString().split('T')[0],
      effectiveTo: ''
    });
    this.showAssignModal = true;
  }

  closeAssignModal() {
    this.showAssignModal = false;
  }

  saveAssignment() {
    if (this.assignmentForm.invalid) return;
    this.saving = true;
    const payload = {
      ...this.assignmentForm.value,
      tenantId: this.selectedTenantId
    };
    this.api.assignShift(payload).subscribe({
      next: (res) => {
        this.saving = false;
        if (res.success) {
          this.showToast('Shift assigned successfully.');
          this.closeAssignModal();
        } else {
          alert(res.message || 'Failed to assign shift.');
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
