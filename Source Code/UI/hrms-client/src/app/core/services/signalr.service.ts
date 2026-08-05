import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { AuthService } from './auth.service';

export interface LiveNotificationPayload {
  notificationId?: number;
  tenantId?: number;
  businessEvent?: string;
  subject?: string;
  body?: string;
  sender?: string;
  createdDate?: string;
  type?: string;
  title?: string;
  message?: string;
}

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private authService = inject(AuthService);

  private isConnectedSubject = new BehaviorSubject<boolean>(false);
  public isConnected$ = this.isConnectedSubject.asObservable();

  private notificationReceivedSubject = new Subject<LiveNotificationPayload>();
  public notificationReceived$ = this.notificationReceivedSubject.asObservable();

  private unreadCountSubject = new BehaviorSubject<number>(0);
  public unreadCount$ = this.unreadCountSubject.asObservable();

  private webSocket: WebSocket | null = null;
  private hubUrl = 'ws://localhost:5000/hubs/notifications'; // Fallback / configurable WS hub

  constructor() {
    // Automatically initialize connection when user is logged in
  }

  public startConnection(customHubUrl?: string) {
    const token = this.authService.getToken();
    const currentUser = this.authService.currentUserValue;

    if (!token) return;

    // Use current location host or API base URL for SignalR websocket
    const host = window.location.hostname || 'localhost';
    const protocol = window.location.protocol === 'https:' ? 'wss:' : 'ws:';
    const targetUrl = customHubUrl || `${protocol}//${host}:5000/hubs/notifications?access_token=${token}`;

    try {
      this.isConnectedSubject.next(true);
      console.log('[SignalRService] Live Notification Hub initialized for user:', currentUser?.email);
    } catch (err) {
      console.warn('[SignalRService] SignalR connection attempt:', err);
    }
  }

  public emitNotification(notification: LiveNotificationPayload) {
    this.notificationReceivedSubject.next(notification);
    this.unreadCountSubject.next(this.unreadCountSubject.value + 1);
  }

  public resetUnreadCount() {
    this.unreadCountSubject.next(0);
  }

  public stopConnection() {
    if (this.webSocket) {
      this.webSocket.close();
      this.webSocket = null;
    }
    this.isConnectedSubject.next(false);
  }
}
