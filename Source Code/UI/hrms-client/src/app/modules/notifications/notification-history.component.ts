import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NotificationService } from '../../core/services/notification.service';
import { NotificationQueue } from '../../core/models/notification.models';

@Component({
  selector: 'app-notification-history',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './notification-history.component.html',
  styleUrl: './notification-history.component.scss'
})
export class NotificationHistoryComponent implements OnInit {
  private notificationService = inject(NotificationService);

  history: NotificationQueue[] = [];
  isLoading = true;
  tenantId = 1;

  // Filters
  selectedChannel = '';
  selectedStatus = '';
  selectedEvent = '';
  page = 1;
  pageSize = 10;

  channels = ['In-App', 'Email', 'SMS', 'Push', 'Teams'];
  statuses = ['Queued', 'Sent', 'Delivered', 'Failed', 'Cancelled', 'Read'];
  events = ['Tenant Created', 'Subscription Expiring', 'Payment Failed', 'License Threshold Reached'];

  toastMessage = '';

  ngOnInit() {
    this.tenantId = Number(localStorage.getItem('tenantId') || '1');
    this.loadHistory();
  }

  loadHistory() {
    this.isLoading = true;
    this.notificationService.getHistory(
      this.tenantId,
      this.selectedChannel || undefined,
      this.selectedStatus || undefined,
      this.selectedEvent || undefined,
      this.page,
      this.pageSize
    ).subscribe({
      next: (res) => {
        this.history = res.data || [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load notification history', err);
        this.isLoading = false;
        this.showToast('Failed to load notification history.');
      }
    });
  }

  retry(id: number) {
    this.notificationService.retryNotification(id, this.tenantId).subscribe({
      next: (res) => {
        if (res.success) {
          this.showToast('Notification queued for retry.');
          this.loadHistory();
        } else {
          this.showToast(res.message || 'Retry failed.');
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('An error occurred.');
      }
    });
  }

  cancel(id: number) {
    if (confirm('Are you sure you want to cancel this queued notification?')) {
      this.notificationService.cancelNotification(id, this.tenantId).subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast('Notification cancelled.');
            this.loadHistory();
          } else {
            this.showToast(res.message || 'Cancel failed.');
          }
        },
        error: (err) => {
          console.error(err);
          this.showToast('An error occurred.');
        }
      });
    }
  }

  markAsRead(id: number) {
    this.notificationService.readNotification(id, this.tenantId).subscribe({
      next: (res) => {
        if (res.success) {
          this.showToast('Notification marked as read.');
          this.loadHistory();
        } else {
          this.showToast(res.message || 'Action failed.');
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('An error occurred.');
      }
    });
  }

  resetFilters() {
    this.selectedChannel = '';
    this.selectedStatus = '';
    this.selectedEvent = '';
    this.page = 1;
    this.loadHistory();
  }

  prevPage() {
    if (this.page > 1) {
      this.page--;
      this.loadHistory();
    }
  }

  nextPage() {
    this.page++;
    this.loadHistory();
  }

  showToast(msg: string) {
    this.toastMessage = msg;
    setTimeout(() => {
      this.toastMessage = '';
    }, 3000);
  }
}
