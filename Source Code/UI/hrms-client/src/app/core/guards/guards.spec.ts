import { TestBed } from '@angular/core/testing';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { authGuard, permissionGuard } from './guards';

describe('Guards', () => {
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      providers: [
        { provide: Router, useValue: spy }
      ]
    });
    routerSpy = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    localStorage.clear();
  });

  describe('authGuard', () => {
    it('should return true if user is authenticated', () => {
      localStorage.setItem('accessToken', 'token');
      const route = {} as ActivatedRouteSnapshot;
      const state = {} as RouterStateSnapshot;
      
      const result = TestBed.runInInjectionContext(() => authGuard(route, state));
      expect(result).toBeTrue();
    });

    it('should navigate to login and return false if user is not authenticated', () => {
      const route = {} as ActivatedRouteSnapshot;
      const state = {} as RouterStateSnapshot;
      
      const result = TestBed.runInInjectionContext(() => authGuard(route, state));
      expect(result).toBeFalse();
      expect(routerSpy.navigate).toHaveBeenCalledWith(['/login']);
    });
  });

  describe('permissionGuard', () => {
    it('should return true if no permission is required', () => {
      const route = { data: {} } as unknown as ActivatedRouteSnapshot;
      const state = {} as RouterStateSnapshot;

      const result = TestBed.runInInjectionContext(() => permissionGuard(route, state));
      expect(result).toBeTrue();
    });

    it('should return true if user has required permission', () => {
      localStorage.setItem('permissions', JSON.stringify(['COMPANY_MANAGE']));
      const route = { data: { permission: 'COMPANY_MANAGE' } } as unknown as ActivatedRouteSnapshot;
      const state = {} as RouterStateSnapshot;

      const result = TestBed.runInInjectionContext(() => permissionGuard(route, state));
      expect(result).toBeTrue();
    });

    it('should redirect to dashboard and return false if user lack permission', () => {
      localStorage.setItem('permissions', JSON.stringify(['SOME_OTHER_PERM']));
      const route = { data: { permission: 'COMPANY_MANAGE' } } as unknown as ActivatedRouteSnapshot;
      const state = {} as RouterStateSnapshot;

      const result = TestBed.runInInjectionContext(() => permissionGuard(route, state));
      expect(result).toBeFalse();
      expect(routerSpy.navigate).toHaveBeenCalledWith(['/dashboard']);
    });
  });
});
