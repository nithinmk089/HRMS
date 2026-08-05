import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-travel-advance',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="travel-advance-container">
      <div class="header-section">
        <div>
          <h1>Travel Advances</h1>
          <p>Request, approve, or disburse cash advances for employee travel expenditures.</p>
        </div>
        <button class="btn btn-primary" (click)="openCreateModal()">
          <i class="bi bi-plus-circle me-2"></i>Request Advance
        </button>
      </div>

      <!-- Toast Notifications -->
      <div *ngIf="successMessage" class="alert alert-success mt-3">{{ successMessage }}</div>
      <div *ngIf="errorMessage" class="alert alert-danger mt-3">{{ errorMessage }}</div>

      <!-- Advances Grid -->
      <div class="card grid-card mt-4">
        <div class="table-responsive">
          <table class="table custom-table">
            <thead>
              <tr>
                <th>Advance ID</th>
                <th>Employee Name</th>
                <th>Travel Request ID</th>
                <th>Advance Amount</th>
                <th>Disbursement Date</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let adv of advances">
                <td>#{{ adv.travelAdvanceID }}</td>
                <td>{{ adv.employeeName }}</td>
                <td>#{{ adv.travelRequestID }}</td>
                <td><strong>\${{ adv.advanceAmount | number:'1.2-2' }}</strong></td>
                <td>{{ adv.disbursementDate ? (adv.disbursementDate | date:'mediumDate') : 'Pending' }}</td>
                <td>
                  <span class="badge" [ngClass]="getStatusClass(adv.advanceStatus)">
                    {{ adv.advanceStatus }}
                  </span>
                </td>
                <td>
                  <button *ngIf="adv.advanceStatus === 'Requested'" class="btn btn-sm btn-success me-2" (click)="approve(adv.travelAdvanceID)">
                    Approve
                  </button>
                  <button *ngIf="adv.advanceStatus === 'Approved'" class="btn btn-sm btn-primary" (click)="disburse(adv.travelAdvanceID)">
                    Disburse
                  </button>
                </td>
              </tr>
              <tr *ngIf="advances.length === 0">
                <td colspan="7" class="text-center text-muted">No travel advances found.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Modal -->
      <div class="modal-backdrop" *ngIf="showModal" (click)="closeModal()"></div>
      <div class="custom-modal" *ngIf="showModal">
        <div class="modal-header">
          <h2>Request Travel Advance</h2>
          <button class="close-btn" (click)="closeModal()">&times;</button>
        </div>
        <form [formGroup]="form" (ngSubmit)="onSubmit()">
          <div class="modal-body">
            <div class="form-group mb-3">
              <label for="travelRequestId">Travel Request ID</label>
              <input id="travelRequestId" type="number" formControlName="travelRequestID" class="form-control" placeholder="Enter Travel Request ID">
            </div>

            <div class="form-group mb-3">
              <label for="advanceAmount">Advance Amount ($)</label>
              <input id="advanceAmount" type="number" formControlName="advanceAmount" class="form-control" placeholder="0.00">
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" (click)="closeModal()">Cancel</button>
            <button type="submit" class="btn btn-primary" [disabled]="form.invalid">Submit Request</button>
          </div>
        </form>
      </div>
    </div>
  `,
  styles: [`
    .travel-advance-container {
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
    .header-section p { color: #c5c6c7; margin: 0; }
    .grid-card {
      background: #1f2833;
      border: 1px solid rgba(255, 255, 255, 0.05);
      border-radius: 16px;
      padding: 24px;
    }
    .custom-table { color: #c5c6c7; margin: 0; }
    .custom-table th { border-bottom: 2px solid rgba(255, 255, 255, 0.1); color: #fff; }
    .custom-table td { border-bottom: 1px solid rgba(255, 255, 255, 0.05); padding: 14px 8px; }
    .badge { padding: 6px 12px; border-radius: 20px; }
    .badge-approved { background-color: rgba(46,204,113,0.2); color: #2ecc71; border: 1px solid #2ecc71; }
    .badge-pending { background-color: rgba(241,196,15,0.2); color: #f1c40f; border: 1px solid #f1c40f; }
    .badge-disbursed { background-color: rgba(52,152,219,0.2); color: #3498db; border: 1px solid #3498db; }

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
    .form-control { background: #0b0c10; border: 1px solid rgba(255,255,255,0.1); color: #fff; }
    .form-control:focus { background: #0b0c10; color: #fff; border-color: #45f3ff; box-shadow: 0 0 8px rgba(69,243,255,0.25); }
    .btn-primary { background: #1f85de; border: none; }
    .btn-success { background: #2ecc71; border: none; }
  `]
})
export class TravelAdvanceComponent implements OnInit {
  advances: any[] = [];
  form!: FormGroup;
  showModal = false;
  successMessage = '';
  errorMessage = '';

  constructor(private api: ApiService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initForm();
    this.loadAdvances();
  }

  initForm(): void {
    this.form = this.fb.group({
      travelRequestID: ['', [Validators.required, Validators.min(1)]],
      advanceAmount: ['', [Validators.required, Validators.min(1)]]
    });
  }

  loadAdvances(): void {
    this.api.getTravelAdvances(1).subscribe(res => {
      this.advances = res.data || [];
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

    this.api.createTravelAdvance(payload).subscribe({
      next: () => {
        this.successMessage = 'Advance requested successfully!';
        this.loadAdvances();
        this.closeModal();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: () => {
        this.errorMessage = 'Failed to request travel advance.';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  approve(id: number): void {
    this.api.approveTravelAdvance(id, 1).subscribe({
      next: () => {
        this.successMessage = 'Advance approved.';
        this.loadAdvances();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: () => {
        this.errorMessage = 'Failed to approve advance.';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  disburse(id: number): void {
    this.api.disburseTravelAdvance(id, 1).subscribe({
      next: () => {
        this.successMessage = 'Advance marked as disbursed.';
        this.loadAdvances();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: () => {
        this.errorMessage = 'Failed to disburse advance.';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  getStatusClass(status: string): string {
    const s = (status || '').toLowerCase();
    if (s === 'disbursed') return 'badge-disbursed';
    if (s === 'approved') return 'badge-approved';
    return 'badge-pending';
  }
}
