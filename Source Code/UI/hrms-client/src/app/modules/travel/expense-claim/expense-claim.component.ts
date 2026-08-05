import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-expense-claim',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="expense-claim-container">
      <div class="header-section">
        <div>
          <h1>Expense Claims</h1>
          <p>Submit business travel expenses, category-wise expenditure items, and receipts.</p>
        </div>
        <button class="btn btn-primary" (click)="openCreateModal()">
          <i class="bi bi-plus-circle me-2"></i>New Expense Claim
        </button>
      </div>

      <!-- Toast Notifications -->
      <div *ngIf="successMessage" class="alert alert-success mt-3">{{ successMessage }}</div>
      <div *ngIf="errorMessage" class="alert alert-danger mt-3">{{ errorMessage }}</div>

      <!-- Claims Grid -->
      <div class="card grid-card mt-4">
        <div class="table-responsive">
          <table class="table custom-table">
            <thead>
              <tr>
                <th>Claim ID</th>
                <th>Employee Name</th>
                <th>Travel Destination</th>
                <th>Claim Date</th>
                <th>Total Amount</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let claim of claims">
                <td>#{{ claim.expenseClaimID }}</td>
                <td>{{ claim.employeeName }}</td>
                <td>{{ claim.travelDestination || 'General' }}</td>
                <td>{{ claim.claimDate | date:'mediumDate' }}</td>
                <td><strong>\${{ claim.totalAmount | number:'1.2-2' }}</strong></td>
                <td>
                  <span class="badge" [ngClass]="getStatusClass(claim.claimStatus)">
                    {{ claim.claimStatus }}
                  </span>
                </td>
                <td>
                  <button *ngIf="claim.claimStatus === 'Draft'" class="btn btn-sm btn-outline-primary" (click)="submitClaim(claim.expenseClaimID)">
                    Submit
                  </button>
                </td>
              </tr>
              <tr *ngIf="claims.length === 0">
                <td colspan="7" class="text-center text-muted">No expense claims found.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Modal -->
      <div class="modal-backdrop" *ngIf="showModal" (click)="closeModal()"></div>
      <div class="custom-modal" *ngIf="showModal">
        <div class="modal-header">
          <h2>New Expense Claim</h2>
          <button class="close-btn" (click)="closeModal()">&times;</button>
        </div>
        <form [formGroup]="form" (ngSubmit)="onSubmit()">
          <div class="modal-body scrollable-body">
            <div class="form-group mb-3">
              <label for="employeeId">Employee ID</label>
              <input id="employeeId" type="number" formControlName="employeeId" class="form-control">
            </div>

            <div class="form-group mb-3">
              <label for="travelRequestId">Travel Request ID (Optional)</label>
              <input id="travelRequestId" type="number" formControlName="travelRequestID" class="form-control">
            </div>

            <div class="form-group mb-3">
              <label for="claimDate">Claim Date</label>
              <input id="claimDate" type="date" formControlName="claimDate" class="form-control">
            </div>

            <!-- Expense Items FormArray -->
            <div class="items-header mt-4 mb-2">
              <h4>Expense Items</h4>
              <button type="button" class="btn btn-sm btn-outline-success" (click)="addItem()">
                + Add Item
              </button>
            </div>

            <div formArrayName="items">
              <div *ngFor="let item of items.controls; let idx = index" [formGroupName]="idx" class="item-row mb-3 p-3">
                <div class="d-flex justify-content-between mb-2">
                  <h5>Item #{{ idx + 1 }}</h5>
                  <button type="button" class="btn-close btn-close-white" (click)="removeItem(idx)"></button>
                </div>
                <div class="row">
                  <div class="col-md-4 form-group">
                    <label>Category</label>
                    <select formControlName="expenseCategoryID" class="form-select form-control">
                      <option *ngFor="let cat of categories" [value]="cat.expenseCategoryID">
                        {{ cat.categoryName }} (Limit: \${{ cat.maximumLimit }})
                      </option>
                    </select>
                  </div>
                  <div class="col-md-4 form-group">
                    <label>Amount ($)</label>
                    <input type="number" formControlName="expenseAmount" class="form-control">
                  </div>
                  <div class="col-md-4 form-group">
                    <label>Date</label>
                    <input type="date" formControlName="expenseDate" class="form-control">
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" (click)="closeModal()">Cancel</button>
            <button type="submit" class="btn btn-primary" [disabled]="form.invalid">Create Claim</button>
          </div>
        </form>
      </div>
    </div>
  `,
  styles: [`
    .expense-claim-container {
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
    .badge-rejected { background-color: rgba(231,76,60,0.2); color: #e74c3c; border: 1px solid #e74c3c; }
    .badge-draft { background-color: rgba(189,195,199,0.2); color: #bdc3c7; border: 1px solid #bdc3c7; }

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
      width: 600px;
      max-width: 95%;
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
    .scrollable-body {
      max-height: 60vh;
      overflow-y: auto;
    }
    .modal-footer {
      padding: 20px;
      border-top: 1px solid rgba(255,255,255,0.1);
      display: flex;
      justify-content: flex-end;
      gap: 12px;
    }
    .items-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      border-bottom: 1px solid rgba(255,255,255,0.1);
      padding-bottom: 8px;
    }
    .item-row {
      background: rgba(0,0,0,0.25);
      border: 1px solid rgba(255,255,255,0.05);
      border-radius: 8px;
    }
    .form-control { background: #0b0c10; border: 1px solid rgba(255,255,255,0.1); color: #fff; }
    .form-control:focus { background: #0b0c10; color: #fff; border-color: #45f3ff; box-shadow: 0 0 8px rgba(69,243,255,0.25); }
    .btn-primary { background: #1f85de; border: none; }
    .btn-outline-success { color: #2ecc71; border-color: #2ecc71; background: transparent; }
    .btn-outline-success:hover { background: #2ecc71; color: #fff; }
    .btn-outline-primary { color: #1f85de; border-color: #1f85de; background: transparent; }
    .btn-outline-primary:hover { background: #1f85de; color: #fff; }
  `]
})
export class ExpenseClaimComponent implements OnInit {
  claims: any[] = [];
  categories: any[] = [];
  form!: FormGroup;
  showModal = false;
  successMessage = '';
  errorMessage = '';

  constructor(private api: ApiService, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.initForm();
    this.loadClaims();
    this.loadCategories();
  }

  initForm(): void {
    this.form = this.fb.group({
      employeeId: ['', [Validators.required, Validators.min(1)]],
      travelRequestID: [''],
      claimDate: ['', Validators.required],
      items: this.fb.array([])
    });
  }

  get items(): FormArray {
    return this.form.get('items') as FormArray;
  }

  createItem(): FormGroup {
    return this.fb.group({
      expenseCategoryID: ['', Validators.required],
      expenseAmount: ['', [Validators.required, Validators.min(1)]],
      expenseDate: ['', Validators.required]
    });
  }

  addItem(): void {
    this.items.push(this.createItem());
  }

  removeItem(idx: number): void {
    this.items.removeAt(idx);
  }

  loadClaims(): void {
    this.api.getExpenseClaims(1).subscribe(res => {
      this.claims = res.data || [];
    });
  }

  loadCategories(): void {
    this.api.getExpenseCategories(1).subscribe(res => {
      this.categories = res.data || [];
    });
  }

  openCreateModal(): void {
    this.form.reset();
    while (this.items.length !== 0) {
      this.items.removeAt(0);
    }
    this.addItem(); // default item
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

    this.api.createExpenseClaim(payload).subscribe({
      next: () => {
        this.successMessage = 'Expense claim created successfully!';
        this.loadClaims();
        this.closeModal();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: () => {
        this.errorMessage = 'Failed to create expense claim.';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  submitClaim(id: number): void {
    this.api.submitExpenseClaim(id, 1).subscribe({
      next: () => {
        this.successMessage = 'Expense claim submitted for approval.';
        this.loadClaims();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: () => {
        this.errorMessage = 'Failed to submit claim.';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }

  getStatusClass(status: string): string {
    const s = (status || '').toLowerCase();
    if (s === 'approved' || s === 'settled') return 'badge-approved';
    if (s === 'submitted') return 'badge-pending';
    if (s === 'rejected') return 'badge-rejected';
    return 'badge-draft';
  }
}
