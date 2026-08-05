import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Tenant } from '../../core/models/phase01.models';
import { OvertimeRequest } from '../../core/models/time-attendance.models';

@Component({
  selector: 'app-overtime-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './overtime-management.component.html',
  styleUrl: './overtime-management.component.scss'
})
export class OvertimeManagementComponent implements OnInit {
  private api = inject(ApiService);

  tenants: Tenant[] = [];
  overtimeRequests: OvertimeRequest[] = [];
  selectedTenantId = 0;
  statusFilter = '';

  loading = false;
  toastMessage = '';

  ngOnInit() {
    this.loadTenants();
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.selectedTenantId = this.tenants[0].tenantId;
          this.loadOvertimeRequests();
        }
      }
    });
  }

  loadOvertimeRequests() {
    if (!this.selectedTenantId) return;
    this.loading = true;
    const status = this.statusFilter || undefined;
    this.api.getOvertimeRequests(this.selectedTenantId, undefined, status).subscribe({
      next: (res) => {
        this.overtimeRequests = res.data || [];
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  onTenantChange() {
    this.loadOvertimeRequests();
  }

  approveRequest(req: OvertimeRequest) {
    if (confirm(`Approve ${req.requestedHours} hours overtime for ${req.employeeName}?`)) {
      this.api.approveOvertimeRequest(req.overtimeRequestId, req.tenantId, 'Approved').subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Overtime request approved.');
            this.loadOvertimeRequests();
          }
        }
      });
    }
  }

  rejectRequest(req: OvertimeRequest) {
    const remarks = prompt('Enter rejection remarks:');
    if (remarks !== null) {
      this.api.rejectOvertimeRequest(req.overtimeRequestId, req.tenantId, remarks).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Overtime request rejected.');
            this.loadOvertimeRequests();
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
