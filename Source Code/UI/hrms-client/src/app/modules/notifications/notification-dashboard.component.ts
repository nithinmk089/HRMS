import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NotificationService } from '../../core/services/notification.service';
import { NotificationMetrics } from '../../core/models/notification.models';

@Component({
  selector: 'app-notification-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './notification-dashboard.component.html',
  styleUrl: './notification-dashboard.component.scss'
})
export class NotificationDashboardComponent implements OnInit {
  private notificationService = inject(NotificationService);

  metrics: NotificationMetrics | null = null;
  isLoading = true;
  tenantId = 1;

  ngOnInit() {
    this.tenantId = Number(localStorage.getItem('tenantId') || '1');
    this.loadMetrics();
  }

  loadMetrics() {
    this.isLoading = true;
    this.notificationService.getMetrics(this.tenantId).subscribe({
      next: (res) => {
        this.metrics = res.data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load notification metrics', err);
        this.isLoading = false;
      }
    });
  }
}
