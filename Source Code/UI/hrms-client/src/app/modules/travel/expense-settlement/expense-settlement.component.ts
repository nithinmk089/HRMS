import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-expense-settlement',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="expense-settlement-container">
      <div class="header-section">
        <h1>Expense Settlements</h1>
        <p>Review final expense calculations, deduct travel advances, and process employee payments.</p>
      </div>

      <!-- Toast Notifications -->
      <div *ngIf="successMessage" class="alert alert-success mt-3">{{ successMessage }}</div>
      <div *ngIf="errorMessage" class="alert alert-danger mt-3">{{ errorMessage }}</div>

      <!-- Pending Claims to Settle -->
      <div class="card grid-card mt-4">
        <h2>Approved Claims Awaiting Settlement</h2>
        <div class="table-responsive mt-3">
          <table class="table custom-table">
            <thead>
              <tr>
                <th>Claim ID</th>
                <th>Employee</th>
                <th>Destination</th>
                <th>Total Claimed</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let claim of pendingClaims">
                <td>#{{ claim.expenseClaimID }}</td>
                <td>{{ claim.employeeName }}</td>
                <td>{{ claim.travelDestination || 'General' }}</td>
                <td><strong>\${{ claim.totalAmount | number:'1.2-2' }}</strong></td>
                <td>
                  <button class="btn btn-sm btn-primary" (click)="openSettleModal(claim)">
                    Process Settlement
                  </button>
                </td>
              </tr>
              <tr *ngIf="pendingClaims.length === 0">
                <td colspan="5" class="text-center text-muted">No claims awaiting settlement.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Settlement Modal -->
      <div class="modal-backdrop" *ngIf="showModal" (click)="closeModal()"></div>
      <div class="custom-modal" *ngIf="showModal">
        <div class="modal-header">
          <h2>Process Settlement (Claim #{{ selectedClaim?.expenseClaimID }})</h2>
          <button class="close-btn" (click)="closeModal()">&times;</button>
        </div>
        <form [formGroup]="form" (ngSubmit)="onSubmit()">
          <div class="modal-body">
            <p>Employee: <strong>{{ selectedClaim?.employeeName }}</strong></p>
            <p>Total Claim Amount: <strong>\${{ selectedClaim?.totalAmount | number:'1.2-2' }}</strong></p>

            <div class="form-group mb-3">
              <label for="advanceAmount">Deduct Travel Advance ($)</label>
              <input id="advanceAmount" type="number" formControlName="advanceAmount" class="form-control" (input)="recalc()">
            </div>

            <div class="form-group mb-3">
              <label>Net Settlement Amount ($)</label>
              <div class="net-settlement-box">\${{ netSettlement | number:'1.2-2' }}</div>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" (click)="closeModal()">Cancel</button>
            <button type="submit" class="btn btn-primary" [disabled]="form.invalid">Confirm Settlement</button>
          </div>
        </form>
      </div>
    </div>
  `,
  styles: [`
    .expense-settlement-container {
      padding: 24px;
      font-family: 'Inter', sans-serif;
      color: #fff;
      background-color: #0b0c10;
      min-height: 100vh;
    }
    .header-section {
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
    .grid-card h2 { font-size: 1.3rem; color: #45f3ff; margin: 0; }
    .custom-table { color: #c5c6c7; margin: 0; }
    .custom-table th { border-bottom: 2px solid rgba(255, 255, 255, 0.1); color: #fff; }
    .custom-table td { border-bottom: 1px solid rgba(255, 255, 255, 0.05); padding: 14px 8px; }
    .btn-primary { background-color: #1f85de; border: none; }
    .net-settlement-box {
      font-size: 1.5rem;
      font-weight: 700;
      color: #2ecc71;
      padding: 12px;
      background: rgba(0,0,0,0.2);
      border-radius: 8px;
      border: 1px dashed #2ecc71;
    }

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
  `]
})
export class ExpenseSettlementComponent implements OnInit {
  pendingClaims: any[] = [];
  selectedClaim: any = null;
  form!: FormGroup;
  showModal = false;
  netSettlement = 0;
  successMessage = '';
  errorMessage = '';

  constructor(private api: ApiService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initForm();
    this.loadPendingClaims();
  }

  initForm(): void {
    this.form = this.fb.group({
      advanceAmount: [0, [Validators.required, Validators.min(0)]]
    });
  }

  loadPendingClaims(): void {
    this.api.getExpenseClaims(1).subscribe(res => {
      this.pendingClaims = (res.data || []).filter((c: any) => c.claimStatus === 'Approved');
    });
  }

  openSettleModal(claim: any): void {
    this.selectedClaim = claim;
    this.netSettlement = claim.totalAmount;
    this.form.reset({ advanceAmount: 0 });
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.selectedClaim = null;
  }

  recalc(): void {
    const adv = this.form.value.advanceAmount || 0;
    this.netSettlement = Math.max(0, this.selectedClaim.totalAmount - adv);
  }

  onSubmit(): void {
    if (this.form.invalid || !this.selectedClaim) return;

    const payload = {
      expenseClaimID: this.selectedClaim.expenseClaimID,
      tenantId: 1,
      advanceAmount: this.form.value.advanceAmount,
      settlementAmount: this.netSettlement
    };

    this.api.processExpenseSettlement(payload).subscribe({
      next: () => {
        this.successMessage = 'Settlement completed successfully!';
        this.loadPendingClaims();
        this.closeModal();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: () => {
        this.errorMessage = 'Failed to process settlement.';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }
}
