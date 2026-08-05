import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AssetCategory } from '../../../core/models/asset.models';

@Component({
  selector: 'app-asset-category',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './asset-category.component.html'
})
export class AssetCategoryComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  categories: AssetCategory[] = [];
  selectedTenantId = 1;
  loading = false;
  saving = false;
  showModal = false;
  errorMessage = '';

  categoryForm!: FormGroup;

  constructor() {
    this.categoryForm = this.fb.group({
      categoryCode: ['', [Validators.required, Validators.maxLength(50)]],
      categoryName: ['', [Validators.required, Validators.maxLength(100)]],
      parentCategoryID: [null],
      description: [''],
      isDepreciable: [true]
    });
  }

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    this.loading = true;
    this.api.getCategories(this.selectedTenantId).subscribe({
      next: (res: any) => {
        this.categories = res.data || [];
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  openModal() {
    this.categoryForm.reset({ isDepreciable: true });
    this.showModal = true;
    this.errorMessage = '';
  }

  closeModal() {
    this.showModal = false;
  }

  saveCategory() {
    if (this.categoryForm.invalid) return;
    this.saving = true;
    const payload = {
      ...this.categoryForm.value,
      tenantId: this.selectedTenantId
    };
    this.api.createCategory(payload).subscribe({
      next: (res: any) => {
        this.saving = false;
        if (res.success) {
          this.closeModal();
          this.loadCategories();
        } else {
          this.errorMessage = res.message || 'Failed to create category.';
        }
      },
      error: () => this.saving = false
    });
  }
}