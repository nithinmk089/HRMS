import { inject } from '@angular/core';
import { CanActivateFn, ResolveFn, Router } from '@angular/router';
import { ApiService } from '../services/api.service';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const isAuthenticated = localStorage.getItem('accessToken') !== null;
  if (!isAuthenticated) {
    router.navigate(['/login']);
    return false;
  }
  return true;
};

export const permissionGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const requiredPermission = route.data['permission'] as string;
  if (!requiredPermission) return true;

  const permissions: string[] = JSON.parse(localStorage.getItem('permissions') || '[]');
  const isSysAdmin = permissions.includes('SYSADMIN');
  const hasPermission = isSysAdmin || permissions.includes(requiredPermission);
  if (!hasPermission) {
    router.navigate(['/dashboard']);
    return false;
  }
  return true;
};

export const tenantGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const tenantId = localStorage.getItem('tenantId');
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
  const tenantId = Number(localStorage.getItem('tenantId') || '1');
  return inject(ApiService).getUser(id, tenantId);
};

export const companyResolver: ResolveFn<any> = (route, state) => {
  const id = Number(route.paramMap.get('id'));
  if (!id) return null;
  const tenantId = Number(localStorage.getItem('tenantId') || '1');
  return inject(ApiService).getCompany(id, tenantId);
};
