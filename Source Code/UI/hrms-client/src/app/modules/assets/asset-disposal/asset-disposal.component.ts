import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AssetDisposal, Asset } from '../../../core/models/asset.models';

@Component({
  selector: 'app-asset-disposal',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './asset-disposal.component.html'
})
export class AssetDisposalComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  disposals: AssetDisposal[] = [];
  assets: Asset[] = [];
  selectedTenantId = 1;
  loading = false;
  saving = false;
  showModal = false;
  errorMessage = '';

  disposalForm!: FormGroup;

  constructor() {
    this.disposalForm = this.fb.group({
      assetID: ['', [Validators.required]],
      disposalDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      disposalMethod: ['', [Validators.required]],
      disposalValue: [0, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit() {
    this.loadDisposals();
    this.loadAssets();
  }

  loadDisposals() {
    this.loading = true;
    this.api.getDisposals(this.selectedTenantId).subscribe({
      next: (res: any) => {
        this.disposals = res.data || [];
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  loadAssets() {
    this.api.getAssets(this.selectedTenantId).subscribe({
      next: (res: any) => this.assets = (res.data || []).filter((a: any) => a.status !== 'Disposed')
    });
  }

  openModal() {
    this.disposalForm.reset({
      disposalDate: new Date().toISOString().split('T')[0],
      disposalValue: 0
    });
    this.showModal = true;
    this.errorMessage = '';
  }

  closeModal() {
    this.showModal = false;
  }

  saveDisposal() {
    if (this.disposalForm.invalid) return;
    this.saving = true;
    const payload = {
      ...this.disposalForm.value,
      tenantId: this.selectedTenantId
    };
    this.api.createDisposal(payload).subscribe({
      next: (res: any) => {
        this.saving = false;
        if (res.success) {
          this.closeModal();
          this.loadDisposals();
        } else {
          this.errorMessage = res.message || 'Failed to submit disposal request.';
        }
      },
      error: () => this.saving = false
    });
  }

  approve(d: AssetDisposal) {
    this.api.approveDisposal(d.assetDisposalID!, this.selectedTenantId).subscribe({
      next: () => this.loadDisposals()
    });
  }

  closeDisposal(d: AssetDisposal) {
    this.api.closeDisposal(d.assetDisposalID!, this.selectedTenantId).subscribe({
      next: () => this.loadDisposals()
    });
  }
}