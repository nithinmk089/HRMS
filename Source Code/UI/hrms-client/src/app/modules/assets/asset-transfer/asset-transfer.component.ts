import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';
import { AssetTransfer, Asset } from '../../../core/models/asset.models';
import { EmployeeDirectoryDto } from '../../../core/models/employee.models';

@Component({
  selector: 'app-asset-transfer',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './asset-transfer.component.html'
})
export class AssetTransferComponent implements OnInit {
  private api = inject(ApiService);
  private fb = inject(FormBuilder);

  transfers: AssetTransfer[] = [];
  employees: EmployeeDirectoryDto[] = [];
  assets: Asset[] = [];
  selectedTenantId = 1;
  loading = false;
  saving = false;
  showModal = false;
  errorMessage = '';

  transferForm!: FormGroup;

  constructor() {
    this.transferForm = this.fb.group({
      assetID: ['', [Validators.required]],
      fromEmployeeID: [null],
      toEmployeeID: ['', [Validators.required]],
      transferDate: [new Date().toISOString().split('T')[0], [Validators.required]],
      transferReason: ['']
    });
  }

  ngOnInit() {
    this.loadTransfers();
    this.loadEmployees();
    this.loadAssets();
  }

  loadTransfers() {
    this.loading = true;
    this.api.getTransfers(this.selectedTenantId).subscribe({
      next: (res: any) => {
        this.transfers = res.data || [];
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  loadEmployees() {
    this.api.getEmployees(this.selectedTenantId).subscribe({
      next: (res: any) => this.employees = res.data || []
    });
  }

  loadAssets() {
    this.api.getAssets(this.selectedTenantId).subscribe({
      next: (res: any) => this.assets = (res.data || []).filter((a: any) => a.status === 'Assigned')
    });
  }

  openModal() {
    this.transferForm.reset({
      transferDate: new Date().toISOString().split('T')[0]
    });
    this.showModal = true;
    this.errorMessage = '';
  }

  closeModal() {
    this.showModal = false;
  }

  onAssetChange(event: any) {
    const assetId = Number(event.target.value);
    // Find current active assignment holder
    this.api.getAssignments(this.selectedTenantId, undefined, assetId, 'Active').subscribe({
      next: (res: any) => {
        const assigns = res.data || [];
        if (assigns.length > 0) {
          this.transferForm.patchValue({ fromEmployeeID: assigns[0].employeeID });
        } else {
          this.transferForm.patchValue({ fromEmployeeID: null });
        }
      }
    });
  }

  saveTransfer() {
    if (this.transferForm.invalid) return;
    this.saving = true;
    const payload = {
      ...this.transferForm.value,
      tenantId: this.selectedTenantId
    };
    this.api.createAssetTransfer(payload).subscribe({
      next: (res: any) => {
        this.saving = false;
        if (res.success) {
          this.closeModal();
          this.loadTransfers();
        } else {
          this.errorMessage = res.message || 'Failed to submit transfer.';
        }
      },
      error: () => this.saving = false
    });
  }

  approve(t: AssetTransfer) {
    this.api.approveAssetTransfer(t.assetTransferID!, this.selectedTenantId).subscribe({
      next: () => this.loadTransfers()
    });
  }

  complete(t: AssetTransfer) {
    this.api.completeAssetTransfer(t.assetTransferID!, this.selectedTenantId).subscribe({
      next: () => this.loadTransfers()
    });
  }
}