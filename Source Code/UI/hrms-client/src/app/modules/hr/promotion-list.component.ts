import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Tenant, Designation } from '../../core/models/phase01.models';
import { EmployeePromotion, EmployeeDirectoryDto } from '../../core/models/employee.models';

@Component({
  selector: 'app-promotion-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './promotion-list.component.html',
  styleUrl: './promotion-list.component.scss'
})
export class PromotionListComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  tenants: Tenant[] = [];
  employees: EmployeeDirectoryDto[] = [];
  designations: Designation[] = [];
  promotions: EmployeePromotion[] = [];

  tenantIdFilter = 0;
  employeeIdFilter: number | null = null;
  searchText = '';
  isLoading = false;
  saving = false;
  submitted = false;

  showModal = false;
  promotionForm!: FormGroup;
  errorMessage = '';
  toastMessage = '';

  constructor() {
    this.initForm();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForm() {
    this.promotionForm = this.fb.group({
      tenantId: ['', [Validators.required]],
      employeeId: ['', [Validators.required]],
      oldDesignationId: ['', [Validators.required]],
      newDesignationId: ['', [Validators.required]],
      oldGrade: [''],
      newGrade: [''],
      effectiveDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      reason: ['']
    });
  }

  get f() {
    return this.promotionForm.controls;
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.tenantIdFilter = this.tenants[0].tenantId;
          this.promotionForm.patchValue({ tenantId: this.tenantIdFilter });
          this.loadMetadata(this.tenantIdFilter);
          this.loadPromotions();
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
    this.api.getDesignations(tenantId).subscribe(res => this.designations = res.data || []);
  }

  loadPromotions() {
    if (this.tenantIdFilter === 0) return;
    this.isLoading = true;
    const empId = this.employeeIdFilter || undefined;
    this.api.getPromotionHistoryReport(this.tenantIdFilter, empId).subscribe({
      next: (res) => {
        let list = res.data || [];
        if (this.searchText) {
          const searchLower = this.searchText.toLowerCase().trim();
          list = list.filter((p: any) =>
            (p.employeeFullName && p.employeeFullName.toLowerCase().includes(searchLower)) ||
            (p.employeeCode && p.employeeCode.toLowerCase().includes(searchLower))
          );
        }
        this.promotions = list;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showToast('Failed to load promotions.');
      }
    });
  }

  onTenantChange() {
    this.loadMetadata(this.tenantIdFilter);
    this.promotionForm.patchValue({ tenantId: this.tenantIdFilter });
    this.loadPromotions();
  }

  onEmployeeSelectChange() {
    const selectedEmpId = Number(this.promotionForm.value.employeeId);
    if (!selectedEmpId) return;

    this.api.getEmployee(selectedEmpId, this.tenantIdFilter).subscribe({
      next: (res) => {
        const emp = res.data;
        if (emp) {
          this.promotionForm.patchValue({
            oldDesignationId: emp.designationID || '',
            oldGrade: emp.grade || ''
          });
        }
      }
    });
  }

  resetFilters() {
    this.employeeIdFilter = null;
    this.searchText = '';
    this.loadPromotions();
  }

  openCreateModal() {
    this.submitted = false;
    this.errorMessage = '';
    this.initForm();
    this.promotionForm.patchValue({ tenantId: this.tenantIdFilter });
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  savePromotion() {
    this.submitted = true;
    if (this.promotionForm.invalid) return;

    this.saving = true;
    this.errorMessage = '';
    const formData = { ...this.promotionForm.value };

    this.api.createPromotion(formData).subscribe({
      next: (res) => {
        this.saving = false;
        if (res.success) {
          this.showToast('Promotion request initiated.');
          this.closeModal();
          this.loadPromotions();
        } else {
          this.errorMessage = res.message || 'Failed to initiate promotion.';
        }
      },
      error: (err) => {
        this.saving = false;
        this.errorMessage = 'An error occurred.';
        console.error(err);
      }
    });
  }

  approvePromotion(p: EmployeePromotion) {
    if (confirm(`Approve promotion for ${p.employeeFullName}?`)) {
      this.api.approvePromotion(p.employeePromotionID, p.tenantID).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Promotion request approved.');
            this.loadPromotions();
          } else {
            this.showToast(res.message || 'Approval failed.');
          }
        },
        error: (err) => console.error(err)
      });
    }
  }

  completePromotion(p: EmployeePromotion) {
    if (confirm(`Complete promotion execution for ${p.employeeFullName}? This will update designation and grade parameters immediately.`)) {
      this.api.completePromotion(p.employeePromotionID, p.tenantID).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Promotion completed and executed.');
            this.loadPromotions();
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
