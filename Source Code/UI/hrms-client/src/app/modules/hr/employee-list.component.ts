import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ApiService } from '../../core/services/api.service';
import { Tenant, Company, BusinessUnit, Department, Designation, Location, CostCenter } from '../../core/models/phase01.models';
import { Employee, EmployeeDirectoryDto } from '../../core/models/employee.models';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule],
  templateUrl: './employee-list.component.html',
  styleUrl: './employee-list.component.scss'
})
export class EmployeeListComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);
  private router = inject(Router);

  tenants: Tenant[] = [];
  companies: Company[] = [];
  businessUnits: BusinessUnit[] = [];
  departments: Department[] = [];
  designations: Designation[] = [];
  locations: Location[] = [];
  costCenters: CostCenter[] = [];
  employees: EmployeeDirectoryDto[] = [];

  // Filter bindings
  tenantIdFilter = 0;
  searchText = '';
  statusFilter = '';
  page = 1;
  pageSize = 50;

  // Headcount KPI metrics
  totalCount = 0;
  activeCount = 0;
  suspendedCount = 0;
  terminatedCount = 0;

  isLoading = false;
  saving = false;
  submitted = false;

  // Add/Edit Modal
  showModal = false;
  editMode = false;
  selectedEmpId: number | null = null;
  empForm!: FormGroup;
  errorMessage = '';
  toastMessage = '';

  constructor() {
    this.initForm();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForm() {
    this.empForm = this.fb.group({
      tenantId: ['', [Validators.required]],
      employeeCode: ['', [Validators.required, Validators.maxLength(50), Validators.pattern('^[A-Za-z0-9-]+$')]],
      employeeNumber: ['', [Validators.required, Validators.maxLength(50)]],
      firstName: ['', [Validators.required, Validators.maxLength(100)]],
      middleName: ['', [Validators.maxLength(100)]],
      lastName: ['', [Validators.required, Validators.maxLength(100)]],
      preferredName: ['', [Validators.maxLength(100)]],
      gender: ['Male'],
      dateOfBirth: [''],
      maritalStatus: ['Single'],
      nationality: ['Indian'],
      personalEmail: ['', [Validators.email]],
      mobileNumber: ['', [Validators.maxLength(20)]],
      status: ['Active', [Validators.required]]
    });
  }

  get f() {
    return this.empForm.controls;
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.tenantIdFilter = this.tenants[0].tenantId;
          this.empForm.patchValue({ tenantId: this.tenantIdFilter });
          this.loadMetadata(this.tenantIdFilter);
          this.loadEmployees();
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to load tenants.');
      }
    });
  }

  loadMetadata(tenantId: number) {
    // Parallel fetching of metadata
    this.api.getCompanies(tenantId, undefined, 1, 100).subscribe(res => this.companies = res.data || []);
    this.api.getBusinessUnits(tenantId).subscribe(res => this.businessUnits = res.data || []);
    this.api.getDepartments(tenantId).subscribe(res => this.departments = res.data || []);
    this.api.getDesignations(tenantId).subscribe(res => this.designations = res.data || []);
    this.api.getLocations(tenantId).subscribe(res => this.locations = res.data || []);
    this.api.getCostCenters(tenantId).subscribe(res => this.costCenters = res.data || []);
  }

  loadEmployees() {
    if (this.tenantIdFilter === 0) return;
    this.isLoading = true;
    this.api.getEmployees(this.tenantIdFilter, this.searchText, this.statusFilter || undefined, this.page, this.pageSize).subscribe({
      next: (res) => {
        this.employees = res.data || [];
        this.calculateMetrics();
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showToast('Failed to load employee list.');
      }
    });
  }

  calculateMetrics() {
    this.totalCount = this.employees.length;
    this.activeCount = this.employees.filter(e => e.status.toLowerCase() === 'active').length;
    this.suspendedCount = this.employees.filter(e => e.status.toLowerCase() === 'suspended').length;
    this.terminatedCount = this.employees.filter(e => e.status.toLowerCase() === 'terminated').length;
  }

  onTenantChange() {
    this.loadMetadata(this.tenantIdFilter);
    this.empForm.patchValue({ tenantId: this.tenantIdFilter });
    this.loadEmployees();
  }

  resetFilters() {
    this.searchText = '';
    this.statusFilter = '';
    this.page = 1;
    this.loadEmployees();
  }

  viewDetail(employeeId: number) {
    this.router.navigate(['/hr/employees', employeeId], { queryParams: { tenantId: this.tenantIdFilter } });
  }

  openCreateModal() {
    this.editMode = false;
    this.submitted = false;
    this.selectedEmpId = null;
    this.errorMessage = '';
    this.initForm();
    this.empForm.patchValue({ tenantId: this.tenantIdFilter });
    this.showModal = true;
  }

  openEditModal(emp: EmployeeDirectoryDto, event: Event) {
    event.stopPropagation(); // prevent row click routing
    this.editMode = true;
    this.submitted = false;
    this.selectedEmpId = emp.employeeID;
    this.errorMessage = '';

    // Need to fetch details for full edit form binding
    this.api.getEmployee(emp.employeeID, emp.tenantID).subscribe({
      next: (res) => {
        const full = res.data;
        if (full) {
          this.empForm.patchValue({
            tenantId: full.tenantID,
            employeeCode: full.employeeCode,
            employeeNumber: full.employeeNumber,
            firstName: full.firstName,
            middleName: full.middleName || '',
            lastName: full.lastName,
            preferredName: full.preferredName || '',
            gender: full.gender || 'Male',
            dateOfBirth: full.dateOfBirth ? new Date(full.dateOfBirth).toISOString().split('T')[0] : '',
            maritalStatus: full.maritalStatus || 'Single',
            nationality: full.nationality || 'Indian',
            personalEmail: full.personalEmail || '',
            mobileNumber: full.mobileNumber || '',
            status: full.employeeStatus || 'Active'
          });
          this.showModal = true;
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to load employee details for editing.');
      }
    });
  }

  closeModal() {
    this.showModal = false;
  }

  saveEmployee() {
    this.submitted = true;
    if (this.empForm.invalid) return;

    this.saving = true;
    this.errorMessage = '';
    const formData = { ...this.empForm.value };
    if (!formData.dateOfBirth) {
      formData.dateOfBirth = null;
    }

    if (this.editMode && this.selectedEmpId) {
      const updateData = { ...formData, employeeId: this.selectedEmpId };
      this.api.updateEmployee(this.selectedEmpId, updateData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Employee profile updated.');
            this.closeModal();
            this.loadEmployees();
          } else {
            this.errorMessage = res.message || 'Failed to update employee.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred while updating employee.';
          console.error(err);
        }
      });
    } else {
      this.api.createEmployee(formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Employee profile created.');
            this.closeModal();
            this.loadEmployees();
          } else {
            this.errorMessage = res.message || 'Failed to create employee.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred while creating employee.';
          console.error(err);
        }
      });
    }
  }

  deleteEmployee(emp: EmployeeDirectoryDto, event: Event) {
    event.stopPropagation();
    if (confirm(`Are you sure you want to delete employee: ${emp.firstName} ${emp.lastName}? This will perform a soft delete.`)) {
      this.api.deleteEmployee(emp.employeeID, emp.tenantID).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Employee deleted.');
            this.loadEmployees();
          } else {
            this.showToast(res.message || 'Failed to delete employee.');
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
