import { Component, OnInit, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Company, Tenant } from '../../core/models/phase01.models';
import { CompanyCreateModalComponent } from '../../shared/components/modals/company-create-modal/company-create-modal.component';
import { TruncatePipe } from '../../shared/pipes/truncate.pipe';

@Component({
  selector: 'app-company-list',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CompanyCreateModalComponent, TruncatePipe],
  templateUrl: './company-list.component.html',
  styleUrl: './company-list.component.scss'
})
export class CompanyListComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  tenants: Tenant[] = [];
  companies: Company[] = [];
  
  tenantIdFilter = 0;
  searchText = '';
  page = 1;
  pageSize = 10;
  
  isLoading = false;
  saving = false;
  submitted = false;

  showModal = false;
  editMode = false;
  selectedCompanyId: number | null = null;
  selectedCompany: Company | null = null;
  companyForm!: FormGroup;

  toastMessage = '';
  errorMessage = '';

  constructor() {
    this.initForm();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForm() {
    this.companyForm = this.fb.group({
      tenantId: ['', [Validators.required]],
      companyCode: ['', [Validators.required]],
      companyName: ['', [Validators.required, Validators.maxLength(200)]],
      legalName: [''],
      taxNumber: [''],
      email: ['', [Validators.email]],
      phone: [''],
      website: ['']
    });
  }

  get f() {
    return this.companyForm.controls;
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.tenantIdFilter = this.tenants[0].tenantId;
          this.loadCompanies();
        }
      },
      error: (err) => {
        console.error('Error loading tenants', err);
        this.showToast('Could not load tenants.');
      }
    });
  }

  loadCompanies() {
    if (this.tenantIdFilter === 0) return;
    this.isLoading = true;
    this.api.getCompanies(this.tenantIdFilter, this.searchText, this.page, this.pageSize).subscribe({
      next: (res) => {
        this.companies = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching companies', err);
        this.isLoading = false;
        this.showToast('Failed to load companies.');
      }
    });
  }

  onTenantChange() {
    this.page = 1;
    this.loadCompanies();
  }

  resetFilters() {
    this.searchText = '';
    this.page = 1;
    if (this.tenants.length > 0) {
      this.tenantIdFilter = this.tenants[0].tenantId;
    }
    this.loadCompanies();
  }

  prevPage() {
    if (this.page > 1) {
      this.page--;
      this.loadCompanies();
    }
  }

  nextPage() {
    this.page++;
    this.loadCompanies();
  }

  openCreateModal() {
    this.editMode = false;
    this.selectedCompanyId = null;
    this.selectedCompany = null;
    this.errorMessage = '';
    this.showModal = true;
  }

  openEditModal(company: Company) {
    this.editMode = true;
    this.selectedCompanyId = company.companyId;
    this.selectedCompany = company;
    this.errorMessage = '';
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.selectedCompany = null;
  }

  onSaveCompany(formData: any) {
    this.saving = true;
    this.errorMessage = '';

    if (this.editMode && this.selectedCompanyId) {
      this.api.updateCompany(this.selectedCompanyId, formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Company updated successfully.');
            this.closeModal();
            this.loadCompanies();
          } else {
            this.errorMessage = res.message || 'Failed to update company.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred while saving.';
          console.error(err);
        }
      });
    } else {
      this.api.createCompany(formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Company created successfully.');
            this.closeModal();
            this.loadCompanies();
          } else {
            this.errorMessage = res.message || 'Failed to create company.';
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

  deleteCompany(company: Company) {
    if (confirm(`Are you sure you want to delete company: ${company.companyName}?`)) {
      this.api.deleteCompany(company.companyId, company.tenantId).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Company deleted successfully.');
            this.loadCompanies();
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
