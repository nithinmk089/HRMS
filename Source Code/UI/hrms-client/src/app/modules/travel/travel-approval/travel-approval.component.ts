import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-travel-approval',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="travel-approval-container">
      <div class="header-section">
        <h1>Travel Approvals Inbox</h1>
        <p>Review, approve, or reject employee business travel requests.</p>
      </div>

      <div *ngIf="successMessage" class="alert alert-success mt-3">{{ successMessage }}</div>
      <div *ngIf="errorMessage" class="alert alert-danger mt-3">{{ errorMessage }}</div>

      <!-- Pending Items Grid -->
      <div class="card grid-card mt-4">
        <h2>Pending Travel Requests</h2>
        <div class="table-responsive mt-3">
          <table class="table custom-table">
            <thead>
              <tr>
                <th>Request ID</th>
                <th>Employee Name</th>
                <th>Destination</th>
                <th>Dates</th>
                <th>Type</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let req of pendingRequests">
                <td>#{{ req.travelRequestID }}</td>
                <td>{{ req.employeeName }}</td>
                <td>{{ req.destination }}</td>
                <td>{{ req.travelStartDate | date:'mediumDate' }} - {{ req.travelEndDate | date:'mediumDate' }}</td>
                <td>{{ req.travelType }}</td>
                <td>
                  <button class="btn btn-sm btn-success me-2" (click)="decide(req.travelRequestID, 'Approved')">
                    Approve
                  </button>
                  <button class="btn btn-sm btn-danger" (click)="decide(req.travelRequestID, 'Rejected')">
                    Reject
                  </button>
                </td>
              </tr>
              <tr *ngIf="pendingRequests.length === 0">
                <td colspan="6" class="text-center text-muted">No pending approvals found.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .travel-approval-container {
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
export class TravelApprovalComponent implements OnInit {
  pendingRequests: any[] = [];
  successMessage = '';
  errorMessage = '';

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadPending();
  }

  loadPending(): void {
    this.api.getTravelRequests(1).subscribe(res => {
      this.pendingRequests = (res.data || []).filter((r: any) => r.requestStatus === 'Pending');
    });
  }

  decide(requestId: number, decision: string): void {
    const payload = {
      approverId: 1,
      approvalStatus: decision
    };
    this.api.approveTravelRequest(requestId, payload).subscribe({
      next: () => {
        this.successMessage = `Travel request ${decision.toLowerCase()} successfully!`;
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
