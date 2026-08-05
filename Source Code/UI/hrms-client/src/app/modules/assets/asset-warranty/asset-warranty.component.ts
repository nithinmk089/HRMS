import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AssetWarranty, Asset } from '../../../core/models/asset.models';

@Component({
  selector: 'app-asset-warranty',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './asset-warranty.component.html'
})
export class AssetWarrantyComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  warranties: AssetWarranty[] = [];
  assets: Asset[] = [];
  selectedTenantId = 1;
  loading = false;
  saving = false;
  showModal = false;
  errorMessage = '';

  warrantyForm!: FormGroup;

  constructor() {
    this.warrantyForm = this.fb.group({
      assetID: ['', [Validators.required]],
      warrantyStartDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      warrantyEndDate: ['', [Validators.required]],
      warrantyProvider: ['', [Validators.required]]
    });
  }

  ngOnInit() {
    this.loadWarranties();
    this.loadAssets();
  }

  loadWarranties() {
    this.loading = true;
    this.api.getWarranties(this.selectedTenantId).subscribe({
      next: (res: any) => {
        this.warranties = res.data || [];
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
    this.warrantyForm.reset({
      warrantyStartDate: new Date().toISOString().split('T')[0]
    });
    this.showModal = true;
    this.errorMessage = '';
  }

  closeModal() {
    this.showModal = false;
  }

  saveWarranty() {
    if (this.warrantyForm.invalid) return;
    this.saving = true;
    const payload = {
      ...this.warrantyForm.value,
      tenantId: this.selectedTenantId
    };
    this.api.createWarranty(payload).subscribe({
      next: (res: any) => {
        this.saving = false;
        if (res.success) {
          this.closeModal();
          this.loadWarranties();
        } else {
          this.errorMessage = res.message || 'Failed to save warranty.';
        }
      },
      error: () => this.saving = false
    });
  }
}