import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AssetMaintenance, Asset } from '../../../core/models/asset.models';

@Component({
  selector: 'app-asset-maintenance',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './asset-maintenance.component.html'
})
export class AssetMaintenanceComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  maintenances: AssetMaintenance[] = [];
  assets: Asset[] = [];
  selectedTenantId = 1;
  loading = false;
  saving = false;
  showModal = false;
  errorMessage = '';

  maintForm!: FormGroup;

  constructor() {
    this.maintForm = this.fb.group({
      assetID: ['', [Validators.required]],
      maintenanceDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      maintenanceType: ['', [Validators.required]],
      vendorName: [''],
      cost: [0, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit() {
    this.loadMaintenances();
    this.loadAssets();
  }

  loadMaintenances() {
    this.loading = true;
    this.api.getMaintenance(this.selectedTenantId).subscribe({
      next: (res: any) => {
        this.maintenances = res.data || [];
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

  openModal() {
    this.maintForm.reset({
      maintenanceDate: new Date().toISOString().split('T')[0],
      cost: 0
    });
    this.showModal = true;
    this.errorMessage = '';
  }

  closeModal() {
    this.showModal = false;
  }

  saveMaintenance() {
    if (this.maintForm.invalid) return;
    this.saving = true;
    const payload = {
      ...this.maintForm.value,
      tenantId: this.selectedTenantId
    };
    this.api.createMaintenance(payload).subscribe({
      next: (res: any) => {
        this.saving = false;
        if (res.success) {
          this.closeModal();
          this.loadMaintenances();
        } else {
          this.errorMessage = res.message || 'Failed to schedule maintenance.';
        }
      },
      error: () => this.saving = false
    });
  }

  closeMaint(m: AssetMaintenance) {
    if (confirm('Close this maintenance session and mark asset as Available?')) {
      this.api.closeMaintenance(m.assetMaintenanceID!, this.selectedTenantId, 'Serviced successfully.').subscribe({
        next: () => this.loadMaintenances()
      });
    }
  }
}