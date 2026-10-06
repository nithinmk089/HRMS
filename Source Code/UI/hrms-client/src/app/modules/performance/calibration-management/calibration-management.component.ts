import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../../core/services/api.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-calibration-management',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './calibration-management.component.html',
  styleUrls: ['./calibration-management.component.scss']
})
export class CalibrationManagementComponent implements OnInit {
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
    this.api.getPerformanceCycles(this.tenantId).subscribe({
      next: (res) => {
        this.items = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Failed to load calibration sessions';
        this.isLoading = false;
      }
    });
  }
}