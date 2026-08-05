import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-my-travel',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="my-travel-container">
      <div class="header-section">
        <h1>My Travel & Expenses</h1>
        <p>Employee Self-Service Travel Portal. Manage your travel requests, advances, and claims.</p>
      </div>

      <div class="tabs-header mt-4">
        <button class="tab-btn" [class.active]="activeTab === 'requests'" (click)="setTab('requests')">
          Travel Requests
        </button>
        <button class="tab-btn" [class.active]="activeTab === 'advances'" (click)="setTab('advances')">
          Cash Advances
        </button>
        <button class="tab-btn" [class.active]="activeTab === 'claims'" (click)="setTab('claims')">
          Expense Claims
        </button>
      </div>

      <!-- Tab Content: Requests -->
      <div *ngIf="activeTab === 'requests'" class="card grid-card mt-3">
        <h2>My Travel Requests</h2>
        <div class="table-responsive mt-3">
          <table class="table custom-table">
            <thead>
              <tr>
                <th>Request ID</th>
                <th>Destination</th>
                <th>Start Date</th>
                <th>End Date</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let req of myRequests">
                <td>#{{ req.travelRequestID }}</td>
                <td>{{ req.destination }}</td>
                <td>{{ req.travelStartDate | date:'mediumDate' }}</td>
                <td>{{ req.travelEndDate | date:'mediumDate' }}</td>
                <td>
                  <span class="badge" [ngClass]="getStatusClass(req.requestStatus)">
                    {{ req.requestStatus }}
                  </span>
                </td>
              </tr>
              <tr *ngIf="myRequests.length === 0">
                <td colspan="5" class="text-center text-muted">You have no travel requests.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Tab Content: Advances -->
      <div *ngIf="activeTab === 'advances'" class="card grid-card mt-3">
        <h2>My Cash Advances</h2>
        <div class="table-responsive mt-3">
          <table class="table custom-table">
            <thead>
              <tr>
                <th>Advance ID</th>
                <th>Travel Request ID</th>
                <th>Amount</th>
                <th>Disbursement Date</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let adv of myAdvances">
                <td>#{{ adv.travelAdvanceID }}</td>
                <td>#{{ adv.travelRequestID }}</td>
                <td><strong>\${{ adv.advanceAmount | number:'1.2-2' }}</strong></td>
                <td>{{ adv.disbursementDate ? (adv.disbursementDate | date:'mediumDate') : 'Pending' }}</td>
                <td>
                  <span class="badge" [ngClass]="getStatusClass(adv.advanceStatus)">
                    {{ adv.advanceStatus }}
                  </span>
                </td>
              </tr>
              <tr *ngIf="myAdvances.length === 0">
                <td colspan="5" class="text-center text-muted">You have no cash advances.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Tab Content: Claims -->
      <div *ngIf="activeTab === 'claims'" class="card grid-card mt-3">
        <h2>My Expense Claims</h2>
        <div class="table-responsive mt-3">
          <table class="table custom-table">
            <thead>
              <tr>
                <th>Claim ID</th>
                <th>Destination</th>
                <th>Claim Date</th>
                <th>Total Amount</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let claim of myClaims">
                <td>#{{ claim.expenseClaimID }}</td>
                <td>{{ claim.travelDestination || 'General' }}</td>
                <td>{{ claim.claimDate | date:'mediumDate' }}</td>
                <td><strong>\${{ claim.totalAmount | number:'1.2-2' }}</strong></td>
                <td>
                  <span class="badge" [ngClass]="getStatusClass(claim.claimStatus)">
                    {{ claim.claimStatus }}
                  </span>
                </td>
              </tr>
              <tr *ngIf="myClaims.length === 0">
                <td colspan="5" class="text-center text-muted">You have no expense claims.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .my-travel-container {
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
    .tabs-header {
      display: flex;
      gap: 12px;
      border-bottom: 2px solid rgba(255,255,255,0.08);
      padding-bottom: 8px;
    }
    .tab-btn {
      background: transparent;
      border: none;
      color: #c5c6c7;
      font-size: 1.1rem;
      padding: 8px 16px;
      cursor: pointer;
      position: relative;
    }
    .tab-btn.active {
      color: #45f3ff;
      font-weight: 600;
    }
    .tab-btn.active::after {
      content: '';
      position: absolute;
      bottom: -10px; left: 0; right: 0;
      height: 2px;
      background-color: #45f3ff;
    }
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
    .badge { padding: 6px 12px; border-radius: 20px; }
    .badge-approved { background-color: rgba(46,204,113,0.2); color: #2ecc71; border: 1px solid #2ecc71; }
    .badge-pending { background-color: rgba(241,196,15,0.2); color: #f1c40f; border: 1px solid #f1c40f; }
    .badge-rejected { background-color: rgba(231,76,60,0.2); color: #e74c3c; border: 1px solid #e74c3c; }
    .badge-draft { background-color: rgba(189,195,199,0.2); color: #bdc3c7; border: 1px solid #bdc3c7; }
    .badge-disbursed { background-color: rgba(52,152,219,0.2); color: #3498db; border: 1px solid #3498db; }
  `]
})
export class MyTravelComponent implements OnInit {
  activeTab = 'requests';
  myRequests: any[] = [];
  myAdvances: any[] = [];
  myClaims: any[] = [];
  employeeId = 5; // Simulating employee self-service login context

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadMyData();
  }

  loadMyData(): void {
    this.api.getTravelRequests(1, this.employeeId).subscribe(res => {
      this.myRequests = res.data || [];
    });
    this.api.getTravelAdvances(1, this.employeeId).subscribe(res => {
      this.myAdvances = res.data || [];
    });
    this.api.getExpenseClaims(1, this.employeeId).subscribe(res => {
      this.myClaims = res.data || [];
    });
  }

  setTab(tab: string): void {
    this.activeTab = tab;
  }

  getStatusClass(status: string): string {
    const s = (status || '').toLowerCase();
    if (s === 'approved' || s === 'settled' || s === 'disbursed') return 'badge-approved';
    if (s === 'pending' || s === 'requested' || s === 'submitted') return 'badge-pending';
    if (s === 'rejected' || s === 'cancelled') return 'badge-rejected';
    return 'badge-draft';
  }
}
