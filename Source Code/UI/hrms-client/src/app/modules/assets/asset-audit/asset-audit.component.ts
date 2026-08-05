import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AssetAudit, Asset } from '../../../core/models/asset.models';
import { EmployeeDirectoryDto } from '../../../core/models/employee.models';

@Component({
  selector: 'app-asset-audit',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './asset-audit.component.html'
})
export class AssetAuditComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  audits: AssetAudit[] = [];
  assets: Asset[] = [];
  employees: EmployeeDirectoryDto[] = [];
  selectedTenantId = 1;
  loading = false;
  saving = false;
  showModal = false;
  errorMessage = '';

  auditForm!: FormGroup;

  constructor() {
    this.auditForm = this.fb.group({
      assetID: ['', [Validators.required]],
      auditDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      auditorID: ['', [Validators.required]]
    });
  }

  ngOnInit() {
    this.loadAudits();
    this.loadAssets();
    this.loadEmployees();
  }

  loadAudits() {
    this.loading = true;
    this.api.getAudits(this.selectedTenantId).subscribe({
      next: (res: any) => {
        this.audits = res.data || [];
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  loadAssets() {
    this.api.getAssets(this.selectedTenantId).subscribe({
      next: (res: any) => this.assets = res.data || []
    });
  }

  loadEmployees() {
    this.api.getEmployees(this.selectedTenantId).subscribe({
      next: (res: any) => this.employees = res.data || []
    });
  }

  openModal() {
    this.auditForm.reset({
      auditDate: new Date().toISOString().split('T')[0]
    });
    this.showModal = true;
    this.errorMessage = '';
  }

  closeModal() {
    this.showModal = false;
  }

  saveAudit() {
    if (this.auditForm.invalid) return;
    this.saving = true;
    const payload = {
      ...this.auditForm.value,
      tenantId: this.selectedTenantId
    };
    this.api.createAudit(payload).subscribe({
      next: (res: any) => {
        this.saving = false;
        if (res.success) {
          this.closeModal();
          this.loadAudits();
        } else {
          this.errorMessage = res.message || 'Failed to schedule audit.';
        }
      },
      error: () => this.saving = false
    });
  }

  completeAudit(a: AssetAudit) {
    const findings = prompt('Enter audit findings/remarks:');
    if (findings !== null) {
      this.api.completeAudit(a.assetAuditID!, this.selectedTenantId, 'Completed', findings).subscribe({
        next: () => this.loadAudits()
      });
    }
  }
}