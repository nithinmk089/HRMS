import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Tenant } from '../../core/models/phase01.models';
import { LeaveType, LeavePolicy, LeaveRequest } from '../../core/models/time-attendance.models';
import { EmployeeDirectoryDto } from '../../core/models/employee.models';

@Component({
  selector: 'app-leave-management',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './leave-management.component.html',
  styleUrl: './leave-management.component.scss'
})
export class LeaveManagementComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  tenants: Tenant[] = [];
  leaveTypes: LeaveType[] = [];
  leaveRequests: LeaveRequest[] = [];
  selectedTenantId = 0;

  typeForm!: FormGroup;
  policyForm!: FormGroup;
  showTypeModal = false;
  showPolicyModal = false;
  saving = false;
  loading = false;
  toastMessage = '';
  errorMessage = '';

  constructor() {
    this.initForms();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForms() {
    this.typeForm = this.fb.group({
      leaveCode: ['', [Validators.required, Validators.maxLength(50)]],
      leaveName: ['', [Validators.required, Validators.maxLength(100)]],
      isPaid: [true],
      isAccrualBased: [true]
    });

    this.policyForm = this.fb.group({
      policyName: ['', [Validators.required, Validators.maxLength(100)]],
      leaveTypeId: ['', [Validators.required]],
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
          this.loadLeaveTypes();
          this.loadLeaveRequests();
        }
      }
    });
  }

  loadLeaveTypes() {
    if (!this.selectedTenantId) return;
    this.api.getLeaveTypes(this.selectedTenantId).subscribe({
      next: (res) => this.leaveTypes = res.data || []
    });
  }

  loadLeaveRequests() {
    if (!this.selectedTenantId) return;
    this.loading = true;
    this.api.getLeaveRequests(this.selectedTenantId).subscribe({
      next: (res) => {
        this.leaveRequests = res.data || [];
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  onTenantChange() {
    this.loadLeaveTypes();
    this.loadLeaveRequests();
  }

  openTypeModal() {
    this.errorMessage = '';
    this.typeForm.reset({
      isPaid: true,
      isAccrualBased: true
    });
    this.showTypeModal = true;
  }

  closeTypeModal() {
    this.showTypeModal = false;
  }

  saveLeaveType() {
    if (this.typeForm.invalid) return;
    this.saving = true;
    this.errorMessage = '';
    const payload = {
      ...this.typeForm.value,
      tenantId: this.selectedTenantId
    };
    this.api.createLeaveType(payload).subscribe({
      next: (res) => {
        this.saving = false;
        if (res.success) {
          this.showToast('Leave type created successfully.');
          this.closeTypeModal();
          this.loadLeaveTypes();
        } else {
          this.errorMessage = res.message || 'Failed to create leave type.';
        }
      },
      error: () => this.saving = false
    });
  }

  openPolicyModal() {
    this.policyForm.reset({
      effectiveFrom: new Date().toISOString().split('T')[0],
      effectiveTo: ''
    });
    this.showPolicyModal = true;
  }

  closePolicyModal() {
    this.showPolicyModal = false;
  }

  savePolicy() {
    if (this.policyForm.invalid) return;
    this.saving = true;
    const payload = {
      ...this.policyForm.value,
      tenantId: this.selectedTenantId
    };
    this.api.createLeavePolicy(payload).subscribe({
      next: (res: any) => {
        this.saving = false;
        if (res.success) {
          this.showToast('Leave policy created successfully.');
          this.closePolicyModal();
        } else {
          alert(res.message || 'Failed to create policy.');
        }
      },
      error: () => this.saving = false
    });
  }

  approveLeave(request: LeaveRequest) {
    if (confirm(`Approve leave request for ${request.employeeName}?`)) {
      this.api.approveLeaveRequest(request.leaveRequestId, request.tenantId, 'Approved by admin').subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Leave request approved.');
            this.loadLeaveRequests();
          } else {
            alert(res.message || 'Failed to approve request.');
          }
        }
      });
    }
  }

  rejectLeave(request: LeaveRequest) {
    const remarks = prompt('Enter rejection remarks:');
    if (remarks !== null) {
      this.api.rejectLeaveRequest(request.leaveRequestId, request.tenantId, remarks).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Leave request rejected.');
            this.loadLeaveRequests();
          } else {
            alert(res.message || 'Failed to reject request.');
          }
        }
      });
    }
  }

  showToast(msg: string) {
    this.toastMessage = msg;
    setTimeout(() => this.toastMessage = '', 3000);
  }
}
