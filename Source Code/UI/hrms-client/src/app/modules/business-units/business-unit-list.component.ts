import { Component, OnInit, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { BusinessUnit, Company, Tenant } from '../../core/models/phase01.models';
import { TruncatePipe } from '../../shared/pipes/truncate.pipe';

@Component({
  selector: 'app-business-unit-list',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, TruncatePipe],
  templateUrl: './business-unit-list.component.html',
  styleUrl: './business-unit-list.component.scss'
})
export class BusinessUnitListComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  tenants: Tenant[] = [];
  companies: Company[] = [];
  formCompanies: Company[] = [];
  businessUnits: BusinessUnit[] = [];

  tenantIdFilter = 0;
  companyIdFilter = 0;
  searchText = '';

  isLoading = false;
  saving = false;
  submitted = false;

  showModal = false;
  editMode = false;
  selectedBuId: number | null = null;
  buForm!: FormGroup;

  toastMessage = '';
  errorMessage = '';

  constructor() {
    this.initForm();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForm() {
    this.buForm = this.fb.group({
      tenantId: ['', [Validators.required]],
      companyId: ['', [Validators.required]],
      businessUnitCode: ['', [Validators.required]],
      businessUnitName: ['', [Validators.required]]
    });
  }

  get f() {
    return this.buForm.controls;
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.tenantIdFilter = this.tenants[0].tenantId;
          this.loadCompanies(this.tenantIdFilter);
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to load tenants.');
      }
    });
  }

  loadCompanies(tenantId: number) {
    this.api.getCompanies(tenantId, undefined, 1, 100).subscribe({
      next: (res) => {
        this.companies = res.data || [];
        this.formCompanies = [...this.companies];
        this.companyIdFilter = 0;
        this.loadBusinessUnits();
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to load companies.');
      }
    });
  }

  loadBusinessUnits() {
    if (this.tenantIdFilter === 0) return;
    this.isLoading = true;
    const cid = this.companyIdFilter > 0 ? this.companyIdFilter : undefined;

    this.api.getBusinessUnits(this.tenantIdFilter, cid, this.searchText).subscribe({
      next: (res) => {
        this.businessUnits = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showToast('Failed to load business units.');
      }
    });
  }

  onTenantChange() {
    this.loadCompanies(this.tenantIdFilter);
  }

  onFormTenantChange() {
    const tid = Number(this.buForm.value.tenantId);
    if (tid) {
      this.api.getCompanies(tid, undefined, 1, 100).subscribe({
        next: (res) => {
          this.formCompanies = res.data || [];
          if (this.formCompanies.length > 0) {
            this.buForm.patchValue({ companyId: this.formCompanies[0].companyId });
          } else {
            this.buForm.patchValue({ companyId: '' });
          }
        }
      });
    }
  }

  getCompanyName(companyId: number): string {
    const matched = this.companies.find(c => c.companyId === companyId);
    return matched ? matched.companyName : `Company ID: ${companyId}`;
  }

  resetFilters() {
    this.searchText = '';
    this.companyIdFilter = 0;
    if (this.tenants.length > 0) {
      this.tenantIdFilter = this.tenants[0].tenantId;
      this.loadCompanies(this.tenantIdFilter);
    }
  }

  openCreateModal() {
    this.editMode = false;
    this.submitted = false;
    this.selectedBuId = null;
    this.errorMessage = '';
    this.initForm();
    
    this.buForm.patchValue({
      tenantId: this.tenantIdFilter,
      companyId: this.companyIdFilter > 0 ? this.companyIdFilter : (this.companies.length > 0 ? this.companies[0].companyId : '')
    });
    this.formCompanies = [...this.companies];
    this.showModal = true;
  }

  openEditModal(bu: BusinessUnit) {
    this.editMode = true;
    this.submitted = false;
    this.selectedBuId = bu.businessUnitId;
    this.errorMessage = '';

    this.api.getCompanies(bu.tenantId, undefined, 1, 100).subscribe({
      next: (res) => {
        this.formCompanies = res.data || [];
        this.buForm.patchValue({
          tenantId: bu.tenantId,
          companyId: bu.companyId,
          businessUnitCode: bu.businessUnitCode,
          businessUnitName: bu.businessUnitName
        });
        this.showModal = true;
      }
    });
  }

  closeModal() {
    this.showModal = false;
  }

  saveBusinessUnit() {
    this.submitted = true;
    if (this.buForm.invalid) return;

    this.saving = true;
    this.errorMessage = '';
    const formData = { ...this.buForm.value };

    if (this.editMode && this.selectedBuId) {
      this.api.updateBusinessUnit(this.selectedBuId, formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Business Unit updated.');
            this.closeModal();
            this.loadBusinessUnits();
          } else {
            this.errorMessage = res.message || 'Failed to update business unit.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred during save.';
          console.error(err);
        }
      });
    } else {
      this.api.createBusinessUnit(formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Business Unit created.');
            this.closeModal();
            this.loadBusinessUnits();
          } else {
            this.errorMessage = res.message || 'Failed to create business unit.';
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

  deleteBusinessUnit(bu: BusinessUnit) {
    if (confirm(`Delete business unit ${bu.businessUnitName}?`)) {
      this.api.deleteBusinessUnit(bu.businessUnitId, bu.tenantId).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Business unit deleted.');
            this.loadBusinessUnits();
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
  }

  showToast(msg: string) {
    this.toastMessage = msg;
    setTimeout(() => {
      this.toastMessage = '';
    }, 3000);
  }
}
