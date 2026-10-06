import { inject } from '@angular/core';
import { CanActivateFn, ResolveFn, Router } from '@angular/router';
import { ApiService } from '../services/api.service';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const token = authService.getToken();

  if (!token || authService.isTokenExpired()) {
    authService.logout();
    return false;
  }
  return true;
};

export const permissionGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const requiredPermission = route.data['permission'] as string;
  if (!requiredPermission) return true;

  if (authService.isTokenExpired()) {
    authService.logout();
    return false;
  }

  const hasPermission = authService.hasPermission(requiredPermission);
  if (!hasPermission) {
    router.navigate(['/dashboard']);
    return false;
  }
  return true;
};

export const tenantGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const tenantId = authService.getTenantId();
  if (!tenantId) {
    router.navigate(['/login']);
    return false;
  }
  return true;
};

export const tenantResolver: ResolveFn<any> = (route, state) => {
  const id = Number(route.paramMap.get('id'));
  if (!id) return null;
  return inject(ApiService).getTenant(id);
};

export const userResolver: ResolveFn<any> = (route, state) => {
  const id = Number(route.paramMap.get('id'));
  if (!id) return null;
  const tenantId = inject(AuthService).getTenantId();
  return inject(ApiService).getUser(id, tenantId);
};

export const companyResolver: ResolveFn<any> = (route, state) => {
  const id = Number(route.paramMap.get('id'));
  if (!id) return null;
  const tenantId = inject(AuthService).getTenantId();
  return inject(ApiService).getCompany(id, tenantId);
};
