import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-rating-management',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './rating-management.component.html',
  styleUrls: ['./rating-management.component.scss']
})
export class RatingManagementComponent implements OnInit {
  items: any[] = [];
  errorMessage = '';
  isLoading = false;
  tenantId = 1;

  constructor(
    private api: ApiService,
    private auth: AuthService
  ) {}

  ngOnInit(): void {
    this.tenantId = this.auth.getTenantId() || 1;
    this.loadData();
  }

  loadData(): void {
    this.isLoading = true;
    this.api.getGoals(this.tenantId).subscribe({
      next: (res) => {
        this.items = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Failed to load ratings';
        this.isLoading = false;
      }
    });
  }

  getRatingLabel(pct: number): string {
    if (pct >= 90) return '5 - Outstanding';
    if (pct >= 75) return '4 - Exceeds Expectations';
    if (pct >= 60) return '3 - Meets Expectations';
    if (pct >= 40) return '2 - Needs Improvement';
    return '1 - Unsatisfactory';
  }

  getRatingBadgeClass(pct: number): string {
    if (pct >= 75) return 'badge-success';
    if (pct >= 60) return 'badge-info';
    return 'badge-warning';
  }
}