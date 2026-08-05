import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AssetRepair, Asset } from '../../../core/models/asset.models';

@Component({
  selector: 'app-asset-repair',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './asset-repair.component.html'
})
export class AssetRepairComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  repairs: AssetRepair[] = [];
  assets: Asset[] = [];
  selectedTenantId = 1;
  loading = false;
  saving = false;
  showModal = false;
  errorMessage = '';

  repairForm!: FormGroup;

  constructor() {
    this.repairForm = this.fb.group({
      assetID: ['', [Validators.required]],
      repairDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      repairReason: ['', [Validators.required]],
      repairCost: [0, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit() {
    this.loadRepairs();
    this.loadAssets();
  }

  loadRepairs() {
    this.loading = true;
    this.api.getRepairs(this.selectedTenantId).subscribe({
      next: (res: any) => {
        this.repairs = res.data || [];
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
    this.repairForm.reset({
      repairDate: new Date().toISOString().split('T')[0],
      repairCost: 0
    });
    this.showModal = true;
    this.errorMessage = '';
  }

  closeModal() {
    this.showModal = false;
  }

  saveRepair() {
    if (this.repairForm.invalid) return;
    this.saving = true;
    const payload = {
      ...this.repairForm.value,
      tenantId: this.selectedTenantId
    };
    this.api.createRepair(payload).subscribe({
      next: (res: any) => {
        this.saving = false;
        if (res.success) {
          this.closeModal();
          this.loadRepairs();
        } else {
          this.errorMessage = res.message || 'Failed to submit repair request.';
        }
      },
      error: () => this.saving = false
    });
  }

  closeRepair(r: AssetRepair) {
    if (confirm('Verify repair completion and mark asset as Available?')) {
      this.api.closeRepair(r.assetRepairID!, this.selectedTenantId, 'Repaired by IT team.').subscribe({
        next: () => this.loadRepairs()
      });
    }
  }
}