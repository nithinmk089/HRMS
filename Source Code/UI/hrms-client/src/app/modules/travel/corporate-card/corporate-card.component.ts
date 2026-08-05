import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-corporate-card',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="corporate-card-container">
      <div class="header-section">
        <div>
          <h1>Corporate Card Reconciliation</h1>
          <p>Import corporate credit card transactions and reconcile them with approved expense claims.</p>
        </div>
        <button class="btn btn-primary" (click)="openImportModal()">
          <i class="bi bi-file-earmark-arrow-up me-2"></i>Import Transaction
        </button>
      </div>

      <!-- Toast Notifications -->
      <div *ngIf="successMessage" class="alert alert-success mt-3">{{ successMessage }}</div>
      <div *ngIf="errorMessage" class="alert alert-danger mt-3">{{ errorMessage }}</div>

      <!-- Transactions Grid -->
      <div class="card grid-card mt-4">
        <h2>Unreconciled Transactions</h2>
        <div class="table-responsive mt-3">
          <table class="table custom-table">
            <thead>
              <tr>
                <th>Tx ID</th>
                <th>Employee ID</th>
                <th>Transaction Date</th>
                <th>Merchant</th>
                <th>Amount</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let tx of transactions">
                <td>#{{ tx.corporateCardTransactionID }}</td>
                <td>{{ tx.employeeID }}</td>
                <td>{{ tx.transactionDate | date:'mediumDate' }}</td>
                <td>{{ tx.merchantName }}</td>
                <td><strong>\${{ tx.transactionAmount | number:'1.2-2' }}</strong></td>
                <td>
                  <button class="btn btn-sm btn-outline-success" (click)="reconcile(tx.corporateCardTransactionID)">
                    Auto-Reconcile
                  </button>
                </td>
              </tr>
              <tr *ngIf="transactions.length === 0">
                <td colspan="6" class="text-center text-muted">All card transactions reconciled!</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Import Modal -->
      <div class="modal-backdrop" *ngIf="showModal" (click)="closeModal()"></div>
      <div class="custom-modal" *ngIf="showModal">
        <div class="modal-header">
          <h2>Import Credit Card Transaction</h2>
          <button class="close-btn" (click)="closeModal()">&times;</button>
        </div>
        <form [formGroup]="form" (ngSubmit)="onSubmit()">
          <div class="modal-body">
            <div class="form-group mb-3">
              <label for="employeeId">Employee ID</label>
              <input id="employeeId" type="number" formControlName="employeeId" class="form-control">
            </div>

            <div class="form-group mb-3">
              <label for="transactionDate">Transaction Date</label>
              <input id="transactionDate" type="date" formControlName="transactionDate" class="form-control">
            </div>

            <div class="form-group mb-3">
              <label for="merchantName">Merchant Name</label>
              <input id="merchantName" type="text" formControlName="merchantName" class="form-control" placeholder="e.g. Delta Air Lines">
            </div>

            <div class="form-group mb-3">
              <label for="transactionAmount">Transaction Amount ($)</label>
              <input id="transactionAmount" type="number" formControlName="transactionAmount" class="form-control" placeholder="0.00">
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" (click)="closeModal()">Cancel</button>
            <button type="submit" class="btn btn-primary" [disabled]="form.invalid">Import</button>
          </div>
        </form>
      </div>
    </div>
  `,
  styles: [`
    .corporate-card-container {
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
    .grid-card h2 { font-size: 1.3rem; color: #45f3ff; margin: 0; }
    .custom-table { color: #c5c6c7; margin: 0; }
    .custom-table th { border-bottom: 2px solid rgba(255, 255, 255, 0.1); color: #fff; }
    .custom-table td { border-bottom: 1px solid rgba(255, 255, 255, 0.05); padding: 14px 8px; }
    .btn-primary { background-color: #1f85de; border: none; }
    .btn-outline-success { color: #2ecc71; border-color: #2ecc71; background: transparent; }
    .btn-outline-success:hover { background: #2ecc71; color: #fff; }

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
export class CorporateCardComponent implements OnInit {
  transactions: any[] = [];
  form!: FormGroup;
  showModal = false;
  successMessage = '';
  errorMessage = '';

  constructor(private api: ApiService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initForm();
    this.loadTransactions();
  }

  initForm(): void {
    this.form = this.fb.group({
      employeeId: ['', [Validators.required, Validators.min(1)]],
      transactionDate: ['', Validators.required],
      merchantName: ['', Validators.required],
      transactionAmount: ['', [Validators.required, Validators.min(1)]]
    });
  }

  loadTransactions(): void {
    this.api.getCorporateCardTransactions(1).subscribe(res => {
      this.transactions = res.data || [];
    });
  }

  openImportModal(): void {
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

    this.api.importCorporateCardTransaction(payload).subscribe({
      next: () => {
        this.successMessage = 'Corporate card transaction imported!';
        this.loadTransactions();
        this.closeModal();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: () => {
        this.errorMessage = 'Failed to import transaction.';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  reconcile(txId: number): void {
    const payload = {
      corporateCardTransactionID: txId,
      expenseClaimItemID: 1, // assume matching item
      tenantId: 1
    };

    this.api.reconcileCorporateCardTransaction(payload).subscribe({
      next: () => {
        this.successMessage = 'Transaction successfully reconciled!';
        this.loadTransactions();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: () => {
        this.errorMessage = 'Reconciliation failed.';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }
}
