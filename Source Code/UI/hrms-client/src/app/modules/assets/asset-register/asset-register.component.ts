import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { Asset, AssetCategory } from '../../../core/models/asset.models';

@Component({
  selector: 'app-asset-register',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './asset-register.component.html'
})
export class AssetRegisterComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  assets: Asset[] = [];
  categories: AssetCategory[] = [];
  selectedTenantId = 1;
  loading = false;
  saving = false;
  showModal = false;
  errorMessage = '';
  searchText = '';
  statusFilter = '';

  assetForm!: FormGroup;

  constructor() {
    this.assetForm = this.fb.group({
      assetCode: ['', [Validators.required, Validators.maxLength(50)]],
      assetTag: ['', [Validators.required, Validators.maxLength(50)]],
      assetName: ['', [Validators.required, Validators.maxLength(150)]],
      assetCategoryID: ['', [Validators.required]],
      manufacturer: [''],
      model: [''],
      serialNumber: ['', [Validators.required, Validators.maxLength(100)]],
      purchaseDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      purchaseCost: [0, [Validators.required, Validators.min(0)]],
      currentBookValue: [0, [Validators.required, Validators.min(0)]],
      status: ['Available']
    });
  }

  ngOnInit() {
    this.loadAssets();
    this.loadCategories();
  }

  loadAssets() {
    this.loading = true;
    this.api.getAssets(this.selectedTenantId, this.searchText || undefined, this.statusFilter || undefined).subscribe({
      next: (res: any) => {
        this.assets = res.data || [];
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  resetFilters() {
    this.searchText = '';
    this.statusFilter = '';
    this.loadAssets();
  }

  loadCategories() {
    this.api.getCategories(this.selectedTenantId).subscribe({
      next: (res: any) => this.categories = res.data || []
    });
  }

  openModal() {
    this.assetForm.reset({
      purchaseDate: new Date().toISOString().split('T')[0],
      purchaseCost: 0,
      currentBookValue: 0,
      status: 'Available'
    });
    this.showModal = true;
    this.errorMessage = '';
  }

  closeModal() {
    this.showModal = false;
  }

  saveAsset() {
    if (this.assetForm.invalid) return;
    this.saving = true;
    const payload = {
      ...this.assetForm.value,
      tenantId: this.selectedTenantId
    };
    this.api.createAsset(payload).subscribe({
      next: (res: any) => {
        this.saving = false;
        if (res.success) {
          this.closeModal();
          this.loadAssets();
        } else {
          this.errorMessage = res.message || 'Failed to register asset.';
        }
      },
      error: () => this.saving = false
    });
  }
}