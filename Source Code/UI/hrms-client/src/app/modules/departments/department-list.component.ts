import { Component, OnInit, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Department, BusinessUnit, Tenant } from '../../core/models/phase01.models';
import { DepartmentCreateModalComponent } from '../../shared/components/modals/department-create-modal/department-create-modal.component';
import { TruncatePipe } from '../../shared/pipes/truncate.pipe';

@Component({
  selector: 'app-department-list',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, DepartmentCreateModalComponent, TruncatePipe],
  templateUrl: './department-list.component.html',
  styleUrl: './department-list.component.scss'
})
export class DepartmentListComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  tenants: Tenant[] = [];
  businessUnits: BusinessUnit[] = [];
  formBusinessUnits: BusinessUnit[] = [];
  departments: Department[] = [];
  potentialParents: Department[] = [];

  tenantIdFilter = 0;
  buIdFilter = 0;
  searchText = '';

  isLoading = false;
  saving = false;
  submitted = false;

  showModal = false;
  editMode = false;
  selectedDeptId: number | null = null;
  selectedDept: Department | null = null;
  deptForm!: FormGroup;

  toastMessage = '';
  errorMessage = '';

  constructor() {
    this.initForm();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForm() {
    this.deptForm = this.fb.group({
      tenantId: ['', [Validators.required]],
      businessUnitId: ['', [Validators.required]],
      parentDepartmentId: [''],
      departmentCode: ['', [Validators.required]],
      departmentName: ['', [Validators.required]]
    });
  }

  get f() {
    return this.deptForm.controls;
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.tenantIdFilter = this.tenants[0].tenantId;
          this.loadBusinessUnits(this.tenantIdFilter);
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to load tenants.');
      }
    });
  }

  loadBusinessUnits(tenantId: number) {
    this.api.getBusinessUnits(tenantId, undefined, undefined).subscribe({
      next: (res) => {
        this.businessUnits = res.data || [];
        this.formBusinessUnits = [...this.businessUnits];
        this.buIdFilter = 0;
        this.loadDepartments();
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to load business units.');
      }
    });
  }

  loadDepartments() {
    if (this.tenantIdFilter === 0) return;
    this.isLoading = true;
    const buId = this.buIdFilter > 0 ? this.buIdFilter : undefined;

    this.api.getDepartments(this.tenantIdFilter, buId, this.searchText).subscribe({
      next: (res) => {
        this.departments = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showToast('Failed to load departments.');
      }
    });
  }

  onTenantChange() {
    this.loadBusinessUnits(this.tenantIdFilter);
  }

  onFormTenantChange() {
    // Left for potential custom handling, though context loading is now inside the modal
  }

  getBuName(buId: number): string {
    const matched = this.businessUnits.find(b => b.businessUnitId === buId);
    return matched ? matched.businessUnitName : `BU ID: ${buId}`;
  }

  getParentDeptName(pid: number): string {
    const matched = this.departments.find(d => d.departmentId === pid);
    return matched ? matched.departmentName : `Dept ID: ${pid}`;
  }

  resetFilters() {
    this.searchText = '';
    this.buIdFilter = 0;
    if (this.tenants.length > 0) {
      this.tenantIdFilter = this.tenants[0].tenantId;
      this.loadBusinessUnits(this.tenantIdFilter);
    }
  }

  openCreateModal() {
    this.editMode = false;
    this.selectedDeptId = null;
    this.selectedDept = null;
    this.errorMessage = '';
    this.showModal = true;
  }

  openEditModal(dept: Department) {
    this.editMode = true;
    this.selectedDeptId = dept.departmentId;
    this.selectedDept = dept;
    this.errorMessage = '';
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.selectedDept = null;
  }

  onSaveDepartment(formData: any) {
    this.saving = true;
    this.errorMessage = '';

    if (!formData.parentDepartmentId) {
      formData.parentDepartmentId = null;
    } else {
      formData.parentDepartmentId = Number(formData.parentDepartmentId);
    }

    if (this.editMode && this.selectedDeptId) {
      this.api.updateDepartment(this.selectedDeptId, formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Department updated.');
            this.closeModal();
            this.loadDepartments();
          } else {
            this.errorMessage = res.message || 'Failed to update department.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred during save.';
          console.error(err);
        }
      });
    } else {
      this.api.createDepartment(formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Department created.');
            this.closeModal();
            this.loadDepartments();
          } else {
            this.errorMessage = res.message || 'Failed to create department.';
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

  deleteDepartment(dept: Department) {
    if (confirm(`Delete department: ${dept.departmentName}?`)) {
      this.api.deleteDepartment(dept.departmentId, dept.tenantId).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Department deleted.');
            this.loadDepartments();
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
