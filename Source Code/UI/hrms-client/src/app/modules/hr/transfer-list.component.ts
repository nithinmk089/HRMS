import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Tenant, Department, Location } from '../../core/models/phase01.models';
import { EmployeeTransfer, EmployeeDirectoryDto } from '../../core/models/employee.models';

@Component({
  selector: 'app-transfer-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './transfer-list.component.html',
  styleUrl: './transfer-list.component.scss'
})
export class TransferListComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  tenants: Tenant[] = [];
  employees: EmployeeDirectoryDto[] = [];
  departments: Department[] = [];
  locations: Location[] = [];
  transfers: EmployeeTransfer[] = [];

  tenantIdFilter = 0;
  employeeIdFilter: number | null = null;
  searchText = '';
  isLoading = false;
  saving = false;
  submitted = false;
  
  showModal = false;
  transferForm!: FormGroup;
  errorMessage = '';
  toastMessage = '';

  constructor() {
    this.initForm();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForm() {
    this.transferForm = this.fb.group({
      tenantId: ['', [Validators.required]],
      employeeId: ['', [Validators.required]],
      fromDepartmentId: ['', [Validators.required]],
      toDepartmentId: ['', [Validators.required]],
      fromLocationId: ['', [Validators.required]],
      toLocationId: ['', [Validators.required]],
      effectiveDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      reason: ['']
    });
  }

  get f() {
    return this.transferForm.controls;
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.tenantIdFilter = this.tenants[0].tenantId;
          this.transferForm.patchValue({ tenantId: this.tenantIdFilter });
          this.loadMetadata(this.tenantIdFilter);
          this.loadTransfers();
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to load tenants.');
      }
    });
  }

  loadMetadata(tenantId: number) {
    this.api.getEmployees(tenantId, undefined, 'Active', 1, 1000).subscribe(res => this.employees = res.data || []);
    this.api.getDepartments(tenantId).subscribe(res => this.departments = res.data || []);
    this.api.getLocations(tenantId).subscribe(res => this.locations = res.data || []);
  }

  loadTransfers() {
    if (this.tenantIdFilter === 0) return;
    this.isLoading = true;
    const empId = this.employeeIdFilter || undefined;
    this.api.getTransferHistoryReport(this.tenantIdFilter, empId).subscribe({
      next: (res) => {
        let list = res.data || [];
        if (this.searchText) {
          const searchLower = this.searchText.toLowerCase().trim();
          list = list.filter((t: any) =>
            (t.employeeFullName && t.employeeFullName.toLowerCase().includes(searchLower)) ||
            (t.employeeCode && t.employeeCode.toLowerCase().includes(searchLower))
          );
        }
        this.transfers = list;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showToast('Failed to load transfer history.');
      }
    });
  }

  onTenantChange() {
    this.loadMetadata(this.tenantIdFilter);
    this.transferForm.patchValue({ tenantId: this.tenantIdFilter });
    this.loadTransfers();
  }

  onEmployeeSelectChange() {
    const selectedEmpId = Number(this.transferForm.value.employeeId);
    if (!selectedEmpId) return;

    // Fetch details to pre-populate 'from' department and location
    this.api.getEmployee(selectedEmpId, this.tenantIdFilter).subscribe({
      next: (res) => {
        const emp = res.data;
        if (emp) {
          this.transferForm.patchValue({
            fromDepartmentId: emp.departmentID || '',
            fromLocationId: emp.locationID || ''
          });
        }
      }
    });
  }

  resetFilters() {
    this.employeeIdFilter = null;
    this.searchText = '';
    this.loadTransfers();
  }

  openCreateModal() {
    this.submitted = false;
    this.errorMessage = '';
    this.initForm();
    this.transferForm.patchValue({ tenantId: this.tenantIdFilter });
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  saveTransfer() {
    this.submitted = true;
    if (this.transferForm.invalid) return;

    this.saving = true;
    this.errorMessage = '';
    const formData = { ...this.transferForm.value };

    this.api.createTransfer(formData).subscribe({
      next: (res) => {
        this.saving = false;
        if (res.success) {
          this.showToast('Transfer request initiated.');
          this.closeModal();
          this.loadTransfers();
        } else {
          this.errorMessage = res.message || 'Failed to initiate transfer.';
        }
      },
      error: (err) => {
        this.saving = false;
        this.errorMessage = 'An error occurred.';
        console.error(err);
      }
    });
  }

  approveTransfer(t: EmployeeTransfer) {
    if (confirm(`Approve transfer for ${t.employeeFullName}?`)) {
      this.api.approveTransfer(t.employeeTransferID, t.tenantID).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Transfer request approved.');
            this.loadTransfers();
          } else {
            this.showToast(res.message || 'Approval failed.');
          }
        },
        error: (err) => console.error(err)
      });
    }
  }

  completeTransfer(t: EmployeeTransfer) {
    if (confirm(`Complete transfer execution for ${t.employeeFullName}? This will update department and location parameters immediately.`)) {
      this.api.completeTransfer(t.employeeTransferID, t.tenantID).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Transfer completed and executed.');
            this.loadTransfers();
          } else {
            this.showToast(res.message || 'Completion failed.');
          }
        },
        error: (err) => console.error(err)
      });
    }
  }

  showToast(msg: string) {
    this.toastMessage = msg;
    setTimeout(() => {
      this.toastMessage = '';
    }, 3000);
  }
}
