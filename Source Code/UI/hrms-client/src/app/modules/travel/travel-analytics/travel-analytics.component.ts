import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';

@Component({
  selector: 'app-travel-analytics',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="travel-analytics-container">
      <div class="header-section">
        <h1>Travel Spend Analytics</h1>
        <p>Review travel cost distributions, average expenditures per employee, and monthly budget patterns.</p>
      </div>

      <!-- Aggregated Analytics -->
      <div class="card grid-card mt-4">
        <h2>Travel Cost Distribution & Analysis</h2>
        <div class="table-responsive mt-3">
          <table class="table custom-table">
            <thead>
              <tr>
                <th>Analytics ID</th>
                <th>Employee Name</th>
                <th>Total Trips</th>
                <th>Total Expenses ($)</th>
                <th>Reporting Period</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let item of analytics">
                <td>#{{ item.travelAnalyticsID }}</td>
                <td>{{ item.employeeName }}</td>
                <td>{{ item.totalTrips }}</td>
                <td><strong>\&nbsp;\${{ item.totalExpenses | number:'1.2-2' }}</strong></td>
                <td>{{ item.reportingPeriod }}</td>
              </tr>
              <tr *ngIf="analytics.length === 0">
                <td colspan="5" class="text-center text-muted">No analytical data compiled.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .travel-analytics-container {
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
  `]
})
export class TravelAnalyticsComponent implements OnInit {
  analytics: any[] = [];

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadAnalytics();
  }

  loadAnalytics(): void {
    this.api.getTravelAnalyticsSummary(1).subscribe(res => {
      this.analytics = res.data || [];
    });
  }
}
