import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-travel-compliance',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="travel-compliance-container">
      <div class="header-section">
        <h1>Travel & Expense Compliance</h1>
        <p>Monitor employee travel policy violations, budget limit warnings, and regulatory audits.</p>
      </div>

      <!-- Compliance Grid -->
      <div class="card grid-card mt-4">
        <h2>Policy Warnings & Violations</h2>
        <div class="table-responsive mt-3">
          <table class="table custom-table">
            <thead>
              <tr>
                <th>Compliance ID</th>
                <th>Request ID</th>
                <th>Employee ID</th>
                <th>Destination</th>
                <th>Violation Type</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let item of complianceItems">
                <td>#{{ item.travelComplianceID }}</td>
                <td>#{{ item.travelRequestID }}</td>
                <td>{{ item.employeeID }}</td>
                <td>{{ item.destination }}</td>
                <td><span class="text-warning">{{ item.complianceType }}</span></td>
                <td>
                  <span class="badge" [ngClass]="item.complianceStatus === 'Violation' ? 'badge-violation' : 'badge-compliant'">
                    {{ item.complianceStatus }}
                  </span>
                </td>
              </tr>
              <tr *ngIf="complianceItems.length === 0">
                <td colspan="6" class="text-center text-muted">All travel requests are compliant! No violations detected.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .travel-compliance-container {
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
    .badge { padding: 6px 12px; border-radius: 20px; }
    .badge-compliant { background-color: rgba(46,204,113,0.2); color: #2ecc71; border: 1px solid #2ecc71; }
    .badge-violation { background-color: rgba(231,76,60,0.2); color: #e74c3c; border: 1px solid #e74c3c; }
  `]
})
export class TravelComplianceComponent implements OnInit {
  complianceItems: any[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadCompliance();
  }

  loadCompliance(): void {
    this.api.getTravelCompliance(1).subscribe(res => {
      this.complianceItems = res.data || [];
    });
  }
}
