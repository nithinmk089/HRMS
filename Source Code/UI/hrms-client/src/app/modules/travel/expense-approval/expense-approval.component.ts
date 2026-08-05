import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-expense-approval',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="expense-approval-container">
      <div class="header-section">
        <h1>Expense Approvals Inbox</h1>
        <p>Review submitted expense claims, check itemized billing, and confirm or reject claims.</p>
      </div>

      <div *ngIf="successMessage" class="alert alert-success mt-3">{{ successMessage }}</div>
      <div *ngIf="errorMessage" class="alert alert-danger mt-3">{{ errorMessage }}</div>

      <!-- Pending Grid -->
      <div class="card grid-card mt-4">
        <h2>Pending Claims</h2>
        <div class="table-responsive mt-3">
          <table class="table custom-table">
            <thead>
              <tr>
                <th>Claim ID</th>
                <th>Employee</th>
                <th>Destination</th>
                <th>Claim Date</th>
                <th>Total Amount</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let claim of pendingClaims">
                <td>#{{ claim.expenseClaimID }}</td>
                <td>{{ claim.employeeName }}</td>
                <td>{{ claim.travelDestination || 'General' }}</td>
                <td>{{ claim.claimDate | date:'mediumDate' }}</td>
                <td><strong>\${{ claim.totalAmount | number:'1.2-2' }}</strong></td>
                <td>
                  <button class="btn btn-sm btn-success me-2" (click)="decide(claim.expenseClaimID, 'Approved')">
                    Approve
                  </button>
                  <button class="btn btn-sm btn-danger" (click)="decide(claim.expenseClaimID, 'Rejected')">
                    Reject
                  </button>
                </td>
              </tr>
              <tr *ngIf="pendingClaims.length === 0">
                <td colspan="6" class="text-center text-muted">No pending expense approvals.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .expense-approval-container {
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
    .btn-success { background-color: #2ecc71; border: none; }
    .btn-danger { background-color: #e74c3c; border: none; }
  `]
})
export class ExpenseApprovalComponent implements OnInit {
  pendingClaims: any[] = [];
  successMessage = '';
  errorMessage = '';

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadPending();
  }

  loadPending(): void {
    this.api.getExpenseClaims(1).subscribe(res => {
      this.pendingClaims = (res.data || []).filter((c: any) => c.claimStatus === 'Submitted');
    });
  }

  decide(claimId: number, decision: string): void {
    const payload = {
      approverId: 1,
      approvalStatus: decision
    };
    this.api.approveExpenseClaim(claimId, payload).subscribe({
      next: () => {
        this.successMessage = `Expense claim ${decision.toLowerCase()} successfully!`;
        this.loadPending();
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: () => {
        this.errorMessage = 'Failed to submit decision.';
        setTimeout(() => this.errorMessage = '', 3000);
      }
    });
  }
}
