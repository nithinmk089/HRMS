import { Component, OnInit, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { CostCenter, Tenant } from '../../core/models/phase01.models';
import { TruncatePipe } from '../../shared/pipes/truncate.pipe';

@Component({
  selector: 'app-cost-center-list',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, TruncatePipe],
  templateUrl: './cost-center-list.component.html',
  styleUrl: './cost-center-list.component.scss'
})
export class CostCenterListComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  tenants: Tenant[] = [];
  costCenters: CostCenter[] = [];

  tenantIdFilter = 0;
  searchText = '';

  isLoading = false;
  saving = false;
  submitted = false;

  showModal = false;
  editMode = false;
  selectedCcId: number | null = null;
  ccForm!: FormGroup;

  toastMessage = '';
  errorMessage = '';

  constructor() {
    this.initForm();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForm() {
    this.ccForm = this.fb.group({
      tenantId: ['', [Validators.required]],
      costCenterCode: ['', [Validators.required]],
      costCenterName: ['', [Validators.required]]
    });
  }

  get f() {
    return this.ccForm.controls;
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.tenantIdFilter = this.tenants[0].tenantId;
          this.loadCostCenters();
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to load tenants.');
      }
    });
  }

  loadCostCenters() {
    if (this.tenantIdFilter === 0) return;
    this.isLoading = true;
    this.api.getCostCenters(this.tenantIdFilter, this.searchText).subscribe({
      next: (res) => {
        this.costCenters = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showToast('Failed to load cost centers.');
      }
    });
  }

  resetFilters() {
    this.searchText = '';
    if (this.tenants.length > 0) {
      this.tenantIdFilter = this.tenants[0].tenantId;
    }
    this.loadCostCenters();
  }

  openCreateModal() {
    this.editMode = false;
    this.submitted = false;
    this.selectedCcId = null;
    this.errorMessage = '';
    this.initForm();
    this.ccForm.patchValue({
      tenantId: this.tenantIdFilter
    });
    this.showModal = true;
  }

  openEditModal(cc: CostCenter) {
    this.editMode = true;
    this.submitted = false;
    this.selectedCcId = cc.costCenterId;
    this.errorMessage = '';

    this.ccForm.patchValue({
      tenantId: cc.tenantId,
      costCenterCode: cc.costCenterCode,
      costCenterName: cc.costCenterName
    });
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  saveCostCenter() {
    this.submitted = true;
    if (this.ccForm.invalid) return;

    this.saving = true;
    this.errorMessage = '';
    const formData = { ...this.ccForm.value };

    if (this.editMode && this.selectedCcId) {
      this.api.updateCostCenter(this.selectedCcId, formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Cost Center updated.');
            this.closeModal();
            this.loadCostCenters();
          } else {
            this.errorMessage = res.message || 'Failed to update cost center.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred during save.';
          console.error(err);
        }
      });
    } else {
      this.api.createCostCenter(formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Cost Center created.');
            this.closeModal();
            this.loadCostCenters();
          } else {
            this.errorMessage = res.message || 'Failed to create cost center.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred during save.';
          console.error(err);
        }
      });
    }
  }

  deleteCostCenter(cc: CostCenter) {
    if (confirm(`Delete cost center: ${cc.costCenterName}?`)) {
      this.api.deleteCostCenter(cc.costCenterId, cc.tenantId).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Cost center deleted.');
            this.loadCostCenters();
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
