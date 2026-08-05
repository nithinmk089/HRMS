import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-travel-policy',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="travel-policy-container">
      <div class="header-section">
        <div>
          <h1>Travel Policy Management</h1>
          <p>Configure corporate travel limits, effective dates, and reimbursement policy rules.</p>
        </div>
        <button class="btn btn-primary" (click)="openCreateModal()">
          <i class="bi bi-plus-circle me-2"></i>New Travel Policy
        </button>
      </div>

      <!-- Toast Notifications -->
      <div *ngIf="successMessage" class="alert alert-success mt-3">{{ successMessage }}</div>
      <div *ngIf="errorMessage" class="alert alert-danger mt-3">{{ errorMessage }}</div>

      <!-- Policy Grid -->
      <div class="card grid-card mt-4">
        <div class="table-responsive">
          <table class="table custom-table">
            <thead>
              <tr>
                <th>Policy Code</th>
                <th>Policy Name</th>
                <th>Effective From</th>
                <th>Effective To</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let policy of policies">
                <td><strong>{{ policy.policyCode }}</strong></td>
                <td>{{ policy.policyName }}</td>
                <td>{{ policy.effectiveFrom | date:'mediumDate' }}</td>
                <td>{{ policy.effectiveTo ? (policy.effectiveTo | date:'mediumDate') : 'Indefinite' }}</td>
                <td>
                  <span class="badge" [ngClass]="policy.isActive ? 'badge-active' : 'badge-inactive'">
                    {{ policy.isActive ? 'Active' : 'Inactive' }}
                  </span>
                </td>
              </tr>
              <tr *ngIf="policies.length === 0">
                <td colspan="5" class="text-center text-muted">No travel policies configured.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Modal -->
      <div class="modal-backdrop" *ngIf="showModal" (click)="closeModal()"></div>
      <div class="custom-modal" *ngIf="showModal">
        <div class="modal-header">
          <h2>Create Travel Policy</h2>
          <button class="close-btn" (click)="closeModal()">&times;</button>
        </div>
        <form [formGroup]="form" (ngSubmit)="onSubmit()">
          <div class="modal-body">
            <div class="form-group mb-3">
              <label for="policyCode">Policy Code</label>
              <input id="policyCode" type="text" formControlName="policyCode" class="form-control" placeholder="e.g. TRV_US_STD">
            </div>

            <div class="form-group mb-3">
              <label for="policyName">Policy Name</label>
              <input id="policyName" type="text" formControlName="policyName" class="form-control" placeholder="e.g. Standard US Domestic Policy">
            </div>

            <div class="row">
              <div class="col-md-6 form-group mb-3">
                <label for="effectiveFrom">Effective From</label>
                <input id="effectiveFrom" type="date" formControlName="effectiveFrom" class="form-control">
              </div>
              <div class="col-md-6 form-group mb-3">
                <label for="effectiveTo">Effective To</label>
                <input id="effectiveTo" type="date" formControlName="effectiveTo" class="form-control">
              </div>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" (click)="closeModal()">Cancel</button>
            <button type="submit" class="btn btn-primary" [disabled]="form.invalid">Create</button>
          </div>
        </form>
      </div>
    </div>
  `,
  styles: [`
    .travel-policy-container {
      padding: 24px;
      font-family: 'Inter', sans-serif;
      color: #fff;
      background-color: #0b0c10;
      min-height: 100vh;
    }
    .header-section {
      display: flex;
      justify-content: space-between;
      align-items: center;
      background: linear-gradient(135deg, #1f2833 0%, #0b0c10 100%);
      padding: 24px 32px;
      border-radius: 16px;
      border: 1px solid #45f3ff22;
    }
    .header-section h1 {
      font-size: 2rem;
      margin: 0 0 6px 0;
      background: linear-gradient(to right, #45f3ff, #1f85de);
      -webkit-background-clip: text;
      -webkit-text-fill-color: transparent;
    }
    .header-section p {
      color: #c5c6c7;
      margin: 0;
    }
    .grid-card {
      background: #1f2833;
      border: 1px solid rgba(255, 255, 255, 0.05);
      border-radius: 16px;
      padding: 24px;
    }
    .custom-table {
      color: #c5c6c7;
      margin: 0;
    }
    .custom-table th {
      border-bottom: 2px solid rgba(255, 255, 255, 0.1);
      color: #fff;
    }
    .custom-table td {
      border-bottom: 1px solid rgba(255, 255, 255, 0.05);
      padding: 14px 8px;
    }
    .badge {
      padding: 6px 12px;
      border-radius: 20px;
    }
    .badge-active { background-color: rgba(46,204,113,0.2); color: #2ecc71; border: 1px solid #2ecc71; }
    .badge-inactive { background-color: rgba(231,76,60,0.2); color: #e74c3c; border: 1px solid #e74c3c; }

    /* Modal Styles */
    .modal-backdrop {
      position: fixed;
      top: 0; left: 0; right: 0; bottom: 0;
      background: rgba(0,0,0,0.75);
      z-index: 1000;
    }
    .custom-modal {
      position: fixed;
      top: 50%; left: 50%;
      transform: translate(-50%, -50%);
      background: #1f2833;
      border: 1px solid #45f3ff44;
      border-radius: 16px;
      width: 500px;
      max-width: 90%;
      z-index: 1001;
      color: #fff;
    }
    .modal-header {
      padding: 20px;
      border-bottom: 1px solid rgba(255,255,255,0.1);
      display: flex;
      justify-content: space-between;
      align-items: center;
    }
    .modal-header h2 { margin: 0; font-size: 1.3rem; color: #45f3ff; }
    .close-btn { background: none; border: none; color: #fff; font-size: 1.5rem; cursor: pointer; }
    .modal-body { padding: 20px; }
    .modal-footer {
      padding: 20px;
      border-top: 1px solid rgba(255,255,255,0.1);
      display: flex;
      justify-content: flex-end;
      gap: 12px;
    }
    .form-control {
      background: #0b0c10;
      border: 1px solid rgba(255,255,255,0.1);
      color: #fff;
    }
    .form-control:focus {
      background: #0b0c10;
      color: #fff;
      border-color: #45f3ff;
      box-shadow: 0 0 8px rgba(69,243,255,0.25);
    }
    .btn-primary {
      background: #1f85de;
      border: none;
    }
    .btn-primary:hover {
      background: #1972c2;
    }
  `]
})
export class TravelPolicyComponent implements OnInit {
  policies: any[] = [];
  form!: FormGroup;
  showModal = false;
  successMessage = '';
  errorMessage = '';

  constructor(private api: ApiService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initForm();
    this.loadPolicies();
  }

  initForm(): void {
    this.form = this.fb.group({
      policyCode: ['', Validators.required],
      policyName: ['', Validators.required],
      effectiveFrom: ['', Validators.required],
      effectiveTo: ['']
    });
  }

  loadPolicies(): void {
    this.api.getTravelPolicies(1).subscribe(res => {
      this.policies = res.data || [];
    });
  }

  openCreateModal(): void {
    this.form.reset();
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    const payload = {
      tenantId: 1,
      ...this.form.value
    };

    this.api.createTravelPolicy(payload).subscribe({
      next: () => {
        this.successMessage = 'Policy created successfully!';
        this.loadPolicies();
        this.closeModal();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: () => {
        this.errorMessage = 'Failed to create policy.';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }
}
