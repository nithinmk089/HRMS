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
  employeeId?: number;
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

export function parseJwt(token: string): any {
  try {
    const base64Url = token.split('.')[1];
    if (!base64Url) return null;
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    return JSON.parse(jsonPayload);
  } catch (e) {
    return null;
  }
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

  public getDecodedToken(): any {
    const token = this.getToken();
    return token ? parseJwt(token) : null;
  }

  public isTokenExpired(): boolean {
    const decoded = this.getDecodedToken();
    if (!decoded || !decoded.exp) return true;
    return Date.now() >= decoded.exp * 1000;
  }

  public getRolesFromToken(): string[] {
    const decoded = this.getDecodedToken();
    if (!decoded) return [];
    const roles = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || decoded['role'] || [];
    return Array.isArray(roles) ? roles : [roles];
  }

  public getPermissionsFromToken(): string[] {
    const decoded = this.getDecodedToken();
    if (!decoded) return [];
    const perms = decoded['permission'] || [];
    return Array.isArray(perms) ? perms : [perms];
  }

  public hasPermission(requiredPermission: string): boolean {
    const roles = this.getRolesFromToken();
    if (roles.includes('SYSADMIN') || roles.includes('ADMIN')) return true;
    const perms = this.getPermissionsFromToken();
    return perms.includes(requiredPermission);
  }

  public hasRole(requiredRole: string): boolean {
    const roles = this.getRolesFromToken();
    return roles.includes('SYSADMIN') || roles.includes(requiredRole);
  }

  public getTenantId(): number {
    const decoded = this.getDecodedToken();
    if (decoded && decoded['tenantId']) {
      return Number(decoded['tenantId']);
    }
    if (this.currentUserValue && this.currentUserValue.tenantId) {
      return Number(this.currentUserValue.tenantId);
    }
    const storedTenant = localStorage.getItem('tenantId');
    return storedTenant ? Number(storedTenant) : 1;
  }

  public getEmployeeId(): number | null {
    const decoded = this.getDecodedToken();
    if (decoded && decoded['employeeId']) {
      return Number(decoded['employeeId']);
    }
    return this.currentUserValue?.employeeId ?? null;
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
