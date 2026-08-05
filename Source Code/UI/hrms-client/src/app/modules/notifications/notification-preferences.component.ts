import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NotificationService } from '../../core/services/notification.service';
import { UserNotificationPreference } from '../../core/models/notification.models';

@Component({
  selector: 'app-notification-preferences',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './notification-preferences.component.html',
  styleUrl: './notification-preferences.component.scss'
})
export class NotificationPreferencesComponent implements OnInit {
  private notificationService = inject(NotificationService);

  tenantId = 1;
  userId = 1;
  isLoading = true;
  toastMessage = '';

  // Matrix categories
  businessEvents = ['Tenant Created', 'Subscription Expiring', 'Payment Failed', 'License Threshold Reached'];
  channels = ['In-App', 'Email', 'SMS', 'Push', 'Teams'];
  frequencies = ['Immediate', 'Daily Digest', 'Weekly Digest'];

  // Flattened preference map: key is "EventName|ChannelName"
  preferencesMap: { [key: string]: UserNotificationPreference } = {};

  ngOnInit() {
    this.tenantId = Number(localStorage.getItem('tenantId') || '1');
    this.userId = Number(localStorage.getItem('userId') || '1');
    this.loadPreferences();
  }

  loadPreferences() {
    this.isLoading = true;
    this.notificationService.getPreferences(this.userId, this.tenantId).subscribe({
      next: (res) => {
        const list = res.data || [];
        this.preferencesMap = {};
        
        // Initialize default matrix
        for (const ev of this.businessEvents) {
          for (const ch of this.channels) {
            const key = `${ev}|${ch}`;
            this.preferencesMap[key] = {
              preferenceId: 0,
              tenantId: this.tenantId,
              userID: this.userId,
              businessEvent: ev,
              channel: ch,
              frequency: 'Immediate',
              isEnabled: false // default disabled until enabled
            };
          }
        }

        // Map loaded values from API
        for (const pref of list) {
          const key = `${pref.businessEvent}|${pref.channel}`;
          if (this.preferencesMap[key]) {
            this.preferencesMap[key] = { ...pref };
          }
        }
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load preferences', err);
        this.isLoading = false;
        this.showToast('Failed to load preference settings.');
      }
    });
  }

  getPref(event: string, channel: string): UserNotificationPreference {
    return this.preferencesMap[`${event}|${channel}`];
  }

  togglePreference(event: string, channel: string) {
    const pref = this.getPref(event, channel);
    // Auto-save on change
    this.savePref(pref);
  }

  changeFrequency(event: string, channel: string) {
    const pref = this.getPref(event, channel);
    if (pref.isEnabled) {
      this.savePref(pref);
    }
  }

  savePref(pref: UserNotificationPreference) {
    this.notificationService.savePreference(pref).subscribe({
      next: (res) => {
        if (res.success) {
          // If creation succeeded, we will get success. Reloading can refresh the preference ID if it was 0
          this.showToast(`Updated settings for ${pref.channel} on ${pref.businessEvent}.`);
        } else {
          this.showToast(res.message || 'Save failed.');
        }
      },
      error: (err) => {
        console.error(err);
        this.showToast('Failed to save preferences.');
      }
    });
  }

  showToast(msg: string) {
    this.toastMessage = msg;
    setTimeout(() => {
      this.toastMessage = '';
    }, 3000);
  }
}
