import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-travel-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="travel-dashboard-container">
      <div class="header-banner">
        <h1>Travel & Expense Dashboard</h1>
        <p>Monitor corporate travel requests, expense claims, outstanding advances, and compliance status.</p>
      </div>

      <!-- KPI Summary Cards -->
      <div class="metrics-grid">
        <div class="card metric-card gradient-blue">
          <div class="card-icon"><i class="bi bi-airplane"></i></div>
          <div class="card-content">
            <h3>Active Travel Requests</h3>
            <div class="metric-value">{{ activeRequests }}</div>
          </div>
        </div>

        <div class="card metric-card gradient-purple">
          <div class="card-icon"><i class="bi bi-wallet2"></i></div>
          <div class="card-content">
            <h3>Total Expense Claims</h3>
            <div class="metric-value">{{ expenseClaims.length }}</div>
          </div>
        </div>

        <div class="card metric-card gradient-green">
          <div class="card-icon"><i class="bi bi-cash-coin"></i></div>
          <div class="card-content">
            <h3>Outstanding Advances</h3>
            <div class="metric-value">\${{ outstandingAdvances | number:'1.2-2' }}</div>
          </div>
        </div>

        <div class="card metric-card gradient-orange">
          <div class="card-icon"><i class="bi bi-shield-check"></i></div>
          <div class="card-content">
            <h3>Compliance Violations</h3>
            <div class="metric-value">{{ complianceViolations }}</div>
          </div>
        </div>
      </div>

      <!-- Main Dashboard Grid -->
      <div class="dashboard-details-grid">
        <!-- Recent Travel Requests -->
        <div class="card details-card">
          <h2>Recent Travel Requests</h2>
          <div class="table-responsive">
            <table class="table custom-table">
              <thead>
                <tr>
                  <th>Employee</th>
                  <th>Destination</th>
                  <th>Dates</th>
                  <th>Status</th>
                </tr>
              </thead>
              <tbody>
                <tr *ngFor="let request of recentRequests">
                  <td>{{ request.employeeName }}</td>
                  <td>{{ request.destination }}</td>
                  <td>{{ request.travelStartDate | date:'shortDate' }} - {{ request.travelEndDate | date:'shortDate' }}</td>
                  <td>
                    <span class="badge" [ngClass]="getStatusClass(request.requestStatus)">
                      {{ request.requestStatus }}
                    </span>
                  </td>
                </tr>
                <tr *ngIf="recentRequests.length === 0">
                  <td colspan="4" class="text-center text-muted">No travel requests found.</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>

        <!-- Recent Expense Claims -->
        <div class="card details-card">
          <h2>Recent Expense Claims</h2>
          <div class="table-responsive">
            <table class="table custom-table">
              <thead>
                <tr>
                  <th>Employee</th>
                  <th>Claim Date</th>
                  <th>Amount</th>
                  <th>Status</th>
                </tr>
              </thead>
              <tbody>
                <tr *ngFor="let claim of expenseClaims">
                  <td>{{ claim.employeeName }}</td>
                  <td>{{ claim.claimDate | date:'shortDate' }}</td>
                  <td>\${{ claim.totalAmount | number:'1.2-2' }}</td>
                  <td>
                    <span class="badge" [ngClass]="getStatusClass(claim.claimStatus)">
                      {{ claim.claimStatus }}
                    </span>
                  </td>
                </tr>
                <tr *ngIf="expenseClaims.length === 0">
                  <td colspan="4" class="text-center text-muted">No expense claims found.</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .travel-dashboard-container {
      padding: 24px;
      font-family: 'Inter', -apple-system, sans-serif;
      color: #fff;
      background-color: #0b0c10;
      min-height: 100vh;
    }
    .header-banner {
      background: linear-gradient(135deg, #1f2833 0%, #0b0c10 100%);
      padding: 32px;
      border-radius: 16px;
      margin-bottom: 24px;
      border: 1px solid #45f3ff22;
      box-shadow: 0 4px 30px rgba(0, 0, 0, 0.4);
    }
    .header-banner h1 {
      font-size: 2.2rem;
      font-weight: 700;
      margin: 0 0 8px 0;
      background: linear-gradient(to right, #45f3ff, #1f85de);
      -webkit-background-clip: text;
      -webkit-text-fill-color: transparent;
    }
    .header-banner p {
      color: #c5c6c7;
      margin: 0;
      font-size: 1.1rem;
    }
    .metrics-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(240px, 1fr));
      gap: 24px;
      margin-bottom: 24px;
    }
    .metric-card {
      display: flex;
      align-items: center;
      padding: 24px;
      border-radius: 16px;
      border: 1px solid rgba(255, 255, 255, 0.08);
      position: relative;
      overflow: hidden;
      box-shadow: 0 8px 32px 0 rgba(0, 0, 0, 0.3);
    }
    .gradient-blue { background: linear-gradient(135deg, rgba(31,133,222,0.1) 0%, rgba(11,12,16,0.8) 100%); }
    .gradient-purple { background: linear-gradient(135deg, rgba(138,43,226,0.1) 0%, rgba(11,12,16,0.8) 100%); }
    .gradient-green { background: linear-gradient(135deg, rgba(46,204,113,0.1) 0%, rgba(11,12,16,0.8) 100%); }
    .gradient-orange { background: linear-gradient(135deg, rgba(230,126,34,0.1) 0%, rgba(11,12,16,0.8) 100%); }

    .card-icon {
      font-size: 2.5rem;
      margin-right: 20px;
      color: #45f3ff;
    }
    .card-content h3 {
      font-size: 0.95rem;
      text-transform: uppercase;
      letter-spacing: 1px;
      color: #c5c6c7;
      margin: 0 0 6px 0;
    }
    .metric-value {
      font-size: 2rem;
      font-weight: 700;
      color: #fff;
    }
    .dashboard-details-grid {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 24px;
    }
    @media (max-width: 992px) {
      .dashboard-details-grid {
        grid-template-columns: 1fr;
      }
    }
    .details-card {
      background: #1f2833;
      border: 1px solid rgba(255, 255, 255, 0.05);
      border-radius: 16px;
      padding: 24px;
      box-shadow: 0 8px 32px 0 rgba(0, 0, 0, 0.3);
    }
    .details-card h2 {
      font-size: 1.3rem;
      margin-top: 0;
      margin-bottom: 20px;
      color: #45f3ff;
      border-bottom: 1px solid rgba(255, 255, 255, 0.1);
      padding-bottom: 12px;
    }
    .custom-table {
      color: #c5c6c7;
      margin: 0;
    }
    .custom-table th {
      border-bottom: 2px solid rgba(255, 255, 255, 0.1);
      color: #fff;
      font-weight: 600;
    }
    .custom-table td {
      border-bottom: 1px solid rgba(255, 255, 255, 0.05);
      padding: 12px 8px;
    }
    .badge {
      padding: 6px 12px;
      border-radius: 20px;
      font-weight: 500;
    }
    .badge-approved { background-color: rgba(46,204,113,0.2); color: #2ecc71; border: 1px solid #2ecc71; }
    .badge-pending { background-color: rgba(241,196,15,0.2); color: #f1c40f; border: 1px solid #f1c40f; }
    .badge-rejected { background-color: rgba(231,76,60,0.2); color: #e74c3c; border: 1px solid #e74c3c; }
    .badge-draft { background-color: rgba(189,195,199,0.2); color: #bdc3c7; border: 1px solid #bdc3c7; }
  `]
})
export class TravelDashboardComponent implements OnInit {
  activeRequests = 0;
  outstandingAdvances = 0.00;
  complianceViolations = 0;
  recentRequests: any[] = [];
  expenseClaims: any[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.api.getTravelRequests(1).subscribe(res => {
      this.recentRequests = (res.data || []).slice(0, 5);
      this.activeRequests = (res.data || []).filter((r: any) => r.requestStatus === 'Approved' || r.requestStatus === 'Pending').length;
    });

    this.api.getExpenseClaims(1).subscribe(res => {
      this.expenseClaims = (res.data || []).slice(0, 5);
    });

    this.api.getTravelAdvances(1).subscribe(res => {
      this.outstandingAdvances = (res.data || []).reduce((acc: number, curr: any) => acc + curr.advanceAmount, 0);
    });

    this.api.getTravelCompliance(1).subscribe(res => {
      this.complianceViolations = (res.data || []).filter((c: any) => c.complianceStatus === 'Violation').length;
    });
  }

  getStatusClass(status: string): string {
    const s = (status || '').toLowerCase();
    if (s === 'approved' || s === 'settled' || s === 'disbursed') return 'badge-approved';
    if (s === 'pending' || s === 'requested') return 'badge-pending';
    if (s === 'rejected' || s === 'cancelled') return 'badge-rejected';
    return 'badge-draft';
  }
}
