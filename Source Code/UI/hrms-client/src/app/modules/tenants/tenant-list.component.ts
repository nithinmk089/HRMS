import { Component, OnInit, inject } from '@angular/core';
import { DatePipe, NgClass } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Tenant } from '../../core/models/phase01.models';
import { TenantCreateModalComponent } from '../../shared/components/modals/tenant-create-modal/tenant-create-modal.component';
import { TruncatePipe } from '../../shared/pipes/truncate.pipe';

@Component({
  selector: 'app-tenant-list',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, DatePipe, NgClass, TenantCreateModalComponent, TruncatePipe],
  templateUrl: './tenant-list.component.html',
  styleUrl: './tenant-list.component.scss'
})
export class TenantListComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  tenants: Tenant[] = [];
  isLoading = true;
  saving = false;
  submitted = false;

  searchText = '';
  statusFilter = '';
  page = 1;
  pageSize = 10;

  showModal = false;
  editMode = false;
  selectedTenantId: number | null = null;
  selectedTenant: Tenant | null = null;
  tenantForm!: FormGroup;

  toastMessage = '';
  errorMessage = '';

  constructor() {
    this.initForm();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForm() {
    this.tenantForm = this.fb.group({
      tenantCode: ['', [Validators.required]],
      tenantName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
      effectiveFrom: [new Date().toISOString().substring(0, 10), [Validators.required]],
      effectiveTo: [''],
      status: ['Active']
    });
  }

  get f() {
    return this.tenantForm.controls;
  }

  loadTenants() {
    this.isLoading = true;
    this.api.getTenants(this.searchText, this.statusFilter || undefined, this.page, this.pageSize).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching tenants', err);
        this.isLoading = false;
        this.showToast('Failed to load tenants.');
      }
    });
  }

  resetFilters() {
    this.searchText = '';
    this.statusFilter = '';
    this.page = 1;
    this.loadTenants();
  }

  prevPage() {
    if (this.page > 1) {
      this.page--;
      this.loadTenants();
    }
  }

  nextPage() {
    this.page++;
    this.loadTenants();
  }

  openCreateModal() {
    this.editMode = false;
    this.selectedTenantId = null;
    this.selectedTenant = null;
    this.errorMessage = '';
    this.showModal = true;
  }

  openEditModal(tenant: Tenant) {
    this.editMode = true;
    this.selectedTenantId = tenant.tenantId;
    this.selectedTenant = tenant;
    this.errorMessage = '';
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.selectedTenant = null;
  }

  onSaveTenant(formData: any) {
    this.saving = true;
    this.errorMessage = '';

    if (!formData.effectiveTo) {
      formData.effectiveTo = null;
    }

    if (this.editMode && this.selectedTenantId) {
      this.api.updateTenant(this.selectedTenantId, formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Tenant updated successfully.');
            this.closeModal();
            this.loadTenants();
          } else {
            this.errorMessage = res.message || 'Failed to update tenant.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred while saving.';
          console.error(err);
        }
      });
    } else {
      this.api.createTenant(formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Tenant created successfully.');
            this.closeModal();
            this.loadTenants();
          } else {
            this.errorMessage = res.message || 'Failed to create tenant.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred while saving.';
          console.error(err);
        }
      });
    }
  }

  toggleActivation(tenant: Tenant, activate: boolean) {
    const action = activate ? this.api.activateTenant(tenant.tenantId) : this.api.deactivateTenant(tenant.tenantId);
    action.subscribe({
      next: (res) => {
        if (res.success) {
          this.showToast(`Tenant ${activate ? 'activated' : 'deactivated'} successfully.`);
          this.loadTenants();
        } else {
          this.showToast(res.message || 'Action failed.');
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('An error occurred.');
      }
    });
  }

  deleteTenant(id: number) {
    if (confirm('Are you sure you want to soft delete this tenant? This will set its active status to false.')) {
      this.api.deleteTenant(id).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Tenant deleted successfully.');
            this.loadTenants();
          } else {
            this.showToast(res.message || 'Delete failed.');
          }
        },
        error: (err) => {
          console.error(err);
          this.showToast('An error occurred.');
        }
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
