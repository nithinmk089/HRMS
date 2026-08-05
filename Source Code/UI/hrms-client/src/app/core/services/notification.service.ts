import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { NotificationTemplate, NotificationQueue, UserNotificationPreference, NotificationMetrics } from '../models/notification.models';
import { ApiResponse } from '../models/phase01.models';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private baseUrl = `${environment.apiUrl}/notifications`;

  constructor(private http: HttpClient) {}

  // --- Templates ---
  getTemplates(tenantId: number, searchText?: string, page: number = 1, pageSize: number = 50): Observable<ApiResponse<NotificationTemplate[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString()).set('page', page.toString()).set('pageSize', pageSize.toString());
    if (searchText) params = params.set('searchText', searchText);
    return this.http.get<ApiResponse<NotificationTemplate[]>>(`${this.baseUrl}/templates`, { params });
  }

  createTemplate(template: Partial<NotificationTemplate>): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/templates`, template);
  }

  updateTemplate(id: number, template: Partial<NotificationTemplate>): Observable<ApiResponse<boolean>> {
    return this.http.put<ApiResponse<boolean>>(`${this.baseUrl}/templates/${id}`, template);
  }

  deleteTemplate(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/templates/${id}`, { params });
  }

  // --- Queue / History ---
  getHistory(tenantId: number, channel?: string, status?: string, businessEvent?: string, page: number = 1, pageSize: number = 50): Observable<ApiResponse<NotificationQueue[]>> {
    let params = new HttpParams().set('tenantId', tenantId.toString()).set('page', page.toString()).set('pageSize', pageSize.toString());
    if (channel) params = params.set('channel', channel);
    if (status) params = params.set('status', status);
    if (businessEvent) params = params.set('businessEvent', businessEvent);
    return this.http.get<ApiResponse<NotificationQueue[]>>(this.baseUrl, { params });
  }

  sendNotification(notification: any): Observable<ApiResponse<number>> {
    return this.http.post<ApiResponse<number>>(`${this.baseUrl}/send`, notification);
  }

  retryNotification(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/${id}/retry`, {}, { params });
  }

  cancelNotification(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/${id}/cancel`, {}, { params });
  }

  readNotification(id: number, tenantId: number): Observable<ApiResponse<boolean>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/${id}/read`, {}, { params });
  }

  getMetrics(tenantId: number): Observable<ApiResponse<NotificationMetrics>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<NotificationMetrics>>(`${this.baseUrl}/metrics`, { params });
  }

  // --- User Preferences ---
  getPreferences(userId: number, tenantId: number): Observable<ApiResponse<UserNotificationPreference[]>> {
    const params = new HttpParams().set('tenantId', tenantId.toString());
    return this.http.get<ApiResponse<UserNotificationPreference[]>>(`${this.baseUrl}/preferences/${userId}`, { params });
  }

  savePreference(pref: UserNotificationPreference): Observable<ApiResponse<boolean>> {
    return this.http.post<ApiResponse<boolean>>(`${this.baseUrl}/preferences`, pref);
  }
}
