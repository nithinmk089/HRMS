import { Component, OnInit, inject } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../core/services/api.service';
import { Designation, Tenant } from '../../core/models/phase01.models';
import { TruncatePipe } from '../../shared/pipes/truncate.pipe';

@Component({
  selector: 'app-designation-list',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, TruncatePipe],
  templateUrl: './designation-list.component.html',
  styleUrl: './designation-list.component.scss'
})
export class DesignationListComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  tenants: Tenant[] = [];
  designations: Designation[] = [];

  tenantIdFilter = 0;
  searchText = '';

  isLoading = false;
  saving = false;
  submitted = false;

  showModal = false;
  editMode = false;
  selectedDesgId: number | null = null;
  desgForm!: FormGroup;

  toastMessage = '';
  errorMessage = '';

  constructor() {
    this.initForm();
  }

  ngOnInit() {
    this.loadTenants();
  }

  initForm() {
    this.desgForm = this.fb.group({
      tenantId: ['', [Validators.required]],
      designationCode: ['', [Validators.required]],
      designationName: ['', [Validators.required]],
      grade: ['']
    });
  }

  get f() {
    return this.desgForm.controls;
  }

  loadTenants() {
    this.api.getTenants(undefined, 'Active', 1, 100).subscribe({
      next: (res) => {
        this.tenants = res.data || [];
        if (this.tenants.length > 0) {
          this.tenantIdFilter = this.tenants[0].tenantId;
          this.loadDesignations();
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to load tenants.');
      }
    });
  }

  loadDesignations() {
    if (this.tenantIdFilter === 0) return;
    this.isLoading = true;
    this.api.getDesignations(this.tenantIdFilter, this.searchText).subscribe({
      next: (res) => {
        this.designations = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        this.showToast('Failed to load designations.');
      }
    });
  }

  resetFilters() {
    this.searchText = '';
    if (this.tenants.length > 0) {
      this.tenantIdFilter = this.tenants[0].tenantId;
    }
    this.loadDesignations();
  }

  openCreateModal() {
    this.editMode = false;
    this.submitted = false;
    this.selectedDesgId = null;
    this.errorMessage = '';
    this.initForm();
    this.desgForm.patchValue({
      tenantId: this.tenantIdFilter
    });
    this.showModal = true;
  }

  openEditModal(desg: Designation) {
    this.editMode = true;
    this.submitted = false;
    this.selectedDesgId = desg.designationId;
    this.errorMessage = '';

    this.desgForm.patchValue({
      tenantId: desg.tenantId,
      designationCode: desg.designationCode,
      designationName: desg.designationName,
      grade: desg.grade || ''
    });
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  saveDesignation() {
    this.submitted = true;
    if (this.desgForm.invalid) return;

    this.saving = true;
    this.errorMessage = '';
    const formData = { ...this.desgForm.value };

    if (this.editMode && this.selectedDesgId) {
      this.api.updateDesignation(this.selectedDesgId, formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Designation updated.');
            this.closeModal();
            this.loadDesignations();
          } else {
            this.errorMessage = res.message || 'Failed to update designation.';
          }
        },
        error: (err) => {
          this.saving = false;
          this.errorMessage = 'An error occurred during save.';
          console.error(err);
        }
      });
    } else {
      this.api.createDesignation(formData).subscribe({
        next: (res) => {
          this.saving = false;
          if (res.success) {
            this.showToast('Designation created.');
            this.closeModal();
            this.loadDesignations();
          } else {
            this.errorMessage = res.message || 'Failed to create designation.';
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

  deleteDesignation(desg: Designation) {
    if (confirm(`Delete designation: ${desg.designationName}?`)) {
      this.api.deleteDesignation(desg.designationId, desg.tenantId).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Designation deleted.');
            this.loadDesignations();
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
