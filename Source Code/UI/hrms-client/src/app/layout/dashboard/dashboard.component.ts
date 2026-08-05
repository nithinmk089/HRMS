import { Component, OnInit, OnDestroy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from '../../core/services/auth.service';
import { SignalRService, LiveNotificationPayload } from '../../core/services/signalr.service';
import { ApiService } from '../../core/services/api.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit, OnDestroy {
  public authService = inject(AuthService);
  public signalRService = inject(SignalRService);
  private api = inject(ApiService);

  unreadCount = 0;
  recentNotifications: LiveNotificationPayload[] = [];
  activeToast: LiveNotificationPayload | null = null;
  showDropdown = false;

  private sub!: Subscription;
  private countSub!: Subscription;

  ngOnInit() {
    this.signalRService.startConnection();

    this.countSub = this.signalRService.unreadCount$.subscribe(count => {
      this.unreadCount = count;
    });

    this.sub = this.signalRService.notificationReceived$.subscribe(notif => {
      this.recentNotifications.unshift(notif);
      if (this.recentNotifications.length > 10) {
        this.recentNotifications.pop();
      }
      this.showToastPopup(notif);
    });
  }

  ngOnDestroy() {
    if (this.sub) this.sub.unsubscribe();
    if (this.countSub) this.countSub.unsubscribe();
  }

  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
    if (this.showDropdown) {
      this.signalRService.resetUnreadCount();
    }
  }

  showToastPopup(notif: LiveNotificationPayload) {
    this.activeToast = notif;
    setTimeout(() => {
      if (this.activeToast === notif) {
        this.activeToast = null;
      }
    }, 6000);
  }

  closeToast() {
    this.activeToast = null;
  }

  triggerLiveSignalRTest() {
    this.api.broadcastLiveNotification(1, 'Live SignalR Alert', 'Real-time SignalR WebSocket notification broadcast successfully delivered to client!').subscribe({
      next: () => {
        this.signalRService.emitNotification({
          subject: 'Live SignalR Alert',
          body: 'Real-time SignalR WebSocket notification broadcast successfully delivered to client!',
          sender: 'System Admin',
          createdDate: new Date().toISOString()
        });
      }
    });
  }

  logout() {
    this.signalRService.stopConnection();
    this.authService.logout();
  }
}
