import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AssetAssignment, Asset } from '../../../core/models/asset.models';
import { EmployeeDirectoryDto } from '../../../core/models/employee.models';

@Component({
  selector: 'app-asset-assignment',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './asset-assignment.component.html'
})
export class AssetAssignmentComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  assignments: AssetAssignment[] = [];
  employees: EmployeeDirectoryDto[] = [];
  assets: Asset[] = [];
  selectedTenantId = 1;
  loading = false;
  saving = false;
  showModal = false;
  errorMessage = '';
  employeeIdFilter: number | null = null;
  statusFilter = '';

  assignmentForm!: FormGroup;

  constructor() {
    this.assignmentForm = this.fb.group({
      assetID: ['', [Validators.required]],
      employeeID: ['', [Validators.required]],
      assignedDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      expectedReturnDate: ['']
    });
  }

  ngOnInit() {
    this.loadAssignments();
    this.loadEmployees();
    this.loadAssets();
  }

  loadAssignments() {
    this.loading = true;
    this.api.getAssignments(
      this.selectedTenantId,
      this.employeeIdFilter || undefined,
      undefined,
      this.statusFilter || undefined
    ).subscribe({
      next: (res: any) => {
        this.assignments = res.data || [];
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  resetFilters() {
    this.employeeIdFilter = null;
    this.statusFilter = '';
    this.loadAssignments();
  }

  loadEmployees() {
    this.api.getEmployees(this.selectedTenantId).subscribe({
      next: (res: any) => this.employees = res.data || []
    });
  }

  loadAssets() {
    this.api.getAssets(this.selectedTenantId, undefined, 'Available').subscribe({
      next: (res: any) => this.assets = res.data || []
    });
  }

  openModal() {
    this.assignmentForm.reset({
      assignedDate: new Date().toISOString().split('T')[0]
    });
    this.showModal = true;
    this.errorMessage = '';
    this.loadAssets(); // Fetch fresh available assets
  }

  closeModal() {
    this.showModal = false;
  }

  saveAssignment() {
    if (this.assignmentForm.invalid) return;
    this.saving = true;
    const payload = {
      ...this.assignmentForm.value,
      tenantId: this.selectedTenantId
    };
    this.api.assignAsset(payload).subscribe({
      next: (res: any) => {
        this.saving = false;
        if (res.success) {
          this.closeModal();
          this.loadAssignments();
        } else {
          this.errorMessage = res.message || 'Failed to assign asset.';
        }
      },
      error: () => this.saving = false
    });
  }

  returnAsset(assign: AssetAssignment) {
    if (confirm(`Confirm return of asset for ${assign.employeeName}?`)) {
      this.api.returnAsset(assign.assetAssignmentID!, {
        tenantId: this.selectedTenantId,
        returnedDate: new Date().toISOString().split('T')[0],
        returnCondition: 'Good Condition'
      }).subscribe({
        next: (res: any) => {
          if (res.success) {
            this.loadAssignments();
          } else {
            alert(res.message || 'Failed to process return.');
          }
        }
      });
    }
  }
}