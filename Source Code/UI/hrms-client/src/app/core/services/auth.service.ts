import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/phase01.models';

export interface UserDto {
  userId: number;
  email: string;
  firstName: string;
  lastName: string;
  organizationId: number;
  tenantId?: number;
}

export interface CompanyDto {
  companyId: number;
  companyCode: string;
  companyName: string;
  isDefault: boolean;
}

export interface AuthResponse {
  success: boolean;
  message?: string;
  accessToken: string;
  refreshToken: string;
  user?: UserDto;
  roles?: string[];
  permissions?: string[];
  companies?: CompanyDto[];
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  
  private apiUrl = `${environment.apiUrl}/auth`;

  private currentUserSubject = new BehaviorSubject<UserDto | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor() {
    // Attempt to restore user session if page is refreshed
    const storedUser = localStorage.getItem('currentUser');
    if (storedUser) {
      try {
        this.currentUserSubject.next(JSON.parse(storedUser));
      } catch (e) {
        console.error('Error parsing stored user', e);
      }
    }
  }

  public get currentUserValue(): UserDto | null {
    return this.currentUserSubject.value;
  }

  public getToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  checkSetupStatus(): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(`${this.apiUrl}/setup-status`);
  }

  setupInitialAdmin(setupData: any): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(`${this.apiUrl}/setup-admin`, setupData).pipe(
      tap(response => {
        if (response.success && response.data) {
          const authData = response.data;
          localStorage.setItem('accessToken', authData.accessToken);
          localStorage.setItem('refreshToken', authData.refreshToken);
          if (authData.user) {
            localStorage.setItem('currentUser', JSON.stringify(authData.user));
            this.currentUserSubject.next(authData.user);
            localStorage.setItem('tenantId', (authData.user.tenantId || 1).toString());
          }
          localStorage.setItem('roles', JSON.stringify(authData.roles || ['SYSADMIN']));
          localStorage.setItem('permissions', JSON.stringify(authData.permissions || ['SYSADMIN']));
        }
      })
    );
  }

  login(email: string, password: string): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(`${this.apiUrl}/login`, {
      email,
      password,
      ipAddress: '127.0.0.1',
      browserInfo: navigator.userAgent
    }).pipe(
      tap(response => {
        if (response.success && response.data) {
          const authData = response.data;
          
          // Save tokens and settings in localStorage
          localStorage.setItem('accessToken', authData.accessToken);
          localStorage.setItem('refreshToken', authData.refreshToken);
          
          if (authData.user) {
            localStorage.setItem('currentUser', JSON.stringify(authData.user));
            this.currentUserSubject.next(authData.user);
            
            if (authData.user.tenantId) {
              localStorage.setItem('tenantId', authData.user.tenantId.toString());
            } else {
              localStorage.setItem('tenantId', '1'); // Fallback default tenant
            }
          }

          localStorage.setItem('roles', JSON.stringify(authData.roles || []));
          localStorage.setItem('permissions', JSON.stringify(authData.permissions || []));
        }
      })
    );
  }

  logout() {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('currentUser');
    localStorage.removeItem('tenantId');
    localStorage.removeItem('roles');
    localStorage.removeItem('permissions');
    
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }
}
